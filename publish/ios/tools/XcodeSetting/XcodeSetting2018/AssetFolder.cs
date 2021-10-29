// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.AssetFolder
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.iOS.Xcode
{
  internal class AssetFolder : AssetCatalogItem
  {
    private List<AssetCatalogItem> m_Items = new List<AssetCatalogItem>();
    private bool m_ProvidesNamespace = false;

    public bool providesNamespace
    {
      get
      {
        return this.m_ProvidesNamespace;
      }
      set
      {
        if (this.m_Items.Count > 0 && value != this.m_ProvidesNamespace)
          throw new Exception("Asset folder namespace providing status can't be changed after items have been added");
        this.m_ProvidesNamespace = value;
      }
    }

    internal AssetFolder(string parentPath, string name, string authorId)
      : base(name, authorId)
    {
      if (name != null)
        this.m_Path = Path.Combine(parentPath, name);
      else
        this.m_Path = parentPath;
    }

    public AssetFolder OpenFolder(string name)
    {
      AssetCatalogItem child = this.GetChild(name);
      if (child != null)
      {
        if (child is AssetFolder)
          return child as AssetFolder;
        throw new Exception("The given path is already occupied with an asset");
      }
      AssetFolder assetFolder = new AssetFolder(this.m_Path, name, this.authorId);
      this.m_Items.Add((AssetCatalogItem) assetFolder);
      return assetFolder;
    }

    private T GetExistingItemWithType<T>(string name) where T : class
    {
      AssetCatalogItem child = this.GetChild(name);
      if (child == null)
        return (T) null;
      if (child is T)
        return (object) child as T;
      throw new Exception("The given path is already occupied with an asset");
    }

    public AssetDataSet OpenDataSet(string name)
    {
      AssetDataSet existingItemWithType = this.GetExistingItemWithType<AssetDataSet>(name);
      if (existingItemWithType != null)
        return existingItemWithType;
      AssetDataSet assetDataSet = new AssetDataSet(this.m_Path, name, this.authorId);
      this.m_Items.Add((AssetCatalogItem) assetDataSet);
      return assetDataSet;
    }

    public AssetImageSet OpenImageSet(string name)
    {
      AssetImageSet existingItemWithType = this.GetExistingItemWithType<AssetImageSet>(name);
      if (existingItemWithType != null)
        return existingItemWithType;
      AssetImageSet assetImageSet = new AssetImageSet(this.m_Path, name, this.authorId);
      this.m_Items.Add((AssetCatalogItem) assetImageSet);
      return assetImageSet;
    }

    public AssetImageStack OpenImageStack(string name)
    {
      AssetImageStack existingItemWithType = this.GetExistingItemWithType<AssetImageStack>(name);
      if (existingItemWithType != null)
        return existingItemWithType;
      AssetImageStack assetImageStack = new AssetImageStack(this.m_Path, name, this.authorId);
      this.m_Items.Add((AssetCatalogItem) assetImageStack);
      return assetImageStack;
    }

    public AssetBrandAssetGroup OpenBrandAssetGroup(string name)
    {
      AssetBrandAssetGroup existingItemWithType = this.GetExistingItemWithType<AssetBrandAssetGroup>(name);
      if (existingItemWithType != null)
        return existingItemWithType;
      AssetBrandAssetGroup assetBrandAssetGroup = new AssetBrandAssetGroup(this.m_Path, name, this.authorId);
      this.m_Items.Add((AssetCatalogItem) assetBrandAssetGroup);
      return assetBrandAssetGroup;
    }

    public AssetCatalogItem GetChild(string name)
    {
      foreach (AssetCatalogItem assetCatalogItem in this.m_Items)
      {
        if (assetCatalogItem.name == name)
          return assetCatalogItem;
      }
      return (AssetCatalogItem) null;
    }

    private void WriteJson()
    {
      if (!this.providesNamespace)
        return;
      JsonDocument doc = new JsonDocument();
      this.WriteInfoToJson(doc);
      doc.root.CreateDict("properties").SetBoolean("provides-namespace", this.providesNamespace);
      doc.WriteToFile(Path.Combine(this.m_Path, "Contents.json"));
    }

    public override void Write(List<string> warnings)
    {
      if (Directory.Exists(this.m_Path))
        Directory.Delete(this.m_Path, true);
      Directory.CreateDirectory(this.m_Path);
      this.WriteJson();
      foreach (AssetCatalogItem assetCatalogItem in this.m_Items)
        assetCatalogItem.Write(warnings);
    }
  }
}
