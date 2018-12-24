package vn.soha.game.sdk.dialogs;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.SohaSDK;
import vn.soha.game.sdk.utils.NetworkUtils;
import vn.soha.game.sdk.utils.ToastUtils;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnDismissListener;
import android.content.IntentFilter;
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
@SuppressLint({ "JavascriptInterface", "SetJavaScriptEnabled", "InflateParams" }) public class DialogLogoutTokenExpried {

	private static Activity mActivity;
	public static Dialog mDialog;


	private WebView wv;
	private IntentFilter mIntentFilter = new IntentFilter("android.net.conn.CONNECTIVITY_CHANGE");
	String mPageErrorHtml;
	String mFailingUrl;


	TextView tvContent,tvOK;
	String message;

	public DialogLogoutTokenExpried(Activity activity,String mesage ) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		this.message = mesage;
		vInitUI();
		
	}

   public static void closeConnectDialog()
   {
	   if(mDialog!=null)
	   {
		   mDialog.dismiss();  
	   }
   }
	public void vInitUI() {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.getWindow().setSoftInputMode(LayoutParams.SOFT_INPUT_STATE_HIDDEN);
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_logout);
		mDialog.setCancelable(true);
		tvContent = (TextView) mDialog.findViewById(R.id.tvContent);
		tvContent.setText(message);
		tvOK = (TextView) mDialog.findViewById(R.id.tvOK);
		
		
		mDialog.setOnDismissListener(new OnDismissListener() {
			
			@Override
			public void onDismiss(DialogInterface dialog) {
				// TODO Auto-generated method stub
			  SohaSDK.logoutFromServer();
			}
		});
		
		tvOK.setOnClickListener(new OnClickListener() {
			
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
