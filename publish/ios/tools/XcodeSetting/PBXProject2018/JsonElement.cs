// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.JsonElement
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

namespace UnityEditor.iOS.Xcode
{
  internal class JsonElement
  {
    public JsonElement this[string key]
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

    protected JsonElement()
    {
    }

    public string AsString()
    {
      return ((JsonElementString) this).value;
    }

    public int AsInteger()
    {
      return ((JsonElementInteger) this).value;
    }

    public bool AsBoolean()
    {
      return ((JsonElementBoolean) this).value;
    }

    public JsonElementArray AsArray()
    {
      return (JsonElementArray) this;
    }

    public JsonElementDict AsDict()
    {
      return (JsonElementDict) this;
    }
  }
}
