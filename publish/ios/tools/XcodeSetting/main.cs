using System;
using Google;

namespace UnityEditor.iOS.Xcode.Custom {
    class main {
        public static void Main (string[] args) {
            string command = args[0];
            if (command == "--pbx") {
                string xcodePath = args[1];
                string configPath = args[2];
                Console.WriteLine ("xcodePath=" + xcodePath + " configPath=" + configPath);
                XcodeSetting.OnPostprocessBuild (xcodePath, configPath);
                Console.WriteLine ("********XcodeSetting success");
            } else if (command == "--pod") {
                string xcodePath = args[1];
                string configPath = args[2];
                string podType = args[3]; //workspace,project
                try {
                    object cocoaPodType = Enum.Parse (typeof (IOSResolver.CocoapodsIntegrationMethod), podType);
                    IOSResolver.OnPosetProcess (xcodePath, configPath, cocoaPodType);
                } catch (Exception ex) {
                    Console.WriteLine (string.Format("CocoaPodType is null.['Workspace','Project','None']   msg:{0} \nStackTrace:{1}"
                    ,ex.Message,ex.StackTrace));
                }
            }
            // object cocoaPodType = Enum.Parse (typeof (IOSResolver.CocoapodsIntegrationMethod), "Project");
            // IOSResolver.OnPosetProcess("E:\\github\\usdk\\publish\\ios\\build\\baidu", "E:\\github\\usdk\\publish\\ios\\sdk\\plugins\\Bugly\\module\\CocoapodsDependencies.json", cocoaPodType);
            // //XcodeSetting.OnPostprocessBuild("E:\\Usdk\\publish\\ios\\build\\chuxinhudong", "E:/Usdk/publish/ios/build/XcodeSetting.json");
            // Console.ReadKey();
        }
    }
}