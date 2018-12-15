package com.usdk.main;

import android.annotation.SuppressLint;
import android.annotation.TargetApi;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.WindowManager;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import com.usdk.sdk.UsdkFactory;

public class UsdkMainActivity extends UnityPlayerActivity{

	public UnityPlayer getUnityPlayer()
	{
		return mUnityPlayer;
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		UsdkFactory.onCreate(UsdkMainActivity.this, savedInstanceState);
		getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
	}

	@Override
	protected void onStart() {
		// TODO Auto-generated method stub
		super.onStart();
		UsdkFactory.onStart();
	}

	@Override
	public void onDestroy() {
		super.onDestroy();
		UsdkFactory.onDestroy();
	}

	@Override
	public void onRestart() {
		super.onRestart();
		UsdkFactory.onRestart();
	}

	@Override
	public void onStop() {
		super.onStop();
		UsdkFactory.onStop();
	}

	@Override
	public void onResume() {
		super.onResume();
		UsdkFactory.onResume();
	}

	@Override
	public void onPause() {
		super.onPause();
		UsdkFactory.onPause();
	}

	@Override
	public void onActivityResult(int requestCode, int resultCode, Intent intent) {
		super.onActivityResult(requestCode, resultCode, intent);
		UsdkFactory.onActivityResult(requestCode, resultCode, intent);
	}

	@SuppressLint("Override")
	@TargetApi(23)
    //@Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
//		if(Build.VERSION.SDK_INT >= 23)
//		{
//			super.onRequestPermissionsResult(requestCode, permissions, grantResults);
//		}
		UsdkFactory.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }

	@Override
	public void onNewIntent(Intent intent) {
		super.onNewIntent(intent);
		UsdkFactory.onNewIntent(intent);
	}

	@Override
	public void onConfigurationChanged(Configuration arg0) {
		// TODO Auto-generated method stub
		super.onConfigurationChanged(arg0);
		UsdkFactory.onConfigurationChanged(arg0);
	}

	@Override
	protected void onSaveInstanceState(Bundle outState) {
		// TODO Auto-generated method stub
		super.onSaveInstanceState(outState);
		UsdkFactory.onSaveInstanceState(outState);
	}

	@Override
	public void onBackPressed() {
		// TODO Auto-generated method stub
		super.onBackPressed();
		UsdkFactory.onBackPressed();
	}

	@Override
	public void finish() {
		// TODO Auto-generated method stub
		UsdkFactory.finish();
		super.finish();
	}

	@Override
	public boolean onKeyDown(int arg0, KeyEvent arg1) {
		// TODO Auto-generated method stub
		return UsdkFactory.onKeyDown(arg0, arg1);
		
	}

	@Override
	public boolean onKeyUp(int arg0, KeyEvent arg1) {
		// TODO Auto-generated method stub
		return UsdkFactory.onKeyUp(arg0, arg1);
	}
	
	
}
