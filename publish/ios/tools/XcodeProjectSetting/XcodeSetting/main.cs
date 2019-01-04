using System;

namespace UnityEditor.iOS.Xcode.Custom
{
    class main
    {
        public static void Main(string[] args)
        {
            string xcodePath = args[0];
            string configPath = args[1];
            Console.WriteLine("xcodePath=" + xcodePath + " configPath=" + configPath);
            XcodeSetting.OnPostprocessBuild(xcodePath, configPath);
            Console.WriteLine("********XcodeSetting success");

            //XcodeSetting.OnPostprocessBuild("E:\\Usdk\\publish\\ios\\build\\chuxinhudong", "E:/Usdk/publish/ios/build/XcodeSetting.json");
        }
    }
}
