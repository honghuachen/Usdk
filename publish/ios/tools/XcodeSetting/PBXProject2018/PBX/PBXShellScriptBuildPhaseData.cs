// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXShellScriptBuildPhaseData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXShellScriptBuildPhaseData : FileGUIDListBase
  {
    public string name;
    public string shellPath;
    public string shellScript;
    public List<string> inputPaths;

    public static PBXShellScriptBuildPhaseData Create(string name, string shellPath, string shellScript, List<string> inputPaths)
    {
      PBXShellScriptBuildPhaseData scriptBuildPhaseData = new PBXShellScriptBuildPhaseData();
      scriptBuildPhaseData.guid = PBXGUID.Generate();
      scriptBuildPhaseData.SetPropertyString("isa", "PBXShellScriptBuildPhase");
      scriptBuildPhaseData.SetPropertyString("buildActionMask", "2147483647");
      scriptBuildPhaseData.files = (GUIDList) new List<string>();
      scriptBuildPhaseData.SetPropertyString("runOnlyForDeploymentPostprocessing", "0");
      scriptBuildPhaseData.name = name;
      scriptBuildPhaseData.shellPath = shellPath;
      scriptBuildPhaseData.shellScript = shellScript;
      scriptBuildPhaseData.inputPaths = inputPaths;
      return scriptBuildPhaseData;
    }

    public override void UpdateProps()
    {
      base.UpdateProps();
      this.SetPropertyString("name", this.name);
      this.SetPropertyString("shellPath", this.shellPath);
      this.SetPropertyString("shellScript", this.shellScript);
      this.SetPropertyList("inputPaths", this.inputPaths);
    }

    public override void UpdateVars()
    {
      base.UpdateVars();
      this.name = this.GetPropertyString("name");
      this.shellPath = this.GetPropertyString("shellPath");
      this.shellScript = this.GetPropertyString("shellScript");
      this.inputPaths = this.GetPropertyList("inputPaths");
    }
  }
}
