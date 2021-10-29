// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXFrameworksBuildPhaseData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
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
