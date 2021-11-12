// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.Serializer
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class Serializer
  {
    private static string k_Indent = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";

    public static PBXElementDict ParseTreeAST(TreeAST ast, TokenList tokens, string text)
    {
      PBXElementDict pbxElementDict = new PBXElementDict();
      foreach (KeyValueAST keyValueAst in ast.values)
      {
        PBXElementString pbxElementString = Serializer.ParseIdentifierAST(keyValueAst.key, tokens, text);
        PBXElement pbxElement = Serializer.ParseValueAST(keyValueAst.value, tokens, text);
        pbxElementDict[pbxElementString.value] = pbxElement;
      }
      return pbxElementDict;
    }

    public static PBXElementArray ParseArrayAST(ArrayAST ast, TokenList tokens, string text)
    {
      PBXElementArray pbxElementArray = new PBXElementArray();
      foreach (ValueAST ast1 in ast.values)
        pbxElementArray.values.Add(Serializer.ParseValueAST(ast1, tokens, text));
      return pbxElementArray;
    }

    public static PBXElement ParseValueAST(ValueAST ast, TokenList tokens, string text)
    {
      if (ast is TreeAST)
        return (PBXElement) Serializer.ParseTreeAST((TreeAST) ast, tokens, text);
      if (ast is ArrayAST)
        return (PBXElement) Serializer.ParseArrayAST((ArrayAST) ast, tokens, text);
      if (ast is IdentifierAST)
        return (PBXElement) Serializer.ParseIdentifierAST((IdentifierAST) ast, tokens, text);
      return (PBXElement) null;
    }

    public static PBXElementString ParseIdentifierAST(IdentifierAST ast, TokenList tokens, string text)
    {
      Token token = tokens[ast.value];
      switch (token.type)
      {
        case TokenType.String:
          return new PBXElementString(text.Substring(token.begin, token.end - token.begin));
        case TokenType.QuotedString:
          return new PBXElementString(PBXStream.UnquoteString(text.Substring(token.begin, token.end - token.begin)));
        default:
          throw new Exception("Internal parser error");
      }
    }

    private static string GetIndent(int indent)
    {
      return Serializer.k_Indent.Substring(0, indent);
    }

    private static void WriteStringImpl(StringBuilder sb, string s, bool comment, GUIDToCommentMap comments)
    {
      if (comment)
        comments.WriteStringBuilder(sb, s);
      else
        sb.Append(PBXStream.QuoteStringIfNeeded(s));
    }

    public static void WriteDictKeyValue(StringBuilder sb, string key, PBXElement value, int indent, bool compact, PropertyCommentChecker checker, GUIDToCommentMap comments)
    {
      if (!compact)
      {
        sb.Append("\n");
        sb.Append(Serializer.GetIndent(indent));
      }
      Serializer.WriteStringImpl(sb, key, checker.CheckKeyInDict(key), comments);
      sb.Append(" = ");
      if (value is PBXElementString)
        Serializer.WriteStringImpl(sb, value.AsString(), checker.CheckStringValueInDict(key, value.AsString()), comments);
      else if (value is PBXElementDict)
        Serializer.WriteDict(sb, value.AsDict(), indent, compact, checker.NextLevel(key), comments);
      else if (value is PBXElementArray)
        Serializer.WriteArray(sb, value.AsArray(), indent, compact, checker.NextLevel(key), comments);
      sb.Append(";");
      if (!compact)
        return;
      sb.Append(" ");
    }

    public static void WriteDict(StringBuilder sb, PBXElementDict el, int indent, bool compact, PropertyCommentChecker checker, GUIDToCommentMap comments)
    {
      sb.Append("{");
      if (el.Contains("isa"))
        Serializer.WriteDictKeyValue(sb, "isa", el["isa"], indent + 1, compact, checker, comments);
      List<string> list = new List<string>((IEnumerable<string>) el.values.Keys);
      list.Sort((IComparer<string>) StringComparer.Ordinal);
      foreach (string key in list)
      {
        if (key != "isa")
          Serializer.WriteDictKeyValue(sb, key, el[key], indent + 1, compact, checker, comments);
      }
      if (!compact)
      {
        sb.Append("\n");
        sb.Append(Serializer.GetIndent(indent));
      }
      sb.Append("}");
    }

    public static void WriteArray(StringBuilder sb, PBXElementArray el, int indent, bool compact, PropertyCommentChecker checker, GUIDToCommentMap comments)
    {
      sb.Append("(");
      foreach (PBXElement pbxElement in el.values)
      {
        if (!compact)
        {
          sb.Append("\n");
          sb.Append(Serializer.GetIndent(indent + 1));
        }
        if (pbxElement is PBXElementString)
          Serializer.WriteStringImpl(sb, pbxElement.AsString(), checker.CheckStringValueInArray(pbxElement.AsString()), comments);
        else if (pbxElement is PBXElementDict)
          Serializer.WriteDict(sb, pbxElement.AsDict(), indent + 1, compact, checker.NextLevel("*"), comments);
        else if (pbxElement is PBXElementArray)
          Serializer.WriteArray(sb, pbxElement.AsArray(), indent + 1, compact, checker.NextLevel("*"), comments);
        sb.Append(",");
        if (compact)
          sb.Append(" ");
      }
      if (!compact)
      {
        sb.Append("\n");
        sb.Append(Serializer.GetIndent(indent));
      }
      sb.Append(")");
    }
  }
}
