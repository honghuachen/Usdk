// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.ProjectReference
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
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
