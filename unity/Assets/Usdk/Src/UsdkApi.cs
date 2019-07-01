using System;
using System.Collections.Generic;
using UnityEngine;

namespace Usdk {
    public static class UsdkApi {
        public const string PLATFORM_NAME = "PlatformProxy";
        private static IUsdkApi api = null;

        static UsdkApi () {
            //sdk api
            if (Application.platform == RuntimePlatform.Android)
                api = new UsdkAndroidApi ();
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                api = new UsdkiOSApi ();
            else
                api = new UsdkWindowsApi ();

            //sdk callback
            UsdkCallBack callBack = UsdkCallBack.Create ();
            callBack.OnCallBack = OnUsdkCallBack;
        }

        #region sdk api
        public static string GetConfig (string key) {
            if (key == null)
                return string.Empty;
            return CallPlugin<string> (PLATFORM_NAME, "getConfig", key);
        }

        public static void Login () {
            CallPlugin (PLATFORM_NAME, "login");
        }

        public static void Logout () {
            CallPlugin (PLATFORM_NAME, "logout");
        }

        public static void Pay (SdkPayInfo payInfo) {
            CallPlugin (PLATFORM_NAME, "pay", payInfo.ToString ());
        }

        public static void Quit () {
            CallPlugin (PLATFORM_NAME, "exitGame");
        }

        public static void OpenUserCenter () {
            CallPlugin (PLATFORM_NAME, "openUserCenter");
        }

        public static void SwitchAccount () {
            CallPlugin (PLATFORM_NAME, "switchAccount");
        }

        public static void OpenAppstoreComment (string appid) {
            CallPlugin (PLATFORM_NAME, "openAppstoreComment", appid);
        }

        public static void ReleaseSdkResource () {
            CallPlugin (PLATFORM_NAME, "releaseSdkResource");
        }

        public static void CallPlugin (string pluginName, string methodName, params string[] parameters) {
            api.CallPlugin (pluginName, methodName, parameters);
        }

        public static R CallPlugin<R> (string pluginName, string methodName, params string[] parameters) {
            return api.CallPlugin<R> (pluginName, methodName, parameters);
        }
        #endregion

        #region sdk callback
        private static void OnUsdkCallBack (int errorCode, List<string> msg) {
            if (errorCode == (int)UsdkCallBackErrorCode.InitSuccess) {
                //初始化成功
                Debug.Log ("sdk init success");
            } else if (errorCode == (int)UsdkCallBackErrorCode.InitFail) {
                //初始化失败
                Debug.Log ("sdk init failed");
            } else if (errorCode == (int)UsdkCallBackErrorCode.ExitNoChannelExiter) {
                //SDK无退出页，需要调用游戏内部退出页
                Application.Quit ();
            } else if (errorCode == (int)UsdkCallBackErrorCode.ExitSuccess) {
                //SDK自带退出页并且已经确认退出
                Application.Quit ();
            } else if (errorCode == (int)UsdkCallBackErrorCode.LogoutFinish) {
                //SDK退出成功
                Debug.Log ("logout success");
            } else if (errorCode == (int)UsdkCallBackErrorCode.LoginSuccess) {
                //SDK登录成功
                Debug.Log ("login success");
            } else if (errorCode == (int)UsdkCallBackErrorCode.LoginCancel) {
                //SDK取消登录
                Debug.Log ("login cancel");
            } else if (errorCode == (int)UsdkCallBackErrorCode.LoginFail) {
                //SDK登录失败
                Debug.Log ("login failed");
            } else if (errorCode == (int)UsdkCallBackErrorCode.PaySuccess) {
                //SDK支付成功
                Debug.Log ("pay success");
            } else if (errorCode == (int)UsdkCallBackErrorCode.PayCancel) {
                //SDK取消支付
                Debug.Log ("pay cancel");
            } else if (errorCode == (int)UsdkCallBackErrorCode.PayProgress) {
                //SDK支付进行中
                Debug.Log ("pay progress");
            } else if (errorCode == (int)UsdkCallBackErrorCode.PayOthers) {
                //SDK其他支付错误码
                Debug.Log ("pay other errorcode");
            } else if (errorCode == (int)UsdkCallBackErrorCode.PayFail) {
                //SDK支付失败
                Debug.Log ("pay failed");
            } else {
                Debug.LogError (string.Format ("usdk call back error.'{0}' undefine in 'UsdkCallBackErrorCode'", errorCode));
            }
        }
        #endregion
    }
}