// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXProjectSection
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXProjectSection : KnownSectionBase<PBXProjectObjectData>
  {
    public PBXProjectObjectData project
    {
      get
      {
        using (IEnumerator<KeyValuePair<string, PBXProjectObjectData>> enumerator = this.GetEntries().GetEnumerator())
        {
          if (enumerator.MoveNext())
            return enumerator.Current.Value;
        }
        return (PBXProjectObjectData) null;
      }
    }

    public PBXProjectSection()
      : base("PBXProject")
    {
    }
  }
}
