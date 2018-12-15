using System.Runtime.InteropServices;
using System;

namespace Usdk
{
    public class UsdkiOSApi : IUsdkApi
    {
        [DllImport("__Internal")]
        private static extern void SDKSetSdkCallBackReceiver(string receiverName);
        public void setSdkCallBackReceiver(string receiverName)
        {
            if (string.IsNullOrEmpty(receiverName))
                return;
            SDKSetSdkCallBackReceiver(receiverName);
        }

        [DllImport("__Internal")]
        private static extern void SDKLogin(string arg);
        public void login(string arg = "")
        {
            SDKLogin(arg);
        }

        [DllImport("__Internal")]
        private static extern void SDKLogout(string arg);
        public void logout(string arg = "")
        {
            SDKLogout(arg);
        }

        [DllImport("__Internal")]
        private static extern void SDKOpenUserCenter(string arg);
        public void openUserCenter(string arg = "")
        {
            SDKOpenUserCenter(arg);
        }

        [DllImport("__Internal")]
        private static extern void SDKExitGame(string arg);
        public void exit(string arg = "")
        {
            SDKExitGame(arg);
        }

        [DllImport("__Internal")]
        private static extern void SDKPay(string payInfo);
        public void pay(SdkPayInfo payInfo)
        {
            SDKPay(payInfo.ToString());
        }

        [DllImport("__Internal")]
        private static extern void SDKReleaseSdkResource(string arg);
        public void releaseSdkResource(string arg = "")
        {
            SDKReleaseSdkResource(arg);
        }

        [DllImport("__Internal")]
        private static extern void SDKSwitchAccount(string arg);
        public void switchAccount(string arg = "")
        {
            SDKSwitchAccount(arg);
        }

        [DllImport("__Internal")]
        private static extern void SDKOpenAppstoreComment(string appid);
        //appid app在GP 或 App Store中的AppID标识
        public void openAppstoreComment(string appid)
        {
            SDKOpenAppstoreComment(appid);
        }

        [DllImport("__Internal")]
        private static extern string SDKGetConfig(string key);
        public string getConfig(string key)
        {
            return SDKGetConfig(key);
        }

        [DllImport("__Internal")]
        private static extern void SDKCallPugin(string pluginName, string methodName, params object[] parameters);
        public void callPugin(string pluginName, string methodName, params object[] parameters)
        {
            SDKCallPugin(pluginName, methodName, parameters);
        }

        [DllImport("__Internal")]
        private static extern string SDKCallPuginR(string pluginName, string methodName, params object[] parameters);
        public string callPuginR(string pluginName, string methodName, params object[] parameters)
        {
            return SDKCallPuginR(pluginName, methodName, parameters);
        }
    }
}