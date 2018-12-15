using System;
using UnityEngine;

namespace Usdk
{
    public class AndroidSdkAPI : IUsdk
    {
        private static AndroidSdkAPI _instance = null;
        public static AndroidSdkAPI instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AndroidSdkAPI();
                return _instance;
            }
        }

#if UNITY_ANDROID
        private static AndroidJavaObject currentActivity
        {
            get
            {
                AndroidJavaClass player = new AndroidJavaClass("com.usdk.sdk.UsdkFactory");
                AndroidJavaObject context = player.CallStatic<AndroidJavaObject>("getProxy");
                return context;
            }
        }
#endif

        private void SendAndroidMessage(string method, params object[] parameters)
        {
#if UNITY_ANDROID
		    if(currentActivity != null)
                currentActivity.Call(method, parameters);
#endif
        }

        private T SendAndroidMessage<T>(string method, params object[] parameters)
        {
#if UNITY_ANDROID
		    if(currentActivity != null)
                return currentActivity.Call<T>(method, parameters);
#endif
            return default(T);
        }

        public void setSdkCallBackReceiver(string receiver_name_)
        {
            SendAndroidMessage("setCallBackReceiver", receiver_name_);
        }

        public void login(string custom_params_)
        {
            if (string.IsNullOrEmpty(custom_params_))
                custom_params_ = string.Empty;
            SendAndroidMessage("login", custom_params_);
        }

        public void logout(string custom_params_)
        {
            if (string.IsNullOrEmpty(custom_params_))
                custom_params_ = string.Empty;
            SendAndroidMessage("logout", custom_params_);
        }

        public void openUserCenter(string custom_params_)
        {
            if (string.IsNullOrEmpty(custom_params_))
                custom_params_ = string.Empty;
            SendAndroidMessage("openUserCenter", custom_params_);
        }

        public void exit(string custom_params_)
        {
            if (string.IsNullOrEmpty(custom_params_))
                custom_params_ = string.Empty;
            SendAndroidMessage("exit", custom_params_);
        }

        public void pay(SdkPayInfo pay_info_)
        {
            SendAndroidMessage("pay", pay_info_.ToString());
        }

        public void releaseSdkResource(string custom_params_)
        {
            if (string.IsNullOrEmpty(custom_params_))
                custom_params_ = string.Empty;

            SendAndroidMessage("releaseSdkResource", custom_params_);
        }

        public void switchAccount(string custom_params_)
        {
            if (string.IsNullOrEmpty(custom_params_))
                custom_params_ = string.Empty;

            SendAndroidMessage("switchAccount", custom_params_);
        }

        //appid app在GP 或 App Store中的AppID标识
        public void openAppstoreComment(string appid)
        {
            SendAndroidMessage("openAppstoreComment", appid);
        }

        public string getConfig(string key)
        {
            return SendAndroidMessage<string>("getConfig", key);
        }
    }
}