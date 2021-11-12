// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.ProjectCapabilityManager
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
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

    public ProjectCapabilityManager(string pbxProjectPath, string entitlementFilePath, string targetName = null, string targetGuid = null)
    {
      this.m_BuildPath = Directory.GetParent(Path.GetDirectoryName(pbxProjectPath)).FullName;
      this.m_EntitlementFilePath = entitlementFilePath;
      this.m_PBXProjectPath = pbxProjectPath;
      this.project = new PBXProject();
      this.project.ReadFromString(File.ReadAllText(this.m_PBXProjectPath));
      if (targetName == "Unity-iPhone")
      {
        targetName = (string) null;
        targetGuid = this.project.GetUnityMainTargetGuid();
      }
      if (targetName == null && targetGuid == null || targetName != null && targetGuid != null)
        throw new Exception(string.Format("Invalid targets please specify only one of them targetName: {0} and targetGuid: {1} ", (object) targetName, (object) targetGuid));
      if (targetName != null)
      {
        this.m_TargetGuid = this.project.TargetGuidByName(targetName);
        if (this.m_TargetGuid == null)
          throw new Exception(string.Format("Could not find target: {0} in {1}.", (object) targetName, (object) pbxProjectPath));
      }
      else
        this.m_TargetGuid = targetGuid;
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
      PlistElementArray plistElementArray1 = (createEntitlementDoc.root[ICloudEntitlements.ContainerIdKey] = (PlistElement) new PlistElementArray()) as PlistElementArray;
      PlistElementArray plistElementArray2 = (PlistElementArray) null;
      if (enableiCloudDocument | enablecloudKit)
        plistElementArray2 = (createEntitlementDoc.root[ICloudEntitlements.ServicesKey] = (PlistElement) new PlistElementArray()) as PlistElementArray;
      if (enableiCloudDocument)
      {
        plistElementArray1.values.Add((PlistElement) new PlistElementString(ICloudEntitlements.ContainerIdValue));
        plistElementArray2.values.Add((PlistElement) new PlistElementString(ICloudEntitlements.ServicesDocValue));
        PlistElementArray plistElementArray3 = (createEntitlementDoc.root[ICloudEntitlements.UbiquityContainerIdKey] = (PlistElement) new PlistElementArray()) as PlistElementArray;
        if (addDefaultContainers)
          plistElementArray3.values.Add((PlistElement) new PlistElementString(ICloudEntitlements.UbiquityContainerIdValue));
        if (customContainers != null && (uint) customContainers.Length > 0U)
        {
          for (int index = 0; index < customContainers.Length; ++index)
            plistElementArray3.values.Add((PlistElement) new PlistElementString(customContainers[index]));
        }
      }
      if (enablecloudKit)
      {
        if (addDefaultContainers && !enableiCloudDocument)
          plistElementArray1.values.Add((PlistElement) new PlistElementString(ICloudEntitlements.ContainerIdValue));
        if (customContainers != null && (uint) customContainers.Length > 0U)
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
      this.GetOrCreateEntitlementDoc().root[PushNotificationEntitlements.Key] = (PlistElement) new PlistElementString(development ? PushNotificationEntitlements.DevelopmentValue : PushNotificationEntitlements.ProductionValue);
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.PushNotifications, this.m_EntitlementFilePath, false);
    }

    public void AddGameCenter()
    {
      ((this.GetOrCreateInfoDoc().root[GameCenterInfo.Key] ?? (this.GetOrCreateInfoDoc().root[GameCenterInfo.Key] = (PlistElement) new PlistElementArray())) as PlistElementArray).values.Add((PlistElement) new PlistElementString(GameCenterInfo.Value));
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.GameCenter, (string) null, false);
    }

    public void AddWallet(string[] passSubset)
    {
      PlistElementArray plistElementArray = (this.GetOrCreateEntitlementDoc().root[WalletEntitlements.Key] = (PlistElement) new PlistElementArray()) as PlistElementArray;
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
      PlistElementArray plistElementArray = (this.GetOrCreateEntitlementDoc().root[ApplePayEntitlements.Key] = (PlistElement) new PlistElementArray()) as PlistElementArray;
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
      PlistElementArray root1 = (this.GetOrCreateInfoDoc().root[MapsInfo.BundleKey] ?? (this.GetOrCreateInfoDoc().root[MapsInfo.BundleKey] = (PlistElement) new PlistElementArray())) as PlistElementArray;
      root1.values.Add((PlistElement) new PlistElementDict());
      PlistElementDict dictElementInArray = this.GetOrCreateUniqueDictElementInArray(root1);
      dictElementInArray[MapsInfo.BundleNameKey] = (PlistElement) new PlistElementString(MapsInfo.BundleNameValue);
      this.GetOrCreateStringElementInArray((dictElementInArray[MapsInfo.BundleTypeKey] ?? (dictElementInArray[MapsInfo.BundleTypeKey] = (PlistElement) new PlistElementArray())) as PlistElementArray, MapsInfo.BundleTypeValue);
      PlistElementArray root2 = (this.GetOrCreateInfoDoc().root[MapsInfo.ModeKey] ?? (this.GetOrCreateInfoDoc().root[MapsInfo.ModeKey] = (PlistElement) new PlistElementArray())) as PlistElementArray;
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
      ((this.GetOrCreateEntitlementDoc().root[VPNEntitlements.Key] = (PlistElement) new PlistElementArray()) as PlistElementArray).values.Add((PlistElement) new PlistElementString(VPNEntitlements.Value));
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.PersonalVPN, this.m_EntitlementFilePath, false);
    }

    public void AddBackgroundModes(BackgroundModesOptions options)
    {
      PlistElementArray root = (this.GetOrCreateInfoDoc().root[BackgroundInfo.Key] ?? (this.GetOrCreateInfoDoc().root[BackgroundInfo.Key] = (PlistElement) new PlistElementArray())) as PlistElementArray;
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
      PlistElementArray plistElementArray = (this.GetOrCreateEntitlementDoc().root[KeyChainEntitlements.Key] = (PlistElement) new PlistElementArray()) as PlistElementArray;
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
      PlistElementArray plistElementArray = (this.GetOrCreateEntitlementDoc().root[AssociatedDomainsEntitlements.Key] = (PlistElement) new PlistElementArray()) as PlistElementArray;
      for (int index = 0; index < domains.Length; ++index)
        plistElementArray.values.Add((PlistElement) new PlistElementString(domains[index]));
      this.project.AddCapability(this.m_TargetGuid, PBXCapabilityType.AssociatedDomains, this.m_EntitlementFilePath, false);
    }

    public void AddAppGroups(string[] groups)
    {
      PlistElementArray plistElementArray = (this.GetOrCreateEntitlementDoc().root[AppGroupsEntitlements.Key] = (PlistElement) new PlistElementArray()) as PlistElementArray;
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
      this.GetOrCreateStringElementInArray((this.GetOrCreateInfoDoc().root[HealthInfo.Key] ?? (this.GetOrCreateInfoDoc().root[HealthInfo.Key] = (PlistElement) new PlistElementArray())) as PlistElementArray, HealthInfo.Value);
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
      ((this.GetOrCreateEntitlementDoc().root[SignInWithAppleEntitlements.Key] = (PlistElement) new PlistElementArray()) as PlistElementArray).values.Add((PlistElement) new PlistElementString(SignInWithAppleEntitlements.Value));
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
        if ((uint) files.Length > 0U)
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
