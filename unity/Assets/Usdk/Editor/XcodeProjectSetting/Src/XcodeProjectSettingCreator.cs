using UnityEngine;
using UnityEditor;


public class XcodeProjectSettingCreator
{
    [MenuItem("Assets/Create/XcodeProjectSetting")]
    public static void CreateAsset()
    {
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Usdk/Editor/XcodeProjectSetting/Setting/XcodeProjectSetting.asset");
        //string path = "Assets/Editor/XCodeAPI/Setting/XcodeProjectSetting.asset";

        XcodeProjectSetting data = ScriptableObject.CreateInstance<XcodeProjectSetting>();
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.SaveAssets();
    }
}