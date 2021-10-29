// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.AssetBrandAssetGroup
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.iOS.Xcode
{
  internal class AssetBrandAssetGroup : AssetCatalogItem
  {
    private List<AssetBrandAssetGroup.AssetBrandAssetItem> m_Items = new List<AssetBrandAssetGroup.AssetBrandAssetItem>();

    internal AssetBrandAssetGroup(string assetCatalogPath, string name, string authorId)
      : base(name, authorId)
    {
      this.m_Path = Path.Combine(assetCatalogPath, name + ".brandassets");
    }

    private void AddItem(AssetCatalogItem item, string idiom, string role, int width, int height)
    {
      foreach (AssetBrandAssetGroup.AssetBrandAssetItem assetBrandAssetItem in this.m_Items)
      {
        if (assetBrandAssetItem.item.name == item.name)
          throw new Exception("An item with given name already exists");
      }
      this.m_Items.Add(new AssetBrandAssetGroup.AssetBrandAssetItem()
      {
        item = item,
        idiom = idiom,
        role = role,
        width = width,
        height = height
      });
    }

    public AssetImageSet OpenImageSet(string name, string idiom, string role, int width, int height)
    {
      AssetImageSet assetImageSet = new AssetImageSet(this.m_Path, name, this.authorId);
      this.AddItem((AssetCatalogItem) assetImageSet, idiom, role, width, height);
      return assetImageSet;
    }

    public AssetImageStack OpenImageStack(string name, string idiom, string role, int width, int height)
    {
      AssetImageStack assetImageStack = new AssetImageStack(this.m_Path, name, this.authorId);
      this.AddItem((AssetCatalogItem) assetImageStack, idiom, role, width, height);
      return assetImageStack;
    }

    public override void Write(List<string> warnings)
    {
      Directory.CreateDirectory(this.m_Path);
      JsonDocument doc = new JsonDocument();
      this.WriteInfoToJson(doc);
      JsonElementArray array = doc.root.CreateArray("assets");
      foreach (AssetBrandAssetGroup.AssetBrandAssetItem assetBrandAssetItem in this.m_Items)
      {
        JsonElementDict jsonElementDict = array.AddDict();
        jsonElementDict.SetString("size", string.Format("{0}x{1}", (object) assetBrandAssetItem.width, (object) assetBrandAssetItem.height));
        jsonElementDict.SetString("idiom", assetBrandAssetItem.idiom);
        jsonElementDict.SetString("role", assetBrandAssetItem.role);
        jsonElementDict.SetString("filename", Path.GetFileName(assetBrandAssetItem.item.path));
        assetBrandAssetItem.item.Write(warnings);
      }
      doc.WriteToFile(Path.Combine(this.m_Path, "Contents.json"));
    }

    private class AssetBrandAssetItem
    {
      internal string idiom = (string) null;
      internal string role = (string) null;
      internal AssetCatalogItem item = (AssetCatalogItem) null;
      internal int width;
      internal int height;
    }
  }
}
