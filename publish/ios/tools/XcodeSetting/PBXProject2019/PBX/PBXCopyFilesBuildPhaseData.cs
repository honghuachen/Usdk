// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXCopyFilesBuildPhaseData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXCopyFilesBuildPhaseData : FileGUIDListBase
  {
    private static PropertyCommentChecker checkerData;
    public string name;
    public string dstPath;
    public string dstSubfolderSpec;

    internal override PropertyCommentChecker checker
    {
      get
      {
        return PBXCopyFilesBuildPhaseData.checkerData;
      }
    }

    static PBXCopyFilesBuildPhaseData()
    {
      string[] strArray = new string[1];
      int index = 0;
      string str = "files/*";
      strArray[index] = str;
      PBXCopyFilesBuildPhaseData.checkerData = new PropertyCommentChecker((IEnumerable<string>) strArray);
    }

    public static PBXCopyFilesBuildPhaseData Create(string name, string dstPath, string subfolderSpec)
    {
      PBXCopyFilesBuildPhaseData filesBuildPhaseData = new PBXCopyFilesBuildPhaseData();
      filesBuildPhaseData.guid = PBXGUID.Generate();
      filesBuildPhaseData.SetPropertyString("isa", "PBXCopyFilesBuildPhase");
      filesBuildPhaseData.SetPropertyString("buildActionMask", "2147483647");
      filesBuildPhaseData.dstPath = dstPath;
      filesBuildPhaseData.dstSubfolderSpec = subfolderSpec;
      filesBuildPhaseData.files = (GUIDList) new List<string>();
      filesBuildPhaseData.SetPropertyString("runOnlyForDeploymentPostprocessing", "0");
      filesBuildPhaseData.name = name;
      return filesBuildPhaseData;
    }

    public override void UpdateProps()
    {
      this.SetPropertyList("files", (List<string>) this.files);
      this.SetPropertyString("name", this.name);
      this.SetPropertyString("dstPath", this.dstPath);
      this.SetPropertyString("dstSubfolderSpec", this.dstSubfolderSpec);
    }

    public override void UpdateVars()
    {
      this.files = (GUIDList) this.GetPropertyList("files");
      this.name = this.GetPropertyString("name");
      this.dstPath = this.GetPropertyString("dstPath");
      this.dstSubfolderSpec = this.GetPropertyString("dstSubfolderSpec");
    }
  }
}
