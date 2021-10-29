// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXSourcesBuildPhaseData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXSourcesBuildPhaseData : FileGUIDListBase
  {
    public static PBXSourcesBuildPhaseData Create()
    {
      PBXSourcesBuildPhaseData sourcesBuildPhaseData = new PBXSourcesBuildPhaseData();
      sourcesBuildPhaseData.guid = PBXGUID.Generate();
      sourcesBuildPhaseData.SetPropertyString("isa", "PBXSourcesBuildPhase");
      sourcesBuildPhaseData.SetPropertyString("buildActionMask", "2147483647");
      sourcesBuildPhaseData.files = (GUIDList) new List<string>();
      sourcesBuildPhaseData.SetPropertyString("runOnlyForDeploymentPostprocessing", "0");
      return sourcesBuildPhaseData;
    }
  }
}
