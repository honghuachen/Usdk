// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PropertyCommentChecker
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PropertyCommentChecker
  {
    private int m_Level;
    private bool m_All;
    private List<List<string>> m_Props;

    protected PropertyCommentChecker(int level, List<List<string>> props)
    {
      this.m_Level = level;
      this.m_All = false;
      this.m_Props = props;
    }

    public PropertyCommentChecker()
    {
      this.m_Level = 0;
      this.m_All = false;
      this.m_Props = new List<List<string>>();
    }

    public PropertyCommentChecker(IEnumerable<string> props)
    {
      this.m_Level = 0;
      this.m_All = false;
      this.m_Props = new List<List<string>>();
      foreach (string str1 in props)
      {
        List<List<string>> list1 = this.m_Props;
        string str2 = str1;
        char[] chArray = new char[1];
        int index = 0;
        int num = 47;
        chArray[index] = (char) num;
        List<string> list2 = new List<string>((IEnumerable<string>) str2.Split(chArray));
        list1.Add(list2);
      }
    }

    private bool CheckContained(string prop)
    {
      if (this.m_All)
        return true;
      foreach (List<string> list in this.m_Props)
      {
        if (list.Count == this.m_Level + 1)
        {
          if (list[this.m_Level] == prop)
            return true;
          if (list[this.m_Level] == "*")
          {
            this.m_All = true;
            return true;
          }
        }
      }
      return false;
    }

    public bool CheckStringValueInArray(string value)
    {
      return this.CheckContained(value);
    }

    public bool CheckKeyInDict(string key)
    {
      return this.CheckContained(key);
    }

    public bool CheckStringValueInDict(string key, string value)
    {
      foreach (List<string> list in this.m_Props)
      {
        if (list.Count == this.m_Level + 2 && ((list[this.m_Level] == "*" || list[this.m_Level] == key) && list[this.m_Level + 1] == "*" || list[this.m_Level + 1] == value))
          return true;
      }
      return false;
    }

    public PropertyCommentChecker NextLevel(string prop)
    {
      List<List<string>> props = new List<List<string>>();
      foreach (List<string> list in this.m_Props)
      {
        if (list.Count > this.m_Level + 1 && (list[this.m_Level] == "*" || list[this.m_Level] == prop))
          props.Add(list);
      }
      return new PropertyCommentChecker(this.m_Level + 1, props);
    }
  }
}
