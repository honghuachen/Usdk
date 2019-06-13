using Google;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;

namespace GooglePlayServices
{
	internal class XmlDependencies
	{
		internal HashSet<Regex> fileRegularExpressions = new HashSet<Regex>
		{
			new Regex(".*[/\\\\]Editor[/\\\\].*Dependencies\\.xml$")
		};

		protected string dependencyType = "dependencies";

		private List<string> FindFiles()
		{
			List<string> list = new List<string>();
			string[] array = AssetDatabase.FindAssets("t:TextAsset");
			for (int i = 0; i < array.Length; i++)
			{
				string guid = array[i];
				string text = AssetDatabase.GUIDToAssetPath(guid);
				foreach (Regex current in this.fileRegularExpressions)
				{
					if (current.Match(text).Success)
					{
						list.Add(text);
					}
				}
			}
			return list;
		}

		protected virtual bool Read(string filename, Logger logger)
		{
			return false;
		}

		public virtual bool ReadAll(Logger logger)
		{
			bool result = true;
			foreach (string current in this.FindFiles())
			{
				if (!this.Read(current, logger))
				{
					logger.Log(string.Format("Unable to read {0} from {1}.\n{0} in this file will be ignored.", this.dependencyType, current), LogLevel.Error);
					result = false;
				}
			}
			return result;
		}
	}
}
