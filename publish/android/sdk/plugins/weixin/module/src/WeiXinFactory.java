package com.zwwx.plugins.weixin;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;

import android.app.Activity;
import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.os.Bundle;
import android.widget.Toast;


import com.tencent.mm.opensdk.modelmsg.SendMessageToWX;
import com.tencent.mm.opensdk.modelmsg.WXImageObject;
import com.tencent.mm.opensdk.modelmsg.WXMediaMessage;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;
import com.zwwx.adapter.ProxyBase;
import com.zwwx.sdk.SDKBase;
import com.zwwx.sdk.SDKFactory;
import com.zwwx.util.Cn2Spell;
import com.zwwx.util.Content;

public class WeiXinFactory extends SDKBase implements IWeiXin {
	
	private String appID = "";
	private IWXAPI wxapi;
	
	private String shareFilePath;
	private String shareFilePathTemp;
	
	private String roleName = "xuelingjue";
	private String serverName = "xuelingjue";
	
	private static final int THUMB_SIZE = 40;// ???????????С
	
	public String getWeixinAppID()
	{
		return appID;
	}
	
	@Override
	public void onCreate(Activity activity, Bundle arg1) {
		super.onCreate(activity, arg1);
		debug("onCreate");
		
		ProxyBase proxy = SDKFactory.getProxy();	
		this.initResource("WeiXinConfig.xml");
		String packageName = mActivity.getPackageName();
		appID = this.getConfig("AppID."+packageName);
		debug("appID="+appID);
		
		shareFilePath = mActivity.getExternalFilesDir("res/ui/uiassets").getAbsolutePath()+String.format("/%s.jpg", Cn2Spell.getPinYin(Content.getApplicationName()));
		shareFilePathTemp = mActivity.getExternalFilesDir("res/ui/uiassets").getAbsolutePath()+ "/weixinshare_temp.jpg";
		
		//单独处理天拓渠道
		if( (!(proxy.getChannelEnum()).equals("1003")))
		{
			wxapi = WXAPIFactory.createWXAPI(mActivity, appID, true);
			// ????app??????
			wxapi.registerApp(appID);
		}
	}
	
	public void setWXRoleName(String name){
		roleName = name;
	}
	
	public void setWXServerName(String name){
		serverName = name;
	}
	
	private void shareBitmap2Circle(){
		Bitmap bmp = null;
		String assetPath = String.format("%s.jpg", Cn2Spell.getPinYin(Content.getApplicationName()));
		if(shareFileIsExists()){
			bmp = getBitmapFromFiles();
		}else{	
			bmp = getBitmapFromAssetsFile(assetPath);
		}
		
		if(null == bmp){
			Toast.makeText(mActivity, "没有找到路径："+shareFilePath+"------------>"+assetPath, Toast.LENGTH_SHORT).show();
			return;
		}
		
		//??????
		String roleNamePosition = getConfig("RoleNamePosition");
		String serverNamePosition = getConfig("ServerNamePosition");
		String[] arr0 = roleNamePosition.split(",");
		String[] arr1 = serverNamePosition.split(",");
		int roleNameX = Integer.parseInt(arr0[0]);
		int roleNameY = Integer.parseInt(arr0[1]);
		int serverNameX = Integer.parseInt(arr1[0]);
		int serverNameY = Integer.parseInt(arr1[1]);
		int fontSize =  Integer.parseInt(getConfig("FontSize"));
		
		//?????????????
		Bitmap bmp_temp = drawTextToBitmap(bmp, roleName, fontSize, roleNameX, roleNameY);
		bmp_temp = drawTextToBitmap(bmp_temp, serverName, fontSize, serverNameX, serverNameY);
		saveBitmapFile(bmp_temp);
		debug("bmp_temp="+bmp_temp);

		//??????????
		WXImageObject imageObject = new WXImageObject();
		imageObject.setImagePath(shareFilePathTemp);
		WXMediaMessage msg = new WXMediaMessage();
		msg.mediaObject = imageObject;
		
		//?????????
		Bitmap thumbBmp = Bitmap.createScaledBitmap(bmp_temp, THUMB_SIZE, THUMB_SIZE, true);
		msg.thumbData = bmpToByteArray(thumbBmp, true);
		debug("thumbBitmap byte count: " + thumbBmp.getByteCount());
		
		//??????????
		SendMessageToWX.Req req = new SendMessageToWX.Req();
		req.transaction = buildTransaction("weixin");
		req.message = msg;
		req.scene = SendMessageToWX.Req.WXSceneTimeline;
		boolean result = wxapi.sendReq(req);
		debug("WeiXinFactory->sendWXSceneTimeline() :  result="+result);
	}
	
	@Override
	public void sendWXSceneTimeline() {
		debug("sendWXSceneTimeline");
		mActivity.runOnUiThread(new Runnable(){
			@Override
			public void run() {
				shareBitmap2Circle();
			}});
	}
	
	@Override
	public void initWXShare(String appID) {
		// TODO Auto-generated method stub	
		ProxyBase proxy = SDKFactory.getProxy();	
		debug("appID="+appID);
		if( (!(proxy.getChannelEnum()).equals("1003")))
			return;
		
		if(wxapi!=null)
			return;
		wxapi = WXAPIFactory.createWXAPI(mActivity, appID, true);
		wxapi.registerApp(appID);
		debug("WeiXinFactory->onCreate()");
	}

	private byte[] bmpToByteArray(Bitmap bmp, final boolean needRecycle) {
		ByteArrayOutputStream baos = new ByteArrayOutputStream();
		bmp.compress(Bitmap.CompressFormat.JPEG, 100, baos);
		if (needRecycle) {
			bmp.recycle();
		}
		byte[] datas = baos.toByteArray();
		try {
			baos.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
		return datas;
	}
	
	private boolean shareFileIsExists(){
		 try{
			 File f=new File(shareFilePath);
			 if(!f.exists()){
				 return false;
			 }
		 }catch (Exception e) {
			 return false;
		 }
		 return true;
	}
	
	private Bitmap getBitmapFromAssetsFile(String fileName)
	{
	      Bitmap image = null;
	      AssetManager am = mActivity.getResources().getAssets();
	      try  
	      {  
	          InputStream is = am.open(fileName);
	          image = BitmapFactory.decodeStream(is);
	          is.close();
	      }
	      catch (IOException e)
	      {
	          e.printStackTrace();
	      }
	      return image;
	}
	
	private Bitmap getBitmapFromFiles(){
		Bitmap image = null;
		try{
			FileInputStream in = new FileInputStream(shareFilePath);
			image = BitmapFactory.decodeStream(in);
			in.close();
		}catch(IOException e){
			 e.printStackTrace();
		}
		return image;
	}
	
	private void saveBitmapFile(Bitmap bmp){
		try{
			FileOutputStream out = new FileOutputStream(shareFilePathTemp);
			bmp.compress(Bitmap.CompressFormat.JPEG, 90, out);
			out.close();
		}catch(IOException e){
			 e.printStackTrace();
		}
	}
	
	private  Bitmap drawTextToBitmap(Bitmap bitmap, String text,
	           int size, int paddingLeft, int paddingTop) {
			
	        Bitmap.Config bitmapConfig = bitmap.getConfig();
	        Paint paint = new Paint();
	        paint.setDither(true); // ?????????????????
	        paint.setFilterBitmap(true);// ?????Щ
	        paint.setColor(Color.WHITE);
	        paint.setTextSize(dp2px((float)size));
	        paint.setFakeBoldText(true);
	        if (bitmapConfig == null) {
	            bitmapConfig = android.graphics.Bitmap.Config.ARGB_8888;
	        }
	        bitmap = bitmap.copy(bitmapConfig, true);
	        Canvas canvas = new Canvas(bitmap);

	        canvas.drawText(text, paddingLeft, paddingTop, paint);
	        return bitmap;
    }
	
	private int dp2px(float dp) { 
        final float scale = mActivity.getResources().getDisplayMetrics().density; 
        return (int) (dp * scale + 0.5f); 
    } 
	
	private String buildTransaction(final String type) {
		return (type == null) ? String.valueOf(System.currentTimeMillis()) : type + System.currentTimeMillis();
	}
}
