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

        static UsdkApi(){
            SetCallBack(new UsdkCallBackListener(new UsdkCallBack()));
        }

        #region sdk api
        public static void SetCallBack(UsdkCallBackListener callBack)
        {
            api.SetCallBack(callBack);
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
    }
}