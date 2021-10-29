// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.BuildConfigEntryData
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class BuildConfigEntryData
  {
    public List<string> val = new List<string>();
    public string name;

    public static string ExtractValue(string src)
    {
      string str = src.Trim();
      char[] chArray = new char[1];
      int index = 0;
      int num = 44;
      chArray[index] = (char) num;
      return PBXStream.UnquoteString(str.TrimEnd(chArray));
    }

    public void AddValue(string value)
    {
      if (this.val.Contains(value))
        return;
      this.val.Add(value);
    }

        public void RemoveValue(string value)
        {
            this.val.RemoveAll((Predicate<string>)(v => v == value));
        }

    public void RemoveValueList(IEnumerable<string> values)
    {
      List<string> list = new List<string>(values);
      if (list.Count == 0)
        return;
      for (int index1 = 0; index1 < this.val.Count - list.Count; ++index1)
      {
        bool flag = true;
        for (int index2 = 0; index2 < list.Count; ++index2)
        {
          if (this.val[index1 + index2] != list[index2])
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          this.val.RemoveRange(index1, list.Count);
          break;
        }
      }
    }

    public static BuildConfigEntryData FromNameValue(string name, string value)
    {
      BuildConfigEntryData buildConfigEntryData = new BuildConfigEntryData();
      buildConfigEntryData.name = name;
      buildConfigEntryData.AddValue(value);
      return buildConfigEntryData;
    }
  }
}
