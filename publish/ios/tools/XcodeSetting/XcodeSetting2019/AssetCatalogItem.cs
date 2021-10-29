// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.AssetCatalogItem
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode
{
  internal abstract class AssetCatalogItem
  {
    protected Dictionary<string, string> m_Properties = new Dictionary<string, string>();
    public readonly string name;
    public readonly string authorId;
    protected string m_Path;

    public string path
    {
      get
      {
        return this.m_Path;
      }
    }

    public AssetCatalogItem(string name, string authorId)
    {
      if (name != null && name.Contains("/"))
        throw new Exception("Asset catalog item must not have slashes in name");
      this.name = name;
      this.authorId = authorId;
    }

    protected JsonElementDict WriteInfoToJson(JsonDocument doc)
    {
      JsonElementDict dict = doc.root.CreateDict("info");
      dict.SetInteger("version", 1);
      dict.SetString("author", this.authorId);
      return dict;
    }

    public abstract void Write(List<string> warnings);
  }
}
