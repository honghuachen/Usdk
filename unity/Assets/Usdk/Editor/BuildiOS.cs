using System;
using System.Collections.Generic;
using UnityEditor;

public class BuildiOS : Editor
{
    //在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }

    //导出xcode工程
    public static void Build()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        string buildType = string.Empty, platform = string.Empty, subPlatform = string.Empty, versionName = string.Empty, versionCode = string.Empty,
            package = string.Empty, appName = string.Empty, cdn = string.Empty, plugins = string.Empty, splash = string.Empty, icon = string.Empty,
            xcodeOut = string.Empty;
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            if (arg.StartsWith("appName", StringComparison.OrdinalIgnoreCase))
            {
                appName = arg.Split('=')[1];
                PlayerSettings.productName = appName;
                PlayerSettings.iOS.applicationDisplayName = appName;
            }
            else if (arg.StartsWith("package", StringComparison.OrdinalIgnoreCase))
            {
                package = arg.Split('=')[1];
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, package);
            }
            else if (arg.StartsWith("versionName", StringComparison.OrdinalIgnoreCase))
            {
                versionName = arg.Split('=')[1];
                PlayerSettings.bundleVersion = versionName;
            }
            else if (arg.StartsWith("versionCode", StringComparison.OrdinalIgnoreCase))
            {
                versionCode = arg.Split('=')[1];
                PlayerSettings.iOS.buildNumber = versionCode;
            }
            else if (arg.StartsWith("buildType", StringComparison.OrdinalIgnoreCase))
                buildType = arg.Split('=')[1];
            else if (arg.StartsWith("platform", StringComparison.OrdinalIgnoreCase))
                platform = arg.Split('=')[1];
            else if (arg.StartsWith("subPlatform", StringComparison.OrdinalIgnoreCase))
                subPlatform = arg.Split('=')[1];
            else if (arg.StartsWith("plugins", StringComparison.OrdinalIgnoreCase))
                plugins = arg.Split('=')[1];
            else if (arg.StartsWith("splash", StringComparison.OrdinalIgnoreCase))
                splash = arg.Split('=')[1];
            else if (arg.StartsWith("icon", StringComparison.OrdinalIgnoreCase))
                icon = arg.Split('=')[1];
            else if (arg.StartsWith("url", StringComparison.OrdinalIgnoreCase))
                cdn = arg.Split('=')[1];
            else if (arg.StartsWith("xcodeOut", StringComparison.OrdinalIgnoreCase))
                xcodeOut = arg.Split('=')[1];
        }

        PlayerSettings.iOS.appleEnableAutomaticSigning = false;
        // SetAutomaticallyAddCapabilities(false);
        
        // PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad; //目标设备
        // PlayerSettings.iOS.targetOSVersionString = "8.0"; //最低iOS版本要求
        // PlayerSettings.iOS.statusBarStyle = iOSStatusBarStyle.Default;
        // PlayerSettings.statusBarHidden = true;
        // PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        // PlayerSettings.allowedAutorotateToLandscapeRight = true;
        // PlayerSettings.allowedAutorotateToPortrait = false;
        // PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        // PlayerSettings.accelerometerFrequency = 30;

        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_Standard_2_0);
        // PlayerSettings.SetPropertyInt("ScriptingBackend", (int)ScriptingImplementation.IL2CPP, BuildTargetGroup.iOS);
        // PlayerSettings.SetPropertyInt("Architecture", (int)iOSTargetDevice.iPhoneAndiPad, BuildTargetGroup.iOS); //支持armv7和arm64
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        // PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 2);
        // PlayerSettings.iOS.requiresPersistentWiFi = true;


        BuildPipeline.BuildPlayer(GetBuildScenes(), xcodeOut, BuildTarget.iOS, BuildOptions.None);
    }

    private static void SetAutomaticallyAddCapabilities(bool auto)
    {
        System.Reflection.Assembly editorAsm = System.Reflection.Assembly.GetAssembly(typeof(Editor));
        System.Type t = typeof(UnityEditor.PlayerSettings.iOS);
        System.Reflection.PropertyInfo method = t.GetProperty("automaticallyDetectAndAddCapabilities", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        method.SetValue(null, auto);
    }
}