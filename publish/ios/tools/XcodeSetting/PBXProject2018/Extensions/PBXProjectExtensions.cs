// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.Extensions.PBXProjectExtensions
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
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
    internal static PBXProjectExtensions.FlagList appExtensionReleaseBuildFlags = new PBXProjectExtensions.FlagList()
    {
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "$(inherited)"
      },
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "@executable_path/Frameworks"
      },
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "@executable_path/../../Frameworks"
      },
      {
        "PRODUCT_NAME",
        "$(TARGET_NAME)"
      },
      {
        "SKIP_INSTALL",
        "YES"
      }
    };
    internal static PBXProjectExtensions.FlagList appExtensionDebugBuildFlags = new PBXProjectExtensions.FlagList()
    {
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "$(inherited)"
      },
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "@executable_path/Frameworks"
      },
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "@executable_path/../../Frameworks"
      },
      {
        "PRODUCT_NAME",
        "$(TARGET_NAME)"
      },
      {
        "SKIP_INSTALL",
        "YES"
      }
    };
    internal static PBXProjectExtensions.FlagList watchExtensionReleaseBuildFlags = new PBXProjectExtensions.FlagList()
    {
      {
        "ASSETCATALOG_COMPILER_COMPLICATION_NAME",
        "Complication"
      },
      {
        "CLANG_ANALYZER_NONNULL",
        "YES"
      },
      {
        "CLANG_WARN_DOCUMENTATION_COMMENTS",
        "YES"
      },
      {
        "CLANG_WARN_INFINITE_RECURSION",
        "YES"
      },
      {
        "CLANG_WARN_SUSPICIOUS_MOVE",
        "YES"
      },
      {
        "DEBUG_INFORMATION_FORMAT",
        "dwarf-with-dsym"
      },
      {
        "GCC_NO_COMMON_BLOCKS",
        "YES"
      },
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "$(inherited)"
      },
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "@executable_path/Frameworks"
      },
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "@executable_path/../../Frameworks"
      },
      {
        "PRODUCT_NAME",
        "${TARGET_NAME}"
      },
      {
        "SDKROOT",
        "watchos"
      },
      {
        "SKIP_INSTALL",
        "YES"
      },
      {
        "TARGETED_DEVICE_FAMILY",
        "4"
      },
      {
        "WATCHOS_DEPLOYMENT_TARGET",
        "3.1"
      },
      {
        "ARCHS",
        "$(ARCHS_STANDARD)"
      },
      {
        "SUPPORTED_PLATFORMS",
        "watchos"
      },
      {
        "SUPPORTED_PLATFORMS",
        "watchsimulator"
      }
    };
    internal static PBXProjectExtensions.FlagList watchExtensionDebugBuildFlags = new PBXProjectExtensions.FlagList()
    {
      {
        "ASSETCATALOG_COMPILER_COMPLICATION_NAME",
        "Complication"
      },
      {
        "CLANG_ANALYZER_NONNULL",
        "YES"
      },
      {
        "CLANG_WARN_DOCUMENTATION_COMMENTS",
        "YES"
      },
      {
        "CLANG_WARN_INFINITE_RECURSION",
        "YES"
      },
      {
        "CLANG_WARN_SUSPICIOUS_MOVE",
        "YES"
      },
      {
        "DEBUG_INFORMATION_FORMAT",
        "dwarf"
      },
      {
        "ENABLE_TESTABILITY",
        "YES"
      },
      {
        "GCC_NO_COMMON_BLOCKS",
        "YES"
      },
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "$(inherited)"
      },
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "@executable_path/Frameworks"
      },
      {
        "LD_RUNPATH_SEARCH_PATHS",
        "@executable_path/../../Frameworks"
      },
      {
        "PRODUCT_NAME",
        "${TARGET_NAME}"
      },
      {
        "SDKROOT",
        "watchos"
      },
      {
        "SKIP_INSTALL",
        "YES"
      },
      {
        "TARGETED_DEVICE_FAMILY",
        "4"
      },
      {
        "WATCHOS_DEPLOYMENT_TARGET",
        "3.1"
      },
      {
        "ARCHS",
        "$(ARCHS_STANDARD)"
      },
      {
        "SUPPORTED_PLATFORMS",
        "watchos"
      },
      {
        "SUPPORTED_PLATFORMS",
        "watchsimulator"
      }
    };
    internal static PBXProjectExtensions.FlagList watchAppReleaseBuildFlags = new PBXProjectExtensions.FlagList()
    {
      {
        "ASSETCATALOG_COMPILER_APPICON_NAME",
        "AppIcon"
      },
      {
        "CLANG_ANALYZER_NONNULL",
        "YES"
      },
      {
        "CLANG_WARN_DOCUMENTATION_COMMENTS",
        "YES"
      },
      {
        "CLANG_WARN_INFINITE_RECURSION",
        "YES"
      },
      {
        "CLANG_WARN_SUSPICIOUS_MOVE",
        "YES"
      },
      {
        "DEBUG_INFORMATION_FORMAT",
        "dwarf-with-dsym"
      },
      {
        "GCC_NO_COMMON_BLOCKS",
        "YES"
      },
      {
        "PRODUCT_NAME",
        "$(TARGET_NAME)"
      },
      {
        "SDKROOT",
        "watchos"
      },
      {
        "SKIP_INSTALL",
        "YES"
      },
      {
        "TARGETED_DEVICE_FAMILY",
        "4"
      },
      {
        "WATCHOS_DEPLOYMENT_TARGET",
        "3.1"
      },
      {
        "ARCHS",
        "$(ARCHS_STANDARD)"
      },
      {
        "SUPPORTED_PLATFORMS",
        "watchos"
      },
      {
        "SUPPORTED_PLATFORMS",
        "watchsimulator"
      }
    };
    internal static PBXProjectExtensions.FlagList watchAppDebugBuildFlags = new PBXProjectExtensions.FlagList()
    {
      {
        "ASSETCATALOG_COMPILER_APPICON_NAME",
        "AppIcon"
      },
      {
        "CLANG_ANALYZER_NONNULL",
        "YES"
      },
      {
        "CLANG_WARN_DOCUMENTATION_COMMENTS",
        "YES"
      },
      {
        "CLANG_WARN_INFINITE_RECURSION",
        "YES"
      },
      {
        "CLANG_WARN_SUSPICIOUS_MOVE",
        "YES"
      },
      {
        "DEBUG_INFORMATION_FORMAT",
        "dwarf"
      },
      {
        "ENABLE_TESTABILITY",
        "YES"
      },
      {
        "GCC_NO_COMMON_BLOCKS",
        "YES"
      },
      {
        "PRODUCT_NAME",
        "$(TARGET_NAME)"
      },
      {
        "SDKROOT",
        "watchos"
      },
      {
        "SKIP_INSTALL",
        "YES"
      },
      {
        "TARGETED_DEVICE_FAMILY",
        "4"
      },
      {
        "WATCHOS_DEPLOYMENT_TARGET",
        "3.1"
      },
      {
        "ARCHS",
        "$(ARCHS_STANDARD)"
      },
      {
        "SUPPORTED_PLATFORMS",
        "watchos"
      },
      {
        "SUPPORTED_PLATFORMS",
        "watchsimulator"
      }
    };

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

    public static void AddFileToEmbedFrameworks(this PBXProject proj, string targetGuid, string fileGuid)
    {
      PBXNativeTargetData nativeTargetData = proj.nativeTargets[targetGuid];
      string index = proj.AddCopyFilesBuildPhase(targetGuid, "Embed Frameworks", "", "10");
      PBXCopyFilesBuildPhaseData phase = proj.copyFiles[index];
      PBXBuildFileData buildFile = proj.FindFrameworkByFileGuid(phase, fileGuid);
      if (buildFile == null)
      {
        buildFile = PBXBuildFileData.CreateFromFile(fileGuid, false, (string) null);
        proj.BuildFilesAdd(targetGuid, buildFile);
        phase.files.AddGUID(buildFile.guid);
      }
      proj.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");
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
