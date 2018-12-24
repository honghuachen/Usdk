package com.usdk.plugin;

import java.io.InputStream;

import com.unity3d.player.UnityPlayer;
import com.usdk.sdk.UsdkBase;
import com.usdk.util.ReflectionUtils;

import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.drawable.BitmapDrawable;
import android.os.Bundle;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;

public class SplashProxy extends UsdkBase {
	private ImageView bgView = null;
	private UnityPlayer mUnityPlayer = null;

	@Override
	public void OnCreate(Activity activity, Bundle savedInstanceState) {
		debug("onCreate");
		mUnityPlayer = (UnityPlayer) ReflectionUtils.getFieldValue(activity,
				"mUnityPlayer");
		ShowSplash();
	}

	@Override
	public void OnDestroy() {
		HideSplash();
	}

	public void ShowSplash() {
		if (bgView != null)
			return;

		try {
			bgView = new ImageView(mActivity);		
			InputStream is = mActivity.getAssets().open("bin/Data/splash.png");
            Bitmap bitmap = BitmapFactory.decodeStream(is);
            is.close();
			BitmapDrawable bd = new BitmapDrawable(mActivity.getResources(),
					bitmap);
			bgView.setBackground(bd);

			bgView.setScaleType(ScaleType.CENTER);
			mUnityPlayer.addView(bgView, mUnityPlayer.getView().getWidth(),
					mUnityPlayer.getView().getHeight());
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public void HideSplash() {
		try {
			if (bgView == null)
				return;
			UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
				public void run() {
					mUnityPlayer.removeView(bgView);
					bgView = null;
				}
			});
		} catch (Exception e) {
			error("[onHideSplash]" + e.toString());
		}
	}
}
