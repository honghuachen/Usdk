// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXHeadersBuildPhaseData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXHeadersBuildPhaseData : FileGUIDListBase
  {
    public static PBXHeadersBuildPhaseData Create()
    {
      PBXHeadersBuildPhaseData headersBuildPhaseData = new PBXHeadersBuildPhaseData();
      headersBuildPhaseData.guid = PBXGUID.Generate();
      headersBuildPhaseData.SetPropertyString("isa", "PBXHeadersBuildPhase");
      headersBuildPhaseData.SetPropertyString("buildActionMask", "2147483647");
      headersBuildPhaseData.files = (GUIDList) new List<string>();
      headersBuildPhaseData.SetPropertyString("runOnlyForDeploymentPostprocessing", "0");
      return headersBuildPhaseData;
    }
  }
}
