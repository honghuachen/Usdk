// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PlistElementArray
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
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
