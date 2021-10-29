// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXBuildFileData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXBuildFileData : PBXObjectData
  {
    public string fileRef;
    public string compileFlags;
    public bool weak;
    public bool codeSignOnCopy;
    public bool removeHeadersOnCopy;
    public List<string> assetTags;
    private static PropertyCommentChecker checkerData;

    internal override PropertyCommentChecker checker
    {
      get
      {
        return PBXBuildFileData.checkerData;
      }
    }

    internal override bool shouldCompact
    {
      get
      {
        return true;
      }
    }

    static PBXBuildFileData()
    {
      string[] strArray = new string[1];
      int index = 0;
      string str = "fileRef/*";
      strArray[index] = str;
      PBXBuildFileData.checkerData = new PropertyCommentChecker((IEnumerable<string>) strArray);
    }

    public static PBXBuildFileData CreateFromFile(string fileRefGUID, bool weak, string compileFlags)
    {
      PBXBuildFileData pbxBuildFileData = new PBXBuildFileData();
      pbxBuildFileData.guid = PBXGUID.Generate();
      pbxBuildFileData.SetPropertyString("isa", "PBXBuildFile");
      pbxBuildFileData.fileRef = fileRefGUID;
      pbxBuildFileData.compileFlags = compileFlags;
      pbxBuildFileData.weak = weak;
      pbxBuildFileData.codeSignOnCopy = false;
      pbxBuildFileData.removeHeadersOnCopy = false;
      pbxBuildFileData.assetTags = new List<string>();
      return pbxBuildFileData;
    }

    private PBXElementDict UpdatePropsAttribute(PBXElementDict settings, bool value, string attributeName)
    {
            PBXElementArray pbxElementArray = (PBXElementArray)null;
            if (value && settings == null)
                settings = this.m_Properties.CreateDict("settings");
            if (settings != null && settings.Contains("ATTRIBUTES"))
                pbxElementArray = settings["ATTRIBUTES"].AsArray();
            if (value)
            {
                if (pbxElementArray == null)
                    pbxElementArray = settings.CreateArray("ATTRIBUTES");
                if (!Enumerable.Any<PBXElement>((IEnumerable<PBXElement>)pbxElementArray.values, (Func<PBXElement, bool>)(attr => attr is PBXElementString && attr.AsString() == attributeName)))
                    pbxElementArray.AddString(attributeName);
            }
            else if (pbxElementArray != null)
            {
                pbxElementArray.values.RemoveAll((Predicate<PBXElement>)(el =>
                {
                    if (el is PBXElementString)
                        return el.AsString() == attributeName;
                    return false;
                }));
                if (pbxElementArray.values.Count == 0)
                    settings.Remove("ATTRIBUTES");
            }
            return settings;
        }

    public override void UpdateProps()
    {
      this.SetPropertyString("fileRef", this.fileRef);
      PBXElementDict settings = (PBXElementDict) null;
      if (this.m_Properties.Contains("settings"))
        settings = this.m_Properties["settings"].AsDict();
      if (this.compileFlags != null && this.compileFlags != "")
      {
        if (settings == null)
          settings = this.m_Properties.CreateDict("settings");
        settings.SetString("COMPILER_FLAGS", this.compileFlags);
      }
      else if (settings != null)
        settings.Remove("COMPILER_FLAGS");
      PBXElementDict pbxElementDict = this.UpdatePropsAttribute(this.UpdatePropsAttribute(this.UpdatePropsAttribute(settings, this.weak, "Weak"), this.codeSignOnCopy, "CodeSignOnCopy"), this.removeHeadersOnCopy, "RemoveHeadersOnCopy");
      if (this.assetTags.Count > 0)
      {
        if (pbxElementDict == null)
          pbxElementDict = this.m_Properties.CreateDict("settings");
        PBXElementArray array = pbxElementDict.CreateArray("ASSET_TAGS");
        foreach (string val in this.assetTags)
          array.AddString(val);
      }
      else if (pbxElementDict != null)
        pbxElementDict.Remove("ASSET_TAGS");
      if (pbxElementDict == null || pbxElementDict.values.Count != 0)
        return;
      this.m_Properties.Remove("settings");
    }

    public override void UpdateVars()
    {
      this.fileRef = this.GetPropertyString("fileRef");
      this.compileFlags = (string) null;
      this.weak = false;
      this.assetTags = new List<string>();
      if (!this.m_Properties.Contains("settings"))
        return;
      PBXElementDict pbxElementDict = this.m_Properties["settings"].AsDict();
      if (pbxElementDict.Contains("COMPILER_FLAGS"))
        this.compileFlags = pbxElementDict["COMPILER_FLAGS"].AsString();
      if (pbxElementDict.Contains("ATTRIBUTES"))
      {
        foreach (PBXElement pbxElement in pbxElementDict["ATTRIBUTES"].AsArray().values)
        {
          if (pbxElement is PBXElementString && pbxElement.AsString() == "Weak")
            this.weak = true;
          if (pbxElement is PBXElementString && pbxElement.AsString() == "CodeSignOnCopy")
            this.codeSignOnCopy = true;
          if (pbxElement is PBXElementString && pbxElement.AsString() == "RemoveHeadersOnCopy")
            this.removeHeadersOnCopy = true;
        }
      }
      if (pbxElementDict.Contains("ASSET_TAGS"))
      {
        foreach (PBXElement pbxElement in pbxElementDict["ASSET_TAGS"].AsArray().values)
          this.assetTags.Add(pbxElement.AsString());
      }
    }
  }
}
