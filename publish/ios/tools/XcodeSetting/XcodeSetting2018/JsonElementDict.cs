// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.JsonElementDict
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode
{
  internal class JsonElementDict : JsonElement
  {
    private SortedDictionary<string, JsonElement> m_PrivateValue = new SortedDictionary<string, JsonElement>();

    public IDictionary<string, JsonElement> values
    {
      get
      {
        return (IDictionary<string, JsonElement>) this.m_PrivateValue;
      }
    }

    public new JsonElement this[string key]
    {
      get
      {
        if (this.values.ContainsKey(key))
          return this.values[key];
        return (JsonElement) null;
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

    public void SetInteger(string key, int val)
    {
      this.values[key] = (JsonElement) new JsonElementInteger(val);
    }

    public void SetString(string key, string val)
    {
      this.values[key] = (JsonElement) new JsonElementString(val);
    }

    public void SetBoolean(string key, bool val)
    {
      this.values[key] = (JsonElement) new JsonElementBoolean(val);
    }

    public JsonElementArray CreateArray(string key)
    {
      JsonElementArray jsonElementArray = new JsonElementArray();
      this.values[key] = (JsonElement) jsonElementArray;
      return jsonElementArray;
    }

    public JsonElementDict CreateDict(string key)
    {
      JsonElementDict jsonElementDict = new JsonElementDict();
      this.values[key] = (JsonElement) jsonElementDict;
      return jsonElementDict;
    }
  }
}
