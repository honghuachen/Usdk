using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System;
#if UNITY_XCODE_API_BUILD
using UnityEditor.iOS.Xcode.PBX;
#else
using UnityEditor.iOS.Xcode.Custom.PBX;
#endif

#if UNITY_XCODE_API_BUILD
namespace UnityEditor.iOS.Xcode.Extensions
#else
namespace UnityEditor.iOS.Xcode.Custom.Extensions
#endif
{
    /*  This class implements a number of static methods for performing common tasks
        on xcode projects. 
        TODO: Make sure enough stuff is exposed so that it's possible to perform the tasks
        without using internal APIs
    */
    public static class PBXProjectExtensions
    {
        // Create a wrapper class so that collection initializers work and we can have a 
        // compact notation. Note that we can't use Dictionary because the keys may be duplicate
        internal class FlagList : List<KeyValuePair<string, string>>
        {
            public void Add(string flag, string value)
            {
                Add(new KeyValuePair<string, string>(flag, value));
            }
        }

        internal static FlagList appExtensionReleaseBuildFlags = new FlagList
        {
            // { "INFOPLIST_FILE", <path/to/info.plist> },
            { "LD_RUNPATH_SEARCH_PATHS", "$(inherited)" },
            { "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks" },
            { "LD_RUNPATH_SEARCH_PATHS", "@executable_path/../../Frameworks" },
            // { "PRODUCT_BUNDLE_IDENTIFIER", "<bundle id>" },
            { "PRODUCT_NAME", "$(TARGET_NAME)" },
            { "SKIP_INSTALL", "YES" },
        };

        internal static FlagList appExtensionDebugBuildFlags = new FlagList
        {
            // { "INFOPLIST_FILE", <path/to/info.plist> },
            { "LD_RUNPATH_SEARCH_PATHS", "$(inherited)" },
            { "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks" },
            { "LD_RUNPATH_SEARCH_PATHS", "@executable_path/../../Frameworks" },
            // { "PRODUCT_BUNDLE_IDENTIFIER", "<bundle id>" },
            { "PRODUCT_NAME", "$(TARGET_NAME)" },
            { "SKIP_INSTALL", "YES" },
        };

        internal static FlagList watchExtensionReleaseBuildFlags = new FlagList
        {
            { "ASSETCATALOG_COMPILER_COMPLICATION_NAME", "Complication" },
            { "CLANG_ANALYZER_NONNULL", "YES" },
            { "CLANG_WARN_DOCUMENTATION_COMMENTS", "YES" },
            { "CLANG_WARN_INFINITE_RECURSION", "YES" },
            { "CLANG_WARN_SUSPICIOUS_MOVE", "YES" },
            { "DEBUG_INFORMATION_FORMAT", "dwarf-with-dsym" },
            { "GCC_NO_COMMON_BLOCKS", "YES" },
            //{ "INFOPLIST_FILE", "<path/to/Info.plist>" },
            { "LD_RUNPATH_SEARCH_PATHS", "$(inherited)" },
            { "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks" },
            { "LD_RUNPATH_SEARCH_PATHS", "@executable_path/../../Frameworks" },
            // { "PRODUCT_BUNDLE_IDENTIFIER", "<bundle id>" },
            { "PRODUCT_NAME", "${TARGET_NAME}" },
            { "SDKROOT", "watchos" },
            { "SKIP_INSTALL", "YES" },
            { "TARGETED_DEVICE_FAMILY", "4" },
            { "WATCHOS_DEPLOYMENT_TARGET", "3.1" },
            // the following are needed to override project settings in Unity Xcode project
            { "ARCHS", "$(ARCHS_STANDARD)" },
            { "SUPPORTED_PLATFORMS", "watchos" },
            { "SUPPORTED_PLATFORMS", "watchsimulator" },
        };

        internal static FlagList watchExtensionDebugBuildFlags = new FlagList
        {
            { "ASSETCATALOG_COMPILER_COMPLICATION_NAME", "Complication" },
            { "CLANG_ANALYZER_NONNULL", "YES" },
            { "CLANG_WARN_DOCUMENTATION_COMMENTS", "YES" },
            { "CLANG_WARN_INFINITE_RECURSION", "YES" },
            { "CLANG_WARN_SUSPICIOUS_MOVE", "YES" },
            { "DEBUG_INFORMATION_FORMAT", "dwarf" },
            { "ENABLE_TESTABILITY", "YES" },
            { "GCC_NO_COMMON_BLOCKS", "YES" },
            // { "INFOPLIST_FILE", "<path/to/Info.plist>" },
            { "LD_RUNPATH_SEARCH_PATHS", "$(inherited)" },
            { "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks" },
            { "LD_RUNPATH_SEARCH_PATHS", "@executable_path/../../Frameworks" },
            // { "PRODUCT_BUNDLE_IDENTIFIER", "<bundle id>" },
            { "PRODUCT_NAME", "${TARGET_NAME}" },
            { "SDKROOT", "watchos" },
            { "SKIP_INSTALL", "YES" },
            { "TARGETED_DEVICE_FAMILY", "4" },
            { "WATCHOS_DEPLOYMENT_TARGET", "3.1" },
            // the following are needed to override project settings in Unity Xcode project
            { "ARCHS", "$(ARCHS_STANDARD)" },
            { "SUPPORTED_PLATFORMS", "watchos" },
            { "SUPPORTED_PLATFORMS", "watchsimulator" },
        };

        internal static FlagList watchAppReleaseBuildFlags = new FlagList
        {
            { "ASSETCATALOG_COMPILER_APPICON_NAME", "AppIcon" },
            { "CLANG_ANALYZER_NONNULL", "YES" },
            { "CLANG_WARN_DOCUMENTATION_COMMENTS", "YES" },
            { "CLANG_WARN_INFINITE_RECURSION", "YES" },
            { "CLANG_WARN_SUSPICIOUS_MOVE", "YES" },
            { "DEBUG_INFORMATION_FORMAT", "dwarf-with-dsym" },
            { "GCC_NO_COMMON_BLOCKS", "YES" },
            //{ "IBSC_MODULE", "the extension target name with ' ' replaced with '_'" },
            //{ "INFOPLIST_FILE", "<path/to/Info.plist>" },
            //{ "PRODUCT_BUNDLE_IDENTIFIER", "<bundle id>" },
            { "PRODUCT_NAME", "$(TARGET_NAME)" },
            { "SDKROOT", "watchos" },
            { "SKIP_INSTALL", "YES" },
            { "TARGETED_DEVICE_FAMILY", "4" },
            { "WATCHOS_DEPLOYMENT_TARGET", "3.1" },
            // the following are needed to override project settings in Unity Xcode project
            { "ARCHS", "$(ARCHS_STANDARD)" },
            { "SUPPORTED_PLATFORMS", "watchos" },
            { "SUPPORTED_PLATFORMS", "watchsimulator" },
        };

        internal static FlagList watchAppDebugBuildFlags = new FlagList
        {
            { "ASSETCATALOG_COMPILER_APPICON_NAME", "AppIcon" },
            { "CLANG_ANALYZER_NONNULL", "YES" },
            { "CLANG_WARN_DOCUMENTATION_COMMENTS", "YES" },
            { "CLANG_WARN_INFINITE_RECURSION", "YES" },
            { "CLANG_WARN_SUSPICIOUS_MOVE", "YES" },
            { "DEBUG_INFORMATION_FORMAT", "dwarf" },
            { "ENABLE_TESTABILITY", "YES" },
            { "GCC_NO_COMMON_BLOCKS", "YES" },
            //{ "IBSC_MODULE", "the extension target name with ' ' replaced with '_'" },
            //{ "INFOPLIST_FILE", "<path/to/Info.plist>" },
            //{ "PRODUCT_BUNDLE_IDENTIFIER", "<bundle id>" },
            { "PRODUCT_NAME", "$(TARGET_NAME)" },
            { "SDKROOT", "watchos" },
            { "SKIP_INSTALL", "YES" },
            { "TARGETED_DEVICE_FAMILY", "4" },
            { "WATCHOS_DEPLOYMENT_TARGET", "3.1" },
            // the following are needed to override project settings in Unity Xcode project
            { "ARCHS", "$(ARCHS_STANDARD)" },
            { "SUPPORTED_PLATFORMS", "watchos" },
            { "SUPPORTED_PLATFORMS", "watchsimulator" },
        };

        static void SetBuildFlagsFromDict(this PBXProject proj, string configGuid, IEnumerable<KeyValuePair<string, string>> data)
        {
            foreach (var kv in data)
                proj.AddBuildPropertyForConfig(configGuid, kv.Key, kv.Value);
        }

        internal static void SetDefaultAppExtensionReleaseBuildFlags(this PBXProject proj, string configGuid)
        {
            SetBuildFlagsFromDict(proj, configGuid, appExtensionReleaseBuildFlags);
        }

        internal static void SetDefaultAppExtensionDebugBuildFlags(this PBXProject proj, string configGuid)
        {
            SetBuildFlagsFromDict(proj, configGuid, appExtensionDebugBuildFlags);
        }

        internal static void SetDefaultWatchExtensionReleaseBuildFlags(this PBXProject proj, string configGuid)
        {
            SetBuildFlagsFromDict(proj, configGuid, watchExtensionReleaseBuildFlags);
        }

        internal static void SetDefaultWatchExtensionDebugBuildFlags(this PBXProject proj, string configGuid)
        {
            SetBuildFlagsFromDict(proj, configGuid, watchExtensionDebugBuildFlags);
        }

        internal static void SetDefaultWatchAppReleaseBuildFlags(this PBXProject proj, string configGuid)
        {
            SetBuildFlagsFromDict(proj, configGuid, watchAppReleaseBuildFlags);
        }

        internal static void SetDefaultWatchAppDebugBuildFlags(this PBXProject proj, string configGuid)
        {
            SetBuildFlagsFromDict(proj, configGuid, watchAppDebugBuildFlags);
        }

        /// <summary>
        /// Creates an app extension.
        /// </summary>
        /// <returns>The GUID of the new target.</returns>
        /// <param name="proj">A project passed as this argument.</param>
        /// <param name="mainTargetGuid">The GUID of the main target to link the app to.</param>
        /// <param name="name">The name of the app extension.</param>
        /// <param name="bundleId">The bundle ID of the app extension. The bundle ID must be
        /// prefixed with the parent app bundle ID.</param>
        /// <param name="infoPlistPath">Path to the app extension Info.plist document.</param>
        public static string AddAppExtension(this PBXProject proj, string mainTargetGuid, 
                                             string name, string bundleId, string infoPlistPath)
        {
            string ext = ".appex";
            var newTargetGuid = proj.AddTarget(name, ext, "com.apple.product-type.app-extension");

            foreach (var configName in proj.BuildConfigNames())
            {
                var configGuid = proj.BuildConfigByName(newTargetGuid, configName);
                if (configName.Contains("Debug"))
                    SetDefaultAppExtensionDebugBuildFlags(proj, configGuid);
                else
                    SetDefaultAppExtensionReleaseBuildFlags(proj, configGuid);
                proj.SetBuildPropertyForConfig(configGuid, "INFOPLIST_FILE", infoPlistPath);
                proj.SetBuildPropertyForConfig(configGuid, "PRODUCT_BUNDLE_IDENTIFIER", bundleId);
            }

            proj.AddSourcesBuildPhase(newTargetGuid);
            proj.AddResourcesBuildPhase(newTargetGuid);
            proj.AddFrameworksBuildPhase(newTargetGuid);
            string copyFilesPhaseGuid = proj.AddCopyFilesBuildPhase(mainTargetGuid, "Embed App Extensions", "", "13");
            proj.AddFileToBuildSection(mainTargetGuid, copyFilesPhaseGuid, proj.GetTargetProductFileRef(newTargetGuid));

            proj.AddTargetDependency(mainTargetGuid, newTargetGuid);

            return newTargetGuid;
        }

        /// <summary>
        /// Creates a watch application.
        /// </summary>
        /// <returns>The GUID of the new target.</returns>
        /// <param name="proj">A project passed as this argument.</param>
        /// <param name="mainTargetGuid">The GUID of the main target to link the watch app to.</param>
        /// <param name="watchExtensionTargetGuid">The GUID of watch extension as returned by [[AddWatchExtension()]].</param>
        /// <param name="name">The name of the watch app. It must the same as the name of the watch extension.</param>
        /// <param name="bundleId">The bundle ID of the watch app.</param>
        /// <param name="infoPlistPath">Path to the watch app Info.plist document.</param>
        public static string AddWatchApp(this PBXProject proj, string mainTargetGuid, string watchExtensionTargetGuid, 
                                         string name, string bundleId, string infoPlistPath)
        {
            var newTargetGuid = proj.AddTarget(name, ".app", "com.apple.product-type.application.watchapp2");

            var isbcModuleName = proj.nativeTargets[watchExtensionTargetGuid].name.Replace(" ", "_");

            foreach (var configName in proj.BuildConfigNames())
            {
                var configGuid = proj.BuildConfigByName(newTargetGuid, configName);
                if (configName.Contains("Debug"))
                    SetDefaultWatchAppDebugBuildFlags(proj, configGuid);
                else
                    SetDefaultWatchAppReleaseBuildFlags(proj, configGuid);
                proj.SetBuildPropertyForConfig(configGuid, "PRODUCT_BUNDLE_IDENTIFIER", bundleId);
                proj.SetBuildPropertyForConfig(configGuid, "INFOPLIST_FILE", infoPlistPath);
                proj.SetBuildPropertyForConfig(configGuid, "IBSC_MODULE", isbcModuleName);
            }

            proj.AddResourcesBuildPhase(newTargetGuid);
            string copyFilesGuid = proj.AddCopyFilesBuildPhase(newTargetGuid, "Embed App Extensions", "", "13");
            proj.AddFileToBuildSection(newTargetGuid, copyFilesGuid, proj.GetTargetProductFileRef(watchExtensionTargetGuid));

            string copyWatchFilesGuid = proj.AddCopyFilesBuildPhase(mainTargetGuid, "Embed Watch Content", "$(CONTENTS_FOLDER_PATH)/Watch", "16");
            proj.AddFileToBuildSection(mainTargetGuid, copyWatchFilesGuid, proj.GetTargetProductFileRef(newTargetGuid));

            proj.AddTargetDependency(newTargetGuid, watchExtensionTargetGuid);
            proj.AddTargetDependency(mainTargetGuid, newTargetGuid);

            return newTargetGuid;
        }

        /// <summary>
        /// Creates a watch extension.
        /// </summary>
        /// <returns>The GUID of the new target.</returns>
        /// <param name="proj">A project passed as this argument.</param>
        /// <param name="mainTarget">The GUID of the main target to link the watch extension to.</param>
        /// <param name="name">The name of the watch extension.</param>
        /// <param name="bundleId">The bundle ID of the watch extension. The bundle ID must be
        /// prefixed with the parent watch app bundle ID.</param>
        /// <param name="infoPlistPath">Path to the watch extension Info.plist document.</param>
        public static string AddWatchExtension(this PBXProject proj, string mainTarget, 
                                               string name, string bundleId, string infoPlistPath)
        {
            var newTargetGuid = proj.AddTarget(name, ".appex", "com.apple.product-type.watchkit2-extension");

            foreach (var configName in proj.BuildConfigNames())
            {
                var configGuid = proj.BuildConfigByName(newTargetGuid, configName);
                if (configName.Contains("Debug"))
                    SetDefaultWatchExtensionDebugBuildFlags(proj, configGuid);
                else
                    SetDefaultWatchExtensionReleaseBuildFlags(proj, configGuid);
                proj.SetBuildPropertyForConfig(configGuid, "PRODUCT_BUNDLE_IDENTIFIER", bundleId);
                proj.SetBuildPropertyForConfig(configGuid, "INFOPLIST_FILE", infoPlistPath);
            }

            proj.AddSourcesBuildPhase(newTargetGuid);
            proj.AddResourcesBuildPhase(newTargetGuid);
            proj.AddFrameworksBuildPhase(newTargetGuid);

            return newTargetGuid;
        }

        /// <summary>
        /// Adds an external project dependency to the project.
        /// </summary>
        /// <param name="path">The path to the external Xcode project (the .xcodeproj file).</param>
        /// <param name="projectPath">The project path to the new project.</param>
        /// <param name="sourceTree">The source tree the path is relative to. The [[PBXSourceTree.Group]] tree is not supported.</param>
        internal static void AddExternalProjectDependency(this PBXProject proj, string path, string projectPath, PBXSourceTree sourceTree)
        {
            if (sourceTree == PBXSourceTree.Group)
                throw new Exception("sourceTree must not be PBXSourceTree.Group");
            path = PBXPath.FixSlashes(path);
            projectPath = PBXPath.FixSlashes(projectPath);

            // note: we are duplicating products group for the project reference. Otherwise Xcode crashes.
            PBXGroupData productGroup = PBXGroupData.CreateRelative("Products");
            proj.GroupsAddDuplicate(productGroup); // don't use GroupsAdd here

            PBXFileReferenceData fileRef = PBXFileReferenceData.CreateFromFile(path, Path.GetFileName(projectPath),
                                                                               sourceTree);
            proj.FileRefsAdd(path, projectPath, null, fileRef);
            proj.CreateSourceGroup(PBXPath.GetDirectory(projectPath)).children.AddGUID(fileRef.guid);

            proj.project.project.AddReference(productGroup.guid, fileRef.guid);
        }

        /** This function must be called only after the project the library is in has
            been added as a dependency via AddExternalProjectDependency. projectPath must be
            the same as the 'path' parameter passed to the AddExternalProjectDependency.
            remoteFileGuid must be the guid of the referenced file as specified in
            PBXFileReference section of the external project

            TODO: what. is remoteInfo entry in PBXContainerItemProxy? Is in referenced project name or
            referenced library name without extension?
        */
        internal static void AddExternalLibraryDependency(this PBXProject proj, string targetGuid, string filename, string remoteFileGuid, string projectPath,
                                                          string remoteInfo)
        {
            PBXNativeTargetData target = proj.nativeTargets[targetGuid];
            filename = PBXPath.FixSlashes(filename);
            projectPath = PBXPath.FixSlashes(projectPath);

            // find the products group to put the new library in
            string projectGuid = proj.FindFileGuidByRealPath(projectPath);
            if (projectGuid == null)
                throw new Exception("No such project");

            string productsGroupGuid = null;
            foreach (var projRef in proj.project.project.projectReferences)
            {
                if (projRef.projectRef == projectGuid)
                {
                    productsGroupGuid = projRef.group;
                    break;
                }
            }

            if (productsGroupGuid == null)
                throw new Exception("Malformed project: no project in project references");

            PBXGroupData productGroup = proj.GroupsGet(productsGroupGuid);

            // verify file extension
            string ext = Path.GetExtension(filename);
            if (!FileTypeUtils.IsBuildableFile(ext))
                throw new Exception("Wrong file extension");

            // create ContainerItemProxy object
            var container = PBXContainerItemProxyData.Create(projectGuid, "2", remoteFileGuid, remoteInfo);
            proj.containerItems.AddEntry(container);

            // create a reference and build file for the library
            string typeName = FileTypeUtils.GetTypeName(ext);

            var libRef = PBXReferenceProxyData.Create(filename, typeName, container.guid, "BUILT_PRODUCTS_DIR");
            proj.references.AddEntry(libRef);
            PBXBuildFileData libBuildFile = PBXBuildFileData.CreateFromFile(libRef.guid, false, null);
            proj.BuildFilesAdd(targetGuid, libBuildFile);
            proj.BuildSectionAny(target, ext, false).files.AddGUID(libBuildFile.guid);

            // add to products folder
            productGroup.children.AddGUID(libRef.guid);
        }

        /// <summary>
        /// Configures file for embed framework section for the given native target.
        ///
        /// This function also internally calls <code>proj.AddFileToBuild(targetGuid, fileGuid)</code>
        /// to ensure that the framework is added to the list of linked frameworks.
        ///
        /// If the target has already configured the given file as embedded framework, this function has
        /// no effect.
        ///
        /// A projects containing multiple native targets, a single file or folder reference can be
        /// configured to be built in all, some or none of the targets. The file or folder reference is
        /// added to appropriate build section depending on the file extension.
        /// </summary>
        /// <param name="proj">A project passed as this argument.</param>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="fileGuid">The file GUID returned by [[AddFile]] or [[AddFolderReference]].</param>
        public static void AddFileToEmbedFrameworks(this PBXProject proj, string targetGuid, string fileGuid)
        {
            PBXNativeTargetData target = proj.nativeTargets[targetGuid];

            var phaseGuid = proj.AddCopyFilesBuildPhase(targetGuid, "Embed Frameworks", "", "10");
            var phase = proj.copyFiles[phaseGuid];
            var frameworkEmbedFileData = proj.FindFrameworkByFileGuid(phase, fileGuid);

            if (frameworkEmbedFileData == null)
            {
                frameworkEmbedFileData = PBXBuildFileData.CreateFromFile(fileGuid, false, null);
                proj.BuildFilesAdd(targetGuid, frameworkEmbedFileData);

                phase.files.AddGUID(frameworkEmbedFileData.guid);
            }

            frameworkEmbedFileData.codeSignOnCopy = true;
            frameworkEmbedFileData.removeHeadersOnCopy = true;
        }
    }
} // namespace UnityEditor.iOS.Xcode
