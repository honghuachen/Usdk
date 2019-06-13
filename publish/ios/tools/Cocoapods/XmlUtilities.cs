using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Google
{
	internal class XmlUtilities
	{
		internal delegate bool ParseElement(XmlTextReader reader, string elementName, bool isStart, string parentElementName, List<string> elementNameStack);

		private class Reader
		{
			private XmlTextReader reader;

			private int lineNumber = -1;

			private int linePosition = -1;

			public bool Reading
			{
				get;
				private set;
			}

			public bool XmlReaderIsAhead
			{
				get
				{
					return this.lineNumber != this.reader.LineNumber || this.linePosition != this.reader.LinePosition;
				}
			}

			public Reader(XmlTextReader xmlReader)
			{
				this.reader = xmlReader;
				this.Reading = this.reader.Read();
				this.lineNumber = this.reader.LineNumber;
				this.linePosition = this.reader.LinePosition;
			}

			public bool Read()
			{
				bool result = false;
				if (this.Reading && !this.XmlReaderIsAhead)
				{
					this.Reading = this.reader.Read();
					result = true;
				}
				this.lineNumber = this.reader.LineNumber;
				this.linePosition = this.reader.LinePosition;
				return result;
			}
		}

		internal static bool ParseXmlTextFileElements(string filename, Logger logger, XmlUtilities.ParseElement parseElement)
		{
			if (!File.Exists(filename))
			{
				return false;
			}
			try
			{
				using (XmlTextReader xmlTextReader = new XmlTextReader(new StreamReader(filename)))
				{
					List<string> elementNameStack = new List<string>();
					Func<string> func = () => (elementNameStack.Count <= 0) ? string.Empty : elementNameStack[0];
					XmlUtilities.Reader reader = new XmlUtilities.Reader(xmlTextReader);
					while (reader.Reading)
					{
						string name = xmlTextReader.Name;
						string text = func();
						if (xmlTextReader.NodeType == XmlNodeType.Element)
						{
							if (parseElement(xmlTextReader, name, true, text, elementNameStack))
							{
								elementNameStack.Insert(0, name);
							}
							if (reader.XmlReaderIsAhead)
							{
								reader.Read();
								continue;
							}
						}
						if ((xmlTextReader.NodeType == XmlNodeType.EndElement || (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.IsEmptyElement)) && !string.IsNullOrEmpty(text))
						{
							if (elementNameStack[0] == name)
							{
								elementNameStack.RemoveAt(0);
							}
							else
							{
								elementNameStack.Clear();
							}
							parseElement(xmlTextReader, name, false, func(), elementNameStack);
						}
						reader.Read();
					}
				}
			}
			catch (XmlException ex)
			{
				logger.Log(string.Format("Failed while parsing XML file {0}\n{1}\n", filename, ex.ToString()), LogLevel.Error);
				return false;
			}
			return true;
		}
	}
}
