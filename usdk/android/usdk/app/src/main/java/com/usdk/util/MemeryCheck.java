package com.usdk.util;

import java.io.File;
import java.text.DecimalFormat;
import java.util.*;
import android.app.*;
import android.app.ActivityManager.*;
import android.content.*;
import android.content.pm.*;
import android.os.*;

import com.unity3d.player.UnityPlayer;

public class MemeryCheck {

	@SuppressWarnings("unused")
	private static long getAvailMemeryKB() {
		final ActivityManager activityManager = (ActivityManager) UnityPlayer.currentActivity
				.getSystemService(Activity.ACTIVITY_SERVICE);
		ActivityManager.MemoryInfo info = new ActivityManager.MemoryInfo();
		activityManager.getMemoryInfo(info);
		return info.availMem >> 10;
	}

	public static long getAppMemeryKB() {
		ActivityManager mActivityManager = (ActivityManager) UnityPlayer.currentActivity
				.getSystemService(Context.ACTIVITY_SERVICE);
		ApplicationInfo ai = UnityPlayer.currentActivity.getApplicationInfo();
		List<RunningAppProcessInfo> runningAppProcessesList = mActivityManager
				.getRunningAppProcesses();
		for (RunningAppProcessInfo runningAppProcessInfo : runningAppProcessesList) {
			int pid = runningAppProcessInfo.pid;
			int[] pids = new int[] { pid };
			Debug.MemoryInfo[] memoryInfo = mActivityManager
					.getProcessMemoryInfo(pids);
			if (ai.uid == runningAppProcessInfo.uid)
				return memoryInfo[0].getTotalPrivateDirty();
		}
		return -1;
	}

	@SuppressWarnings("deprecation")
	public static long getSDCardRemainRoom() {
		String state = Environment.getExternalStorageState();
		if (Environment.MEDIA_MOUNTED.equals(state)) {
			File sdcardDir = Environment.getExternalStorageDirectory();
			StatFs sf = new StatFs(sdcardDir.getPath());
			long blockSize = sf.getBlockSize();
			// long blockCount = sf.getBlockCount();
			long availCount = sf.getAvailableBlocks();

			return availCount * blockSize;
		}

		return 0;
	}

	@SuppressWarnings("deprecation")
	public static long getSystemRemainRoom() {
		File root = Environment.getRootDirectory();
		StatFs sf = new StatFs(root.getPath());
		long blockSize = sf.getBlockSize();
		// long blockCount = sf.getBlockCount();
		long availCount = sf.getAvailableBlocks();

		return availCount * blockSize;
	}

	public static boolean sdCardExist() {
		return android.os.Environment.getExternalStorageState().equals(
				android.os.Environment.MEDIA_MOUNTED);
	}

	public static String fileSizeFormat(long fileSize) {
		String size = "";
		DecimalFormat df = new DecimalFormat("#.00");
		if (fileSize < 1024) {
			size = df.format((double) fileSize) + "BT";
		} else if (fileSize < 1024 * 1024) {
			size = df.format((double) fileSize / 1024) + "KB";
		} else if (fileSize < 1024 * 1024 * 1024) {
			size = df.format((double) fileSize / (1024 * 1024)) + "MB";
		} else if (fileSize < 1024 * 1024 * 1024 * 1024) {
			size = df.format((double) fileSize / (1024 * 1024 * 1024)) + "GB";
		} else {
			size = df.format((double) fileSize / (1024 * 1024 * 1024 * 1024))
					+ "TB";
		}
		return size;
	}
}
