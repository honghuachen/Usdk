// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.AssetCatalogItemWithVariants
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.iOS.Xcode
{
  internal abstract class AssetCatalogItemWithVariants : AssetCatalogItem
  {
    protected List<AssetCatalogItemWithVariants.VariantData> m_Variants = new List<AssetCatalogItemWithVariants.VariantData>();
    protected List<string> m_ODRTags = new List<string>();

    protected AssetCatalogItemWithVariants(string name, string authorId)
      : base(name, authorId)
    {
    }

    public bool HasVariant(DeviceRequirement requirement)
    {
      foreach (AssetCatalogItemWithVariants.VariantData variantData in this.m_Variants)
      {
        if (variantData.requirement.values == requirement.values)
          return true;
      }
      return false;
    }

    public void AddOnDemandResourceTag(string tag)
    {
      if (this.m_ODRTags.Contains(tag))
        return;
      this.m_ODRTags.Add(tag);
    }

    protected void AddVariant(AssetCatalogItemWithVariants.VariantData newItem)
    {
      foreach (AssetCatalogItemWithVariants.VariantData variantData in this.m_Variants)
      {
        if (variantData.requirement.values == newItem.requirement.values)
          throw new Exception("The given requirement has been already added");
        if (Path.GetFileName(variantData.path) == Path.GetFileName(this.path))
          throw new Exception("Two items within the same set must not have the same file name");
      }
      if (Path.GetFileName(newItem.path) == "Contents.json")
        throw new Exception("The file name must not be equal to Contents.json");
      this.m_Variants.Add(newItem);
    }

    protected void WriteODRTagsToJson(JsonElementDict info)
    {
      if (this.m_ODRTags.Count <= 0)
        return;
      JsonElementArray array = info.CreateArray("on-demand-resource-tags");
      foreach (string val in this.m_ODRTags)
        array.AddString(val);
    }

    protected void WriteRequirementsToJson(JsonElementDict item, DeviceRequirement req)
    {
      foreach (KeyValuePair<string, string> keyValuePair in req.values)
      {
        if (keyValuePair.Value != null && keyValuePair.Value != "")
          item.SetString(keyValuePair.Key, keyValuePair.Value);
      }
    }

    protected string CopyFileToSet(string path, HashSet<string> existingFilenames, List<string> warnings)
    {
      string str = Path.GetFileName(path);
      if (!File.Exists(path))
      {
        if (warnings != null)
          warnings.Add("File not found: " + path);
      }
      else
      {
        int num = 1;
        string withoutExtension = Path.GetFileNameWithoutExtension(str);
        string extension = Path.GetExtension(str);
        while (existingFilenames.Contains(str))
        {
          str = string.Format("{0}-{1}{2}", (object) withoutExtension, (object) num, (object) extension);
          ++num;
        }
        existingFilenames.Add(str);
        File.Copy(path, Path.Combine(this.m_Path, str));
      }
      return str;
    }

    protected class VariantData
    {
      public DeviceRequirement requirement;
      public string path;

      public VariantData(DeviceRequirement requirement, string path)
      {
        this.requirement = requirement;
        this.path = path;
      }
    }
  }
}
