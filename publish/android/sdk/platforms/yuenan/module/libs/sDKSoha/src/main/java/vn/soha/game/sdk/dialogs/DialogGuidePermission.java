package vn.soha.game.sdk.dialogs;

import android.app.Activity;
import android.app.Dialog;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.provider.Settings;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.webkit.WebView;
import android.widget.TextView;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.SohaSDK;
import vn.soha.game.sdk.server.API;

/**
 * Created by Admin on 8/20/2018.
 */

public class DialogGuidePermission {
    private Activity mActivity;
    private Dialog mDialog;
    private TextView tvSetting;
    private WebView wvGuide;
    public DialogGuidePermission(Activity mActivity) {
        this.mActivity = mActivity;
        // TODO Auto-generated constructor stub
        initUI();
    }
    private void initUI(){
        mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
        mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
        mDialog.getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
        mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        mDialog.setCancelable(false);
        mDialog.setContentView(R.layout.dialog_guide_permission);
        mDialog.show();
        tvSetting = mDialog.findViewById(R.id.tv_setting);
        wvGuide = mDialog.findViewById(R.id.wv_guide_permission);

        wvGuide.loadUrl(API.linkGuidePermission);
        Log.e("LOG_LINK_GUIDE", API.linkGuidePermission);
        tvSetting.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                mDialog.dismiss();
                // TODO Auto-generated method stub
                if (Build.VERSION.SDK_INT!=27) {
                    Intent intent = new Intent(
                            Settings.ACTION_MANAGE_OVERLAY_PERMISSION,
                            Uri.parse("package:"
                                    + mActivity
                                    .getPackageName()));
                    mActivity.startActivityForResult(intent,
                            SohaSDK.REQUEST_DRAW_OVERLAY);
                } else {
                    Intent myAppSettings = new Intent(Settings.ACTION_APPLICATION_DETAILS_SETTINGS,
                            Uri.parse("package:" + mActivity.getPackageName()));
                    mActivity.startActivityForResult(myAppSettings, SohaSDK.REQUEST_DRAW_OVERLAY);
                }
            }
        });
    }
}
