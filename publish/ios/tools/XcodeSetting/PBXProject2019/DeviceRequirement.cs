// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.DeviceRequirement
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;

namespace UnityEditor.iOS.Xcode
{
  internal class DeviceRequirement
  {
    internal Dictionary<string, string> values = new Dictionary<string, string>();

    public DeviceRequirement()
    {
      this.values.Add("idiom", DeviceTypeRequirement.Any);
    }

    public DeviceRequirement AddDevice(string device)
    {
      this.AddCustom(DeviceTypeRequirement.Key, device);
      return this;
    }

    public DeviceRequirement AddMemory(string memory)
    {
      this.AddCustom(MemoryRequirement.Key, memory);
      return this;
    }

    public DeviceRequirement AddGraphics(string graphics)
    {
      this.AddCustom(GraphicsRequirement.Key, graphics);
      return this;
    }

    public DeviceRequirement AddWidthClass(string sizeClass)
    {
      this.AddCustom(SizeClassRequirement.WidthKey, sizeClass);
      return this;
    }

    public DeviceRequirement AddHeightClass(string sizeClass)
    {
      this.AddCustom(SizeClassRequirement.HeightKey, sizeClass);
      return this;
    }

    public DeviceRequirement AddScale(string scale)
    {
      this.AddCustom(ScaleRequirement.Key, scale);
      return this;
    }

    public DeviceRequirement AddCustom(string key, string value)
    {
      if (this.values.ContainsKey(key))
        this.values.Remove(key);
      this.values.Add(key, value);
      return this;
    }
  }
}
