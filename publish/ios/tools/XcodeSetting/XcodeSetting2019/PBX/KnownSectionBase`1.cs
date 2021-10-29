// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.KnownSectionBase`1
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class KnownSectionBase<T> : SectionBase where T : PBXObjectData, new()
  {
    private Dictionary<string, T> m_Entries = new Dictionary<string, T>();
    private string m_Name;

    public T this[string guid]
    {
      get
      {
        if (this.m_Entries.ContainsKey(guid))
          return this.m_Entries[guid];
        return default (T);
      }
    }

    public KnownSectionBase(string sectionName)
    {
      this.m_Name = sectionName;
    }

    public IEnumerable<KeyValuePair<string, T>> GetEntries()
    {
      return (IEnumerable<KeyValuePair<string, T>>) this.m_Entries;
    }

    public IEnumerable<string> GetGuids()
    {
      return (IEnumerable<string>) this.m_Entries.Keys;
    }

    public IEnumerable<T> GetObjects()
    {
      return (IEnumerable<T>) this.m_Entries.Values;
    }

    public override void AddObject(string key, PBXElementDict value)
    {
      T instance = Activator.CreateInstance<T>();
      instance.guid = key;
      instance.SetPropertiesWhenSerializing(value);
      instance.UpdateVars();
      this.m_Entries[instance.guid] = instance;
    }

    public override void WriteSection(StringBuilder sb, GUIDToCommentMap comments)
    {
      if (this.m_Entries.Count == 0)
        return;
      sb.AppendFormat("\n\n/* Begin {0} section */", (object) this.m_Name);
      List<string> list = new List<string>((IEnumerable<string>) this.m_Entries.Keys);
      list.Sort((IComparer<string>) StringComparer.Ordinal);
      foreach (string index in list)
      {
        T obj = this.m_Entries[index];
        obj.UpdateProps();
        sb.Append("\n\t\t");
        comments.WriteStringBuilder(sb, obj.guid);
        sb.Append(" = ");
        Serializer.WriteDict(sb, obj.GetPropertiesWhenSerializing(), 2, obj.shouldCompact, obj.checker, comments);
        sb.Append(";");
      }
      sb.AppendFormat("\n/* End {0} section */", (object) this.m_Name);
    }

    public bool HasEntry(string guid)
    {
      return this.m_Entries.ContainsKey(guid);
    }

    public void AddEntry(T obj)
    {
      this.m_Entries[obj.guid] = obj;
    }

    public void RemoveEntry(string guid)
    {
      if (!this.m_Entries.ContainsKey(guid))
        return;
      this.m_Entries.Remove(guid);
    }
  }
}
