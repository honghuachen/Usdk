using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Usdk
{
    public class UsdkApi
    {
        public const string PLATFORM_NAME = "PlatformProxy";
#if UNITY_ANDROID
        private static IUsdkApi api = new UsdkAndroidApi ();
#elif UNITY_IOS
        private static IUsdkApi api = new UsdkiOSApi ();
#else
        private static IUsdkApi api = new UsdkWindowsApi();
#endif

        public static UsdkCallBack CreateCallBack()
        {
            //sdk callback
            UsdkCallBack callBack = UsdkCallBack.Create();
            callBack.OnCallBack = OnUsdkCallBack;
            return callBack;
        }

        #region sdk api
        public static void SetCallBack(string pluginName, UsdkCallBackListener callBack)
        {
            CallPlugin(pluginName, "setCallBack", callBack);
        }

        public static string GetConfig(string key)
        {
            if (key == null)
                return string.Empty;
            return CallPluginR<string>(PLATFORM_NAME, "getConfig", key);
        }

        public static void CallPlugin(string pluginName, string methodName, params object[] parameters)
        {
            api.CallPlugin(pluginName, methodName, parameters);
        }

        public static R CallPluginR<R>(string pluginName, string methodName, params object[] parameters)
        {
            return api.CallPlugin<R>(pluginName, methodName, parameters);
        }

        public static object CallPluginR(string type, string pluginName, string methodName, params object[] parameters)
        {
            if (type == "int")
                return CallPluginR<int>(pluginName, methodName, parameters);
            else if (type == "float")
                return CallPluginR<float>(pluginName, methodName, parameters);
            else if (type == "long")
                return CallPluginR<long>(pluginName, methodName, parameters);
            else if (type == "bool")
                return CallPluginR<bool>(pluginName, methodName, parameters);
            else if (type == "string")
                return CallPluginR<string>(pluginName, methodName, parameters);
            else if (type == "double")
                return CallPluginR<double>(pluginName, methodName, parameters);
            else if (type == "object")
                return CallPluginR<AndroidJavaObject>(pluginName, methodName, parameters);

            return null;
        }

        public bool IsExistPlugin(string pluginName)
        {
            return api.IsExistPlugin(pluginName);
        }

        public bool isExistField(string pluginName, string fieldName)
        {
            return api.isExistField(pluginName, fieldName);
        }

        public bool isExistMethod(string pluginName, string methodName)
        {
            return api.isExistMethod(pluginName, methodName);
        }
        #endregion

        #region sdk callback
        private static void OnUsdkCallBack(int errorCode, List<string> msg)
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
            else if (errorCode == (int)UsdkCallBackErrorCode.ExitNoChannelExiter)
            {
                //SDK无退出页，需要调用游戏内部退出页
                Application.Quit();
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.ExitSuccess)
            {
                //SDK自带退出页并且已经确认退出
                Application.Quit();
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.LogoutFinish)
            {
                //SDK退出成功
                Debug.Log("logout success");
            }
            else if (errorCode == (int)UsdkCallBackErrorCode.LoginSuccess)
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
            else if (errorCode == (int)UsdkCallBackErrorCode.PaySuccess)
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
            else
            {
                Debug.LogError(string.Format("usdk call back error.'{0}' undefine in 'UsdkCallBackErrorCode'", errorCode));
            }
        }
        #endregion
    }
}