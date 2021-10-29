// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.AssetImageStackLayer
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;
using System.IO;

namespace UnityEditor.iOS.Xcode
{
  internal class AssetImageStackLayer : AssetCatalogItem
  {
    private AssetImageSet m_Imageset = (AssetImageSet) null;
    private string m_ReferencedName = (string) null;

    internal AssetImageStackLayer(string assetCatalogPath, string name, string authorId)
      : base(name, authorId)
    {
      this.m_Path = Path.Combine(assetCatalogPath, name + ".imagestacklayer");
      this.m_Imageset = new AssetImageSet(this.m_Path, "Content", authorId);
    }

    public void SetReference(string name)
    {
      this.m_Imageset = (AssetImageSet) null;
      this.m_ReferencedName = name;
    }

    public string ReferencedName()
    {
      return this.m_ReferencedName;
    }

    public AssetImageSet GetImageSet()
    {
      return this.m_Imageset;
    }

    public override void Write(List<string> warnings)
    {
      Directory.CreateDirectory(this.m_Path);
      JsonDocument doc = new JsonDocument();
      this.WriteInfoToJson(doc);
      if (this.m_ReferencedName != null)
      {
        JsonElementDict dict = doc.root.CreateDict("properties").CreateDict("content-reference");
        dict.SetString("type", "image-set");
        dict.SetString("name", this.m_ReferencedName);
        dict.SetString("matching-style", "fully-qualified-name");
      }
      if (this.m_Imageset != null)
        this.m_Imageset.Write(warnings);
      doc.WriteToFile(Path.Combine(this.m_Path, "Contents.json"));
    }
  }
}
