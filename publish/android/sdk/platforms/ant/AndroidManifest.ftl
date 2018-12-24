<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android" 
    package="${entity.javaPackage}" 
    android:theme="@android:style/Theme.NoTitleBar"
    android:installLocation="preferExternal">
	
    <uses-sdk android:minSdkVersion="12" android:targetSdkVersion="26" />
    <supports-screens android:smallScreens="true" android:normalScreens="true" android:largeScreens="true" android:xlargeScreens="true" android:anyDensity="true" />
	
	<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
	<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	<uses-permission android:name="android.permission.READ_PHONE_STATE" />
	<uses-permission android:name="android.permission.INTERNET" />
	<uses-permission android:name="android.permission.GET_TASKS" />
	<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />

    <application android:debuggable="false" android:icon="@drawable/app_icon" android:label="@string/app_name" android:theme="@android:style/Theme.NoTitleBar.Fullscreen">       
    </application>
</manifest>

