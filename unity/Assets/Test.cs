using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Usdk;

public class Test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        //无返回值
        if (GUI.Button(new Rect(0, 400, 100, 50), "CallPlugin"))
        {
            UsdkApi.CallPlugin("PlatformProxy", "callPlugin", "arg1", "1", "true", "1.15");
        }

        //返回int类型值
        if (GUI.Button(new Rect(0, 450, 100, 50), "CallPluginRInt"))
        {
            int ret = UsdkApi.CallPluginR<int>("PlatformProxy", "callPluginRInt", "arg2", "2", "false", "24.15");
            Debug.Log("[unity]callPluginRInt ret=" + ret);
        }
        //返回string类型值
        if (GUI.Button(new Rect(0, 500, 100, 50), "CallPluginRString"))
        {
            string ret = UsdkApi.CallPluginR<string>("PlatformProxy", "callPluginRString", "arg2", "2", "false", "24.15");
            Debug.Log("[unity]callPluginRString ret=" + ret);
        }
        //返回bool类型值
        if (GUI.Button(new Rect(0, 550, 100, 50), "CallPluginRBool"))
        {
            bool ret = UsdkApi.CallPluginR<bool>("PlatformProxy", "callPluginRBool", "arg2", "2", "false", "24.15");
            Debug.Log("[unity]callPluginRBool ret=" + ret);
        }
        //返回float类型值
        if (GUI.Button(new Rect(0, 600, 100, 50), "CallPluginRFloat"))
        {
            float ret = UsdkApi.CallPluginR<float>("PlatformProxy", "callPluginRFloat", "arg2", "2", "false", "24.15");
            Debug.Log("[unity]callPluginRFloat ret=" + ret);
        }
    }
}