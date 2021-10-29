// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PlistElement
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;

namespace UnityEditor.iOS.Xcode
{
  public class PlistElement
  {
    public PlistElement this[string key]
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

    protected PlistElement()
    {
    }

    public string AsString()
    {
      return ((PlistElementString) this).value;
    }

    public int AsInteger()
    {
      return ((PlistElementInteger) this).value;
    }

    public bool AsBoolean()
    {
      return ((PlistElementBoolean) this).value;
    }

    public PlistElementArray AsArray()
    {
      return (PlistElementArray) this;
    }

    public PlistElementDict AsDict()
    {
      return (PlistElementDict) this;
    }

    public float AsReal()
    {
      return ((PlistElementReal) this).value;
    }

    public DateTime AsDate()
    {
      return ((PlistElementDate) this).value;
    }
  }
}
