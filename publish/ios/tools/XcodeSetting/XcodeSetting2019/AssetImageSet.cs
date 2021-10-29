// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.AssetImageSet
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;
using System.IO;

namespace UnityEditor.iOS.Xcode
{
  internal class AssetImageSet : AssetCatalogItemWithVariants
  {
    internal AssetImageSet(string assetCatalogPath, string name, string authorId)
      : base(name, authorId)
    {
      this.m_Path = Path.Combine(assetCatalogPath, name + ".imageset");
    }

    public void AddVariant(DeviceRequirement requirement, string path)
    {
      this.AddVariant((AssetCatalogItemWithVariants.VariantData) new AssetImageSet.ImageSetVariant(requirement, path));
    }

    public void AddVariant(DeviceRequirement requirement, string path, ImageAlignment alignment, ImageResizing resizing)
    {
      this.AddVariant((AssetCatalogItemWithVariants.VariantData) new AssetImageSet.ImageSetVariant(requirement, path)
      {
        alignment = alignment,
        resizing = resizing
      });
    }

    private void WriteAlignmentToJson(JsonElementDict item, ImageAlignment alignment)
    {
      JsonElementDict dict = item.CreateDict("alignment-insets");
      dict.SetInteger("top", alignment.top);
      dict.SetInteger("bottom", alignment.bottom);
      dict.SetInteger("left", alignment.left);
      dict.SetInteger("right", alignment.right);
    }

    private static string GetSlicingMode(ImageResizing.SlicingType mode)
    {
      switch (mode)
      {
        case ImageResizing.SlicingType.Horizontal:
          return "3-part-horizontal";
        case ImageResizing.SlicingType.Vertical:
          return "3-part-vertical";
        case ImageResizing.SlicingType.HorizontalAndVertical:
          return "9-part";
        default:
          return "";
      }
    }

    private static string GetCenterResizeMode(ImageResizing.ResizeMode mode)
    {
      switch (mode)
      {
        case ImageResizing.ResizeMode.Stretch:
          return "stretch";
        case ImageResizing.ResizeMode.Tile:
          return "tile";
        default:
          return "";
      }
    }

    private void WriteResizingToJson(JsonElementDict item, ImageResizing resizing)
    {
      JsonElementDict dict1 = item.CreateDict("resizing");
      dict1.SetString("mode", AssetImageSet.GetSlicingMode(resizing.type));
      JsonElementDict dict2 = dict1.CreateDict("center");
      dict2.SetString("mode", AssetImageSet.GetCenterResizeMode(resizing.centerResizeMode));
      dict2.SetInteger("width", resizing.centerWidth);
      dict2.SetInteger("height", resizing.centerHeight);
      JsonElementDict dict3 = dict1.CreateDict("cap-insets");
      dict3.SetInteger("top", resizing.top);
      dict3.SetInteger("bottom", resizing.bottom);
      dict3.SetInteger("left", resizing.left);
      dict3.SetInteger("right", resizing.right);
    }

    public override void Write(List<string> warnings)
    {
      Directory.CreateDirectory(this.m_Path);
      JsonDocument doc = new JsonDocument();
      this.WriteODRTagsToJson(this.WriteInfoToJson(doc));
      JsonElementArray array = doc.root.CreateArray("images");
      HashSet<string> existingFilenames = new HashSet<string>();
      foreach (AssetImageSet.ImageSetVariant imageSetVariant in this.m_Variants)
      {
        string val = this.CopyFileToSet(imageSetVariant.path, existingFilenames, warnings);
        JsonElementDict jsonElementDict = array.AddDict();
        jsonElementDict.SetString("filename", val);
        this.WriteRequirementsToJson(jsonElementDict, imageSetVariant.requirement);
        if (imageSetVariant.alignment != null)
          this.WriteAlignmentToJson(jsonElementDict, imageSetVariant.alignment);
        if (imageSetVariant.resizing != null)
          this.WriteResizingToJson(jsonElementDict, imageSetVariant.resizing);
      }
      doc.WriteToFile(Path.Combine(this.m_Path, "Contents.json"));
    }

    private class ImageSetVariant : AssetCatalogItemWithVariants.VariantData
    {
      public ImageAlignment alignment = (ImageAlignment) null;
      public ImageResizing resizing = (ImageResizing) null;

      public ImageSetVariant(DeviceRequirement requirement, string path)
        : base(requirement, path)
      {
      }
    }
  }
}
