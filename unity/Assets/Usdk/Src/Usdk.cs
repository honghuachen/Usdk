using System.Collections.Generic;
using UnityEngine;

namespace Usdk
{
    public class Usdk
    {
        private IUsdkApi api = null;
        public static Usdk _instance = null;
        public static Usdk Instance
        {
            get
            {
                if (_instance == null) 
                    _instance = new Usdk();
                return _instance;
            }
        }

        public Usdk()
        {
            //sdk api
#if UNITY_EDITOR
            api = new UsdkEditorApi();
#elif UNITY_ANDROID
            api = new UsdkAndroidApi();
#elif UNITY_IOS
            api = new UsdkiOSApi();
#endif

            //sdk callback
            UsdkCallBack callBack = UsdkCallBack.Create();
            callBack.OnInitSDKCallBack = OnInitSDKCallBack;
            callBack.OnLoginCallBack = OnLoginCallBack;
            callBack.OnLogoutCallBack = OnLogoutCallBack;
            callBack.OnExitGameCallBack = OnExitGameCallBack;
            callBack.OnPayCallBack = OnPayCallBack;
            SetSdkCallBackReceiver(callBack.gameObject.name);
        }

        #region sdk api
        public void SetSdkCallBackReceiver(string receiverName)
        {
            if (string.IsNullOrEmpty(receiverName))
                return;
            api.setSdkCallBackReceiver(receiverName);
        }

        public string GetConfig(string key)
        {
            if (key == null)
                return string.Empty;
            return api.getConfig(key);
        }

        public void Login(string arg = "")
        {
            api.login(arg);
        }

        public void Logout(string arg = "")
        {
            api.logout(arg);
        }

        public void Pay(SdkPayInfo payInfo)
        {
            api.pay(payInfo);
        }

        public void Exit(string arg = "") {
            api.exit(arg);
        }

        public void OpenUserCenter(string arg = "")
        {
            api.openUserCenter(arg);
        }

        public void SwitchAccount(string arg = "")
        {
            api.switchAccount(arg);
        }

        public void OpenAppstoreComment(string appid)
        {
            api.openAppstoreComment(appid);
        }

        public void ReleaseSdkResource(string arg = "")
        {
            api.releaseSdkResource(arg);
        }

        public void CallPugin(string pluginName, string methodName, params object[] parameters)
        {
           api.callPugin(pluginName, methodName, parameters);
        }

        public string CallPuginR(string pluginName, string methodName, params object[] parameters)
        {
            return api.callPuginR(pluginName, methodName, parameters);
        }
        #endregion

        #region sdk callback
        private void OnPayCallBack(int errorCode, List<string> msg)
        {
            if (errorCode == (int)UsdkCallBackErrorCode.InitSuccess)
            {
                //初始化成功
                Debug.Log("sdk init success");
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.InitFail)
            {
                //初始化失败
                Debug.Log("sdk init failed");
            }
        }

        private void OnExitGameCallBack(int errorCode, List<string> msg)
        {
            Debug.Log("exit game");
            if (errorCode == (int)UsdkCallBackErrorCode.ExitNoChannelExiter)
            {
                //SDK无退出页，需要调用游戏内部退出页
                Application.Quit();
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.ExitSuccess)
            {
                //SDK自带退出页并且已经确认退出
                Application.Quit();
            }
        }

        private void OnLogoutCallBack(int errorCode, List<string> msg)
        {
            if (errorCode == (int)UsdkCallBackErrorCode.LogoutFinish)
            {
                //SDK退出成功
                Debug.Log("logout success");
            }
        }

        private void OnLoginCallBack(int errorCode, List<string> msg)
        {
            if (errorCode == (int)UsdkCallBackErrorCode.LoginSuccess)
            {
                //SDK登录成功
                Debug.Log("login success");
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.LoginCancel)
            {
                //SDK取消登录
                Debug.Log("login cancel");
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.LoginFail)
            {
                //SDK登录失败
                Debug.Log("login failed");
            }
        }

        private void OnInitSDKCallBack(int errorCode, List<string> msg)
        {
            if (errorCode == (int)UsdkCallBackErrorCode.PaySuccess)
            {
                //SDK支付成功
                Debug.Log("pay success");
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.PayCancel)
            {
                //SDK取消支付
                Debug.Log("pay cancel");
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.PayProgress)
            {
                //SDK支付进行中
                Debug.Log("pay progress");
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.PayOthers)
            {
                //SDK其他支付错误码
                Debug.Log("pay other errorcode");
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.PayFail)
            {
                //SDK支付失败
                Debug.Log("pay failed");
            }
        }
        #endregion
    }
}
