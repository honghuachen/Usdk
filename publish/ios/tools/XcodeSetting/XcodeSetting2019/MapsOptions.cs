// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.MapsOptions
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;

namespace UnityEditor.iOS.Xcode
{
  [Flags]
  [Serializable]
  public enum MapsOptions
  {
    None = 0,
    Airplane = 1,
    Bike = 2,
    Bus = 4,
    Car = 8,
    Ferry = 16,
    Pedestrian = 32,
    RideSharing = 64,
    StreetCar = 128,
    Subway = 256,
    Taxi = 512,
    Train = 1024,
    Other = 2048,
  }
}
