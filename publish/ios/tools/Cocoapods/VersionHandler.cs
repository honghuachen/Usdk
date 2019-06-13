using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Google
{
	[InitializeOnLoad]
	public class VersionHandler
	{
		public delegate bool FilenameFilter(string filename);

		private const string VERSION_HANDLER_ASSEMBLY_NAME = "Google.VersionHandlerImpl";

		private const string VERSION_HANDLER_IMPL_CLASS = "Google.VersionHandlerImpl";

		private static Regex VERSION_HANDLER_FILENAME_RE;

		private const string BOOT_STRAPPING_PATH = "Temp/VersionHandlerBootStrapping";

		private const string BOOT_STRAPPING_COMMAND = "BootStrapping";

		private const string CALLBACKS_PATH = "Temp/VersionHandlerCallbacks";

		private static Dictionary<string, Type> typeCache;

		private static float unityVersionMajorMinor;

		[CompilerGenerated]
		private static EditorApplication.CallbackFunction callbackFunction0;

		[CompilerGenerated]
		private static EditorApplication.CallbackFunction callbackFunction1;

		[CompilerGenerated]
		private static EditorApplication.CallbackFunction callbackFunction2;

		private static Type Impl
		{
			get
			{
				return VersionHandler.FindClass("Google.VersionHandlerImpl", "Google.VersionHandlerImpl");
			}
		}

		private static Type BootStrappedImpl
		{
			get
			{
				if (VersionHandler.Impl == null)
				{
					VersionHandler.BootStrap();
				}
				return VersionHandler.Impl;
			}
		}

		private static bool BootStrapping
		{
			get
			{
				return File.Exists("Temp/VersionHandlerBootStrapping");
			}
			set
			{
				bool bootStrapping = VersionHandler.BootStrapping;
				if (value != bootStrapping)
				{
					if (value)
					{
						VersionHandler.AddToBootStrappingFile(new List<string>
						{
							"BootStrapping"
						});
					}
					else if (bootStrapping)
					{
						VersionHandler.UpdateCompleteMethods = VersionHandler.UpdateCompleteMethodsInternal;
						HashSet<string> hashSet = new HashSet<string>();
						List<string> list = new List<string>();
						foreach (string current in VersionHandler.ReadBootStrappingFile())
						{
							if (!(current == "BootStrapping"))
							{
								if (!hashSet.Contains(current))
								{
									hashSet.Add(current);
									list.Add(current);
								}
							}
						}
						while (list.Count > 0)
						{
							string methodName = list[0];
							list.RemoveAt(0);
							File.WriteAllText("Temp/VersionHandlerBootStrapping", string.Join("\n", list.ToArray()));
							VersionHandler.InvokeImplMethod(methodName, null, null, false);
						}
						VersionHandler.UpdateCompleteMethodsInternal = null;
						File.Delete("Temp/VersionHandlerBootStrapping");
					}
				}
			}
		}

		public static bool Enabled
		{
			get
			{
				return VersionHandler.GetPropertyByName<bool>("Enabled", false);
			}
			set
			{
				VersionHandler.SetPropertyByName<bool>("Enabled", value);
			}
		}

		public static bool CleanUpPromptEnabled
		{
			get
			{
				return VersionHandler.GetPropertyByName<bool>("CleanUpPromptEnabled", false);
			}
			set
			{
				VersionHandler.SetPropertyByName<bool>("CleanUpPromptEnabled", value);
			}
		}

		public static bool RenameToCanonicalFilenames
		{
			get
			{
				return VersionHandler.GetPropertyByName<bool>("RenameToCanonicalFilenames", false);
			}
			set
			{
				VersionHandler.SetPropertyByName<bool>("RenameToCanonicalFilenames", value);
			}
		}

		public static bool VerboseLoggingEnabled
		{
			get
			{
				return VersionHandler.GetPropertyByName<bool>("VerboseLoggingEnabled", false);
			}
			set
			{
				VersionHandler.SetPropertyByName<bool>("VerboseLoggingEnabled", value);
			}
		}

		public static IEnumerable<string> UpdateCompleteMethods
		{
			get
			{
				return VersionHandler.GetPropertyByName<IEnumerable<string>>("UpdateCompleteMethods", VersionHandler.UpdateCompleteMethodsInternal);
			}
			set
			{
				if (VersionHandler.Impl != null)
				{
					VersionHandler.SetPropertyByName<IEnumerable<string>>("UpdateCompleteMethods", value);
				}
				else
				{
					VersionHandler.UpdateCompleteMethodsInternal = value;
				}
			}
		}

		private static IEnumerable<string> UpdateCompleteMethodsInternal
		{
			get
			{
				if (File.Exists("Temp/VersionHandlerCallbacks"))
				{
					return File.ReadAllText("Temp/VersionHandlerCallbacks").Split(new char[]
					{
						'\n'
					});
				}
				return new List<string>();
			}
			set
			{
				File.WriteAllText("Temp/VersionHandlerCallbacks", (value != null) ? string.Join("\n", new List<string>(value).ToArray()) : string.Empty);
			}
		}

		static VersionHandler()
		{
			VersionHandler.VERSION_HANDLER_FILENAME_RE = new Regex(string.Format(".*[\\/]({0})(.*)(\\.dll)$", "Google.VersionHandlerImpl".Replace(".", "\\.")), RegexOptions.IgnoreCase);
			VersionHandler.typeCache = new Dictionary<string, Type>();
			VersionHandler.unityVersionMajorMinor = -1f;
			if (Environment.CommandLine.Contains("-gvh_disable"))
			{
				Debug.Log(string.Format("{0} bootstrap disabled", "Google.VersionHandlerImpl"));
			}
			else
			{
				Delegate arg_8C_0 = EditorApplication.update;
				if (VersionHandler.callbackFunction0 == null)
				{
					VersionHandler.callbackFunction0 = new EditorApplication.CallbackFunction(VersionHandler.BootStrap);
				}
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(arg_8C_0, VersionHandler.callbackFunction0);
				Delegate arg_BD_0 = EditorApplication.update;
				if (VersionHandler.callbackFunction1 == null)
				{
					VersionHandler.callbackFunction1 = new EditorApplication.CallbackFunction(VersionHandler.BootStrap);
				}
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(arg_BD_0, VersionHandler.callbackFunction1);
			}
		}

		private static void AddToBootStrappingFile(List<string> lines)
		{
			File.AppendAllText("Temp/VersionHandlerBootStrapping", string.Join("\n", lines.ToArray()) + "\n");
		}

		private static IEnumerable<string> ReadBootStrappingFile()
		{
			return File.ReadAllLines("Temp/VersionHandlerBootStrapping");
		}

		private static void BootStrap()
		{
			bool bootStrapping = VersionHandler.BootStrapping;
			bool flag = VersionHandler.Impl != null;
			if (bootStrapping)
			{
				VersionHandler.BootStrapping = !flag;
				return;
			}
			Delegate arg_44_0 = EditorApplication.update;
			if (VersionHandler.callbackFunction2 == null)
			{
				VersionHandler.callbackFunction2 = new EditorApplication.CallbackFunction(VersionHandler.BootStrap);
			}
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(arg_44_0, VersionHandler.callbackFunction2);
			if (flag)
			{
				return;
			}
			List<Match> list = new List<Match>();
			string[] array = AssetDatabase.FindAssets("l:gvh");
			for (int i = 0; i < array.Length; i++)
			{
				string guid = array[i];
				string input = AssetDatabase.GUIDToAssetPath(guid);
				Match match = VersionHandler.VERSION_HANDLER_FILENAME_RE.Match(input);
				if (match.Success)
				{
					list.Add(match);
				}
			}
			if (list.Count == 0)
			{
				Debug.LogWarning(string.Format("No {0} DLL found to bootstrap", "Google.VersionHandlerImpl"));
				return;
			}
			string text = null;
			int num = -1;
			foreach (Match current in list)
			{
				string value = current.Groups[0].Value;
				string value2 = current.Groups[2].Value;
				string[] array2 = value2.Split(new char[]
				{
					'.'
				});
				Array.Reverse(array2);
				int num2 = 0;
				int num3 = 1000;
				int num4 = 1;
				string[] array3 = array2;
				for (int j = 0; j < array3.Length; j++)
				{
					string s = array3[j];
					try
					{
						num2 += int.Parse(s) * num4;
					}
					catch (FormatException)
					{
					}
					num4 *= num3;
				}
				if (num2 > num)
				{
					num = num2;
					text = value;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				Debug.LogWarning(string.Format("Failed to get the most recent {0} DLL.  Unable to bootstrap.", "Google.VersionHandlerImpl"));
				return;
			}
			VersionHandler.BootStrapping = true;
			if (VersionHandler.FindClass("UnityEditor", "UnityEditor.PluginImporter") != null)
			{
				VersionHandler.EnableEditorPlugin(text);
			}
			else
			{
				VersionHandler.ReimportPlugin(text);
			}
		}

		private static void ReimportPlugin(string path)
		{
			File.Delete(path + ".meta");
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
		}

		private static void EnableEditorPlugin(string path)
		{
			PluginImporter pluginImporter = AssetImporter.GetAtPath(path) as PluginImporter;
			if (pluginImporter == null)
			{
				Debug.Log(string.Format("Failed to enable editor plugin {0}", path));
				return;
			}
			pluginImporter.SetCompatibleWithEditor(true);
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
		}

		private static PropertyInfo GetPropertyByName(string propertyName)
		{
			Type bootStrappedImpl = VersionHandler.BootStrappedImpl;
			if (bootStrappedImpl == null)
			{
				return null;
			}
			return bootStrappedImpl.GetProperty(propertyName);
		}

		private static T GetPropertyByName<T>(string propertyName, T defaultValue)
		{
			PropertyInfo propertyByName = VersionHandler.GetPropertyByName(propertyName);
			if (propertyByName == null)
			{
				return defaultValue;
			}
			return (T)((object)propertyByName.GetValue(null, null));
		}

		private static void SetPropertyByName<T>(string propertyName, T value)
		{
			PropertyInfo propertyByName = VersionHandler.GetPropertyByName(propertyName);
			if (propertyByName == null)
			{
				return;
			}
			propertyByName.SetValue(null, value, null);
		}

		public static void ShowSettings()
		{
			VersionHandler.InvokeImplMethod("ShowSettings", null, null, false);
		}

		public static void UpdateNow()
		{
			VersionHandler.InvokeImplMethod("UpdateNow", null, null, true);
		}

		private static string[] StringArrayFromObject(object obj)
		{
			return (obj == null) ? new string[0] : ((string[])obj);
		}

		public static string[] SearchAssetDatabase(string assetsFilter = null, VersionHandler.FilenameFilter filter = null)
		{
			return VersionHandler.StringArrayFromObject(VersionHandler.InvokeImplMethod("SearchAssetDatabase", null, new Dictionary<string, object>
			{
				{
					"assetsFilter",
					assetsFilter
				},
				{
					"filter",
					filter
				}
			}, false));
		}

		public static string[] FindAllAssets()
		{
			return VersionHandler.StringArrayFromObject(VersionHandler.InvokeImplMethod("FindAllAssets", null, null, false));
		}

		public static void UpdateVersionedAssets(bool forceUpdate = false)
		{
			string methodName = "UpdateVersionedAssets";
			Dictionary<string, object> namedArgs = new Dictionary<string, object>
			{
				{
					"forceUpdate",
					forceUpdate
				}
			};
			VersionHandler.InvokeImplMethod(methodName, null, namedArgs, true);
		}

		public static float GetUnityVersionMajorMinor()
		{
			if (VersionHandler.unityVersionMajorMinor > 0f)
			{
				return VersionHandler.unityVersionMajorMinor;
			}
			float result;
			try
			{
				object obj = VersionHandler.InvokeImplMethod("GetUnityVersionMajorMinor", null, null, false);
				VersionHandler.unityVersionMajorMinor = (float)obj;
				result = VersionHandler.unityVersionMajorMinor;
			}
			catch (Exception)
			{
				result = 0f;
			}
			return result;
		}

		private static object InvokeImplMethod(string methodName, object[] args = null, Dictionary<string, object> namedArgs = null, bool schedule = false)
		{
			Type bootStrappedImpl = VersionHandler.BootStrappedImpl;
			if (bootStrappedImpl == null)
			{
				if (VersionHandler.BootStrapping && schedule)
				{
					VersionHandler.AddToBootStrappingFile(new List<string>
					{
						methodName
					});
				}
				return null;
			}
			return VersionHandler.InvokeStaticMethod(bootStrappedImpl, methodName, args, namedArgs);
		}

		public static Type FindClass(string assemblyName, string className)
		{
			bool flag = !string.IsNullOrEmpty(assemblyName);
			string text = (!flag) ? className : (className + ", " + assemblyName);
			Type type;
			if (VersionHandler.typeCache.TryGetValue(text, out type))
			{
				return type;
			}
			type = Type.GetType(text);
			if (type == null)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					Assembly assembly = assemblies[i];
					if (flag)
					{
						if (assembly.GetName().Name == assemblyName)
						{
							type = Type.GetType(className + ", " + assembly.FullName);
							break;
						}
					}
					else
					{
						Type[] types = assembly.GetTypes();
						for (int j = 0; j < types.Length; j++)
						{
							Type type2 = types[j];
							if (type2.FullName == className)
							{
								type = type2;
							}
						}
						if (type != null)
						{
							break;
						}
					}
				}
			}
			if (type != null)
			{
				VersionHandler.typeCache[text] = type;
			}
			return type;
		}

		public static object InvokeInstanceMethod(object objectInstance, string methodName, object[] args, Dictionary<string, object> namedArgs = null)
		{
			return VersionHandler.InvokeMethod(objectInstance.GetType(), objectInstance, methodName, args, namedArgs);
		}

		public static object InvokeStaticMethod(Type type, string methodName, object[] args, Dictionary<string, object> namedArgs = null)
		{
			return VersionHandler.InvokeMethod(type, null, methodName, args, namedArgs);
		}

		public static object InvokeMethod(Type type, object objectInstance, string methodName, object[] args, Dictionary<string, object> namedArgs = null)
		{
			MethodInfo method = type.GetMethod(methodName);
			ParameterInfo[] parameters = method.GetParameters();
			int num = parameters.Length;
			object[] array = new object[num];
			int num2 = (args == null) ? 0 : args.Length;
			ParameterInfo[] array2 = parameters;
			for (int i = 0; i < array2.Length; i++)
			{
				ParameterInfo parameterInfo = array2[i];
				int position = parameterInfo.Position;
				if (position < num2)
				{
					array[position] = args[position];
				}
				else
				{
					object obj = parameterInfo.RawDefaultValue;
					object obj2;
					if (namedArgs != null && namedArgs.TryGetValue(parameterInfo.Name, out obj2))
					{
						obj = obj2;
					}
					array[position] = obj;
				}
			}
			return method.Invoke(objectInstance, array);
		}
	}
}
