// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.ProjectReference
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class ProjectReference
  {
    public string group;
    public string projectRef;

    public static ProjectReference Create(string group, string projectRef)
    {
      return new ProjectReference()
      {
        group = group,
        projectRef = projectRef
      };
    }
  }
}
