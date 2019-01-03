using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using System.IO;

//Info.Plist修改设置
public static class InfoPlistProcessor
{
    private static string GetInfoPlistPath(string buildPath)
    {
        return Path.Combine(buildPath, XcodeProjectSetting.INFO_PLIST_NAME);
    }

    private static PlistDocument GetInfoPlist(string buildPath)
    {
        string plistPath = GetInfoPlistPath(buildPath);
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);
        return plist;
    }

    private static void SetPrivacySensiticeData(PlistDocument plist, List<string> permission, string description = "")
    {
        PlistElementDict rootDict = plist.root;
        int count = permission.Count;
        for (int i = 0; i < count; i++)
        {
            rootDict.SetString(permission[i], description);
        }
    }

    private static void SetApplicationQueriesSchemes(PlistDocument plist, List<string> _applicationQueriesSchemes)
    {
        PlistElementArray queriesSchemes;
        int count = _applicationQueriesSchemes.Count;
        string queriesScheme = null;

        if (plist.root.values.ContainsKey(XcodeProjectSetting.APPLICATION_QUERIES_SCHEMES_KEY))
            queriesSchemes = plist.root[XcodeProjectSetting.APPLICATION_QUERIES_SCHEMES_KEY].AsArray();
        else
            queriesSchemes = plist.root.CreateArray(XcodeProjectSetting.APPLICATION_QUERIES_SCHEMES_KEY);

        for (int i = 0; i < count; i++)
        {
            queriesScheme = _applicationQueriesSchemes[i];
            if (!queriesSchemes.values.Contains(new PlistElementString(queriesScheme)))
                queriesSchemes.AddString(queriesScheme);
        }
    }

    private static void SetBackgroundModes(PlistDocument plist, List<string> modes)
    {
        PlistElementDict rootDict = plist.root;
        PlistElementArray bgModes = rootDict.CreateArray("UIBackgroundModes");
        int count = modes.Count;
        for (int i = 0; i < count; i++)
        {
            bgModes.AddString(modes[i]);
        }
    }

    private static void SetURLSchemes(PlistDocument plist, List<XcodeProjectSetting.BundleUrlType> urlList)
    {
        PlistElementArray urlTypes;
        PlistElementDict itmeDict;
        if (plist.root.values.ContainsKey(XcodeProjectSetting.URL_TYPES_KEY))
            urlTypes = plist.root[XcodeProjectSetting.URL_TYPES_KEY].AsArray();
        else
            urlTypes = plist.root.CreateArray(XcodeProjectSetting.URL_TYPES_KEY);

        for (int i = 0; i < urlList.Count; i++)
        {
            itmeDict = urlTypes.AddDict();
            itmeDict.SetString(XcodeProjectSetting.URL_TYPE_ROLE_KEY, "Editor");
            itmeDict.SetString(XcodeProjectSetting.URL_IDENTIFIER_KEY, urlList[i].identifier);
            PlistElementArray schemesArray = itmeDict.CreateArray(XcodeProjectSetting.URL_SCHEMES_KEY);
            if (itmeDict.values.ContainsKey(XcodeProjectSetting.URL_SCHEMES_KEY))
                schemesArray = itmeDict[XcodeProjectSetting.URL_SCHEMES_KEY].AsArray();
            else
                schemesArray = itmeDict.CreateArray(XcodeProjectSetting.URL_SCHEMES_KEY);
            //TODO:按理说要排除已经存在的，但由于我们是新生成，所以不做排除
            for (int j = 0; j < urlList[i].bundleSchmes.Count; j++)
            {
                schemesArray.AddString(urlList[i].bundleSchmes[j]);
            }
        }
    }

    public static void SetInfoPlist(string buildPath, XcodeProjectSetting setting)
    {
        PlistDocument plist = GetInfoPlist(buildPath);
        SetPrivacySensiticeData(plist, setting.privacySensiticeData, "privacySensiticeData");
        SetApplicationQueriesSchemes(plist, setting.ApplicationQueriesSchemes);
        SetBackgroundModes(plist, setting.BackgroundModes);
        SetURLSchemes(plist, setting.BundleUrlTypeList);
        plist.WriteToFile(GetInfoPlistPath(buildPath));
    }





  

}