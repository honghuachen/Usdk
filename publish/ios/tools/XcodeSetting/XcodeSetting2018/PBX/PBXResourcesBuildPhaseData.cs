// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXResourcesBuildPhaseData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
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
