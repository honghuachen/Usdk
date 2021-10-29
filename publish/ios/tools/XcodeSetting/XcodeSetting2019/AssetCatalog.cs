// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.AssetCatalog
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityEditor.iOS.Xcode
{
  internal class AssetCatalog
  {
    private AssetFolder m_Root;

    public string path
    {
      get
      {
        return this.m_Root.path;
      }
    }

    public AssetFolder root
    {
      get
      {
        return this.m_Root;
      }
    }

    public AssetCatalog(string path, string authorId)
    {
      if (Path.GetExtension(path) != ".xcassets")
        throw new Exception("Asset catalogs must have xcassets extension");
      this.m_Root = new AssetFolder(path, (string) null, authorId);
    }

    private AssetFolder OpenFolderForResource(string relativePath)
    {
      List<string> list = Enumerable.ToList<string>((IEnumerable<string>) PBXPath.Split(relativePath));
      list.RemoveAt(list.Count - 1);
      AssetFolder assetFolder = this.root;
      foreach (string name in list)
        assetFolder = assetFolder.OpenFolder(name);
      return assetFolder;
    }

    public AssetDataSet OpenDataSet(string relativePath)
    {
      return this.OpenFolderForResource(relativePath).OpenDataSet(Path.GetFileName(relativePath));
    }

    public AssetImageSet OpenImageSet(string relativePath)
    {
      return this.OpenFolderForResource(relativePath).OpenImageSet(Path.GetFileName(relativePath));
    }

    public AssetImageStack OpenImageStack(string relativePath)
    {
      return this.OpenFolderForResource(relativePath).OpenImageStack(Path.GetFileName(relativePath));
    }

    public AssetBrandAssetGroup OpenBrandAssetGroup(string relativePath)
    {
      return this.OpenFolderForResource(relativePath).OpenBrandAssetGroup(Path.GetFileName(relativePath));
    }

    public AssetFolder OpenFolder(string relativePath)
    {
      if (relativePath == null)
        return this.root;
      string[] strArray = PBXPath.Split(relativePath);
      if (strArray.Length == 0)
        return this.root;
      AssetFolder assetFolder = this.root;
      foreach (string name in strArray)
        assetFolder = assetFolder.OpenFolder(name);
      return assetFolder;
    }

    public AssetFolder OpenNamespacedFolder(string relativeBasePath, string namespacePath)
    {
      AssetFolder assetFolder = this.OpenFolder(relativeBasePath);
      foreach (string name in PBXPath.Split(namespacePath))
      {
        assetFolder = assetFolder.OpenFolder(name);
        assetFolder.providesNamespace = true;
      }
      return assetFolder;
    }

    public void Write()
    {
      this.Write((List<string>) null);
    }

    public void Write(List<string> warnings)
    {
      this.m_Root.Write(warnings);
    }
  }
}
