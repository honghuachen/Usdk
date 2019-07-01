package com.usdk.platform.adapter;

import java.util.ArrayList;
import java.util.List;

import android.app.Activity;
import android.os.Bundle;

import com.usdk.sdk.IPlatformDelegate;
import com.usdk.sdk.UsdkBase;

public class PlatformProxyBase extends UsdkBase implements IPlatformDelegate {
	protected List<String> mTokens = new ArrayList<String>();
	
	@Override
	public void OnCreate(Activity activity, Bundle savedInstanceState) {
		this.parseXmlConfig("usdkconfig.xml");
	}

	@Override
	public void OnDestroy() {
		this.releaseSdkResource("");
	}

	@Override
	public void login(String custom_params_) {
	}

	@Override
	public void logout(String custom_params_) {
	}

	@Override
	public void exit(String custom_params_) {
		releaseSdkResource("");
		mActivity.finish();
	}

	@Override
	public void pay(String pay_info_) {
	}

	@Override
	public void releaseSdkResource(String custom_params_) {
	}

	@Override
	public void switchAccount(String custom_params_) {
	}

	@Override
	public void openUserCenter(String custom_params_) {
	}
}