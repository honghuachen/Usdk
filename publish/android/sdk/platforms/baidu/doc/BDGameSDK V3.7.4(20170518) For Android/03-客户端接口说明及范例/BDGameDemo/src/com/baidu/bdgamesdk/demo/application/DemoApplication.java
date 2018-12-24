package com.baidu.bdgamesdk.demo.application;

import android.app.Application;

public class DemoApplication extends Application {

    @Override
    public void onCreate() {
        super.onCreate();
        com.baidu.gamesdk.BDGameSDK.initApplication(this);
    }

}
