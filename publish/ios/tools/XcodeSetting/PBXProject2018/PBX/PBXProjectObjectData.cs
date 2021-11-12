// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXProjectObjectData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;
using UnityEditor.iOS.Xcode;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXProjectObjectData : PBXObjectData
  {
    public List<ProjectReference> projectReferences = new List<ProjectReference>();
    public List<string> targets = new List<string>();
    public List<string> knownAssetTags = new List<string>();
    public List<PBXCapabilityType.TargetCapabilityPair> capabilities = new List<PBXCapabilityType.TargetCapabilityPair>();
    public Dictionary<string, string> teamIDs = new Dictionary<string, string>();
    private static PropertyCommentChecker checkerData;
    public string buildConfigList;
    public string entitlementsFile;

    internal override PropertyCommentChecker checker
    {
      get
      {
        return PBXProjectObjectData.checkerData;
      }
    }

    public string mainGroup
    {
      get
      {
        return this.GetPropertyString("mainGroup");
      }
    }

    static PBXProjectObjectData()
    {
      string[] strArray = new string[5];
      int index1 = 0;
      string str1 = "buildConfigurationList/*";
      strArray[index1] = str1;
      int index2 = 1;
      string str2 = "mainGroup/*";
      strArray[index2] = str2;
      int index3 = 2;
      string str3 = "projectReferences/*/ProductGroup/*";
      strArray[index3] = str3;
      int index4 = 3;
      string str4 = "projectReferences/*/ProjectRef/*";
      strArray[index4] = str4;
      int index5 = 4;
      string str5 = "targets/*";
      strArray[index5] = str5;
      PBXProjectObjectData.checkerData = new PropertyCommentChecker((IEnumerable<string>) strArray);
    }

    public void AddReference(string productGroup, string projectRef)
    {
      this.projectReferences.Add(ProjectReference.Create(productGroup, projectRef));
    }

    public override void UpdateProps()
    {
      this.m_Properties.values.Remove("projectReferences");
      if (this.projectReferences.Count > 0)
      {
        PBXElementArray array = this.m_Properties.CreateArray("projectReferences");
        foreach (ProjectReference projectReference in this.projectReferences)
        {
          PBXElementDict pbxElementDict = array.AddDict();
          pbxElementDict.SetString("ProductGroup", projectReference.group);
          pbxElementDict.SetString("ProjectRef", projectReference.projectRef);
        }
      }
      this.SetPropertyList("targets", this.targets);
      this.SetPropertyString("buildConfigurationList", this.buildConfigList);
      if (this.knownAssetTags.Count > 0)
      {
        PBXElementArray array = (!this.m_Properties.Contains("attributes") ? this.m_Properties.CreateDict("attributes") : this.m_Properties["attributes"].AsDict()).CreateArray("knownAssetTags");
        foreach (string val in this.knownAssetTags)
          array.AddString(val);
      }
      foreach (PBXCapabilityType.TargetCapabilityPair targetCapabilityPair in this.capabilities)
      {
        PBXElementDict pbxElementDict1 = !this.m_Properties.Contains("attributes") ? this.m_Properties.CreateDict("attributes") : this.m_Properties["attributes"].AsDict();
        PBXElementDict pbxElementDict2 = !pbxElementDict1.Contains("TargetAttributes") ? pbxElementDict1.CreateDict("TargetAttributes") : pbxElementDict1["TargetAttributes"].AsDict();
        PBXElementDict pbxElementDict3 = !pbxElementDict2.Contains(targetCapabilityPair.targetGuid) ? pbxElementDict2.CreateDict(targetCapabilityPair.targetGuid) : pbxElementDict2[targetCapabilityPair.targetGuid].AsDict();
        PBXElementDict pbxElementDict4 = !pbxElementDict3.Contains("SystemCapabilities") ? pbxElementDict3.CreateDict("SystemCapabilities") : pbxElementDict3["SystemCapabilities"].AsDict();
        string id = targetCapabilityPair.capability.id;
        (!pbxElementDict4.Contains(id) ? pbxElementDict4.CreateDict(id) : pbxElementDict4[id].AsDict()).SetString("enabled", "1");
      }
      foreach (KeyValuePair<string, string> keyValuePair in this.teamIDs)
      {
        PBXElementDict pbxElementDict1 = !this.m_Properties.Contains("attributes") ? this.m_Properties.CreateDict("attributes") : this.m_Properties["attributes"].AsDict();
        PBXElementDict pbxElementDict2 = !pbxElementDict1.Contains("TargetAttributes") ? pbxElementDict1.CreateDict("TargetAttributes") : pbxElementDict1["TargetAttributes"].AsDict();
        (!pbxElementDict2.Contains(keyValuePair.Key) ? pbxElementDict2.CreateDict(keyValuePair.Key) : pbxElementDict2[keyValuePair.Key].AsDict()).SetString("DevelopmentTeam", keyValuePair.Value);
      }
    }

    public override void UpdateVars()
    {
      this.projectReferences = new List<ProjectReference>();
      if (this.m_Properties.Contains("projectReferences"))
      {
        foreach (PBXElement pbxElement in this.m_Properties["projectReferences"].AsArray().values)
        {
          PBXElementDict pbxElementDict = pbxElement.AsDict();
          if (pbxElementDict.Contains("ProductGroup") && pbxElementDict.Contains("ProjectRef"))
            this.projectReferences.Add(ProjectReference.Create(pbxElementDict["ProductGroup"].AsString(), pbxElementDict["ProjectRef"].AsString()));
        }
      }
      this.targets = this.GetPropertyList("targets");
      this.buildConfigList = this.GetPropertyString("buildConfigurationList");
      this.knownAssetTags = new List<string>();
      if (!this.m_Properties.Contains("attributes"))
        return;
      PBXElementDict pbxElementDict1 = this.m_Properties["attributes"].AsDict();
      if (pbxElementDict1.Contains("knownAssetTags"))
      {
        foreach (PBXElement pbxElement in pbxElementDict1["knownAssetTags"].AsArray().values)
          this.knownAssetTags.Add(pbxElement.AsString());
      }
      this.capabilities = new List<PBXCapabilityType.TargetCapabilityPair>();
      this.teamIDs = new Dictionary<string, string>();
      if (pbxElementDict1.Contains("TargetAttributes"))
      {
        foreach (KeyValuePair<string, PBXElement> keyValuePair1 in (IEnumerable<KeyValuePair<string, PBXElement>>) pbxElementDict1["TargetAttributes"].AsDict().values)
        {
          if (keyValuePair1.Key == "DevelopmentTeam")
            this.teamIDs.Add(keyValuePair1.Key, keyValuePair1.Value.AsString());
          if (keyValuePair1.Key == "SystemCapabilities")
          {
            foreach (KeyValuePair<string, PBXElement> keyValuePair2 in (IEnumerable<KeyValuePair<string, PBXElement>>) pbxElementDict1["SystemCapabilities"].AsDict().values)
              this.capabilities.Add(new PBXCapabilityType.TargetCapabilityPair(keyValuePair1.Key, PBXCapabilityType.StringToPBXCapabilityType(keyValuePair2.Value.AsString())));
          }
        }
      }
    }
  }
}
