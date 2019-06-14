using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

#if UNITY_XCODE_API_BUILD
namespace UnityEditor.iOS.Xcode
#else
namespace UnityEditor.iOS.Xcode.Custom
#endif
{

    public class PlistElement
    {
        protected PlistElement() {}

        // convenience methods
        public string AsString() { return ((PlistElementString)this).value; }
        public int AsInteger()   { return ((PlistElementInteger)this).value; }
        public bool AsBoolean()  { return ((PlistElementBoolean)this).value; }
        public PlistElementArray AsArray() { return (PlistElementArray)this; }
        public PlistElementDict AsDict()   { return (PlistElementDict)this; }
        public float AsReal() { return ((PlistElementReal)this).value; }
        public DateTime AsDate() { return ((PlistElementDate)this).value; }

        public PlistElement this[string key]
        {
            get { return AsDict()[key]; }
            set { AsDict()[key] = value; }
        }
    }

    public class PlistElementString : PlistElement
    {
        public PlistElementString(string v) { value = v; }

        public string value;
    }

    public class PlistElementInteger : PlistElement
    {
        public PlistElementInteger(int v) { value = v; }

        public int value;
    }

    public class PlistElementReal : PlistElement
    {
        public PlistElementReal(float v) { value = v; }

        public float value;
    }

    public class PlistElementBoolean : PlistElement
    {
        public PlistElementBoolean(bool v) { value = v; }

        public bool value;
    }

    public class PlistElementDate : PlistElement
    {
        public PlistElementDate(DateTime date) { value = date; }

        public DateTime value;
    }

    public class PlistElementDict : PlistElement
    {
        public PlistElementDict() : base() {}

        private SortedDictionary<string, PlistElement> m_PrivateValue = new SortedDictionary<string, PlistElement>();
        public IDictionary<string, PlistElement> values { get { return m_PrivateValue; }}

        new public PlistElement this[string key]
        {
            get {
                if (values.ContainsKey(key))
                    return values[key];
                return null;
            }
            set { this.values[key] = value; }
        }


        // convenience methods
        public void SetInteger(string key, int val)
        {
            values[key] = new PlistElementInteger(val);
        }

        public void SetString(string key, string val)
        {
            values[key] = new PlistElementString(val);
        }

        public void SetBoolean(string key, bool val)
        {
            values[key] = new PlistElementBoolean(val);
        }

        public void SetDate(string key, DateTime val)
        {
            values[key] = new PlistElementDate(val);
        }

        public void SetReal(string key, float val)
        {
            values[key] = new PlistElementReal(val);
        }

        public PlistElementArray CreateArray(string key)
        {
            var v = new PlistElementArray();
            values[key] = v;
            return v;
        }

        public PlistElementDict CreateDict(string key)
        {
            var v = new PlistElementDict();
            values[key] = v;
            return v;
        }
    }

    public class PlistElementArray : PlistElement
    {
        public PlistElementArray() : base() {}
        public List<PlistElement> values = new List<PlistElement>();

        // convenience methods
        public void AddString(string val)
        {
            values.Add(new PlistElementString(val));
        }

        public void AddInteger(int val)
        {
            values.Add(new PlistElementInteger(val));
        }

        public void AddBoolean(bool val)
        {
            values.Add(new PlistElementBoolean(val));
        }

        public void AddDate(DateTime val)
        {
            values.Add(new PlistElementDate(val));
        }

        public void AddReal(float val)
        {
            values.Add(new PlistElementReal(val));
        }

        public PlistElementArray AddArray()
        {
            var v = new PlistElementArray();
            values.Add(v);
            return v;
        }

        public PlistElementDict AddDict()
        {
            var v = new PlistElementDict();
            values.Add(v);
            return v;
        }
    }

    public class PlistDocument
    {
        public PlistElementDict root;
        public string version;

        private XDocumentType documentType;

        public PlistDocument()
        {
            root = new PlistElementDict();
            version = "1.0";
        }

        // Parses a string that contains a XML file. No validation is done.
        internal static XDocument ParseXmlNoDtd(string text)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;
            settings.XmlResolver = null; // prevent DTD download

            XmlReader xmlReader = XmlReader.Create(new StringReader(text), settings);
            return XDocument.Load(xmlReader);
        }

        // LINQ serializes XML DTD declaration with an explicit empty 'internal subset'
        // (a pair of square brackets at the end of Doctype declaration).
        // Even though this is valid XML, XCode does not like it, hence this workaround.
        internal static string CleanDtdToString(XDocument doc, XDocumentType documentType)
        {
            // LINQ does not support changing the DTD of existing XDocument instances,
            // so we create a dummy document for printing of the Doctype declaration.
            // A single dummy element is added to force LINQ not to omit the declaration.
            // Also, utf-8 encoding is forced since this is the encoding we use when writing to file in UpdateInfoPlist.
            if (documentType != null)
            {
                XDocument tmpDoc =
                    new XDocument(new XDeclaration("1.0", "utf-8", null),
                                  new XDocumentType(documentType.Name, documentType.PublicId, documentType.SystemId, null),
                                  new XElement(doc.Root.Name));
                return "" + tmpDoc.Declaration + "\n" + tmpDoc.DocumentType + "\n" + doc.Root + "\n";
            }
            else
            {
                XDocument tmpDoc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement(doc.Root.Name));
                return "" + tmpDoc.Declaration + Environment.NewLine + doc.Root + "\n";
            }
        }

        internal static string CleanDtdToString(XDocument doc)
        {
            return CleanDtdToString(doc, doc.DocumentType);
        }

        private static string GetText(XElement xml)
        {
            return String.Join("", xml.Nodes().OfType<XText>().Select(x => x.Value).ToArray());
        }

        private static PlistElement ReadElement(XElement xml)
        {
            switch (xml.Name.LocalName)
            {
                case "dict":
                {
                    List<XElement> children = xml.Elements().ToList();
                    var el = new PlistElementDict();

                    if (children.Count % 2 == 1)
                        throw new Exception("Malformed plist file");

                    for (int i = 0; i < children.Count - 1; i++)
                    {
                        if (children[i].Name != "key")
                            throw new Exception("Malformed plist file. Found '"+children[i].Name+"' where 'key' was expected.");
                        string key = GetText(children[i]).Trim();
                        var newChild = ReadElement(children[i+1]);
                        if (newChild != null)
                        {
                            i++;
                            el[key] = newChild;
                        }
                    }
                    return el;
                }
                case "array":
                {
                    List<XElement> children = xml.Elements().ToList();
                    var el = new PlistElementArray();

                    foreach (var childXml in children)
                    {
                        var newChild = ReadElement(childXml);
                        if (newChild != null)
                            el.values.Add(newChild);
                    }
                    return el;
                }
                case "string":
                    return new PlistElementString(GetText(xml));
                case "integer":
                {
                    int r;
                    if (int.TryParse(GetText(xml), out r))
                        return new PlistElementInteger(r);
                    return null;
                }
                case "real":
                {
                    float f;
                    if (float.TryParse(GetText(xml), out f))
                        return new PlistElementReal(f);
                    return null;
                }
                case "date":
                {
                    DateTime date;
                    if (DateTime.TryParse(GetText(xml), out date))
                        return new PlistElementDate(date.ToUniversalTime());
                    return null;
                }
                case "true":
                    return new PlistElementBoolean(true);
                case "false":
                    return new PlistElementBoolean(false);
                default:
                    return null;
            }
        }

        public void Create()
        {
            const string doc = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                               "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">" +
                               "<plist version=\"1.0\">" +
                               "<dict>" +
                               "</dict>" +
                               "</plist>";
            ReadFromString(doc);
        }

        public void ReadFromFile(string path)
        {
            ReadFromString(File.ReadAllText(path));
        }

        public void ReadFromStream(TextReader tr)
        {
            ReadFromString(tr.ReadToEnd());
        }

        public void ReadFromString(string text)
        {
            XDocument doc = ParseXmlNoDtd(text);
            version = (string) doc.Root.Attribute("version");
            XElement xml = doc.XPathSelectElement("plist/dict");

            var dict = ReadElement(xml);
            if (dict == null)
                throw new Exception("Error parsing plist file");
            root = dict as PlistElementDict;
            if (root == null)
                throw new Exception("Malformed plist file");
            documentType = doc.DocumentType;
        }

        private static XElement WriteElement(PlistElement el)
        {
            if (el is PlistElementBoolean)
            {
                var realEl = el as PlistElementBoolean;
                return new XElement(realEl.value ? "true" : "false");
            }
            if (el is PlistElementInteger)
            {
                var realEl = el as PlistElementInteger;
                return new XElement("integer", realEl.value.ToString());
            }
            if (el is PlistElementString)
            {
                var realEl = el as PlistElementString;
                return new XElement("string", realEl.value);
            }
            if (el is PlistElementReal)
            {
                var realEl = el as PlistElementReal;
                return new XElement("real", realEl.value.ToString());
            }
            if (el is PlistElementDate)
            {
                var realEl = el as PlistElementDate;
                return new XElement("date", realEl.value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }
            if (el is PlistElementDict)
            {
                var realEl = el as PlistElementDict;
                var dictXml = new XElement("dict");
                foreach (var kv in realEl.values)
                {
                    var keyXml = new XElement("key", kv.Key);
                    var valueXml = WriteElement(kv.Value);
                    if (valueXml != null)
                    {
                        dictXml.Add(keyXml);
                        dictXml.Add(valueXml);
                    }
                }
                return dictXml;
            }
            if (el is PlistElementArray)
            {
                var realEl = el as PlistElementArray;
                var arrayXml = new XElement("array");
                foreach (var v in realEl.values)
                {
                    var elXml = WriteElement(v);
                    if (elXml != null)
                        arrayXml.Add(elXml);
                }
                return arrayXml;
            }
            return null;
        }

        public void WriteToFile(string path)
        {
            System.Text.Encoding utf8WithoutBom = new System.Text.UTF8Encoding(false);
            File.WriteAllText(path, WriteToString(), utf8WithoutBom);
        }

        public void WriteToStream(TextWriter tw)
        {
            tw.Write(WriteToString());
        }

        public string WriteToString()
        {
            var el = WriteElement(root);
            var rootEl = new XElement("plist");
            rootEl.Add(new XAttribute("version", version));
            rootEl.Add(el);

            var doc = new XDocument();
            doc.Add(rootEl);
            return CleanDtdToString(doc, documentType).Replace("\r\n", "\n");
        }
    }

} // namespace UnityEditor.iOS.XCode
