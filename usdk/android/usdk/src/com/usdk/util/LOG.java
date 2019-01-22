package com.usdk.util;

import com.unity3d.player.UnityPlayer;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.util.Log;

public class LOG {
	public static final String logTag = "usdk";
	public static boolean open = true;
	public final void setOpen(boolean isOpen){
		open = isOpen;
	}
	
	public static void d(String log){
		if(open)
			Log.d(logTag, log);
	}
	
	public static void e(String log){
		if(open)
			Log.e(logTag, log);
	}
	
	public static void i(String log){
		if(open)
			Log.i(logTag, log);
	}
	
	public  static void ShowAlertDialog(final String title,final String content)
	{
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			public void run() {
				AlertDialog.Builder builder = new AlertDialog.Builder(UnityPlayer.currentActivity)
						.setTitle(title).setMessage(content)
						.setPositiveButton("???", new OnClickListener() {
							@Override
							public void onClick(DialogInterface dialog,
									int which) {
								// TODO Auto-generated method stub
								UnityPlayer.currentActivity.finish();
							}
						});
				builder.setCancelable(false);
				builder.show();
			}
		});	
	}
}
