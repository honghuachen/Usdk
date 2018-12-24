package vn.soha.game.sdk.dialogs;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.utils.AnimationUtils;
import vn.soha.game.sdk.utils.PhotoLoader;
import vn.soha.game.sdk.utils.Utils;
import android.app.Dialog;
import android.content.Context;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager.LayoutParams;
import android.widget.ImageView;
import android.widget.TextView;

/**
 * @since January 27, 2015
 * @author Sohagame SDK Team
 *
 */
public class DialogWarning implements OnClickListener {

	private Context mContext;
	private Dialog mDialog;

	public DialogWarning(Context context) {
		// TODO Auto-generated constructor stub
		mContext = context;
		vInitUI();

	}

	public void vInitUI() {
		mDialog = new Dialog(mContext, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.getWindow().setSoftInputMode(LayoutParams.SOFT_INPUT_STATE_HIDDEN);
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_warning);
		mDialog.setCancelable(true);

		PhotoLoader.loadPhotoOrigin(Utils.getWarningImageAge(mContext), (ImageView) mDialog.findViewById(R.id.ivWarning), mContext,
				new PhotoLoader.OnImageLoaderListener() {

			@Override
			public void onImageLoaded() {
				// TODO Auto-generated method stub
				mDialog.show();
				// Utils.vSaveWarningInfo(mContext, "", "", false);
			}
		});

		((TextView) mDialog.findViewById(R.id.tvWarning)).setText(Utils.getWarningTimeMessage(mContext));
		mDialog.findViewById(R.id.ibClose).setOnClickListener(this);
	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		AnimationUtils.vAnimationClick(v);
		if (v.getId() == R.id.ibClose) {
			mDialog.dismiss();
		}
	}

}
