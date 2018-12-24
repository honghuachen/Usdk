package vn.soha.game.sdk.dialogs;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.utils.AnimationUtils;
import vn.soha.game.sdk.utils.AppUtils;
import vn.soha.game.sdk.utils.PhotoLoader;
import vn.soha.game.sdk.utils.ScreenUtils;
import vn.soha.game.sdk.utils.Utils;
import android.app.Activity;
import android.app.Dialog;
import android.content.Intent;
import android.net.Uri;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager.LayoutParams;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

/**
 * @since January 27, 2015
 * @author Sohagame SDK Team
 *
 */
public class DialogUpdate implements OnClickListener {

	private Activity mActivity;
	private Dialog mDialog;

	private ImageView ivGameIcon;
	private TextView tvFormatUpdate;

	private boolean mIsRequired;

	private String mUrl;

	public DialogUpdate(Activity activity, boolean isRequired, String url) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		mIsRequired = isRequired;
		mUrl = url;
		vInitUI();
	}


	public void vInitUI() {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.getWindow().setSoftInputMode(LayoutParams.SOFT_INPUT_STATE_HIDDEN);
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_update);
		mDialog.show();

		ivGameIcon = (ImageView) mDialog.findViewById(R.id.ivGameIcon);
		tvFormatUpdate = (TextView) mDialog.findViewById(R.id.tvFormatUpdate);

		ScreenUtils.vSetPadding(mActivity, mDialog.findViewById(R.id.layoutParent));
		PhotoLoader.loadPhotoOrigin2(Utils.getIconGame(mActivity), ivGameIcon, mActivity);
		tvFormatUpdate.setText(String.format(mActivity.getString(R.string.textviewFormatUpdate), AppUtils.getAppName(mActivity)));

		mDialog.findViewById(R.id.btDownloadNow).setOnClickListener(this);
		mDialog.findViewById(R.id.btContinue).setOnClickListener(this);

		if (mIsRequired) {
			mDialog.setCancelable(false);
			mDialog.findViewById(R.id.btContinue).setVisibility(View.GONE);
		}
	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		AnimationUtils.vAnimationClick(v);
		if (v.getId() == R.id.btDownloadNow) {
			if(Utils.checkInternet(mActivity))
			{
				mActivity.startActivity(new Intent(Intent.ACTION_VIEW,
						Uri.parse(mUrl)));
				mActivity.finish();
			}
			else
			{
				Toast.makeText(mActivity, mActivity.getResources().getString(R.string.errorConnect), 1000).show();
			}
			
		} else if (v.getId() == R.id.btContinue) {
			mDialog.dismiss();
		} 
	}

}
