// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PlistElementDate
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;

namespace UnityEditor.iOS.Xcode
{
  public class PlistElementDate : PlistElement
  {
    public DateTime value;

    public PlistElementDate(DateTime date)
    {
      this.value = date;
    }
  }
}
