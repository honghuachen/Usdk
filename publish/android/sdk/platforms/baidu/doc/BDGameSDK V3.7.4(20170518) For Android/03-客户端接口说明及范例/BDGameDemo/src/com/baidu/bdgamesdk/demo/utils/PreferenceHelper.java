package com.baidu.bdgamesdk.demo.utils;

import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;

public class PreferenceHelper {

    private static final String PREFERENCE = "demo";

    public static final String DEBUG = "DEBUG";
    public static final String ORIENTATION = "ORIENTATION";

    public static boolean getValue(Context context, String key) {
        SharedPreferences settings = context.getSharedPreferences(PREFERENCE, Context.MODE_PRIVATE);
        boolean value = settings.getBoolean(key, false);
        return value;
    }

    public static void setValue(Context context, String key, boolean value) {
        SharedPreferences settings = context.getSharedPreferences(PREFERENCE, Context.MODE_PRIVATE);
        Editor editor = settings.edit();
        editor.putBoolean(key, value);
        editor.commit();
    }

    public static int getIntValue(Context context, String key, int defaultValue) {
        SharedPreferences settings = context.getSharedPreferences(PREFERENCE, Context.MODE_PRIVATE);
        int value = settings.getInt(key, defaultValue);
        return value;
    }

    public static void setIntValue(Context context, String key, int value) {
        SharedPreferences settings = context.getSharedPreferences(PREFERENCE, Context.MODE_PRIVATE);
        Editor editor = settings.edit();
        editor.putInt(key, value);
        editor.commit();
    }

}
