using System;
using UnityEngine;

namespace Usdk
{
    public class UsdkAndroidApi : IUsdkApi
    {
        const string platfromName = "PlatformProxy";
        const string pluginPreName = "com.usdk.plugin.";
#if UNITY_ANDROID
        private static AndroidJavaClass usdkFactory
        {
            get
            {
                try
                {
                    AndroidJavaClass factory = new AndroidJavaClass("com.usdk.sdk.UsdkFactory");
                    return factory;
                }
                catch(Exception ex){
                    return null;
                }
            }
        }
#endif

        private void SendAndroidMessage(string pluginName, string method, params object[] parameters)
        {
#if UNITY_ANDROID
            try
            {
                pluginName = pluginPreName + pluginName;
		        if(factory != null){
                    AndroidJavaObject context = factory.CallStatic<AndroidJavaObject>("getPlugin",pluginName);
                    if(context != null)
                        context.Call(method, parameters);
                }
            }
            catch(Exception ex){}
#endif
        }

        private R SendAndroidMessage<R>(string pluginName,string method, params object[] parameters)
        {
#if UNITY_ANDROID
		    try
            {
                pluginName = pluginPreName + pluginName;
		        if(factory != null){
                    AndroidJavaObject context = factory.CallStatic<AndroidJavaObject>("getPlugin",pluginName);
                    if(context != null)
                        return context.Call<T>(method, parameters);
                }
            }
            catch(Exception ex){
                return default(T);
            }
#endif
            return default(R);
        }

        public void setSdkCallBackReceiver(string receiverName)
        {
            if (string.IsNullOrEmpty(receiverName))
                return;
            SendAndroidMessage(platfromName, "setCallBackReceiver", receiverName);
        }

        public void login(string arg = "")
        {
            SendAndroidMessage(platfromName,"login", arg);
        }

        public void logout(string arg = "")
        {
            SendAndroidMessage(platfromName,"logout", arg);
        }

        public void openUserCenter(string arg = "")
        {
            SendAndroidMessage(platfromName,"openUserCenter", arg);
        }

        public void exit(string arg = "")
        {
            SendAndroidMessage(platfromName,"exit", arg);
        }

        public void pay(SdkPayInfo payInfo)
        {
            SendAndroidMessage(platfromName,"pay", payInfo.ToString());
        }

        public void releaseSdkResource(string arg = "")
        {
            SendAndroidMessage(platfromName,"releaseSdkResource", arg);
        }

        public void switchAccount(string arg = "")
        {
            SendAndroidMessage(platfromName,"switchAccount", arg);
        }

        //appid app在GP 或 App Store中的AppID标识
        public void openAppstoreComment(string appid)
        {
            SendAndroidMessage(platfromName,"openAppstoreComment", appid);
        }

        public string getConfig(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;
            return SendAndroidMessage<string>(platfromName,"getConfig", key);
        }

        public void callPugin(string pluginName, string methodName, params object[] parameters)
        {
           SendAndroidMessage<string>(pluginName, methodName, parameters);
        }

        public string callPuginR(string pluginName, string methodName, params object[] parameters)
        {
            return SendAndroidMessage<string>(pluginName, methodName, parameters);
        }
    }
}