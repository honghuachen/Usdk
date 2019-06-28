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
        if (GUI.Button (new Rect (0, 0, 100, 100), "Login")) {
            UsdkApi.Login();
        }

        if (GUI.Button (new Rect (0, 100, 100, 100), "Logout")) {
            UsdkApi.Logout();
        }

        if (GUI.Button (new Rect (0, 200, 100, 100), "Pay")) {
            SdkPayInfo info = new SdkPayInfo();
            info.payAmount = 100;
            UsdkApi.Pay(info);
        }

        if (GUI.Button (new Rect (0, 300, 100, 100), "Quit")) {
            UsdkApi.Quit();
        }

        if (GUI.Button (new Rect (0, 400, 100, 100), "OpenUserCenter")) {
            UsdkApi.OpenUserCenter();
        }

        if (GUI.Button (new Rect (0, 500, 100, 100), "SwitchAccount")) {
            UsdkApi.SwitchAccount();
        }

        if (GUI.Button (new Rect (0, 600, 100, 100), "OpenAppstoreComment")) {
            UsdkApi.OpenAppstoreComment("appid");
        }

        if (GUI.Button (new Rect (0, 700, 100, 100), "ReleaseSdkResource")) {
            UsdkApi.ReleaseSdkResource();
        }

        if (GUI.Button (new Rect (0, 700, 100, 100), "CallPugin")) {
            UsdkApi.CallPugin("PlatformProxy","callPugin","arg1","1","true","1.15");
        }

         if (GUI.Button (new Rect (0, 700, 100, 100), "CallPuginR")) {
            int ret = UsdkApi.CallPugin<int>("PlatformProxy","callPuginR","arg2","2","false","24.15");
            Debug.Log("[unity]callPuginR ret="+ret);
        }
    }
}