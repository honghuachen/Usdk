using System;
using UnityEngine;

namespace Usdk
{
    public class UsdkWindowsApi : IUsdkApi
    {
        public void CallPlugin(string pluginName, string methodName, params string[] parameters) { }

        public R CallPlugin<R>(string pluginName, string methodName, params string[] parameters)
        {
            return default(R);
        }
    }
}