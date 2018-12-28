using UnityEngine;
using UnityEditor;


public class XcodeProjectSettingCreator : MonoBehaviour
{
    [MenuItem("Assets/Create/XcodeProjectSetting")]
    public static void CreateAsset()
    {
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Editor/XCodeAPI/Setting/XcodeProjectSetting.asset");
        XcodeProjectSetting data = ScriptableObject.CreateInstance<XcodeProjectSetting>();
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.SaveAssets();
    }
}