// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.BackgroundModesOptions
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
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
