package vn.soha.game.sdk.dialogs;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.TextView;

import vn.sgame.sdk.R;

/**
 * Created by Admin on 8/20/2018.
 */

public class DialogPermissionAcceptDashboard {
    private static Activity mActivity;
    private Context mContext;
    private Dialog mDialog;
    private TextView tvSetting;
    private TextView tvGuide;
    private clickSetting clickSetting;
    public DialogPermissionAcceptDashboard(Activity mActivity, clickSetting clickSetting) {
        // TODO Auto-generated constructor stub
        this.mActivity = mActivity;
        this.clickSetting = clickSetting;
        initUI();

    }
    private void initUI(){
        mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
        mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
        mDialog.getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_HIDDEN);
        mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        mDialog.setCancelable(false);
        mDialog.setContentView(R.layout.dialog_accept_dashboard);
        tvSetting = (TextView) mDialog.findViewById(R.id.tv_setting);
        tvGuide = (TextView) mDialog.findViewById(R.id.tv_guide);
        tvSetting.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                mDialog.dismiss();
                // TODO Auto-generated method stub
                clickSetting.click();
            }
        });
        tvGuide.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                mDialog.dismiss();
                // TODO Auto-generated method stub
                DialogGuidePermission dialog = new DialogGuidePermission(mActivity);
            }
        });
        mDialog.show();

    }

    public interface clickSetting {
        void click();
    }
}
