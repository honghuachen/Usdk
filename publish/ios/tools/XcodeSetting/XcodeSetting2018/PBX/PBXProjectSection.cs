// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXProjectSection
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
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
