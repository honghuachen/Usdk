using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Xcode项目的一些设定值
/// </summary>
public class XcodeProjectSetting : ScriptableObject
{
    public const string PROJECT_ROOT = "$(PROJECT_DIR)/";
    public const string IMAGE_XCASSETS_DIRECTORY_NAME = "Unity-iPhone";
    public const string LINKER_FLAG_KEY = "OTHER_LDFLAGS";
    public const string FRAMEWORK_SEARCH_PATHS_KEY = "FRAMEWORK_SEARCH_PATHS";
    public const string LIBRARY_SEARCH_PATHS_KEY = "LIBRARY_SEARCH_PATHS";
    public const string ENABLE_BITCODE_KEY = "ENABLE_BITCODE";
    public const string DEVELOPMENT_TEAM = "DEVELOPMENT_TEAM";
    public const string GCC_ENABLE_CPP_EXCEPTIONS = "GCC_ENABLE_CPP_EXCEPTIONS";
    public const string GCC_ENABLE_CPP_RTTI = "GCC_ENABLE_CPP_RTTI";
    public const string GCC_ENABLE_OBJC_EXCEPTIONS = "GCC_ENABLE_OBJC_EXCEPTIONS";
    public const string INFO_PLIST_NAME = "Info.plist";

    public const string URL_TYPES_KEY = "CFBundleURLTypes";
    public const string URL_TYPE_ROLE_KEY = "CFBundleTypeRole";
    public const string URL_IDENTIFIER_KEY = "CFBundleURLName";
    public const string URL_SCHEMES_KEY = "CFBundleURLSchemes";
    public const string APPLICATION_QUERIES_SCHEMES_KEY = "LSApplicationQueriesSchemes";

    #region XCodeproj
    public bool EnableBitCode = false;
    public bool EnableCppEcceptions = true;
    public bool EnableCppRtti = true;
    public bool EnableObjcExceptions = true;

    //要拷贝到XCode内的文件的路径
    public string CopyDirectoryPath = "Assets/Editor/XCodeAPI/Frameworks";
    //AppleDevelopment内AppID表示
    public string DevelopmentTeam = "";
    //引用的内部Framework
    public List<string> FrameworkList = new List<string>() { };
    //引用的内部.tbd
    public List<string> TbdList = new List<string>() { };
    //设置OtherLinkerFlag
    public string[] LinkerFlagArray = new string[] { };
    //设置FrameworkSearchPath
    public string[] FrameworkSearchPathArray = new string[] { "$(inherited)", "$(PROJECT_DIR)/Frameworks" };

    #region 针对单个文件进行flag标记
    [System.Serializable]
    public struct CompilerFlagsSet
    {
        public string Flags;
        public List<string> TargetPathList;

        public CompilerFlagsSet(string flags, List<string> targetPathList)
        {
            Flags = flags;
            TargetPathList = targetPathList;
        }
    }

    public List<CompilerFlagsSet> CompilerFlagsSetList = new List<CompilerFlagsSet>()
    {
        /*new CompilerFlagsSet ("-fno-objc-arc", new List<string> () {"Plugin/Plugin.mm"})*/ //实例，请勿删除
    };
    #endregion

    #endregion

    #region 拷贝文件
    [System.Serializable]
    public struct CopeFiles
    {
        public string sourcePath;
        public string copyPath;

        public CopeFiles(string sourcePath, string copyPath)
        {
            this.sourcePath = sourcePath;
            this.copyPath = copyPath;
        }
    }

    public List<CopeFiles> CopeFilesList = new List<CopeFiles>() { };
    #endregion

    #region info.Plist
    //白名单
    public List<string> ApplicationQueriesSchemes = new List<string>() { };

    //iOS10新的特性
    public List<string> privacySensiticeData = new List<string>() { };

    #region 第三方平台URL Scheme
    [System.Serializable]
    public struct BundleUrlType
    {
        public string identifier;
        public List<string> bundleSchmes;

        public BundleUrlType(string identifier, List<string> bundleSchmes)
        {
            this.identifier = identifier;
            this.bundleSchmes = bundleSchmes;
        }
    }

    public List<BundleUrlType> BundleUrlTypeList = new List<BundleUrlType>() { };
    #endregion

    //放置后台需要开启的功能
    public List<string> BackgroundModes = new List<string>() { };
    #endregion
}