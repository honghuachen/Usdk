using System;

namespace Usdk
{
    public class UsdkiOSApi : IUsdkApi
    {
        private UsdkCallBackListener _callback;
        private delegate void UsdkCallBackListener_Callback(string callbackName, string jsonMsg);
        [AOT.MonoPInvokeCallback(typeof(UsdkCallBackListener_Callback))]
        private static void UsdkCallBackListener_Method(string callbackName, string jsonMsg)
        {
            if (_callback != null)
                _callback.OnCallBack(callbackName, jsonMsg);
        }

#if UNITY_IOS
        [DllImport ("__Internal")]
        private static extern void __CallPlugin (string pluginName, string methodName, string[] parameters);
#else
        private static void __CallPlugin(string pluginName, string methodName, string[] parameters) { }
#endif

        public void CallPlugin(string pluginName, string methodName, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                parameters = new string[] { "" };
            string[] args = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                args[i] = parameters[i].ToString();
            __CallPlugin(pluginName, methodName, args);
        }

#if UNITY_IOS
        [DllImport ("__Internal")]
        private static extern string __CallPluginR (string pluginName, string methodName, string[] parameters);
#else
        private static string __CallPluginR(string pluginName, string methodName, string[] parameters)
        {
            return string.Empty;
        }
#endif

        public R CallPlugin<R>(string pluginName, string methodName, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                parameters = new string[] { "" };
            string[] args = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                args[i] = parameters[i].ToString();

            string ret = __CallPluginR(pluginName, methodName, args);
            R retR = (R)Convert.ChangeType(ret, typeof(R));
            return retR;
        }

#if UNITY_IOS
        [DllImport ("__Internal")]
        private static extern bool __IsExistPlugin (string pluginName);
#else
        private static bool __IsExistPlugin(string pluginName)
        {
            return true;
        }
#endif
        public bool IsExistPlugin(string pluginName)
        {
            return __IsExistPlugin(pluginName);
        }

#if UNITY_IOS
        [DllImport ("__Internal")]
        private static extern bool __IsExistMethod (string pluginName, string methodName);
#else
        private static bool __IsExistMethod(string pluginName, string methodName)
        {
            return true;
        }
#endif
        public bool isExistMethod(string pluginName, string methodName)
        {
            return __IsExistMethod(pluginName, methodName);
        }

        public bool isExistField(string pluginName, string fieldName)
        {
            return true;
        }

#if UNITY_IOS
        [DllImport ("__Internal")]
        private static extern void __SetCallBack (UsdkCallBackListener_Method callback);
#else
        private static void __SetCallBack(UsdkCallBackListener_Method callback)
        {
        }
#endif
        public void SetCallBack(UsdkCallBackListener callback)
        {
            this._callback = callback;
            return __SetCallBack(UsdkCallBackListener_Method);
        }
    }
}