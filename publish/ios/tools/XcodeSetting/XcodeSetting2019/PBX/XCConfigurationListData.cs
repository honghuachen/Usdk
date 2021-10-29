// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.XCConfigurationListData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class XCConfigurationListData : PBXObjectData
  {
    public GUIDList buildConfigs;
    private static PropertyCommentChecker checkerData;

    internal override PropertyCommentChecker checker
    {
      get
      {
        return XCConfigurationListData.checkerData;
      }
    }

    static XCConfigurationListData()
    {
      string[] strArray = new string[1];
      int index = 0;
      string str = "buildConfigurations/*";
      strArray[index] = str;
      XCConfigurationListData.checkerData = new PropertyCommentChecker((IEnumerable<string>) strArray);
    }

    public static XCConfigurationListData Create()
    {
      XCConfigurationListData configurationListData = new XCConfigurationListData();
      configurationListData.guid = PBXGUID.Generate();
      configurationListData.SetPropertyString("isa", "XCConfigurationList");
      configurationListData.buildConfigs = new GUIDList();
      configurationListData.SetPropertyString("defaultConfigurationIsVisible", "0");
      return configurationListData;
    }

    public override void UpdateProps()
    {
      this.SetPropertyList("buildConfigurations", (List<string>) this.buildConfigs);
    }

    public override void UpdateVars()
    {
      this.buildConfigs = (GUIDList) this.GetPropertyList("buildConfigurations");
    }
  }
}
