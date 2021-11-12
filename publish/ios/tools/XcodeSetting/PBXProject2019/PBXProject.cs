// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBXProject
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEditor.iOS.Xcode.PBX;

namespace UnityEditor.iOS.Xcode
{
  public class PBXProject
  {
    private PBXProjectData m_Data = new PBXProjectData();

    internal KnownSectionBase<PBXContainerItemProxyData> containerItems
    {
      get
      {
        return this.m_Data.containerItems;
      }
    }

    internal KnownSectionBase<PBXReferenceProxyData> references
    {
      get
      {
        return this.m_Data.references;
      }
    }

    internal KnownSectionBase<PBXSourcesBuildPhaseData> sources
    {
      get
      {
        return this.m_Data.sources;
      }
    }

    internal KnownSectionBase<PBXHeadersBuildPhaseData> headers
    {
      get
      {
        return this.m_Data.headers;
      }
    }

    internal KnownSectionBase<PBXFrameworksBuildPhaseData> frameworks
    {
      get
      {
        return this.m_Data.frameworks;
      }
    }

    internal KnownSectionBase<PBXResourcesBuildPhaseData> resources
    {
      get
      {
        return this.m_Data.resources;
      }
    }

    internal KnownSectionBase<PBXCopyFilesBuildPhaseData> copyFiles
    {
      get
      {
        return this.m_Data.copyFiles;
      }
    }

    internal KnownSectionBase<PBXShellScriptBuildPhaseData> shellScripts
    {
      get
      {
        return this.m_Data.shellScripts;
      }
    }

    internal KnownSectionBase<PBXNativeTargetData> nativeTargets
    {
      get
      {
        return this.m_Data.nativeTargets;
      }
    }

    internal KnownSectionBase<PBXTargetDependencyData> targetDependencies
    {
      get
      {
        return this.m_Data.targetDependencies;
      }
    }

    internal KnownSectionBase<PBXVariantGroupData> variantGroups
    {
      get
      {
        return this.m_Data.variantGroups;
      }
    }

    internal KnownSectionBase<XCBuildConfigurationData> buildConfigs
    {
      get
      {
        return this.m_Data.buildConfigs;
      }
    }

    internal KnownSectionBase<XCConfigurationListData> buildConfigLists
    {
      get
      {
        return this.m_Data.buildConfigLists;
      }
    }

    internal PBXProjectSection project
    {
      get
      {
        return this.m_Data.project;
      }
    }

    internal PBXBuildFileData BuildFilesGet(string guid)
    {
      return this.m_Data.BuildFilesGet(guid);
    }

    internal void BuildFilesAdd(string targetGuid, PBXBuildFileData buildFile)
    {
      this.m_Data.BuildFilesAdd(targetGuid, buildFile);
    }

    internal void BuildFilesRemove(string targetGuid, string fileGuid)
    {
      this.m_Data.BuildFilesRemove(targetGuid, fileGuid);
    }

    internal PBXBuildFileData BuildFilesGetForSourceFile(string targetGuid, string fileGuid)
    {
      return this.m_Data.BuildFilesGetForSourceFile(targetGuid, fileGuid);
    }

    internal IEnumerable<PBXBuildFileData> BuildFilesGetAll()
    {
      return this.m_Data.BuildFilesGetAll();
    }

    internal void FileRefsAdd(string realPath, string projectPath, PBXGroupData parent, PBXFileReferenceData fileRef)
    {
      this.m_Data.FileRefsAdd(realPath, projectPath, parent, fileRef);
    }

    internal PBXFileReferenceData FileRefsGet(string guid)
    {
      return this.m_Data.FileRefsGet(guid);
    }

    internal PBXFileReferenceData FileRefsGetByRealPath(string path, PBXSourceTree sourceTree)
    {
      return this.m_Data.FileRefsGetByRealPath(path, sourceTree);
    }

    internal PBXFileReferenceData FileRefsGetByProjectPath(string path)
    {
      return this.m_Data.FileRefsGetByProjectPath(path);
    }

    internal void FileRefsRemove(string guid)
    {
      this.m_Data.FileRefsRemove(guid);
    }

    internal PBXGroupData GroupsGet(string guid)
    {
      return this.m_Data.GroupsGet(guid);
    }

    internal PBXGroupData GroupsGetByChild(string childGuid)
    {
      return this.m_Data.GroupsGetByChild(childGuid);
    }

    internal PBXGroupData GroupsGetMainGroup()
    {
      return this.m_Data.GroupsGetMainGroup();
    }

    internal PBXGroupData GroupsGetByProjectPath(string sourceGroup)
    {
      return this.m_Data.GroupsGetByProjectPath(sourceGroup);
    }

    internal void GroupsAdd(string projectPath, PBXGroupData parent, PBXGroupData gr)
    {
      this.m_Data.GroupsAdd(projectPath, parent, gr);
    }

    internal void GroupsAddDuplicate(PBXGroupData gr)
    {
      this.m_Data.GroupsAddDuplicate(gr);
    }

    internal void GroupsRemove(string guid)
    {
      this.m_Data.GroupsRemove(guid);
    }

    internal FileGUIDListBase BuildSectionAny(PBXNativeTargetData target, string path, bool isFolderRef)
    {
      return this.m_Data.BuildSectionAny(target, path, isFolderRef);
    }

    internal FileGUIDListBase BuildSectionAny(string sectionGuid)
    {
      return this.m_Data.BuildSectionAny(sectionGuid);
    }

    public static string GetPBXProjectPath(string buildPath)
    {
      return PBXPath.Combine(buildPath, "Unity-iPhone.xcodeproj/project.pbxproj");
    }

    [Obsolete("This function is deprecated. There are two targets now, call GetUnityMainTargetGuid() - for app or GetUnityFrameworkTargetGuid() - for source/plugins to get Guid instead of calling to TargetGuidByName(GetUnityTargetName()).", true)]
    public static string GetUnityTargetName()
    {
      return "Unity-iPhone";
    }

    public string GetUnityMainTargetGuid()
    {
      return this.FindTargetGuidByName("Unity-iPhone");
    }

    public string GetUnityFrameworkTargetGuid()
    {
      return this.FindTargetGuidByName("UnityFramework");
    }

    private string FindTargetGuidByName(string name)
    {
      foreach (KeyValuePair<string, PBXNativeTargetData> keyValuePair in this.nativeTargets.GetEntries())
      {
        if (keyValuePair.Value.name == name)
          return keyValuePair.Key;
      }
      return (string) null;
    }

    public static string GetUnityTestTargetName()
    {
      return "Unity-iPhone Tests";
    }

    public string ProjectGuid()
    {
      return this.project.project.guid;
    }

    public string TargetGuidByName(string name)
    {
      if (name == "Unity-iPhone")
        throw new Exception("Calling TargetGuidByName with name='Unity-iPhone' is deprecated. There are two targets now, call GetUnityMainTargetGuid() - for app or GetUnityFrameworkTargetGuid() - for source/plugins to get Guid instead.");
      return this.FindTargetGuidByName(name);
    }

    public static bool IsKnownExtension(string ext)
    {
      return FileTypeUtils.IsKnownExtension(ext);
    }

    public static bool IsBuildable(string ext)
    {
      return FileTypeUtils.IsBuildableFile(ext);
    }

    private string AddFileImpl(string path, string projectPath, PBXSourceTree tree, bool isFolderReference)
    {
      path = PBXPath.FixSlashes(path);
      projectPath = PBXPath.FixSlashes(projectPath);
      if (!isFolderReference && Path.GetExtension(path) != Path.GetExtension(projectPath))
        throw new Exception("Project and real path extensions do not match");
      string str = this.FindFileGuidByProjectPath(projectPath) ?? this.FindFileGuidByRealPath(path);
      if (str == null)
      {
        PBXFileReferenceData fileRef = !isFolderReference ? PBXFileReferenceData.CreateFromFile(path, PBXPath.GetFilename(projectPath), tree) : PBXFileReferenceData.CreateFromFolderReference(path, PBXPath.GetFilename(projectPath), tree);
        PBXGroupData sourceGroup = this.CreateSourceGroup(PBXPath.GetDirectory(projectPath));
        sourceGroup.children.AddGUID(fileRef.guid);
        this.FileRefsAdd(path, projectPath, sourceGroup, fileRef);
        str = fileRef.guid;
      }
      return str;
    }

    public string AddFile(string path, string projectPath, PBXSourceTree sourceTree = PBXSourceTree.Source)
    {
      if (sourceTree == PBXSourceTree.Group)
        throw new Exception("sourceTree must not be PBXSourceTree.Group");
      return this.AddFileImpl(path, projectPath, sourceTree, false);
    }

    public string AddFolderReference(string path, string projectPath, PBXSourceTree sourceTree = PBXSourceTree.Source)
    {
      if (sourceTree == PBXSourceTree.Group)
        throw new Exception("sourceTree must not be PBXSourceTree.Group");
      return this.AddFileImpl(path, projectPath, sourceTree, true);
    }

    private void AddBuildFileImpl(string targetGuid, string fileGuid, bool weak, string compileFlags)
    {
      PBXNativeTargetData target = this.nativeTargets[targetGuid];
      PBXFileReferenceData fileReferenceData = this.FileRefsGet(fileGuid);
      if (!FileTypeUtils.IsBuildable(Path.GetExtension(fileReferenceData.path), fileReferenceData.isFolderReference) || this.BuildFilesGetForSourceFile(targetGuid, fileGuid) != null)
        return;
      PBXBuildFileData fromFile = PBXBuildFileData.CreateFromFile(fileGuid, weak, compileFlags);
      this.BuildFilesAdd(targetGuid, fromFile);
      this.BuildSectionAny(target, fileReferenceData.path, fileReferenceData.isFolderReference).files.AddGUID(fromFile.guid);
    }

    public void AddFileToBuild(string targetGuid, string fileGuid)
    {
      this.AddBuildFileImpl(targetGuid, fileGuid, false, (string) null);
    }

    public void AddFileToBuildWithFlags(string targetGuid, string fileGuid, string compileFlags)
    {
      this.AddBuildFileImpl(targetGuid, fileGuid, false, compileFlags);
    }

    public void AddFileToBuildSection(string targetGuid, string sectionGuid, string fileGuid)
    {
      PBXBuildFileData fromFile = PBXBuildFileData.CreateFromFile(fileGuid, false, (string) null);
      this.BuildFilesAdd(targetGuid, fromFile);
      this.BuildSectionAny(sectionGuid).files.AddGUID(fromFile.guid);
    }

    public void AddPublicHeaderToBuild(string targetGuid, string fileGuid)
    {
      PBXBuildFileData forSourceFile = this.m_Data.BuildFilesGetForSourceFile(targetGuid, fileGuid);
      if (forSourceFile == null)
      {
        this.AddFileToBuild(targetGuid, fileGuid);
        forSourceFile = this.m_Data.BuildFilesGetForSourceFile(targetGuid, fileGuid);
      }
      forSourceFile.publicHeader = true;
    }

    public List<string> GetCompileFlagsForFile(string targetGuid, string fileGuid)
    {
      PBXBuildFileData forSourceFile = this.BuildFilesGetForSourceFile(targetGuid, fileGuid);
      if (forSourceFile == null)
        return (List<string>) null;
      if (forSourceFile.compileFlags == null)
        return new List<string>();
      string str = forSourceFile.compileFlags;
      char[] separator = new char[1];
      int index = 0;
      int num1 = 32;
      separator[index] = (char) num1;
      int num2 = 1;
      return new List<string>((IEnumerable<string>) str.Split(separator, (StringSplitOptions) num2));
    }

    public void SetCompileFlagsForFile(string targetGuid, string fileGuid, List<string> compileFlags)
    {
      PBXBuildFileData forSourceFile = this.BuildFilesGetForSourceFile(targetGuid, fileGuid);
      if (forSourceFile == null)
        return;
      if (compileFlags == null)
        forSourceFile.compileFlags = (string) null;
      else
        forSourceFile.compileFlags = string.Join(" ", compileFlags.ToArray());
    }

    public void AddAssetTagForFile(string targetGuid, string fileGuid, string tag)
    {
      PBXBuildFileData forSourceFile = this.BuildFilesGetForSourceFile(targetGuid, fileGuid);
      if (forSourceFile == null)
        return;
      if (!forSourceFile.assetTags.Contains(tag))
        forSourceFile.assetTags.Add(tag);
      if (this.project.project.knownAssetTags.Contains(tag))
        return;
      this.project.project.knownAssetTags.Add(tag);
    }

    public void RemoveAssetTagForFile(string targetGuid, string fileGuid, string tag)
    {
      PBXBuildFileData forSourceFile = this.BuildFilesGetForSourceFile(targetGuid, fileGuid);
      if (forSourceFile == null)
        return;
      forSourceFile.assetTags.Remove(tag);
      foreach (PBXBuildFileData pbxBuildFileData in this.BuildFilesGetAll())
      {
        if (pbxBuildFileData.assetTags.Contains(tag))
          return;
      }
      this.project.project.knownAssetTags.Remove(tag);
    }

    public void AddAssetTagToDefaultInstall(string targetGuid, string tag)
    {
      if (!this.project.project.knownAssetTags.Contains(tag))
        return;
      this.AddBuildProperty(targetGuid, "ON_DEMAND_RESOURCES_INITIAL_INSTALL_TAGS", tag);
    }

    public void RemoveAssetTagFromDefaultInstall(string targetGuid, string tag)
    {
      string targetGuid1 = targetGuid;
      string name = "ON_DEMAND_RESOURCES_INITIAL_INSTALL_TAGS";

      string[] strArray = new string[1];
      int index = 0;
      string str = tag;
      strArray[index] = str;
      this.UpdateBuildProperty(targetGuid1, name, null, (IEnumerable<string>) strArray);
    }

    public void RemoveAssetTag(string tag)
    {
      foreach (PBXBuildFileData pbxBuildFileData in this.BuildFilesGetAll())
        pbxBuildFileData.assetTags.Remove(tag);
      foreach (string targetGuid in this.nativeTargets.GetGuids())
        this.RemoveAssetTagFromDefaultInstall(targetGuid, tag);
      this.project.project.knownAssetTags.Remove(tag);
    }

    public bool ContainsFileByRealPath(string path)
    {
      return this.FindFileGuidByRealPath(path) != null;
    }

    public bool ContainsFileByRealPath(string path, PBXSourceTree sourceTree)
    {
      if (sourceTree == PBXSourceTree.Group)
        throw new Exception("sourceTree must not be PBXSourceTree.Group");
      return this.FindFileGuidByRealPath(path, sourceTree) != null;
    }

    public bool ContainsFileByProjectPath(string path)
    {
      return this.FindFileGuidByProjectPath(path) != null;
    }

    public bool ContainsFramework(string targetGuid, string framework)
    {
      string fileGuidByRealPath = this.FindFileGuidByRealPath("System/Library/Frameworks/" + framework, PBXSourceTree.Sdk);
      if (fileGuidByRealPath == null)
        return false;
      return this.BuildFilesGetForSourceFile(targetGuid, fileGuidByRealPath) != null;
    }

    public void AddFrameworkToProject(string targetGuid, string framework, bool weak)
    {
      string fileGuid = this.AddFile("System/Library/Frameworks/" + framework, "Frameworks/" + framework, PBXSourceTree.Sdk);
      this.AddBuildFileImpl(targetGuid, fileGuid, weak, (string) null);
    }

    public void RemoveFrameworkFromProject(string targetGuid, string framework)
    {
      string fileGuidByRealPath = this.FindFileGuidByRealPath("System/Library/Frameworks/" + framework, PBXSourceTree.Sdk);
      if (fileGuidByRealPath == null)
        return;
      PBXBuildFileData forSourceFile = this.BuildFilesGetForSourceFile(targetGuid, fileGuidByRealPath);
      if (forSourceFile == null)
        return;
      foreach (FileGUIDListBase fileGuidListBase in this.m_Data.frameworks.GetObjects())
        fileGuidListBase.files.RemoveGUID(forSourceFile.guid);
      this.BuildFilesRemove(targetGuid, fileGuidByRealPath);
    }

    public bool AddCapability(string targetGuid, PBXCapabilityType capability, string entitlementsFilePath = null, bool addOptionalFramework = false)
    {
      if (capability.requiresEntitlements && entitlementsFilePath == "")
        throw new Exception("Couldn't add the Xcode Capability " + capability.id + " to the PBXProject file because this capability requires an entitlement file.");
      PBXProjectObjectData project = this.project.project;
      if (project.entitlementsFile != null && entitlementsFilePath != null && project.entitlementsFile != entitlementsFilePath)
      {
        if (project.capabilities.Count > 0)
          throw new WarningException("Attention, it seems that you have multiple entitlements file. Only one will be added the Project : " + project.entitlementsFile);
        return false;
      }
      if (project.capabilities.Contains(new PBXCapabilityType.TargetCapabilityPair(targetGuid, capability)))
        throw new WarningException("This capability has already been added. Method ignored");
      project.capabilities.Add(new PBXCapabilityType.TargetCapabilityPair(targetGuid, capability));
      if (capability.framework != "" && !capability.optionalFramework || ((!(capability.framework != "") ? 0 : (capability.optionalFramework ? 1 : 0)) & (addOptionalFramework ? 1 : 0)) != 0)
        this.AddFrameworkToProject(targetGuid, capability.framework, false);
      if (entitlementsFilePath != null && project.entitlementsFile == null)
      {
        project.entitlementsFile = entitlementsFilePath;
        this.AddFileImpl(entitlementsFilePath, entitlementsFilePath, PBXSourceTree.Source, false);
        this.SetBuildProperty(targetGuid, "CODE_SIGN_ENTITLEMENTS", PBXPath.FixSlashes(entitlementsFilePath));
      }
      return true;
    }

    public void SetTeamId(string targetGuid, string teamId)
    {
      this.SetBuildProperty(targetGuid, "DEVELOPMENT_TEAM", teamId);
      this.project.project.teamIDs.Add(targetGuid, teamId);
    }

    public string FindFileGuidByRealPath(string path, PBXSourceTree sourceTree)
    {
      if (sourceTree == PBXSourceTree.Group)
        throw new Exception("sourceTree must not be PBXSourceTree.Group");
      path = PBXPath.FixSlashes(path);
      PBXFileReferenceData byRealPath = this.FileRefsGetByRealPath(path, sourceTree);
      if (byRealPath != null)
        return byRealPath.guid;
      return (string) null;
    }

    public string FindFileGuidByRealPath(string path)
    {
      path = PBXPath.FixSlashes(path);
      foreach (PBXSourceTree sourceTree in FileTypeUtils.AllAbsoluteSourceTrees())
      {
        string fileGuidByRealPath = this.FindFileGuidByRealPath(path, sourceTree);
        if (fileGuidByRealPath != null)
          return fileGuidByRealPath;
      }
      return (string) null;
    }

    public string FindFileGuidByProjectPath(string path)
    {
      path = PBXPath.FixSlashes(path);
      PBXFileReferenceData byProjectPath = this.FileRefsGetByProjectPath(path);
      if (byProjectPath != null)
        return byProjectPath.guid;
      return (string) null;
    }

    public void RemoveFileFromBuild(string targetGuid, string fileGuid)
    {
      PBXBuildFileData forSourceFile = this.BuildFilesGetForSourceFile(targetGuid, fileGuid);
      if (forSourceFile == null)
        return;
      this.BuildFilesRemove(targetGuid, fileGuid);
      string guid = forSourceFile.guid;
      if (guid == null)
        return;
      foreach (KeyValuePair<string, PBXSourcesBuildPhaseData> keyValuePair in this.sources.GetEntries())
        keyValuePair.Value.files.RemoveGUID(guid);
      foreach (KeyValuePair<string, PBXResourcesBuildPhaseData> keyValuePair in this.resources.GetEntries())
        keyValuePair.Value.files.RemoveGUID(guid);
      foreach (KeyValuePair<string, PBXCopyFilesBuildPhaseData> keyValuePair in this.copyFiles.GetEntries())
        keyValuePair.Value.files.RemoveGUID(guid);
      foreach (KeyValuePair<string, PBXFrameworksBuildPhaseData> keyValuePair in this.frameworks.GetEntries())
        keyValuePair.Value.files.RemoveGUID(guid);
    }

    public void RemoveFile(string fileGuid)
    {
      if (fileGuid == null)
        return;
      PBXGroupData byChild = this.GroupsGetByChild(fileGuid);
      if (byChild != null)
        byChild.children.RemoveGUID(fileGuid);
      this.RemoveGroupIfEmpty(byChild);
      foreach (KeyValuePair<string, PBXNativeTargetData> keyValuePair in this.nativeTargets.GetEntries())
        this.RemoveFileFromBuild(keyValuePair.Value.guid, fileGuid);
      this.FileRefsRemove(fileGuid);
    }

    private void RemoveGroupIfEmpty(PBXGroupData gr)
    {
      if (gr.children.Count != 0 || gr == this.GroupsGetMainGroup())
        return;
      PBXGroupData byChild = this.GroupsGetByChild(gr.guid);
      byChild.children.RemoveGUID(gr.guid);
      this.RemoveGroupIfEmpty(byChild);
      this.GroupsRemove(gr.guid);
    }

    private void RemoveGroupChildrenRecursive(PBXGroupData parent)
    {
      List<string> list = new List<string>((IEnumerable<string>) parent.children);
      parent.children.Clear();
      foreach (string str in list)
      {
        if (this.FileRefsGet(str) != null)
        {
          foreach (KeyValuePair<string, PBXNativeTargetData> keyValuePair in this.nativeTargets.GetEntries())
            this.RemoveFileFromBuild(keyValuePair.Value.guid, str);
          this.FileRefsRemove(str);
        }
        else
        {
          PBXGroupData parent1 = this.GroupsGet(str);
          if (parent1 != null)
          {
            this.RemoveGroupChildrenRecursive(parent1);
            this.GroupsRemove(parent1.guid);
          }
        }
      }
    }

    internal void RemoveFilesByProjectPathRecursive(string projectPath)
    {
      projectPath = PBXPath.FixSlashes(projectPath);
      PBXGroupData byProjectPath = this.GroupsGetByProjectPath(projectPath);
      if (byProjectPath == null)
        return;
      this.RemoveGroupChildrenRecursive(byProjectPath);
      this.RemoveGroupIfEmpty(byProjectPath);
    }

    public void ReplaceFile(string fileGuid, string path, PBXSourceTree source = PBXSourceTree.Absolute)
    {
      if (fileGuid == null)
        return;
      path = PBXPath.FixSlashes(path);
      PBXFileReferenceData fileReferenceData = this.FileRefsGet(fileGuid);
      if (fileReferenceData == null)
        return;
      fileReferenceData.path = path;
      fileReferenceData.tree = source;
    }

    internal List<string> GetGroupChildrenFiles(string projectPath)
    {
      projectPath = PBXPath.FixSlashes(projectPath);
      PBXGroupData byProjectPath = this.GroupsGetByProjectPath(projectPath);
      if (byProjectPath == null)
        return (List<string>) null;
      List<string> list = new List<string>();
      foreach (string guid in (IEnumerable<string>) byProjectPath.children)
      {
        PBXFileReferenceData fileReferenceData = this.FileRefsGet(guid);
        if (fileReferenceData != null)
          list.Add(fileReferenceData.name);
      }
      return list;
    }

    internal HashSet<string> GetGroupChildrenFilesRefs(string projectPath)
    {
      projectPath = PBXPath.FixSlashes(projectPath);
      PBXGroupData byProjectPath = this.GroupsGetByProjectPath(projectPath);
      if (byProjectPath == null)
        return new HashSet<string>();
      HashSet<string> hashSet = new HashSet<string>();
      foreach (string guid in (IEnumerable<string>) byProjectPath.children)
      {
        PBXFileReferenceData fileReferenceData = this.FileRefsGet(guid);
        if (fileReferenceData != null)
          hashSet.Add(fileReferenceData.path);
      }
      return hashSet == null ? new HashSet<string>() : hashSet;
    }

    internal HashSet<string> GetFileRefsByProjectPaths(IEnumerable<string> paths)
    {
      HashSet<string> hashSet = new HashSet<string>();
      foreach (string path in paths)
      {
        PBXFileReferenceData byProjectPath = this.FileRefsGetByProjectPath(PBXPath.FixSlashes(path));
        if (byProjectPath != null)
          hashSet.Add(byProjectPath.path);
      }
      return hashSet;
    }

    private PBXGroupData GetPBXGroupChildByName(PBXGroupData group, string name)
    {
      foreach (string guid in (IEnumerable<string>) group.children)
      {
        PBXGroupData pbxGroupData = this.GroupsGet(guid);
        if (pbxGroupData != null && pbxGroupData.name == name)
          return pbxGroupData;
      }
      return (PBXGroupData) null;
    }

    internal PBXGroupData CreateSourceGroup(string sourceGroup)
    {
      sourceGroup = PBXPath.FixSlashes(sourceGroup);
      if (sourceGroup == null || sourceGroup == "")
        return this.GroupsGetMainGroup();
      PBXGroupData byProjectPath = this.GroupsGetByProjectPath(sourceGroup);
      if (byProjectPath != null)
        return byProjectPath;
      PBXGroupData pbxGroupData = this.GroupsGetMainGroup();
      string[] strArray = PBXPath.Split(sourceGroup);
      string projectPath = (string) null;
      foreach (string str in strArray)
      {
        projectPath = projectPath != null ? projectPath + "/" + str : str;
        PBXGroupData groupChildByName = this.GetPBXGroupChildByName(pbxGroupData, str);
        if (groupChildByName != null)
        {
          pbxGroupData = groupChildByName;
        }
        else
        {
          PBXGroupData gr = PBXGroupData.Create(str, str, PBXSourceTree.Group);
          pbxGroupData.children.AddGUID(gr.guid);
          this.GroupsAdd(projectPath, pbxGroupData, gr);
          pbxGroupData = gr;
        }
      }
      return pbxGroupData;
    }

    public string AddTarget(string name, string ext, string type)
    {
      XCConfigurationListData configurationListData = XCConfigurationListData.Create();
      this.buildConfigLists.AddEntry(configurationListData);
      string path = name + "." + FileTypeUtils.TrimExtension(ext);
      string productRef = this.AddFile(path, "Products/" + path, PBXSourceTree.Build);
      PBXNativeTargetData nativeTargetData = PBXNativeTargetData.Create(name, productRef, type, configurationListData.guid);
      this.nativeTargets.AddEntry(nativeTargetData);
      this.project.project.targets.Add(nativeTargetData.guid);
      foreach (string name1 in this.BuildConfigNames())
        this.AddBuildConfigForTarget(nativeTargetData.guid, name1);
      return nativeTargetData.guid;
    }

    internal PBXBuildFileData FindFrameworkByFileGuid(PBXCopyFilesBuildPhaseData phase, string fileGuid)
    {
      foreach (string guid in (IEnumerable<string>) phase.files)
      {
        PBXBuildFileData pbxBuildFileData = this.BuildFilesGet(guid);
        if (pbxBuildFileData.fileRef == fileGuid)
          return pbxBuildFileData;
      }
      return (PBXBuildFileData) null;
    }

    private IEnumerable<string> GetAllTargetGuids()
    {
      List<string> list = new List<string>();
      list.Add(this.project.project.guid);
      list.AddRange(this.nativeTargets.GetGuids());
      return (IEnumerable<string>) list;
    }

    public string GetTargetProductFileRef(string targetGuid)
    {
      return this.nativeTargets[targetGuid].productReference;
    }

    public void AddTargetDependency(string targetGuid, string targetDependencyGuid)
    {
      string remoteInfo = this.nativeTargets[targetDependencyGuid].name;
      PBXContainerItemProxyData containerItemProxyData = PBXContainerItemProxyData.Create(this.project.project.guid, "1", targetDependencyGuid, remoteInfo);
      this.containerItems.AddEntry(containerItemProxyData);
      PBXTargetDependencyData targetDependencyData = PBXTargetDependencyData.Create(targetDependencyGuid, containerItemProxyData.guid);
      this.targetDependencies.AddEntry(targetDependencyData);
      this.nativeTargets[targetGuid].dependencies.AddGUID(targetDependencyData.guid);
    }

    private string AddBuildConfigForTarget(string targetGuid, string name)
    {
      if (this.BuildConfigByName(targetGuid, name) != null)
        throw new Exception(string.Format("A build configuration by name {0} already exists for target {1}", (object) targetGuid, (object) name));
      XCBuildConfigurationData configurationData = XCBuildConfigurationData.Create(name);
      this.buildConfigs.AddEntry(configurationData);
      this.buildConfigLists[this.GetConfigListForTarget(targetGuid)].buildConfigs.AddGUID(configurationData.guid);
      return configurationData.guid;
    }

    private void RemoveBuildConfigForTarget(string targetGuid, string name)
    {
      string guid = this.BuildConfigByName(targetGuid, name);
      if (guid == null)
        return;
      this.buildConfigs.RemoveEntry(guid);
      this.buildConfigLists[this.GetConfigListForTarget(targetGuid)].buildConfigs.RemoveGUID(guid);
    }

    public string BuildConfigByName(string targetGuid, string name)
    {
      foreach (string index in (IEnumerable<string>) this.buildConfigLists[this.GetConfigListForTarget(targetGuid)].buildConfigs)
      {
        XCBuildConfigurationData configurationData = this.buildConfigs[index];
        if (configurationData != null && configurationData.name == name)
          return configurationData.guid;
      }
      return (string) null;
    }

    public IEnumerable<string> BuildConfigNames()
    {
      List<string> list = new List<string>();
      foreach (string index in (IEnumerable<string>) this.buildConfigLists[this.project.project.buildConfigList].buildConfigs)
        list.Add(this.buildConfigs[index].name);
      return (IEnumerable<string>) list;
    }

    public void AddBuildConfig(string name)
    {
      foreach (string targetGuid in this.GetAllTargetGuids())
        this.AddBuildConfigForTarget(targetGuid, name);
    }

    public void RemoveBuildConfig(string name)
    {
      foreach (string targetGuid in this.GetAllTargetGuids())
        this.RemoveBuildConfigForTarget(targetGuid, name);
    }

    public string GetBuildPhaseName(string phaseGuid)
    {
      foreach (PBXNativeTargetData nativeTargetData in this.nativeTargets.GetObjects())
      {
        if (nativeTargetData.phases.Contains(phaseGuid))
        {
          FileGUIDListBase fileGuidListBase = this.BuildSectionAny(phaseGuid);
          if (fileGuidListBase is PBXCopyFilesBuildPhaseData)
            return ((PBXCopyFilesBuildPhaseData) fileGuidListBase).name;
          if (fileGuidListBase is PBXShellScriptBuildPhaseData)
            return ((PBXShellScriptBuildPhaseData) fileGuidListBase).name;
          return fileGuidListBase.GetPropertyString("isa");
        }
      }
      return (string) null;
    }

    public string GetBuildPhaseType(string phaseGuid)
    {
      foreach (PBXNativeTargetData nativeTargetData in this.nativeTargets.GetObjects())
      {
        if (nativeTargetData.phases.Contains(phaseGuid))
          return this.BuildSectionAny(phaseGuid).GetPropertyString("isa");
      }
      return (string) null;
    }

    public string GetSourcesBuildPhaseByTarget(string targetGuid)
    {
      foreach (string sectionGuid in (IEnumerable<string>) this.nativeTargets[targetGuid].phases)
      {
        if (this.BuildSectionAny(sectionGuid) is PBXSourcesBuildPhaseData)
          return sectionGuid;
      }
      return (string) null;
    }

    public string AddSourcesBuildPhase(string targetGuid)
    {
      string buildPhaseByTarget = this.GetSourcesBuildPhaseByTarget(targetGuid);
      if (buildPhaseByTarget != null)
        return buildPhaseByTarget;
      PBXSourcesBuildPhaseData sourcesBuildPhaseData = PBXSourcesBuildPhaseData.Create();
      this.sources.AddEntry(sourcesBuildPhaseData);
      this.nativeTargets[targetGuid].phases.AddGUID(sourcesBuildPhaseData.guid);
      return sourcesBuildPhaseData.guid;
    }

    public string GetHeadersBuildPhaseByTarget(string targetGuid)
    {
      foreach (string sectionGuid in (IEnumerable<string>) this.nativeTargets[targetGuid].phases)
      {
        if (this.BuildSectionAny(sectionGuid) is PBXHeadersBuildPhaseData)
          return sectionGuid;
      }
      return (string) null;
    }

    public string AddHeadersBuildPhase(string targetGuid)
    {
      string buildPhaseByTarget = this.GetHeadersBuildPhaseByTarget(targetGuid);
      if (buildPhaseByTarget != null)
        return buildPhaseByTarget;
      PBXHeadersBuildPhaseData headersBuildPhaseData = PBXHeadersBuildPhaseData.Create();
      this.headers.AddEntry(headersBuildPhaseData);
      this.nativeTargets[targetGuid].phases.AddGUID(headersBuildPhaseData.guid);
      return headersBuildPhaseData.guid;
    }

    public string GetResourcesBuildPhaseByTarget(string targetGuid)
    {
      foreach (string sectionGuid in (IEnumerable<string>) this.nativeTargets[targetGuid].phases)
      {
        if (this.BuildSectionAny(sectionGuid) is PBXResourcesBuildPhaseData)
          return sectionGuid;
      }
      return (string) null;
    }

    public string AddResourcesBuildPhase(string targetGuid)
    {
      string buildPhaseByTarget = this.GetResourcesBuildPhaseByTarget(targetGuid);
      if (buildPhaseByTarget != null)
        return buildPhaseByTarget;
      PBXResourcesBuildPhaseData resourcesBuildPhaseData = PBXResourcesBuildPhaseData.Create();
      this.resources.AddEntry(resourcesBuildPhaseData);
      this.nativeTargets[targetGuid].phases.AddGUID(resourcesBuildPhaseData.guid);
      return resourcesBuildPhaseData.guid;
    }

    public string GetFrameworksBuildPhaseByTarget(string targetGuid)
    {
      foreach (string sectionGuid in (IEnumerable<string>) this.nativeTargets[targetGuid].phases)
      {
        if (this.BuildSectionAny(sectionGuid) is PBXFrameworksBuildPhaseData)
          return sectionGuid;
      }
      return (string) null;
    }

    internal List<string> GetRealPathsOfAllFiles(PBXSourceTree sourceTree)
    {
      return this.m_Data.GetRealPathsRelativeToSourceTree(sourceTree);
    }

    public string AddFrameworksBuildPhase(string targetGuid)
    {
      string buildPhaseByTarget = this.GetFrameworksBuildPhaseByTarget(targetGuid);
      if (buildPhaseByTarget != null)
        return buildPhaseByTarget;
      PBXFrameworksBuildPhaseData frameworksBuildPhaseData = PBXFrameworksBuildPhaseData.Create();
      this.frameworks.AddEntry(frameworksBuildPhaseData);
      this.nativeTargets[targetGuid].phases.AddGUID(frameworksBuildPhaseData.guid);
      return frameworksBuildPhaseData.guid;
    }

    public string GetCopyFilesBuildPhaseByTarget(string targetGuid, string name, string dstPath, string subfolderSpec)
    {
      foreach (string sectionGuid in (IEnumerable<string>) this.nativeTargets[targetGuid].phases)
      {
        FileGUIDListBase fileGuidListBase = this.BuildSectionAny(sectionGuid);
        if (fileGuidListBase is PBXCopyFilesBuildPhaseData)
        {
          PBXCopyFilesBuildPhaseData filesBuildPhaseData = (PBXCopyFilesBuildPhaseData) fileGuidListBase;
          if (filesBuildPhaseData.name == name && filesBuildPhaseData.dstPath == dstPath && filesBuildPhaseData.dstSubfolderSpec == subfolderSpec)
            return sectionGuid;
        }
      }
      return (string) null;
    }

    public string AddCopyFilesBuildPhase(string targetGuid, string name, string dstPath, string subfolderSpec)
    {
      string buildPhaseByTarget = this.GetCopyFilesBuildPhaseByTarget(targetGuid, name, dstPath, subfolderSpec);
      if (buildPhaseByTarget != null)
        return buildPhaseByTarget;
      PBXCopyFilesBuildPhaseData filesBuildPhaseData = PBXCopyFilesBuildPhaseData.Create(name, dstPath, subfolderSpec);
      this.copyFiles.AddEntry(filesBuildPhaseData);
      this.nativeTargets[targetGuid].phases.AddGUID(filesBuildPhaseData.guid);
      return filesBuildPhaseData.guid;
    }

    public string InsertCopyFilesBuildPhase(int index, string targetGuid, string name, string dstPath, string subfolderSpec)
    {
      PBXCopyFilesBuildPhaseData filesBuildPhaseData = PBXCopyFilesBuildPhaseData.Create(name, dstPath, subfolderSpec);
      this.copyFiles.AddEntry(filesBuildPhaseData);
      this.nativeTargets[targetGuid].phases.InsertGUID(index, filesBuildPhaseData.guid);
      return filesBuildPhaseData.guid;
    }

    internal string GetConfigListForTarget(string targetGuid)
    {
      if (targetGuid == this.project.project.guid)
        return this.project.project.buildConfigList;
      return this.nativeTargets[targetGuid].buildConfigList;
    }

    public string[] GetAllBuildPhasesForTarget(string targetGuid)
    {
      return Enumerable.ToArray<string>((IEnumerable<string>) this.nativeTargets[targetGuid].phases);
    }

    public string GetBaseReferenceForConfig(string configGuid)
    {
      return this.buildConfigs[configGuid].baseConfigurationReference;
    }

    public void SetBaseReferenceForConfig(string configGuid, string baseReference)
    {
      this.buildConfigs[configGuid].baseConfigurationReference = baseReference;
    }

    public void AddBuildProperty(string targetGuid, string name, string value)
    {
      foreach (string configGuid in (IEnumerable<string>) this.buildConfigLists[this.GetConfigListForTarget(targetGuid)].buildConfigs)
        this.AddBuildPropertyForConfig(configGuid, name, value);
    }

    public void AddBuildProperty(IEnumerable<string> targetGuids, string name, string value)
    {
      foreach (string targetGuid in targetGuids)
        this.AddBuildProperty(targetGuid, name, value);
    }

    public void AddBuildPropertyForConfig(string configGuid, string name, string value)
    {
      this.buildConfigs[configGuid].AddProperty(name, value);
    }

    public void AddBuildPropertyForConfig(IEnumerable<string> configGuids, string name, string value)
    {
      foreach (string configGuid in configGuids)
        this.AddBuildPropertyForConfig(configGuid, name, value);
    }

    public void SetBuildProperty(string targetGuid, string name, string value)
    {
      foreach (string configGuid in (IEnumerable<string>) this.buildConfigLists[this.GetConfigListForTarget(targetGuid)].buildConfigs)
        this.SetBuildPropertyForConfig(configGuid, name, value);
    }

    public void SetBuildProperty(IEnumerable<string> targetGuids, string name, string value)
    {
      foreach (string targetGuid in targetGuids)
        this.SetBuildProperty(targetGuid, name, value);
    }

    public string GetBuildPropertyForAnyConfig(IEnumerable<string> targetGuids, string name)
    {
      foreach (string targetGuid in targetGuids)
      {
        string propertyForAnyConfig = this.GetBuildPropertyForAnyConfig(targetGuid, name);
        if (propertyForAnyConfig != null)
          return propertyForAnyConfig;
      }
      return (string) null;
    }

    public string GetBuildPropertyForAnyConfig(string targetGuid, string name)
    {
      foreach (string configGuid in (IEnumerable<string>) this.buildConfigLists[this.GetConfigListForTarget(targetGuid)].buildConfigs)
      {
        string propertyForConfig = this.GetBuildPropertyForConfig(configGuid, name);
        if (propertyForConfig != null)
          return propertyForConfig;
      }
      return (string) null;
    }

    public string GetBuildPropertyForConfig(string configGuid, string name)
    {
      if (this.buildConfigs[configGuid] != null)
        return this.buildConfigs[configGuid].GetProperty(name);
      return (string) null;
    }

    public void SetBuildPropertyForConfig(string configGuid, string name, string value)
    {
      this.buildConfigs[configGuid].SetProperty(name, value);
    }

    public void SetBuildPropertyForConfig(IEnumerable<string> configGuids, string name, string value)
    {
      foreach (string configGuid in configGuids)
        this.SetBuildPropertyForConfig(configGuid, name, value);
    }

    internal void RemoveBuildProperty(string targetGuid, string name)
    {
      foreach (string configGuid in (IEnumerable<string>) this.buildConfigLists[this.GetConfigListForTarget(targetGuid)].buildConfigs)
        this.RemoveBuildPropertyForConfig(configGuid, name);
    }

    internal void RemoveBuildProperty(IEnumerable<string> targetGuids, string name)
    {
      foreach (string targetGuid in targetGuids)
        this.RemoveBuildProperty(targetGuid, name);
    }

    internal void RemoveBuildPropertyForConfig(string configGuid, string name)
    {
      this.buildConfigs[configGuid].RemoveProperty(name);
    }

    internal void RemoveBuildPropertyForConfig(IEnumerable<string> configGuids, string name)
    {
      foreach (string configGuid in configGuids)
        this.RemoveBuildPropertyForConfig(configGuid, name);
    }

    internal void RemoveBuildPropertyValueList(string targetGuid, string name, IEnumerable<string> valueList)
    {
      foreach (string configGuid in (IEnumerable<string>) this.buildConfigLists[this.GetConfigListForTarget(targetGuid)].buildConfigs)
        this.RemoveBuildPropertyValueListForConfig(configGuid, name, valueList);
    }

    internal void RemoveBuildPropertyValueList(IEnumerable<string> targetGuids, string name, IEnumerable<string> valueList)
    {
      foreach (string targetGuid in targetGuids)
        this.RemoveBuildPropertyValueList(targetGuid, name, valueList);
    }

    internal void RemoveBuildPropertyValueListForConfig(string configGuid, string name, IEnumerable<string> valueList)
    {
      this.buildConfigs[configGuid].RemovePropertyValueList(name, valueList);
    }

    internal void RemoveBuildPropertyValueListForConfig(IEnumerable<string> configGuids, string name, IEnumerable<string> valueList)
    {
      foreach (string configGuid in configGuids)
        this.RemoveBuildPropertyValueListForConfig(configGuid, name, valueList);
    }

    public void UpdateBuildProperty(string targetGuid, string name, IEnumerable<string> addValues, IEnumerable<string> removeValues)
    {
      foreach (string configGuid in (IEnumerable<string>) this.buildConfigLists[this.GetConfigListForTarget(targetGuid)].buildConfigs)
        this.UpdateBuildPropertyForConfig(configGuid, name, addValues, removeValues);
    }

    public void UpdateBuildProperty(IEnumerable<string> targetGuids, string name, IEnumerable<string> addValues, IEnumerable<string> removeValues)
    {
      foreach (string targetGuid in targetGuids)
        this.UpdateBuildProperty(targetGuid, name, addValues, removeValues);
    }

    public void UpdateBuildPropertyForConfig(string configGuid, string name, IEnumerable<string> addValues, IEnumerable<string> removeValues)
    {
      XCBuildConfigurationData configurationData = this.buildConfigs[configGuid];
      if (configurationData == null)
        return;
      if (removeValues != null)
      {
        foreach (string str in removeValues)
          configurationData.RemovePropertyValue(name, str);
      }
      if (addValues != null)
      {
        foreach (string str in addValues)
          configurationData.AddProperty(name, str);
      }
    }

    public void UpdateBuildPropertyForConfig(IEnumerable<string> configGuids, string name, IEnumerable<string> addValues, IEnumerable<string> removeValues)
    {
      foreach (string targetGuid in configGuids)
        this.UpdateBuildProperty(targetGuid, name, addValues, removeValues);
    }

    internal string ShellScriptByName(string targetGuid, string name)
    {
      foreach (string index in (IEnumerable<string>) this.nativeTargets[targetGuid].phases)
      {
        PBXShellScriptBuildPhaseData scriptBuildPhaseData = this.shellScripts[index];
        if (scriptBuildPhaseData != null && scriptBuildPhaseData.name == name)
          return scriptBuildPhaseData.guid;
      }
      return (string) null;
    }

    private bool ListsEqual(List<string> a, List<string> b)
    {
      if (a == null && b == null)
        return true;
      if (a == null || b == null)
        return false;
      return a.Count == b.Count && Enumerable.SequenceEqual<string>((IEnumerable<string>) a, (IEnumerable<string>) b);
    }

    public string GetShellScriptBuildPhaseForTarget(string targetGuid, string name, string shellPath, string shellScript)
    {
      return this.GetShellScriptBuildPhaseForTarget(targetGuid, name, shellPath, shellScript, (List<string>) null);
    }

    public string GetShellScriptBuildPhaseForTarget(string targetGuid, string name, string shellPath, string shellScript, List<string> inputPaths)
    {
      foreach (string sectionGuid in (IEnumerable<string>) this.nativeTargets[targetGuid].phases)
      {
        FileGUIDListBase fileGuidListBase = this.BuildSectionAny(sectionGuid);
        if (fileGuidListBase is PBXShellScriptBuildPhaseData)
        {
          PBXShellScriptBuildPhaseData scriptBuildPhaseData = (PBXShellScriptBuildPhaseData) fileGuidListBase;
          if (scriptBuildPhaseData.name == name && scriptBuildPhaseData.shellPath == shellPath && scriptBuildPhaseData.shellScript == shellScript && this.ListsEqual(scriptBuildPhaseData.inputPaths, inputPaths))
            return sectionGuid;
        }
      }
      return (string) null;
    }

    public string AddShellScriptBuildPhase(string targetGuid, string name, string shellPath, string shellScript)
    {
      return this.AddShellScriptBuildPhase(targetGuid, name, shellPath, shellScript, (List<string>) null);
    }

    public string AddShellScriptBuildPhase(string targetGuid, string name, string shellPath, string shellScript, List<string> inputPaths)
    {
      PBXShellScriptBuildPhaseData scriptBuildPhaseData = PBXShellScriptBuildPhaseData.Create(name, shellPath, shellScript, inputPaths);
      this.shellScripts.AddEntry(scriptBuildPhaseData);
      this.nativeTargets[targetGuid].phases.AddGUID(scriptBuildPhaseData.guid);
      return scriptBuildPhaseData.guid;
    }

    public string InsertShellScriptBuildPhase(int index, string targetGuid, string name, string shellPath, string shellScript)
    {
      return this.InsertShellScriptBuildPhase(index, targetGuid, name, shellPath, shellScript, (List<string>) null);
    }

    public string InsertShellScriptBuildPhase(int index, string targetGuid, string name, string shellPath, string shellScript, List<string> inputPaths)
    {
      PBXShellScriptBuildPhaseData scriptBuildPhaseData = PBXShellScriptBuildPhaseData.Create(name, shellPath, shellScript, inputPaths);
      this.shellScripts.AddEntry(scriptBuildPhaseData);
      this.nativeTargets[targetGuid].phases.InsertGUID(index, scriptBuildPhaseData.guid);
      return scriptBuildPhaseData.guid;
    }

    internal void AppendShellScriptBuildPhase(IEnumerable<string> targetGuids, string name, string shellPath, string shellScript)
    {
      this.AppendShellScriptBuildPhase(targetGuids, name, shellPath, shellScript, (List<string>) null);
    }

    internal void AppendShellScriptBuildPhase(IEnumerable<string> targetGuids, string name, string shellPath, string shellScript, List<string> inputPaths)
    {
      PBXShellScriptBuildPhaseData scriptBuildPhaseData = PBXShellScriptBuildPhaseData.Create(name, shellPath, shellScript, inputPaths);
      this.shellScripts.AddEntry(scriptBuildPhaseData);
      foreach (string index in targetGuids)
        this.nativeTargets[index].phases.AddGUID(scriptBuildPhaseData.guid);
    }

    public void ReadFromFile(string path)
    {
      this.ReadFromString(File.ReadAllText(path));
    }

    public void ReadFromString(string src)
    {
      this.ReadFromStream((TextReader) new StringReader(src));
    }

    public void ReadFromStream(TextReader sr)
    {
      this.m_Data.ReadFromStream(sr);
    }

    public void WriteToFile(string path)
    {
      File.WriteAllText(path, this.WriteToString());
    }

    public void WriteToStream(TextWriter sw)
    {
      sw.Write(this.WriteToString());
    }

    public string WriteToString()
    {
      return this.m_Data.WriteToString();
    }

    internal PBXProjectObjectData GetProjectInternal()
    {
      return this.project.project;
    }

    internal void SetTargetAttributes(string key, string value)
    {
      PBXElementDict propertiesRaw = this.project.project.GetPropertiesRaw();
      PBXElementDict pbxElementDict1 = !propertiesRaw.Contains("attributes") ? propertiesRaw.CreateDict("attributes") : propertiesRaw["attributes"] as PBXElementDict;
      PBXElementDict pbxElementDict2 = !pbxElementDict1.Contains("TargetAttributes") ? pbxElementDict1.CreateDict("TargetAttributes") : pbxElementDict1["TargetAttributes"] as PBXElementDict;
      foreach (KeyValuePair<string, PBXNativeTargetData> keyValuePair in this.nativeTargets.GetEntries())
        (!pbxElementDict2.Contains(keyValuePair.Key) ? pbxElementDict2.CreateDict(keyValuePair.Key) : pbxElementDict2[keyValuePair.Key].AsDict()).SetString(key, value);
      this.project.project.UpdateVars();
    }
  }
}
