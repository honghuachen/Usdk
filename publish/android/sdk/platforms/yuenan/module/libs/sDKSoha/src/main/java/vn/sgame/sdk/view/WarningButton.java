package vn.sgame.sdk.view;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.dialogs.DialogProfile;
import vn.soha.game.sdk.utils.PhotoLoader;
import vn.soha.game.sdk.utils.Utils;
import android.app.Activity;
import android.content.Context;
import android.graphics.PixelFormat;
import android.graphics.Rect;
import android.os.Build;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnTouchListener;
import android.view.Window;
import android.view.WindowManager;
import android.view.WindowManager.LayoutParams;
import android.view.animation.Animation;
import android.view.animation.OvershootInterpolator;
import android.view.animation.Transformation;
import android.widget.ImageView;
import android.widget.RelativeLayout;

public class WarningButton {
	Activity mActivity;
	View supportView;
	View notificationView;
	RelativeLayout rlContainer;
	ImageView ivIcon;

	long timeTouchDown;

	//int buttonSize = 100;
	int buttonSizeWidth=0;
	int buttonSizeHeight=0;
	int textSummaryHeight = 40;
	int textSummaryMargin = 3;
	int textSummaryWidth = 250;

	int screenWidth, screenHeight;
	float deltaX, deltaY;
	float density;

	int timeRequestNotify = 60 * 1000;

	boolean isDestroyed = false;
	boolean isPendingShow = true;

	int mStatusBarHeight;
	int mPositionX = 0;
	int mPositionY = 0;

	int currentX = 0;
	int currentY = 0;
	int nextX = 0;
	int nextY = 0;

	public DialogProfile dialogProfile;

	public WarningButton(Activity activity) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		isPendingShow = true;
		density = mActivity.getResources().getDisplayMetrics().density;

		buttonSizeWidth =Utils.getWarningWidth(mActivity);
		buttonSizeHeight=Utils.getWarningHeight(mActivity);
				buttonSizeWidth = (int) (buttonSizeWidth * density);
				buttonSizeHeight = (int) (buttonSizeHeight * density);
		textSummaryHeight = (int) (textSummaryHeight * density);
		textSummaryMargin = (int) (textSummaryMargin * density);
		textSummaryWidth = (int) (textSummaryWidth * density);

		screenWidth = mActivity.getResources().getDisplayMetrics().widthPixels;
		screenHeight = mActivity.getResources().getDisplayMetrics().heightPixels;

		mPositionX = Utils.getWarningButtonPositionX(mActivity);
		mPositionY = Utils.getWarningButtonPositionY(mActivity);

		getStatusBarHeight();
		initView();
		attachView();
	}



	public void getStatusBarHeight() {
		Rect rectangle= new Rect();
		Window window= mActivity.getWindow();
		window.getDecorView().getWindowVisibleDisplayFrame(rectangle);
		int statusBarHeight= rectangle.top;
		mStatusBarHeight = statusBarHeight;
	}	

	@SuppressWarnings("deprecation")
	public void initView() {
		supportView = LayoutInflater.from(mActivity).inflate(R.layout.warning_button, null);
		ivIcon = (ImageView) supportView.findViewById(R.id.ivIcon);
		//rlContainer = (RelativeLayout) supportView.findViewById(R.id.rl_container);
		
		android.widget.RelativeLayout.LayoutParams layoutParam = (android.widget.RelativeLayout.LayoutParams) ivIcon.getLayoutParams();
		layoutParam.width = buttonSizeWidth;
		layoutParam.height=buttonSizeHeight;
		
		//ivIcon.setAlpha(125);
		supportView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				// TODO Auto-generated method stub
				if (event.getAction() == MotionEvent.ACTION_DOWN) {
					//ivIcon.setAlpha(255);
					timeTouchDown = System.currentTimeMillis();
					deltaX = event.getX();
					deltaY = event.getY();
				} else if (event.getAction() == MotionEvent.ACTION_MOVE) {
					//ivIcon.setAlpha(255);
					float marginLeft = event.getRawX() - deltaX;
					float marginTop = event.getRawY() - deltaY;
					marginTop = marginTop - mStatusBarHeight;

					if (marginLeft < 0) {
						marginLeft = 0;
					}
					if (marginLeft > screenWidth - buttonSizeWidth) {
						marginLeft = screenWidth - buttonSizeWidth;
					}

					if (marginTop < 0) {
						marginTop = 0;
					}
					if (marginTop > screenHeight - buttonSizeHeight) {
						marginTop = screenHeight - buttonSizeHeight;
					}

					WindowManager.LayoutParams lp = (WindowManager.LayoutParams) supportView.getLayoutParams();
					lp.x = (int) marginLeft;
					lp.y = (int) marginTop;

					supportView.setLayoutParams(lp);

					WindowManager windowManager = mActivity.getWindowManager();
					windowManager.updateViewLayout(supportView, lp);

					if (dialogProfile != null) {
						dialogProfile = null;
					}
				} else if (event.getAction() == MotionEvent.ACTION_UP) {
					if (System.currentTimeMillis() - timeTouchDown < 110) {
						if (dialogProfile == null) {
							// moveToTop();
							hide();
							if(!DialogProfile.isShowWarningDetail)
							{
								dialogProfile = new DialogProfile(mActivity, WarningButton.this);
							}
							
							// ivIcon.setAlpha(255);
						} else {
							dialogProfile = null;
							// moveToCurrent();
							show();
						}
					} else {
						moveToEdge();
					}
				}

				return true;
			}
		});
	}

	public void attachView() {		
		PhotoLoader.loadPhotoOrigin(Utils.getWarningImageAge(mActivity), ivIcon, mActivity,
				new PhotoLoader.OnImageLoaderListener() {

			@Override
			public void onImageLoaded() {
				// TODO Auto-generated method stub

				if(Build.VERSION.SDK_INT<19  || (Build.VERSION.SDK_INT >= 23 && Build.VERSION.SDK_INT <= 25)){
					WindowManager windowManager = (WindowManager) mActivity.getApplicationContext().getSystemService(Activity.WINDOW_SERVICE);
					WindowManager.LayoutParams params = new WindowManager.LayoutParams(WindowManager.LayoutParams.WRAP_CONTENT,
							WindowManager.LayoutParams.WRAP_CONTENT,
							WindowManager.LayoutParams.TYPE_PHONE,
							WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
							PixelFormat.TRANSPARENT);
					params.gravity = Gravity.LEFT | Gravity.TOP;
					params.x = mPositionX;
					params.y = mPositionY;
					windowManager.addView(supportView, params);
				}else
				if(Build.VERSION.SDK_INT>=19 && Build.VERSION.SDK_INT < 23){
					WindowManager windowManager = (WindowManager) mActivity.getApplicationContext().getSystemService(Activity.WINDOW_SERVICE);
					WindowManager.LayoutParams params = new WindowManager.LayoutParams(WindowManager.LayoutParams.WRAP_CONTENT,
							WindowManager.LayoutParams.WRAP_CONTENT,
							WindowManager.LayoutParams.TYPE_TOAST,
							WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
							PixelFormat.TRANSPARENT);
					params.gravity = Gravity.LEFT | Gravity.TOP;
					params.x = mPositionX;
					params.y = mPositionY;
					windowManager.addView(supportView, params);
				} else {
					WindowManager windowManager = (WindowManager) mActivity.getApplicationContext().getSystemService(Activity.WINDOW_SERVICE);
					WindowManager.LayoutParams params = new WindowManager.LayoutParams(WindowManager.LayoutParams.WRAP_CONTENT,
							WindowManager.LayoutParams.WRAP_CONTENT,
							WindowManager.LayoutParams.TYPE_APPLICATION_OVERLAY,
							WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
							PixelFormat.TRANSPARENT);
					params.gravity = Gravity.LEFT | Gravity.TOP;
					params.x = mPositionX;
					params.y = mPositionY;
					windowManager.addView(supportView, params);
				}
//				WindowManager windowManager = (WindowManager) mActivity.getApplicationContext().getSystemService(Activity.WINDOW_SERVICE);
//				WindowManager.LayoutParams params = new WindowManager.LayoutParams(WindowManager.LayoutParams.WRAP_CONTENT,
//						WindowManager.LayoutParams.WRAP_CONTENT,
//						WindowManager.LayoutParams.TYPE_TOAST,
//						WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE,
//						PixelFormat.TRANSPARENT);
//				params.gravity = Gravity.LEFT | Gravity.TOP;
//				params.x = mPositionX;
//				params.y = mPositionY;
//				windowManager.addView(supportView, params);
			}
		});
	}

	@SuppressWarnings("deprecation")
	public void moveToEdge() {
		final WindowManager windowManager = (WindowManager) mActivity.getApplicationContext().getSystemService(Context.WINDOW_SERVICE);

		Log.e("stk", "movetoEdgeeeeeeeeeeeeeeeeeee");
		WindowManager.LayoutParams lp = (LayoutParams) supportView.getLayoutParams();
		final int marginLeft = lp.x;
		final int marginTop = lp.y;
		int marginRight = (int) (screenWidth - buttonSizeWidth - lp.x);

		Animation animation = null;

		if (marginLeft >= marginRight) {
			animation = new Animation() {
				@Override
				protected void applyTransformation(float interpolatedTime, Transformation t) {
					// TODO Auto-generated method stub
					super.applyTransformation(interpolatedTime, t);
					((WindowManager.LayoutParams)supportView.getLayoutParams()).x = (int) (marginLeft + (screenWidth-buttonSizeWidth-marginLeft) * interpolatedTime);
					windowManager.updateViewLayout(supportView, supportView.getLayoutParams());

					int newPositionX = (int) marginLeft + (screenWidth-buttonSizeWidth-marginLeft);
					int newPositionY = (int) screenHeight-buttonSizeHeight-(screenHeight - buttonSizeHeight - marginTop);
					Utils.vSaveWarningButtonPosition(mActivity, newPositionX, newPositionY);
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
					int newPositionY = (int) screenHeight-buttonSizeHeight-(screenHeight - buttonSizeHeight - marginTop);
					Utils.vSaveWarningButtonPosition(mActivity, newPositionX, newPositionY);
				}
			};			
		}

		animation.setDuration(1000);
		animation.setInterpolator(new OvershootInterpolator());
		supportView.findViewById(R.id.rl_container).startAnimation(animation);
		//ivIcon.setAlpha(125);
	}

/*	public void moveToTop() {
		currentX = ((WindowManager.LayoutParams)supportView.getLayoutParams()).x;
		currentY = ((WindowManager.LayoutParams)supportView.getLayoutParams()).y;

		nextX = (int) (screenWidth/2 - buttonSize/2);
		nextY = 10;

		Animation animation = new Animation() {
			@Override
			protected void applyTransformation(float interpolatedTime, Transformation t) {
				// TODO Auto-generated method stub
				super.applyTransformation(interpolatedTime, t);
				((WindowManager.LayoutParams)supportView.getLayoutParams()).x = (int) (currentX + (nextX - currentX) * interpolatedTime);
				((WindowManager.LayoutParams)supportView.getLayoutParams()).y = (int) (currentY + (nextY - currentY) * interpolatedTime);
				((WindowManager)mActivity.getApplicationContext().getSystemService(Context.WINDOW_SERVICE)).updateViewLayout(supportView, supportView.getLayoutParams());
			}
		};

		animation.setDuration(1000);
		animation.setInterpolator(new OvershootInterpolator());
		supportView.findViewById(R.id.rl_container).startAnimation(animation);
	}*/

	@SuppressWarnings("deprecation")
	public void moveToCurrent() {
		dialogProfile = null;

		Animation animation = new Animation() {
			@Override
			protected void applyTransformation(float interpolatedTime, Transformation t) {
				// TODO Auto-generated method stub
				super.applyTransformation(interpolatedTime, t);
				((WindowManager.LayoutParams)supportView.getLayoutParams()).x = (int) (nextX + (currentX - nextX) * interpolatedTime);
				((WindowManager.LayoutParams)supportView.getLayoutParams()).y = (int) (nextY + (currentY - nextY) * interpolatedTime);
				((WindowManager)mActivity.getApplicationContext().getSystemService(Context.WINDOW_SERVICE)).updateViewLayout(supportView, supportView.getLayoutParams());
			}
		};

		animation.setDuration(1000);
		animation.setInterpolator(new OvershootInterpolator());
		supportView.findViewById(R.id.rl_container).startAnimation(animation);		
		//ivIcon.setAlpha(125);
	}

	@SuppressWarnings("deprecation")
	public void vSetAlphaCryStal() {
		//ivIcon.setAlpha(125);
	}

	public void hide() {
		Log.e("", "__w hide warning button3");
		supportView.setVisibility(View.GONE);
	}

	public void show() {
		Log.e("", "__w show warning button4");
		supportView.setVisibility(View.VISIBLE);
	}

	public void setPendingShow(boolean pendingShow) {
		isPendingShow = pendingShow;
	}

	public boolean getPendingShow() {
		return isPendingShow;
	}

	public void onDestroy() {
		isDestroyed = true;
		Log.e("", "__w hide warning button5");
		supportView.setVisibility(View.GONE);
	}

}
