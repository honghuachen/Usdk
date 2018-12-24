package vn.soha.game.sdk.dialogs;

import vn.sgame.sdk.R;
import vn.sgame.sdk.view.WarningButton;
import vn.soha.game.sdk.SohaSDK;
import vn.soha.game.sdk.utils.AnimationUtils;
import vn.soha.game.sdk.utils.Logger;
import vn.soha.game.sdk.utils.Utils;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnDismissListener;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.TextView;

/**
 * @since October 26, 2015
 * @author Sohagame SDK Team
 * 
 */
@SuppressLint("SetJavaScriptEnabled")
public class DialogProfile implements OnClickListener {

	private Activity mActivity;
	private Dialog mDialog;
	private WarningButton mWaringButton;
	public static Boolean isShowWarningDetail =false;
	WebView wv;
	LinearLayout lnErr;
	private IntentFilter mIntentFilter = new IntentFilter(
			"android.net.conn.CONNECTIVITY_CHANGE");
	String url="";
	public DialogProfile(Activity activity, WarningButton warningButton) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		mWaringButton = warningButton;
		vInit();
		SohaSDK.registerBroadCast(mBroadcastReceiver, mIntentFilter);
	}
	
	private BroadcastReceiver mBroadcastReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			ConnectivityManager connectivityManager = (ConnectivityManager) context
					.getSystemService(Context.CONNECTIVITY_SERVICE);
			NetworkInfo activeNetworkInfo = connectivityManager
					.getActiveNetworkInfo();
			if (activeNetworkInfo == null) {
				
				lnErr.setVisibility(View.VISIBLE);
				// imgBack.setVisibility(View.GONE);
			} else {
				// mDialog.findViewById(R.id.rlCheckInternet).setVisibility(View.GONE);
				// wv.reload();
				wv.reload();
			
				lnErr.setVisibility(View.GONE);
				
			}
		}
	};

	public void vInit() {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_profile);
		mDialog.setCancelable(true);
		mDialog.setOnDismissListener(new OnDismissListener() {

			@Override
			public void onDismiss(DialogInterface dialog) {
				// TODO Auto-generated method stub
				if (mWaringButton != null) {
					isShowWarningDetail =false;
					mWaringButton.dialogProfile =null;
					mWaringButton.show();
					mWaringButton.vSetAlphaCryStal();
				}
			}
		});
		mDialog.show();
		isShowWarningDetail =true;

		TextView tvHeaderTitle = (TextView) mDialog.findViewById(R.id.tvHeaderTitle);
		tvHeaderTitle.setText(Utils.getUserName(mActivity));
		ImageButton imgBack = (ImageButton) mDialog.findViewById(R.id.ibHeaderLeft);
		imgBack.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				if(mDialog!=null)
				{
					mDialog.dismiss();
					
				}
			}
		});
		
		
		mDialog.findViewById(R.id.ibHeaderLeft).setOnClickListener(this);
		lnErr = (LinearLayout) mDialog.findViewById(R.id.lnErr);
		wv = (WebView) mDialog.findViewById(R.id.wv);
		wv.getSettings().setJavaScriptEnabled(true);
		wv.setHorizontalScrollBarEnabled(false);
		wv.setVerticalScrollBarEnabled(false);
		wv.setWebViewClient(new WebViewClient() {			
			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				// TODO Auto-generated method stub
				super.onPageStarted(view, url, favicon);
				mDialog.findViewById(R.id.rl).setVisibility(View.VISIBLE);
				Logger.e(DialogProfile.class.getSimpleName() + ": " + url);
			}

			@Override
			public void onPageFinished(WebView view, String url) {
				// TODO Auto-generated method stub
				super.onPageFinished(view, url);
				mDialog.findViewById(R.id.rl).setVisibility(View.GONE);
			}
		});
		
		ConnectivityManager connectivityManager = (ConnectivityManager) mActivity
				.getSystemService(Context.CONNECTIVITY_SERVICE);
		NetworkInfo activeNetworkInfo = connectivityManager
				.getActiveNetworkInfo();
		if (activeNetworkInfo == null) {
			
			lnErr.setVisibility(View.VISIBLE);
			// imgBack.setVisibility(View.GONE);
		}
		url = Utils.getWarningURL(mActivity);
		Log.e("Warningurl", "___"+url + "?app_id="
				+ Utils.getAppId(mActivity)+Utils.createDefaultSOAPParams(mActivity));
		wv.loadUrl(url + "?app_id="
				+ Utils.getAppId(mActivity)+Utils.createDefaultSOAPParams(mActivity));
	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		AnimationUtils.vAnimationClick(v);
		if (v.getId() == R.id.ibHeaderLeft) {
			mDialog.dismiss();
		}
	}


}
