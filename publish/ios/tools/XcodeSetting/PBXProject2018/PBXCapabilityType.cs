// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBXCapabilityType
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

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
            switch (cap)
            {
                case "com.apple.ApplePay":
                    return ApplePay;
                case "com.apple.ApplicationGroups.iOS":
                    return AppGroups;
                case "com.apple.SafariKeychain":
                    return AssociatedDomains;
                case "com.apple.BackgroundModes":
                    return BackgroundModes;
                case "com.apple.DataProtection":
                    return DataProtection;
                case "com.apple.GameCenter":
                    return GameCenter;
                case "com.apple.HealthKit":
                    return HealthKit;
                case "com.apple.HomeKit":
                    return HomeKit;
                case "com.apple.iCloud":
                    return iCloud;
                case "com.apple.InAppPurchase":
                    return InAppPurchase;
                case "com.apple.InterAppAudio":
                    return InterAppAudio;
                case "com.apple.KeychainSharing":
                    return KeychainSharing;
                case "com.apple.Maps.iOS":
                    return Maps;
                case "com.apple.VPNLite":
                    return PersonalVPN;
                case "com.apple.Push":
                    return PushNotifications;
                case "com.apple.Siri":
                    return Siri;
                case "com.apple.Wallet":
                    return Wallet;
                case "WAC":
                    return WirelessAccessoryConfiguration;
                case "com.apple.SignInWithApple":
                    return SignInWithApple;
                case "com.apple.AccessWiFiInformation":
                    return AccessWiFiInformation;
                default:
                    return null;
            }
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
