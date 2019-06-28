using System.Runtime.InteropServices;
using System;

namespace Usdk
{
    public class UsdkiOSApi : IUsdkApi
    {
        // public const string PLATFORM_NAME = "PlatformProxy";
        
        // // [DllImport("__Internal")]
        // // private static extern void SDKSetSdkCallBackReceiver(string receiverName);
        // public void setSdkCallBackReceiver(string receiverName)
        // {
        //     if (string.IsNullOrEmpty(receiverName))
        //         return;
        //     // SDKSetSdkCallBackReceiver(receiverName);
        //     callPugin(PLATFORM_NAME,"setSdkCallBackReceiver",receiverName);
        // }

        // // [DllImport("__Internal")]
        // // private static extern void SDKLogin(string arg);
        // public void login(string arg)
        // {
        //     // SDKLogin(arg);
        //     callPugin(PLATFORM_NAME,"login",arg);
        // }

        // // [DllImport("__Internal")]
        // // private static extern void SDKLogout(string arg);
        // public void logout(string arg)
        // {
        //     // SDKLogout(arg);
        //     callPugin(PLATFORM_NAME,"logout",arg);
        // }

        // // [DllImport("__Internal")]
        // // private static extern void SDKOpenUserCenter(string arg);
        // public void openUserCenter(string arg)
        // {
        //     // SDKOpenUserCenter(arg);
        //     callPugin(PLATFORM_NAME,"openUserCenter",arg);
        // }

        // // [DllImport("__Internal")]
        // // private static extern void SDKExitGame(string arg);
        // public void exit(string arg)
        // {
        //     // SDKExitGame(arg);
        //     callPugin(PLATFORM_NAME,"exitGame",arg);
        // }

        // // [DllImport("__Internal")]
        // // private static extern void SDKPay(string payInfo);
        // public void pay(SdkPayInfo payInfo)
        // {
        //     // SDKPay(payInfo.ToString());
        //     callPugin(PLATFORM_NAME,"pay",payInfo.ToString());
        // }

        // // [DllImport("__Internal")]
        // // private static extern void SDKReleaseSdkResource(string arg);
        // public void releaseSdkResource(string arg)
        // {
        //     // SDKReleaseSdkResource(arg);
        //     callPugin(PLATFORM_NAME,"releaseSdkResource",arg);
        // }

        // // [DllImport("__Internal")]
        // // private static extern void SDKSwitchAccount(string arg);
        // public void switchAccount(string arg)
        // {
        //     // SDKSwitchAccount(arg);
        //     callPugin(PLATFORM_NAME,"switchAccount",arg);
        // }

        // // [DllImport("__Internal")]
        // // private static extern void SDKOpenAppstoreComment(string appid);
        // //appid app在GP 或 App Store中的AppID标识
        // public void openAppstoreComment(string appid)
        // {
        //     // SDKOpenAppstoreComment(appid);
        //     callPugin(PLATFORM_NAME,"openAppstoreComment",appid);
        // }

        // // [DllImport("__Internal")]
        // // private static extern string SDKGetConfig(string key);
        // public string getConfig(string key)
        // {
        //     return callPuginR(PLATFORM_NAME,"getConfig",key);
        // }

        [DllImport("__Internal")]
        private static extern void __CallPlugin(string pluginName, string methodName, params string[] parameters);
        public void CallPlugin(string pluginName, string methodName, params string[] parameters)
        {
            __CallPlugin(pluginName, methodName, parameters);
        }

        [DllImport("__Internal")]
        private static extern string __CallPluginR(string pluginName, string methodName, params string[] parameters);
        public R CallPlugin<R>(string pluginName, string methodName, params string[] parameters)
        {
            string ret = __CallPluginR(pluginName, methodName, parameters);
            R retR = (R)Convert.ChangeType(ret,typeof(R));
            return retR;
        }
    }
}