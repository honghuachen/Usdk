package vn.soha.game.sdk.dialogs;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.utils.AnimationUtils;
import vn.soha.game.sdk.utils.Logger;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.webkit.WebView;
import android.webkit.WebViewClient;

/**
 * @since April 08, 2015
 * @author Sohagame SDK Team
 * 
 */
@SuppressLint("SetJavaScriptEnabled")
public class DialogNotifyUpdate implements OnClickListener {

	private Activity mActivity;
	private String mURL;
	private Dialog mDialog;
	private boolean enableClose;

	public DialogNotifyUpdate(Activity activity, String URL, boolean isEnableClose) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		mURL = URL;
		enableClose = isEnableClose;
		vInit();
	}

	public void vInit() {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_notify_update);
		mDialog.setCancelable(enableClose);
		mDialog.show();

		final WebView wv = (WebView) mDialog.findViewById(R.id.wv);
		wv.getSettings().setJavaScriptEnabled(true);
		wv.setHorizontalScrollBarEnabled(false);
		wv.setVerticalScrollBarEnabled(false);
		wv.setWebViewClient(new WebViewClient() {			
			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				// TODO Auto-generated method stub
				super.onPageStarted(view, url, favicon);
				mDialog.findViewById(R.id.rl).setVisibility(View.VISIBLE);
				// mDialog.findViewById(R.id.rl).startAnimation(AnimationUtils.loadAnimation(mActivity, android.R.anim.fade_in));
				Logger.e(DialogNotifyUpdate.class.getSimpleName() + ": " + url);
				if (url.contains("openbrowser=1")) {
					Intent intent = new Intent(Intent.ACTION_VIEW, 
							Uri.parse(url));
					mActivity.startActivity(intent);
				} 
			}

			@Override
			public void onPageFinished(WebView view, String url) {
				// TODO Auto-generated method stub
				super.onPageFinished(view, url);
				// mDialog.findViewById(R.id.rl).startAnimation(AnimationUtils.loadAnimation(mActivity, android.R.anim.fade_out));
				mDialog.findViewById(R.id.rl).setVisibility(View.GONE);
				if (url.contains("openbrowser=1")) {
					wv.goBack();
				}
			}
		});
		wv.loadUrl(mURL);
	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		AnimationUtils.vAnimationClick(v);

	}


}
