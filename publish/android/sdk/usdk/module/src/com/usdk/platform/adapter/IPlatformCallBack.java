package com.usdk.platform.adapter;

public interface IPlatformCallBack {
    public abstract void setSdkCallBackReceiver(String receiverName);
	public abstract void initSDKCallBack(int errorCode, String paramString);
	public abstract void exitGameCallBack(int errorCode);
	public abstract void loginCallBack(int errorCode, String paramString);
	public abstract void payCallBack(int errorCode, String paramString);
	public abstract void logoutCallBack(int errorCode,String paramString);
	public abstract void freshQuestionnaireCallBack(String questionnaireName);
	public abstract void finishQuestionnaireCallBack(String giftInfo);
	public abstract void exchangeGiftCodeFinishCallBack(int errorCode,String paramString);
	public abstract void WeiXinShareCallback(int errorCode,String paramString);
	public abstract void PickHeadCallback(int errorCode,String paramString);
	
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
		ExchangeGiftCodeFinish,
		WeiXinShareSucess,
		WeiXinShareFail,
		PickHeadFinish,
	}
}