using System;
using System.Reflection;
using UnityEngine;

namespace Usdk {
    public class UsdkWindowsApi : IUsdkApi {
        public void CallPlugin (string pluginName, string methodName, params object[] parameters) {
            MethodInfo method = this.GetType ().GetMethod (methodName);
            BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            if (method != null)
                method.Invoke (this, flag, Type.DefaultBinder, parameters, null);
        }

        public R CallPlugin<R> (string pluginName, string methodName, params object[] parameters) {
            MethodInfo method = this.GetType ().GetMethod (methodName);
            BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            if (method != null)
                return (R) method.Invoke (this, flag, Type.DefaultBinder, parameters, null);
            return default (R);
        }

        private void SendCallBack (UsdkCallBackErrorCode code) {
            SendCallBack (code, null);
        }

        private void SendCallBack (UsdkCallBackErrorCode code, string ret) {
            string msg = string.Format ("errorCode={0}", (int) code);
            if (!string.IsNullOrEmpty (ret)) {
                msg = string.Format ("{0}&paramString={1}", msg, ret);
            }
            GameObject.Find ("UsdkCallBack").SendMessage ("CallBack", msg);
        }

        #region 扩展windows层接口示例（CallPlugin通过反射调用）
        private void login () {
            Debug.Log ("Invoke 'login'");
            SendCallBack (UsdkCallBackErrorCode.LoginSuccess, "login");
        }

        private void logout () {
            Debug.Log ("Invoke 'logout'");
            SendCallBack (UsdkCallBackErrorCode.LogoutFinish);
        }

        private void pay (SdkPayInfo payInfo) {
            Debug.Log ("Invoke 'pay'");
            SendCallBack (UsdkCallBackErrorCode.PaySuccess, "pay");
        }

        private void exitGame () {
            Debug.Log ("Invoke 'exitGame'");
            Application.Quit ();
        }
        #endregion
    }
}