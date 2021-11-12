// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.PBX.Lexer
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A48C51A-69F3-4C98-8891-786C2A21B1EB
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

namespace UnityEditor.iOS.Xcode.PBX
{
  internal class Lexer
  {
    private string text;
    private int pos;
    private int length;
    private int line;

    public static TokenList Tokenize(string text)
    {
      Lexer lexer = new Lexer();
      lexer.SetText(text);
      return lexer.ScanAll();
    }

    public void SetText(string text)
    {
      this.text = text + "    ";
      this.pos = 0;
      this.length = text.Length;
      this.line = 0;
    }

    public TokenList ScanAll()
    {
      TokenList tokenList = new TokenList();
      Token tok;
      do
      {
        tok = new Token();
        this.ScanOne(tok);
        tokenList.Add(tok);
      }
      while (tok.type != TokenType.EOF);
      return tokenList;
    }

    private void UpdateNewlineStats(char ch)
    {
      if ((int) ch != 10)
        return;
      this.line = this.line + 1;
    }

    private void ScanOne(Token tok)
    {
      for (; this.pos < this.length && char.IsWhiteSpace(this.text[this.pos]); this.pos = this.pos + 1)
        this.UpdateNewlineStats(this.text[this.pos]);
      if (this.pos >= this.length)
      {
        tok.type = TokenType.EOF;
      }
      else
      {
        char ch1 = this.text[this.pos];
        char ch2 = this.text[this.pos + 1];
        if ((int) ch1 == 34)
          this.ScanQuotedString(tok);
        else if ((int) ch1 == 47 && (int) ch2 == 42)
          this.ScanMultilineComment(tok);
        else if ((int) ch1 == 47 && (int) ch2 == 47)
          this.ScanComment(tok);
        else if (this.IsOperator(ch1))
          this.ScanOperator(tok);
        else
          this.ScanString(tok);
      }
    }

    private void ScanString(Token tok)
    {
      tok.type = TokenType.String;
      tok.begin = this.pos;
      for (; this.pos < this.length; this.pos = this.pos + 1)
      {
        char ch1 = this.text[this.pos];
        char ch2 = this.text[this.pos + 1];
        if (char.IsWhiteSpace(ch1) || (int) ch1 == 34 || (int) ch1 == 47 && (int) ch2 == 42 || ((int) ch1 == 47 && (int) ch2 == 47 || this.IsOperator(ch1)))
          break;
      }
      tok.end = this.pos;
      tok.line = this.line;
    }

    private void ScanQuotedString(Token tok)
    {
      tok.type = TokenType.QuotedString;
      tok.begin = this.pos;
      this.pos = this.pos + 1;
      while (this.pos < this.length)
      {
        if ((int) this.text[this.pos] == 92 && (int) this.text[this.pos + 1] == 34)
          this.pos = this.pos + 2;
        else if ((int) this.text[this.pos] != 34)
        {
          this.UpdateNewlineStats(this.text[this.pos]);
          this.pos = this.pos + 1;
        }
        else
          break;
      }
      this.pos = this.pos + 1;
      tok.end = this.pos;
      tok.line = this.line;
    }

    private void ScanMultilineComment(Token tok)
    {
      tok.type = TokenType.Comment;
      tok.begin = this.pos;
      for (this.pos = this.pos + 2; this.pos < this.length && ((int) this.text[this.pos] != 42 || (int) this.text[this.pos + 1] != 47); this.pos = this.pos + 1)
        this.UpdateNewlineStats(this.text[this.pos]);
      this.pos = this.pos + 2;
      tok.end = this.pos;
      tok.line = this.line;
    }

    private void ScanComment(Token tok)
    {
      tok.type = TokenType.Comment;
      tok.begin = this.pos;
      this.pos = this.pos + 2;
      while (this.pos < this.length && (int) this.text[this.pos] != 10)
        this.pos = this.pos + 1;
      this.UpdateNewlineStats(this.text[this.pos]);
      this.pos = this.pos + 1;
      tok.end = this.pos;
      tok.line = this.line;
    }

    private bool IsOperator(char ch)
    {
      return (int) ch == 59 || (int) ch == 44 || ((int) ch == 61 || (int) ch == 40) || ((int) ch == 41 || (int) ch == 123) || (int) ch == 125;
    }

    private void ScanOperator(Token tok)
    {
      char ch = this.text[this.pos];
      if ((uint) ch <= 59U)
      {
        switch (ch)
        {
          case '(':
            this.ScanOperatorSpecific(tok, TokenType.LParen);
            break;
          case ')':
            this.ScanOperatorSpecific(tok, TokenType.RParen);
            break;
          case ',':
            this.ScanOperatorSpecific(tok, TokenType.Comma);
            break;
          case ';':
            this.ScanOperatorSpecific(tok, TokenType.Semicolon);
            break;
        }
      }
      else if ((int) ch != 61)
      {
        if ((int) ch != 123)
        {
          if ((int) ch != 125)
            return;
          this.ScanOperatorSpecific(tok, TokenType.RBrace);
        }
        else
          this.ScanOperatorSpecific(tok, TokenType.LBrace);
      }
      else
        this.ScanOperatorSpecific(tok, TokenType.Eq);
    }

    private void ScanOperatorSpecific(Token tok, TokenType type)
    {
      tok.type = type;
      tok.begin = this.pos;
      this.pos = this.pos + 1;
      tok.end = this.pos;
      tok.line = this.line;
    }
  }
}
