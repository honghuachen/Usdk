package vn.soha.game.sdk.dialogs;

import java.util.ArrayList;
import java.util.List;

import com.squareup.picasso.Picasso;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.content.res.Resources;
import android.net.Uri;
import android.util.TypedValue;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager.LayoutParams;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import vn.sgame.sdk.R;
import vn.sgame.sdk.view.DashboardButton;
import vn.soha.game.sdk.SohaApplication;
import vn.soha.game.sdk.SohaSDK;
import vn.soha.game.sdk.adapter.DashBoardAdapter;
import vn.soha.game.sdk.interact.InteractConfirmOpenFanpage;
import vn.soha.game.sdk.model.DashBoardItem;
import vn.soha.game.sdk.utils.Logger;
import vn.soha.game.sdk.utils.MQTTUtils;
import vn.soha.game.sdk.utils.ToastUtils;
import vn.soha.game.sdk.utils.Utils;

public class DialogDashboardNew implements OnItemClickListener {

	private static Activity mActivity;
	public static Dialog mDialog;
	private DashBoardAdapter mDashBoardAdapter;
	private LinearLayout mLinearLayoutBodyDB6, mLinearLayoutBodyDB8;
	private RelativeLayout mRelativeLayoutDb;
	private GridView mGridView;
	private List<DashBoardItem> mListItems;
	private boolean mIsMoreItems = false;
	private String FACEBOOK_URL = "https://www.facebook.com/";
	private String TWITTER_URL = "https://twitter.com/";
	private DialogDashboardConfirmOpenFanpage mDialogConfirm;
	private int mSizeWidthGridView = 0;

	public DialogDashboardNew(Activity activity, List<DashBoardItem> listItem) {
		mActivity = activity;
		mListItems = listItem;

		SohaSDK.hideDashboard();
		SohaSDK.fuckLog(MQTTUtils.ACTION_OPEN_DB, "");
		SohaSDK.sendLog(MQTTUtils.ACTION_OPEN_DB, "");
		SohaApplication.getInstance().trackEvent("sdk", MQTTUtils.ACTION_OPEN_DB, "");

		vInitUI();
		vInitData();
	}

	private void vInitUI() {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.getWindow().setSoftInputMode(LayoutParams.SOFT_INPUT_STATE_HIDDEN);
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_dashboard);
		mDialog.setCancelable(true);

		mDialog.setOnCancelListener(new DialogInterface.OnCancelListener() {
			@Override
			public void onCancel(DialogInterface dialog) {
				sendLogDialogClose();
			}
		});
		mRelativeLayoutDb = (RelativeLayout) mDialog.findViewById(R.id.relative_layout_db);
		mRelativeLayoutDb.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View view) {
				sendLogDialogClose();
			}
		});

		mLinearLayoutBodyDB6 = (LinearLayout) mDialog.findViewById(R.id.ln_body_db_6);
		mLinearLayoutBodyDB8 = (LinearLayout) mDialog.findViewById(R.id.ln_body_db_8);

		calculateMeasureOfDashBoard();

		// 8 items
		mGridView = (GridView) mDialog.findViewById(R.id.gridview_db);
		mGridView.setOnItemClickListener(this);

		mDialog.show();
	}

	private void calculateMeasureOfDashBoard() {
		int iDisplayWidthPx = mActivity.getResources().getDisplayMetrics().widthPixels;
		int iDisplayHeightPx = mActivity.getResources().getDisplayMetrics().heightPixels;
		int pixeldpi = Resources.getSystem().getDisplayMetrics().densityDpi;
		int iDisplayWidthDp = 0;
		if (mActivity.getRequestedOrientation() == ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE) {
			iDisplayWidthDp = (iDisplayHeightPx / pixeldpi) * 160;
		} else if (mActivity.getRequestedOrientation() == ActivityInfo.SCREEN_ORIENTATION_PORTRAIT) {
			iDisplayWidthDp = (iDisplayWidthPx / pixeldpi) * 160;
		}
		Logger.e("Screen: px " + iDisplayWidthPx + "-" + iDisplayHeightPx + " dp " + iDisplayWidthDp + " dpi "
				+ pixeldpi);

		if (iDisplayWidthDp <= 320)
			mSizeWidthGridView = ((340 - 60) * pixeldpi) / 160;
		else if (iDisplayWidthDp <= 480 && iDisplayWidthDp > 320) {
			mSizeWidthGridView = ((360 - 60) * pixeldpi) / 160;
		} else if (iDisplayWidthDp <= 640 && iDisplayWidthDp > 480) {
			mSizeWidthGridView = ((380 - 60) * pixeldpi) / 160;
		} else if (iDisplayWidthDp <= 800 && iDisplayWidthDp > 640) {
			mSizeWidthGridView = ((440 - 60) * pixeldpi) / 160;
		}

		RelativeLayout.LayoutParams lp1 = new RelativeLayout.LayoutParams(mSizeWidthGridView, mSizeWidthGridView);
		lp1.addRule(RelativeLayout.CENTER_IN_PARENT, RelativeLayout.TRUE);
		mLinearLayoutBodyDB8.setLayoutParams(lp1);

		RelativeLayout.LayoutParams lp2 = new RelativeLayout.LayoutParams(mSizeWidthGridView, mSizeWidthGridView);
		lp2.addRule(RelativeLayout.CENTER_IN_PARENT, RelativeLayout.TRUE);
		mLinearLayoutBodyDB6.setLayoutParams(lp2);
	}

	public static int getDPsFromPixel(Activity activity, int pxs) {
		Resources r = activity.getResources();
		int dps = (int) (TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_PX, pxs, r.getDisplayMetrics()));
		return dps;
	}

	private void vInitData() {
		if (mListItems.size() == 6) {
			// 6 items in DB
			mLinearLayoutBodyDB8.setVisibility(View.GONE);
			mLinearLayoutBodyDB6.setVisibility(View.VISIBLE);

			// fill data
			initDataTextViewTitle(mListItems.get(0).getTitle(), R.id.tv1);
			initDataImageViewIcon(mListItems.get(0).getIcon(), R.id.iv1);
			initDataTextViewNotify(mListItems.get(0).getNotify(), R.id.tvNotify1);
			initDataOnClickItem(mListItems.get(0), R.id.linear_layout_1);

			initDataTextViewTitle(mListItems.get(1).getTitle(), R.id.tv2);
			initDataImageViewIcon(mListItems.get(1).getIcon(), R.id.iv2);
			initDataTextViewNotify(mListItems.get(1).getNotify(), R.id.tvNotify2);
			initDataOnClickItem(mListItems.get(1), R.id.linear_layout_2);

			initDataTextViewTitle(mListItems.get(2).getTitle(), R.id.tv3);
			initDataImageViewIcon(mListItems.get(2).getIcon(), R.id.iv3);
			initDataTextViewNotify(mListItems.get(2).getNotify(), R.id.tvNotify3);
			initDataOnClickItem(mListItems.get(2), R.id.linear_layout_3);

			initDataTextViewTitle(mListItems.get(3).getTitle(), R.id.tv4);
			initDataImageViewIcon(mListItems.get(3).getIcon(), R.id.iv4);
			initDataTextViewNotify(mListItems.get(3).getNotify(), R.id.tvNotify4);
			initDataOnClickItem(mListItems.get(3), R.id.linear_layout_4);

			initDataTextViewTitle(mListItems.get(4).getTitle(), R.id.tv5);
			initDataImageViewIcon(mListItems.get(4).getIcon(), R.id.iv5);
			initDataTextViewNotify(mListItems.get(4).getNotify(), R.id.tvNotify5);
			initDataOnClickItem(mListItems.get(4), R.id.linear_layout_5);

			initDataTextViewTitle(mListItems.get(5).getTitle(), R.id.tv6);
			initDataImageViewIcon(mListItems.get(5).getIcon(), R.id.iv6);
			initDataTextViewNotify(mListItems.get(5).getNotify(), R.id.tvNotify6);
			initDataOnClickItem(mListItems.get(5), R.id.linear_layout_6);
		} else {
			// 8 items in DB or more than
			mLinearLayoutBodyDB6.setVisibility(View.GONE);
			mLinearLayoutBodyDB8.setVisibility(View.VISIBLE);

			if (mListItems.size() == 8) {
				mListItems.add(4, new DashBoardItem("blank", "blank", "blank", "blank", -1, -1));
				mDashBoardAdapter = new DashBoardAdapter(mActivity, mListItems, false);
			} else if (mListItems.size() > 8) {
				mIsMoreItems = true;
				mListItems.add(4, new DashBoardItem("blank", "blank", "blank", "blank", -1, -1));
				mListItems.add(8, new DashBoardItem(mActivity.getResources().getString(R.string.textviewMore), "blank",
						"blank", "blank", -1, -1));
				if (mListItems.size() >= 13)
					mListItems.add(13, new DashBoardItem("blank", "blank", "blank", "blank", -1, -1));

				while (mListItems.size() % 9 != 0) {
					mListItems.add(new DashBoardItem("blank", "blank", "blank", "blank", -1, -1));
				}

				List<DashBoardItem> dbItems = new ArrayList<DashBoardItem>();
				for (int i = 0; i <= 8; i++) {
					dbItems.add(mListItems.get(i));
				}
				mDashBoardAdapter = new DashBoardAdapter(mActivity, dbItems, false);
			}

			mGridView.setAdapter(mDashBoardAdapter);
		}
	}

	private void initDataTextViewNotify(int totalNotify, int resId) {
		TextView tvNotify = (TextView) mDialog.findViewById(resId);
		if (totalNotify > 0) {
			tvNotify.setText(String.valueOf(totalNotify));
			tvNotify.setVisibility(View.VISIBLE);
		} else {
			tvNotify.setVisibility(View.GONE);
		}
	}

	private void initDataTextViewTitle(String title, int resId) {
		((TextView) mDialog.findViewById(resId)).setText(title);
	}

	private void initDataImageViewIcon(String url, int resId) {
		Picasso.with(mActivity).load(url).into((ImageView) mDialog.findViewById(resId));
	}

	private void initDataOnClickItem(DashBoardItem dbItem, int resId) {
		((LinearLayout) mDialog.findViewById(resId)).setOnClickListener(new OnClickCheckDataListener(dbItem));
	}

	public class OnClickCheckDataListener implements OnClickListener {

		DashBoardItem mDbItem;

		public OnClickCheckDataListener(DashBoardItem dbItem) {
			this.mDbItem = dbItem;
		}

		@Override
		public void onClick(View v) {
			checkDataToClick(mDbItem);
		}

	};

	@Override
	public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
		List<DashBoardItem> mDB = mDashBoardAdapter.getListDashBoard();
		DashBoardItem mDashBoard = mDB.get(position);
		if (position == 8 && mIsMoreItems) {
			List<DashBoardItem> dbItems = new ArrayList<DashBoardItem>();
			for (int i = 9; i < mListItems.size(); i++) {
				dbItems.add(mListItems.get(i));
			}
			mDashBoardAdapter = new DashBoardAdapter(mActivity, dbItems, true);
			mGridView.setAdapter(mDashBoardAdapter);
		} else if (position == 4 && mIsMoreItems) {
			List<DashBoardItem> dbItems = new ArrayList<DashBoardItem>();
			for (int i = 0; i <= 8; i++) {
				dbItems.add(mListItems.get(i));
			}
			mDashBoardAdapter = new DashBoardAdapter(mActivity, dbItems, false);
			mGridView.setAdapter(mDashBoardAdapter);
		}

		checkDataToClick(mDashBoard);

	}

	public void startFacebookPage(Context context, String pageId) {
		Intent facebookIntent = null;
		try {
			facebookIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("fb://page/" + pageId));
		} catch (Exception e) {
			facebookIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(FACEBOOK_URL + pageId));
		}
		if (facebookIntent.resolveActivity(mActivity.getPackageManager()) != null) {
			context.startActivity(facebookIntent);
		} else {
			facebookIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(FACEBOOK_URL + pageId));
			context.startActivity(facebookIntent);
		}
	}

	public void startTwitterPage(Context context, String pageId) {
		Intent twitterintent = null;
		try {
			twitterintent = new Intent(Intent.ACTION_VIEW, Uri.parse("twitter://user?user_id=" + pageId));
			twitterintent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
		} catch (Exception e) {
			twitterintent = new Intent(Intent.ACTION_VIEW, Uri.parse(TWITTER_URL + pageId));
		}
		context.startActivity(twitterintent);
	}

	public void startSohaCare(Context context, String packageName) {
		Intent intent = context.getPackageManager().getLaunchIntentForPackage(packageName);
		if (intent == null) {
			intent = new Intent(Intent.ACTION_VIEW);
			intent.setData(Uri.parse("market://details?id=" + packageName));
		}
		intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
		intent.putExtra("token", Utils.getSoapAccessToken(mActivity));
		context.startActivity(intent);
	}

	public void sendLogDialogClose() {
		SohaSDK.fuckLog(MQTTUtils.ACTION_CLOSE_DB, "");
		SohaSDK.sendLog(MQTTUtils.ACTION_CLOSE_DB, "");
		SohaApplication.getInstance().trackEvent("sdk", MQTTUtils.ACTION_CLOSE_DB, "");

		mDialog.dismiss();
		DashboardButton.isPendingClick = false;
		if (!isCallApp) {
			SohaSDK.showDashboard(mActivity);
		} else {
			SohaSDK.showPendingDashBoard();
		}

	}

	Boolean isCallApp = false;

	private void checkDataToClick(final DashBoardItem mDashBoard) {
		if (mDashBoard.getActive() == 1) {
			if (mDashBoard.getType().contains("page_facebook")) {

				mDialogConfirm = new DialogDashboardConfirmOpenFanpage(mActivity,
						mActivity.getResources().getString(R.string.contentOpenFanpageFB),
						new InteractConfirmOpenFanpage() {

							@Override
							public void onCancel() {
							}

							@Override
							public void onAccept() {
								isCallApp = true;
								mDialog.dismiss();
								startFacebookPage(mActivity, mDashBoard.getPageId());
								sendLogDialogClose();
							}
						});
				mDialogConfirm.show();
			} else if (mDashBoard.getType().contains("page_twitter")) {
				mDialogConfirm = new DialogDashboardConfirmOpenFanpage(mActivity,
						mActivity.getResources().getString(R.string.contentOpenFanpageTwitter),
						new InteractConfirmOpenFanpage() {

							@Override
							public void onCancel() {
							}

							@Override
							public void onAccept() {
								isCallApp = true;
								mDialog.dismiss();
								startTwitterPage(mActivity, mDashBoard.getPageId());
								sendLogDialogClose();
							}
						});
				mDialogConfirm.show();
			} else if (mDashBoard.getType().contains("sohacare")) {
				mDialogConfirm = new DialogDashboardConfirmOpenFanpage(mActivity,
						mActivity.getResources().getString(R.string.contentOpenFanpageSohaCare),
						new InteractConfirmOpenFanpage() {

							@Override
							public void onCancel() {
							}

							@Override
							public void onAccept() {
								isCallApp = true;
								mDialog.dismiss();
								startSohaCare(mActivity, "com.soha.sohacustomerservices");
								sendLogDialogClose();
							}
						});
				mDialogConfirm.show();
			} else {
				mDialog.dismiss();
				new DialogDetailDashBoard(mActivity, mDashBoard.getUrl());
			}
		} else if (mDashBoard.getActive() == 0) {
			ToastUtils.vToastShort(mActivity, mDashBoard.getMessageActive());
		}
	}

}
