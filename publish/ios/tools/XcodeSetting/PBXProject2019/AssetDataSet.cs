// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.AssetDataSet
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.iOS.Xcode
{
  internal class AssetDataSet : AssetCatalogItemWithVariants
  {
    internal AssetDataSet(string parentPath, string name, string authorId)
      : base(name, authorId)
    {
      this.m_Path = Path.Combine(parentPath, name + ".dataset");
    }

    public void AddVariant(DeviceRequirement requirement, string path, string typeIdentifier)
    {
      foreach (AssetDataSet.DataSetVariant dataSetVariant in this.m_Variants)
      {
        if (dataSetVariant.id != null && typeIdentifier != null && dataSetVariant.id == typeIdentifier)
          throw new Exception("Two items within the same dataset must not have the same id");
      }
      this.AddVariant((AssetCatalogItemWithVariants.VariantData) new AssetDataSet.DataSetVariant(requirement, path, typeIdentifier));
    }

    public override void Write(List<string> warnings)
    {
      Directory.CreateDirectory(this.m_Path);
      JsonDocument doc = new JsonDocument();
      this.WriteODRTagsToJson(this.WriteInfoToJson(doc));
      JsonElementArray array = doc.root.CreateArray("data");
      HashSet<string> existingFilenames = new HashSet<string>();
      foreach (AssetDataSet.DataSetVariant dataSetVariant in this.m_Variants)
      {
        string val = this.CopyFileToSet(dataSetVariant.path, existingFilenames, warnings);
        JsonElementDict jsonElementDict = array.AddDict();
        jsonElementDict.SetString("filename", val);
        this.WriteRequirementsToJson(jsonElementDict, dataSetVariant.requirement);
        if (dataSetVariant.id != null)
          jsonElementDict.SetString("universal-type-identifier", dataSetVariant.id);
      }
      doc.WriteToFile(Path.Combine(this.m_Path, "Contents.json"));
    }

    private class DataSetVariant : AssetCatalogItemWithVariants.VariantData
    {
      public string id;

      public DataSetVariant(DeviceRequirement requirement, string path, string id)
        : base(requirement, path)
      {
        this.id = id;
      }
    }
  }
}
