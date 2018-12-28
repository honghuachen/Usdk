package com.usdk.platform.adapter;

import com.unity3d.player.UnityPlayer;

public class PlatformCallback implements IPlatformCallBack{
	private String mReceiverName = "SDKCallBack";
	
	@Override
	public void setSdkCallBackReceiver(String receiverName) {
		// TODO Auto-generated method stub
		this.mReceiverName = receiverName;
	}

	@Override
	public void initSDKCallBack(int errorCode, String paramString) {
		// TODO Auto-generated method stub
		String retMsg = "errorCode=" + errorCode + "&paramString="
				+ paramString;
		sendMessage("initSDKCallBack",retMsg);
	}

	@Override
	public void exitGameCallBack(int errorCode) {
		// TODO Auto-generated method stub
		String retMsg = "errorCode=" + errorCode;
		sendMessage("exitGameCallBack", retMsg);
	}

	@Override
	public void loginCallBack(int errorCode, String paramString) {
		// TODO Auto-generated method stub
		String retMsg = "errorCode=" + errorCode + "&paramString="
				+ paramString;
		sendMessage("loginCallBack",retMsg);
	}

	@Override
	public void payCallBack(int errorCode, String paramString) {
		// TODO Auto-generated method stub
		String retMsg = "errorCode=" + errorCode + "&paramString="
				+ paramString;
		sendMessage("payCallBack",retMsg);
	}

	@Override
	public void logoutCallBack(int errorCode, String paramString) {
		// TODO Auto-generated method stub
		String retMsg = "errorCode=" + errorCode + "&paramString="
				+ paramString;
		sendMessage("logoutCallBack",retMsg);
	}

	@Override
	public void freshQuestionnaireCallBack(String questionnaireName) {
		// TODO Auto-generated method stub
		sendMessage("freshQuestionnaireCallBack",questionnaireName);
	}

	@Override
	public void finishQuestionnaireCallBack(String giftInfo) {
		// TODO Auto-generated method stub
		sendMessage("finishQuestionnaireCallBack",giftInfo);
	}

	@Override
	public void exchangeGiftCodeFinishCallBack(int errorCode, String paramString) {
		// TODO Auto-generated method stub
		String retMsg = "errorCode=" + errorCode + "&paramString="
				+ paramString;
		sendMessage("exchangeGiftCodeFinishCallBack",retMsg);
	}
	
	@Override
	public void WeiXinShareCallback(int errorCode, String paramString) {
		// TODO Auto-generated method stub
		
		String retMsg = "errorCode=" + errorCode + "&paramString="
				+ paramString;
		sendMessage("WeiXinShareCallback",retMsg);
	}
	
	@Override
	public void PickHeadCallback(int errorCode, String paramString) {
		String retMsg = "errorCode=" + errorCode + "&paramString="
				+ paramString;
		sendMessage("TakePhotoCallback",retMsg);
	}
	
	@SuppressWarnings("unused")
	private void sendMessage(String method) {
		sendMessage(method, "");
	}

	private void sendMessage(String method, String param) {
		UnityPlayer.UnitySendMessage(this.mReceiverName, method, param);
	}
}
