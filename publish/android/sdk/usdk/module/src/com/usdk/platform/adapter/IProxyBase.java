package com.usdk.platform.adapter;

public interface IProxyBase {
	public void setSdkCallBackReceiver(String receiverName);
	public void login(String custom_params_);
	public void logout(String custom_params_);
	public void exit(String custom_params_);
	public void pay(String pay_info_);
	public void releaseSdkResource(String custom_params_);
	public void switchAccount(String custom_params_);
	public void openUserCenter(String custom_params_);
	public void openAppstoreComment(String appid);
	public String getConfig(String key);
}
