using UnityEngine;
//#if UNITY_STANDALONE_OSX
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections.Generic;
using System.IO;
//#endif

public class XCodeProjectMod : MonoBehaviour
{
//#if UNITY_STANDALONE_OSX
    private const string SETTING_DATA_PATH = "Assets/Usdk/Editor/XcodeProjectSetting/Setting/XcodeProjectSetting.asset";
    [PostProcessBuild]
    private static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget != BuildTarget.iOS)
            return;
        string pbxProjPath = PBXProject.GetPBXProjectPath(buildPath);
        XcodeProjectSetting setting = AssetDatabase.LoadAssetAtPath<XcodeProjectSetting>(SETTING_DATA_PATH);
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromString(File.ReadAllText(pbxProjPath));
        string targetGuid = pbxProject.TargetGuidByName(PBXProject.GetUnityTargetName());

        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.ENABLE_BITCODE_KEY, setting.EnableBitCode ? "YES" : "NO");
        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.DEVELOPMENT_TEAM, setting.DevelopmentTeam);
        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.GCC_ENABLE_CPP_EXCEPTIONS, setting.EnableCppEcceptions ? "YES" : "NO");
        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.GCC_ENABLE_CPP_RTTI, setting.EnableCppRtti ? "YES" : "NO");
        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.GCC_ENABLE_OBJC_EXCEPTIONS, setting.EnableObjcExceptions ? "YES" : "NO");

        //if (!string.IsNullOrEmpty(setting.CopyDirectoryPath))
        //    DirectoryProcessor.CopyAndAddBuildToXcode(pbxProject, targetGuid, setting.CopyDirectoryPath, buildPath, "");

        //编译器标记（Compiler flags）
        foreach (XcodeProjectSetting.CompilerFlagsSet compilerFlagsSet in setting.CompilerFlagsSetList)
        {
            foreach (string targetPath in compilerFlagsSet.TargetPathList)
            {
                if (!pbxProject.ContainsFileByProjectPath(targetPath))
                    continue;
                string fileGuid = pbxProject.FindFileGuidByProjectPath(targetPath);
                List<string> flagsList = pbxProject.GetCompileFlagsForFile(targetGuid, fileGuid);
                flagsList.Add(compilerFlagsSet.Flags);
                pbxProject.SetCompileFlagsForFile(targetGuid, fileGuid, flagsList);
            }
        }

        //引用内部框架
        foreach (string framework in setting.FrameworkList)
        {
            pbxProject.AddFrameworkToProject(targetGuid, framework, false);
        }

        //引用.tbd文件
        foreach (string tbd in setting.TbdList)
        {
            pbxProject.AddFileToBuild(targetGuid, pbxProject.AddFile("usr/lib/" + tbd, "Frameworks/" + tbd, PBXSourceTree.Sdk));
        }

        //设置OTHER_LDFLAGS
        pbxProject.UpdateBuildProperty(targetGuid, XcodeProjectSetting.LINKER_FLAG_KEY, setting.LinkerFlagArray, null);
        //设置Framework Search Paths
        pbxProject.UpdateBuildProperty(targetGuid, XcodeProjectSetting.FRAMEWORK_SEARCH_PATHS_KEY, setting.FrameworkSearchPathArray, null);
        File.WriteAllText(pbxProjPath, pbxProject.WriteToString());

        //已经存在的文件，拷贝替换
        foreach (XcodeProjectSetting.CopeFiles file in setting.CopeFilesList)
        {
            File.Copy(Application.dataPath + file.sourcePath, buildPath + file.copyPath, true);
        }

        //File.Copy(Application.dataPath + "/Editor/XCodeAPI/UnityAppController.h", buildPath + "/Classes/UnityAppController.h", true);
        //File.Copy(Application.dataPath + "/Editor/XCodeAPI/UnityAppController.mm", buildPath + "/Classes/UnityAppController.mm", true);

        //设置Plist
        InfoPlistProcessor.SetInfoPlist(buildPath, setting);
    }
//#endif
}