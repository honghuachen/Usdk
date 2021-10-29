// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXReferenceProxyData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXReferenceProxyData : PBXObjectData
  {
    private static PropertyCommentChecker checkerData;

    internal override PropertyCommentChecker checker
    {
      get
      {
        return PBXReferenceProxyData.checkerData;
      }
    }

    public string path
    {
      get
      {
        return this.GetPropertyString("path");
      }
    }

    static PBXReferenceProxyData()
    {
      string[] strArray = new string[1];
      int index = 0;
      string str = "remoteRef/*";
      strArray[index] = str;
      PBXReferenceProxyData.checkerData = new PropertyCommentChecker((IEnumerable<string>) strArray);
    }

    public static PBXReferenceProxyData Create(string path, string fileType, string remoteRef, string sourceTree)
    {
      PBXReferenceProxyData referenceProxyData = new PBXReferenceProxyData();
      referenceProxyData.guid = PBXGUID.Generate();
      referenceProxyData.SetPropertyString("isa", "PBXReferenceProxy");
      referenceProxyData.SetPropertyString("path", path);
      referenceProxyData.SetPropertyString("fileType", fileType);
      referenceProxyData.SetPropertyString("remoteRef", remoteRef);
      referenceProxyData.SetPropertyString("sourceTree", sourceTree);
      return referenceProxyData;
    }
  }
}
