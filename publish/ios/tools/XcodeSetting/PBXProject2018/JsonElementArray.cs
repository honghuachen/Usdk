// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.JsonElementArray
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode
{
  internal class JsonElementArray : JsonElement
  {
    public List<JsonElement> values = new List<JsonElement>();

    public void AddString(string val)
    {
      this.values.Add((JsonElement) new JsonElementString(val));
    }

    public void AddInteger(int val)
    {
      this.values.Add((JsonElement) new JsonElementInteger(val));
    }

    public void AddBoolean(bool val)
    {
      this.values.Add((JsonElement) new JsonElementBoolean(val));
    }

    public JsonElementArray AddArray()
    {
      JsonElementArray jsonElementArray = new JsonElementArray();
      this.values.Add((JsonElement) jsonElementArray);
      return jsonElementArray;
    }

    public JsonElementDict AddDict()
    {
      JsonElementDict jsonElementDict = new JsonElementDict();
      this.values.Add((JsonElement) jsonElementDict);
      return jsonElementDict;
    }
  }
}
