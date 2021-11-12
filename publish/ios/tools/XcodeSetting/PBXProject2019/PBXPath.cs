// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBXPath
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.IO;

namespace UnityEditor.iOS.Xcode
{
  internal class PBXPath
  {
    public static string FixSlashes(string path)
    {
      if (path == null)
        return (string) null;
      return path.Replace('\\', '/');
    }

    public static void Combine(string path1, PBXSourceTree tree1, string path2, PBXSourceTree tree2, out string resPath, out PBXSourceTree resTree)
    {
      if (tree2 == PBXSourceTree.Group)
      {
        resPath = PBXPath.Combine(path1, path2);
        resTree = tree1;
      }
      else
      {
        resPath = path2;
        resTree = tree2;
      }
    }

    public static string Combine(string path1, string path2)
    {
      if (path2.StartsWith("/"))
        return path2;
      if (path1.EndsWith("/"))
        return path1 + path2;
      if (path1 == "")
        return path2;
      if (path2 == "")
        return path1;
      return path1 + "/" + path2;
    }

    public static string GetDirectory(string path)
    {
      string str = path;
      char[] chArray = new char[1];
      int index = 0;
      int num = 47;
      chArray[index] = (char) num;
      path = str.TrimEnd(chArray);
      int length = path.LastIndexOf('/');
      if (length == -1)
        return "";
      return path.Substring(0, length);
    }

    public static string GetCurrentDirectory()
    {
      if (Environment.OSVersion.Platform != PlatformID.MacOSX && Environment.OSVersion.Platform != PlatformID.Unix)
        throw new Exception("PBX project compatible current directory can only obtained on OSX");
      string path = PBXPath.FixSlashes(Directory.GetCurrentDirectory());
      if (!PBXPath.IsPathRooted(path))
        return "/" + path;
      return path;
    }

    public static string GetFilename(string path)
    {
      int num = path.LastIndexOf('/');
      if (num == -1)
        return path;
      return path.Substring(num + 1);
    }

    public static bool IsPathRooted(string path)
    {
      if (path == null || path.Length == 0)
        return false;
      return (int) path[0] == 47;
    }

    public static string GetFullPath(string path)
    {
      if (PBXPath.IsPathRooted(path))
        return path;
      return PBXPath.Combine(PBXPath.GetCurrentDirectory(), path);
    }

    public static string[] Split(string path)
    {
      if (string.IsNullOrEmpty(path))
        return new string[0];
      string str = path;
      char[] separator = new char[1];
      int index = 0;
      int num1 = 47;
      separator[index] = (char) num1;
      int num2 = 1;
      return str.Split(separator, (StringSplitOptions) num2);
    }
  }
}
