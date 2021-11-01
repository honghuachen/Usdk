using UnityEngine;

namespace Usdk
{
    public interface IUsdkCallBackListener
    {
        void OnCallBack(string eventName, string jsonMsg);
    }

    public class UsdkCallBack : IUsdkCallBackListener
    {
        public void OnCallBack(string eventName, string msg)
        {
        }
    }

#if UNITY_ANDROID
    public class UsdkCallBackListener : AndroidJavaProxy
    {
        private IUsdkCallBackListener callback;
        public UsdkCallBackListener(IUsdkCallBackListener callback) : base("com.usdk.sdk.UsdkCallBackListener")
        {
            this.callback = callback;
        }

        public void OnCallBack(string eventName, string msg)
        {
            Debug.LogFormat("[UsdkCallBackListener]eventName={0} jsonMsg={1}", eventName, msg);
            UnityDispatcher.PostTask(() =>
            {
                if (this.callback != null)
                {
                    this.callback.OnCallBack(eventName, msg);
                }
            });
        }
    }
#else
    public class UsdkCallBackListener
    {
        private IUsdkCallBackListener callback;
        public UsdkCallBackListener(IUsdkCallBackListener callback)
        {
            this.callback = callback;
        }

        public void OnCallBack(string eventName, string msg)
        {
            Debug.LogFormat("[UsdkCallBackListener]eventName={0} jsonMsg={1}", eventName, msg);
            UnityDispatcher.PostTask(() =>
            {
                if (this.callback != null)
                {
                    this.callback.OnCallBack(eventName, msg);
                }
            });
        }
    }
#endif
}