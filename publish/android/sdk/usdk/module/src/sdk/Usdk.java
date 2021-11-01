package com.usdk.sdk;

import java.util.LinkedHashMap;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;

import com.unity3d.player.UnityPlayer;
import com.usdk.util.ReflectionUtils;

public class Usdk {
	private static LinkedHashMap<String, UsdkBase> m_pluginMap = new LinkedHashMap<String, UsdkBase>();
	private static Activity _unityActivity = null;
	private static Bundle _unityBundle = null;
	private static UsdkCallBackListener _callback;
	private static String TAG = "usdk";

	public static Activity getUnityActivity(){
		return _unityActivity;
	}

	public static Bundle getUnityBundle(){
		return _unityBundle;
	}

	public static UnityPlayer getUnityPlayer(){
		UsdkMainActivity unityActivity = getUnityActivity();
		return unityActivity.getUnityPlayer();
	}

	public static void setCallBack(UsdkCallBackListener callback){
		_callback = callback;
	}

	public static void sendCallBack2Unity(String eventName,String msg){
		if(_callback != null)
			_callback.OnCallBack(eventName,msg);
	}

	public static void addPlugin(String name) {
		if (!m_pluginMap.containsKey(name))
			m_pluginMap.put(name, null);
	}

	public static UsdkBase getPlugin(String name) {	
		if (m_pluginMap.containsKey(name))
			return m_pluginMap.get(name);
		else
			return load(name);
	}

	public static boolean isExistField(String pluginName, String fieldName) {
		UsdkBase plugin = getPlugin(pluginName);
		if (plugin != null)
			return ReflectionUtils.isExistField(plugin, fieldName);
		return false;
	}

	public static boolean isExistMethod(String pluginName, String methodName) {
		UsdkBase plugin = getPlugin(pluginName);
		if (plugin != null)
			return ReflectionUtils.isExistMethod(plugin, methodName);
		return false;
	}

	private static UsdkBase load(String name) {
		UsdkBase plugin = loadClass(Usdk.class.getClassLoader(), name,
				UsdkBase.class);
		m_pluginMap.put(name, plugin);
		
		if (plugin == null)
			Log.e(TAG, String.format("not found '%s' class.", name));
		else {
			plugin.onCreate(unityActivity, unityBundle);
		}
			
		return plugin;
	}

	@SuppressWarnings("unchecked")
	private static <T> T loadClass(ClassLoader loader, String className,
			Class<T> claz) {
		if (loader == null) {
			return null;
		}
		try {
			Class<?> loaded = loader.loadClass(className);
			if (claz.isAssignableFrom(loaded))
				return (T) loaded.newInstance();
		} catch (ClassNotFoundException e) {
			Log.i(TAG, "can not find class " + className);
		} catch (Exception e) {
			Log.e(TAG, "can not create instance for class " + className,
					e);
		}
		return null;
	}

	public static void onCreate(Activity activity, Bundle savedInstanceState) {
		_unityActivity = activity;
		_unityBundle = savedInstanceState;
	
		load("com.usdk.plugin.PlatformProxy");
	}

	public static void onStart() {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onStart();
		}
	}

	public static void onDestroy() {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onDestroy();
		}
	}

	public static void onStop() {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onStop();
		}
	}

	public static void onResume() {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onResume();
		}
	}

	public static void onPause() {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onPause();
		}
	}

	public static void onRestart() {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onRestart();
		}
	}

	public static void onActivityResult(int requestCode, int resultCode,
			Intent data) {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onActivityResult(requestCode, resultCode, data);
		}
	}
	
	public static void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}

	public static void onNewIntent(Intent intent) {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onNewIntent(intent);
		}
	}

	public static void onConfigurationChanged(Configuration arg0) {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onConfigurationChanged(arg0);
		}
	}

	public static void onSaveInstanceState(Bundle outState) {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onSaveInstanceState(outState);
		}
	}

	public static void onBackPressed() {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.onBackPressed();
		}
	}

	public static void finish() {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				plugin.finish();
		}
	}

	public static boolean onKeyDown(int arg0, KeyEvent arg1) {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				return plugin.onKeyDown(arg0, arg1);
		}
		return false;
	}

	public static boolean onKeyUp(int arg0, KeyEvent arg1) {
		LinkedHashMap<String, UsdkBase> tmp_pluginMap = new LinkedHashMap<String, UsdkBase>();
		tmp_pluginMap.putAll(m_pluginMap);
		for (String key : tmp_pluginMap.keySet()) {
			UsdkBase plugin = tmp_pluginMap.get(key);
			if (plugin != null)
				return plugin.onKeyUp(arg0, arg1);
		}
		return false;
	}
}
