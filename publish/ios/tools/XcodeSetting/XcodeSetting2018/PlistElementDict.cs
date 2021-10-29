// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PlistElementDict
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode
{
  public class PlistElementDict : PlistElement
  {
    private SortedDictionary<string, PlistElement> m_PrivateValue = new SortedDictionary<string, PlistElement>();

    public IDictionary<string, PlistElement> values
    {
      get
      {
        return (IDictionary<string, PlistElement>) this.m_PrivateValue;
      }
    }

    public new PlistElement this[string key]
    {
      get
      {
        if (this.values.ContainsKey(key))
          return this.values[key];
        return (PlistElement) null;
      }
      set
      {
        this.values[key] = value;
      }
    }

    public void SetInteger(string key, int val)
    {
      this.values[key] = (PlistElement) new PlistElementInteger(val);
    }

    public void SetString(string key, string val)
    {
      this.values[key] = (PlistElement) new PlistElementString(val);
    }

    public void SetBoolean(string key, bool val)
    {
      this.values[key] = (PlistElement) new PlistElementBoolean(val);
    }

    public void SetDate(string key, DateTime val)
    {
      this.values[key] = (PlistElement) new PlistElementDate(val);
    }

    public void SetReal(string key, float val)
    {
      this.values[key] = (PlistElement) new PlistElementReal(val);
    }

    public PlistElementArray CreateArray(string key)
    {
      PlistElementArray plistElementArray = new PlistElementArray();
      this.values[key] = (PlistElement) plistElementArray;
      return plistElementArray;
    }

    public PlistElementDict CreateDict(string key)
    {
      PlistElementDict plistElementDict = new PlistElementDict();
      this.values[key] = (PlistElement) plistElementDict;
      return plistElementDict;
    }
  }
}
