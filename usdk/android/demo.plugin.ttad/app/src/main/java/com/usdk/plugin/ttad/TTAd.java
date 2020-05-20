package com.usdk.plugin.ttad;

import android.app.Activity;
import android.os.Bundle;
import android.widget.Toast;

import com.bytedance.sdk.openadsdk.AdSlot;
import com.bytedance.sdk.openadsdk.TTAdConfig;
import com.bytedance.sdk.openadsdk.TTAdConstant;
import com.bytedance.sdk.openadsdk.TTAdNative;
import com.bytedance.sdk.openadsdk.TTAdSdk;
import com.bytedance.sdk.openadsdk.TTAppDownloadListener;
import com.bytedance.sdk.openadsdk.TTRewardVideoAd;
import com.usdk.sdk.UsdkBase;

public class TTAd extends UsdkBase {
    private TTAdNative mTTAdNative;
    private Activity mActivity;
    private TTRewardVideoAd mttRewardVideoAd;

    @Override
    protected void OnCreate(Activity activity, Bundle savedInstanceState) {
        super.OnCreate(activity, savedInstanceState);
        this.mActivity = activity;
        TTAdSdk.getAdManager().requestPermissionIfNecessary(activity);

        //强烈建议在应用对应的Application#onCreate()方法中调用，避免出现content为null的异常
        TTAdSdk.init(activity,
                new TTAdConfig.Builder()
                        .appId("5001121")
                        .useTextureView(false) //使用TextureView控件播放视频,默认为SurfaceView,当有SurfaceView冲突的场景，可以使用TextureView
                        .appName("APP测试媒体")
                        .titleBarTheme(TTAdConstant.TITLE_BAR_THEME_DARK)
                        .allowShowNotify(true) //是否允许sdk展示通知栏提示
                        .allowShowPageWhenScreenLock(true) //是否在锁屏场景支持展示广告落地页
                        .debug(true) //测试阶段打开，可以通过日志排查问题，上线时去除该调用
                        .directDownloadNetworkType(TTAdConstant.NETWORK_STATE_WIFI, TTAdConstant.NETWORK_STATE_4G) //允许直接下载的网络状态集合
                        .supportMultiProcess(false) //是否支持多进程，true支持
                        //.httpStack(new MyOkStack3())//自定义网络库，demo中给出了okhttp3版本的样例，其余请自行开发或者咨询工作人员。
                        .build());
        mTTAdNative = TTAdSdk.getAdManager().createAdNative(activity);
    }

    public void loadRewardVideo(String codeId, final TTAdNative.RewardVideoAdListener rewardVideoAdListener, final TTRewardVideoAd.RewardAdInteractionListener rewardAdInteractionListener, final TTAppDownloadListener downloadListener) {
        AdSlot adSlot = new AdSlot.Builder()
                .setCodeId(codeId)
                .setSupportDeepLink(true)
                .setAdCount(2)
                .setImageAcceptedSize(1080, 1920)
                .setRewardName("金币") //奖励的名称
                .setRewardAmount(3)   //奖励的数量
                //必传参数，表来标识应用侧唯一用户；若非服务器回调模式或不需sdk透传
                //可设置为空字符串
                .setUserID("")
                .setOrientation(TTAdConstant.VERTICAL)  //设置期望视频播放的方向，为TTAdConstant.HORIZONTAL或TTAdConstant.VERTICAL
//                .setMediaExtra("media_extra") //用户透传的信息，可不传
                .build();
        mTTAdNative.loadRewardVideoAd(adSlot, new TTAdNative.RewardVideoAdListener() {
            @Override
            public void onError(int code, String message) {
                Toast.makeText(mActivity, message, Toast.LENGTH_SHORT).show();
                rewardVideoAdListener.onError(code, message);
            }

            //视频广告加载后的视频文件资源缓存到本地的回调
            @Override
            public void onRewardVideoCached() {
                Toast.makeText(mActivity, "rewardVideoAd video cached", Toast.LENGTH_SHORT).show();
                rewardVideoAdListener.onRewardVideoCached();
            }

            //视频广告素材加载到，如title,视频url等，不包括视频文件
            @Override
            public void onRewardVideoAdLoad(TTRewardVideoAd ad) {
                Toast.makeText(mActivity, "rewardVideoAd loaded", Toast.LENGTH_SHORT).show();
                rewardVideoAdListener.onRewardVideoAdLoad(ad);
                mttRewardVideoAd = ad;
                //mttRewardVideoAd.setShowDownLoadBar(false);
                mttRewardVideoAd.setRewardAdInteractionListener(new TTRewardVideoAd.RewardAdInteractionListener() {
                                                                    @Override
                                                                    public void onAdShow() {
                                                                        Toast.makeText(mActivity, "rewardVideoAd show", Toast.LENGTH_SHORT).show();
                                                                        rewardAdInteractionListener.onAdShow();
                                                                    }

                                                                    @Override
                                                                    public void onAdVideoBarClick() {
                                                                        Toast.makeText(mActivity, "rewardVideoAd bar click", Toast.LENGTH_SHORT).show();
                                                                        rewardAdInteractionListener.onAdVideoBarClick();
                                                                    }

                                                                    @Override
                                                                    public void onAdClose() {
                                                                        Toast.makeText(mActivity, "rewardVideoAd close", Toast.LENGTH_SHORT).show();
                                                                        rewardAdInteractionListener.onAdClose();
                                                                    }

                                                                    @Override
                                                                    public void onVideoComplete() {
                                                                        Toast.makeText(mActivity, "rewardVideoAd complete", Toast.LENGTH_SHORT).show();
                                                                        rewardAdInteractionListener.onVideoComplete();
                                                                    }

                                                                    @Override
                                                                    public void onVideoError() {
                                                                        rewardAdInteractionListener.onVideoError();
                                                                    }

                                                                    @Override
                                                                    public void onRewardVerify(boolean rewardVerify, int rewardAmount, String rewardName) {
                                                                        Toast.makeText(mActivity, "verify:" + rewardVerify + " amount:" + rewardAmount +
                                                                                        " name:" + rewardName,
                                                                                Toast.LENGTH_SHORT).show();
                                                                        rewardAdInteractionListener.onRewardVerify(rewardVerify, rewardAmount, rewardName);
                                                                    }

                                                                    @Override
                                                                    public void onSkippedVideo() {
                                                                        rewardAdInteractionListener.onSkippedVideo();
                                                                    }
                                                                }

                );
                mttRewardVideoAd.setDownloadListener(new TTAppDownloadListener() {
                    @Override
                    public void onIdle() {
                        downloadListener.onIdle();
                    }

                    @Override
                    public void onDownloadActive(long totalBytes, long currBytes, String fileName, String appName) {
                        downloadListener.onDownloadActive(totalBytes, currBytes, fileName, appName);
                    }

                    @Override
                    public void onDownloadPaused(long totalBytes, long currBytes, String fileName, String appName) {
                        downloadListener.onDownloadPaused(totalBytes, currBytes, fileName, appName);
                    }

                    @Override
                    public void onDownloadFailed(long totalBytes, long currBytes, String fileName, String appName) {
                        downloadListener.onDownloadFailed(totalBytes, currBytes, fileName, appName);
                    }

                    @Override
                    public void onDownloadFinished(long totalBytes, String fileName, String appName) {
                        downloadListener.onDownloadFinished(totalBytes, fileName, appName);
                    }

                    @Override
                    public void onInstalled(String fileName, String appName) {
                        downloadListener.onInstalled(fileName, appName);
                    }
                });
            }
        });
    }
}
