package vn.soha.game.sdk.dialogs;
/*package vn.sgame.sdk.dialogs;


import vn.sgame.sdk.R;
import vn.sgame.sdk.SGameSDK;
import vn.sgame.sdk.utils.AnimationUtils;
import vn.sgame.sdk.utils.Logger;
import vn.sgame.sdk.utils.Utils;
import vn.sgame.sdk.view.WarningButton;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnDismissListener;
import android.graphics.Bitmap;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.TextView;

*//**
 * @since October 26, 2015
 * @author Sohagame SDK Team
 * 
 *//*
@SuppressLint("SetJavaScriptEnabled")
public class DialogProfile implements OnClickListener {

	private Activity mActivity;
	private Dialog mDialog;
	private WarningButton mWaringButton;

	public DialogProfile(Activity activity, WarningButton warningButton) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		mWaringButton = warningButton;
		vInit();
	}

	public void vInit() {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_webview);
		mDialog.setCancelable(true);
		mDialog.setOnDismissListener(new OnDismissListener() {

			@Override
			public void onDismiss(DialogInterface dialog) {
				// TODO Auto-generated method stub
				if (mWaringButton != null) {
					mWaringButton.show();
					mWaringButton.vSetAlphaCryStal();
				}
				SGameSDK.isShowProfile = false;
			}
		});
		mDialog.show();
		SGameSDK.isShowProfile = true;

		TextView tvHeaderTitle = (TextView) mDialog.findViewById(R.id.tvHeaderTitle);
		tvHeaderTitle.setText(Utils.getUserName(mActivity));
		mDialog.findViewById(R.id.ibHeaderLeft).setOnClickListener(this);

		WebView wv = (WebView) mDialog.findViewById(R.id.wv);
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
		wv.loadUrl("http://m.mysoha.vn/" + Utils.getUserName(mActivity));
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
*/