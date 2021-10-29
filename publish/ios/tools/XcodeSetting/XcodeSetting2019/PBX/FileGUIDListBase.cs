﻿// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.FileGUIDListBase
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class FileGUIDListBase : PBXObjectData
  {
    public GUIDList files;
    private static PropertyCommentChecker checkerData;

    internal override PropertyCommentChecker checker
    {
      get
      {
        return FileGUIDListBase.checkerData;
      }
    }

    static FileGUIDListBase()
    {
      string[] strArray = new string[1];
      int index = 0;
      string str = "files/*";
      strArray[index] = str;
      FileGUIDListBase.checkerData = new PropertyCommentChecker((IEnumerable<string>) strArray);
    }

    public override void UpdateProps()
    {
      this.SetPropertyList("files", (List<string>) this.files);
    }

    public override void UpdateVars()
    {
      this.files = (GUIDList) this.GetPropertyList("files");
    }
  }
}
