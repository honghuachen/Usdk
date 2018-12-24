package vn.soha.game.sdk.dialogs;

import java.util.List;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.model.DashBoardItem;
import vn.soha.game.sdk.utils.Utils;
import android.app.Activity;
import android.app.Dialog;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.view.View;
import android.view.Window;
import android.widget.Button;

public class DialogDashboardClose {

	private Activity mActivity;
	private static Dialog mDialog;
	public boolean isDestroyed;


Button btSmallClose,btFullClose;
    
  
    List<DashBoardItem> listItem;
	public DialogDashboardClose(Activity activity) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		initDialog();
		
	
		
	
		
	}

	public Boolean isShowing()
	{
		return isShowing;
	}
Boolean isShowing =false;
	public void show() {
		if (mDialog != null) {
			mDialog.show();
			isShowing = true;
			btSmallClose.setVisibility(View.VISIBLE);
			btFullClose.setVisibility(View.GONE);
//			Button btnClose = (Button)mDialog.findViewById(R.id.btnCloseDashBoard);
//			
//			TranslateAnimation  transionANimation = new TranslateAnimation (0f,200f,0f,200f);
//			
//
//			transionANimation.setDuration(1000);
//		//animation.setInterpolator(new OvershootInterpolator());
//			btnClose.findViewById(R.id.rl_support_button).startAnimation(transionANimation);
		}
	}
	
	public void hide()
	{
		if (mDialog != null) {
			mDialog.dismiss();
			isShowing = false;
		}
	}

public void showFull()
{
	btSmallClose.setVisibility(View.GONE);
	btFullClose.setVisibility(View.VISIBLE);
}
public void showSmall()
{
	btSmallClose.setVisibility(View.VISIBLE);
	btFullClose.setVisibility(View.GONE);
}

	Boolean isSelected =false;

	public void initDialog() {
		if (Utils.isFullScreen(mActivity)) {
			mDialog = new Dialog(mActivity, android.R.style.Theme_Light_NoTitleBar_Fullscreen);	
		} else {
			mDialog = new Dialog(mActivity, android.R.style.Theme_Light_NoTitleBar);
		}
		mDialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_dashboard_close);
		btSmallClose = mDialog.findViewById(R.id.btnSmallClose);
		btFullClose = mDialog.findViewById(R.id.btnFullClose);
		mDialog.setCancelable(false);
	}
	
}
