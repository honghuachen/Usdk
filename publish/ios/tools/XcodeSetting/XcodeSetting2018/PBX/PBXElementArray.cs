// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXElementArray
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
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
