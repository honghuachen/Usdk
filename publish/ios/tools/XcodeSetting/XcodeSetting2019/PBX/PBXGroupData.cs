// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXGroupData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXGroupData : PBXObjectData
  {
    public GUIDList children;
    public PBXSourceTree tree;
    public string name;
    public string path;
    private static PropertyCommentChecker checkerData;

    internal override PropertyCommentChecker checker
    {
      get
      {
        return PBXGroupData.checkerData;
      }
    }

    static PBXGroupData()
    {
      string[] strArray = new string[1];
      int index = 0;
      string str = "children/*";
      strArray[index] = str;
      PBXGroupData.checkerData = new PropertyCommentChecker((IEnumerable<string>) strArray);
    }

    public static PBXGroupData Create(string name, string path, PBXSourceTree tree)
    {
      if (name.Contains("/"))
        throw new Exception("Group name must not contain '/'");
      PBXGroupData pbxGroupData = new PBXGroupData();
      pbxGroupData.guid = PBXGUID.Generate();
      pbxGroupData.SetPropertyString("isa", "PBXGroup");
      pbxGroupData.name = name;
      pbxGroupData.path = path;
      pbxGroupData.tree = PBXSourceTree.Group;
      pbxGroupData.children = new GUIDList();
      return pbxGroupData;
    }

    public static PBXGroupData CreateRelative(string name)
    {
      return PBXGroupData.Create(name, name, PBXSourceTree.Group);
    }

    public override void UpdateProps()
    {
      this.SetPropertyList("children", (List<string>) this.children);
      if (this.name == this.path)
        this.SetPropertyString("name", (string) null);
      else
        this.SetPropertyString("name", this.name);
      if (this.path == "")
        this.SetPropertyString("path", (string) null);
      else
        this.SetPropertyString("path", this.path);
      this.SetPropertyString("sourceTree", FileTypeUtils.SourceTreeDesc(this.tree));
    }

    public override void UpdateVars()
    {
      this.children = (GUIDList) this.GetPropertyList("children");
      this.path = this.GetPropertyString("path");
      this.name = this.GetPropertyString("name");
      if (this.name == null)
        this.name = this.path;
      if (this.path == null)
        this.path = "";
      this.tree = FileTypeUtils.ParseSourceTree(this.GetPropertyString("sourceTree"));
    }
  }
}
