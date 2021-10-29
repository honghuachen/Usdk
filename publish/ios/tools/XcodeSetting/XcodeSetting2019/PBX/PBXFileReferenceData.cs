// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXFileReferenceData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.IO;
using UnityEditor.iOS.Xcode;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXFileReferenceData : PBXObjectData
  {
    private string m_Path = (string) null;
    private string m_ExplicitFileType = (string) null;
    private string m_LastKnownFileType = (string) null;
    public string name;
    public PBXSourceTree tree;

    public string path
    {
      get
      {
        return this.m_Path;
      }
      set
      {
        this.m_ExplicitFileType = (string) null;
        this.m_LastKnownFileType = (string) null;
        this.m_Path = value;
      }
    }

    public bool isFolderReference
    {
      get
      {
        return this.m_LastKnownFileType != null && this.m_LastKnownFileType == "folder";
      }
    }

    internal override bool shouldCompact
    {
      get
      {
        return true;
      }
    }

    public static PBXFileReferenceData CreateFromFile(string path, string projectFileName, PBXSourceTree tree)
    {
      string str = PBXGUID.Generate();
      PBXFileReferenceData fileReferenceData = new PBXFileReferenceData();
      fileReferenceData.SetPropertyString("isa", "PBXFileReference");
      fileReferenceData.guid = str;
      fileReferenceData.path = path;
      fileReferenceData.name = projectFileName;
      fileReferenceData.tree = tree;
      return fileReferenceData;
    }

    public static PBXFileReferenceData CreateFromFolderReference(string path, string projectFileName, PBXSourceTree tree)
    {
      PBXFileReferenceData fromFile = PBXFileReferenceData.CreateFromFile(path, projectFileName, tree);
      fromFile.m_LastKnownFileType = "folder";
      return fromFile;
    }

    public override void UpdateProps()
    {
      string ext = (string) null;
      if (this.m_ExplicitFileType != null)
        this.SetPropertyString("explicitFileType", this.m_ExplicitFileType);
      else if (this.m_LastKnownFileType != null)
      {
        this.SetPropertyString("lastKnownFileType", this.m_LastKnownFileType);
      }
      else
      {
        if (this.name != null)
          ext = Path.GetExtension(this.name);
        else if (this.m_Path != null)
          ext = Path.GetExtension(this.m_Path);
        if (ext != null)
        {
          if (FileTypeUtils.IsFileTypeExplicit(ext))
            this.SetPropertyString("explicitFileType", FileTypeUtils.GetTypeName(ext));
          else
            this.SetPropertyString("lastKnownFileType", FileTypeUtils.GetTypeName(ext));
        }
      }
      if (this.m_Path == this.name)
        this.SetPropertyString("name", (string) null);
      else
        this.SetPropertyString("name", this.name);
      if (this.m_Path == null)
        this.SetPropertyString("path", "");
      else
        this.SetPropertyString("path", this.m_Path);
      this.SetPropertyString("sourceTree", FileTypeUtils.SourceTreeDesc(this.tree));
    }

    public override void UpdateVars()
    {
      this.name = this.GetPropertyString("name");
      this.m_Path = this.GetPropertyString("path");
      if (this.name == null)
        this.name = this.m_Path;
      if (this.m_Path == null)
        this.m_Path = "";
      this.tree = FileTypeUtils.ParseSourceTree(this.GetPropertyString("sourceTree"));
      this.m_ExplicitFileType = this.GetPropertyString("explicitFileType");
      this.m_LastKnownFileType = this.GetPropertyString("lastKnownFileType");
    }
  }
}
