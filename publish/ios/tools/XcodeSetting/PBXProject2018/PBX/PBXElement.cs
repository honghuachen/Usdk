// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXElement
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXElement
  {
    public PBXElement this[string key]
    {
      get
      {
        return this.AsDict()[key];
      }
      set
      {
        this.AsDict()[key] = value;
      }
    }

    protected PBXElement()
    {
    }

    public string AsString()
    {
      return ((PBXElementString) this).value;
    }

    public PBXElementArray AsArray()
    {
      return (PBXElementArray) this;
    }

    public PBXElementDict AsDict()
    {
      return (PBXElementDict) this;
    }
  }
}
