package com.usdk.util;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.lang.reflect.InvocationTargetException;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;
import java.util.zip.ZipInputStream;
import java.util.zip.ZipEntry;

import org.xmlpull.v1.XmlPullParser;

import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.content.res.AssetManager;
import android.content.res.Resources;
import android.content.res.Resources.NotFoundException;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.provider.Settings.Secure;
import android.telephony.TelephonyManager;
import android.text.TextUtils;
import android.util.Log;
import android.util.Xml;

import com.unity3d.player.UnityPlayer;

public class Content {
	public static String getSystemModel() {
		return android.os.Build.MODEL;
	}

	public static String getAppVersion() {
		return getPackageInfo().versionName;
	}

	public static int getAppVersionCode() {
		return getPackageInfo().versionCode;
	}
	
	public static int getBatteryLevel() {
		Intent batteryInfoIntent = UnityPlayer.currentActivity
                .registerReceiver( null ,  
                        new IntentFilter( Intent.ACTION_BATTERY_CHANGED ) ) ;  

        int level = batteryInfoIntent.getIntExtra( "level" , 0 );//������0-100��  
		return level;
	}

	private static PackageInfo getPackageInfo() {
		PackageInfo pi = null;
		try {
			PackageManager pm = UnityPlayer.currentActivity.getPackageManager();
			pi = pm.getPackageInfo(
					UnityPlayer.currentActivity.getPackageName(),
					PackageManager.GET_CONFIGURATIONS);

			return pi;
		} catch (Exception e) {
			e.printStackTrace();
		}

		return pi;
	}

	public static String getMetaDataValue(String name) {
		Object value = null;
		PackageManager packageManager = UnityPlayer.currentActivity
				.getPackageManager();
		ApplicationInfo applicationInfo;
		try {

			applicationInfo = packageManager.getApplicationInfo(
					UnityPlayer.currentActivity.getPackageName(), 128);
			if (applicationInfo != null && applicationInfo.metaData != null) {
				value = applicationInfo.metaData.get(name);
			}
		} catch (NotFoundException e) {
			throw new RuntimeException(
					"Could not read the name in the manifest file.", e);
		} catch (NameNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		if (value == null) {
			throw new RuntimeException("The name '" + name
					+ "' is not defined in the manifest file's meta data.");
		}
		if (value.toString().length() < 6) {
			return "0" + value.toString();
		} else {
			return value.toString();
		}
	}

	public static String getCountry() {
		String country = UnityPlayer.currentActivity.getResources()
				.getConfiguration().locale.getCountry();
		return country;
	}

	public static String _getDeviceUUID() {
		TelephonyManager tm = (TelephonyManager) UnityPlayer.currentActivity
				.getSystemService(Context.TELEPHONY_SERVICE);
		String tmDevice = null;
		String tmMac = null;
		String androidId = null;
		try {
			tmDevice = tm.getDeviceId();
			String deviceId2 = tm.getDeviceId();
			if (!TextUtils.equals(tmDevice, deviceId2)) {
				tmDevice = null;
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		try {
			tmMac = _getMacAddress(UnityPlayer.currentActivity);
		} catch (Exception e) {
			e.printStackTrace();
		}
		try {
			androidId = Secure.getString(
					UnityPlayer.currentActivity.getContentResolver(),
					Secure.ANDROID_ID);
		} catch (Exception e) {
			e.printStackTrace();
		}
		if (tmDevice == null) {
			tmDevice = "";
		}
		if (tmMac == null) {
			tmMac = "";
		}
		if (androidId == null) {
			androidId = "";
		}
		UUID deviceUuid = new UUID(androidId.hashCode(),
				((long) tmDevice.hashCode() << 32) | tmMac.hashCode());
		return deviceUuid.toString();
	}

	private static String _getMacAddress(Context paramContext) {
		String str = null;
		try {
			WifiManager localWifiManager = (WifiManager) paramContext
					.getSystemService("wifi");
			WifiInfo localWifiInfo = localWifiManager.getConnectionInfo();
			str = localWifiInfo.getMacAddress();
		} catch (Exception localException) {
			localException.printStackTrace();
		}
		return str;
	}

	// --------------------------------------------------------------------------------
	// ��ȡѹ���ļ�����ѹ��??

	static public class ExtractContext {
		public String zip_file;
		public volatile int total_count;
		public volatile long total_size;
		public volatile int extracted_count;
		public volatile String extracting_file;
		public volatile boolean error = false;

		public boolean IsComplete() {
			return (total_count == extracted_count);
		}

		public String GetZipFile() {
			return zip_file;
		}

		public int GetTotalCount() {
			return this.total_count;
		}

		public int GetExtractedCount() {
			return extracted_count;
		}

		public String GetExtractingFile() {
			return extracting_file;
		}

		public boolean HaveError() {
			return error;
		}
	}

	public static ExtractContext ExtractZipFile(String zipfile,
			String output_dir,String apk_dir) throws InstantiationException, IllegalAccessException, IllegalArgumentException, InvocationTargetException, NoSuchMethodException {
		try {
			ExtractContext context = new ExtractContext();
			context.zip_file = zipfile;
			InputStream stream = null;
			
			apk_dir = apk_dir.replace("jar:file:///", "").replace("!/assets", "");
			AssetManager assetMAnager = AssetManager.class.newInstance();
			AssetManager.class.getDeclaredMethod("addAssetPath", String.class).invoke(assetMAnager, apk_dir);
			Resources resources = new Resources(assetMAnager, UnityPlayer.currentActivity.getResources().getDisplayMetrics(), UnityPlayer.currentActivity.getResources().getConfiguration());
			stream = resources.getAssets().open(zipfile);
			
			int[] cont_size = GetZipFileCountSize(stream);
			context.total_count = cont_size[0];
			context.total_size = cont_size[1];
			stream.close();

			String zipExtractSize = MemeryCheck
					.fileSizeFormat(context.total_size);
			if (MemeryCheck.sdCardExist()) {
				if (MemeryCheck.getSDCardRemainRoom() < context.total_size) {
					LOG.ShowAlertDialog(
							"sdcard room is not enough" + MemeryCheck.getSystemRemainRoom(),
							"need room" + zipExtractSize);
					throw new IOException("sdcard room is not enough");
				}
			} else {
				if (MemeryCheck.getSystemRemainRoom() < context.total_size) {
					LOG.ShowAlertDialog(
							"system room is not enough" + MemeryCheck.getSystemRemainRoom(),
							"need room" + zipExtractSize);
					throw new IOException("system room is not enough");
				}
			}

			stream = resources.getAssets().open(zipfile);
			ExtractThread thread = new ExtractThread(stream, output_dir,
					context);
			thread.start();

			return context;
		} catch (IOException e) {
			e.printStackTrace();
			return null;
		}
	}

	/*
	 * 获取zip文件中包含的文件
	 */
	private static int[] GetZipFileCountSize(InputStream stream)
			throws IOException {
		/*
		 * ZipInputStream zip_stream = new ZipInputStream(stream); ZipEntry
		 * entry = zip_stream.getNextEntry(); int count = 0; while (entry !=
		 * null) { if (!entry.isDirectory()) ++count; entry =
		 * zip_stream.getNextEntry(); }
		 */
		ZipInputStream zipIn = new ZipInputStream(stream);
		ZipEntry zipEntry;
		int count = 0;
		int size = 0;
		while ((zipEntry = zipIn.getNextEntry()) != null) {
			zipIn.closeEntry();
			if (!zipEntry.isDirectory()) {
				++count;
				size += zipEntry.getSize();
			}
		}
		zipIn.close();

		return new int[] { count, size };
	}

	static class ExtractThread extends Thread {
		private InputStream stream;
		private String output_dir;
		private ExtractContext context;

		public ExtractThread(InputStream stream, String out_dir,
				ExtractContext ctx) {
			this.stream = stream;
			this.output_dir = out_dir;
			this.context = ctx;
		}

		public void run() {
			try {
				DoExtract();
			} catch (final IOException e) {
				this.context.error = true;
				Log.e("usdk", "extract zip file exception: " + e.toString());

				// print error info to player
				LOG.ShowAlertDialog(
						"zip",
						"extract failed"
								+ MemeryCheck
										.fileSizeFormat(context.total_size));
				e.printStackTrace();
			}
		}

		public void DoExtract() throws IOException {
			File file = new File(this.output_dir);
			if (!file.exists())
				file.mkdirs();

			ZipInputStream zip_stream = new ZipInputStream(this.stream);
			byte[] buffer = new byte[4 * 1024];
			ZipEntry entry = zip_stream.getNextEntry();
			while (entry != null) {
				/*
				 * ��ʹ�����????????? if ( entry.isDirectory() ) { file = new File(
				 * this.output_dir + File.separator + entry.getName() ); if (
				 * !file.exists() ) file.mkdirs(); } else {
				 * this.context.extracting_file = entry.getName(); file = new
				 * File(this.output_dir + File.separator + entry.getName() );
				 * file.getParentFile().mkdirs(); FileOutputStream file_stream =
				 * new FileOutputStream(file); int size = 0; while ( ( size =
				 * zip_stream.read( buffer )) > 0 ) { file_stream.write( buffer,
				 * 0, size ); } file_stream.flush(); file_stream.close();
				 * ++this.context.extracted_count; }
				 */
				File new_file = new File(this.output_dir + File.separator
						+ entry.getName());

				File temp_file = new File(new_file.getParent());
				if (!temp_file.exists())
					temp_file.mkdirs();
				temp_file = null;

				if (!entry.isDirectory()) {
					FileOutputStream fos = new FileOutputStream(new_file);
					int len;
					while ((len = zip_stream.read(buffer)) > 0) {
						fos.write(buffer, 0, len);
						fos.flush();
					}
					fos.close();
					++this.context.extracted_count;
				}
				/*
				 * try { sleep(1); } catch ( Exception e ) {}
				 */

				zip_stream.closeEntry();
				entry = zip_stream.getNextEntry();
			}

			zip_stream.closeEntry();
			zip_stream.close();
		}
	}
	
	public static String getConfig(String xmlName, String key) {
		Map<String, String> configInfo = getConfigInfo(xmlName);
		if(configInfo != null && configInfo.containsKey(key))
			return configInfo.get(key);
		
		return "";
	}

	public static Map<String, String> getConfigInfo(String xmlName) {
		Map<String, String> argsMap = new HashMap<String, String>();
		try {
			InputStream xml = UnityPlayer.currentActivity.getResources().getAssets()
					.open(xmlName);
			XmlPullParser pullParser = Xml.newPullParser();
			pullParser.setInput(xml, "UTF-8");
			int event = pullParser.getEventType();
			String name = null;
			String value = null;
			while (event != 1) {
				switch (event) {
				case 2:
					if ("data".equals(pullParser.getName())) {
						name = pullParser.getAttributeValue(0);
					}
					if ("value".equals(pullParser.getName())) {
						value = pullParser.nextText();
					}
					break;
				case 3:
					if ("data".equals(pullParser.getName())) {
						argsMap.put(name, value);
					}
					break;
				}

				event = pullParser.next();
			}
		} catch (IOException e) {
			e.printStackTrace();
		} catch (Exception e) {
			e.printStackTrace();
		}

		return argsMap;
	}
	
	public static byte[] getFileData(String filePath)
	{
		Log.d("usdk", "getFileData"+filePath);		
        try {
            InputStream is = UnityPlayer.currentActivity.getResources().getAssets().open(filePath);
            int size = is.available();
            byte[] data = new byte[size];
            is.read(data);
            is.close();
           return data;
        }catch (IOException e) {
			e.printStackTrace();
		} catch (Exception e) {
			e.printStackTrace();
		}
		return new byte[0];
	}
	
	public static byte[] getStreamingAssetsFileData(String apk_dir, String filePath)
	{
		Log.d("usdk", "getStreamingAssetsFileData"+filePath);		
        try {
        	apk_dir = apk_dir.replace("jar:file:///", "").replace("!/assets", "");
			AssetManager assetMAnager = AssetManager.class.newInstance();
			AssetManager.class.getDeclaredMethod("addAssetPath", String.class).invoke(assetMAnager, apk_dir);
			Resources resources = new Resources(assetMAnager, UnityPlayer.currentActivity.getResources().getDisplayMetrics(), UnityPlayer.currentActivity.getResources().getConfiguration());
			InputStream is = resources.getAssets().open(filePath);
            int size = is.available();
            byte[] data = new byte[size];
            is.read(data);
            is.close();
           return data;
        }catch (IOException e) {
			e.printStackTrace();
		} catch (Exception e) {
			e.printStackTrace();
		}
		return new byte[0];
	}
	
//	public void openAppstoreComment(String appid) {
//		if(!appid.isEmpty())
//			this.packageName = appid;
//
//        Uri uri = Uri.parse("market://details?id=" + this.packageName);
//        Intent goToMarket = new Intent(Intent.ACTION_VIEW, uri);
//
//        goToMarket.addFlags(Intent.FLAG_ACTIVITY_NO_HISTORY |
//                Intent.FLAG_ACTIVITY_MULTIPLE_TASK);
//        try {
//        	this.mActivity.startActivity(goToMarket);
//        } catch (ActivityNotFoundException e) {
//        	this.mActivity.startActivity(new Intent(Intent.ACTION_VIEW,
//                    Uri.parse("http://play.google.com/store/apps/details?id=" + this.packageName)));
//        }
//	}
}
