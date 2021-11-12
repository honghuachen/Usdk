// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.PBXStream
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class PBXStream
  {
    private static bool DontNeedQuotes(string src)
    {
      if (src.Length == 0)
        return false;
      bool flag = false;
      for (int index = 0; index < src.Length; ++index)
      {
        char c = src[index];
        if (!char.IsLetterOrDigit(c) && (int) c != 46 && (int) c != 42 && (int) c != 95)
        {
          if ((int) c != 47)
            return false;
          flag = true;
        }
      }
      return !flag || !src.Contains("//") && !src.Contains("/*") && !src.Contains("*/");
    }

    public static string QuoteStringIfNeeded(string src)
    {
      if (PBXStream.DontNeedQuotes(src))
        return src;
      return "\"" + src.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n") + "\"";
    }

    public static string UnquoteString(string src)
    {
      if (!src.StartsWith("\"") || !src.EndsWith("\""))
        return src;
      return src.Substring(1, src.Length - 2).Replace("\\\\", "嚟").Replace("\\\"", "\"").Replace("\\n", "\n").Replace("嚟", "\\");
    }
  }
}
