// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXObjectData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXObjectData
  {
    private static PropertyCommentChecker checkerData = new PropertyCommentChecker();
    protected PBXElementDict m_Properties = new PBXElementDict();
    public string guid;

    internal virtual PropertyCommentChecker checker
    {
      get
      {
        return PBXObjectData.checkerData;
      }
    }

    internal virtual bool shouldCompact
    {
      get
      {
        return false;
      }
    }

    internal void SetPropertiesWhenSerializing(PBXElementDict props)
    {
      this.m_Properties = props;
    }

    internal PBXElementDict GetPropertiesWhenSerializing()
    {
      return this.m_Properties;
    }

    internal PBXElementDict GetPropertiesRaw()
    {
      this.UpdateProps();
      return this.m_Properties;
    }

    internal string GetPropertyString(string name)
    {
      PBXElement pbxElement = this.m_Properties[name];
      if (pbxElement == null)
        return (string) null;
      return pbxElement.AsString();
    }

    protected void SetPropertyString(string name, string value)
    {
      if (value == null)
        this.m_Properties.Remove(name);
      else
        this.m_Properties.SetString(name, value);
    }

    internal List<string> GetPropertyList(string name)
    {
      PBXElement pbxElement1 = this.m_Properties[name];
      if (pbxElement1 == null)
        return (List<string>) null;
      List<string> list = new List<string>();
      foreach (PBXElement pbxElement2 in pbxElement1.AsArray().values)
        list.Add(pbxElement2.AsString());
      return list;
    }

    protected void SetPropertyList(string name, List<string> value)
    {
      if (value == null)
      {
        this.m_Properties.Remove(name);
      }
      else
      {
        PBXElementArray array = this.m_Properties.CreateArray(name);
        foreach (string val in value)
          array.AddString(val);
      }
    }

    public virtual void UpdateProps()
    {
    }

    public virtual void UpdateVars()
    {
    }
  }
}
