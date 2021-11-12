// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.BackgroundModesOptions
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;

namespace UnityEditor.iOS.Xcode
{
  [Flags]
  [Serializable]
  public enum BackgroundModesOptions
  {
    None = 0,
    AudioAirplayPiP = 1,
    LocationUpdates = 2,
    VoiceOverIP = 4,
    NewsstandDownloads = 8,
    ExternalAccessoryCommunication = 16,
    UsesBluetoothLEAccessory = 32,
    ActsAsABluetoothLEAccessory = 64,
    BackgroundFetch = 128,
    RemoteNotifications = 256,
  }
}
