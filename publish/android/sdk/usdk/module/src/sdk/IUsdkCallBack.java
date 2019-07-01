package com.usdk.sdk;

public interface IUsdkCallBack {
	public void sendCallBack2Unity(ErrorCode errorCode);
	public void sendCallBack2Unity(ErrorCode errorCode, String paramString);
	
	public enum ErrorCode
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
		PayOthers,	
	}
}