using UnityEngine;
using System;
using System.Collections.Generic;

namespace Usdk
{
    public class UsdkCallBack : MonoBehaviour, IUsdkCallBack
    {
        public Action<int, List<string>> OnInitSDKCallBack;
        public Action<int, List<string>> OnExitGameCallBack;
        public Action<int, List<string>> OnLoginCallBack;
        public Action<int, List<string>> OnPayCallBack;
        public Action<int, List<string>> OnLogoutCallBack;

        public static UsdkCallBack Create()
        {
            GameObject callBackObj = new GameObject("UsdkCallBack");
            UsdkCallBack callBack = callBackObj.AddComponent<UsdkCallBack>();
            DontDestroyOnLoad(callBackObj);
            return callBack;
        }

        public void initSDKCallBack(string ret)
        {
            UsdkCallBackRetMsg retInfo = new UsdkCallBackRetMsg(ret);
            if (OnInitSDKCallBack != null)
                OnInitSDKCallBack(retInfo.errorCode, retInfo.msg);
        }

        public void exitGameCallBack(string ret)
        {
            UsdkCallBackRetMsg retInfo = new UsdkCallBackRetMsg(ret);
            if (OnExitGameCallBack != null)
                OnExitGameCallBack(retInfo.errorCode, retInfo.msg);
        }

        public void loginCallBack(string ret)
        {
            UsdkCallBackRetMsg retInfo = new UsdkCallBackRetMsg(ret);
            if (OnLoginCallBack != null)
                OnLoginCallBack(retInfo.errorCode, retInfo.msg);
        }

        public void logoutCallBack(string ret)
        {
            UsdkCallBackRetMsg retInfo = new UsdkCallBackRetMsg(ret);
            if (OnLogoutCallBack != null)
                OnLogoutCallBack(retInfo.errorCode, retInfo.msg);
        }

        public void payCallBack(string ret)
        {
            UsdkCallBackRetMsg retInfo = new UsdkCallBackRetMsg(ret);
            if (OnPayCallBack != null)
                OnPayCallBack(retInfo.errorCode, retInfo.msg);
        }
    }
}