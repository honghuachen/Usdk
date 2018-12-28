package com.usdk.platform.adapter;

import java.util.ArrayList;
import java.util.List;

import android.app.Activity;
import android.content.ActivityNotFoundException;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import com.usdk.sdk.UsdkBase;

public class PlatformProxyBase extends UsdkBase implements IPlatformProxyBase {
	protected PlatformCallback platformCallBack = null;
	protected List<String> mTokens = new ArrayList<String>();
	
	public PlatformProxyBase() {
		platformCallBack = new PlatformCallback();
	}
	
	@Override
	public void OnCreate(Activity activity, Bundle savedInstanceState) {
		this.parseXmlConfig("usdkconfig.xml");
	}

	@Override
	public void OnDestroy() {
		this.releaseSdkResource("");
	}

	@Override
	public void setSdkCallBackReceiver(String receiverName) {
		platformCallBack.setSdkCallBackReceiver(receiverName);
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

	@Override
	public void openAppstoreComment(String appid) {
		if(!appid.isEmpty())
			this.packageName = appid;

        Uri uri = Uri.parse("market://details?id=" + this.packageName);
        Intent goToMarket = new Intent(Intent.ACTION_VIEW, uri);

        goToMarket.addFlags(Intent.FLAG_ACTIVITY_NO_HISTORY |
                Intent.FLAG_ACTIVITY_MULTIPLE_TASK);
        try {
        	this.mActivity.startActivity(goToMarket);
        } catch (ActivityNotFoundException e) {
        	this.mActivity.startActivity(new Intent(Intent.ACTION_VIEW,
                    Uri.parse("http://play.google.com/store/apps/details?id=" + this.packageName)));
        }
	}
}