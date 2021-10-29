// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.AssetImageStack
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.iOS.Xcode
{
  internal class AssetImageStack : AssetCatalogItem
  {
    private List<AssetImageStackLayer> m_Layers = new List<AssetImageStackLayer>();

    internal AssetImageStack(string assetCatalogPath, string name, string authorId)
      : base(name, authorId)
    {
      this.m_Path = Path.Combine(assetCatalogPath, name + ".imagestack");
    }

    public AssetImageStackLayer AddLayer(string name)
    {
      foreach (AssetCatalogItem assetCatalogItem in this.m_Layers)
      {
        if (assetCatalogItem.name == name)
          throw new Exception("A layer with given name already exists");
      }
      AssetImageStackLayer assetImageStackLayer = new AssetImageStackLayer(this.m_Path, name, this.authorId);
      this.m_Layers.Add(assetImageStackLayer);
      return assetImageStackLayer;
    }

    public override void Write(List<string> warnings)
    {
      Directory.CreateDirectory(this.m_Path);
      JsonDocument doc = new JsonDocument();
      this.WriteInfoToJson(doc);
      JsonElementArray array = doc.root.CreateArray("layers");
      foreach (AssetImageStackLayer assetImageStackLayer in this.m_Layers)
      {
        assetImageStackLayer.Write(warnings);
        array.AddDict().SetString("filename", Path.GetFileName(assetImageStackLayer.path));
      }
      doc.WriteToFile(Path.Combine(this.m_Path, "Contents.json"));
    }
  }
}
