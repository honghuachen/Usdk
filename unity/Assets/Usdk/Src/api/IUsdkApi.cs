using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Usdk
{
    public interface IUsdkApi
    {
        void CallPlugin(string pluginName, string methodName, params object[] parameters);
        R CallPlugin<R>(string pluginName, string methodName, params object[] parameters);
        bool IsExistPlugin(string pluginName);
        bool isExistField(string pluginName, string fieldName);
        bool isExistMethod(string pluginName, string methodName);
        void SetCallBack(UsdkCallBackListener callback);
    }
}