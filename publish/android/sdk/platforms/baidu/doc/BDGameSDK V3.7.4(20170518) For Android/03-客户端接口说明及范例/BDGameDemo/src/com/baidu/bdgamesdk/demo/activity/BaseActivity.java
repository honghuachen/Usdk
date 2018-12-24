package com.baidu.bdgamesdk.demo.activity;

import android.app.Activity;
import android.os.Bundle;

import com.baidu.gamesdk.BDGameSDK;

public class BaseActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        // TODO Auto-generated method stub
        super.onCreate(savedInstanceState);
    }

    @Override
    protected void onResume() {
        super.onResume();
        BDGameSDK.onResume(this);
    }

    @Override
    protected void onPause() {
        super.onPause();
        BDGameSDK.onPause(this);
    }

}
