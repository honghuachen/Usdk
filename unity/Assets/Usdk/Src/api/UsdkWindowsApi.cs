using System;
using UnityEngine;

namespace Usdk
{
    public class UsdkWindowsApi : IUsdkApi
    {
        // public void setSdkCallBackReceiver(string receiverName) { }

        // public void login(string arg) { }

        // public void logout(string arg)
        // {
        //     string retMsg = string.Format("errorCode={0}&msg={1}", (int)UsdkCallBackErrorCode.LogoutFinish, null);
        //     GameObject.Find("SDKCallBack").SendMessage("logoutCallBack", retMsg);
        // }

        // public void openUserCenter(string arg) { }

        // public void exit(string arg)
        // {
        //     string retMsg = string.Format("errorCode={0}&msg={1}", (int)UsdkCallBackErrorCode.ExitNoChannelExiter, null);
        //     GameObject.Find("SDKCallBack").SendMessage("exitGameCallBack", retMsg);
        // }

        // public void payStart(string productId, int amount) { }

        // public void pay(SdkPayInfo payInfo)
        // {
        //     string retMsg = string.Format("errorCode={0}&msg={1}", (int)UsdkCallBackErrorCode.PaySuccess, null);
        //     GameObject.Find("UsdkCallBack").SendMessage("payCallBack", retMsg);
        // }

        // public void releaseSdkResource(string arg) { }

        // public void switchAccount(string arg) { }

        // //appid app在GP 或 App Store中的AppID标识
        // public void openAppstoreComment(string appid) { }

        // public string getConfig(string key)
        // {
        //     return string.Empty;
        // }

        public void CallPlugin(string pluginName, string methodName, params string[] parameters) { }

        public R CallPlugin<R>(string pluginName, string methodName, params string[] parameters)
        {
            return default(R);
        }
    }
}