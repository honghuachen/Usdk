// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXFrameworksBuildPhaseData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXFrameworksBuildPhaseData : FileGUIDListBase
  {
    public static PBXFrameworksBuildPhaseData Create()
    {
      PBXFrameworksBuildPhaseData frameworksBuildPhaseData = new PBXFrameworksBuildPhaseData();
      frameworksBuildPhaseData.guid = PBXGUID.Generate();
      frameworksBuildPhaseData.SetPropertyString("isa", "PBXFrameworksBuildPhase");
      frameworksBuildPhaseData.SetPropertyString("buildActionMask", "2147483647");
      frameworksBuildPhaseData.files = (GUIDList) new List<string>();
      frameworksBuildPhaseData.SetPropertyString("runOnlyForDeploymentPostprocessing", "0");
      return frameworksBuildPhaseData;
    }
  }
}
