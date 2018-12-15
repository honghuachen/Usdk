using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usdk
{
    public interface IUsdkCallBack
    {
        void initSDKCallBack(string ret);
        void exitGameCallBack(string ret);
        void loginCallBack(string ret);
        void logoutCallBack(string ret);
        void payCallBack(string ret);
        void exchangeGiftCodeCallBack(string ret);
    }

    public enum UsdkCallBackErrorCode
    {
        InitSuccess = 0,
        InitFail,
        LoginSuccess,
        LoginCancel,
        LoginFail,
        LogoutFinish,
        ExitNoChannelExiter,
        ExitSuccess,
        PaySuccess,
        PayCancel,
        PayFail,
        PayProgress,
        PayOthers,
        ExchangeGiftCodeFinish,
        WeiXinShareSucess,
        WeiXinShareFail,
    }

    public class UsdkCallBackRetMsg
    {
        public int errorCode;
        public List<string> msg;

        public UsdkCallBackRetMsg(string ret)
        {
            msg = new List<string>();
            string[] retInfo = ret.Split('&');
            for (int i = 0; i < retInfo.Length; i++)
            {
                string[] subMsgs = retInfo[i].Split('=');
                if (subMsgs[0] == "errorCode")
                    errorCode = Convert.ToInt32(subMsgs[1]);
                else
                    msg.Add(subMsgs[1]);
            }
        }
    }
}
