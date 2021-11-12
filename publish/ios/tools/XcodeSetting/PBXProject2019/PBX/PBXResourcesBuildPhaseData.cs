// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXResourcesBuildPhaseData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXResourcesBuildPhaseData : FileGUIDListBase
  {
    public static PBXResourcesBuildPhaseData Create()
    {
      PBXResourcesBuildPhaseData resourcesBuildPhaseData = new PBXResourcesBuildPhaseData();
      resourcesBuildPhaseData.guid = PBXGUID.Generate();
      resourcesBuildPhaseData.SetPropertyString("isa", "PBXResourcesBuildPhase");
      resourcesBuildPhaseData.SetPropertyString("buildActionMask", "2147483647");
      resourcesBuildPhaseData.files = (GUIDList) new List<string>();
      resourcesBuildPhaseData.SetPropertyString("runOnlyForDeploymentPostprocessing", "0");
      return resourcesBuildPhaseData;
    }
  }
}
