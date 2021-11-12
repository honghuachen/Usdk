// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.GUIDList
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class GUIDList : IEnumerable<string>, IEnumerable
  {
    private List<string> m_List = new List<string>();

    public int Count
    {
      get
      {
        return this.m_List.Count;
      }
    }

    public GUIDList()
    {
    }

    public GUIDList(List<string> data)
    {
      this.m_List = data;
    }

    public static implicit operator List<string>(GUIDList list)
    {
      return list.m_List;
    }

    public static implicit operator GUIDList(List<string> data)
    {
      return new GUIDList(data);
    }

    public void InsertGUID(int index, string guid)
    {
      if (index < this.m_List.Count)
        this.m_List.Insert(index, guid);
      else
        this.m_List.Add(guid);
    }

    public void AddGUID(string guid)
    {
      this.m_List.Add(guid);
    }

    public void RemoveGUID(string guid)
    {
      this.m_List.RemoveAll((Predicate<string>) (x => x == guid));
    }

    public bool Contains(string guid)
    {
      return this.m_List.Contains(guid);
    }

    public void Clear()
    {
      this.m_List.Clear();
    }

    IEnumerator<string> IEnumerable<string>.GetEnumerator()
    {
      return (IEnumerator<string>) this.m_List.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.m_List.GetEnumerator();
    }
  }
}
