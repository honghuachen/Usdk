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
            UsdkApi.Login();
        }

        if (GUI.Button (new Rect (0, 50, 100, 50), "Logout")) {
            UsdkApi.Logout();
        }

        if (GUI.Button (new Rect (0, 100, 100, 50), "Pay")) {
            SdkPayInfo info = new SdkPayInfo();
            info.payAmount = 100;
            UsdkApi.Pay(info);
        }

        if (GUI.Button (new Rect (0, 150, 100, 50), "Quit")) {
            UsdkApi.Quit();
        }

        if (GUI.Button (new Rect (0, 200, 100, 50), "OpenUserCenter")) {
            UsdkApi.OpenUserCenter();
        }

        if (GUI.Button (new Rect (0, 250, 100, 50), "SwitchAccount")) {
            UsdkApi.SwitchAccount();
        }

        if (GUI.Button (new Rect (0, 300, 100, 50), "OpenAppstoreComment")) {
            UsdkApi.OpenAppstoreComment("appid");
        }

        if (GUI.Button (new Rect (0, 350, 100, 50), "ReleaseSdkResource")) {
            UsdkApi.ReleaseSdkResource();
        }

        if (GUI.Button (new Rect (0, 400, 100, 50), "CallPlugin")) {
            UsdkApi.CallPlugin("PlatformProxy","callPlugin","arg1","1","true","1.15");
        }

         if (GUI.Button (new Rect (0, 450, 100, 50), "CallPluginR")) {
            int ret = UsdkApi.CallPlugin<int>("PlatformProxy","callPluginR","arg2","2","false","24.15");
            Debug.Log("[unity]CallPluginR ret="+ret);
        }
    }
}