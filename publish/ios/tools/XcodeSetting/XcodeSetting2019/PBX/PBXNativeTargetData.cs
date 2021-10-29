// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXNativeTargetData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXNativeTargetData : PBXObjectData
  {
    public GUIDList phases;
    public string buildConfigList;
    public string name;
    public GUIDList dependencies;
    public string productReference;
    private static PropertyCommentChecker checkerData;

    internal override PropertyCommentChecker checker
    {
      get
      {
        return PBXNativeTargetData.checkerData;
      }
    }

    static PBXNativeTargetData()
    {
      string[] strArray = new string[5];
      int index1 = 0;
      string str1 = "buildPhases/*";
      strArray[index1] = str1;
      int index2 = 1;
      string str2 = "buildRules/*";
      strArray[index2] = str2;
      int index3 = 2;
      string str3 = "dependencies/*";
      strArray[index3] = str3;
      int index4 = 3;
      string str4 = "productReference/*";
      strArray[index4] = str4;
      int index5 = 4;
      string str5 = "buildConfigurationList/*";
      strArray[index5] = str5;
      PBXNativeTargetData.checkerData = new PropertyCommentChecker((IEnumerable<string>) strArray);
    }

    public static PBXNativeTargetData Create(string name, string productRef, string productType, string buildConfigList)
    {
      PBXNativeTargetData nativeTargetData = new PBXNativeTargetData();
      nativeTargetData.guid = PBXGUID.Generate();
      nativeTargetData.SetPropertyString("isa", "PBXNativeTarget");
      nativeTargetData.buildConfigList = buildConfigList;
      nativeTargetData.phases = new GUIDList();
      nativeTargetData.SetPropertyList("buildRules", new List<string>());
      nativeTargetData.dependencies = new GUIDList();
      nativeTargetData.name = name;
      nativeTargetData.productReference = productRef;
      nativeTargetData.SetPropertyString("productName", name);
      nativeTargetData.SetPropertyString("productReference", productRef);
      nativeTargetData.SetPropertyString("productType", productType);
      return nativeTargetData;
    }

    public override void UpdateProps()
    {
      this.SetPropertyString("buildConfigurationList", this.buildConfigList);
      this.SetPropertyString("name", this.name);
      this.SetPropertyString("productReference", this.productReference);
      this.SetPropertyList("buildPhases", (List<string>) this.phases);
      this.SetPropertyList("dependencies", (List<string>) this.dependencies);
    }

    public override void UpdateVars()
    {
      this.buildConfigList = this.GetPropertyString("buildConfigurationList");
      this.name = this.GetPropertyString("name");
      this.productReference = this.GetPropertyString("productReference");
      this.phases = (GUIDList) this.GetPropertyList("buildPhases");
      this.dependencies = (GUIDList) this.GetPropertyList("dependencies");
    }
  }
}
