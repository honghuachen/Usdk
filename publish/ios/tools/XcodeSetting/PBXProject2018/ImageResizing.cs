// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.ImageResizing
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
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
