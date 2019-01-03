using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
#if UNITY_XCODE_API_BUILD
using UnityEditor.iOS.Xcode;
#else
using UnityEditor.iOS.Xcode.Custom;
#endif

namespace UnityEditor.iOS.Xcode.Tests
{
    [TestFixture]
    public class ProjectCapabilityManagerTests : GenericTester
    {
        public ProjectCapabilityManagerTests() : base("ProjectCapabilityManagerTestFiles", "ProjectCapabilityManagerTestOutput", false /*true for debug*/)
        {
        }
        
        private static void ResetGuidGenerator()
        {
#if UNITY_XCODE_API_BUILD
            UnityEditor.iOS.Xcode.PBX.PBXGUID.SetGuidGenerator(LinearGuidGenerator.Generate);
#else
            UnityEditor.iOS.Xcode.Custom.PBX.PBXGUID.SetGuidGenerator(LinearGuidGenerator.Generate);
#endif            
            LinearGuidGenerator.Reset();
        }
        
        private void TestOutputProject(PBXProject proj, string testFilename)
        {
            string sourceFile = Path.Combine(GetTestSourcePath(), testFilename);
            string outputFile = Path.Combine(GetTestOutputPath(), testFilename);

            proj.WriteToFile(outputFile);
            Assert.IsTrue(TestUtils.FileContentsEqual(outputFile, sourceFile),
                          string.Format("Output not equivalent to the expected output ({0}, {1})", outputFile, sourceFile));

            PBXProject other = new PBXProject();
            other.ReadFromFile(outputFile);
            other.WriteToFile(outputFile);
            Assert.IsTrue(TestUtils.FileContentsEqual(outputFile, sourceFile));
            if (!DebugEnabled())
                File.Delete(outputFile);
        }

        private void TestOutput(string output, string testFilename)
        {
            string sourceFile = Path.Combine(GetTestSourcePath(), testFilename);
            string outputFile = Path.Combine(GetTestOutputPath(), output);

            Assert.IsTrue(TestUtils.FileContentsEqual(outputFile, sourceFile),
                          "Output not equivalent to the expected output");

            Assert.IsTrue(TestUtils.FileContentsEqual(outputFile, sourceFile));
            if (!DebugEnabled())
                File.Delete(outputFile);
        }
        
        private PBXProject ReadPBXProject()
        {
            return ReadPBXProject("base.pbxproj");
        }

        private PBXProject ReadPBXProject(string project)
        {
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(Path.Combine(GetTestSourcePath(), project)));
            return proj;
        }

        private void SetupTestProject()
        {
            if (File.Exists(Path.Combine(GetTestOutputPath(), "test.entitlements")))
                File.Delete(Path.Combine(GetTestOutputPath(), "test.entitlements"));

            if (File.Exists(Path.Combine(GetTestOutputPath(), "Info.plist")))
                File.Delete(Path.Combine(GetTestOutputPath(), "Info.plist"));

            File.Copy(Path.Combine(GetTestSourcePath(), "base_info.plist"), Path.Combine(GetTestOutputPath(), "Info.plist"));
            File.Copy(Path.Combine(GetTestSourcePath(), "base.entitlements"), Path.Combine(GetTestOutputPath(), "test.entitlements"));

            string xcodeProj = Path.Combine(GetTestOutputPath(), "Unity-iPhone.xcodeproj");
            if (Directory.Exists(xcodeProj))
                Directory.Delete(xcodeProj, true);

            Directory.CreateDirectory(xcodeProj);
            File.Copy(Path.Combine(GetTestSourcePath(), "base.pbxproj"), Path.Combine(xcodeProj, "project.pbxproj"));
        }

        [Test]
        public void AddiCloudKVWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddiCloud(true, false, false, true, new string[] {});
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_icloud.pbxproj");
            TestOutput("test.entitlements", "add_icloud_kv.entitlements");
        }

        [Test]
        public void AddiCloudDocumentWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddiCloud(false, true, false, true, new string[] {});
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_icloud.pbxproj");
            TestOutput("test.entitlements", "add_icloud_doc.entitlements");
        }

        [Test]
        public void AddiCloudDocumentCustomWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddiCloud(false, true, false, true, new string[] { "iCloud.custom.container.$(CFBundleIdentifier)" });
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_icloud.pbxproj");
            TestOutput("test.entitlements", "add_icloud_doc_custom.entitlements");
        }

        [Test]
        public void AddiCloudCloudKitWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddiCloud(false, false, true, true, new string[] { });
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_icloud_cloudkit.pbxproj");
            TestOutput("test.entitlements", "add_icloud_cloudkit.entitlements");
        }

        [Test]
        public void AddiCloudCloudKitCustomWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddiCloud(false, false, true, true, new string[] { "iCloud.custom.container.$(CFBundleIdentifier)" });
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_icloud_cloudkit.pbxproj");
            TestOutput("test.entitlements", "add_icloud_cloudkit_custom.entitlements");
        }

        [Test]
        public void AddiCloudAll()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddiCloud(true, true, true, true, new string[] { });
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_icloud_cloudkit.pbxproj");
            TestOutput("test.entitlements", "add_icloud_all.entitlements");
        }

        [Test]
        public void AddiCloudAllCustom()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddiCloud(true, true, true, true, new string[] { "iCloud.custom.container.$(CFBundleIdentifier)" });
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_icloud_cloudkit.pbxproj");
            TestOutput("test.entitlements", "add_icloud_all_custom.entitlements");
        }

        [Test]
        public void AddPushNotificationsWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddPushNotifications(true);
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_pushnotification.pbxproj");
            TestOutput("test.entitlements", "add_pushnotification.entitlements");
        }

        [Test]
        public void AddGameCenterWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddGameCenter();
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_gamecenter.pbxproj");
            TestOutput("Info.plist", "add_gamecenter.plist");
        }

        [Test]
        public void AddWalletWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddWallet(new string[] {"test1", "test2"});
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_wallet.pbxproj");
            TestOutput("test.entitlements", "add_wallet.entitlements");
        }

        [Test]
        public void AddSiriWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddSiri();
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_siri.pbxproj");
            TestOutput("test.entitlements", "add_siri.entitlements");
        }

        [Test]
        public void AddApplePayWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddApplePay(new string[] {"test1", "test2"});
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_applepay.pbxproj");
            TestOutput("test.entitlements", "add_applepay.entitlements");
        }

        [Test]
        public void AddInAppPurchaseWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddInAppPurchase();
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_iap.pbxproj");
        }

        [Test]
        public void AddMapsWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddMaps(MapsOptions.Airplane);
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_maps.pbxproj");
            TestOutput("Info.plist", "add_maps.plist");
        }

        [Test]
        public void AddPersonalVPNWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddPersonalVPN();
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_personalvpn.pbxproj");
            TestOutput("test.entitlements", "add_personalvpn.entitlements");
        }

        [Test]
        public void AddBackgroundModesWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddBackgroundModes(BackgroundModesOptions.BackgroundFetch);
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_backgroundmodes.pbxproj");
            TestOutput("Info.plist", "add_backgroundmodes.plist");
        }

        [Test]
        public void AddKeychainSharingWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddKeychainSharing(new string[] {"test1", "test2"});
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_keychainsharing.pbxproj");
            TestOutput("test.entitlements", "add_keychainsharing.entitlements");
        }

        [Test]
        public void AddInterAppAudioWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddInterAppAudio();
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_interappaudio.pbxproj");
            TestOutput("test.entitlements", "add_interappaudio.entitlements");
        }

        [Test]
        public void AddAssociatedDomainsWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddAssociatedDomains(new string[] {"webcredentials:example.com", "webcredentials:example2.com"});
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_associateddomains.pbxproj");
            TestOutput("test.entitlements", "add_associateddomains.entitlements");
        }

        [Test]
        public void AddAppGroupsWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddAppGroups(new string[] {"test1", "test2"});
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_appgroups.pbxproj");
            TestOutput("test.entitlements", "add_appgroups.entitlements");
        }

        [Test]
        public void AddHomeKitWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddHomeKit();
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_homekit.pbxproj");
            TestOutput("test.entitlements", "add_homekit.entitlements");
        }

        [Test]
        public void AddDataProtectionWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddDataProtection();
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_dataprotection.pbxproj");
            TestOutput("test.entitlements", "add_dataprotection.entitlements");
        }

        [Test]
        public void AddHealthKitWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddHealthKit();
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_healthkit.pbxproj");
            TestOutput("test.entitlements", "add_healthkit.entitlements");
            TestOutput("Info.plist", "add_healthkit.plist");
        }

        [Test]
        public void AddWirelessAccessoryConfigurationWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddWirelessAccessoryConfiguration();
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_wirelessaccessory.pbxproj");
            TestOutput("test.entitlements", "add_wirelessaccessory.entitlements");
        }

        [Test]
        public void AddMultipleCapabilitiesWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddGameCenter();
            capManager.AddInAppPurchase();
            capManager.AddMaps(MapsOptions.Airplane);
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_multiple.pbxproj");
        }

        [Test]
        public void AddMultipleCapabilitiesWithEntitlementWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(PBXProject.GetPBXProjectPath(GetTestOutputPath()), "test.entitlements", PBXProject.GetUnityTargetName());
            capManager.AddiCloud(true, false, false, true, new string[] {});
            capManager.AddApplePay(new string[] {"test1", "test2"});
            capManager.AddSiri();
            capManager.WriteToFile();

            TestOutputProject(capManager.project, "add_multiple_entitlements.pbxproj");
            TestOutput("test.entitlements", "add_multiple_entitlements.entitlements");
        }
    }
}
