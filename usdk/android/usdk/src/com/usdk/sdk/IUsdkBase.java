package com.usdk.sdk;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.view.KeyEvent;

public interface IUsdkBase {
	public void onCreate(Activity activity, Bundle savedInstanceState);
	public void onStart();
	public void onDestroy();
	public void onStop() ;
	public void onResume();
	public void onPause();
	public void onRestart();
	public void onActivityResult(int requestCode, int resultCode, Intent data);
	public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults);
	public void onNewIntent(Intent intent);
	public void onConfigurationChanged(Configuration arg0);
	public void onSaveInstanceState(Bundle outState);
	public void onBackPressed();
	public void finish();
	public boolean onKeyDown(int keyCode, KeyEvent event);
	public boolean onKeyUp(int keyCode, KeyEvent event);
}
