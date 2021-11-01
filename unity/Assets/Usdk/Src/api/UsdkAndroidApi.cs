using System;
using UnityEngine;

namespace Usdk
{
    public class UsdkAndroidApi : IUsdkApi
    {
        const string platfromName = "PlatformProxy";
        const string pluginPreName = "com.usdk.plugin.";
#if UNITY_ANDROID
        private static AndroidJavaClass usdk {
            get {
                try {
                    AndroidJavaClass sdk = new AndroidJavaClass ("com.usdk.sdk.Usdk");
                    return sdk;
                } catch (Exception ex) {
                    Debug.LogError (ex.Message);
                    return null;
                }
            }
        }
#endif

        private void SendAndroidMessage(string pluginName, string method, params object[] parameters)
        {
#if UNITY_ANDROID
            try {
                pluginName = pluginPreName + pluginName;
                if (usdk != null) {
                    AndroidJavaObject context = usdk.CallStatic<AndroidJavaObject> ("getPlugin", pluginName);
                    if (context != null)
                        context.Call (method, parameters);
                }
            } catch (Exception ex) { Debug.LogError (ex.Message); }
#endif
        }

        private R SendAndroidMessage<R>(string pluginName, string method, params object[] parameters)
        {
#if UNITY_ANDROID
            try {
                pluginName = pluginPreName + pluginName;
                if (usdk != null) {
                    AndroidJavaObject context = usdk.CallStatic<AndroidJavaObject> ("getPlugin", pluginName);
                    if (context != null)
                        return context.Call<R> (method, parameters);
                }
            } catch (Exception ex) {
                Debug.LogError (ex.Message);
                return default (R);
            }
#endif
            return default(R);
        }

        public void CallPlugin(string pluginName, string methodName, params object[] parameters)
        {
            SendAndroidMessage(pluginName, methodName, parameters);
        }

        public R CallPlugin<R>(string pluginName, string methodName, params object[] parameters)
        {
            return SendAndroidMessage<R>(pluginName, methodName, parameters);
        }

        public bool IsExistPlugin(string pluginName)
        {
#if UNITY_ANDROID
            pluginName = pluginPreName + pluginName;
            if (usdk != null) {
                AndroidJavaObject context = usdk.CallStatic<AndroidJavaObject> ("getPlugin", pluginName);
                return context != null ? true : false;
            }
#endif
            return true;
        }

        public bool isExistField(string pluginName, string fieldName)
        {
#if UNITY_ANDROID
            pluginName = pluginPreName + pluginName;
            if (usdk != null) {
                return usdk.CallStatic<bool> ("isExistField", pluginName, fieldName);
            }
#endif
            return true;
        }

        public bool isExistMethod(string pluginName, string methodName)
        {
#if UNITY_ANDROID
            pluginName = pluginPreName + pluginName;
            if (usdk != null) {
                return usdk.CallStatic<bool> ("isExistMethod", pluginName, methodName);
            }
#endif
            return true;
        }

        public void SetCallBack(UsdkCallBackListener callback)
        {
#if UNITY_ANDROID
            if (usdk != null)
            {
                usdk.CallStatic("setCallBack", callback);
            }
#endif
        }
    }
}