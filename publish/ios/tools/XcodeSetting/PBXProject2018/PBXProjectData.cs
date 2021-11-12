// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBXProjectData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.iOS.Xcode.PBX;

namespace UnityEditor.iOS.Xcode
{
  internal class PBXProjectData
  {
    private Dictionary<string, SectionBase> m_Section = (Dictionary<string, SectionBase>) null;
    private PBXElementDict m_RootElements = (PBXElementDict) null;
    private PBXElementDict m_UnknownObjects = (PBXElementDict) null;
    private string m_ObjectVersion = (string) null;
    private List<string> m_SectionOrder = (List<string>) null;
    private KnownSectionBase<PBXBuildFileData> buildFiles = (KnownSectionBase<PBXBuildFileData>) null;
    private KnownSectionBase<PBXFileReferenceData> fileRefs = (KnownSectionBase<PBXFileReferenceData>) null;
    private KnownSectionBase<PBXGroupData> groups = (KnownSectionBase<PBXGroupData>) null;
    public KnownSectionBase<PBXContainerItemProxyData> containerItems = (KnownSectionBase<PBXContainerItemProxyData>) null;
    public KnownSectionBase<PBXReferenceProxyData> references = (KnownSectionBase<PBXReferenceProxyData>) null;
    public KnownSectionBase<PBXSourcesBuildPhaseData> sources = (KnownSectionBase<PBXSourcesBuildPhaseData>) null;
    public KnownSectionBase<PBXFrameworksBuildPhaseData> frameworks = (KnownSectionBase<PBXFrameworksBuildPhaseData>) null;
    public KnownSectionBase<PBXResourcesBuildPhaseData> resources = (KnownSectionBase<PBXResourcesBuildPhaseData>) null;
    public KnownSectionBase<PBXCopyFilesBuildPhaseData> copyFiles = (KnownSectionBase<PBXCopyFilesBuildPhaseData>) null;
    public KnownSectionBase<PBXShellScriptBuildPhaseData> shellScripts = (KnownSectionBase<PBXShellScriptBuildPhaseData>) null;
    public KnownSectionBase<PBXNativeTargetData> nativeTargets = (KnownSectionBase<PBXNativeTargetData>) null;
    public KnownSectionBase<PBXTargetDependencyData> targetDependencies = (KnownSectionBase<PBXTargetDependencyData>) null;
    public KnownSectionBase<PBXVariantGroupData> variantGroups = (KnownSectionBase<PBXVariantGroupData>) null;
    public KnownSectionBase<XCBuildConfigurationData> buildConfigs = (KnownSectionBase<XCBuildConfigurationData>) null;
    public KnownSectionBase<XCConfigurationListData> buildConfigLists = (KnownSectionBase<XCConfigurationListData>) null;
    public PBXProjectSection project = (PBXProjectSection) null;
    private Dictionary<string, Dictionary<string, PBXBuildFileData>> m_FileGuidToBuildFileMap = (Dictionary<string, Dictionary<string, PBXBuildFileData>>) null;
    private Dictionary<string, PBXFileReferenceData> m_ProjectPathToFileRefMap = (Dictionary<string, PBXFileReferenceData>) null;
    private Dictionary<string, string> m_FileRefGuidToProjectPathMap = (Dictionary<string, string>) null;
    private Dictionary<PBXSourceTree, Dictionary<string, PBXFileReferenceData>> m_RealPathToFileRefMap = (Dictionary<PBXSourceTree, Dictionary<string, PBXFileReferenceData>>) null;
    private Dictionary<string, PBXGroupData> m_ProjectPathToGroupMap = (Dictionary<string, PBXGroupData>) null;
    private Dictionary<string, string> m_GroupGuidToProjectPathMap = (Dictionary<string, string>) null;
    private Dictionary<string, PBXGroupData> m_GuidToParentGroupMap = (Dictionary<string, PBXGroupData>) null;
    private Dictionary<string, KnownSectionBase<PBXObjectData>> m_UnknownSections;

    public PBXBuildFileData BuildFilesGet(string guid)
    {
      return this.buildFiles[guid];
    }

    public void BuildFilesAdd(string targetGuid, PBXBuildFileData buildFile)
    {
      if (!this.m_FileGuidToBuildFileMap.ContainsKey(targetGuid))
        this.m_FileGuidToBuildFileMap[targetGuid] = new Dictionary<string, PBXBuildFileData>();
      this.m_FileGuidToBuildFileMap[targetGuid][buildFile.fileRef] = buildFile;
      this.buildFiles.AddEntry(buildFile);
    }

    public void BuildFilesRemove(string targetGuid, string fileGuid)
    {
      PBXBuildFileData forSourceFile = this.BuildFilesGetForSourceFile(targetGuid, fileGuid);
      if (forSourceFile == null)
        return;
      this.m_FileGuidToBuildFileMap[targetGuid].Remove(forSourceFile.fileRef);
      this.buildFiles.RemoveEntry(forSourceFile.guid);
    }

    public PBXBuildFileData BuildFilesGetForSourceFile(string targetGuid, string fileGuid)
    {
      if (!this.m_FileGuidToBuildFileMap.ContainsKey(targetGuid) || !this.m_FileGuidToBuildFileMap[targetGuid].ContainsKey(fileGuid))
        return (PBXBuildFileData) null;
      return this.m_FileGuidToBuildFileMap[targetGuid][fileGuid];
    }

    public IEnumerable<PBXBuildFileData> BuildFilesGetAll()
    {
      return this.buildFiles.GetObjects();
    }

    public void FileRefsAdd(string realPath, string projectPath, PBXGroupData parent, PBXFileReferenceData fileRef)
    {
      this.fileRefs.AddEntry(fileRef);
      this.m_ProjectPathToFileRefMap.Add(projectPath, fileRef);
      this.m_FileRefGuidToProjectPathMap.Add(fileRef.guid, projectPath);
      this.m_RealPathToFileRefMap[fileRef.tree].Add(realPath, fileRef);
      this.m_GuidToParentGroupMap.Add(fileRef.guid, parent);
    }

    public PBXFileReferenceData FileRefsGet(string guid)
    {
      return this.fileRefs[guid];
    }

    public PBXFileReferenceData FileRefsGetByRealPath(string path, PBXSourceTree sourceTree)
    {
      if (this.m_RealPathToFileRefMap[sourceTree].ContainsKey(path))
        return this.m_RealPathToFileRefMap[sourceTree][path];
      return (PBXFileReferenceData) null;
    }

    internal List<string> GetRealPathsRelativeToSourceTree(PBXSourceTree sourceTree)
    {
      if (this.m_RealPathToFileRefMap[sourceTree] != null)
        return new List<string>((IEnumerable<string>) this.m_RealPathToFileRefMap[sourceTree].Keys);
      return (List<string>) null;
    }

    public PBXFileReferenceData FileRefsGetByProjectPath(string path)
    {
      if (this.m_ProjectPathToFileRefMap.ContainsKey(path))
        return this.m_ProjectPathToFileRefMap[path];
      return (PBXFileReferenceData) null;
    }

    public void FileRefsRemove(string guid)
    {
      PBXFileReferenceData fileReferenceData = this.fileRefs[guid];
      this.fileRefs.RemoveEntry(guid);
      this.m_ProjectPathToFileRefMap.Remove(this.m_FileRefGuidToProjectPathMap[guid]);
      this.m_FileRefGuidToProjectPathMap.Remove(guid);
      foreach (PBXSourceTree index in FileTypeUtils.AllAbsoluteSourceTrees())
        this.m_RealPathToFileRefMap[index].Remove(fileReferenceData.path);
      this.m_GuidToParentGroupMap.Remove(guid);
    }

    public PBXGroupData GroupsGet(string guid)
    {
      return this.groups[guid];
    }

    public PBXGroupData GroupsGetByChild(string childGuid)
    {
      return this.m_GuidToParentGroupMap[childGuid];
    }

    public PBXGroupData GroupsGetMainGroup()
    {
      return this.groups[this.project.project.mainGroup];
    }

    public PBXGroupData GroupsGetByProjectPath(string sourceGroup)
    {
      if (this.m_ProjectPathToGroupMap.ContainsKey(sourceGroup))
        return this.m_ProjectPathToGroupMap[sourceGroup];
      return (PBXGroupData) null;
    }

    public void GroupsAdd(string projectPath, PBXGroupData parent, PBXGroupData gr)
    {
      this.m_ProjectPathToGroupMap.Add(projectPath, gr);
      this.m_GroupGuidToProjectPathMap.Add(gr.guid, projectPath);
      this.m_GuidToParentGroupMap.Add(gr.guid, parent);
      this.groups.AddEntry(gr);
    }

    public void GroupsAddDuplicate(PBXGroupData gr)
    {
      this.groups.AddEntry(gr);
    }

    public void GroupsRemove(string guid)
    {
      this.m_ProjectPathToGroupMap.Remove(this.m_GroupGuidToProjectPathMap[guid]);
      this.m_GroupGuidToProjectPathMap.Remove(guid);
      this.m_GuidToParentGroupMap.Remove(guid);
      this.groups.RemoveEntry(guid);
    }

    public FileGUIDListBase BuildSectionAny(PBXNativeTargetData target, string path, bool isFolderRef)
    {
      switch (FileTypeUtils.GetFileType(Path.GetExtension(path), isFolderRef))
      {
        case PBXFileType.NotBuildable:
          return (FileGUIDListBase) null;
        case PBXFileType.Framework:
          using (IEnumerator<string> enumerator = ((IEnumerable<string>) target.phases).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              if (this.frameworks.HasEntry(current))
                return (FileGUIDListBase) this.frameworks[current];
            }
            break;
          }
        case PBXFileType.Source:
          using (IEnumerator<string> enumerator = ((IEnumerable<string>) target.phases).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              if (this.sources.HasEntry(current))
                return (FileGUIDListBase) this.sources[current];
            }
            break;
          }
        case PBXFileType.Resource:
          using (IEnumerator<string> enumerator = ((IEnumerable<string>) target.phases).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              if (this.resources.HasEntry(current))
                return (FileGUIDListBase) this.resources[current];
            }
            break;
          }
        case PBXFileType.CopyFile:
          using (IEnumerator<string> enumerator = ((IEnumerable<string>) target.phases).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              if (this.copyFiles.HasEntry(current))
                return (FileGUIDListBase) this.copyFiles[current];
            }
            break;
          }
        case PBXFileType.ShellScript:
          using (IEnumerator<string> enumerator = ((IEnumerable<string>) target.phases).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              if (this.shellScripts.HasEntry(current))
                return (FileGUIDListBase) this.shellScripts[current];
            }
            break;
          }
      }
      throw new Exception(string.Format("The given path {0} does not refer to a file in a known build section", (object) path));
    }

    public FileGUIDListBase BuildSectionAny(string sectionGuid)
    {
      if (this.frameworks.HasEntry(sectionGuid))
        return (FileGUIDListBase) this.frameworks[sectionGuid];
      if (this.resources.HasEntry(sectionGuid))
        return (FileGUIDListBase) this.resources[sectionGuid];
      if (this.sources.HasEntry(sectionGuid))
        return (FileGUIDListBase) this.sources[sectionGuid];
      if (this.copyFiles.HasEntry(sectionGuid))
        return (FileGUIDListBase) this.copyFiles[sectionGuid];
      if (this.shellScripts.HasEntry(sectionGuid))
        return (FileGUIDListBase) this.shellScripts[sectionGuid];
      return (FileGUIDListBase) null;
    }

    private void RefreshBuildFilesMapForBuildFileGuidList(Dictionary<string, PBXBuildFileData> mapForTarget, FileGUIDListBase list)
    {
      foreach (string index in (IEnumerable<string>) list.files)
      {
        PBXBuildFileData pbxBuildFileData = this.buildFiles[index];
        mapForTarget[pbxBuildFileData.fileRef] = pbxBuildFileData;
      }
    }

    private void RefreshMapsForGroupChildren(string projectPath, string realPath, PBXSourceTree realPathTree, PBXGroupData parent)
    {
      foreach (string key1 in new List<string>((IEnumerable<string>) parent.children))
      {
        PBXFileReferenceData fileReferenceData = this.fileRefs[key1];
        string resPath;
        PBXSourceTree resTree;
        if (fileReferenceData != null)
        {
          string key2 = PBXPath.Combine(projectPath, fileReferenceData.name);
          PBXPath.Combine(realPath, realPathTree, fileReferenceData.path, fileReferenceData.tree, out resPath, out resTree);
          if (!this.m_ProjectPathToFileRefMap.ContainsKey(key2))
            this.m_ProjectPathToFileRefMap.Add(key2, fileReferenceData);
          if (!this.m_FileRefGuidToProjectPathMap.ContainsKey(fileReferenceData.guid))
            this.m_FileRefGuidToProjectPathMap.Add(fileReferenceData.guid, key2);
          if (!this.m_RealPathToFileRefMap[resTree].ContainsKey(resPath))
            this.m_RealPathToFileRefMap[resTree].Add(resPath, fileReferenceData);
          if (!this.m_GuidToParentGroupMap.ContainsKey(key1))
            this.m_GuidToParentGroupMap.Add(key1, parent);
        }
        else
        {
          PBXGroupData parent1 = this.groups[key1];
          if (parent1 != null)
          {
            string str = PBXPath.Combine(projectPath, parent1.name);
            PBXPath.Combine(realPath, realPathTree, parent1.path, parent1.tree, out resPath, out resTree);
            if (!this.m_ProjectPathToGroupMap.ContainsKey(str))
              this.m_ProjectPathToGroupMap.Add(str, parent1);
            if (!this.m_GroupGuidToProjectPathMap.ContainsKey(parent1.guid))
              this.m_GroupGuidToProjectPathMap.Add(parent1.guid, str);
            if (!this.m_GuidToParentGroupMap.ContainsKey(key1))
              this.m_GuidToParentGroupMap.Add(key1, parent);
            this.RefreshMapsForGroupChildren(str, resPath, resTree, parent1);
          }
        }
      }
    }

    private void RefreshAuxMaps()
    {
      foreach (KeyValuePair<string, PBXNativeTargetData> keyValuePair in this.nativeTargets.GetEntries())
      {
        Dictionary<string, PBXBuildFileData> mapForTarget = new Dictionary<string, PBXBuildFileData>();
        foreach (string guid in (IEnumerable<string>) keyValuePair.Value.phases)
        {
          if (this.frameworks.HasEntry(guid))
            this.RefreshBuildFilesMapForBuildFileGuidList(mapForTarget, (FileGUIDListBase) this.frameworks[guid]);
          if (this.resources.HasEntry(guid))
            this.RefreshBuildFilesMapForBuildFileGuidList(mapForTarget, (FileGUIDListBase) this.resources[guid]);
          if (this.sources.HasEntry(guid))
            this.RefreshBuildFilesMapForBuildFileGuidList(mapForTarget, (FileGUIDListBase) this.sources[guid]);
          if (this.copyFiles.HasEntry(guid))
            this.RefreshBuildFilesMapForBuildFileGuidList(mapForTarget, (FileGUIDListBase) this.copyFiles[guid]);
        }
        this.m_FileGuidToBuildFileMap[keyValuePair.Key] = mapForTarget;
      }
      this.RefreshMapsForGroupChildren("", "", PBXSourceTree.Source, this.GroupsGetMainGroup());
    }

    public void Clear()
    {
      this.buildFiles = new KnownSectionBase<PBXBuildFileData>("PBXBuildFile");
      this.fileRefs = new KnownSectionBase<PBXFileReferenceData>("PBXFileReference");
      this.groups = new KnownSectionBase<PBXGroupData>("PBXGroup");
      this.containerItems = new KnownSectionBase<PBXContainerItemProxyData>("PBXContainerItemProxy");
      this.references = new KnownSectionBase<PBXReferenceProxyData>("PBXReferenceProxy");
      this.sources = new KnownSectionBase<PBXSourcesBuildPhaseData>("PBXSourcesBuildPhase");
      this.frameworks = new KnownSectionBase<PBXFrameworksBuildPhaseData>("PBXFrameworksBuildPhase");
      this.resources = new KnownSectionBase<PBXResourcesBuildPhaseData>("PBXResourcesBuildPhase");
      this.copyFiles = new KnownSectionBase<PBXCopyFilesBuildPhaseData>("PBXCopyFilesBuildPhase");
      this.shellScripts = new KnownSectionBase<PBXShellScriptBuildPhaseData>("PBXShellScriptBuildPhase");
      this.nativeTargets = new KnownSectionBase<PBXNativeTargetData>("PBXNativeTarget");
      this.targetDependencies = new KnownSectionBase<PBXTargetDependencyData>("PBXTargetDependency");
      this.variantGroups = new KnownSectionBase<PBXVariantGroupData>("PBXVariantGroup");
      this.buildConfigs = new KnownSectionBase<XCBuildConfigurationData>("XCBuildConfiguration");
      this.buildConfigLists = new KnownSectionBase<XCConfigurationListData>("XCConfigurationList");
      this.project = new PBXProjectSection();
      this.m_UnknownSections = new Dictionary<string, KnownSectionBase<PBXObjectData>>();
      this.m_Section = new Dictionary<string, SectionBase>()
      {
        {
          "PBXBuildFile",
          (SectionBase) this.buildFiles
        },
        {
          "PBXFileReference",
          (SectionBase) this.fileRefs
        },
        {
          "PBXGroup",
          (SectionBase) this.groups
        },
        {
          "PBXContainerItemProxy",
          (SectionBase) this.containerItems
        },
        {
          "PBXReferenceProxy",
          (SectionBase) this.references
        },
        {
          "PBXSourcesBuildPhase",
          (SectionBase) this.sources
        },
        {
          "PBXFrameworksBuildPhase",
          (SectionBase) this.frameworks
        },
        {
          "PBXResourcesBuildPhase",
          (SectionBase) this.resources
        },
        {
          "PBXCopyFilesBuildPhase",
          (SectionBase) this.copyFiles
        },
        {
          "PBXShellScriptBuildPhase",
          (SectionBase) this.shellScripts
        },
        {
          "PBXNativeTarget",
          (SectionBase) this.nativeTargets
        },
        {
          "PBXTargetDependency",
          (SectionBase) this.targetDependencies
        },
        {
          "PBXVariantGroup",
          (SectionBase) this.variantGroups
        },
        {
          "XCBuildConfiguration",
          (SectionBase) this.buildConfigs
        },
        {
          "XCConfigurationList",
          (SectionBase) this.buildConfigLists
        },
        {
          "PBXProject",
          (SectionBase) this.project
        }
      };
      this.m_RootElements = new PBXElementDict();
      this.m_UnknownObjects = new PBXElementDict();
      this.m_ObjectVersion = (string) null;
      this.m_SectionOrder = new List<string>()
      {
        "PBXBuildFile",
        "PBXContainerItemProxy",
        "PBXCopyFilesBuildPhase",
        "PBXFileReference",
        "PBXFrameworksBuildPhase",
        "PBXGroup",
        "PBXNativeTarget",
        "PBXProject",
        "PBXReferenceProxy",
        "PBXResourcesBuildPhase",
        "PBXShellScriptBuildPhase",
        "PBXSourcesBuildPhase",
        "PBXTargetDependency",
        "PBXVariantGroup",
        "XCBuildConfiguration",
        "XCConfigurationList"
      };
      this.m_FileGuidToBuildFileMap = new Dictionary<string, Dictionary<string, PBXBuildFileData>>();
      this.m_ProjectPathToFileRefMap = new Dictionary<string, PBXFileReferenceData>();
      this.m_FileRefGuidToProjectPathMap = new Dictionary<string, string>();
      this.m_RealPathToFileRefMap = new Dictionary<PBXSourceTree, Dictionary<string, PBXFileReferenceData>>();
      foreach (PBXSourceTree key in FileTypeUtils.AllAbsoluteSourceTrees())
        this.m_RealPathToFileRefMap.Add(key, new Dictionary<string, PBXFileReferenceData>());
      this.m_ProjectPathToGroupMap = new Dictionary<string, PBXGroupData>();
      this.m_GroupGuidToProjectPathMap = new Dictionary<string, string>();
      this.m_GuidToParentGroupMap = new Dictionary<string, PBXGroupData>();
    }

    private void BuildCommentMapForBuildFiles(GUIDToCommentMap comments, List<string> guids, string sectName)
    {
      foreach (string guid in guids)
      {
        PBXBuildFileData pbxBuildFileData = this.BuildFilesGet(guid);
        if (pbxBuildFileData != null)
        {
          PBXFileReferenceData fileReferenceData = this.FileRefsGet(pbxBuildFileData.fileRef);
          if (fileReferenceData != null)
          {
            comments.Add(guid, string.Format("{0} in {1}", (object) fileReferenceData.name, (object) sectName));
          }
          else
          {
            PBXReferenceProxyData referenceProxyData = this.references[pbxBuildFileData.fileRef];
            if (referenceProxyData != null)
              comments.Add(guid, string.Format("{0} in {1}", (object) referenceProxyData.path, (object) sectName));
          }
        }
      }
    }

    private GUIDToCommentMap BuildCommentMap()
    {
      GUIDToCommentMap comments = new GUIDToCommentMap();
      foreach (PBXGroupData pbxGroupData in this.groups.GetObjects())
        comments.Add(pbxGroupData.guid, pbxGroupData.name);
      foreach (PBXContainerItemProxyData containerItemProxyData in this.containerItems.GetObjects())
        comments.Add(containerItemProxyData.guid, "PBXContainerItemProxy");
      foreach (PBXReferenceProxyData referenceProxyData in this.references.GetObjects())
        comments.Add(referenceProxyData.guid, referenceProxyData.path);
      foreach (PBXSourcesBuildPhaseData sourcesBuildPhaseData in this.sources.GetObjects())
      {
        comments.Add(sourcesBuildPhaseData.guid, "Sources");
        this.BuildCommentMapForBuildFiles(comments, (List<string>) sourcesBuildPhaseData.files, "Sources");
      }
      foreach (PBXResourcesBuildPhaseData resourcesBuildPhaseData in this.resources.GetObjects())
      {
        comments.Add(resourcesBuildPhaseData.guid, "Resources");
        this.BuildCommentMapForBuildFiles(comments, (List<string>) resourcesBuildPhaseData.files, "Resources");
      }
      foreach (PBXFrameworksBuildPhaseData frameworksBuildPhaseData in this.frameworks.GetObjects())
      {
        comments.Add(frameworksBuildPhaseData.guid, "Frameworks");
        this.BuildCommentMapForBuildFiles(comments, (List<string>) frameworksBuildPhaseData.files, "Frameworks");
      }
      foreach (PBXCopyFilesBuildPhaseData filesBuildPhaseData in this.copyFiles.GetObjects())
      {
        string str = filesBuildPhaseData.name ?? "CopyFiles";
        comments.Add(filesBuildPhaseData.guid, str);
        this.BuildCommentMapForBuildFiles(comments, (List<string>) filesBuildPhaseData.files, str);
      }
      foreach (PBXShellScriptBuildPhaseData scriptBuildPhaseData in this.shellScripts.GetObjects())
        comments.Add(scriptBuildPhaseData.guid, "ShellScript");
      foreach (PBXTargetDependencyData targetDependencyData in this.targetDependencies.GetObjects())
        comments.Add(targetDependencyData.guid, "PBXTargetDependency");
      foreach (PBXNativeTargetData nativeTargetData in this.nativeTargets.GetObjects())
      {
        comments.Add(nativeTargetData.guid, nativeTargetData.name);
        comments.Add(nativeTargetData.buildConfigList, string.Format("Build configuration list for PBXNativeTarget \"{0}\"", (object) nativeTargetData.name));
      }
      foreach (PBXVariantGroupData variantGroupData in this.variantGroups.GetObjects())
        comments.Add(variantGroupData.guid, variantGroupData.name);
      foreach (XCBuildConfigurationData configurationData in this.buildConfigs.GetObjects())
        comments.Add(configurationData.guid, configurationData.name);
      foreach (PBXProjectObjectData projectObjectData in this.project.GetObjects())
      {
        comments.Add(projectObjectData.guid, "Project object");
        comments.Add(projectObjectData.buildConfigList, "Build configuration list for PBXProject \"Unity-iPhone\"");
      }
      foreach (PBXFileReferenceData fileReferenceData in this.fileRefs.GetObjects())
        comments.Add(fileReferenceData.guid, fileReferenceData.name);
      if (this.m_RootElements.Contains("rootObject") && this.m_RootElements["rootObject"] is PBXElementString)
        comments.Add(this.m_RootElements["rootObject"].AsString(), "Project object");
      return comments;
    }

    private static PBXElementDict ParseContent(string content)
    {
      TokenList tokens = Lexer.Tokenize(content);
      return Serializer.ParseTreeAST(new Parser(tokens).ParseTree(), tokens, content);
    }

        public void ReadFromStream(TextReader sr)
        {
            this.Clear();
            this.m_RootElements = PBXProjectData.ParseContent(sr.ReadToEnd());
            if (!this.m_RootElements.Contains("objects"))
                throw new Exception("Invalid PBX project file: no objects element");
            PBXElementDict pbxElementDict1 = this.m_RootElements["objects"].AsDict();
            this.m_RootElements.Remove("objects");
            this.m_RootElements.SetString("objects", "OBJMARKER");
            if (this.m_RootElements.Contains("objectVersion"))
            {
                this.m_ObjectVersion = this.m_RootElements["objectVersion"].AsString();
                this.m_RootElements.Remove("objectVersion");
            }
            List<string> allGuids = new List<string>();
            // ISSUE: reference to a compiler-generated field
            string prevSectionName = null;
            foreach (KeyValuePair<string, PBXElement> keyValuePair in (IEnumerable<KeyValuePair<string, PBXElement>>)pbxElementDict1.values)
            {
                allGuids.Add(keyValuePair.Key);
                PBXElement pbxElement = keyValuePair.Value;
                if (!(pbxElement is PBXElementDict) || !pbxElement.AsDict().Contains("isa"))
                {
                    this.m_UnknownObjects.values.Add(keyValuePair.Key, pbxElement);
                }
                else
                {
                    PBXElementDict pbxElementDict2 = pbxElement.AsDict();
                    string index1 = pbxElementDict2["isa"].AsString();
                    if (this.m_Section.ContainsKey(index1))
                    {
                        this.m_Section[index1].AddObject(keyValuePair.Key, pbxElementDict2);
                    }
                    else
                    {
                        KnownSectionBase<PBXObjectData> knownSectionBase;
                        if (this.m_UnknownSections.ContainsKey(index1))
                        {
                            knownSectionBase = this.m_UnknownSections[index1];
                        }
                        else
                        {
                            knownSectionBase = new KnownSectionBase<PBXObjectData>(index1);
                            this.m_UnknownSections.Add(index1, knownSectionBase);
                        }
                        knownSectionBase.AddObject(keyValuePair.Key, pbxElementDict2);
                        if (!this.m_SectionOrder.Contains(index1))
                        {
                            int pos = 0;
                            if (prevSectionName != null)
                            {
                                // this never fails, because we already added any previous unknown sections
                                // to m_SectionOrder
                                pos = m_SectionOrder.FindIndex(x => x == prevSectionName);
                                pos += 1;
                            }
                            m_SectionOrder.Insert(pos, index1);
                        }
                    }
                    // ISSUE: reference to a compiler-generated field
                    prevSectionName = index1;
                }
            }
            this.RepairStructure(allGuids);
            this.RefreshAuxMaps();
        }

    public string WriteToString()
    {
      GUIDToCommentMap comments1 = this.BuildCommentMap();
      PropertyCommentChecker checker1 = new PropertyCommentChecker();
      GUIDToCommentMap comments2 = new GUIDToCommentMap();
      StringBuilder sb1 = new StringBuilder();
      if (this.m_ObjectVersion != null)
        sb1.AppendFormat("objectVersion = {0};\n\t", (object) this.m_ObjectVersion);
      sb1.Append("objects = {");
      foreach (string key in this.m_SectionOrder)
      {
        if (this.m_Section.ContainsKey(key))
          this.m_Section[key].WriteSection(sb1, comments1);
        else if (this.m_UnknownSections.ContainsKey(key))
          this.m_UnknownSections[key].WriteSection(sb1, comments1);
      }
      foreach (KeyValuePair<string, PBXElement> keyValuePair in (IEnumerable<KeyValuePair<string, PBXElement>>) this.m_UnknownObjects.values)
        Serializer.WriteDictKeyValue(sb1, keyValuePair.Key, keyValuePair.Value, 2, false, checker1, comments2);
      sb1.Append("\n\t};");
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("// !$*UTF8*$!\n");
      StringBuilder sb2 = stringBuilder;
      PBXElementDict el = this.m_RootElements;
      int indent = 0;
      int num = 0;
      string[] strArray = new string[1];
      int index = 0;
      string str = "rootObject/*";
      strArray[index] = str;
      PropertyCommentChecker checker2 = new PropertyCommentChecker((IEnumerable<string>) strArray);
      GUIDToCommentMap comments3 = comments1;
      Serializer.WriteDict(sb2, el, indent, num != 0, checker2, comments3);
      stringBuilder.Append("\n");
      return stringBuilder.ToString().Replace("objects = OBJMARKER;", sb1.ToString());
    }

    private void RepairStructure(List<string> allGuids)
    {
      Dictionary<string, bool> allGuids1 = new Dictionary<string, bool>();
      foreach (string key in allGuids)
        allGuids1.Add(key, false);
      do
        ;
      while (this.RepairStructureImpl(allGuids1));
    }

    private static void RemoveMissingGuidsFromGuidList(GUIDList guidList, Dictionary<string, bool> allGuids)
    {
      List<string> list = (List<string>) null;
      foreach (string key in (IEnumerable<string>) guidList)
      {
        if (!allGuids.ContainsKey(key))
        {
          if (list == null)
            list = new List<string>();
          list.Add(key);
        }
      }
      if (list == null)
        return;
      foreach (string guid in list)
        guidList.RemoveGUID(guid);
    }

    private static bool RemoveObjectsFromSection<T>(KnownSectionBase<T> section, Dictionary<string, bool> allGuids, Func<T, bool> checker) where T : PBXObjectData, new()
    {
      List<string> list = (List<string>) null;
      foreach (KeyValuePair<string, T> keyValuePair in section.GetEntries())
      {
        if (checker(keyValuePair.Value))
        {
          if (list == null)
            list = new List<string>();
          list.Add(keyValuePair.Key);
        }
      }
      if (list == null)
        return false;
      foreach (string str in list)
      {
        section.RemoveEntry(str);
        allGuids.Remove(str);
      }
      return true;
    }

    private bool RepairStructureImpl(Dictionary<string, bool> allGuids)
    {
            bool changed = false;

            // PBXBuildFile
            changed |= RemoveObjectsFromSection(buildFiles, allGuids,
                                                o => (o.fileRef == null || !allGuids.ContainsKey(o.fileRef)));
            // PBXFileReference / fileRefs not cleaned

            // PBXGroup
            changed |= RemoveObjectsFromSection(groups, allGuids, o => o.children == null);
            foreach (var o in groups.GetObjects())
                RemoveMissingGuidsFromGuidList(o.children, allGuids);

            // PBXContainerItem / containerItems not cleaned
            // PBXReferenceProxy / references not cleaned

            // PBXSourcesBuildPhase
            changed |= RemoveObjectsFromSection(sources, allGuids, o => o.files == null);
            foreach (var o in sources.GetObjects())
                RemoveMissingGuidsFromGuidList(o.files, allGuids);
            // PBXFrameworksBuildPhase
            changed |= RemoveObjectsFromSection(frameworks, allGuids, o => o.files == null);
            foreach (var o in frameworks.GetObjects())
                RemoveMissingGuidsFromGuidList(o.files, allGuids);
            // PBXResourcesBuildPhase
            changed |= RemoveObjectsFromSection(resources, allGuids, o => o.files == null);
            foreach (var o in resources.GetObjects())
                RemoveMissingGuidsFromGuidList(o.files, allGuids);
            // PBXCopyFilesBuildPhase
            changed |= RemoveObjectsFromSection(copyFiles, allGuids, o => o.files == null);
            foreach (var o in copyFiles.GetObjects())
                RemoveMissingGuidsFromGuidList(o.files, allGuids);
            // PBXShellScriptsBuildPhase
            changed |= RemoveObjectsFromSection(shellScripts, allGuids, o => o.files == null);
            foreach (var o in shellScripts.GetObjects())
                RemoveMissingGuidsFromGuidList(o.files, allGuids);

            // PBXNativeTarget
            changed |= RemoveObjectsFromSection(nativeTargets, allGuids, o => o.phases == null);
            foreach (var o in nativeTargets.GetObjects())
                RemoveMissingGuidsFromGuidList(o.phases, allGuids);

            // PBXTargetDependency / targetDependencies not cleaned

            // PBXVariantGroup
            changed |= RemoveObjectsFromSection(variantGroups, allGuids, o => o.children == null);
            foreach (var o in variantGroups.GetObjects())
                RemoveMissingGuidsFromGuidList(o.children, allGuids);

            // XCBuildConfiguration / buildConfigs not cleaned

            // XCConfigurationList
            changed |= RemoveObjectsFromSection(buildConfigLists, allGuids, o => o.buildConfigs == null);
            foreach (var o in buildConfigLists.GetObjects())
                RemoveMissingGuidsFromGuidList(o.buildConfigs, allGuids);

            // PBXProject project not cleaned
            return changed;
        }
  }
}
