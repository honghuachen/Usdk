using System;
using System.Reflection;
using UnityEngine;

namespace Usdk {
    public class UsdkWindowsApi : IUsdkApi {
        private object Invoke (object obj, Type type, string method, params object[] objects) {
            MethodInfo methodInfo = type.GetMethod (method,
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public);
            if (methodInfo == null)
                return null;

            return methodInfo.Invoke (obj, objects);
        }

        public void CallPlugin (string pluginName, string methodName, params object[] parameters) {
            Invoke (this, this.GetType (), methodName, parameters);
        }

        public R CallPlugin<R> (string pluginName, string methodName, params object[] parameters) {
            return (R) Invoke (this, this.GetType (), methodName, parameters);
        }

        public bool IsExistPlugin (string pluginName) {
            return true;
        }

        public bool isExistField (string pluginName, string fieldName) {
            return true;
        }

        public bool isExistMethod (string pluginName, string methodName) {
            return true;
        }

        public void SetCallBack(UsdkCallBackListener callback) { 
        }
    }
}