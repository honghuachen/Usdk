// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOS.Xcode.XcScheme
// Assembly: UnityEditor.iOS.Extensions.Xcode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36E9AB58-F6A7-4B4D-BAB5-BE8059694DCD
// Assembly location: D:\workspace\pk\trunk\usdk\publish\ios\tools\UnityEditor.iOS.Extensions.Xcode.dll

using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace UnityEditor.iOS.Xcode
{
  internal class XcScheme
  {
    private XDocument m_Doc;

    public string GetBuildConfiguration()
    {
      XElement xelement = System.Xml.XPath.Extensions.XPathSelectElement((XNode) this.m_Doc.Root, "./LaunchAction");
      if (xelement == null)
        throw new Exception("The xcscheme document does not contain build configuration setting");
      XAttribute xattribute = xelement.Attribute((XName) "buildConfiguration");
      if (xattribute == null)
        return (string) null;
      return xattribute.Value;
    }

    public void SetBuildConfiguration(string buildConfigName)
    {
      XElement xelement = System.Xml.XPath.Extensions.XPathSelectElement((XNode) this.m_Doc.Root, "./LaunchAction");
      if (xelement == null)
        throw new Exception("The xcscheme document does not contain build configuration setting");
      xelement.SetAttributeValue((XName) "buildConfiguration", (object) buildConfigName);
    }

    public void AddArgumentPassedOnLaunch(string argument)
    {
      XElement xelement1 = System.Xml.XPath.Extensions.XPathSelectElement((XNode) this.m_Doc.Root, "./LaunchAction");
      if (xelement1 == null)
        throw new Exception("The xcscheme document does not contain build configuration setting");
      XElement xelement2 = System.Xml.XPath.Extensions.XPathSelectElement((XNode) xelement1, "./CommandLineArguments");
      if (xelement2 == null)
      {
        xelement1.Add((object) new XElement((XName) "CommandLineArguments"));
        xelement2 = System.Xml.XPath.Extensions.XPathSelectElement((XNode) xelement1, "./CommandLineArguments");
      }
      XElement xelement3 = xelement2;
      XName name = (XName) "CommandLineArgument";
      object[] objArray = new object[2];
      int index1 = 0;
      XAttribute xattribute1 = new XAttribute((XName) "argument", (object) argument);
      objArray[index1] = (object) xattribute1;
      int index2 = 1;
      XAttribute xattribute2 = new XAttribute((XName) "isEnabled", (object) "YES");
      objArray[index2] = (object) xattribute2;
      XElement xelement4 = new XElement(name, objArray);
      xelement3.Add((object) xelement4);
    }

    public void SetDebugExecutable(bool enabled)
    {
      XElement xelement = System.Xml.XPath.Extensions.XPathSelectElement((XNode) this.m_Doc.Root, "./LaunchAction");
      if (xelement == null)
        throw new Exception("The xcscheme document does not contain build configuration setting");
      xelement.Attribute((XName) "selectedDebuggerIdentifier").Value = !enabled ? "" : "Xcode.DebuggerFoundation.Debugger.LLDB";
      xelement.Attribute((XName) "selectedLauncherIdentifier").Value = !enabled ? "Xcode.IDEFoundation.Launcher.PosixSpawn" : "Xcode.DebuggerFoundation.Launcher.LLDB";
    }

    public void ReadFromFile(string path)
    {
      this.ReadFromString(File.ReadAllText(path));
    }

    public void ReadFromStream(TextReader tr)
    {
      this.ReadFromString(tr.ReadToEnd());
    }

    public void ReadFromString(string text)
    {
      this.m_Doc = PlistDocument.ParseXmlNoDtd(text);
    }

    public void WriteToFile(string path)
    {
      Encoding encoding = (Encoding) new UTF8Encoding(false);
      File.WriteAllText(path, this.WriteToString(), encoding);
    }

    public void WriteToStream(TextWriter tw)
    {
      tw.Write(this.WriteToString());
    }

    public string WriteToString()
    {
      return PlistDocument.CleanDtdToString(this.m_Doc, (XDocumentType) null).Replace("\r\n", "\n");
    }
  }
}
