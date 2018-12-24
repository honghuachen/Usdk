package vn.soha.game.sdk.dialogs;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.SohaSDK;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.Uri;
import android.provider.Settings;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager.LayoutParams;
import android.webkit.WebView;
import android.widget.TextView;

/**
 * @since January 27, 2015
 * @author Hoang Cao Dev
 *
 */
@SuppressLint({ "JavascriptInterface", "SetJavaScriptEnabled", "InflateParams" }) public class DialogPermisionExplain {

	private static Activity mActivity;
	public static Dialog mDialog;


	private WebView wv;
	private IntentFilter mIntentFilter = new IntentFilter("android.net.conn.CONNECTIVITY_CHANGE");
	String mPageErrorHtml;
	String mFailingUrl;

	TextView tvCancel,tvSetting;

	public DialogPermisionExplain(Activity activity) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		try {
			if(mDialog!=null)
			{
				mDialog.dismiss();
			}
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}
		

		vInitUI();
		
	}


	public void vInitUI() {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.getWindow().setSoftInputMode(LayoutParams.SOFT_INPUT_STATE_HIDDEN);
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_permission_explain);
		mDialog.setCancelable(true);
		tvCancel = (TextView) mDialog.findViewById(R.id.tvCancel);
		tvSetting = (TextView) mDialog.findViewById(R.id.tvSetting);
		tvSetting.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				mDialog.dismiss();
			
				Intent intent = new Intent();
				intent.setAction(Settings.ACTION_APPLICATION_DETAILS_SETTINGS);
				Uri uri = Uri.fromParts("package", mActivity.getPackageName(), null);
				intent.setData(uri);
				mActivity.startActivity(intent);
			
			}
		});
		
		
		tvCancel.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
			mDialog.dismiss();
			
			
			}
		});
		
		try {
			mDialog.show();
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}
	
		
		
	}

	public interface InteractConnectDialog
	{
		public void onCancel();
	}
	
}
