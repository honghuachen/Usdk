using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Usdk;

public class Test : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    void OnGUI () {
        if (GUI.Button (new Rect (0, 0, 100, 50), "Login")) {
            UsdkApi.Login ();
        }

        if (GUI.Button (new Rect (0, 50, 100, 50), "Logout")) {
            UsdkApi.Logout ();
        }

        if (GUI.Button (new Rect (0, 100, 100, 50), "Pay")) {
            SdkPayInfo info = new SdkPayInfo ();
            info.payAmount = 100;
            UsdkApi.Pay (info);
        }

        if (GUI.Button (new Rect (0, 150, 100, 50), "Quit")) {
            UsdkApi.Quit ();
        }

        if (GUI.Button (new Rect (0, 200, 100, 50), "OpenUserCenter")) {
            UsdkApi.OpenUserCenter ();
        }

        if (GUI.Button (new Rect (0, 250, 100, 50), "SwitchAccount")) {
            UsdkApi.SwitchAccount ();
        }

        if (GUI.Button (new Rect (0, 300, 100, 50), "OpenAppstoreComment")) {
            UsdkApi.OpenAppstoreComment ("appid");
        }

        if (GUI.Button (new Rect (0, 350, 100, 50), "ReleaseSdkResource")) {
            UsdkApi.ReleaseSdkResource ();
        }

        //无返回值
        if (GUI.Button (new Rect (0, 400, 100, 50), "CallPlugin")) {
            UsdkApi.CallPlugin ("PlatformProxy", "callPlugin", "arg1", "1", "true", "1.15");
        }

        //返回int类型值
        if (GUI.Button (new Rect (0, 450, 100, 50), "CallPluginRInt")) {
            int ret = UsdkApi.CallPlugin<int> ("PlatformProxy", "callPluginRInt", "arg2", "2", "false", "24.15");
            Debug.Log ("[unity]callPluginRInt ret=" + ret);
        }
        //返回string类型值
        if (GUI.Button (new Rect (0, 500, 100, 50), "CallPluginRString")) {
            string ret = UsdkApi.CallPlugin<string> ("PlatformProxy", "callPluginRString", "arg2", "2", "false", "24.15");
            Debug.Log ("[unity]callPluginRString ret=" + ret);
        }
        //返回bool类型值
        if (GUI.Button (new Rect (0, 550, 100, 50), "CallPluginRBool")) {
            bool ret = UsdkApi.CallPlugin<bool> ("PlatformProxy", "callPluginRBool", "arg2", "2", "false", "24.15");
            Debug.Log ("[unity]callPluginRBool ret=" + ret);
        }
        //返回float类型值
        if (GUI.Button (new Rect (0, 600, 100, 50), "CallPluginRFloat")) {
            float ret = UsdkApi.CallPlugin<float> ("PlatformProxy", "callPluginRFloat", "arg2", "2", "false", "24.15");
            Debug.Log ("[unity]callPluginRFloat ret=" + ret);
        }
    }
}