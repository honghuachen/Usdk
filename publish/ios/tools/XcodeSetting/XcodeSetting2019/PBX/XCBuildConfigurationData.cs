// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.XCBuildConfigurationData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class XCBuildConfigurationData : PBXObjectData
  {
    protected SortedDictionary<string, BuildConfigEntryData> entries = new SortedDictionary<string, BuildConfigEntryData>();
    public string baseConfigurationReference;

    public string name
    {
      get
      {
        return this.GetPropertyString("name");
      }
    }

    private static string EscapeWithQuotesIfNeeded(string name, string value)
    {
      if (name != "LIBRARY_SEARCH_PATHS" && name != "FRAMEWORK_SEARCH_PATHS" || !value.Contains(" ") || (int) Enumerable.First<char>((IEnumerable<char>) value) == 34 && (int) Enumerable.Last<char>((IEnumerable<char>) value) == 34)
        return value;
      return "\"" + value + "\"";
    }

    public string GetProperty(string name)
    {
      if (this.entries.ContainsKey(name))
        return string.Join(" ", this.entries[name].val.ToArray());
      return (string) null;
    }

    public void SetProperty(string name, string value)
    {
      this.entries[name] = BuildConfigEntryData.FromNameValue(name, XCBuildConfigurationData.EscapeWithQuotesIfNeeded(name, value));
    }

    public void AddProperty(string name, string value)
    {
      if (this.entries.ContainsKey(name))
        this.entries[name].AddValue(XCBuildConfigurationData.EscapeWithQuotesIfNeeded(name, value));
      else
        this.SetProperty(name, value);
    }

    public void RemoveProperty(string name)
    {
      if (!this.entries.ContainsKey(name))
        return;
      this.entries.Remove(name);
    }

    public void RemovePropertyValue(string name, string value)
    {
      if (!this.entries.ContainsKey(name))
        return;
      this.entries[name].RemoveValue(XCBuildConfigurationData.EscapeWithQuotesIfNeeded(name, value));
    }

    public void RemovePropertyValueList(string name, IEnumerable<string> valueList)
    {
      if (!this.entries.ContainsKey(name))
        return;
      this.entries[name].RemoveValueList(valueList);
    }

    public static XCBuildConfigurationData Create(string name)
    {
      XCBuildConfigurationData configurationData = new XCBuildConfigurationData();
      configurationData.guid = PBXGUID.Generate();
      configurationData.SetPropertyString("isa", "XCBuildConfiguration");
      configurationData.SetPropertyString("name", name);
      return configurationData;
    }

    public override void UpdateProps()
    {
      this.SetPropertyString("baseConfigurationReference", this.baseConfigurationReference);
      PBXElementDict dict = this.m_Properties.CreateDict("buildSettings");
      foreach (KeyValuePair<string, BuildConfigEntryData> keyValuePair in this.entries)
      {
        if (keyValuePair.Value.val.Count != 0)
        {
          if (keyValuePair.Value.val.Count == 1)
          {
            dict.SetString(keyValuePair.Key, keyValuePair.Value.val[0]);
          }
          else
          {
            PBXElementArray array = dict.CreateArray(keyValuePair.Key);
            foreach (string val in keyValuePair.Value.val)
              array.AddString(val);
          }
        }
      }
    }

    public override void UpdateVars()
    {
      this.baseConfigurationReference = this.GetPropertyString("baseConfigurationReference");
      this.entries = new SortedDictionary<string, BuildConfigEntryData>();
      if (!this.m_Properties.Contains("buildSettings"))
        return;
      PBXElementDict pbxElementDict = this.m_Properties["buildSettings"].AsDict();
      foreach (string index in (IEnumerable<string>) pbxElementDict.values.Keys)
      {
        PBXElement pbxElement1 = pbxElementDict[index];
        if (pbxElement1 is PBXElementString)
        {
          if (this.entries.ContainsKey(index))
            this.entries[index].val.Add(pbxElement1.AsString());
          else
            this.entries.Add(index, BuildConfigEntryData.FromNameValue(index, pbxElement1.AsString()));
        }
        else if (pbxElement1 is PBXElementArray)
        {
          foreach (PBXElement pbxElement2 in pbxElement1.AsArray().values)
          {
            if (pbxElement2 is PBXElementString)
            {
              if (this.entries.ContainsKey(index))
                this.entries[index].val.Add(pbxElement2.AsString());
              else
                this.entries.Add(index, BuildConfigEntryData.FromNameValue(index, pbxElement2.AsString()));
            }
          }
        }
      }
    }
  }
}
