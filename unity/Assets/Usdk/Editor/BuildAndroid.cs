using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Text;

public struct BuildGenernalSetting
{
    public string projectName;
    public string channel;
    public string version;
    public int generation;
    public string prefix;
    public string postfix;
    public string outputPath;
    public string companyName;
    public string productName;

    public bool isDebug; 
}

public class BuildAndroid : Editor
{
	
	//在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
	static string[] GetBuildScenes()
	{
		List<string> names = new List<string>();
		foreach(EditorBuildSettingsScene e in EditorBuildSettings.scenes)
		{
			if(e==null)
				continue;
			if(e.enabled)
				names.Add(e.path);
		}
		return names.ToArray();
	}

    static void BuildForAndroid()
    {
        string keystoreName = string.Empty;
        string keystorePass = string.Empty;
        string keyaliasName = string.Empty;
        string keyaliasPass = string.Empty;
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            if (arg.StartsWith("keystoreName", StringComparison.OrdinalIgnoreCase))
            {
                keystoreName = arg.Split('=')[1];
            }
            else if (arg.StartsWith("keystorePass", StringComparison.OrdinalIgnoreCase))
            {
                keystorePass = arg.Split('=')[1];
            }
            else if (arg.StartsWith("keyaliasName", StringComparison.OrdinalIgnoreCase))
            {
                keyaliasName = arg.Split('=')[1];
            }
            else if (arg.StartsWith("keyaliasPass", StringComparison.OrdinalIgnoreCase))
            {
                keyaliasPass = arg.Split('=')[1];
            }
        }

        // Setting kestore information
        if (!string.IsNullOrEmpty(keystorePass) &&
            !string.IsNullOrEmpty(keyaliasPass) &&
            !string.IsNullOrEmpty(keyaliasName))
        {
            PlayerSettings.Android.keystoreName = keystoreName;
            PlayerSettings.Android.keystorePass = keystorePass;
            PlayerSettings.Android.keyaliasName = keyaliasName;
            PlayerSettings.Android.keyaliasPass = keyaliasPass;
        }

        BuildGenernalSetting buildGenernalSetting = ApplyGeneralSettings();
        AssetDatabase.Refresh();
        string output = string.Format("{0}/../Publish/Android/Apks/{1}", Application.dataPath, buildGenernalSetting.outputPath);
        BuildPipeline.BuildPlayer(GetBuildScenes(), output, BuildTarget.Android, BuildOptions.None);
    }

    private static BuildGenernalSetting ApplyGeneralSettings()
    {
        BuildGenernalSetting settings = new BuildGenernalSetting();
        foreach (string arg in Environment.GetCommandLineArgs()) {
            if (arg.StartsWith("project_name", StringComparison.OrdinalIgnoreCase)) {
                settings.projectName = arg.Split('=')[1];
            } else if (arg.StartsWith("identifier_prefix", StringComparison.OrdinalIgnoreCase)) {
                settings.prefix = arg.Split('=')[1];
            } else if (arg.StartsWith("identifier_postfix", StringComparison.OrdinalIgnoreCase)) {
                settings.postfix = arg.Split('=')[1];
            } else if (arg.StartsWith("channel", StringComparison.OrdinalIgnoreCase)) {
                settings.channel = arg.Split('=')[1];
            } else if (arg.StartsWith("version", StringComparison.OrdinalIgnoreCase)) {
                settings.version = arg.Split('=')[1];
            } else if (arg.StartsWith("generation", StringComparison.OrdinalIgnoreCase)) {
                string code = arg.Split('=')[1];
                int.TryParse(code, out settings.generation);
            } else if (arg.StartsWith("publish", StringComparison.OrdinalIgnoreCase)) {
                settings.isDebug = true;
                string code = arg.Split('=')[1];
                if (code == "release")
                    settings.isDebug = false;
            } else if (arg.StartsWith("output_path", StringComparison.OrdinalIgnoreCase)) {
                settings.outputPath = arg.Split('=')[1];
            } else if (arg.StartsWith("companyName", StringComparison.OrdinalIgnoreCase)) {
                settings.companyName = arg.Split('=')[1];
            } else if (arg.StartsWith("productName", StringComparison.OrdinalIgnoreCase)) {
                settings.productName = arg.Split('=')[1];
            }
        }

        string identifier = settings.prefix;
        if (!string.IsNullOrEmpty(settings.postfix))
        {
            if (!string.IsNullOrEmpty(identifier))
            {
                identifier += "." + settings.postfix;
            }
            else
            {
                identifier = settings.postfix;
            }
        }

        PlayerSettings.companyName = settings.companyName;
        PlayerSettings.productName = settings.productName;
        PlayerSettings.applicationIdentifier = identifier;
        PlayerSettings.bundleVersion = settings.version;
        PlayerSettings.Android.bundleVersionCode = settings.generation;
        return settings;
    }

    static void DeleteFolder(string dir)
    {
        if (!Directory.Exists(dir))
            return;
        foreach (string d in Directory.GetFileSystemEntries(dir))
        {
            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(d);
            }
            else
            {
                DirectoryInfo d1 = new DirectoryInfo(d);
                if (d1.GetFiles().Length != 0)
                {
                    DeleteFolder(d1.FullName);////递归删除子文件夹
                }
                Directory.Delete(d);
            }
        }
    }

    static void CopyDirectory(string sourcePath, string destinationPath)
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        Directory.CreateDirectory(destinationPath);
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);
            if (fsi is System.IO.FileInfo)
                File.Copy(fsi.FullName, destName);
            else
            {
                Directory.CreateDirectory(destName);
                CopyDirectory(fsi.FullName, destName);
            }
        }
    }

}
