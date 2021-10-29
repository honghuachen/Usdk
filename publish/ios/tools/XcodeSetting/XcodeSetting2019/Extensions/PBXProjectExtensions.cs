// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.Extensions.PBXProjectExtensions
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.PBX;

namespace UnityEditor.iOS.Xcode.Extensions
{
  public static class PBXProjectExtensions
  {
    internal static PBXProjectExtensions.FlagList appExtensionReleaseBuildFlags;
    internal static PBXProjectExtensions.FlagList appExtensionDebugBuildFlags;
    internal static PBXProjectExtensions.FlagList watchExtensionReleaseBuildFlags;
    internal static PBXProjectExtensions.FlagList watchExtensionDebugBuildFlags;
    internal static PBXProjectExtensions.FlagList watchAppReleaseBuildFlags;
    internal static PBXProjectExtensions.FlagList watchAppDebugBuildFlags;

    static PBXProjectExtensions()
    {
      PBXProjectExtensions.FlagList flagList1 = new PBXProjectExtensions.FlagList();
      string flag1 = "LD_RUNPATH_SEARCH_PATHS";
      string str1 = "$(inherited)";
      flagList1.Add(flag1, str1);
      string flag2 = "LD_RUNPATH_SEARCH_PATHS";
      string str2 = "@executable_path/Frameworks";
      flagList1.Add(flag2, str2);
      string flag3 = "LD_RUNPATH_SEARCH_PATHS";
      string str3 = "@executable_path/../../Frameworks";
      flagList1.Add(flag3, str3);
      string flag4 = "PRODUCT_NAME";
      string str4 = "$(TARGET_NAME)";
      flagList1.Add(flag4, str4);
      string flag5 = "SKIP_INSTALL";
      string str5 = "YES";
      flagList1.Add(flag5, str5);
      PBXProjectExtensions.appExtensionReleaseBuildFlags = flagList1;
      PBXProjectExtensions.FlagList flagList2 = new PBXProjectExtensions.FlagList();
      string flag6 = "LD_RUNPATH_SEARCH_PATHS";
      string str6 = "$(inherited)";
      flagList2.Add(flag6, str6);
      string flag7 = "LD_RUNPATH_SEARCH_PATHS";
      string str7 = "@executable_path/Frameworks";
      flagList2.Add(flag7, str7);
      string flag8 = "LD_RUNPATH_SEARCH_PATHS";
      string str8 = "@executable_path/../../Frameworks";
      flagList2.Add(flag8, str8);
      string flag9 = "PRODUCT_NAME";
      string str9 = "$(TARGET_NAME)";
      flagList2.Add(flag9, str9);
      string flag10 = "SKIP_INSTALL";
      string str10 = "YES";
      flagList2.Add(flag10, str10);
      PBXProjectExtensions.appExtensionDebugBuildFlags = flagList2;
      PBXProjectExtensions.FlagList flagList3 = new PBXProjectExtensions.FlagList();
      string flag11 = "ASSETCATALOG_COMPILER_COMPLICATION_NAME";
      string str11 = "Complication";
      flagList3.Add(flag11, str11);
      string flag12 = "CLANG_ANALYZER_NONNULL";
      string str12 = "YES";
      flagList3.Add(flag12, str12);
      string flag13 = "CLANG_WARN_DOCUMENTATION_COMMENTS";
      string str13 = "YES";
      flagList3.Add(flag13, str13);
      string flag14 = "CLANG_WARN_INFINITE_RECURSION";
      string str14 = "YES";
      flagList3.Add(flag14, str14);
      string flag15 = "CLANG_WARN_SUSPICIOUS_MOVE";
      string str15 = "YES";
      flagList3.Add(flag15, str15);
      string flag16 = "DEBUG_INFORMATION_FORMAT";
      string str16 = "dwarf-with-dsym";
      flagList3.Add(flag16, str16);
      string flag17 = "GCC_NO_COMMON_BLOCKS";
      string str17 = "YES";
      flagList3.Add(flag17, str17);
      string flag18 = "LD_RUNPATH_SEARCH_PATHS";
      string str18 = "$(inherited)";
      flagList3.Add(flag18, str18);
      string flag19 = "LD_RUNPATH_SEARCH_PATHS";
      string str19 = "@executable_path/Frameworks";
      flagList3.Add(flag19, str19);
      string flag20 = "LD_RUNPATH_SEARCH_PATHS";
      string str20 = "@executable_path/../../Frameworks";
      flagList3.Add(flag20, str20);
      string flag21 = "PRODUCT_NAME";
      string str21 = "${TARGET_NAME}";
      flagList3.Add(flag21, str21);
      string flag22 = "SDKROOT";
      string str22 = "watchos";
      flagList3.Add(flag22, str22);
      string flag23 = "SKIP_INSTALL";
      string str23 = "YES";
      flagList3.Add(flag23, str23);
      string flag24 = "TARGETED_DEVICE_FAMILY";
      string str24 = "4";
      flagList3.Add(flag24, str24);
      string flag25 = "WATCHOS_DEPLOYMENT_TARGET";
      string str25 = "3.1";
      flagList3.Add(flag25, str25);
      string flag26 = "ARCHS";
      string str26 = "$(ARCHS_STANDARD)";
      flagList3.Add(flag26, str26);
      string flag27 = "SUPPORTED_PLATFORMS";
      string str27 = "watchos";
      flagList3.Add(flag27, str27);
      string flag28 = "SUPPORTED_PLATFORMS";
      string str28 = "watchsimulator";
      flagList3.Add(flag28, str28);
      PBXProjectExtensions.watchExtensionReleaseBuildFlags = flagList3;
      PBXProjectExtensions.FlagList flagList4 = new PBXProjectExtensions.FlagList();
      string flag29 = "ASSETCATALOG_COMPILER_COMPLICATION_NAME";
      string str29 = "Complication";
      flagList4.Add(flag29, str29);
      string flag30 = "CLANG_ANALYZER_NONNULL";
      string str30 = "YES";
      flagList4.Add(flag30, str30);
      string flag31 = "CLANG_WARN_DOCUMENTATION_COMMENTS";
      string str31 = "YES";
      flagList4.Add(flag31, str31);
      string flag32 = "CLANG_WARN_INFINITE_RECURSION";
      string str32 = "YES";
      flagList4.Add(flag32, str32);
      string flag33 = "CLANG_WARN_SUSPICIOUS_MOVE";
      string str33 = "YES";
      flagList4.Add(flag33, str33);
      string flag34 = "DEBUG_INFORMATION_FORMAT";
      string str34 = "dwarf";
      flagList4.Add(flag34, str34);
      string flag35 = "ENABLE_TESTABILITY";
      string str35 = "YES";
      flagList4.Add(flag35, str35);
      string flag36 = "GCC_NO_COMMON_BLOCKS";
      string str36 = "YES";
      flagList4.Add(flag36, str36);
      string flag37 = "LD_RUNPATH_SEARCH_PATHS";
      string str37 = "$(inherited)";
      flagList4.Add(flag37, str37);
      string flag38 = "LD_RUNPATH_SEARCH_PATHS";
      string str38 = "@executable_path/Frameworks";
      flagList4.Add(flag38, str38);
      string flag39 = "LD_RUNPATH_SEARCH_PATHS";
      string str39 = "@executable_path/../../Frameworks";
      flagList4.Add(flag39, str39);
      string flag40 = "PRODUCT_NAME";
      string str40 = "${TARGET_NAME}";
      flagList4.Add(flag40, str40);
      string flag41 = "SDKROOT";
      string str41 = "watchos";
      flagList4.Add(flag41, str41);
      string flag42 = "SKIP_INSTALL";
      string str42 = "YES";
      flagList4.Add(flag42, str42);
      string flag43 = "TARGETED_DEVICE_FAMILY";
      string str43 = "4";
      flagList4.Add(flag43, str43);
      string flag44 = "WATCHOS_DEPLOYMENT_TARGET";
      string str44 = "3.1";
      flagList4.Add(flag44, str44);
      string flag45 = "ARCHS";
      string str45 = "$(ARCHS_STANDARD)";
      flagList4.Add(flag45, str45);
      string flag46 = "SUPPORTED_PLATFORMS";
      string str46 = "watchos";
      flagList4.Add(flag46, str46);
      string flag47 = "SUPPORTED_PLATFORMS";
      string str47 = "watchsimulator";
      flagList4.Add(flag47, str47);
      PBXProjectExtensions.watchExtensionDebugBuildFlags = flagList4;
      PBXProjectExtensions.FlagList flagList5 = new PBXProjectExtensions.FlagList();
      string flag48 = "ASSETCATALOG_COMPILER_APPICON_NAME";
      string str48 = "AppIcon";
      flagList5.Add(flag48, str48);
      string flag49 = "CLANG_ANALYZER_NONNULL";
      string str49 = "YES";
      flagList5.Add(flag49, str49);
      string flag50 = "CLANG_WARN_DOCUMENTATION_COMMENTS";
      string str50 = "YES";
      flagList5.Add(flag50, str50);
      string flag51 = "CLANG_WARN_INFINITE_RECURSION";
      string str51 = "YES";
      flagList5.Add(flag51, str51);
      string flag52 = "CLANG_WARN_SUSPICIOUS_MOVE";
      string str52 = "YES";
      flagList5.Add(flag52, str52);
      string flag53 = "DEBUG_INFORMATION_FORMAT";
      string str53 = "dwarf-with-dsym";
      flagList5.Add(flag53, str53);
      string flag54 = "GCC_NO_COMMON_BLOCKS";
      string str54 = "YES";
      flagList5.Add(flag54, str54);
      string flag55 = "PRODUCT_NAME";
      string str55 = "$(TARGET_NAME)";
      flagList5.Add(flag55, str55);
      string flag56 = "SDKROOT";
      string str56 = "watchos";
      flagList5.Add(flag56, str56);
      string flag57 = "SKIP_INSTALL";
      string str57 = "YES";
      flagList5.Add(flag57, str57);
      string flag58 = "TARGETED_DEVICE_FAMILY";
      string str58 = "4";
      flagList5.Add(flag58, str58);
      string flag59 = "WATCHOS_DEPLOYMENT_TARGET";
      string str59 = "3.1";
      flagList5.Add(flag59, str59);
      string flag60 = "ARCHS";
      string str60 = "$(ARCHS_STANDARD)";
      flagList5.Add(flag60, str60);
      string flag61 = "SUPPORTED_PLATFORMS";
      string str61 = "watchos";
      flagList5.Add(flag61, str61);
      string flag62 = "SUPPORTED_PLATFORMS";
      string str62 = "watchsimulator";
      flagList5.Add(flag62, str62);
      PBXProjectExtensions.watchAppReleaseBuildFlags = flagList5;
      PBXProjectExtensions.FlagList flagList6 = new PBXProjectExtensions.FlagList();
      string flag63 = "ASSETCATALOG_COMPILER_APPICON_NAME";
      string str63 = "AppIcon";
      flagList6.Add(flag63, str63);
      string flag64 = "CLANG_ANALYZER_NONNULL";
      string str64 = "YES";
      flagList6.Add(flag64, str64);
      string flag65 = "CLANG_WARN_DOCUMENTATION_COMMENTS";
      string str65 = "YES";
      flagList6.Add(flag65, str65);
      string flag66 = "CLANG_WARN_INFINITE_RECURSION";
      string str66 = "YES";
      flagList6.Add(flag66, str66);
      string flag67 = "CLANG_WARN_SUSPICIOUS_MOVE";
      string str67 = "YES";
      flagList6.Add(flag67, str67);
      string flag68 = "DEBUG_INFORMATION_FORMAT";
      string str68 = "dwarf";
      flagList6.Add(flag68, str68);
      string flag69 = "ENABLE_TESTABILITY";
      string str69 = "YES";
      flagList6.Add(flag69, str69);
      string flag70 = "GCC_NO_COMMON_BLOCKS";
      string str70 = "YES";
      flagList6.Add(flag70, str70);
      string flag71 = "PRODUCT_NAME";
      string str71 = "$(TARGET_NAME)";
      flagList6.Add(flag71, str71);
      string flag72 = "SDKROOT";
      string str72 = "watchos";
      flagList6.Add(flag72, str72);
      string flag73 = "SKIP_INSTALL";
      string str73 = "YES";
      flagList6.Add(flag73, str73);
      string flag74 = "TARGETED_DEVICE_FAMILY";
      string str74 = "4";
      flagList6.Add(flag74, str74);
      string flag75 = "WATCHOS_DEPLOYMENT_TARGET";
      string str75 = "3.1";
      flagList6.Add(flag75, str75);
      string flag76 = "ARCHS";
      string str76 = "$(ARCHS_STANDARD)";
      flagList6.Add(flag76, str76);
      string flag77 = "SUPPORTED_PLATFORMS";
      string str77 = "watchos";
      flagList6.Add(flag77, str77);
      string flag78 = "SUPPORTED_PLATFORMS";
      string str78 = "watchsimulator";
      flagList6.Add(flag78, str78);
      PBXProjectExtensions.watchAppDebugBuildFlags = flagList6;
    }

    private static void SetBuildFlagsFromDict(this PBXProject proj, string configGuid, IEnumerable<KeyValuePair<string, string>> data)
    {
      foreach (KeyValuePair<string, string> keyValuePair in data)
        proj.AddBuildPropertyForConfig(configGuid, keyValuePair.Key, keyValuePair.Value);
    }

    internal static void SetDefaultAppExtensionReleaseBuildFlags(this PBXProject proj, string configGuid)
    {
      PBXProjectExtensions.SetBuildFlagsFromDict(proj, configGuid, (IEnumerable<KeyValuePair<string, string>>) PBXProjectExtensions.appExtensionReleaseBuildFlags);
    }

    internal static void SetDefaultAppExtensionDebugBuildFlags(this PBXProject proj, string configGuid)
    {
      PBXProjectExtensions.SetBuildFlagsFromDict(proj, configGuid, (IEnumerable<KeyValuePair<string, string>>) PBXProjectExtensions.appExtensionDebugBuildFlags);
    }

    internal static void SetDefaultWatchExtensionReleaseBuildFlags(this PBXProject proj, string configGuid)
    {
      PBXProjectExtensions.SetBuildFlagsFromDict(proj, configGuid, (IEnumerable<KeyValuePair<string, string>>) PBXProjectExtensions.watchExtensionReleaseBuildFlags);
    }

    internal static void SetDefaultWatchExtensionDebugBuildFlags(this PBXProject proj, string configGuid)
    {
      PBXProjectExtensions.SetBuildFlagsFromDict(proj, configGuid, (IEnumerable<KeyValuePair<string, string>>) PBXProjectExtensions.watchExtensionDebugBuildFlags);
    }

    internal static void SetDefaultWatchAppReleaseBuildFlags(this PBXProject proj, string configGuid)
    {
      PBXProjectExtensions.SetBuildFlagsFromDict(proj, configGuid, (IEnumerable<KeyValuePair<string, string>>) PBXProjectExtensions.watchAppReleaseBuildFlags);
    }

    internal static void SetDefaultWatchAppDebugBuildFlags(this PBXProject proj, string configGuid)
    {
      PBXProjectExtensions.SetBuildFlagsFromDict(proj, configGuid, (IEnumerable<KeyValuePair<string, string>>) PBXProjectExtensions.watchAppDebugBuildFlags);
    }

    public static string AddAppExtension(this PBXProject proj, string mainTargetGuid, string name, string bundleId, string infoPlistPath)
    {
      string ext = ".appex";
      string str = proj.AddTarget(name, ext, "com.apple.product-type.app-extension");
      foreach (string name1 in proj.BuildConfigNames())
      {
        string configGuid = proj.BuildConfigByName(str, name1);
        if (name1.Contains("Debug"))
          PBXProjectExtensions.SetDefaultAppExtensionDebugBuildFlags(proj, configGuid);
        else
          PBXProjectExtensions.SetDefaultAppExtensionReleaseBuildFlags(proj, configGuid);
        proj.SetBuildPropertyForConfig(configGuid, "INFOPLIST_FILE", infoPlistPath);
        proj.SetBuildPropertyForConfig(configGuid, "PRODUCT_BUNDLE_IDENTIFIER", bundleId);
      }
      proj.AddSourcesBuildPhase(str);
      proj.AddResourcesBuildPhase(str);
      proj.AddFrameworksBuildPhase(str);
      string sectionGuid = proj.AddCopyFilesBuildPhase(mainTargetGuid, "Embed App Extensions", "", "13");
      proj.AddFileToBuildSection(mainTargetGuid, sectionGuid, proj.GetTargetProductFileRef(str));
      proj.AddTargetDependency(mainTargetGuid, str);
      return str;
    }

    public static string AddWatchApp(this PBXProject proj, string mainTargetGuid, string watchExtensionTargetGuid, string name, string bundleId, string infoPlistPath)
    {
      string str1 = proj.AddTarget(name, ".app", "com.apple.product-type.application.watchapp2");
      string str2 = proj.nativeTargets[watchExtensionTargetGuid].name.Replace(" ", "_");
      foreach (string name1 in proj.BuildConfigNames())
      {
        string configGuid = proj.BuildConfigByName(str1, name1);
        if (name1.Contains("Debug"))
          PBXProjectExtensions.SetDefaultWatchAppDebugBuildFlags(proj, configGuid);
        else
          PBXProjectExtensions.SetDefaultWatchAppReleaseBuildFlags(proj, configGuid);
        proj.SetBuildPropertyForConfig(configGuid, "PRODUCT_BUNDLE_IDENTIFIER", bundleId);
        proj.SetBuildPropertyForConfig(configGuid, "INFOPLIST_FILE", infoPlistPath);
        proj.SetBuildPropertyForConfig(configGuid, "IBSC_MODULE", str2);
      }
      proj.AddResourcesBuildPhase(str1);
      string sectionGuid1 = proj.AddCopyFilesBuildPhase(str1, "Embed App Extensions", "", "13");
      proj.AddFileToBuildSection(str1, sectionGuid1, proj.GetTargetProductFileRef(watchExtensionTargetGuid));
      string sectionGuid2 = proj.AddCopyFilesBuildPhase(mainTargetGuid, "Embed Watch Content", "$(CONTENTS_FOLDER_PATH)/Watch", "16");
      proj.AddFileToBuildSection(mainTargetGuid, sectionGuid2, proj.GetTargetProductFileRef(str1));
      proj.AddTargetDependency(str1, watchExtensionTargetGuid);
      proj.AddTargetDependency(mainTargetGuid, str1);
      return str1;
    }

    public static string AddWatchExtension(this PBXProject proj, string mainTarget, string name, string bundleId, string infoPlistPath)
    {
      string targetGuid = proj.AddTarget(name, ".appex", "com.apple.product-type.watchkit2-extension");
      foreach (string name1 in proj.BuildConfigNames())
      {
        string configGuid = proj.BuildConfigByName(targetGuid, name1);
        if (name1.Contains("Debug"))
          PBXProjectExtensions.SetDefaultWatchExtensionDebugBuildFlags(proj, configGuid);
        else
          PBXProjectExtensions.SetDefaultWatchExtensionReleaseBuildFlags(proj, configGuid);
        proj.SetBuildPropertyForConfig(configGuid, "PRODUCT_BUNDLE_IDENTIFIER", bundleId);
        proj.SetBuildPropertyForConfig(configGuid, "INFOPLIST_FILE", infoPlistPath);
      }
      proj.AddSourcesBuildPhase(targetGuid);
      proj.AddResourcesBuildPhase(targetGuid);
      proj.AddFrameworksBuildPhase(targetGuid);
      return targetGuid;
    }

    internal static void AddExternalProjectDependency(this PBXProject proj, string path, string projectPath, PBXSourceTree sourceTree)
    {
      if (sourceTree == PBXSourceTree.Group)
        throw new Exception("sourceTree must not be PBXSourceTree.Group");
      path = PBXPath.FixSlashes(path);
      projectPath = PBXPath.FixSlashes(projectPath);
      PBXGroupData relative = PBXGroupData.CreateRelative("Products");
      proj.GroupsAddDuplicate(relative);
      PBXFileReferenceData fromFile = PBXFileReferenceData.CreateFromFile(path, Path.GetFileName(projectPath), sourceTree);
      proj.FileRefsAdd(path, projectPath, (PBXGroupData) null, fromFile);
      proj.CreateSourceGroup(PBXPath.GetDirectory(projectPath)).children.AddGUID(fromFile.guid);
      proj.project.project.AddReference(relative.guid, fromFile.guid);
    }

    internal static void AddExternalLibraryDependency(this PBXProject proj, string targetGuid, string filename, string remoteFileGuid, string projectPath, string remoteInfo)
    {
      PBXNativeTargetData target = proj.nativeTargets[targetGuid];
      filename = PBXPath.FixSlashes(filename);
      projectPath = PBXPath.FixSlashes(projectPath);
      string fileGuidByRealPath = proj.FindFileGuidByRealPath(projectPath);
      if (fileGuidByRealPath == null)
        throw new Exception("No such project");
      string guid = (string) null;
      foreach (ProjectReference projectReference in proj.project.project.projectReferences)
      {
        if (projectReference.projectRef == fileGuidByRealPath)
        {
          guid = projectReference.group;
          break;
        }
      }
      if (guid == null)
        throw new Exception("Malformed project: no project in project references");
      PBXGroupData pbxGroupData = proj.GroupsGet(guid);
      string extension = Path.GetExtension(filename);
      if (!FileTypeUtils.IsBuildableFile(extension))
        throw new Exception("Wrong file extension");
      PBXContainerItemProxyData containerItemProxyData = PBXContainerItemProxyData.Create(fileGuidByRealPath, "2", remoteFileGuid, remoteInfo);
      proj.containerItems.AddEntry(containerItemProxyData);
      string typeName = FileTypeUtils.GetTypeName(extension);
      PBXReferenceProxyData referenceProxyData = PBXReferenceProxyData.Create(filename, typeName, containerItemProxyData.guid, "BUILT_PRODUCTS_DIR");
      proj.references.AddEntry(referenceProxyData);
      PBXBuildFileData fromFile = PBXBuildFileData.CreateFromFile(referenceProxyData.guid, false, (string) null);
      proj.BuildFilesAdd(targetGuid, fromFile);
      proj.BuildSectionAny(target, extension, false).files.AddGUID(fromFile.guid);
      pbxGroupData.children.AddGUID(referenceProxyData.guid);
    }

    public static void AddFileToEmbedFrameworks(this PBXProject proj, string targetGuid, string fileGuid, string linkToTargetGuid = null)
    {
      linkToTargetGuid = linkToTargetGuid ?? targetGuid;
      PBXNativeTargetData nativeTargetData = proj.nativeTargets[targetGuid];
      string index = proj.AddCopyFilesBuildPhase(targetGuid, "Embed Frameworks", "", "10");
      PBXCopyFilesBuildPhaseData phase = proj.copyFiles[index];
      PBXBuildFileData buildFile = proj.FindFrameworkByFileGuid(phase, fileGuid);
      if (buildFile == null)
      {
        buildFile = PBXBuildFileData.CreateFromFile(fileGuid, false, (string) null);
        proj.BuildFilesAdd(linkToTargetGuid, buildFile);
        phase.files.AddGUID(buildFile.guid);
      }
      proj.SetBuildProperty(linkToTargetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");
      buildFile.codeSignOnCopy = true;
      buildFile.removeHeadersOnCopy = true;
    }

    internal class FlagList : List<KeyValuePair<string, string>>
    {
      public void Add(string flag, string value)
      {
        this.Add(new KeyValuePair<string, string>(flag, value));
      }
    }
  }
}
