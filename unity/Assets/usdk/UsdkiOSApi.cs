using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Usdk
{
    public class IosSdkAPI : IUsdk
    {
        private static IosSdkAPI _instance = null;
        public static IosSdkAPI instance
        {
            get
            {
                if (_instance == null)
                    _instance = new IosSdkAPI();
                return _instance;
            }
        }

        [DllImport("__Internal")]
        private static extern void SDKSetSdkCallBackReceiver(string receiver_name_);
        public void setSdkCallBackReceiver(string receiver_name_)
        {
            SDKSetSdkCallBackReceiver(receiver_name_);
        }

        [DllImport("__Internal")]
        private static extern void SDKLogin(string custom_params_);
        public void login(string custom_params_)
        {
            SDKLogin(custom_params_);
        }

        [DllImport("__Internal")]
        private static extern void SDKLogout(string custom_params_);
        public void logout(string custom_params_)
        {
            SDKLogout(custom_params_);
        }

        [DllImport("__Internal")]
        private static extern void SDKOpenUserCenter(string custom_params_);
        public void openUserCenter(string custom_params_)
        {
            SDKOpenUserCenter(custom_params_);
        }

        [DllImport("__Internal")]
        private static extern void SDKExitGame(string custom_params_);
        public void exit(string custom_params_)
        {
            SDKExitGame(custom_params_);
        }

        [DllImport("__Internal")]
        private static extern void SDKPayStart(string product_id, int amount);
        public void payStart(string product_id, int amount)
        {
            SDKPayStart(product_id, amount);
        }

        [DllImport("__Internal")]
        private static extern void SDKPay(string pay_info);
        public void pay(SdkPayInfo pay_info_)
        {
            SDKPay(pay_info_.ToString());
        }

        [DllImport("__Internal")]
        private static extern void SDKReleaseSdkResource(string custom_params_);
        public void releaseSdkResource(string custom_params_)
        {
            SDKReleaseSdkResource(custom_params_);
        }

        [DllImport("__Internal")]
        private static extern void SDKSwitchAccount(string custom_params_);
        public void switchAccount(string custom_params_)
        {
            SDKSwitchAccount(custom_params_);
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
    }
}