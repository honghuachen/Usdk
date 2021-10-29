﻿// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXGUID
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXGUID
  {
    private static PBXGUID.GuidGenerator guidGenerator = new PBXGUID.GuidGenerator(PBXGUID.DefaultGuidGenerator);

    internal static string DefaultGuidGenerator()
    {
      return Guid.NewGuid().ToString("N").Substring(8).ToUpper();
    }

    internal static void SetGuidGenerator(PBXGUID.GuidGenerator generator)
    {
      PBXGUID.guidGenerator = generator;
    }

    public static string Generate()
    {
      return PBXGUID.guidGenerator();
    }

    internal delegate string GuidGenerator();
  }
}
