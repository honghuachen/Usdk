package com.baidu.bdgamesdk.demo.activity;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Toast;

import com.baidu.bdgamesdk.demo.utils.Utils;
import com.baidu.gamesdk.BDGameSDK;
import com.baidu.gamesdk.BDGameSDKSetting;
import com.baidu.gamesdk.BDGameSDKSetting.Domain;
import com.baidu.gamesdk.IResponse;
import com.baidu.gamesdk.ResultCode;

/**
 * 启动页面
 * 
 */
public class WelcomeActivity extends BaseActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        initBDGameSDK();
    }

    private void initBDGameSDK() { // 初始化游戏SDK
        BDGameSDKSetting mBDGameSDKSetting = new BDGameSDKSetting();
        mBDGameSDKSetting.setAppID(3067515); // APPID设置
        mBDGameSDKSetting.setAppKey("f3Os4GAOqxgm79GqbnkT9L8T"); // APPKEY设置
        mBDGameSDKSetting.setDomain(Domain.RELEASE); // 设置为正式模式
        mBDGameSDKSetting.setOrientation(Utils.getOrientation(this));

        BDGameSDK.init(this, mBDGameSDKSetting, new IResponse<Void>() {

            @Override
            public void onResponse(int resultCode, String resultDesc, Void extraData) {
                switch (resultCode) {
                    case ResultCode.INIT_SUCCESS:
                        // 初始化成功
                        Intent intent = new Intent(WelcomeActivity.this, MainActivity.class);
                        startActivity(intent);
                        finish();
                        break;

                    case ResultCode.INIT_FAIL:
                    default:
                        Toast.makeText(WelcomeActivity.this, "启动失败", Toast.LENGTH_LONG).show();
                        finish();
                        // 初始化失败
                }

            }

        });
    }

}
