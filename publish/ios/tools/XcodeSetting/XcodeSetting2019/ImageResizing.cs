// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.ImageResizing
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

namespace UnityEditor.iOS.Xcode
{
  internal class ImageResizing
  {
    public ImageResizing.SlicingType type = ImageResizing.SlicingType.HorizontalAndVertical;
    public int left = 0;
    public int right = 0;
    public int top = 0;
    public int bottom = 0;
    public ImageResizing.ResizeMode centerResizeMode = ImageResizing.ResizeMode.Stretch;
    public int centerWidth = 0;
    public int centerHeight = 0;

    public enum SlicingType
    {
      Horizontal,
      Vertical,
      HorizontalAndVertical,
    }

    public enum ResizeMode
    {
      Stretch,
      Tile,
    }
  }
}
