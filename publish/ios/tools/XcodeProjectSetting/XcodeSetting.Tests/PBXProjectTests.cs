using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
#if UNITY_XCODE_API_BUILD
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#else
using UnityEditor.iOS.Xcode.Custom;
using UnityEditor.iOS.Xcode.Custom.Extensions;
#endif

namespace UnityEditor.iOS.Xcode.Tests
{
    public class LinearGuidGenerator
    {
        private static int counter = 0;
        
        public static void Reset()
        {
            counter = 0;
        }
        public static string Generate()
        {
            counter = counter + 1;
            return "CCCCCCCC00000000" + counter.ToString("D8");
        }
    }

    /*  It's not possible to test separate components of PBXProject in isolation easily --
        the data stored there has many cross dependencies. We are doing sort-of integration
        testing, thus test data is large. To simplify testing, pbx projects are stored
        in files -- one for source and and one for expected output data. The tests read a
        source file, write a modified version to a temporary test directory and compare to
        the expected output.
        
        Since the changes between source and output files are quite small compared to the
        total quantity of data, it's best to use a diff tool when debugging failures or
        verifying changes.
    */
    [TestFixture]
    public class PBXProjectTests : GenericTester
    {
        public PBXProjectTests() : base("PBXProjectTestFiles", "PBXProjectTestOutput", false /*true for debug*/)
        {
        }
        
        private static void ResetGuidGenerator()
        {
#if UNITY_XCODE_API_BUILD
            UnityEditor.iOS.Xcode.PBX.PBXGUID.SetGuidGenerator(LinearGuidGenerator.Generate);
#else
            UnityEditor.iOS.Xcode.Custom.PBX.PBXGUID.SetGuidGenerator(LinearGuidGenerator.Generate);
#endif
            LinearGuidGenerator.Reset();
        }

        private void TestOutput(PBXProject proj, string testFilename)
        {
            string sourceFile = Path.Combine(GetTestSourcePath(), testFilename);
            string outputFile = Path.Combine(GetTestOutputPath(), testFilename);

            proj.WriteToFile(outputFile);
            Assert.IsTrue(TestUtils.FileContentsEqual(outputFile, sourceFile),
                          "Output not equivalent to the expected output");

            PBXProject other = new PBXProject();
            other.ReadFromFile(outputFile);
            other.WriteToFile(outputFile);
            Assert.IsTrue(TestUtils.FileContentsEqual(outputFile, sourceFile));
            if (!DebugEnabled())
                Directory.Delete(GetTestOutputPath(), recursive:true);
        }
        
        private PBXProject ReadPBXProject()
        {
            return ReadPBXProject("base.pbxproj");
        }

        private PBXProject ReadPBXProject(string project)
        {
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(Path.Combine(GetTestSourcePath(), project)));
            return proj;
        }

        private static PBXProject Reserialize(PBXProject proj)
        {
            string contents = proj.WriteToString();
            proj = new PBXProject();
            proj.ReadFromString(contents);
            return proj;
        }

        [Test]
        public void BuildOptionsWork1()
        {
            ResetGuidGenerator();

            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            string ptarget = proj.ProjectGuid();

            // check that target selection works when setting options
            proj.SetBuildProperty(ptarget, "TEST_PROJ", "projtest");
            proj.SetBuildProperty(target, "TEST", "testdata1");

            // check quoting in various special cases
            proj.AddBuildProperty(target, "TEST_ADD", "testdata2");
            proj.AddBuildProperty(target, "TEST[quoted]", "testdata3");
            proj.AddBuildProperty(target, "TEST quoted", "testdata4");
            proj.AddBuildProperty(target, "TEST//quoted", "testdata4");
            proj.AddBuildProperty(target, "TEST/*quoted", "testdata4");
            proj.AddBuildProperty(target, "TEST*/quoted", "testdata4");

            // check how LIBRARY_SEARCH_PATHS option is quoted
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "test");
            TestOutput(proj, "conf_append1.pbxproj");
        }

        [Test]
        public void BuildOptionsWork2()
        {
            ResetGuidGenerator();
    
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            // check that we can append multiple options
            proj.AddBuildProperty(target, "TEST_ADD", "test2");
            proj.AddBuildProperty(target, "TEST_ADD", "test2");
            proj.AddBuildProperty(target, "TEST_ADD", "test3");

            // check SetBuildProperty when multiple values already exist
            proj.AddBuildProperty(target, "TEST_SET", "test2");        
            proj.AddBuildProperty(target, "TEST_SET", "test3");
            proj.SetBuildProperty(target, "TEST_SET", "test4");

            // check that option removal works
            proj.AddBuildProperty(target, "TEST_REMOVE", "test1");
            proj.AddBuildProperty(target, "TEST_REMOVE", "test2");
            proj.AddBuildProperty(target, "TEST_REMOVE2", "value");
            proj = Reserialize(proj);
            proj.UpdateBuildProperty(target, "TEST_REMOVE", null, new string[]{"test2"});
            proj.RemoveBuildProperty(target, "TEST_REMOVE2");

            // check quoting for LIBRARY_SEARCH_PATHS
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "test test");
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "\"test test\"");
            TestOutput(proj, "conf_append2.pbxproj");
        }

        [Test]
        public void BuildOptionsWork3()
        {
            ResetGuidGenerator();

            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            // check how various operations work for LIBRARY_SEARCH_PATHS as we have 
            // special logic for this key
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "test test");
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "\"test test\"");
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "test test2");
            proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "test test3");
            proj.UpdateBuildProperty(target, "LIBRARY_SEARCH_PATHS", null, new string[]{"test test2"});
            proj = Reserialize(proj);
            proj.UpdateBuildProperty(target, "LIBRARY_SEARCH_PATHS", new string[]{"test test3"}, new string[]{"test test2"}); // tests whether "test test2" is correctly removed
            TestOutput(proj, "conf_append3.pbxproj");
        }

        [Test]
        public void DuplicateOptionHandlingWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject("base_dup.pbxproj");
            
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.UpdateBuildProperty(target, "TEST_DUP", new string[]{"test2"}, new string[]{"test3"});
            proj.UpdateBuildProperty(target, "TEST_DUP2", null, new string[]{"test_key"}); // duplicate value removal
            
            TestOutput(proj, "dup1.pbxproj");
        }

        [Test]
        public void AddSingleSourceFileWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.AddFileToBuild(target, proj.AddFile("relative/path1.cc", "Libraries/path1.cc", PBXSourceTree.Source));
            TestOutput(proj, "add_file1.pbxproj");
        }

        [Test]
        public void AddMultipleSourceFilesWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            // check addition of relative path
            proj.AddFileToBuild(target, proj.AddFile("relative/path1.cc", "Classes/some/path/path1.cc", PBXSourceTree.Source));
            // check addition of absolute path
            proj.AddFileToBuild(target, proj.AddFile("/absolute/path/abs1.cc", "Classes/some/path/abs1.cc", PBXSourceTree.Source));
            proj.AddFileToBuild(target, proj.AddFile("/absolute/path/abs2.cc", "Classes/some/abs2.cc", PBXSourceTree.Source));
            // check addition of files with unknown extensions
            proj.AddFileToBuild(target, proj.AddFile("relative/path1.unknown_ext", "Classes/some/path/path1.unknown_ext", PBXSourceTree.Source));
            // check whether folder references work
            proj.AddFileToBuild(target, proj.AddFolderReference("relative/path2", "Classes/some/path/path2", PBXSourceTree.Source));
            // check whether we correctly add folder references with weird extensions to resources
            proj.AddFileToBuild(target, proj.AddFolderReference("relative/path3.cc", "Classes/some/path/path3.cc", PBXSourceTree.Source));
            // check whether we correctly add files which are not buildable
            proj.AddFileToBuild(target, proj.AddFolderReference("relative/lib.dll", "Classes/some/path/lib.dll", PBXSourceTree.Source));

            Assert.IsTrue(proj.FindFileGuidByRealPath("relative/path1.cc") == "CCCCCCCC0000000000000001");
            Assert.IsTrue(proj.FindFileGuidByRealPath("/absolute/path/abs1.cc") == "CCCCCCCC0000000000000005");
            Assert.IsTrue(proj.FindFileGuidByProjectPath("Classes/some/path/abs1.cc") == "CCCCCCCC0000000000000005");
            Assert.AreEqual(1, proj.GetGroupChildrenFiles("Classes/some").Count);
            
            TestOutput(proj, "add_file2.pbxproj");
        }

        [Test]
        public void AddingDuplicateFilerefReturnsExisting()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();

            string fileGuid = proj.AddFile("relative/path1.cc", "Classes/path1.cc", PBXSourceTree.Source);
            Assert.AreEqual(fileGuid, proj.AddFile("relative/path1.cc", "Classes/path1.cc", PBXSourceTree.Source));
            Assert.AreEqual(fileGuid, proj.AddFile("relative/path2.cc", "Classes/path1.cc", PBXSourceTree.Source));
        }

        [Test]
        public void AddingDuplicateFileToBuildIsIgnored()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            string fileGuid = proj.AddFile("relative/path1.cc", "Classes/path1.cc", PBXSourceTree.Source);
            proj.AddFileToBuildWithFlags(target, fileGuid, "-Wno-newline");
            proj.AddFileToBuildWithFlags(target, fileGuid, "-Wnewline"); // this call should be ignored

            Assert.AreEqual(new List<string>{"-Wno-newline"}, proj.GetCompileFlagsForFile(target, fileGuid));
        }

        [Test]
        public void SetCompilerFlagsForFileWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            string fileGuid = proj.AddFile("relative/path1.cc", "Classes/path1.cc", PBXSourceTree.Source);
            proj.AddFileToBuild(target, fileGuid);
            proj = Reserialize(proj);

            proj.SetCompileFlagsForFile(target, fileGuid, new List<string>{"-flag1", "flag2"});
            proj = Reserialize(proj);

            Assert.AreEqual(new List<string>{"-flag1", "flag2"}, proj.GetCompileFlagsForFile(target, fileGuid));
        }

        [Test]
        public void SpacesInCompilerFlagsAreStripped()
        {
            // NOTE: we may want to change this behavior in the future in order to support spaces in flags
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            string fileGuid = proj.AddFile("relative/path1.cc", "Classes/path1.cc", PBXSourceTree.Source);
            proj.AddFileToBuild(target, fileGuid);
            proj.SetCompileFlagsForFile(target, fileGuid, new List<string>{"  -flag1   ", " flag2  "});
            proj = Reserialize(proj);

            Assert.AreEqual(new List<string>{"-flag1", "flag2"}, proj.GetCompileFlagsForFile(target, fileGuid));
        }

        [Test]
        public void CanRemoveCompilerFlagsForFile()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            string fileGuid = proj.AddFile("relative/path1.cc", "Classes/path1.cc", PBXSourceTree.Source);
            proj.AddFileToBuildWithFlags(target, fileGuid, "-Wno-newline");
            proj.SetCompileFlagsForFile(target, fileGuid, null);

            Assert.AreEqual(0, proj.GetCompileFlagsForFile(target, fileGuid).Count);
        }

        [Test]
        public void AddSourceFileWithFlagsWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            // check if duplicate add is ignored (we don't lose flags)
            proj.AddFileToBuildWithFlags(target, proj.AddFile("relative/path1.cc", "Classes/path1.cc", PBXSourceTree.Source),
                                         "-Wno-newline");
            
            // check if we can add flags to an existing file and remove them
            proj.AddFileToBuild(target, proj.AddFile("relative/path2.cc", "Classes/path2.cc", PBXSourceTree.Source));
            proj.AddFileToBuildWithFlags(target, proj.AddFile("relative/path3.cc", "Classes/path3.cc", PBXSourceTree.Source), 
                                         "-Wno-newline");
            
            proj = Reserialize(proj);
            target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.SetCompileFlagsForFile(target, proj.FindFileGuidByProjectPath("Classes/path2.cc"),
                                        new List<string>{ "-Wno-newline", "-O3" });
            proj.SetCompileFlagsForFile(target, proj.FindFileGuidByProjectPath("Classes/path3.cc"), null);
            TestOutput(proj, "add_file3.pbxproj");
        }

        [Test]
        public void AddFrameworkWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            // check whether we can add framework reference
            proj.AddFrameworkToProject(target, "Twitter.framework", true);
            proj.AddFrameworkToProject(target, "Foundation.framework", false);
            TestOutput(proj, "add_framework1.pbxproj");
        }

        [Test]
        public void ContainsFrameworkWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            string target2Guid = proj.AddTarget("Other", "app", "com.apple.product-type.bundle");

            proj.AddFrameworkToProject(targetGuid, "Twitter.framework", true);
            Assert.IsTrue(proj.ContainsFramework(targetGuid, "Twitter.framework"));
            Assert.IsFalse(proj.ContainsFramework(target2Guid, "Twitter.framework"));
            Assert.IsFalse(proj.ContainsFramework(targetGuid, "Foundation.framework"));
            Assert.IsFalse(proj.ContainsFramework(target2Guid, "Foundation.framework"));
        }

        [Test]
        public void RemoveFrameworkWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            string target2Guid = proj.AddTarget("Other", "app", "com.apple.product-type.bundle");
            proj.AddFrameworksBuildPhase(target2Guid);

            proj.AddFrameworkToProject(targetGuid, "GameCenter.framework", false);
            proj.AddFrameworkToProject(target2Guid, "GameCenter.framework", false);
            Assert.IsTrue(proj.ContainsFramework(targetGuid, "GameCenter.framework"));
            Assert.IsTrue(proj.ContainsFramework(target2Guid, "GameCenter.framework"));
            proj = Reserialize(proj);
 
            proj.RemoveFrameworkFromProject(targetGuid, "GameCenter.framework");
            Assert.IsFalse(proj.ContainsFramework(targetGuid, "GameCenter.framework"));
            Assert.IsTrue(proj.ContainsFramework(target2Guid, "GameCenter.framework"));
            proj = Reserialize(proj);
 
            proj.RemoveFrameworkFromProject(target2Guid, "GameCenter.framework");
            Assert.IsFalse(proj.ContainsFramework(targetGuid, "GameCenter.framework"));
            Assert.IsFalse(proj.ContainsFramework(target2Guid, "GameCenter.framework"));
        }

        public void FindFileGuidWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();

            string fileGuid = proj.AddFile("relative/path1.cc", "Classes/path1.cc", PBXSourceTree.Source);
            Assert.AreEqual(fileGuid, proj.FindFileGuidByProjectPath("Classes/path1.cc"));
            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("relative/path1.cc"));
            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("relative/path1.cc", PBXSourceTree.Source));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("relative/path1.cc", PBXSourceTree.Absolute));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("/relative/path1.cc", PBXSourceTree.Source));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("relative/path1.cc", PBXSourceTree.Sdk));

            proj.AddFile("absolute/path1.cc", "Classes/path2.cc", PBXSourceTree.Absolute);
            Assert.AreEqual(fileGuid, proj.FindFileGuidByProjectPath("Classes/path2.cc"));
            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("absolute/path1.cc"));
            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("absolute/path1.cc", PBXSourceTree.Absolute));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("/absolute/path1.cc", PBXSourceTree.Absolute));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("absolute/path1.cc", PBXSourceTree.Source));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("absolute/path1.cc", PBXSourceTree.Sdk));

            proj.AddFile("/absolute2/path2.cc", "Classes/path3.cc", PBXSourceTree.Absolute);
            Assert.AreEqual(fileGuid, proj.FindFileGuidByProjectPath("Classes/path3.cc"));
            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("/absolute2/path2.cc"));
            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("absolute2/path2.cc"));

            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("absolute/path1.cc", PBXSourceTree.Absolute));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("/absolute/path1.cc", PBXSourceTree.Absolute));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("absolute/path1.cc", PBXSourceTree.Source));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("absolute/path1.cc", PBXSourceTree.Sdk));

            fileGuid = proj.AddFile("sdk/path1.cc", "Classes/path4.cc", PBXSourceTree.Sdk);
            Assert.AreEqual(fileGuid, proj.FindFileGuidByProjectPath("Classes/path4.cc"));
            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("sdk/path1.cc"));
            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("sdk/path1.cc", PBXSourceTree.Sdk));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("sdk/path1.cc", PBXSourceTree.Absolute));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("sdk/path1.cc", PBXSourceTree.Source));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("/sdk/path1.cc", PBXSourceTree.Sdk));

            fileGuid = proj.AddFile("dev/path1.cc", "Classes/path5.cc", PBXSourceTree.Developer);
            Assert.AreEqual(fileGuid, proj.FindFileGuidByProjectPath("Classes/path5.cc"));
            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("dev/path1.cc"));
            Assert.AreEqual(fileGuid, proj.FindFileGuidByRealPath("dev/path1.cc", PBXSourceTree.Developer));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("dev/path1.cc", PBXSourceTree.Absolute));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("dev/path1.cc", PBXSourceTree.Source));
            Assert.AreEqual(null, proj.FindFileGuidByRealPath("/dev/path1.cc", PBXSourceTree.Developer));
        }

        [Test]
        public void RemoveFileWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            proj.AddFileToBuild(target, proj.AddFile("relative/path1.cc", "Classes/path1.cc", PBXSourceTree.Source));
            proj = Reserialize(proj);
            proj.RemoveFile(proj.FindFileGuidByRealPath("relative/path1.cc"));
            proj.RemoveFile(proj.FindFileGuidByProjectPath("Classes/file"));
            TestOutput(proj, "remove_file1.pbxproj");
        }

        [Test]
        public void AssetTagModificationWorks()
        {
            PBXProject proj;
            string target;

            ResetGuidGenerator();
            proj = ReadPBXProject();
            target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            var f1 = proj.AddFile("Data/data1.dat", "Data/data1.dat", PBXSourceTree.Source);
            var f2 = proj.AddFile("Data/data2.dat", "Data/data2.dat", PBXSourceTree.Source);
            var f3 = proj.AddFile("Data/data3.dat", "Data/data3.dat", PBXSourceTree.Source);
            proj.AddFileToBuild(target, f1);
            proj.AddFileToBuild(target, f2);
            proj.AddFileToBuild(target, f3);
            proj.AddAssetTagForFile(target, f1, "test_tag1");
            proj.AddAssetTagForFile(target, f2, "test_tag1");
            proj.AddAssetTagForFile(target, f2, "test_tag2");
            proj.AddAssetTagForFile(target, f2, "test_tag3");
            proj.AddAssetTagToDefaultInstall(target, "test_tag2");
            proj.AddAssetTagToDefaultInstall(target, "test_tag3");
            proj.AddAssetTagForFile(target, f2, "test_tag_remove1");
            proj.AddAssetTagForFile(target, f3, "test_tag_remove2");
            proj.AddAssetTagToDefaultInstall(target, "test_tag_remove2");
            proj = Reserialize(proj);
            proj.RemoveAssetTagForFile(target, f2, "test_tag_remove1");
            proj.RemoveAssetTag("test_tag_remove2");
            proj.RemoveAssetTagFromDefaultInstall(target, "test_tag3");
            TestOutput(proj, "asset_tags1.pbxproj");
        }

        [Test]
        public void AddExternalReferenceWorks()
        {
            PBXProject proj;
            string target;
            
            ResetGuidGenerator();
            proj = ReadPBXProject();
            target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.AddExternalProjectDependency("UnityDevProject/UnityDevProject.xcodeproj", "UnityDevProject.xcodeproj",
                                              PBXSourceTree.Source);
            TestOutput(proj, "add_external_ref1.pbxproj");
            
            ResetGuidGenerator();
            proj = ReadPBXProject();
            target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.AddExternalProjectDependency("UnityDevProject/UnityDevProject.xcodeproj", "UnityDevProject.xcodeproj",
                                              PBXSourceTree.Source);
            proj.AddExternalProjectDependency("UnityDevProject/UnityDevProject2.xcodeproj", "UnityDevProject2.xcodeproj",
                                              PBXSourceTree.Source);
            proj.AddExternalLibraryDependency(target, "UnityDevProject.a", "AA88A7D019101316001E7AB7",
                                              "UnityDevProject/UnityDevProject.xcodeproj", "UnityDevProject");
            TestOutput(proj, "add_external_ref2.pbxproj");
        }

        [Test]
        public void RemoveFilesRecursiveWorks()
        {
            PBXProject proj;
            string target;
            
            ResetGuidGenerator();
            proj = ReadPBXProject();
            target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.AddFileToBuild(target, proj.AddFile("relative/path1.cc", "Classes/path/path1.cc", PBXSourceTree.Source));
            proj.AddFileToBuild(target, proj.AddFile("/absolute/path/abs1.cc", "Classes/path/path2.cc", PBXSourceTree.Source));
            proj.AddFileToBuild(target, proj.AddFile("/absolute/path/abs2.cc", "Classes/path/path3.cc", PBXSourceTree.Source));
            proj.AddFileToBuild(target, proj.AddFile("/absolute/path/abs3.cc", "Classes/path/path2/path4.cc", PBXSourceTree.Source));
            proj = Reserialize(proj);
            proj.RemoveFilesByProjectPathRecursive("Classes/path");
            TestOutput(proj, "rm_recursive1.pbxproj");
        }

        [Test]
        public void ProjectGuidWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            Assert.AreEqual("29B97313FDCFA39411CA2CEA", proj.ProjectGuid());
        }

        [Test]
        public void BuildConfigNamesWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            Assert.AreEqual(new List<string>{"Release"}, proj.BuildConfigNames());
        }

        [Test]
        public void AddBuildConfigWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            var targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            proj.AddBuildConfig("Debug");
            Assert.AreEqual(new List<string>{"Release", "Debug"}, proj.BuildConfigNames());
            Assert.AreEqual("C01FCF5008A954540054247B", proj.BuildConfigByName(proj.ProjectGuid(), "Release"));
            Assert.AreEqual("1D6058950D05DD3E006BFB54", proj.BuildConfigByName(targetGuid, "Release"));
            Assert.AreEqual("CCCCCCCC0000000000000001", proj.BuildConfigByName(proj.ProjectGuid(), "Debug"));
            Assert.AreEqual("CCCCCCCC0000000000000002", proj.BuildConfigByName(targetGuid, "Debug"));
        }

        [Test]
        public void AddBuildConfigThrowsExceptionOnDuplicate()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();

            proj.AddBuildConfig("Existing");
            Assert.Throws<System.Exception>(() => proj.AddBuildConfig("Existing"));
        }
 
        [Test]
        public void RemoveBuildConfigWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            var targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            proj.AddBuildConfig("Debug");
            proj = Reserialize(proj);

            proj.RemoveBuildConfig("Debug");
            proj.RemoveBuildConfig("NotExisting"); // should be ignored

            Assert.AreEqual(new List<string>{"Release"}, proj.BuildConfigNames());
            Assert.AreEqual("C01FCF5008A954540054247B", proj.BuildConfigByName(proj.ProjectGuid(), "Release"));
            Assert.AreEqual("1D6058950D05DD3E006BFB54", proj.BuildConfigByName(targetGuid, "Release"));
            Assert.AreEqual(null, proj.BuildConfigByName(proj.ProjectGuid(), "Debug"));
            Assert.AreEqual(null, proj.BuildConfigByName(targetGuid, "Debug"));
        }

        [Test]
        public void AddTargetOutputIsExpected()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            string newTarget = proj.AddTarget("TestTarget", ".dylib", "test.type");
            proj.AddTargetDependency(target, newTarget);
            TestOutput(proj, "add_target1.pbxproj");
        }

        [Test]
        public void AddTargetAddsRequiredBuildConfigs()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            proj.AddBuildConfig("Debug");
            string targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            string newTargetGuid = proj.AddTarget("TestTarget", ".dylib", "test.type");

            Assert.AreEqual("C01FCF5008A954540054247B", proj.BuildConfigByName(proj.ProjectGuid(), "Release"));
            Assert.AreEqual("1D6058950D05DD3E006BFB54", proj.BuildConfigByName(targetGuid, "Release"));
            Assert.AreEqual("CCCCCCCC0000000000000006", proj.BuildConfigByName(newTargetGuid, "Release"));

            Assert.AreEqual("CCCCCCCC0000000000000001", proj.BuildConfigByName(proj.ProjectGuid(), "Debug"));
            Assert.AreEqual("CCCCCCCC0000000000000002", proj.BuildConfigByName(targetGuid, "Debug"));
            Assert.AreEqual("CCCCCCCC0000000000000007", proj.BuildConfigByName(newTargetGuid, "Debug"));
        }

        [Test]
        public void AddBuildPhasesOutputIsExpected()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.AddTarget("TestTarget", ".dylib", "test.type");
            proj.AddSourcesBuildPhase(target);
            proj.AddResourcesBuildPhase(target);
            proj.AddFrameworksBuildPhase(target);
            proj.AddCopyFilesBuildPhase(target, "Copy resources", "$(DST_PATH)", "13");
            TestOutput(proj, "add_build_phases1.pbxproj");
        }

        [Test]
        public void AddBuildPhasesReturnsExistingBuildPhase()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.AddTarget("TestTarget", ".dylib", "test.type");

            Assert.IsNull(proj.GetSourcesBuildPhaseByTarget(target));
            Assert.AreEqual("CCCCCCCC0000000000000005", proj.AddSourcesBuildPhase(target));
            Assert.AreEqual("CCCCCCCC0000000000000005", proj.GetSourcesBuildPhaseByTarget(target));
            Assert.AreEqual("CCCCCCCC0000000000000005", proj.AddSourcesBuildPhase(target));

            Assert.IsNull(proj.GetResourcesBuildPhaseByTarget(target));
            Assert.AreEqual("CCCCCCCC0000000000000006", proj.AddResourcesBuildPhase(target));
            Assert.AreEqual("CCCCCCCC0000000000000006", proj.GetResourcesBuildPhaseByTarget(target));
            Assert.AreEqual("CCCCCCCC0000000000000006", proj.AddResourcesBuildPhase(target));

            Assert.IsNull(proj.GetFrameworksBuildPhaseByTarget(target));
            Assert.AreEqual("CCCCCCCC0000000000000007", proj.AddFrameworksBuildPhase(target));
            Assert.AreEqual("CCCCCCCC0000000000000007", proj.GetFrameworksBuildPhaseByTarget(target));
            Assert.AreEqual("CCCCCCCC0000000000000007", proj.AddFrameworksBuildPhase(target));

            Assert.IsNull(proj.GetCopyFilesBuildPhaseByTarget(target, "Copy files", "", "13"));
            Assert.AreEqual("CCCCCCCC0000000000000008", proj.AddCopyFilesBuildPhase(target, "Copy files", "", "13"));
            Assert.AreEqual("CCCCCCCC0000000000000008", proj.GetCopyFilesBuildPhaseByTarget(target, "Copy files", "", "13"));
            Assert.AreEqual("CCCCCCCC0000000000000008", proj.AddCopyFilesBuildPhase(target, "Copy files", "", "13"));

            // check whether all parameters are actually matched against existing phases
            Assert.IsNull(proj.GetCopyFilesBuildPhaseByTarget(target, "Copy files2", "", "13"));
            Assert.IsNull(proj.GetCopyFilesBuildPhaseByTarget(target, "Copy files", "path", "13"));
            Assert.IsNull(proj.GetCopyFilesBuildPhaseByTarget(target, "Copy files", "", "14"));

            Assert.AreEqual("CCCCCCCC0000000000000009", proj.AddCopyFilesBuildPhase(target, "Copy files2", "", "13"));
            Assert.AreEqual("CCCCCCCC0000000000000009", proj.GetCopyFilesBuildPhaseByTarget(target, "Copy files2", "", "13"));
            Assert.AreEqual("CCCCCCCC0000000000000010", proj.AddCopyFilesBuildPhase(target, "Copy files", "path", "13"));
            Assert.AreEqual("CCCCCCCC0000000000000010", proj.GetCopyFilesBuildPhaseByTarget(target, "Copy files", "path", "13"));
            Assert.AreEqual("CCCCCCCC0000000000000011", proj.AddCopyFilesBuildPhase(target, "Copy files", "", "14"));
            Assert.AreEqual("CCCCCCCC0000000000000011", proj.GetCopyFilesBuildPhaseByTarget(target, "Copy files", "", "14"));
        }

        [Test]
        public void AddAppExtensionOutputIsExpected()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            proj.AddBuildConfig("Debug");
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.AddAppExtension(target, "App Extension", "com.company.product.appextension", "App Extension/Info.plist");
            TestOutput(proj, "add_app_extension.pbxproj");
        }

        [Test]
        public void AddWatchExtensionOutputIsExpected()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            proj.AddBuildConfig("Debug");
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.AddWatchExtension(target, "Watchtest Extension", "com.company.product.watchapp.watchextension", "Watchtest Extension/Info.plist");
            TestOutput(proj, "add_watch_extension.pbxproj");
        }

        [Test]
        public void AddWatchAppAndExtensionOutputIsExpected()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            proj.AddBuildConfig("Debug");
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            string extTargetGuid = proj.AddWatchExtension(target, "watchtest Extension", "com.company.product.watchapp.watchextension", "watchtest Extension/Info.plist");
            proj.AddWatchApp(target, extTargetGuid, "watchtest", "com.company.product.watchapp", "watchtest/Info.plist");
            TestOutput(proj, "add_watch_app_and_extension.pbxproj");
        }

        [Test]
        public void AddEmbedFrameworkWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            // first, include a framework as a regular file
            var fileGuid = proj.AddFile("path/test.framework", "Frameworks/test.framework");
            Assert.AreEqual("CCCCCCCC0000000000000001", fileGuid);

            proj.AddFileToEmbedFrameworks(target, fileGuid);
            Assert.IsNotNull(proj.GetCopyFilesBuildPhaseByTarget(target, "Embed Frameworks", "", "10"));

            proj = Reserialize(proj);

            var buildFile = proj.BuildFilesGetForSourceFile(target, fileGuid);
            Assert.IsNotNull(buildFile);
            Assert.IsTrue(buildFile.codeSignOnCopy);
            Assert.IsTrue(buildFile.removeHeadersOnCopy);

            var copyPhaseGuid = proj.GetCopyFilesBuildPhaseByTarget(target, "Embed Frameworks", "", "10");
            Assert.IsTrue(proj.copyFiles[copyPhaseGuid].files.Contains(buildFile.guid));
        }

        [Test]
        public void WhenUnknownSectionExistsAddCopyFilesBuildPhaseWorks()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject("base_unknown_section.pbxproj");
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            
            Assert.IsNotNull(proj.GetSourcesBuildPhaseByTarget(target));
            Assert.IsNotNull(proj.GetResourcesBuildPhaseByTarget(target));
            Assert.IsNotNull(proj.GetFrameworksBuildPhaseByTarget(target));
            
            //Adding additional sections when an unknown section is already present in pbxproj should work
            Assert.IsNull(proj.GetCopyFilesBuildPhaseByTarget(target, "Copy files", "", "13"));
            Assert.IsNotNull(proj.AddCopyFilesBuildPhase(target, "Copy files", "", "13"));
        }

        [Test]
        public void StrippedProjectReadingWorks()
        {
            PBXProject proj = ReadPBXProject("base_stripped.pbxproj");
            TestOutput(proj, "stripped1.pbxproj");
        }

        [Test]
        public void UnknownFileTypesOutputIsExpected()
        {
            PBXProject proj = ReadPBXProject("base_unknown.pbxproj");
            TestOutput(proj, "unknown1.pbxproj");
        }

        [Test]
        public void InvalidProjectRepairOutputIsExpected()
        {
            PBXProject proj = ReadPBXProject("base_repair.pbxproj");
            TestOutput(proj, "repair1.pbxproj");
        }

        [Test]
        public void AddCapabilityOutputIsExpected()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.AddCapability(target, PBXCapabilityType.GameCenter);
            TestOutput(proj, "add_capability.pbxproj");
        }

        [Test]
        public void AddCapabilityWithEntitlementOutputIsExpected()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.AddCapability(target, PBXCapabilityType.iCloud, "test.entitlements");
            TestOutput(proj, "add_capability_entitlement.pbxproj");
        }

        [Test]
        public void AddMultipleCapabilitiesOutputIsExpected()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.AddCapability(target, PBXCapabilityType.GameCenter);
            proj.AddCapability(target, PBXCapabilityType.InAppPurchase);
            proj.AddCapability(target, PBXCapabilityType.Maps);
            TestOutput(proj, "add_multiple_capabilities.pbxproj");
        }

        [Test]
        public void AddMultipleCapabilitiesWithEntitlementOutputIsExpected()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.AddCapability(target, PBXCapabilityType.iCloud, "test.entitlements");
            proj.AddCapability(target, PBXCapabilityType.ApplePay, "test.entitlements");
            proj.AddCapability(target, PBXCapabilityType.Siri, "test.entitlements");
            TestOutput(proj, "add_multiple_capabilities_entitlement.pbxproj");
        }

        [Test]
        public void SetTeamIdOutputIsExpected()
        {
            ResetGuidGenerator();
            PBXProject proj = ReadPBXProject();
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            proj.SetTeamId(target, "Z6SFPV59E3");
            TestOutput(proj, "set_teamid.pbxproj");
        }
    }
}
