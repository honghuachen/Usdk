using System;
using System.Collections.Generic;
using UnityEngine;

namespace Usdk {
    public enum UsdkCallBackErrorCode {
	    InitSuccess,
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
	    PayOthers
    }

    public class UsdkCallBackRetMsg {
        public int errorCode;
        public List<string> msg;

        public UsdkCallBackRetMsg (string ret) {
            msg = new List<string> ();
            string[] retInfo = ret.Split ('&');
            for (int i = 0; i < retInfo.Length; i++) {
                string[] subMsgs = retInfo[i].Split ('=');
                if (subMsgs[0] == "errorCode")
                    errorCode = Convert.ToInt32(subMsgs[1]);
                else
                    msg.Add (subMsgs[1]);
            }
        }
    }

    public class UsdkCallBack : MonoBehaviour {
        public Action<int, List<string>> OnCallBack;

        public static UsdkCallBack Create () {
            GameObject callBackObj = new GameObject ("UsdkCallBack");
            UsdkCallBack callBack = callBackObj.AddComponent<UsdkCallBack> ();
            DontDestroyOnLoad (callBackObj);
            return callBack;
        }

        public void CallBack (string ret) {
            UsdkCallBackRetMsg retInfo = new UsdkCallBackRetMsg (ret);
            if(OnCallBack != null) 
                OnCallBack(retInfo.errorCode,retInfo.msg);
        }
    }
}