package vn.soha.game.sdk.dialogs;

import org.json.JSONException;
import org.json.JSONObject;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.SohaSDK;
import vn.soha.game.sdk.utils.AnimationUtils;
import vn.soha.game.sdk.utils.ScreenUtils;
import vn.soha.game.sdk.utils.TextUtils;
import vn.soha.game.sdk.utils.Utils;
import android.app.Activity;
import android.app.Dialog;
import android.content.Intent;
import android.net.Uri;
import android.os.Handler;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager.LayoutParams;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

/**
 * @since January 27, 2015
 * @author Sohagame SDK Team
 *
 */
public class DialogNotice implements OnClickListener {

	private Activity mActivity;
	private Dialog mDialog;
	
	private OnFuckListener onFuckListener;
	Boolean isPendingLogout = false;
	public static Boolean isPendingRetry = false;
	ProgressBar prgLoadding;
	RelativeLayout rlParent;
	int time=0;
	
	public interface OnFuckListener {
		public void onFuck();
	}

	public DialogNotice(Activity activity, JSONObject jsonObject, OnFuckListener onFuckListener) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		this.onFuckListener = onFuckListener;
		vInitUI(jsonObject);
	}

Handler h = new Handler();
	public void vInitUI(JSONObject jsonObject) {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.getWindow().setSoftInputMode(LayoutParams.SOFT_INPUT_STATE_HIDDEN);
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_notice);
		mDialog.setCancelable(false);
		mDialog.show();
		
		prgLoadding = (ProgressBar) mDialog.findViewById(R.id.prgLoadding);
		rlParent = (RelativeLayout)  mDialog.findViewById(R.id.rlParent);

	//	ScreenUtils.vSetPadding(mActivity, mDialog.findViewById(R.id.rlParent));

		try {
			String message = jsonObject.getString("message");
			TextView tv = (TextView) mDialog.findViewById(R.id.tv);
			tv.setText(message);
			time = jsonObject.getInt("time_retry");
			if (jsonObject.getString("retry").equals("1")) {
				
				mDialog.findViewById(R.id.tvRetry).setVisibility(View.VISIBLE);
				mDialog.findViewById(R.id.tvRetry).setOnClickListener(new View.OnClickListener() {

					@Override
					public void onClick(View v) {
						// TODO Auto-generated method stub
						if(Utils.checkInternet(mActivity))
						{
							//AnimationUtils.vAnimationClick(v);
							//mDialog.dismiss();
							if(!isPendingRetry)
							{
								isPendingRetry =true;
								onFuckListener.onFuck();
								h.postDelayed(new Runnable() {
									
									@Override
									public void run() {
										// TODO Auto-generated method stub
										isPendingRetry =false;
										//Toast.makeText(mActivity, "demoo", 1000).show();
									}
								}, time*1000);
								mDialog.dismiss();
							}
							else
							{//Toast.makeText(mActivity, "demoo2", 1000).show();
							rlParent.setVisibility(View.GONE);
							prgLoadding.setVisibility(View.VISIBLE);
							
							h.postDelayed(new Runnable() {
								
								@Override
								public void run() {
									// TODO Auto-generated method stub
								
									rlParent.setVisibility(View.VISIBLE);
									prgLoadding.setVisibility(View.GONE);
								}
							}, 1000);
							}
							
						}
						else
						{
							Toast.makeText(mActivity, mActivity.getResources().getString(R.string.errorConnect), 1000).show();
						}
							
						
					}
				});
			} else {
				mDialog.findViewById(R.id.tvCenterDivider).setVisibility(View.GONE);
				mDialog.findViewById(R.id.tvRetry).setVisibility(View.GONE);
			}

			if (jsonObject.getString("logout").equals("1")) {
				mDialog.findViewById(R.id.tvLogout).setVisibility(View.VISIBLE);
				mDialog.findViewById(R.id.tvLogout).setOnClickListener(new View.OnClickListener() {

					@Override
					public void onClick(View v) {
						// TODO Auto-generated method stub
						
						// TODO Auto-generated method stub
						if(Utils.checkInternet(mActivity))
						{
							mDialog.dismiss();
						
							SohaSDK.logoutForBlock();
						/*	AnimationUtils.vAnimationClick(v);
							Utils.saveSoapAccessToken(mActivity, "");
							mActivity.finish();
							Intent i = mActivity.getBaseContext().getPackageManager()
									.getLaunchIntentForPackage(mActivity.getBaseContext().getPackageName() );
							i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
							mActivity.startActivity(i);*/
							SohaSDK.destroyConnectView();
							SohaSDK.hideWarningButton();
							SohaSDK.hideDashboard();
							
						}
						else
						{
							Toast.makeText(mActivity, mActivity.getResources().getString(R.string.errorConnect), 1000).show();
						}
						
					}
				});
			} else {
				mDialog.findViewById(R.id.tvLogout).setVisibility(View.GONE);
			}

			final String url = jsonObject.getString("detail_url");
			if (!TextUtils.isStringNull(url)) {
				mDialog.findViewById(R.id.btDetail).setVisibility(View.VISIBLE);
				mDialog.findViewById(R.id.btDetail).setOnClickListener(new View.OnClickListener() {

					@Override
					public void onClick(View v) {
						// TODO Auto-generated method stub
						if(Utils.checkInternet(mActivity))
						{
							AnimationUtils.vAnimationClick(v);
							Uri uriUrl = Uri.parse(url); 
							Intent launchBrowser = new Intent(Intent.ACTION_VIEW, uriUrl);  
							mActivity.startActivity(launchBrowser);  
						}
						else
						{
							Toast.makeText(mActivity, mActivity.getResources().getString(R.string.errorConnect), 1000).show();
						}
						
					}
				});
			} else {
				mDialog.findViewById(R.id.btDetail).setVisibility(View.GONE);
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		AnimationUtils.vAnimationClick(v);

	}

}
