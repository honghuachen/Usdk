package com.usdk.plugin;

import com.usdk.sdk.UsdkBase;

import android.app.Activity;
import android.os.Bundle;

public class UUidPlugin extends UsdkBase{
	DeviceUuidFactory uuidKit;
	@Override
	protected void OnCreate(Activity activity, Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.OnCreate(activity, savedInstanceState);
		uuidKit = new DeviceUuidFactory(activity);
	}
	
	public String getUuid() {
		return uuidKit.getUuid().toString();
	}

}
