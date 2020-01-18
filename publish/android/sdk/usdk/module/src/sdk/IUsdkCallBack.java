package com.usdk.sdk;

public interface IUsdkCallBack {
	public void sendCallBack2Unity(UsdkCallBackErrorCode errorCode);
	public void sendCallBack2Unity(UsdkCallBackErrorCode errorCode, String paramString);
	
	public enum UsdkCallBackErrorCode
	{
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
}