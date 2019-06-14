using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;

namespace Google
{
	internal class ProjectSettings
	{
		public enum SettingsSave
		{
			ProjectOnly,
			EditorPrefs,
			BothProjectAndEditorPrefs
		}

		private static readonly string PROJECT_SETTINGS_FILE = Path.Combine("ProjectSettings", "GvhProjectSettings.xml");

		private static SortedDictionary<string, string> settings;

		private static readonly object classLock = new object();

		private static readonly Logger logger = new Logger();

		private readonly string moduleName;

		private static SortedDictionary<string, string> Settings
		{
			get
			{
				ProjectSettings.LoadIfEmpty();
				return ProjectSettings.settings;
			}
		}

		private string UseProjectSettingsName
		{
			get
			{
				return this.moduleName + "UseProjectSettings";
			}
		}

		public bool UseProjectSettings
		{
			get
			{
				return EditorPrefs.GetBool(this.UseProjectSettingsName, true);
			}
			set
			{
				EditorPrefs.SetBool(this.UseProjectSettingsName, value);
			}
		}

		public ProjectSettings(string moduleName)
		{
			this.moduleName = moduleName;
		}

		private static void LoadIfEmpty()
		{
			object obj = ProjectSettings.classLock;
			lock (obj)
			{
				if (ProjectSettings.settings == null)
				{
					ProjectSettings.Load();
				}
			}
		}

		private static void Set<T>(string name, T value)
		{
			object obj = ProjectSettings.classLock;
			lock (obj)
			{
				ProjectSettings.Settings[name] = value.ToString();
				ProjectSettings.Save();
			}
		}

		private void SavePreferences(ProjectSettings.SettingsSave saveLevel, Action saveToProject, Action saveToEditor)
		{
			switch (saveLevel)
			{
			case ProjectSettings.SettingsSave.ProjectOnly:
				saveToProject();
				return;
			case ProjectSettings.SettingsSave.EditorPrefs:
				saveToEditor();
				return;
			}
			saveToEditor();
			saveToProject();
		}

		public void SetBool(string name, bool value, ProjectSettings.SettingsSave saveLevel)
		{
			this.SavePreferences(saveLevel, delegate
			{
				ProjectSettings.Set<bool>(name, value);
			}, delegate
			{
				EditorPrefs.SetBool(name, value);
			});
		}

		public void SetBool(string name, bool value)
		{
			this.SavePreferences(ProjectSettings.SettingsSave.BothProjectAndEditorPrefs, delegate
			{
				ProjectSettings.Set<bool>(name, value);
			}, delegate
			{
				EditorPrefs.SetBool(name, value);
			});
		}

		public void SetFloat(string name, float value, ProjectSettings.SettingsSave saveLevel)
		{
			this.SavePreferences(saveLevel, delegate
			{
				ProjectSettings.Set<float>(name, value);
			}, delegate
			{
				EditorPrefs.SetFloat(name, value);
			});
		}

		public void SetFloat(string name, float value)
		{
			this.SavePreferences(ProjectSettings.SettingsSave.BothProjectAndEditorPrefs, delegate
			{
				ProjectSettings.Set<float>(name, value);
			}, delegate
			{
				EditorPrefs.SetFloat(name, value);
			});
		}

		public void SetInt(string name, int value, ProjectSettings.SettingsSave saveLevel)
		{
			this.SavePreferences(saveLevel, delegate
			{
				ProjectSettings.Set<int>(name, value);
			}, delegate
			{
				EditorPrefs.SetInt(name, value);
			});
		}

		public void SetInt(string name, int value)
		{
			this.SavePreferences(ProjectSettings.SettingsSave.BothProjectAndEditorPrefs, delegate
			{
				ProjectSettings.Set<int>(name, value);
			}, delegate
			{
				EditorPrefs.SetInt(name, value);
			});
		}

		public void SetString(string name, string value, ProjectSettings.SettingsSave saveLevel)
		{
			this.SavePreferences(saveLevel, delegate
			{
				ProjectSettings.Set<string>(name, value);
			}, delegate
			{
				EditorPrefs.SetString(name, value);
			});
		}

		public void SetString(string name, string value)
		{
			this.SavePreferences(ProjectSettings.SettingsSave.BothProjectAndEditorPrefs, delegate
			{
				ProjectSettings.Set<string>(name, value);
			}, delegate
			{
				EditorPrefs.SetString(name, value);
			});
		}

		private static string Get(string name, string defaultValue)
		{
			object obj = ProjectSettings.classLock;
			lock (obj)
			{
				string result;
				if (ProjectSettings.Settings.TryGetValue(name, out result))
				{
					return result;
				}
			}
			return defaultValue;
		}

		private static bool Get(string name, bool defaultValue)
		{
			bool result;
			if (bool.TryParse(ProjectSettings.Get(name, defaultValue.ToString()), out result))
			{
				return result;
			}
			return defaultValue;
		}

		private static float Get(string name, float defaultValue)
		{
			float result;
			if (float.TryParse(ProjectSettings.Get(name, defaultValue.ToString()), out result))
			{
				return result;
			}
			return defaultValue;
		}

		private static int Get(string name, int defaultValue)
		{
			int result;
			if (int.TryParse(ProjectSettings.Get(name, defaultValue.ToString()), out result))
			{
				return result;
			}
			return defaultValue;
		}

		public string GetString(string name, string defaultValue = "")
		{
			string @string = EditorPrefs.GetString(name, defaultValue);
			return (!this.UseProjectSettings) ? @string : ProjectSettings.Get(name, @string);
		}

		public bool GetBool(string name, bool defaultValue = false)
		{
			bool @bool = EditorPrefs.GetBool(name, defaultValue);
			return (!this.UseProjectSettings) ? @bool : ProjectSettings.Get(name, @bool);
		}

		public float GetFloat(string name, float defaultValue = 0f)
		{
			float @float = EditorPrefs.GetFloat(name, defaultValue);
			return (!this.UseProjectSettings) ? @float : ProjectSettings.Get(name, @float);
		}

		public int GetInt(string name, int defaultValue = 0)
		{
			int @int = EditorPrefs.GetInt(name, defaultValue);
			return (!this.UseProjectSettings) ? @int : ProjectSettings.Get(name, @int);
		}

		public bool HasKey(string name)
		{
			if (this.UseProjectSettings)
			{
				object obj = ProjectSettings.classLock;
				lock (obj)
				{
					string text;
					return ProjectSettings.Settings.TryGetValue(name, out text);
				}
			}
			return EditorPrefs.HasKey(name);
		}

		public void DeleteAll()
		{
			EditorPrefs.DeleteAll();
			ProjectSettings.Clear();
			ProjectSettings.Save();
		}

		public void DeleteKey(string name)
		{
			EditorPrefs.DeleteKey(name);
			object obj = ProjectSettings.classLock;
			lock (obj)
			{
				ProjectSettings.Settings.Remove(name);
				ProjectSettings.Save();
			}
		}

		internal void DeleteKeys(IEnumerable<string> names)
		{
			foreach (string current in names)
			{
				if (this.HasKey(current))
				{
					this.DeleteKey(current);
				}
			}
		}

		private static void Clear()
		{
			object obj = ProjectSettings.classLock;
			lock (obj)
			{
				ProjectSettings.settings = new SortedDictionary<string, string>();
			}
		}

		private static bool Load()
		{
			object obj = ProjectSettings.classLock;
			bool result;
			lock (obj)
			{
				ProjectSettings.Clear();
				if (!XmlUtilities.ParseXmlTextFileElements(ProjectSettings.PROJECT_SETTINGS_FILE, ProjectSettings.logger, delegate(XmlTextReader reader, string elementName, bool isStart, string parentElementName, List<string> elementNameStack)
				{
					if (elementName == "projectSettings" && parentElementName == string.Empty)
					{
						return true;
					}
					if (elementName == "projectSetting" && parentElementName == "projectSettings")
					{
						if (isStart)
						{
							string attribute = reader.GetAttribute("name");
							string attribute2 = reader.GetAttribute("value");
							if (!string.IsNullOrEmpty(attribute))
							{
								if (string.IsNullOrEmpty(attribute2))
								{
									ProjectSettings.settings.Remove(attribute);
								}
								else
								{
									ProjectSettings.settings[attribute] = attribute2;
								}
							}
						}
						return true;
					}
					return false;
				}))
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		private static void Save()
		{
			object obj = ProjectSettings.classLock;
			lock (obj)
			{
				if (ProjectSettings.settings != null)
				{
					Directory.CreateDirectory(Path.GetDirectoryName(ProjectSettings.PROJECT_SETTINGS_FILE));
					using (XmlTextWriter xmlTextWriter = new XmlTextWriter(new StreamWriter(ProjectSettings.PROJECT_SETTINGS_FILE))
					{
						Formatting = Formatting.Indented
					})
					{
						xmlTextWriter.WriteStartElement("projectSettings");
						foreach (KeyValuePair<string, string> current in ProjectSettings.settings)
						{
							xmlTextWriter.WriteStartElement("projectSetting");
							if (!string.IsNullOrEmpty(current.Key) && !string.IsNullOrEmpty(current.Value))
							{
								xmlTextWriter.WriteAttributeString("name", current.Key);
								xmlTextWriter.WriteAttributeString("value", current.Value);
							}
							xmlTextWriter.WriteEndElement();
						}
						xmlTextWriter.WriteEndElement();
					}
				}
			}
		}
	}
}
