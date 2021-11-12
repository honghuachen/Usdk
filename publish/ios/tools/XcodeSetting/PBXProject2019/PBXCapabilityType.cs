// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBXCapabilityType
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

namespace UnityEditor.iOS.Xcode
{
  public sealed class PBXCapabilityType
  {
    public static readonly PBXCapabilityType ApplePay = new PBXCapabilityType("com.apple.ApplePay", true, "", false);
    public static readonly PBXCapabilityType AppGroups = new PBXCapabilityType("com.apple.ApplicationGroups.iOS", true, "", false);
    public static readonly PBXCapabilityType AssociatedDomains = new PBXCapabilityType("com.apple.SafariKeychain", true, "", false);
    public static readonly PBXCapabilityType BackgroundModes = new PBXCapabilityType("com.apple.BackgroundModes", false, "", false);
    public static readonly PBXCapabilityType DataProtection = new PBXCapabilityType("com.apple.DataProtection", true, "", false);
    public static readonly PBXCapabilityType GameCenter = new PBXCapabilityType("com.apple.GameCenter.iOS", false, "GameKit.framework", false);
    public static readonly PBXCapabilityType HealthKit = new PBXCapabilityType("com.apple.HealthKit", true, "HealthKit.framework", false);
    public static readonly PBXCapabilityType HomeKit = new PBXCapabilityType("com.apple.HomeKit", true, "HomeKit.framework", false);
    public static readonly PBXCapabilityType iCloud = new PBXCapabilityType("com.apple.iCloud", true, "CloudKit.framework", true);
    public static readonly PBXCapabilityType InAppPurchase = new PBXCapabilityType("com.apple.InAppPurchase", false, "", false);
    public static readonly PBXCapabilityType InterAppAudio = new PBXCapabilityType("com.apple.InterAppAudio", true, "AudioToolbox.framework", false);
    public static readonly PBXCapabilityType KeychainSharing = new PBXCapabilityType("com.apple.KeychainSharing", true, "", false);
    public static readonly PBXCapabilityType Maps = new PBXCapabilityType("com.apple.Maps.iOS", false, "MapKit.framework", false);
    public static readonly PBXCapabilityType PersonalVPN = new PBXCapabilityType("com.apple.VPNLite", true, "NetworkExtension.framework", false);
    public static readonly PBXCapabilityType PushNotifications = new PBXCapabilityType("com.apple.Push", true, "", false);
    public static readonly PBXCapabilityType Siri = new PBXCapabilityType("com.apple.Siri", true, "", false);
    public static readonly PBXCapabilityType Wallet = new PBXCapabilityType("com.apple.Wallet", true, "PassKit.framework", false);
    public static readonly PBXCapabilityType WirelessAccessoryConfiguration = new PBXCapabilityType("com.apple.WAC", true, "ExternalAccessory.framework", false);
    public static readonly PBXCapabilityType AccessWiFiInformation = new PBXCapabilityType("com.apple.developer.networking.wifi-info", true, "", false);
    public static readonly PBXCapabilityType SignInWithApple = new PBXCapabilityType("com.apple.developer.applesignin", true, "AuthenticationServices.framework", false);
    private readonly string m_ID;
    private readonly bool m_RequiresEntitlements;
    private readonly string m_Framework;
    private readonly bool m_OptionalFramework;

    public bool optionalFramework
    {
      get
      {
        return this.m_OptionalFramework;
      }
    }

    public string framework
    {
      get
      {
        return this.m_Framework;
      }
    }

    public string id
    {
      get
      {
        return this.m_ID;
      }
    }

    public bool requiresEntitlements
    {
      get
      {
        return this.m_RequiresEntitlements;
      }
    }

    private PBXCapabilityType(string _id, bool _requiresEntitlements, string _framework = "", bool _optionalFramework = false)
    {
      this.m_ID = _id;
      this.m_RequiresEntitlements = _requiresEntitlements;
      this.m_Framework = _framework;
      this.m_OptionalFramework = _optionalFramework;
    }

    public static PBXCapabilityType StringToPBXCapabilityType(string cap)
    {
      string s = cap;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = PrivateImplementationDetails.ComputeStringHash(s);
      if (stringHash <= 1795649615U)
      {
        if (stringHash <= 380370087U)
        {
          if (stringHash <= 12939752U)
          {
            if ((int) stringHash != 623369)
            {
              if ((int) stringHash == 12939752 && s == "com.apple.ApplePay")
                return PBXCapabilityType.ApplePay;
            }
            else if (s == "com.apple.DataProtection")
              return PBXCapabilityType.DataProtection;
          }
          else if ((int) stringHash != 200988660)
          {
            if ((int) stringHash != 290035059)
            {
              if ((int) stringHash == 380370087 && s == "com.apple.InterAppAudio")
                return PBXCapabilityType.InterAppAudio;
            }
            else if (s == "com.apple.Wallet")
              return PBXCapabilityType.Wallet;
          }
          else if (s == "com.apple.developer.applesignin")
            return PBXCapabilityType.SignInWithApple;
        }
        else if (stringHash <= 634469633U)
        {
          if ((int) stringHash != 399517150)
          {
            if ((int) stringHash == 634469633 && s == "com.apple.GameCenter")
              return PBXCapabilityType.GameCenter;
          }
          else if (s == "com.apple.SafariKeychain")
            return PBXCapabilityType.AssociatedDomains;
        }
        else if ((int) stringHash != 981567319)
        {
          if ((int) stringHash != 1727671163)
          {
            if ((int) stringHash == 1795649615 && s == "com.apple.Siri")
              return PBXCapabilityType.Siri;
          }
          else if (s == "com.apple.ApplicationGroups.iOS")
            return PBXCapabilityType.AppGroups;
        }
        else if (s == "com.apple.HomeKit")
          return PBXCapabilityType.HomeKit;
      }
      else if (stringHash <= 2742965566U)
      {
        if (stringHash <= 2418183063U)
        {
          if ((int) stringHash != -1966183118)
          {
            if ((int) stringHash == -1876784233 && s == "com.apple.InAppPurchase")
              return PBXCapabilityType.InAppPurchase;
          }
          else if (s == "com.apple.VPNLite")
            return PBXCapabilityType.PersonalVPN;
        }
        else if ((int) stringHash != -1868372490)
        {
          if ((int) stringHash != -1807207318)
          {
            if ((int) stringHash == -1552001730 && s == "com.apple.networking.wifi-info")
              return PBXCapabilityType.AccessWiFiInformation;
          }
          else if (s == "com.apple.iCloud")
            return PBXCapabilityType.iCloud;
        }
        else if (s == "com.apple.BackgroundModes")
          return PBXCapabilityType.BackgroundModes;
      }
      else if (stringHash <= 3326784170U)
      {
        if ((int) stringHash != -1457551806)
        {
          if ((int) stringHash == -968183126 && s == "com.apple.Push")
            return PBXCapabilityType.PushNotifications;
        }
        else if (s == "WAC")
          return PBXCapabilityType.WirelessAccessoryConfiguration;
      }
      else if ((int) stringHash != -588709462)
      {
        if ((int) stringHash != -422186908)
        {
          if ((int) stringHash == -34751118 && s == "com.apple.Maps.iOS")
            return PBXCapabilityType.Maps;
        }
        else if (s == "com.apple.HealthKit")
          return PBXCapabilityType.HealthKit;
      }
      else if (s == "com.apple.KeychainSharing")
        return PBXCapabilityType.KeychainSharing;
      return (PBXCapabilityType) null;
    }

    public struct TargetCapabilityPair
    {
      public string targetGuid;
      public PBXCapabilityType capability;

      public TargetCapabilityPair(string guid, PBXCapabilityType type)
      {
        this.targetGuid = guid;
        this.capability = type;
      }
    }
  }
}
