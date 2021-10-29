// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PlistElementArray
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode
{
  public class PlistElementArray : PlistElement
  {
    public List<PlistElement> values = new List<PlistElement>();

    public void AddString(string val)
    {
      this.values.Add((PlistElement) new PlistElementString(val));
    }

    public void AddInteger(int val)
    {
      this.values.Add((PlistElement) new PlistElementInteger(val));
    }

    public void AddBoolean(bool val)
    {
      this.values.Add((PlistElement) new PlistElementBoolean(val));
    }

    public void AddDate(DateTime val)
    {
      this.values.Add((PlistElement) new PlistElementDate(val));
    }

    public void AddReal(float val)
    {
      this.values.Add((PlistElement) new PlistElementReal(val));
    }

    public PlistElementArray AddArray()
    {
      PlistElementArray plistElementArray = new PlistElementArray();
      this.values.Add((PlistElement) plistElementArray);
      return plistElementArray;
    }

    public PlistElementDict AddDict()
    {
      PlistElementDict plistElementDict = new PlistElementDict();
      this.values.Add((PlistElement) plistElementDict);
      return plistElementDict;
    }
  }
}
