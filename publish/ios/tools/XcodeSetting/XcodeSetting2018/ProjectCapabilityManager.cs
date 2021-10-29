// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.ProjectCapabilityManager
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.IO;

namespace UnityEditor.iOS.Xcode
{
  public class ProjectCapabilityManager
  {
    private readonly string m_BuildPath;
    private readonly string m_TargetGuid;
    private readonly string m_PBXProjectPath;
    private readonly string m_EntitlementFilePath;
    private PlistDocument m_Entitlements;
    private PlistDocument m_InfoPlist;
    protected internal PBXProject project;

    public ProjectCapabilityManager(string pbxProjectPath, string entitlementFilePath, string targetName)
    {
      this.m_BuildPath = Directory.GetParent(Path.GetDirectoryName(pbxProjectPath)).FullName;
      this.m_EntitlementFilePath = entitlementFilePath;
      this.m_PBXProjectPath = pbxProjectPath;
      this.project = new PBXProject();
      this.project.ReadFromString(File.ReadAllText(this.m_PBXProjectPath));
      this.m_TargetGuid = this.project.TargetGuidByName(targetName);
    }

    public void WriteToFile()
    {
      File.WriteAllText(this.m_PBXProjectPath, this.project.WriteToString());
      if (this.m_Entitlements != null)
        this.m_Entitlements.WriteToFile(PBXPath.Combine(this.m_BuildPath, this.m_EntitlementFilePath));
      if (this.m_InfoPlist == null)
        return;
      this.m_InfoPlist.WriteToFile(PBXPath.Combine(this.m_BuildPath, "Info.plist"));
    }

    public void AddiCloud(bool enableKeyValueStorage, bool enableiCloudDocument, string[] customContainers)
    {
      this.AddiCloud(enableKeyValueStorage, enableiCloudDocument, true, true, customContainers);
    }

    public void AddiCloud(bool enableKeyValueStorage, bool enableiCloudDocument, bool enablecloudKit, bool addDefaultContainers, string[] customContainers)
    {
      PlistDocument createEntitlementDoc = this.GetOrCreateEntitlementDoc();
      PlistElement plistElement1 = (PlistElement) new PlistElementArray();
      createEntitlementDoc.root[ICloudEntitlements.ContainerIdKey] = plistElement1;
      PlistElementArray plistElementArray1 = plistElement1 as PlistElementArray;
      PlistElementArray plistElementArray2 = (PlistElementArray) null;
      if (enableiCloudDocument || enablecloudKit)
      {
        PlistElement plistElement2 = (PlistElement) new PlistElementArray();
        createEntitlementDoc.root[ICloudEntitlements.ServicesKey] = plistElement2;
        plistElementArray2 = plistElement2 as PlistElementArray;
      }
      if (enableiCloudDocument)
      {
        plistElementArray1.values.Add((PlistElement) new PlistElementString(ICloudEntitlements.ContainerIdValue));
        plistElementArray2.values.Add((PlistElement) new PlistElementString(ICloudEntitlements.ServicesDocValue));
        PlistElement plistElement2 = (PlistElement) new PlistElementArray();
        createEntitlementDoc.root[ICloudEntitlements.UbiquityContainerIdKey] = plistElement2;
        PlistElementArray plistElementArray3 = plistElement2 as PlistElementArray;
        if (addDefaultContainers)
          plistElementArray3.values.Add((PlistElement) new PlistElementString(ICloudEntitlements.UbiquityContainerIdValue));
        if (customContainers != null && customContainers.Length > 0)
        {
          for (int index = 0; index < customContainers.Length; ++index)
            plistElementArray3.values.Add((PlistElement) new PlistElementString(customContainers[index]));
        }
      }
      if (enablecloudKit)
      {
        if (addDefaultContainers && !enableiCloudDocument)
          plistElementArray1.values.Add((PlistElement) new PlistElementString(ICloudEntitlements.ContainerIdValue));
        if (customContainers != null && customContainers.Length > 0)
        {
          for (int index = 0; index < customContainers.Length; ++index)
            plistElementArray1.values.Add((PlistElement) new PlistElementString(customContainers[index]));
        }
        plistElementArray2.values.Add((PlistElement) new PlistElementString(ICloudEntitlements.ServicesKitValue));
      }
      if (enableKeyValueStorage)
        createEntitlementDoc.root[ICloudEntitlements.KeyValueStoreKey] = (PlistElement) new PlistElementString(ICloudEntitlements.KeyValueStoreValue);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.iCloud, this.m_EntitlementFilePath, enablecloudKit);
    }

    public void AddPushNotifications(bool development)
    {
      this.GetOrCreateEntitlementDoc().root[PushNotificationEntitlements.Key] = (PlistElement) new PlistElementString(!development ? PushNotificationEntitlements.ProductionValue : PushNotificationEntitlements.DevelopmentValue);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.PushNotifications, this.m_EntitlementFilePath, false);
    }

    public void AddGameCenter()
    {
      PlistElement plistElement1 = this.GetOrCreateInfoDoc().root[GameCenterInfo.Key];
      if (plistElement1 == null)
      {
        PlistElement plistElement2 = (PlistElement) new PlistElementArray();
        this.GetOrCreateInfoDoc().root[GameCenterInfo.Key] = plistElement2;
        plistElement1 = plistElement2;
      }
      (plistElement1 as PlistElementArray).values.Add((PlistElement) new PlistElementString(GameCenterInfo.Value));
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.GameCenter, (string) null, false);
    }

    public void AddWallet(string[] passSubset)
    {
      PlistElement plistElement = (PlistElement) new PlistElementArray();
      this.GetOrCreateEntitlementDoc().root[WalletEntitlements.Key] = plistElement;
      PlistElementArray plistElementArray = plistElement as PlistElementArray;
      if ((passSubset == null || passSubset.Length == 0) && plistElementArray != null)
      {
        plistElementArray.values.Add((PlistElement) new PlistElementString(WalletEntitlements.BaseValue + WalletEntitlements.BaseValue));
      }
      else
      {
        for (int index = 0; index < passSubset.Length; ++index)
        {
          if (plistElementArray != null)
            plistElementArray.values.Add((PlistElement) new PlistElementString(WalletEntitlements.BaseValue + passSubset[index]));
        }
      }
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.Wallet, this.m_EntitlementFilePath, false);
    }

    public void AddSiri()
    {
      this.GetOrCreateEntitlementDoc().root[SiriEntitlements.Key] = (PlistElement) new PlistElementBoolean(true);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.Siri, this.m_EntitlementFilePath, false);
    }

    public void AddApplePay(string[] merchants)
    {
      PlistElement plistElement = (PlistElement) new PlistElementArray();
      this.GetOrCreateEntitlementDoc().root[ApplePayEntitlements.Key] = plistElement;
      PlistElementArray plistElementArray = plistElement as PlistElementArray;
      for (int index = 0; index < merchants.Length; ++index)
        plistElementArray.values.Add((PlistElement) new PlistElementString(merchants[index]));
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.ApplePay, this.m_EntitlementFilePath, false);
    }

    public void AddInAppPurchase()
    {
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.InAppPurchase, (string) null, false);
    }

    public void AddMaps(MapsOptions options)
    {
      PlistElement plistElement1 = this.GetOrCreateInfoDoc().root[MapsInfo.BundleKey];
      if (plistElement1 == null)
      {
        PlistElement plistElement2 = (PlistElement) new PlistElementArray();
        this.GetOrCreateInfoDoc().root[MapsInfo.BundleKey] = plistElement2;
        plistElement1 = plistElement2;
      }
      PlistElementArray root1 = plistElement1 as PlistElementArray;
      root1.values.Add((PlistElement) new PlistElementDict());
      PlistElementDict dictElementInArray = this.GetOrCreateUniqueDictElementInArray(root1);
      dictElementInArray[MapsInfo.BundleNameKey] = (PlistElement) new PlistElementString(MapsInfo.BundleNameValue);
      PlistElement plistElement3 = dictElementInArray[MapsInfo.BundleTypeKey];
      if (plistElement3 == null)
      {
        PlistElement plistElement2 = (PlistElement) new PlistElementArray();
        dictElementInArray[MapsInfo.BundleTypeKey] = plistElement2;
        plistElement3 = plistElement2;
      }
      this.GetOrCreateStringElementInArray(plistElement3 as PlistElementArray, MapsInfo.BundleTypeValue);
      PlistElement plistElement4 = this.GetOrCreateInfoDoc().root[MapsInfo.ModeKey];
      if (plistElement4 == null)
      {
        PlistElement plistElement2 = (PlistElement) new PlistElementArray();
        this.GetOrCreateInfoDoc().root[MapsInfo.ModeKey] = plistElement2;
        plistElement4 = plistElement2;
      }
      PlistElementArray root2 = plistElement4 as PlistElementArray;
      if ((options & MapsOptions.Airplane) == MapsOptions.Airplane)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModePlaneValue);
      if ((options & MapsOptions.Bike) == MapsOptions.Bike)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModeBikeValue);
      if ((options & MapsOptions.Bus) == MapsOptions.Bus)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModeBusValue);
      if ((options & MapsOptions.Car) == MapsOptions.Car)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModeCarValue);
      if ((options & MapsOptions.Ferry) == MapsOptions.Ferry)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModeFerryValue);
      if ((options & MapsOptions.Other) == MapsOptions.Other)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModeOtherValue);
      if ((options & MapsOptions.Pedestrian) == MapsOptions.Pedestrian)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModePedestrianValue);
      if ((options & MapsOptions.RideSharing) == MapsOptions.RideSharing)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModeRideShareValue);
      if ((options & MapsOptions.StreetCar) == MapsOptions.StreetCar)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModeStreetCarValue);
      if ((options & MapsOptions.Subway) == MapsOptions.Subway)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModeSubwayValue);
      if ((options & MapsOptions.Taxi) == MapsOptions.Taxi)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModeTaxiValue);
      if ((options & MapsOptions.Train) == MapsOptions.Train)
        this.GetOrCreateStringElementInArray(root2, MapsInfo.ModeTrainValue);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.Maps, (string) null, false);
    }

    public void AddPersonalVPN()
    {
      PlistElement plistElement = (PlistElement) new PlistElementArray();
      this.GetOrCreateEntitlementDoc().root[VPNEntitlements.Key] = plistElement;
      (plistElement as PlistElementArray).values.Add((PlistElement) new PlistElementString(VPNEntitlements.Value));
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.PersonalVPN, this.m_EntitlementFilePath, false);
    }

    public void AddBackgroundModes(BackgroundModesOptions options)
    {
      PlistElement plistElement1 = this.GetOrCreateInfoDoc().root[BackgroundInfo.Key];
      if (plistElement1 == null)
      {
        PlistElement plistElement2 = (PlistElement) new PlistElementArray();
        this.GetOrCreateInfoDoc().root[BackgroundInfo.Key] = plistElement2;
        plistElement1 = plistElement2;
      }
      PlistElementArray root = plistElement1 as PlistElementArray;
      if ((options & BackgroundModesOptions.ActsAsABluetoothLEAccessory) == BackgroundModesOptions.ActsAsABluetoothLEAccessory)
        this.GetOrCreateStringElementInArray(root, BackgroundInfo.ModeActsBluetoothValue);
      if ((options & BackgroundModesOptions.AudioAirplayPiP) == BackgroundModesOptions.AudioAirplayPiP)
        this.GetOrCreateStringElementInArray(root, BackgroundInfo.ModeAudioValue);
      if ((options & BackgroundModesOptions.BackgroundFetch) == BackgroundModesOptions.BackgroundFetch)
        this.GetOrCreateStringElementInArray(root, BackgroundInfo.ModeFetchValue);
      if ((options & BackgroundModesOptions.ExternalAccessoryCommunication) == BackgroundModesOptions.ExternalAccessoryCommunication)
        this.GetOrCreateStringElementInArray(root, BackgroundInfo.ModeExtAccessoryValue);
      if ((options & BackgroundModesOptions.LocationUpdates) == BackgroundModesOptions.LocationUpdates)
        this.GetOrCreateStringElementInArray(root, BackgroundInfo.ModeLocationValue);
      if ((options & BackgroundModesOptions.NewsstandDownloads) == BackgroundModesOptions.NewsstandDownloads)
        this.GetOrCreateStringElementInArray(root, BackgroundInfo.ModeNewsstandValue);
      if ((options & BackgroundModesOptions.RemoteNotifications) == BackgroundModesOptions.RemoteNotifications)
        this.GetOrCreateStringElementInArray(root, BackgroundInfo.ModePushValue);
      if ((options & BackgroundModesOptions.VoiceOverIP) == BackgroundModesOptions.VoiceOverIP)
        this.GetOrCreateStringElementInArray(root, BackgroundInfo.ModeVOIPValue);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.BackgroundModes, (string) null, false);
    }

    public void AddKeychainSharing(string[] accessGroups)
    {
      PlistElement plistElement = (PlistElement) new PlistElementArray();
      this.GetOrCreateEntitlementDoc().root[KeyChainEntitlements.Key] = plistElement;
      PlistElementArray plistElementArray = plistElement as PlistElementArray;
      if (accessGroups != null)
      {
        for (int index = 0; index < accessGroups.Length; ++index)
          plistElementArray.values.Add((PlistElement) new PlistElementString(accessGroups[index]));
      }
      else
        plistElementArray.values.Add((PlistElement) new PlistElementString(KeyChainEntitlements.DefaultValue));
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.KeychainSharing, this.m_EntitlementFilePath, false);
    }

    public void AddInterAppAudio()
    {
      this.GetOrCreateEntitlementDoc().root[AudioEntitlements.Key] = (PlistElement) new PlistElementBoolean(true);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.InterAppAudio, this.m_EntitlementFilePath, false);
    }

    public void AddAssociatedDomains(string[] domains)
    {
      PlistElement plistElement = (PlistElement) new PlistElementArray();
      this.GetOrCreateEntitlementDoc().root[AssociatedDomainsEntitlements.Key] = plistElement;
      PlistElementArray plistElementArray = plistElement as PlistElementArray;
      for (int index = 0; index < domains.Length; ++index)
        plistElementArray.values.Add((PlistElement) new PlistElementString(domains[index]));
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.AssociatedDomains, this.m_EntitlementFilePath, false);
    }

    public void AddAppGroups(string[] groups)
    {
      PlistElement plistElement = (PlistElement) new PlistElementArray();
      this.GetOrCreateEntitlementDoc().root[AppGroupsEntitlements.Key] = plistElement;
      PlistElementArray plistElementArray = plistElement as PlistElementArray;
      for (int index = 0; index < groups.Length; ++index)
        plistElementArray.values.Add((PlistElement) new PlistElementString(groups[index]));
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.AppGroups, this.m_EntitlementFilePath, false);
    }

    public void AddHomeKit()
    {
      this.GetOrCreateEntitlementDoc().root[HomeKitEntitlements.Key] = (PlistElement) new PlistElementBoolean(true);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.HomeKit, this.m_EntitlementFilePath, false);
    }

    public void AddDataProtection()
    {
      this.GetOrCreateEntitlementDoc().root[DataProtectionEntitlements.Key] = (PlistElement) new PlistElementString(DataProtectionEntitlements.Value);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.DataProtection, this.m_EntitlementFilePath, false);
    }

    public void AddHealthKit()
    {
      PlistElement plistElement1 = this.GetOrCreateInfoDoc().root[HealthInfo.Key];
      if (plistElement1 == null)
      {
        PlistElement plistElement2 = (PlistElement) new PlistElementArray();
        this.GetOrCreateInfoDoc().root[HealthInfo.Key] = plistElement2;
        plistElement1 = plistElement2;
      }
      this.GetOrCreateStringElementInArray(plistElement1 as PlistElementArray, HealthInfo.Value);
      this.GetOrCreateEntitlementDoc().root[HealthKitEntitlements.Key] = (PlistElement) new PlistElementBoolean(true);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.HealthKit, this.m_EntitlementFilePath, false);
    }

    public void AddWirelessAccessoryConfiguration()
    {
      this.GetOrCreateEntitlementDoc().root[WirelessAccessoryConfigurationEntitlements.Key] = (PlistElement) new PlistElementBoolean(true);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.WirelessAccessoryConfiguration, this.m_EntitlementFilePath, false);
    }

    public void AddAccessWiFiInformation()
    {
      this.GetOrCreateEntitlementDoc().root[AccessWiFiInformationEntitlements.Key] = (PlistElement) new PlistElementBoolean(true);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.AccessWiFiInformation, this.m_EntitlementFilePath, false);
    }

    public void AddSignInWithApple()
    {
      PlistElement plistElement = (PlistElement) new PlistElementArray();
      this.GetOrCreateEntitlementDoc().root[SignInWithAppleEntitlements.Key] = plistElement;
      (plistElement as PlistElementArray).values.Add((PlistElement) new PlistElementString(SignInWithAppleEntitlements.Value));
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.SignInWithApple, this.m_EntitlementFilePath, false);
    }

    private PlistDocument GetOrCreateEntitlementDoc()
    {
      if (this.m_Entitlements == null)
      {
        this.m_Entitlements = new PlistDocument();
        if (File.Exists(PBXPath.Combine(this.m_BuildPath, this.m_EntitlementFilePath)))
          this.m_Entitlements.ReadFromFile(PBXPath.Combine(this.m_BuildPath, this.m_EntitlementFilePath));
        else
          this.m_Entitlements.Create();
      }
      return this.m_Entitlements;
    }

    private PlistDocument GetOrCreateInfoDoc()
    {
      if (this.m_InfoPlist == null)
      {
        this.m_InfoPlist = new PlistDocument();
        string[] files = Directory.GetFiles(this.m_BuildPath + "/", "Info.plist");
        if (files.Length > 0)
          this.m_InfoPlist.ReadFromFile(files[0]);
        else
          this.m_InfoPlist.Create();
      }
      return this.m_InfoPlist;
    }

    private PlistElementString GetOrCreateStringElementInArray(PlistElementArray root, string value)
    {
      PlistElementString plistElementString = (PlistElementString) null;
      int count = root.values.Count;
      bool flag = false;
      for (int index = 0; index < count; ++index)
      {
        if (root.values[index] is PlistElementString && (root.values[index] as PlistElementString).value == value)
        {
          plistElementString = root.values[index] as PlistElementString;
          flag = true;
        }
      }
      if (!flag)
      {
        plistElementString = new PlistElementString(value);
        root.values.Add((PlistElement) plistElementString);
      }
      return plistElementString;
    }

    private PlistElementDict GetOrCreateUniqueDictElementInArray(PlistElementArray root)
    {
      PlistElementDict plistElementDict;
      if (root.values.Count == 0)
      {
        plistElementDict = root.values[0] as PlistElementDict;
      }
      else
      {
        plistElementDict = new PlistElementDict();
        root.values.Add((PlistElement) plistElementDict);
      }
      return plistElementDict;
    }
  }
}
