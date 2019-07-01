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
import com.usdk.sdk.Usdk;

public class UsdkMainActivity extends UnityPlayerActivity{

	public UnityPlayer getUnityPlayer()
	{
		return mUnityPlayer;
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		Usdk.onCreate(UsdkMainActivity.this, savedInstanceState);
		getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
	}

	@Override
	protected void onStart() {
		// TODO Auto-generated method stub
		super.onStart();
		Usdk.onStart();
	}

	@Override
	public void onDestroy() {
		super.onDestroy();
		Usdk.onDestroy();
	}

	@Override
	public void onRestart() {
		super.onRestart();
		Usdk.onRestart();
	}

	@Override
	public void onStop() {
		super.onStop();
		Usdk.onStop();
	}

	@Override
	public void onResume() {
		super.onResume();
		Usdk.onResume();
	}

	@Override
	public void onPause() {
		super.onPause();
		Usdk.onPause();
	}

	@Override
	public void onActivityResult(int requestCode, int resultCode, Intent intent) {
		super.onActivityResult(requestCode, resultCode, intent);
		Usdk.onActivityResult(requestCode, resultCode, intent);
	}

	@SuppressLint("Override")
	@TargetApi(23)
    //@Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
//		if(Build.VERSION.SDK_INT >= 23)
//		{
//			super.onRequestPermissionsResult(requestCode, permissions, grantResults);
//		}
		Usdk.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }

	@Override
	public void onNewIntent(Intent intent) {
		super.onNewIntent(intent);
		Usdk.onNewIntent(intent);
	}

	@Override
	public void onConfigurationChanged(Configuration arg0) {
		// TODO Auto-generated method stub
		super.onConfigurationChanged(arg0);
		Usdk.onConfigurationChanged(arg0);
	}

	@Override
	protected void onSaveInstanceState(Bundle outState) {
		// TODO Auto-generated method stub
		super.onSaveInstanceState(outState);
		Usdk.onSaveInstanceState(outState);
	}

	@Override
	public void onBackPressed() {
		// TODO Auto-generated method stub
		super.onBackPressed();
		Usdk.onBackPressed();
	}

	@Override
	public void finish() {
		// TODO Auto-generated method stub
		Usdk.finish();
		super.finish();
	}

	@Override
	public boolean onKeyDown(int arg0, KeyEvent arg1) {
		// TODO Auto-generated method stub
		return Usdk.onKeyDown(arg0, arg1);
		
	}

	@Override
	public boolean onKeyUp(int arg0, KeyEvent arg1) {
		// TODO Auto-generated method stub
		return Usdk.onKeyUp(arg0, arg1);
	}
	
	
}
