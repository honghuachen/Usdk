// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.Parser
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class Parser
  {
    private TokenList tokens;
    private int currPos;

    public Parser(TokenList tokens)
    {
      this.tokens = tokens;
      this.currPos = this.SkipComments(0);
    }

    private int SkipComments(int pos)
    {
      while (pos < this.tokens.Count && this.tokens[pos].type == TokenType.Comment)
        ++pos;
      return pos;
    }

    private int IncInternal(int pos)
    {
      if (pos >= this.tokens.Count)
        return pos;
      ++pos;
      return this.SkipComments(pos);
    }

    private int Inc()
    {
      int num = this.currPos;
      this.currPos = this.IncInternal(this.currPos);
      return num;
    }

    private TokenType Tok()
    {
      if (this.currPos >= this.tokens.Count)
        return TokenType.EOF;
      return this.tokens[this.currPos].type;
    }

    private void SkipIf(TokenType type)
    {
      if (this.Tok() != type)
        return;
      this.Inc();
    }

    private string GetErrorMsg()
    {
      return "Invalid PBX project (parsing line " + (object) this.tokens[this.currPos].line + ")";
    }

    public IdentifierAST ParseIdentifier()
    {
      if (this.Tok() != TokenType.String && this.Tok() != TokenType.QuotedString)
        throw new Exception(this.GetErrorMsg());
      return new IdentifierAST()
      {
        value = this.Inc()
      };
    }

    public TreeAST ParseTree()
    {
      if (this.Tok() != TokenType.LBrace)
        throw new Exception(this.GetErrorMsg());
      this.Inc();
      TreeAST treeAst = new TreeAST();
      while (this.Tok() != TokenType.RBrace && (uint) this.Tok() > 0U)
        treeAst.values.Add(this.ParseKeyValue());
      this.SkipIf(TokenType.RBrace);
      return treeAst;
    }

    public ArrayAST ParseList()
    {
      if (this.Tok() != TokenType.LParen)
        throw new Exception(this.GetErrorMsg());
      this.Inc();
      ArrayAST arrayAst = new ArrayAST();
      while (this.Tok() != TokenType.RParen && (uint) this.Tok() > 0U)
      {
        arrayAst.values.Add(this.ParseValue());
        this.SkipIf(TokenType.Comma);
      }
      this.SkipIf(TokenType.RParen);
      return arrayAst;
    }

    public KeyValueAST ParseKeyValue()
    {
      KeyValueAST keyValueAst = new KeyValueAST();
      keyValueAst.key = this.ParseIdentifier();
      if (this.Tok() != TokenType.Eq)
        throw new Exception(this.GetErrorMsg());
      this.Inc();
      keyValueAst.value = this.ParseValue();
      this.SkipIf(TokenType.Semicolon);
      return keyValueAst;
    }

    public ValueAST ParseValue()
    {
      if (this.Tok() == TokenType.String || this.Tok() == TokenType.QuotedString)
        return (ValueAST) this.ParseIdentifier();
      if (this.Tok() == TokenType.LBrace)
        return (ValueAST) this.ParseTree();
      if (this.Tok() == TokenType.LParen)
        return (ValueAST) this.ParseList();
      throw new Exception(this.GetErrorMsg());
    }
  }
}
