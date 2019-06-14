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
                object cocoaPodType = Enum.Parse (typeof (IOSResolver.CocoapodsIntegrationMethod), podType);
                if (cocoaPodType == null) {
                    Console.WriteLine ("CocoaPodType is null.{'Workspace','Project','None'}");
                    return;
                }
                IOSResolver.OnPosetProcess (xcodePath, configPath, cocoaPodType);
            }

            //XcodeSetting.OnPostprocessBuild("E:\\Usdk\\publish\\ios\\build\\chuxinhudong", "E:/Usdk/publish/ios/build/XcodeSetting.json");
        }
    }
}