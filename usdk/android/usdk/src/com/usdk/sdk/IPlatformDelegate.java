package com.usdk.sdk;

public interface IPlatformDelegate {
	public void login(String custom_params_);
	public void logout(String custom_params_);
	public void exit(String custom_params_);
	public void pay(String pay_info_);
	public void releaseSdkResource(String custom_params_);
	public void switchAccount(String custom_params_);
	public void openUserCenter(String custom_params_);
	public String getConfig(String key);
}
