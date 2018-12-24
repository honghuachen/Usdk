package vn.soha.game.sdk.dialogs;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.utils.AnimationUtils;
import vn.soha.game.sdk.utils.ScreenUtils;
import android.app.Activity;
import android.app.Dialog;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager.LayoutParams;
import android.widget.TextView;

/**
 * @since January 27, 2015
 * @author Sohagame SDK Team
 *
 */
public class DialogFail implements OnClickListener {

	private Activity mActivity;
	private Dialog mDialog;

	public DialogFail(Activity activity, String msg) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		vInitUI(msg);
	}


	public void vInitUI(String msg) {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.getWindow().setSoftInputMode(LayoutParams.SOFT_INPUT_STATE_HIDDEN);
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_fail);
		mDialog.setCancelable(false);
		mDialog.show();

		ScreenUtils.vSetPadding(mActivity, mDialog.findViewById(R.id.layoutParent));

		TextView tv = (TextView) mDialog.findViewById(R.id.tv);
		tv.setText(msg);
	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		AnimationUtils.vAnimationClick(v);

	}

}
