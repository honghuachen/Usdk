package com.zwwx.platform;

import java.lang.reflect.Method;
import android.app.Application;
import android.content.Context;

public class BaiduApplication extends Application{   
    @Override
    protected void attachBaseContext(Context base) {
        super.attachBaseContext(base);

        try { 
            Class<?> threadClazz = Class.forName("com.baidu.gamesdk.BDGameSDK");
            Method method = threadClazz.getMethod("initApplication", Application.class);
            method.invoke(null, this);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}