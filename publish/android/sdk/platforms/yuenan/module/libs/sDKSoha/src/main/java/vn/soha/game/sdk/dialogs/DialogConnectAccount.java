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
import android.os.Build;
import android.provider.Settings;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager.LayoutParams;
import android.webkit.WebView;
import android.widget.TextView;

/**
 * @author Hoang Cao Dev
 * @since January 27, 2015
 */
@SuppressLint({"JavascriptInterface", "SetJavaScriptEnabled", "InflateParams"})
public class DialogConnectAccount {

    private static Activity mActivity;
    public static Dialog mDialog;


    private WebView wv;
    private IntentFilter mIntentFilter = new IntentFilter("android.net.conn.CONNECTIVITY_CHANGE");
    String mPageErrorHtml;
    String mFailingUrl;
    public static Boolean isShowConnectPopup = false;

    TextView tvCancel, tvConnect;
    InteractConnectDialog interact;

    public DialogConnectAccount(Activity activity, InteractConnectDialog interact) {
        // TODO Auto-generated constructor stub
        mActivity = activity;
        this.interact = interact;
        vInitUI();

    }

    public static void closeConnectDialog() {
        if (mDialog != null) {
            mDialog.dismiss();
        }
    }

    public void vInitUI() {
        mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
        mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
        mDialog.getWindow().setSoftInputMode(LayoutParams.SOFT_INPUT_STATE_HIDDEN);
        mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        mDialog.setContentView(R.layout.dialog_connect_account);
        mDialog.setCancelable(true);
        tvCancel = (TextView) mDialog.findViewById(R.id.tvCancel);
        tvConnect = (TextView) mDialog.findViewById(R.id.tvConnect);
        tvConnect.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                // TODO Auto-generated method stub

                if (NetworkUtils.isInternetConnected(mActivity)) {
                    mDialog.dismiss();
                    //SohaSDK.destroyConnectView();
                    new DialogLoginForConnectNew(mActivity);
                } else {
                    ToastUtils.vToastErrorNetwork(mActivity);
                }

            }
        });

        mDialog.setOnDismissListener(new OnDismissListener() {

            @Override
            public void onDismiss(DialogInterface dialog) {
                // TODO Auto-generated method stub
                isShowConnectPopup = false;
            }
        });

        tvCancel.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                // TODO Auto-generated method stub
                mDialog.dismiss();
                if (interact != null) {
                    if (Build.VERSION.SDK_INT >= 23) {
                        if (Settings.canDrawOverlays(mActivity)) {
                            SohaSDK.showWarningButton(mActivity);
                        }
                    } else {
                        SohaSDK.showWarningButton(mActivity);
                    }
                    interact.onCancel();
                }

            }
        });

        try {
            mDialog.show();
            if (SohaSDK.connectAccountView != null) {
                SohaSDK.connectAccountView.hide();
            }
            isShowConnectPopup = true;
        } catch (Exception e) {
            // TODO: handle exception
            e.printStackTrace();
        }


    }

    public interface InteractConnectDialog {
        public void onCancel();
    }

}
