// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXContainerItemProxyData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXContainerItemProxyData : PBXObjectData
  {
    private static PropertyCommentChecker checkerData;

    internal override PropertyCommentChecker checker
    {
      get
      {
        return PBXContainerItemProxyData.checkerData;
      }
    }

    static PBXContainerItemProxyData()
    {
      string[] strArray = new string[1];
      int index = 0;
      string str = "containerPortal/*";
      strArray[index] = str;
      PBXContainerItemProxyData.checkerData = new PropertyCommentChecker((IEnumerable<string>) strArray);
    }

    public static PBXContainerItemProxyData Create(string containerRef, string proxyType, string remoteGlobalGUID, string remoteInfo)
    {
      PBXContainerItemProxyData containerItemProxyData = new PBXContainerItemProxyData();
      containerItemProxyData.guid = PBXGUID.Generate();
      containerItemProxyData.SetPropertyString("isa", "PBXContainerItemProxy");
      containerItemProxyData.SetPropertyString("containerPortal", containerRef);
      containerItemProxyData.SetPropertyString("proxyType", proxyType);
      containerItemProxyData.SetPropertyString("remoteGlobalIDString", remoteGlobalGUID);
      containerItemProxyData.SetPropertyString("remoteInfo", remoteInfo);
      return containerItemProxyData;
    }
  }
}
