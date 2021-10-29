// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXTargetDependencyData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXTargetDependencyData : PBXObjectData
  {
    private static PropertyCommentChecker checkerData;

    internal override PropertyCommentChecker checker
    {
      get
      {
        return PBXTargetDependencyData.checkerData;
      }
    }

    static PBXTargetDependencyData()
    {
      string[] strArray = new string[2];
      int index1 = 0;
      string str1 = "target/*";
      strArray[index1] = str1;
      int index2 = 1;
      string str2 = "targetProxy/*";
      strArray[index2] = str2;
      PBXTargetDependencyData.checkerData = new PropertyCommentChecker((IEnumerable<string>) strArray);
    }

    public static PBXTargetDependencyData Create(string target, string targetProxy)
    {
      PBXTargetDependencyData targetDependencyData = new PBXTargetDependencyData();
      targetDependencyData.guid = PBXGUID.Generate();
      targetDependencyData.SetPropertyString("isa", "PBXTargetDependency");
      targetDependencyData.SetPropertyString("target", target);
      targetDependencyData.SetPropertyString("targetProxy", targetProxy);
      return targetDependencyData;
    }
  }
}
