using System;

namespace UnityEditor.iOS.Xcode.Custom
{
    class main
    {
        public static void Main(string[] args)
        {
            string xcodePath = args[0];
            string pluginPath = args[1];
            Console.WriteLine("xcodePath=" + xcodePath + " pluginPath=" + pluginPath);
            XcodeSetting.OnPostprocessBuild(xcodePath, pluginPath);
            Console.WriteLine("********XcodeSetting success");

            //XcodeSetting.OnPostprocessBuild("E:/Usdk/publish/ios/xcode/chuxinhudong", "E:/Usdk/publish/ios/sdk/platforms/chuxinhudong");
        }
    }
}
