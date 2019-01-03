using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
#if UNITY_XCODE_API_BUILD
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#else
using UnityEditor.iOS.Xcode.Custom;
using UnityEditor.iOS.Xcode.Custom.Extensions;
#endif

namespace UnityEditor.iOS.Xcode.Tests
{
    public class PlistParserTests
    {

        static string GetPlistDataFromContents(string contents)
        {
            string template = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
{0}
</plist>
";
            return string.Format(template, contents);
        }

        [Test]
        public void CanReadDateKey()
        {
            var doc = new PlistDocument();
            doc.ReadFromString(GetPlistDataFromContents(@"
<dict>
    <key>test</key>
    <date>2009-01-02T10:11:12Z</date>
</dict>
"));
            Assert.IsTrue(doc.root.values.ContainsKey("test"));
            var date = doc.root["test"].AsDate();
            Assert.AreEqual(new System.DateTime(2009, 1, 2, 10, 11, 12, System.DateTimeKind.Utc), date);
        }

        [Test]
        public void CanWriteDateKey()
        {
            var doc = new PlistDocument();
            doc.Create();
            doc.root.SetDate("test", new System.DateTime(2009, 1, 2, 10, 11, 12, System.DateTimeKind.Utc));
            string expected = GetPlistDataFromContents(
@"  <dict>
    <key>test</key>
    <date>2009-01-02T10:11:12Z</date>
  </dict>");
            Assert.AreEqual(expected, doc.WriteToString());
        }

        [Test]
        public void CanReadRealKey()
        {
            var doc = new PlistDocument();
            doc.ReadFromString(GetPlistDataFromContents(@"
<dict>
    <key>test</key>
    <real>12.1234</real>
</dict>
"));
            Assert.IsTrue(doc.root.values.ContainsKey("test"));
            var date = doc.root["test"].AsReal();
            Assert.AreEqual(12.1234f, date);
        }

        [Test]
        public void CanWriteRealKey()
        {
            var doc = new PlistDocument();
            doc.Create();
            doc.root.SetReal("test", 12.1234f);
            string expected = GetPlistDataFromContents(
@"  <dict>
    <key>test</key>
    <real>12.1234</real>
  </dict>");
            Assert.AreEqual(expected, doc.WriteToString());
        }
    }
}
