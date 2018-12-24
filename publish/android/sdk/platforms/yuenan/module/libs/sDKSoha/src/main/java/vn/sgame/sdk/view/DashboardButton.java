package vn.sgame.sdk.view;

import java.util.ArrayList;
import java.util.List;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONArray;
import org.json.JSONObject;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.SohaApplication;
import vn.soha.game.sdk.SohaSDK;
import vn.soha.game.sdk.dialogs.DialogDashboardClose;
import vn.soha.game.sdk.dialogs.DialogDashboardConfirmClose;
import vn.soha.game.sdk.dialogs.DialogDashboardNew;
import vn.soha.game.sdk.interact.InteractConfirmCloseDashBoard;
import vn.soha.game.sdk.model.DashBoardItem;
import vn.soha.game.sdk.server.API;
import vn.soha.game.sdk.utils.JsonParser;
import vn.soha.game.sdk.utils.ScreenUtils;
import vn.soha.game.sdk.utils.Utils;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Context;
import android.graphics.PixelFormat;
import android.os.Build;
import android.os.Handler;
import android.util.Log;
import android.util.TypedValue;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnTouchListener;
import android.view.WindowManager;
import android.view.WindowManager.LayoutParams;
import android.view.animation.AlphaAnimation;
import android.view.animation.Animation;
import android.view.animation.Animation.AnimationListener;
import android.view.animation.OvershootInterpolator;
import android.view.animation.Transformation;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

@TargetApi(23)
public class DashboardButton {
	Activity mActivity;
	View supportView;
	View notificationView;
	ImageView ivIcon;
	
	

	long timeTouchDown;

	int buttonSize = 50;
	int textSummaryHeight = 40;
	int textSummaryMargin = 3;
	int textSummaryWidth = 250;

	int screenWidth, screenHeight;
	float deltaX, deltaY;
	float density;

	int timeRequestNotify = 60 * 10000;

	boolean isDestroyed = false;
	boolean isPendingShow = false;
	boolean isShowOption = false;
	
	//boolean isHideHaftOfButon = true;

	int mStatusBarHeight;
	int mPositionX = 0;
	int mPositionY = 0;

	int currentX = 0;
	int currentY = 0;
	int nextX = 0;
	int nextY = 0;
	int totalNotify=0;
    int baseHideTime =3000;
	
	RelativeLayout rlSupportButton;
	List<DashBoardItem> listItem;
	
	TextView tvNotify;

	Boolean checkShowDashBoash =true;
	
	public static Boolean checkToShowAuto =false;
	public static Boolean isPendingClick =false;
	
	
	public Boolean getCheckShowDashBoash() {
		return checkShowDashBoash;
	}


	public void setCheckShowDashBoash(Boolean checkShowDashBoash) {
		Log.e("Check Show", "___"+checkShowDashBoash+"");
		this.checkShowDashBoash = checkShowDashBoash;
	}


	public DashboardButton(Activity activity, List<DashBoardItem> listItem,int total) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		this.listItem = listItem;
		density = mActivity.getResources().getDisplayMetrics().density;
		buttonSize =  (int) mActivity.getResources().getDimension(R.dimen.dashboardSize);
		textSummaryHeight = (int) (textSummaryHeight * density);
		textSummaryMargin = (int) (textSummaryMargin * density);
		textSummaryWidth = (int) (textSummaryWidth * density);

		screenWidth = mActivity.getResources().getDisplayMetrics().widthPixels;
		screenHeight = mActivity.getResources().getDisplayMetrics().heightPixels;

		mPositionX = Utils.getMySohaButtonPositionX(mActivity);
		mPositionY = Utils.getMySohaButtonPositionY(mActivity);

		//getStatusBarHeight();
		initView();
		attachView();
		totalNotify =total;

		
		getNotify(mActivity,true);
		
		hideToEdge(1);
		
		
	/*	if(checkToShowAuto)
		{
			for(int i =0;i<listItem.size();i++)
			{
				if(listItem.get(i).getAutoOpen()==1)
				{
					DialogDetailDashBoard dialog = new DialogDetailDashBoard(mActivity, listItem.get(i).getUrl());
					SohaSDK.hideDashboard();
					SohaSDK.fuckLog(MQTTUtils.ACTION_OPEN_DB, "");
					SohaSDK.sendLog(MQTTUtils.ACTION_OPEN_DB, "");
					SohaApplication.getInstance().trackEvent("sdk", MQTTUtils.ACTION_OPEN_DB, "");
					break;
				}
			}
			
			checkToShowAuto = false;
		}*/
	}

	
	void updateDashBoard()
	{
		
	}
	public DashboardButton(Activity activity, int positionX, int positionY) {
		// TODO Auto-generated constructor stub
		mPositionX = (int) (positionX * activity.getResources()
				.getDisplayMetrics().density);
		mPositionY = (int) (positionY * activity.getResources()
				.getDisplayMetrics().density);

		mActivity = activity;
		density = mActivity.getResources().getDisplayMetrics().density;
		buttonSize = (int) (buttonSize * density);
		textSummaryHeight = (int) (textSummaryHeight * density);
		textSummaryMargin = (int) (textSummaryMargin * density);
		textSummaryWidth = (int) (textSummaryWidth * density);

		screenWidth = mActivity.getResources().getDisplayMetrics().widthPixels;
		screenHeight = mActivity.getResources().getDisplayMetrics().heightPixels;
		mStatusBarHeight  = (int) mActivity.getResources().getDimension(R.dimen.statusHeght);
		
		//getStatusBarHeight();
		initView();
		//attachView();
		getNotify(mActivity,true);
	}

/*	public void getStatusBarHeight() {
		Rect rectangle = new Rect();
		Window window = mActivity.getWindow();
		window.getDecorView().getWindowVisibleDisplayFrame(rectangle);
		int statusBarHeight = rectangle.top;
		mStatusBarHeight = statusBarHeight;
		Toast.makeText(mActivity, "status height"+ mStatusBarHeight+"",1000).show();
	}
*/
	int oldY =0;
	DialogDashboardClose closeDialog ;
	@SuppressWarnings("deprecation")
	public void initView() {
		supportView = LayoutInflater.from(mActivity).inflate(
				R.layout.support_button, null);
		ivIcon = (ImageView) supportView.findViewById(R.id.ivIcon);
	
		rlSupportButton  = (RelativeLayout) supportView.findViewById(R.id.rl_support_button);
		tvNotify = (TextView) supportView.findViewById(R.id.tvNotify);
		ivIcon.setAlpha(255);
		//ivIcon.setAlpha(125);

        Log.e("iconDB", "__icdb "+Utils.getIconDB(mActivity));
		Picasso.with(mActivity).load(Utils.getIconDB(mActivity)).into(ivIcon);
	
		rlSupportButton.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				// TODO Auto-generated method stub

               /* updateTextViewPosition(totalNotify);
				ivIcon.setVisibility(View.VISIBLE);
				imgIconLeft.setVisibility(View.GONE);
				imgIconRight.setVisibility(View.GONE);*/
				if (event.getAction() == MotionEvent.ACTION_DOWN) {
					
					screenWidth = mActivity.getResources().getDisplayMetrics().widthPixels;
					screenHeight = mActivity.getResources().getDisplayMetrics().heightPixels;
					//isHideHaftOfButon =false;
					timeTouchDown = System.currentTimeMillis();
					if(!isShowOption)
					{
						//ivIcon.setAlpha(255);
						
						WindowManager.LayoutParams lp = (LayoutParams) supportView.getLayoutParams();
						oldY = lp.y;
						deltaX = event.getX();
						deltaY = event.getY();
						
						
					}
					
				} else if (event.getAction() == MotionEvent.ACTION_MOVE) {
					if(!isShowOption)
					{
					//	ivIcon.setAlpha(255);
						float marginLeft = event.getRawX() - deltaX;
						float marginTop = event.getRawY() - deltaY;
					
						marginTop = marginTop - mStatusBarHeight;

						if (marginLeft < 0) {
							marginLeft = 0;
						}
						if (marginLeft > screenWidth - buttonSize) {
							marginLeft = screenWidth - buttonSize;
						}

						if (marginTop < 0) {
							marginTop = 0;
						}
						mStatusBarHeight =(int) mActivity.getResources().getDimension(R.dimen.statusHeght);
						
						if (marginTop > screenHeight - buttonSize-mStatusBarHeight) {
							marginTop = screenHeight - buttonSize-mStatusBarHeight;
						}
						Log.e("margin top ", marginTop +"___"+(screenHeight - buttonSize)+"__"+(screenHeight - buttonSize-mStatusBarHeight)+"___st:"+mStatusBarHeight+" ____new ");
					

						WindowManager.LayoutParams lp = (WindowManager.LayoutParams) supportView
								.getLayoutParams();
						lp.x = (int) marginLeft;
						lp.y = (int) marginTop;

						supportView.setLayoutParams(lp);

						WindowManager windowManager = mActivity.getWindowManager();
						windowManager.updateViewLayout(supportView, lp);
						if(closeDialog==null)
						{
							
							closeDialog = new DialogDashboardClose(mActivity);
							
						}
						else
						{
							closeDialog.show();
						}
						
					}
					
					if(checkHideDashBoard())
					{
					closeDialog.showFull();
					}
					else
					{
						closeDialog.showSmall();
					}
					
				} else if (event.getAction() == MotionEvent.ACTION_UP) {
					
					if(closeDialog!=null&&closeDialog.isShowing())
					{
						closeDialog.hide();
					}
					
					if(!checkHideDashBoard())
					{
						
						
						if (System.currentTimeMillis() - timeTouchDown < 110&&!isPendingClick) {
							isPendingClick =true;
							hide();
							h.postDelayed(new Runnable() {
								
								@Override
								public void run() {
									// TODO Auto-generated method stub
									
									if(listItem!=null)
									{
										new DialogDashboardNew(mActivity,listItem);
									}
									
									WindowManager.LayoutParams lp = (WindowManager.LayoutParams) supportView
											.getLayoutParams();
									lp.y =oldY;
									WindowManager windowManager = mActivity.getWindowManager();
									windowManager.updateViewLayout(supportView, lp);
								}
							}, 100);
							
							
							

						} else {
							
							
							if(!isShowOption)
							{
							//	setPositionLeft();
							moveToEdge();
							}
						}
						
						if(!isShowOption)
						{
							
							hideToEdge(baseHideTime);
						}
						
					}
					else
					{
						SohaSDK.hideDashboard();
						DialogDashboardConfirmClose dialog = new DialogDashboardConfirmClose(mActivity, new InteractConfirmCloseDashBoard() {
							
							@Override
							public void onCancel() {
								// TODO Auto-generated method stub
								checkShowDashBoash =true;
								SohaSDK.showDashboard(mActivity);
								moveToEdge();
							}
							
							@Override
							public void onAccept() {
								// TODO Auto-generated method stub
								isUserCloseDasBoard =true;
								SohaSDK.hideDashboard();
								try {
									SohaSDK.fuckLog("remove_db", "");
									SohaSDK.sendLog("remove_db", "");
									SohaApplication.getInstance().trackEvent("sdk", "remove_db", "");
								/*	SohaSDK.sendLog(MQTTHelper.REMOVE_DB, "");
									SohaSDK.fuckLog(MQTTHelper.REMOVE_DB, "");
									SohaApplication.getInstance().trackEvent(mActivity, "sdk", MQTTHelper.REMOVE_DB, "");*/
								} catch (Exception e) {
									// TODO: handle exception
									e.printStackTrace();
								}
								
							}
						});
						dialog.show();
						
					}
					
					
					
				}

				return true;
			}
		});
	}
	
public	static Boolean isUserCloseDasBoard = false;

	boolean checkHideDashBoard()
	{
		WindowManager.LayoutParams lp = (WindowManager.LayoutParams) supportView
				.getLayoutParams();
		
		int screenWidth = ScreenUtils.getScreenWidth(mActivity);
		int screenHeight = ScreenUtils.getScreenHeight(mActivity);
		int closeButonSize =Utils.dpToPx(mActivity, 15);
		int verticalSpace = Utils.dpToPx(mActivity, 60);
		
		int minWidth = screenWidth/2 -closeButonSize-Utils.dpToPx(mActivity,50 );
		int maxWidth = screenWidth/2 +closeButonSize;
		int minHeight = screenHeight -verticalSpace-Utils.dpToPx(mActivity,50 );
		int maxheight =screenHeight -verticalSpace+closeButonSize;//+closeButonSize;
		if( minWidth <lp.x&&lp.x<maxWidth)
		{
			Log.e("With ok ","___________widt h ok");
			if( minHeight <lp.y&&lp.y<maxheight)
			{
				//Toast.makeText(mActivity, "hehehe", 1000).show();
				
				Log.e("isUserCloseDasBoard ","___________isUserCloseDasBoard ok");
				//isUserCloseDasBoard =true;
				return true;
			}
			else
			{
				Log.e("isUserCloseDasBoard ","___________isUserCloseDasBoard FALSE");
				//isUserCloseDasBoard =false;
			}
		}
	
		return false;
	}
	
	void updateTextViewPosition(int total) {


		if(checkIsRight())
		{
			RelativeLayout.LayoutParams optionParam = (RelativeLayout.LayoutParams) tvNotify.getLayoutParams();
			if(total>=10)
			{
			//	optionParam.setMargins(left, top, right, bottom)
				//optionParam.setMargins(0, 0, buttonSize*5/3, 0);
				optionParam.setMargins(0, 0, buttonSize/2, 0);
			}
			else
			{
				//optionParam.setMargins(0, 0, buttonSize*5/4, 0);
				optionParam.setMargins(0, 0, (buttonSize*7)/10, 0);
			}
			
			tvNotify.requestLayout();
		}
		else
		{
			RelativeLayout.LayoutParams optionParam = (RelativeLayout.LayoutParams) tvNotify.getLayoutParams();
			if(total>=10)
			{
			optionParam.setMargins(buttonSize/2, 0, 0, 0);
			}else
			{
				optionParam.setMargins((buttonSize*7)/10, 0, 0, 0);
			}
			tvNotify.requestLayout();
		}

	}
/*	void updateTextViewPosition(int total)
	{
		
		if(checkIsRight())
		{
			RelativeLayout.LayoutParams optionParam = (RelativeLayout.LayoutParams) tvNotify.getLayoutParams();
			if(total>=10)
			{
				
				optionParam.setMargins(0, 0, buttonSize*5/3, 0);
			}
			else
			{
				optionParam.setMargins(0, 0, buttonSize*5/4, 0);
			}
			
			tvNotify.requestLayout();
		}
		else
		{
			RelativeLayout.LayoutParams optionParam = (RelativeLayout.LayoutParams) tvNotify.getLayoutParams();
			if(total>=10)
			{
			optionParam.setMargins(buttonSize*5/3, 0, 0, 0);
			}else
			{
				optionParam.setMargins(buttonSize*5/4, 0, 0, 0);
			}
			tvNotify.requestLayout();
		}

	}*/
	
	void updateTextViewPositionForHide(int total)
	{
		if(checkIsRight())
		{
			RelativeLayout.LayoutParams optionParam = (RelativeLayout.LayoutParams) tvNotify.getLayoutParams();
			if(total>=10)
			{
				optionParam.setMargins(0, 0, Utils.dpToPx(mActivity, 15), 0);
			}
			else
			{
				optionParam.setMargins(0, 0, Utils.dpToPx(mActivity, 20), 0);
			}
			
			tvNotify.requestLayout();
		}
		else
		{
			RelativeLayout.LayoutParams optionParam = (RelativeLayout.LayoutParams) tvNotify.getLayoutParams();
			if(total>=10)
			{
			optionParam.setMargins(Utils.dpToPx(mActivity, 15), 0, 0, 0);
			}else
			{
				optionParam.setMargins(Utils.dpToPx(mActivity, 20), 0, 0, 0);
			}
			tvNotify.requestLayout();
		}

	}
	void setDefaultLayoutParam()
	{

		WindowManager.LayoutParams oldLayoutParam = (WindowManager.LayoutParams) supportView
				.getLayoutParams();

		WindowManager.LayoutParams newLayoutParam;
		if (Build.VERSION.SDK_INT < 19
				|| (Build.VERSION.SDK_INT >= 23 && Build.VERSION.SDK_INT <= 25)) {
			newLayoutParam = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_PHONE,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
					PixelFormat.TRANSPARENT);

		} else if (Build.VERSION.SDK_INT >= 19 && Build.VERSION.SDK_INT < 23) {
			newLayoutParam = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_TOAST,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
					PixelFormat.TRANSPARENT);
		} else {
			newLayoutParam = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_APPLICATION_OVERLAY,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
					PixelFormat.TRANSPARENT);
		}
	
		
		
		
		newLayoutParam.gravity = Gravity.LEFT | Gravity.TOP;
		
		newLayoutParam.y = oldLayoutParam.y;
		newLayoutParam.x = oldLayoutParam.x;
		
		supportView.setLayoutParams(newLayoutParam);

		WindowManager windowManager = mActivity.getWindowManager();
		windowManager.updateViewLayout(supportView, newLayoutParam);
	}
	void hideToEdge(final int time)
	{
	///	Toast.makeText(mActivity, "call hive option", 1000).show();
		//isHideHaftOfButon=true;
		h.postDelayed(new Runnable() {
			
			@Override
			public void run() {
				
				if(!isShowOption)
				{
					
                  //  updateTextViewPositionForHide(totalNotify);
					WindowManager.LayoutParams oldLayoutParam = (WindowManager.LayoutParams) supportView
							.getLayoutParams();

					WindowManager.LayoutParams newLayoutParam ;
					if (Build.VERSION.SDK_INT < 19
							|| (Build.VERSION.SDK_INT >= 23 && Build.VERSION.SDK_INT <= 25)) {
						newLayoutParam = new WindowManager.LayoutParams(
								WindowManager.LayoutParams.WRAP_CONTENT,
								WindowManager.LayoutParams.WRAP_CONTENT,
								WindowManager.LayoutParams.TYPE_PHONE,
								WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
								PixelFormat.TRANSPARENT);

					} else if (Build.VERSION.SDK_INT >= 19
							&& Build.VERSION.SDK_INT < 23) {
						newLayoutParam = new WindowManager.LayoutParams(
								WindowManager.LayoutParams.WRAP_CONTENT,
								WindowManager.LayoutParams.WRAP_CONTENT,
								WindowManager.LayoutParams.TYPE_TOAST,
								WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
								PixelFormat.TRANSPARENT);
					} else {
						newLayoutParam = new WindowManager.LayoutParams(
								WindowManager.LayoutParams.WRAP_CONTENT,
								WindowManager.LayoutParams.WRAP_CONTENT,
								WindowManager.LayoutParams.TYPE_APPLICATION_OVERLAY,
								WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
								PixelFormat.TRANSPARENT);
					}


					newLayoutParam.gravity = Gravity.LEFT | Gravity.TOP;
					
					newLayoutParam.y = oldLayoutParam.y;
					
					if(checkIsRight())
					{
						
						/*imgIconRight.setVisibility(View.VISIBLE);
						ivIcon.setVisibility(View.GONE);*/
						newLayoutParam.x = ScreenUtils.getScreenWidth(mActivity)-buttonSize/2;
					Log.e("Screnn check ", "screen with "+ScreenUtils.getScreenWidth(mActivity)+" old position "+oldLayoutParam.x+" 30dp:"+Utils.dpToPx(mActivity, 30));
						//newLayoutParam.x = oldLayoutParam.x +Utils.dpToPx(mActivity, 30);
						

						supportView.setLayoutParams(newLayoutParam);

						WindowManager windowManager = mActivity.getWindowManager();
						windowManager.updateViewLayout(supportView, newLayoutParam);
					}
					else
					{
						/*imgIconLeft.setVisibility(View.VISIBLE);
						ivIcon.setVisibility(View.GONE);*/
						//int position = oldLayoutParam.x -Utils.dpToPx(mActivity, 30);
						

						//newLayoutParam.x = oldLayoutParam.x -Utils.dpToPx(mActivity, 30);
						newLayoutParam.x = 0 -buttonSize/2;
					

						supportView.setLayoutParams(newLayoutParam);

						WindowManager windowManager = mActivity.getWindowManager();
						windowManager.updateViewLayout(supportView, newLayoutParam);
					}
					//isHideHaftOfButon = false;
				}
				//ivIcon.setAlpha(125);
			}
		}, time);
		
	}
	
	void setPositionLeft()
	{
		WindowManager.LayoutParams lp = (WindowManager.LayoutParams) supportView
				.getLayoutParams();
		lp.x = lp.x -Utils.dpToPx(mActivity, 30);
		

		supportView.setLayoutParams(lp);

		WindowManager windowManager = mActivity.getWindowManager();
		windowManager.updateViewLayout(supportView, lp);
	}
/*	public Boolean someMethod() {
		if (Build.VERSION.SDK_INT >= 23) {
			// ducnm note_ can bo comment
			 if (!Settings.canDrawOverlays(mActivity)) {
			 Intent intent = new
			 Intent(Settings.ACTION_MANAGE_OVERLAY_PERMISSION,
			 Uri.parse("package:" + mActivity.getPackageName()));
			 mActivity.startActivityForResult(intent,
			 OVERLAY_PERMISSION_REQ_CODE);
			 return false;
			 }
			// ducnm note_end
			return true;
		}
		return true;

	}*/

	public static int OVERLAY_PERMISSION_REQ_CODE = 1234;

	

	
Boolean checkIsRight()
{
	final WindowManager windowManager = (WindowManager) mActivity
			.getApplicationContext().getSystemService(
					Context.WINDOW_SERVICE);

	
	WindowManager.LayoutParams lp = (LayoutParams) supportView
			.getLayoutParams();
	final int marginLeft = lp.x;
	
	int marginRight = (int) (screenWidth - buttonSize - lp.x);


	if (marginLeft >= marginRight) {
		return true;
		
	}
	return false;
	
}

public void attachView() {
	try{
		WindowManager windowManager = (WindowManager) mActivity.getApplicationContext().getSystemService(Activity.WINDOW_SERVICE);
		WindowManager.LayoutParams params;

		if (Build.VERSION.SDK_INT < 19
				|| (Build.VERSION.SDK_INT >= 23 && Build.VERSION.SDK_INT <= 25)) {
			params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_PHONE,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
					PixelFormat.TRANSPARENT);

		} else if (Build.VERSION.SDK_INT >= 19
				&& Build.VERSION.SDK_INT < 23) {
			params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_TOAST,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
					PixelFormat.TRANSPARENT);
		} else{
			params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_APPLICATION_OVERLAY,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
					PixelFormat.TRANSPARENT);
		}
				
				
				
		params.gravity = Gravity.LEFT | Gravity.TOP;
		params.x = mPositionX;
		params.y = mPositionY;
		
		try{
			windowManager.removeView(supportView);
		}catch(Exception e){
		    e.printStackTrace();
		}
		windowManager.addView(supportView, params);
	}catch(Exception ex)
	{
		ex.printStackTrace();
	}

}
public void updateNotify(final Context context) {
	
	
	 new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				
			
			try {
				Thread.sleep(30*1000);
				
				List<NameValuePair> params = new ArrayList<NameValuePair>();
				params.add(new BasicNameValuePair("signed_request", Utils.getDefaultParamsPost(mActivity)));
				Log.e("param", Utils.getDefaultParamsPost(mActivity));
				final JSONObject jsonData = JsonParser.getJSONFromPostUrl(API.getDashboardConfig, params);
				if(jsonData!=null)
				{
					String status = jsonData.getString("status");
					Log.e("sdk dashborad",jsonData.toString());
					if(status.equalsIgnoreCase("success"))
					{
						final int newDBVer = Utils.getNewDashBoardVersion(mActivity);
						Utils.vSaveDashboardData(mActivity, jsonData.toString());
						Utils.vSaveCurrDashboardVersion(mActivity, newDBVer);
						initDashBoardFromData(jsonData,mActivity);
						SohaSDK.cacheDashboardData = jsonData;
					}
				}
				else
				{
					if(SohaSDK.cacheDashboardData!=null)
					{
						initDashBoardFromData(jsonData, mActivity);
					}
				}
				
				
				/*new Handler().postDelayed(new Runnable() {
					@Override
					public void run() {
						// TODO Auto-generated method stub
						Log.e("Call get notify", "___get notify_0");
						if (!isDestroyed) {
							Log.e("Call get notify", "___get notify_0");
							getNotify();
						}
					}
				}, timeRequestNotify);*/
				
			} catch (Exception e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
			}
		}).start();
}
public void getNotify(final Context context,final Boolean isMutil) {
	Log.e("Call get notify", "___get notify");
	 new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				List<NameValuePair> params = new ArrayList<NameValuePair>();
				params.add(new BasicNameValuePair("signed_request", Utils.getDefaultParamsPost(mActivity)));
				Log.e("param", Utils.getDefaultParamsPost(mActivity));
				final JSONObject jsonData = JsonParser.getJSONFromPostUrl(API.getDashboardConfig, params);
			
			try {
				if(jsonData!=null)
				{
					String status = jsonData.getString("status");
					Log.e("sdk dashborad",jsonData.toString());
					if(status.equalsIgnoreCase("success"))
					{
						final int newDBVer = Utils.getNewDashBoardVersion(mActivity);
						Utils.vSaveDashboardData(mActivity, jsonData.toString());
						Utils.vSaveCurrDashboardVersion(mActivity, newDBVer);
						initDashBoardFromData(jsonData,mActivity);
					}
				}
			
				Thread.sleep(timeRequestNotify);
				Log.e("Call get notify", "___get notify_2");

				if(isMutil)
				{
					getNotify(mActivity,true);
				}
				
				/*new Handler().postDelayed(new Runnable() {
					@Override
					public void run() {
						// TODO Auto-generated method stub
						Log.e("Call get notify", "___get notify_0");
						if (!isDestroyed) {
							Log.e("Call get notify", "___get notify_0");
							getNotify();
						}
					}
				}, timeRequestNotify);*/
				
			} catch (Exception e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
			}
		}).start();
}



 void  initDashBoardFromData(JSONObject jsonData,final Activity activity)
{
	final List<DashBoardItem> listItem = new ArrayList<DashBoardItem>();
	try {
		int total =0;
				JSONArray data = jsonData.getJSONArray("data");
				for(int i =0;i<data.length();i++)
				{
					JSONObject item = data.getJSONObject(i);
					DashBoardItem temp = new DashBoardItem();
					temp.setTab(item.getInt("tab"));
					temp.setTitle(item.getString("title"));
					temp.setType(item.getString("type"));
					temp.setIcon(item.getString("icon"));
					temp.setUrl(item.getString("url"));
					total+=item.getInt("notify");
					temp.setNotify(item.getInt("notify"));
					if(temp.getTab()==1)
					{
						JSONArray tablist = item.getJSONArray("data");
						for(int j=0;j<tablist.length();j++)
						{
							JSONObject subItem = tablist.getJSONObject(j);
							DashBoardItem subTemp = new DashBoardItem();
							subTemp.setTitle(subItem.getString("title"));
							subTemp.setType(subItem.getString("type"));
							subTemp.setIcon(subItem.getString("icon"));
							subTemp.setUrl(subItem.getString("url"));
							temp.getListSubTab().add(subTemp);
						}
					}
					try {
						temp.setActive(item.getInt("active"));
						temp.setMessageActive(item.getString("mess_active"));
						temp.setAutoOpen(item.getInt("auto_open"));
						
			
							temp.setPageId(item.getString("id_page"));
						} catch (Exception e) {
							// TODO: handle exception
						}
					listItem.add(temp);
				}	
				
				this.listItem.clear();
                this.listItem.addAll(listItem);
				totalNotify = total;
				activity.runOnUiThread(new Runnable() {
					
					@Override
					public void run() {
						// TODO Auto-generated method stub
						Log.e("total notify ","__"+totalNotify+"");
					
						if(totalNotify>0)
						{
							updateTextViewPosition(totalNotify);
							supportView.findViewById(R.id.tvNotify).setVisibility(View.VISIBLE);
							((TextView)supportView.findViewById(R.id.tvNotify)).setText(totalNotify + "");
						}
						else
						{
							supportView.findViewById(R.id.tvNotify).setVisibility(View.GONE);
						}
                    
					}
				});
	} catch (Exception e) {
		// TODO Auto-generated catch block
		e.printStackTrace();
	}
	
}
//
@SuppressWarnings("deprecation")
public void moveToEdge() {
	final WindowManager windowManager = (WindowManager) mActivity.getApplicationContext().getSystemService(Context.WINDOW_SERVICE);

	
	WindowManager.LayoutParams lp = (LayoutParams) supportView.getLayoutParams();
	final int marginLeft = lp.x;
	final int marginTop = lp.y;

	Log.e("stk", "movetoEdgeeeeeeeeeeeeeeeeeee____________y "+marginTop);
	int marginRight = (int) (screenWidth - buttonSize - lp.x);

	Animation animation = null;
	updateTextViewPosition(totalNotify);
	if (marginLeft >= marginRight) {
		animation = new Animation() {
			@Override
			protected void applyTransformation(float interpolatedTime, Transformation t) {
				// TODO Auto-generated method stub
				super.applyTransformation(interpolatedTime, t);
				((WindowManager.LayoutParams)supportView.getLayoutParams()).x = (int) (marginLeft + (screenWidth-buttonSize-marginLeft) * interpolatedTime);
				windowManager.updateViewLayout(supportView, supportView.getLayoutParams());

				int newPositionX = (int) marginLeft + (screenWidth-buttonSize-marginLeft);
				int newPositionY =(int) screenHeight-buttonSize-(screenHeight - buttonSize - marginTop);
				Utils.vSaveMySohaButtonPosition(mActivity, newPositionX, newPositionY);
			}
		};
	} else {
		animation = new Animation() {
			@Override
			protected void applyTransformation(float interpolatedTime, Transformation t) {
				// TODO Auto-generated method stub
				super.applyTransformation(interpolatedTime, t);
				((WindowManager.LayoutParams)supportView.getLayoutParams()).x = (int) (marginLeft - marginLeft * interpolatedTime);
				windowManager.updateViewLayout(supportView, supportView.getLayoutParams());

				int newPositionX = (int) (marginLeft - marginLeft);
				int newPositionY = screenHeight-buttonSize-(screenHeight - buttonSize - marginTop);
				Utils.vSaveMySohaButtonPosition(mActivity, newPositionX, newPositionY);
			}
		};			
	}

	animation.setDuration(1000);
	animation.setInterpolator(new OvershootInterpolator());
	supportView.findViewById(R.id.rl_support_button).startAnimation(animation);
//	ivIcon.setAlpha(125);
}
//
//public void moveToBottomLeft() {
//	currentX = ((WindowManager.LayoutParams)supportView.getLayoutParams()).x;
//	currentY = ((WindowManager.LayoutParams)supportView.getLayoutParams()).y;
//
//	nextX = dp2px(12);
//	nextY = ScreenUtils.getScreenHeight(mActivity) - (dp2px(10) * 6);
//
//	Animation animation = new Animation() {
//		@Override
//		protected void applyTransformation(float interpolatedTime, Transformation t) {
//			// TODO Auto-generated method stub
//			super.applyTransformation(interpolatedTime, t);
//			((WindowManager.LayoutParams)supportView.getLayoutParams()).x = (int) (currentX + (nextX - currentX) * interpolatedTime);
//			((WindowManager.LayoutParams)supportView.getLayoutParams()).y = (int) (currentY + (nextY - currentY) * interpolatedTime);
//			((WindowManager)mActivity.getApplicationContext().getSystemService(Context.WINDOW_SERVICE)).updateViewLayout(supportView, supportView.getLayoutParams());
//		}
//	};
//
//	animation.setDuration(1000);
//	animation.setInterpolator(new OvershootInterpolator());
//	supportView.findViewById(R.id.rl_support_button).startAnimation(animation);
//}
//
//public void moveTo(int x, int y) {
//	currentX = ((WindowManager.LayoutParams)supportView.getLayoutParams()).x;
//	currentY = ((WindowManager.LayoutParams)supportView.getLayoutParams()).y;
//
//	nextX = x;
//	nextY = y;
//
//	Animation animation = new Animation() {
//		@Override
//		protected void applyTransformation(float interpolatedTime, Transformation t) {
//			// TODO Auto-generated method stub
//			super.applyTransformation(interpolatedTime, t);
//			((WindowManager.LayoutParams)supportView.getLayoutParams()).x = (int) (currentX + (nextX - currentX) * interpolatedTime);
//			((WindowManager.LayoutParams)supportView.getLayoutParams()).y = (int) (currentY + (nextY - currentY) * interpolatedTime);
//			((WindowManager)mActivity.getApplicationContext().getSystemService(Context.WINDOW_SERVICE)).updateViewLayout(supportView, supportView.getLayoutParams());
//		}
//	};
//
//	animation.setDuration(1000);
//	animation.setInterpolator(new OvershootInterpolator());
//	supportView.findViewById(R.id.rl_support_button).startAnimation(animation);
//}
//
//@SuppressWarnings("deprecation")
//public void moveToCurrent() {
//	currentDialogMySoha = null;
//
//	Animation animation = new Animation() {
//		@Override
//		protected void applyTransformation(float interpolatedTime, Transformation t) {
//			// TODO Auto-generated method stub
//			super.applyTransformation(interpolatedTime, t);
//			((WindowManager.LayoutParams)supportView.getLayoutParams()).x = (int) (nextX + (currentX - nextX) * interpolatedTime);
//			((WindowManager.LayoutParams)supportView.getLayoutParams()).y = (int) (nextY + (currentY - nextY) * interpolatedTime);
//			((WindowManager)mActivity.getApplicationContext().getSystemService(Context.WINDOW_SERVICE)).updateViewLayout(supportView, supportView.getLayoutParams());
//		}
//	};
//
//	animation.setDuration(1000);
//	animation.setInterpolator(new OvershootInterpolator());
//	supportView.findViewById(R.id.rl_support_button).startAnimation(animation);		
//	ivIcon.setAlpha(125);
//}

public void showSummaryNotify(final String title) {
	final FrameLayout summaryNotify = (FrameLayout) LayoutInflater.from(mActivity).inflate(R.layout.row_summary_notify, null);
	((TextView)summaryNotify.findViewById(R.id.tv_summary)).setText(title);
	WindowManager.LayoutParams params;
	if (Build.VERSION.SDK_INT < 19
			|| (Build.VERSION.SDK_INT >= 23 && Build.VERSION.SDK_INT <= 25)) {
		params = new WindowManager.LayoutParams(
				WindowManager.LayoutParams.WRAP_CONTENT,
				WindowManager.LayoutParams.WRAP_CONTENT,
				WindowManager.LayoutParams.TYPE_PHONE,
				WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
				PixelFormat.TRANSPARENT);

	} else if (Build.VERSION.SDK_INT >= 19 && Build.VERSION.SDK_INT < 23) {
		params = new WindowManager.LayoutParams(
				WindowManager.LayoutParams.WRAP_CONTENT,
				WindowManager.LayoutParams.WRAP_CONTENT,
				WindowManager.LayoutParams.TYPE_TOAST,
				WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
				PixelFormat.TRANSPARENT);
	} else 		{
		params = new WindowManager.LayoutParams(
				WindowManager.LayoutParams.WRAP_CONTENT,
				WindowManager.LayoutParams.WRAP_CONTENT,
				WindowManager.LayoutParams.TYPE_APPLICATION_OVERLAY,
				WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
				PixelFormat.TRANSPARENT);
	}
	params.gravity = Gravity.LEFT | Gravity.TOP;

	int supportButtonX = ((WindowManager.LayoutParams)supportView.getLayoutParams()).x;
	int supportButtonY = ((WindowManager.LayoutParams)supportView.getLayoutParams()).y;

	if (supportButtonX == 0) {
		params.x = buttonSize + textSummaryMargin;
		params.y = supportButtonY + buttonSize/2 - textSummaryHeight/2;
		summaryNotify.findViewById(R.id.tv_summary).setBackgroundResource(R.drawable.bg_summary_left);
	} else if (supportButtonX == screenWidth - buttonSize) {
		params.x = supportButtonX - textSummaryWidth - textSummaryMargin;
		params.y = supportButtonY + buttonSize/2 - textSummaryHeight/2;
		summaryNotify.findViewById(R.id.tv_summary).setBackgroundResource(R.drawable.bg_summary_right);
	} else {

		return;
	}

	final WindowManager windowManager = (WindowManager)mActivity.getApplicationContext().getSystemService(Context.WINDOW_SERVICE);
	windowManager.addView(summaryNotify, params);

	AlphaAnimation alphaAnimation = new AlphaAnimation(1.0f, 0.3f);
	alphaAnimation.setDuration(2000);
	alphaAnimation.setAnimationListener(new AnimationListener() {
		@Override
		public void onAnimationStart(Animation animation) {}

		@Override
		public void onAnimationRepeat(Animation animation) {}

		@Override
		public void onAnimationEnd(Animation animation) {
			// TODO Auto-generated method stub
			windowManager.removeView(summaryNotify);
		}
	});

	summaryNotify.findViewById(R.id.tv_summary).startAnimation(alphaAnimation);

}


	public void hide() {

		supportView.setVisibility(View.GONE);
		//hideOption();
	}

	public void show() {
		try {
			SohaSDK.isHidedDashBoard =false;
			// ducnm kiem tra neu dashboard ddag hien thi ko show sohabutton
			if(!isUserCloseDasBoard&&checkShowDashBoash)
			{
				h.postDelayed(new Runnable() {
					
					@Override
					public void run() {
						// TODO Auto-generated method stub
						if(!SohaSDK.isHidedDashBoard)
						{
							getNotify(mActivity,false);
							supportView.setVisibility(View.VISIBLE);
							hideToEdge(1);
						}
						
					}
				}, 500);
				
			
			}
			
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}

	}
	Handler h  = new Handler();
	

	public void setPendingShow(boolean pendingShow) {
		Log.e("Pending show", "__"+pendingShow);
		isPendingShow = pendingShow;
	}

	public boolean getPendingShow() {
		return isPendingShow;
	}

	public void onDestroy() {
		isDestroyed = true;
		supportView.setVisibility(View.GONE);
	}

	private int dp2px(int dp) {
		return (int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, dp,
				mActivity.getResources().getDisplayMetrics());
	}

}
