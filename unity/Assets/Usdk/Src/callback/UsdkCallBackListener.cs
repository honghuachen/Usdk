using UnityEngine;

namespace Usdk
{
    public interface IUsdkCallBackListener
    {
        void OnCallBack(string callbackName, string jsonMsg);
    }

#if UNITY_ANDROID
    public class UsdkCallBackListener : AndroidJavaProxy
    {
        private IUsdkCallBackListener callback;
        public UsdkCallBackListener(IUsdkCallBackListener callback) : base("com.usdk.sdk.UsdkCallBackListener")
        {
            this.callback = callback;
        }
        
        public void OnCallBack(string callbackName, string jsonMsg)
        {
            Debug.LogFormat("[UsdkCallBackListener]callbackName={0} jsonMsg={1}", callbackName, jsonMsg);
            UnityDispatcher.PostTask(() =>
            {
                if (this.callback != null)
                {
                    Debug.LogFormat("[callback]callbackName={0} jsonMsg={1}", callbackName, jsonMsg);
                    this.callback.OnCallBack(callbackName, jsonMsg);
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
        
        public void OnCallBack(string callbackName, string jsonMsg)
        {
            Debug.LogFormat("[UsdkCallBackListener]callbackName={0} jsonMsg={1}", callbackName, jsonMsg);
            UnityDispatcher.PostTask(() =>
            {
                if (this.callback != null)
                {
                    Debug.LogFormat("[callback]callbackName={0} jsonMsg={1}", callbackName, jsonMsg);
                    this.callback.OnCallBack(callbackName, jsonMsg);
                }
            });
        }
    }
#endif
}