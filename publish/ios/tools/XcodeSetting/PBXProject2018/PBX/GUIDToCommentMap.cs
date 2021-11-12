// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.GUIDToCommentMap
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;
using System.Text;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class GUIDToCommentMap
  {
    private Dictionary<string, string> m_Dict = new Dictionary<string, string>();

    public string this[string guid]
    {
      get
      {
        if (this.m_Dict.ContainsKey(guid))
          return this.m_Dict[guid];
        return (string) null;
      }
    }

    public void Add(string guid, string comment)
    {
      if (this.m_Dict.ContainsKey(guid))
        return;
      this.m_Dict.Add(guid, comment);
    }

    public void Remove(string guid)
    {
      this.m_Dict.Remove(guid);
    }

    public string Write(string guid)
    {
      string str = this[guid];
      if (str == null)
        return guid;
      return string.Format("{0} /* {1} */", (object) guid, (object) str);
    }

    public void WriteStringBuilder(StringBuilder sb, string guid)
    {
      string str = this[guid];
      if (str == null)
        sb.Append(guid);
      else
        sb.Append(guid).Append(" /* ").Append(str).Append(" */");
    }
  }
}
