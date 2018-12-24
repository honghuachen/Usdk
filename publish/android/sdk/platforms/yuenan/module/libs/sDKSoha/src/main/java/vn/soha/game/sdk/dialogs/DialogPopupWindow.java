package vn.soha.game.sdk.dialogs;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.utils.NetworkUtils;
import vn.soha.game.sdk.utils.ToastUtils;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnDismissListener;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager.LayoutParams;
import android.widget.ImageView;
import android.widget.LinearLayout;

/**
 * @since January 27, 2015
 * @author Hoang Cao Dev
 *
 */
@SuppressLint({ "JavascriptInterface", "SetJavaScriptEnabled", "InflateParams" }) public class DialogPopupWindow {

	private static Activity mActivity;
	public static Dialog mDialog;



ImageView imgArrow;
LinearLayout lnPopupContainer,lnHome,lnLoginMobile,lnLoginFacebook;
InteractConnectPopup interact;

int notifyHeight,arrowLeftMargin,popupMargin;
	public DialogPopupWindow(Activity activity ,int notifyHeight,int arrowLeftMargin,int popupMargin,InteractConnectPopup interact) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		this.notifyHeight = notifyHeight;
		this.arrowLeftMargin =arrowLeftMargin;
		this.popupMargin = popupMargin;
		this.interact = interact;
		
		vInitUI();
		
	}


	public void vInitUI() {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.getWindow().setSoftInputMode(LayoutParams.SOFT_INPUT_STATE_HIDDEN);
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_popup_window);
		mDialog.setCancelable(false);
		
		lnPopupContainer = (LinearLayout) mDialog.findViewById(R.id.lnPopupContainer);
		lnLoginMobile = (LinearLayout) mDialog.findViewById(R.id.lnLoginMobile);
		lnLoginFacebook = (LinearLayout) mDialog.findViewById(R.id.lnLoginfaceBook);
		
		lnHome = (LinearLayout) mDialog.findViewById(R.id.lnHome);
		imgArrow = (ImageView) mDialog.findViewById(R.id.imgArrow);
		android.widget.LinearLayout.LayoutParams arrowParam = (android.widget.LinearLayout.LayoutParams) imgArrow.getLayoutParams();
		arrowParam.topMargin = notifyHeight;
		arrowParam.leftMargin=arrowLeftMargin;
		
		android.widget.LinearLayout.LayoutParams popupParam = (android.widget.LinearLayout.LayoutParams) lnPopupContainer.getLayoutParams();
		
		popupParam.leftMargin=popupMargin;
		
		try {
			mDialog.show();
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}
	
		
		mDialog.setOnDismissListener(new OnDismissListener() {
			
			@Override
			public void onDismiss(DialogInterface dialog) {
				// TODO Auto-generated method stub
				
					interact.onConnectDismiss(isHidePopup);

			}
		});
		lnHome.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				hide();
				
			}
		});
		lnLoginFacebook.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				if (NetworkUtils.isInternetConnected(mActivity)) {
					mDialog.dismiss();
					interact.onConnectFaceBook();
				} else {
					ToastUtils.vToastErrorNetwork(mActivity);
				}
				
			}
		});
		
		lnLoginMobile.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				if (NetworkUtils.isInternetConnected(mActivity)) {
					mDialog.dismiss();
					interact.onConnectMobile();
				} else {
					ToastUtils.vToastErrorNetwork(mActivity);
				}
				
			}
		});
		
	}
	boolean isHidePopup =false;
	public void hide()
	{
		isHidePopup =true;
		if(mDialog!=null)
		{
			
			mDialog.dismiss();
		}
	}

	public interface InteractConnectPopup
	{
		public void onConnectFaceBook();
		public void onConnectMobile();
		public void onConnectDismiss(Boolean isHidePopup);
	}
	
}
