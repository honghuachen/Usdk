package vn.soha.game.sdk.dialogs;

import java.util.List;

import android.app.Activity;
import android.app.Dialog;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.TextView;
import vn.sgame.sdk.R;
import vn.soha.game.sdk.interact.InteractConfirmOpenFanpage;
import vn.soha.game.sdk.model.DashBoardItem;
import vn.soha.game.sdk.utils.Utils;

public class DialogDashboardConfirmOpenFanpage {

	private Activity mActivity;
	private static Dialog mDialog;

	InteractConfirmOpenFanpage interactice;

	TextView btnAccept, btnCancel;

	List<DashBoardItem> listItem;
	private String mContent;

	public DialogDashboardConfirmOpenFanpage(Activity activity, String content, InteractConfirmOpenFanpage interactice) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		this.interactice = interactice;
		this.mContent = content;
		initDialog();

	}

	public void show() {
		if (mDialog != null) {
			mDialog.show();
		}
	}

	public void initDialog() {
		if (Utils.isFullScreen(mActivity)) {
			mDialog = new Dialog(mActivity, android.R.style.Theme_Light_NoTitleBar_Fullscreen);
		} else {
			mDialog = new Dialog(mActivity, android.R.style.Theme_Light_NoTitleBar);
		}
		mDialog.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_dashboard_confirm_open_fanpage);

		((TextView) mDialog.findViewById(R.id.tvContent)).setText(mContent);

		btnAccept = (TextView) mDialog.findViewById(R.id.tvAccept);
		btnCancel = (TextView) mDialog.findViewById(R.id.tvCancel);
		btnCancel.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				mDialog.dismiss();
				interactice.onCancel();

			}
		});

		btnAccept.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				mDialog.dismiss();
				interactice.onAccept();

			}
		});
		mDialog.setCancelable(false);

	}

	public void onDismiss() {
		if (mDialog != null && mDialog.isShowing())
			mDialog.dismiss();
	}

}
