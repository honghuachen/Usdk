// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXElementDict
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXElementDict : PBXElement
  {
    private Dictionary<string, PBXElement> m_PrivateValue = new Dictionary<string, PBXElement>();

    public IDictionary<string, PBXElement> values
    {
      get
      {
        return (IDictionary<string, PBXElement>) this.m_PrivateValue;
      }
    }

    public new PBXElement this[string key]
    {
      get
      {
        if (this.values.ContainsKey(key))
          return this.values[key];
        return (PBXElement) null;
      }
      set
      {
        this.values[key] = value;
      }
    }

    public bool Contains(string key)
    {
      return this.values.ContainsKey(key);
    }

    public void Remove(string key)
    {
      this.values.Remove(key);
    }

    public void SetString(string key, string val)
    {
      this.values[key] = (PBXElement) new PBXElementString(val);
    }

    public PBXElementArray CreateArray(string key)
    {
      PBXElementArray pbxElementArray = new PBXElementArray();
      this.values[key] = (PBXElement) pbxElementArray;
      return pbxElementArray;
    }

    public PBXElementDict CreateDict(string key)
    {
      PBXElementDict pbxElementDict = new PBXElementDict();
      this.values[key] = (PBXElement) pbxElementDict;
      return pbxElementDict;
    }
  }
}
