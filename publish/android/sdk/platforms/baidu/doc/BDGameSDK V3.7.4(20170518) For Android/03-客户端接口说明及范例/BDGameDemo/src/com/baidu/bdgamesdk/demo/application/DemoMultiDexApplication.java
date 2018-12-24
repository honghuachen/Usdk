package com.baidu.bdgamesdk.demo.application;

import java.lang.reflect.Method;

import android.app.Application;
import android.content.Context;
import android.support.multidex.MultiDex;
import android.support.multidex.MultiDexApplication;

public class DemoMultiDexApplication extends MultiDexApplication {

    @Override
    protected void attachBaseContext(Context base) {
        // TODO Auto-generated method stub
        super.attachBaseContext(base);
        MultiDex.install(this);

        try { // 需通过反射调用，否则部分机型会出异常
            Class<?> threadClazz = Class.forName("com.baidu.gamesdk.BDGameSDK");
            Method method = threadClazz.getMethod("initApplication", Application.class);
            method.invoke(null, this);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

}
