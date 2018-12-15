using System;
using UnityEngine;

namespace Usdk
{
    public class UsdkEditorApi : IUsdk
    {
        private static UsdkEditorApi _instance = null;
        public static UsdkEditorApi instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UsdkEditorApi();
                return _instance;
            }
        }

        public void setSdkCallBackReceiver(string receiver_name_) { }

        public void login(string custom_params_) { }

        public void logout(string custom_params_)
        {
            string retMsg = string.Format("errorCode={0}&msg={1}", (int)UsdkCallBackErrorCode.LogoutFinish, null);
            GameObject.Find("SDKCallBack").SendMessage("logoutCallBack", retMsg);
        }

        public void openUserCenter(string custom_params_) { }

        public void exit(string custom_params_)
        {
            string retMsg = string.Format("errorCode={0}&msg={1}", (int)UsdkCallBackErrorCode.ExitNoChannelExiter, null);
            GameObject.Find("SDKCallBack").SendMessage("exitGameCallBack", retMsg);
        }

        public void payStart(string product_id, int amount) { }

        public void pay(SdkPayInfo pay_info_)
        {
            string retMsg = string.Format("errorCode={0}&msg={1}", (int)UsdkCallBackErrorCode.PaySuccess, null);
            GameObject.Find("UsdkCallBack").SendMessage("payCallBack", retMsg);
        }

        public void releaseSdkResource(string custom_params_) { }

        public void switchAccount(string custom_params_) { }

        //appid app在GP 或 App Store中的AppID标识
        public void openAppstoreComment(string appid) { }

        public string getConfig(string key)
        {
            return string.Empty;
        }
    }
}