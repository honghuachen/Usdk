using System.Runtime.InteropServices;
using System;

namespace Usdk
{
    public class UsdkiOSApi : IUsdkApi
    {
        [DllImport("__Internal")]
        private static extern void __CallPlugin(string pluginName, string methodName, params string[] parameters);
        public void CallPlugin(string pluginName, string methodName, params string[] parameters)
        {
            __CallPlugin(pluginName, methodName, parameters);
        }

        [DllImport("__Internal")]
        private static extern string __CallPluginR(string pluginName, string methodName, params string[] parameters);
        public R CallPlugin<R>(string pluginName, string methodName, params string[] parameters)
        {
            string ret = __CallPluginR(pluginName, methodName, parameters);
            R retR = (R)Convert.ChangeType(ret,typeof(R));
            return retR;
        }
    }
}