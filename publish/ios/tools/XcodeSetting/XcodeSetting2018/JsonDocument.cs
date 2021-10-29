// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.JsonDocument
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnityEditor.iOS.Xcode
{
  internal class JsonDocument
  {
    public string indentString = "  ";
    public JsonElementDict root;

    public JsonDocument()
    {
      this.root = new JsonElementDict();
    }

    private void AppendIndent(StringBuilder sb, int indent)
    {
      for (int index = 0; index < indent; ++index)
        sb.Append(this.indentString);
    }

    private void WriteString(StringBuilder sb, string str)
    {
      sb.Append('"');
      sb.Append(str);
      sb.Append('"');
    }

    private void WriteBoolean(StringBuilder sb, bool value)
    {
      sb.Append(!value ? "false" : "true");
    }

    private void WriteInteger(StringBuilder sb, int value)
    {
      sb.Append(value.ToString());
    }

    private void WriteDictKeyValue(StringBuilder sb, string key, JsonElement value, int indent)
    {
      sb.Append("\n");
      this.AppendIndent(sb, indent);
      this.WriteString(sb, key);
      sb.Append(" : ");
      if (value is JsonElementString)
        this.WriteString(sb, value.AsString());
      else if (value is JsonElementInteger)
        this.WriteInteger(sb, value.AsInteger());
      else if (value is JsonElementBoolean)
        this.WriteBoolean(sb, value.AsBoolean());
      else if (value is JsonElementDict)
      {
        this.WriteDict(sb, value.AsDict(), indent);
      }
      else
      {
        if (!(value is JsonElementArray))
          return;
        this.WriteArray(sb, value.AsArray(), indent);
      }
    }

    private void WriteDict(StringBuilder sb, JsonElementDict el, int indent)
    {
      sb.Append("{");
      bool flag = false;
      foreach (string key in (IEnumerable<string>) el.values.Keys)
      {
        if (flag)
          sb.Append(",");
        this.WriteDictKeyValue(sb, key, el[key], indent + 1);
        flag = true;
      }
      sb.Append("\n");
      this.AppendIndent(sb, indent);
      sb.Append("}");
    }

    private void WriteArray(StringBuilder sb, JsonElementArray el, int indent)
    {
      sb.Append("[");
      bool flag = false;
      foreach (JsonElement jsonElement in el.values)
      {
        if (flag)
          sb.Append(",");
        sb.Append("\n");
        this.AppendIndent(sb, indent + 1);
        if (jsonElement is JsonElementString)
          this.WriteString(sb, jsonElement.AsString());
        else if (jsonElement is JsonElementInteger)
          this.WriteInteger(sb, jsonElement.AsInteger());
        else if (jsonElement is JsonElementBoolean)
          this.WriteBoolean(sb, jsonElement.AsBoolean());
        else if (jsonElement is JsonElementDict)
          this.WriteDict(sb, jsonElement.AsDict(), indent + 1);
        else if (jsonElement is JsonElementArray)
          this.WriteArray(sb, jsonElement.AsArray(), indent + 1);
        flag = true;
      }
      sb.Append("\n");
      this.AppendIndent(sb, indent);
      sb.Append("]");
    }

    public void WriteToFile(string path)
    {
      File.WriteAllText(path, this.WriteToString());
    }

    public void WriteToStream(TextWriter tw)
    {
      tw.Write(this.WriteToString());
    }

    public string WriteToString()
    {
      StringBuilder sb = new StringBuilder();
      this.WriteDict(sb, this.root, 0);
      return sb.ToString();
    }
  }
}
