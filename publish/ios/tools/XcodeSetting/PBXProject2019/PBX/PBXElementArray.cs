// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXElementArray
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXElementArray : PBXElement
  {
    public List<PBXElement> values = new List<PBXElement>();

    public void AddString(string val)
    {
      this.values.Add((PBXElement) new PBXElementString(val));
    }

    public PBXElementArray AddArray()
    {
      PBXElementArray pbxElementArray = new PBXElementArray();
      this.values.Add((PBXElement) pbxElementArray);
      return pbxElementArray;
    }

    public PBXElementDict AddDict()
    {
      PBXElementDict pbxElementDict = new PBXElementDict();
      this.values.Add((PBXElement) pbxElementDict);
      return pbxElementDict;
    }
  }
}
