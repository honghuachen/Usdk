// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.FileTypeUtils
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;
using UnityEditor.iOS.Xcode;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class FileTypeUtils
  {
    private static readonly Dictionary<string, FileTypeUtils.FileTypeDesc> types;
    private static readonly Dictionary<PBXSourceTree, string> sourceTree;
    private static readonly Dictionary<string, PBXSourceTree> stringToSourceTreeMap;

    static FileTypeUtils()
    {
      Dictionary<string, FileTypeUtils.FileTypeDesc> dictionary1 = new Dictionary<string, FileTypeUtils.FileTypeDesc>();
      string key1 = "a";
      FileTypeUtils.FileTypeDesc fileTypeDesc1 = new FileTypeUtils.FileTypeDesc("archive.ar", PBXFileType.Framework);
      dictionary1.Add(key1, fileTypeDesc1);
      string key2 = "app";
      FileTypeUtils.FileTypeDesc fileTypeDesc2 = new FileTypeUtils.FileTypeDesc("wrapper.application", PBXFileType.NotBuildable, true);
      dictionary1.Add(key2, fileTypeDesc2);
      string key3 = "appex";
      FileTypeUtils.FileTypeDesc fileTypeDesc3 = new FileTypeUtils.FileTypeDesc("wrapper.app-extension", PBXFileType.CopyFile);
      dictionary1.Add(key3, fileTypeDesc3);
      string key4 = "bin";
      FileTypeUtils.FileTypeDesc fileTypeDesc4 = new FileTypeUtils.FileTypeDesc("archive.macbinary", PBXFileType.Resource);
      dictionary1.Add(key4, fileTypeDesc4);
      string key5 = "s";
      FileTypeUtils.FileTypeDesc fileTypeDesc5 = new FileTypeUtils.FileTypeDesc("sourcecode.asm", PBXFileType.Source);
      dictionary1.Add(key5, fileTypeDesc5);
      string key6 = "c";
      FileTypeUtils.FileTypeDesc fileTypeDesc6 = new FileTypeUtils.FileTypeDesc("sourcecode.c.c", PBXFileType.Source);
      dictionary1.Add(key6, fileTypeDesc6);
      string key7 = "cc";
      FileTypeUtils.FileTypeDesc fileTypeDesc7 = new FileTypeUtils.FileTypeDesc("sourcecode.cpp.cpp", PBXFileType.Source);
      dictionary1.Add(key7, fileTypeDesc7);
      string key8 = "cpp";
      FileTypeUtils.FileTypeDesc fileTypeDesc8 = new FileTypeUtils.FileTypeDesc("sourcecode.cpp.cpp", PBXFileType.Source);
      dictionary1.Add(key8, fileTypeDesc8);
      string key9 = "swift";
      FileTypeUtils.FileTypeDesc fileTypeDesc9 = new FileTypeUtils.FileTypeDesc("sourcecode.swift", PBXFileType.Source);
      dictionary1.Add(key9, fileTypeDesc9);
      string key10 = "dll";
      FileTypeUtils.FileTypeDesc fileTypeDesc10 = new FileTypeUtils.FileTypeDesc("file", PBXFileType.NotBuildable);
      dictionary1.Add(key10, fileTypeDesc10);
      string key11 = "framework";
      FileTypeUtils.FileTypeDesc fileTypeDesc11 = new FileTypeUtils.FileTypeDesc("wrapper.framework", PBXFileType.Framework);
      dictionary1.Add(key11, fileTypeDesc11);
      string key12 = "h";
      FileTypeUtils.FileTypeDesc fileTypeDesc12 = new FileTypeUtils.FileTypeDesc("sourcecode.c.h", PBXFileType.Header);
      dictionary1.Add(key12, fileTypeDesc12);
      string key13 = "pch";
      FileTypeUtils.FileTypeDesc fileTypeDesc13 = new FileTypeUtils.FileTypeDesc("sourcecode.c.h", PBXFileType.Header);
      dictionary1.Add(key13, fileTypeDesc13);
      string key14 = "icns";
      FileTypeUtils.FileTypeDesc fileTypeDesc14 = new FileTypeUtils.FileTypeDesc("image.icns", PBXFileType.Resource);
      dictionary1.Add(key14, fileTypeDesc14);
      string key15 = "xcassets";
      FileTypeUtils.FileTypeDesc fileTypeDesc15 = new FileTypeUtils.FileTypeDesc("folder.assetcatalog", PBXFileType.Resource);
      dictionary1.Add(key15, fileTypeDesc15);
      string key16 = "inc";
      FileTypeUtils.FileTypeDesc fileTypeDesc16 = new FileTypeUtils.FileTypeDesc("sourcecode.inc", PBXFileType.NotBuildable);
      dictionary1.Add(key16, fileTypeDesc16);
      string key17 = "m";
      FileTypeUtils.FileTypeDesc fileTypeDesc17 = new FileTypeUtils.FileTypeDesc("sourcecode.c.objc", PBXFileType.Source);
      dictionary1.Add(key17, fileTypeDesc17);
      string key18 = "mm";
      FileTypeUtils.FileTypeDesc fileTypeDesc18 = new FileTypeUtils.FileTypeDesc("sourcecode.cpp.objcpp", PBXFileType.Source);
      dictionary1.Add(key18, fileTypeDesc18);
      string key19 = "nib";
      FileTypeUtils.FileTypeDesc fileTypeDesc19 = new FileTypeUtils.FileTypeDesc("wrapper.nib", PBXFileType.Resource);
      dictionary1.Add(key19, fileTypeDesc19);
      string key20 = "plist";
      FileTypeUtils.FileTypeDesc fileTypeDesc20 = new FileTypeUtils.FileTypeDesc("text.plist.xml", PBXFileType.Resource);
      dictionary1.Add(key20, fileTypeDesc20);
      string key21 = "png";
      FileTypeUtils.FileTypeDesc fileTypeDesc21 = new FileTypeUtils.FileTypeDesc("image.png", PBXFileType.Resource);
      dictionary1.Add(key21, fileTypeDesc21);
      string key22 = "rtf";
      FileTypeUtils.FileTypeDesc fileTypeDesc22 = new FileTypeUtils.FileTypeDesc("text.rtf", PBXFileType.Resource);
      dictionary1.Add(key22, fileTypeDesc22);
      string key23 = "tiff";
      FileTypeUtils.FileTypeDesc fileTypeDesc23 = new FileTypeUtils.FileTypeDesc("image.tiff", PBXFileType.Resource);
      dictionary1.Add(key23, fileTypeDesc23);
      string key24 = "txt";
      FileTypeUtils.FileTypeDesc fileTypeDesc24 = new FileTypeUtils.FileTypeDesc("text", PBXFileType.Resource);
      dictionary1.Add(key24, fileTypeDesc24);
      string key25 = "json";
      FileTypeUtils.FileTypeDesc fileTypeDesc25 = new FileTypeUtils.FileTypeDesc("text.json", PBXFileType.Resource);
      dictionary1.Add(key25, fileTypeDesc25);
      string key26 = "xcodeproj";
      FileTypeUtils.FileTypeDesc fileTypeDesc26 = new FileTypeUtils.FileTypeDesc("wrapper.pb-project", PBXFileType.NotBuildable);
      dictionary1.Add(key26, fileTypeDesc26);
      string key27 = "xib";
      FileTypeUtils.FileTypeDesc fileTypeDesc27 = new FileTypeUtils.FileTypeDesc("file.xib", PBXFileType.Resource);
      dictionary1.Add(key27, fileTypeDesc27);
      string key28 = "strings";
      FileTypeUtils.FileTypeDesc fileTypeDesc28 = new FileTypeUtils.FileTypeDesc("text.plist.strings", PBXFileType.Resource);
      dictionary1.Add(key28, fileTypeDesc28);
      string key29 = "storyboard";
      FileTypeUtils.FileTypeDesc fileTypeDesc29 = new FileTypeUtils.FileTypeDesc("file.storyboard", PBXFileType.Resource);
      dictionary1.Add(key29, fileTypeDesc29);
      string key30 = "bundle";
      FileTypeUtils.FileTypeDesc fileTypeDesc30 = new FileTypeUtils.FileTypeDesc("wrapper.plug-in", PBXFileType.Resource);
      dictionary1.Add(key30, fileTypeDesc30);
      string key31 = "dylib";
      FileTypeUtils.FileTypeDesc fileTypeDesc31 = new FileTypeUtils.FileTypeDesc("compiled.mach-o.dylib", PBXFileType.Framework);
      dictionary1.Add(key31, fileTypeDesc31);
      string key32 = "tbd";
      FileTypeUtils.FileTypeDesc fileTypeDesc32 = new FileTypeUtils.FileTypeDesc("sourcecode.text-based-dylib-definition", PBXFileType.Framework);
      dictionary1.Add(key32, fileTypeDesc32);
      FileTypeUtils.types = dictionary1;
      Dictionary<PBXSourceTree, string> dictionary2 = new Dictionary<PBXSourceTree, string>();
      int num1 = 0;
      string str1 = "<absolute>";
      dictionary2.Add((PBXSourceTree) num1, str1);
      int num2 = 2;
      string str2 = "<group>";
      dictionary2.Add((PBXSourceTree) num2, str2);
      int num3 = 3;
      string str3 = "BUILT_PRODUCTS_DIR";
      dictionary2.Add((PBXSourceTree) num3, str3);
      int num4 = 4;
      string str4 = "DEVELOPER_DIR";
      dictionary2.Add((PBXSourceTree) num4, str4);
      int num5 = 5;
      string str5 = "SDKROOT";
      dictionary2.Add((PBXSourceTree) num5, str5);
      int num6 = 1;
      string str6 = "SOURCE_ROOT";
      dictionary2.Add((PBXSourceTree) num6, str6);
      FileTypeUtils.sourceTree = dictionary2;
      Dictionary<string, PBXSourceTree> dictionary3 = new Dictionary<string, PBXSourceTree>();
      string key33 = "<absolute>";
      int num7 = 0;
      dictionary3.Add(key33, (PBXSourceTree) num7);
      string key34 = "<group>";
      int num8 = 2;
      dictionary3.Add(key34, (PBXSourceTree) num8);
      string key35 = "BUILT_PRODUCTS_DIR";
      int num9 = 3;
      dictionary3.Add(key35, (PBXSourceTree) num9);
      string key36 = "DEVELOPER_DIR";
      int num10 = 4;
      dictionary3.Add(key36, (PBXSourceTree) num10);
      string key37 = "SDKROOT";
      int num11 = 5;
      dictionary3.Add(key37, (PBXSourceTree) num11);
      string key38 = "SOURCE_ROOT";
      int num12 = 1;
      dictionary3.Add(key38, (PBXSourceTree) num12);
      FileTypeUtils.stringToSourceTreeMap = dictionary3;
    }

    public static string TrimExtension(string ext)
    {
      string str = ext;
      char[] chArray = new char[1];
      int index = 0;
      int num = 46;
      chArray[index] = (char) num;
      return str.TrimStart(chArray);
    }

    public static bool IsKnownExtension(string ext)
    {
      ext = FileTypeUtils.TrimExtension(ext);
      return FileTypeUtils.types.ContainsKey(ext);
    }

    internal static bool IsFileTypeExplicit(string ext)
    {
      ext = FileTypeUtils.TrimExtension(ext);
      if (FileTypeUtils.types.ContainsKey(ext))
        return FileTypeUtils.types[ext].isExplicit;
      return false;
    }

    public static PBXFileType GetFileType(string ext, bool isFolderRef)
    {
      ext = FileTypeUtils.TrimExtension(ext);
      if (isFolderRef || !FileTypeUtils.types.ContainsKey(ext))
        return PBXFileType.Resource;
      return FileTypeUtils.types[ext].type;
    }

    public static string GetTypeName(string ext)
    {
      ext = FileTypeUtils.TrimExtension(ext);
      if (FileTypeUtils.types.ContainsKey(ext))
        return FileTypeUtils.types[ext].name;
      return "file";
    }

    public static bool IsBuildableFile(string ext)
    {
      ext = FileTypeUtils.TrimExtension(ext);
      return !FileTypeUtils.types.ContainsKey(ext) || (uint) FileTypeUtils.types[ext].type > 0U;
    }

    public static bool IsBuildable(string ext, bool isFolderReference)
    {
      ext = FileTypeUtils.TrimExtension(ext);
      if (isFolderReference)
        return true;
      return FileTypeUtils.IsBuildableFile(ext);
    }

    internal static string SourceTreeDesc(PBXSourceTree tree)
    {
      return FileTypeUtils.sourceTree[tree];
    }

    internal static PBXSourceTree ParseSourceTree(string tree)
    {
      if (FileTypeUtils.stringToSourceTreeMap.ContainsKey(tree))
        return FileTypeUtils.stringToSourceTreeMap[tree];
      return PBXSourceTree.Source;
    }

    internal static List<PBXSourceTree> AllAbsoluteSourceTrees()
    {
      List<PBXSourceTree> list = new List<PBXSourceTree>();
      int num1 = 0;
      list.Add((PBXSourceTree) num1);
      int num2 = 3;
      list.Add((PBXSourceTree) num2);
      int num3 = 4;
      list.Add((PBXSourceTree) num3);
      int num4 = 5;
      list.Add((PBXSourceTree) num4);
      int num5 = 1;
      list.Add((PBXSourceTree) num5);
      return list;
    }

    internal class FileTypeDesc
    {
      public string name;
      public PBXFileType type;
      public bool isExplicit;

      public FileTypeDesc(string typeName, PBXFileType type)
      {
        this.name = typeName;
        this.type = type;
        this.isExplicit = false;
      }

      public FileTypeDesc(string typeName, PBXFileType type, bool isExplicit)
      {
        this.name = typeName;
        this.type = type;
        this.isExplicit = isExplicit;
      }
    }
  }
}
