using System.Runtime.InteropServices;
using System;

namespace Usdk
{
    public class UsdkiOSApi : IUsdkApi
    {
        [DllImport("__Internal")]
        private static extern void __CallPlugin(string pluginName, string methodName, string[] parameters);
        public void CallPlugin(string pluginName, string methodName, params object[] parameters)
        {
            string[] args = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                args[i] = parameters[i].ToString();
            __CallPlugin(pluginName, methodName, args);
        }

        [DllImport("__Internal")]
        private static extern string __CallPluginR(string pluginName, string methodName, string[] parameters);
        public R CallPlugin<R>(string pluginName, string methodName, params object[] parameters)
        {
            string[] args = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                args[i] = parameters[i].ToString();
                
            string ret = __CallPluginR(pluginName, methodName, args);
            R retR = (R)Convert.ChangeType(ret,typeof(R));
            return retR;
        }
    }
}