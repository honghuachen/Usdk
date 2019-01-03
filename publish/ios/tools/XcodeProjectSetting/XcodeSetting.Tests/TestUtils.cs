using NUnit.Framework;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace UnityEditor.iOS.Xcode.Tests
{
    public class GenericTester
    {
        string m_Source, m_Output;
        bool m_Debug;
        string m_TestsRoot;

        protected GenericTester(string source, string output, bool debug)
        {
            m_Source = source;
            m_Output = output;
            m_Debug = debug;
            m_TestsRoot = ResolveTestsRoot();
        }

        static string ResolveTestsRoot()
        {
            string path = Directory.GetCurrentDirectory();
            string testsRelPath = "Xcode.Tests";
            for (int i = 0; i < 5; i++)
            {
                string testsPath = Path.Combine(path, testsRelPath);
                if (Directory.Exists(testsPath))
                    return testsPath;
                path = Path.Combine(path, "..");
            }
            Assert.Fail("Could not find root tests directory");
            return null;
        }

        protected string GetTestOutputPath()
        {
            string path = Path.Combine(m_TestsRoot, m_Output);
            Directory.CreateDirectory(path);
            return path;
        }

        protected string GetTestSourcePath()
        {
            return Path.Combine(m_TestsRoot, m_Source);
        }

        protected bool DebugEnabled()
        {
            return m_Debug;
        }
    }

    public class TextTester : GenericTester
    {

        protected TextTester(string source, string output, bool debug) :
            base(source, output, debug)
        {
        }

        protected string ReadSourceFile(string filename)
        {
            return File.ReadAllText(Path.Combine(GetTestSourcePath(), filename));
        }

        protected void WriteResultFile(string filename, string contents)
        {
            if (DebugEnabled())
                File.WriteAllText(Path.Combine(GetTestOutputPath(), filename), contents);
        }

        protected void TestTextUpdate(string inputFile, string expectedFile,
                                      Func<string, string> contentsUpdater)
        {
            string text = inputFile != null ? ReadSourceFile(inputFile) : null;
            string expectedText = ReadSourceFile(expectedFile);

            text = contentsUpdater(text);

            WriteResultFile(expectedFile, text);
            Assert.AreEqual(expectedText, text);
        }

        protected void TestXmlUpdate(string inputFile, string expectedFile,
                                     Func<string, string> contentsUpdater)
        {
            string text = ReadSourceFile(inputFile);
            string expectedText = ReadSourceFile(expectedFile);

            text = contentsUpdater(text);
            WriteResultFile(expectedFile, text);

            XElement tree = XElement.Parse(text);
            XElement expectedTree = XElement.Parse(expectedText);

            Assert.IsTrue(TestUtils.CompareNormalizedXml(tree, expectedTree), "Xml trees are not equal");
        }
    }

    public class TestUtils
    {
        internal static XElement Normalize(XElement el)
        {
            if (el.HasElements)
            {
                return new XElement(el.Name,
                                    el.Attributes().OrderBy(a => a.Name.ToString()),
                                    el.Elements().Select(e => Normalize(e)));
            }

            if (el.IsEmpty || el.Value.Trim().Length == 0)
            {
                return new XElement(el.Name,
                                    el.Attributes().OrderBy(a => a.Name.ToString()));
            }

            return new XElement(el.Name,
                                el.Attributes().OrderBy(a => a.Name.ToString()),
                                el.Value.Trim());
        }

        internal static bool CompareNormalizedXml(XElement lhs, XElement rhs)
        {
            return XElement.DeepEquals(Normalize(lhs), Normalize(rhs));
        }

        /// Compares the contents of two files for equality
        internal static bool FileContentsEqual (string path1, string path2)
        {
            // firstly, check the file sizes
            FileInfo info1 = new FileInfo (path1);
            FileInfo info2 = new FileInfo (path2);
            if (!info1.Exists || !info2.Exists) {
                return false;
            }
            if (info1.Length != info2.Length) {
                return false;
            }

            // compare the contents
            using (var stream1 = new FileStream (path1, FileMode.Open)) {
                using (var stream2 = new FileStream (path2, FileMode.Open)) {
                    const int bufferSize = 4096;
                    byte [] buffer1 = new byte [bufferSize];
                    byte [] buffer2 = new byte [bufferSize];
                    while (true) {
                        int count1 = stream1.Read (buffer1, 0, bufferSize);
                        int count2 = stream2.Read (buffer2, 0, bufferSize);

                        if (count1 != count2)
                            return false;

                        if (count1 == 0)
                            return true;

                        // You might replace the following with an efficient "memcmp"
                        if (!buffer1.Take (count1).SequenceEqual (buffer2.Take (count2)))
                            return false;
                    }
                }
            }
        }
    }

    public class TestFileCache
    {
        string m_Directory;

        public TestFileCache(string directory)
        {
            m_Directory = directory;
            Directory.CreateDirectory(m_Directory);
        }

        public string CreateFile(string filename, string contents)
        {
            string path = Path.Combine(m_Directory, filename);
            File.WriteAllText(path, contents);
            return path;
        }

        public void CleanUp()
        {
            Directory.Delete(m_Directory, true);
        }
    }

}
