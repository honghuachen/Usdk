using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using GooglePlayServices;
using UnityEditor.iOS.Xcode.Custom;

namespace Google {
	public class IOSResolver {
		private class Pod {
			public string name = null;

			public string version = null;

			public Dictionary<string, string> propertiesByName = new Dictionary<string, string> ();

			public bool bitcodeEnabled = true;

			public List<string> sources = new List<string> {
				"https://github.com/CocoaPods/Specs.git"
			};

			public string minTargetSdk = null;

			public string createdBy = Environment.StackTrace;

			public bool fromXmlFile = false;

			public string LocalPath {
				get {
					string text;
					if (!this.propertiesByName.TryGetValue ("path", out text)) {
						return "";
					}
					if (text.StartsWith ("'") && text.EndsWith ("'")) {
						text = text.Substring (1, text.Length - 2);
					}
					return text;
				}
			}

			public string PodFilePodLine {
				get {
					string text = string.Format ("pod '{0}'", this.name);
					if (!string.IsNullOrEmpty (this.version)) {
						text += string.Format (", '{0}'", this.version);
					}
					Dictionary<string, string> dictionary = new Dictionary<string, string> (this.propertiesByName);
					string localPath = this.LocalPath;
					if (!string.IsNullOrEmpty (localPath)) {
						dictionary["path"] = string.Format ("'{0}'", Path.GetFullPath (localPath));
					}
					string text2 = IOSResolver.Pod.PropertyDictionaryToString (dictionary);
					if (!string.IsNullOrEmpty (text2)) {
						text = text + ", " + text2;
					}
					return text;
				}
			}

			public Pod (string name, string version, bool bitcodeEnabled, string minTargetSdk, IEnumerable<string> sources, Dictionary<string, string> propertiesByName) {
				this.name = name;
				this.version = version;
				if (propertiesByName != null) {
					this.propertiesByName = new Dictionary<string, string> (propertiesByName);
				}
				this.bitcodeEnabled = bitcodeEnabled;
				this.minTargetSdk = minTargetSdk;
				if (sources != null) {
					List<string> list = new List<string> (sources);
					list.AddRange (this.sources);
					this.sources = list;
				}
			}

			public static string PropertyDictionaryToString (Dictionary<string, string> propertiesByName) {
				if (propertiesByName == null) {
					return "";
				}
				List<string> list = new List<string> ();
				foreach (KeyValuePair<string, string> current in propertiesByName) {
					list.Add (string.Format (":{0} => {1}", current.Key, current.Value));
				}
				return string.Join (", ", list.ToArray ());
			}

			public int MinTargetSdkToVersion () {
				string text = (!string.IsNullOrEmpty (this.minTargetSdk)) ? this.minTargetSdk : "0.0";
				if (!text.Contains (".")) {
					text += ".0";
				}
				return IOSResolver.TargetSdkStringToVersion (text);
			}

			public override bool Equals (object obj) {
				IOSResolver.Pod pod = obj as IOSResolver.Pod;
				return pod != null && this.name == pod.name && this.version == pod.version && this.propertiesByName.Count == pod.propertiesByName.Count && this.propertiesByName.Keys.All ((string key) => pod.propertiesByName.ContainsKey (key) && this.propertiesByName[key] == pod.propertiesByName[key]);
			}

			public override int GetHashCode () {
				int num = 0;
				if (this.name != null) {
					num ^= this.name.GetHashCode ();
				}
				if (this.version != null) {
					num ^= this.version.GetHashCode ();
				}
				foreach (KeyValuePair<string, string> current in this.propertiesByName) {
					num ^= current.GetHashCode ();
				}
				return num;
			}

			public static SortedDictionary<int, List<string>> BucketByMinSdkVersion (IEnumerable<IOSResolver.Pod> pods) {
				SortedDictionary<int, List<string>> sortedDictionary = new SortedDictionary<int, List<string>> ();
				foreach (IOSResolver.Pod current in pods) {
					int num = current.MinTargetSdkToVersion ();
					if (num != 0) {
						List<string> list = null;
						if (!sortedDictionary.TryGetValue (num, out list)) {
							list = new List<string> ();
						}
						list.Add (current.name);
						sortedDictionary[num] = list;
					}
				}
				return sortedDictionary;
			}
		}

		private class IOSXmlDependencies : XmlDependencies {
			private static string[] PODFILE_POD_PROPERTIES = new string[] {
				"configurations",
				"configuration",
				"modular_headers",
				"source",
				"subspecs",
				"path"
			};

			public IOSXmlDependencies () {
				this.dependencyType = "Cocoapods dependencies";
			}

			protected override bool Read (string filePath, Logger logger) {
				try {
					IOSResolver.Log (string.Format ("Reading iOS dependency json file {0}", filePath), true, LogLevel.Info);
					string json = File.ReadAllText (filePath);
					Hashtable table = json.hashtableFromJson ();
					Hashtable pods = table.SGet<Hashtable> ("pods");
					HashSet<string> trueStrings = new HashSet<string> {
						"true",
						"1"
					};
					HashSet<string> falseStrings = new HashSet<string> {
						"false",
						"0"
					};

					Dictionary<string, string> propertiesByName = new Dictionary<string, string> ();
					foreach (DictionaryEntry p in pods) {
						string podName = p.Key.ToString ();
						Hashtable podProperty = p.Value as Hashtable;
						string podVersion = podProperty["version"].ToString ();
						string bitCode = (podProperty["bitcode"].ToString () ?? "").ToLower ();
						bool bitcodeEnabled = true;
						bitcodeEnabled |= trueStrings.Contains (bitCode);
						bitcodeEnabled &= !falseStrings.Contains (bitCode);
						List<string> sources = null;

						propertiesByName = new Dictionary<string, string> ();
						IOSResolver.AddPodInternal (podName, podVersion, bitcodeEnabled, null, sources, false, null, true, propertiesByName);
					}
					return true;
				} catch (Exception ex) {
					IOSResolver.Log (string.Format ("Reading cocoapods dependency json file failed: filePath={0} msg={1}", filePath, ex.StackTrace), false, LogLevel.Error);
					return false;
				}
			}
		}

		public enum CocoapodsIntegrationMethod {
			None,
			Project,
			Workspace
		}

		private class CommandItem {
			public string Command {
				get;
				set;
			}

			public string Arguments {
				get;
				set;
			}

			public string WorkingDirectory {
				get;
				set;
			}

			public override string ToString () {
				return string.Format ("{0} {1}", this.Command, this.Arguments ?? "");
			}
		}

		private class DelegateContainer<T> {
			public T Handler {
				get;
				set;
			}
		}

		private delegate void LogMessageDelegate (string message, bool verbose = false, LogLevel level = LogLevel.Info);

		private delegate int CommandItemCompletionHandler (int commandIndex, IOSResolver.CommandItem[] commands, CommandLine.Result result);

		private const int BUILD_ORDER_REFRESH_DEPENDENCIES = 1;

		private const int BUILD_ORDER_CHECK_COCOAPODS_INSTALL = 2;

		private const int BUILD_ORDER_PATCH_PROJECT = 3;

		private const int BUILD_ORDER_GEN_PODFILE = 4;

		private const int BUILD_ORDER_INSTALL_PODS = 5;

		private const int BUILD_ORDER_UPDATE_DEPS = 6;

		private const string UNITY_PODFILE_BACKUP_POSTFIX = "_Unity.backup";

		private const string COCOAPOD_INSTALL_INSTRUCTIONS = "You can install CocoaPods with the Ruby gem package manager:\n > sudo gem install -n /usr/local/bin cocoapods\n > pod setup";

		public const string PROJECT_NAME = "Unity-iPhone";

		private const string PREFERENCE_NAMESPACE = "Google.IOSResolver.";

		private const string PREFERENCE_COCOAPODS_INSTALL_ENABLED = "Google.IOSResolver.Enabled";

		private const string PREFERENCE_COCOAPODS_INTEGRATION_METHOD = "Google.IOSResolver.CocoapodsIntegrationMethod";

		private const string PREFERENCE_PODFILE_GENERATION_ENABLED = "Google.IOSResolver.PodfileEnabled";

		private const string PREFERENCE_VERBOSE_LOGGING_ENABLED = "Google.IOSResolver.VerboseLoggingEnabled";

		private const string PREFERENCE_POD_TOOL_EXECUTION_VIA_SHELL_ENABLED = "Google.IOSResolver.PodToolExecutionViaShellEnabled";

		private const string PREFERENCE_AUTO_POD_TOOL_INSTALL_IN_EDITOR = "Google.IOSResolver.AutoPodToolInstallInEditor";

		private const string PREFERENCE_WARN_UPGRADE_WORKSPACE = "Google.IOSResolver.UpgradeToWorkspaceWarningDisabled";

		private const string PREFERENCE_SKIP_POD_INSTALL_WHEN_USING_WORKSPACE_INTEGRATION = "Google.IOSResolver.SkipPodInstallWhenUsingWorkspaceIntegration";

		private const string PODS_DIR = "Pods";

		private const string PODS_PROJECT_NAME = "Pods";

		private const string LIBRARY_FILENAME_PREFIX = "lib";

		private const string LIBRARY_FILENAME_EXTENSION = ".a";

		private const string PODS_VAR_TARGET_SRCROOT = "${PODS_TARGET_SRCROOT}";

		private const int DEFAULT_TARGET_SDK = 82;

		private const float epsilon = 1E-07f;

		private static SortedDictionary<string, IOSResolver.Pod> pods;

		private static string POD_EXECUTABLE;

		private static string[] POD_SEARCH_PATHS;

		private static string GEM_EXECUTABLE;

		private static HashSet<string> SOURCE_FILE_EXTENSIONS;

		private static string[] BUILD_CONFIG_NAMES;

		public static string TARGET_NAME;

		private static string[] PREFERENCE_KEYS;

		private static bool iOSXcodeExtensionLoaded;

		private static bool cocoapodsToolsInstallPresent;

		private static string IOS_PLAYBACK_ENGINES_PATH;

		private static string podsVersion;

		private static string PODFILE_GENERATED_COMMENT;

		private static Regex TARGET_SDK_REGEX;

		private static Regex PODFILE_POD_REGEX;

		private static Regex PODFILE_SOURCE_REGEX;

		private static IOSResolver.IOSXmlDependencies xmlDependencies;

		private static Logger logger;

		private static int CocoapodsIntegrationUpgradeDefault {
			get {
				return (!IOSResolver.LegacyCocoapodsInstallEnabled) ? 1 : 2;
			}
		}

		public static IOSResolver.CocoapodsIntegrationMethod CocoapodsIntegrationMethodPref;
		private static bool LegacyCocoapodsInstallEnabled;
		public static bool PodfileGenerationEnabled;
		public static bool PodToolExecutionViaShellEnabled;
		public static bool AutoPodToolInstallInEditorEnabled;
		public static bool UpgradeToWorkspaceWarningDisabled;
		public static bool VerboseLoggingEnabled;
		public static bool SkipPodInstallWhenUsingWorkspaceIntegration;
		public static bool Enabled {
			get {
				return IOSResolver.iOSXcodeExtensionLoaded;
			}
		}

		/// <summary>
		/// Workspace模式只能unity版本大于5.6
		/// </summary>
		/// <value></value>
		private static bool UnityCanLoadWorkspace {
			get {
				//return (Math.Abs(VersionHandler.GetUnityVersionMajorMinor() - 5.6f) >= 1E-07f || !Application.unityVersion.Contains(".0b")) && !Environment.CommandLine.Contains("-bvrbuildtarget") && VersionHandler.GetUnityVersionMajorMinor() >= 5.6f;
				return true;
			}
		}

		private static bool CocoapodsWorkspaceIntegrationEnabled {
			get {
				return IOSResolver.UnityCanLoadWorkspace && IOSResolver.CocoapodsIntegrationMethodPref == IOSResolver.CocoapodsIntegrationMethod.Workspace;
			}
		}

		private static bool CocoapodsProjectIntegrationEnabled {
			get {
				return IOSResolver.CocoapodsIntegrationMethodPref == IOSResolver.CocoapodsIntegrationMethod.Project || (!IOSResolver.UnityCanLoadWorkspace && IOSResolver.CocoapodsIntegrationMethodPref == IOSResolver.CocoapodsIntegrationMethod.Workspace);
			}
		}

		public static bool CocoapodsIntegrationEnabled {
			get {
				return IOSResolver.CocoapodsIntegrationMethodPref != IOSResolver.CocoapodsIntegrationMethod.None;
			}
		}

		private static string TargetSdk {
			get {
				List<string> val = GetBuildSettingProperties ("IPHONEOS_DEPLOYMENT_TARGET");
				if (val.Count > 0)
					return val[0];
				return "8.2";
			}
			set {
				TargetSdk = value;
			}
		}

		private static int TargetSdkVersion {
			get {
				return IOSResolver.TargetSdkStringToVersion (IOSResolver.TargetSdk);
			}
			set {
				IOSResolver.TargetSdk = IOSResolver.TargetSdkVersionToString (value);
			}
		}

		private static PBXProject proj;
		private static PlistElementDict plistRootDict;
		private static string xcodePath;
		private static string configPath;

		public static void Init (string xcodePath, string configPath) {
			IOSResolver.xcodePath = xcodePath;
			IOSResolver.configPath = configPath;

			IOSResolver.pods = new SortedDictionary<string, IOSResolver.Pod> ();
			IOSResolver.POD_EXECUTABLE = "pod";
			IOSResolver.POD_SEARCH_PATHS = new string[] {
				"/usr/local/bin",
				"/usr/bin"
			};
			IOSResolver.GEM_EXECUTABLE = "gem";
			IOSResolver.SOURCE_FILE_EXTENSIONS = new HashSet<string> (new string[] {
				".h",
				".c",
				".cc",
				".cpp",
				".mm",
				".m"
			});
			IOSResolver.BUILD_CONFIG_NAMES = new string[] {
				"Debug",
				"Release",
				"ReleaseForProfiling",
				"ReleaseForRunning",
				"ReleaseForTesting"
			};
			IOSResolver.TARGET_NAME = null;
			IOSResolver.PREFERENCE_KEYS = new string[] {
				"Google.IOSResolver.Enabled",
				"Google.IOSResolver.CocoapodsIntegrationMethod",
				"Google.IOSResolver.PodfileEnabled",
				"Google.IOSResolver.VerboseLoggingEnabled",
				"Google.IOSResolver.PodToolExecutionViaShellEnabled",
				"Google.IOSResolver.AutoPodToolInstallInEditor",
				"Google.IOSResolver.UpgradeToWorkspaceWarningDisabled",
				"Google.IOSResolver.SkipPodInstallWhenUsingWorkspaceIntegration"
			};
			IOSResolver.iOSXcodeExtensionLoaded = true;
			IOSResolver.cocoapodsToolsInstallPresent = false;
			IOSResolver.IOS_PLAYBACK_ENGINES_PATH = Path.Combine ("PlaybackEngines", "iOSSupport");
			IOSResolver.podsVersion = "";
			IOSResolver.PODFILE_GENERATED_COMMENT = "# IOSResolver Generated Podfile";
			IOSResolver.TARGET_SDK_REGEX = new Regex ("^[0-9]+\\.[0-9]$");
			IOSResolver.PODFILE_POD_REGEX = new Regex ("^\\s*pod\\s+'(?<podname>[^']+)'\\s*(,\\s*'(?<podversion>[^']+)')?(|(,\\s*:(?<propertyname>[^\\s]+)\\s*=>\\s*((?<propertyvalue>[^\\s,]+)\\s*|(?<propertyvalue>'[^']+')\\s*|(?<propertyvalue>\\[[^\\]]+\\])\\s*))+)$");
			IOSResolver.PODFILE_SOURCE_REGEX = new Regex ("^\\s*source\\s+'([^']*)'");
			IOSResolver.xmlDependencies = new IOSResolver.IOSXmlDependencies ();
			IOSResolver.logger = new Logger ();
			try {
				IOSResolver.InitializeTargetName ();
			} catch (Exception ex) {
				Console.WriteLine ("Failed: " + ex.ToString ());
				if (!(ex is FileNotFoundException) && !(ex is TypeInitializationException) && !(ex is TargetInvocationException)) {
					throw ex;
				}
				Console.WriteLine ("Failed to load the UnityEditor.iOS.Extensions.Xcode dll.  Is iOS support installed?");
			}
			if (IOSResolver.AutoPodToolInstallInEditorEnabled && IOSResolver.CocoapodsIntegrationEnabled && !ExecutionEnvironment.InBatchMode) {
				IOSResolver.AutoInstallCocoapods ();
			}

			//plist
			string plistPath = xcodePath + "/Info.plist";
			PlistDocument plist = new PlistDocument ();
			plist.ReadFromString (File.ReadAllText (plistPath));
			plistRootDict = plist.root;

			//build setting
			string projPath = PBXProject.GetPBXProjectPath (xcodePath);
			proj = new PBXProject ();
			proj.ReadFromString (File.ReadAllText (projPath));
		}

		private static List<string> GetBuildSettingProperties (string key) {
			string target = proj.TargetGuidByName (PBXProject.GetUnityTargetName ());
			return proj.GetBuildProperty (target, key);
		}

		private static List<string> FindFile (string searchPath, string fileToFind, int maxDepth, int currentDepth = 0) {
			if (Path.GetFileName (searchPath) == fileToFind) {
				return new List<string> {
					searchPath
				};
			}
			if (maxDepth == currentDepth) {
				return new List<string> ();
			}
			List<string> list = new List<string> ();
			string[] files = Directory.GetFiles (searchPath);
			for (int i = 0; i < files.Length; i++) {
				string text = files[i];
				if (Path.GetFileName (text) == fileToFind) {
					list.Add (text);
				}
			}
			string[] directories = Directory.GetDirectories (searchPath);
			for (int j = 0; j < directories.Length; j++) {
				string searchPath2 = directories[j];
				list.AddRange (IOSResolver.FindFile (searchPath2, fileToFind, maxDepth, currentDepth + 1));
			}
			return list;
		}

		private static void InitializeTargetName () {
			IOSResolver.TARGET_NAME = PBXProject.GetUnityTargetName ();
		}

		internal static void Log (string message, bool verbose = false, LogLevel level = LogLevel.Info) {
			IOSResolver.logger.Level = ((!IOSResolver.VerboseLoggingEnabled && !ExecutionEnvironment.InBatchMode) ? LogLevel.Info : LogLevel.Verbose);
			IOSResolver.logger.Log (message, (!verbose) ? level : LogLevel.Verbose);
		}

		internal static void LogToDialog (string message, bool verbose = false, LogLevel level = LogLevel.Info) {
			IOSResolver.Log (message, verbose, level);
		}

		public static bool PodPresent (string pod) {
			return new List<string> (IOSResolver.pods.Keys).Contains (pod);
		}

		private static bool InjectDependencies () {
			return IOSResolver.Enabled && IOSResolver.pods.Count > 0;
		}

		private static string PodVersionExpressionFromVersionDep (string dependencyVersion) {
			if (string.IsNullOrEmpty (dependencyVersion) || dependencyVersion.Equals ("LATEST")) {
				return null;
			}
			if (dependencyVersion.EndsWith ("+")) {
				return string.Format ("~> {0}", dependencyVersion.Substring (0, dependencyVersion.Length - 1));
			}
			return dependencyVersion;
		}

		public static void AddPod (string podName, string version = null, bool bitcodeEnabled = true, string minTargetSdk = null, IEnumerable<string> sources = null) {
			IOSResolver.AddPodInternal (podName, IOSResolver.PodVersionExpressionFromVersionDep (version), bitcodeEnabled, minTargetSdk, sources, true, null, false, null);
		}

		private static void AddPodInternal (string podName, string preformattedVersion = null, bool bitcodeEnabled = true, string minTargetSdk = null, IEnumerable<string> sources = null, bool overwriteExistingPod = true, string createdBy = null, bool fromXmlFile = false, Dictionary<string, string> propertiesByName = null) {
			IOSResolver.Pod pod = new IOSResolver.Pod (podName, preformattedVersion, bitcodeEnabled, minTargetSdk, sources, propertiesByName);
			pod.createdBy = (createdBy ?? pod.createdBy);
			pod.fromXmlFile = fromXmlFile;
			IOSResolver.Log (string.Format ("AddPod - name: {0} version: {1} bitcode: {2} sdk: {3} sources: {4}, properties: {5}\ncreatedBy: {6}\n\n", new object[] {
				podName,
				preformattedVersion ?? "null",
				bitcodeEnabled.ToString (),
				minTargetSdk ?? "null",
				(sources == null) ? "(null)" : string.Join (", ", new List<string> (sources).ToArray ()),
				IOSResolver.Pod.PropertyDictionaryToString (pod.propertiesByName),
				createdBy ?? pod.createdBy
			}), true, LogLevel.Info);
			IOSResolver.Pod obj = null;
			if (!overwriteExistingPod && IOSResolver.pods.TryGetValue (podName, out obj)) {
				if (!pod.Equals (obj)) {
					IOSResolver.Log (string.Format ("Pod {0} already present, ignoring.\nOriginal declaration {1}\nIgnored declaration {2}\n", podName, IOSResolver.pods[podName].createdBy, createdBy ?? "(unknown)"), false, LogLevel.Warning);
				}
				return;
			}
			IOSResolver.pods[podName] = pod;
			IOSResolver.UpdateTargetSdk (pod, true);
		}

		private static bool UpdateTargetSdk (IOSResolver.Pod pod, bool notifyUser = true) {
			int targetSdkVersion = IOSResolver.TargetSdkVersion;
			int num = pod.MinTargetSdkToVersion ();
			if (targetSdkVersion >= num) {
				return false;
			}
			if (notifyUser) {
				string targetSdk = IOSResolver.TargetSdk;
				IOSResolver.TargetSdkVersion = num;
				IOSResolver.Log (string.Concat (new string[] {
					"iOS Target SDK changed from ",
					targetSdk,
					" to ",
					IOSResolver.TargetSdk,
					" required by the ",
					pod.name,
					" pod"
				}), false, LogLevel.Info);
			}
			return true;
		}

		private static string GetProjectPath (string projectName) {
			return Path.Combine (xcodePath, Path.Combine (projectName + ".xcodeproj", "project.pbxproj"));
		}

		private static string GetProjectPath (string path, string projectName) {
			return Path.Combine (path, Path.Combine (projectName + ".xcodeproj", "project.pbxproj"));
		}

		public static string GetProjectPath () {
			return IOSResolver.GetProjectPath ("Unity-iPhone");
		}

		internal static int TargetSdkStringToVersion (string targetSdk) {
			if (IOSResolver.TARGET_SDK_REGEX.IsMatch (targetSdk)) {
				try {
					return Convert.ToInt32 (targetSdk.Replace (".", ""));
				} catch (FormatException) { }
			}
			IOSResolver.Log (string.Format ("Invalid iOS target SDK version configured \"{0}\".\n\nPlease change this to a valid SDK version (e.g {1}) in:\n  Player Settings -> Other Settings --> Target Minimum iOS Version\n", targetSdk, IOSResolver.TargetSdkVersionToString (82)), false, LogLevel.Warning);
			return 82;
		}

		internal static string TargetSdkVersionToString (int version) {
			int num = version / 10;
			int num2 = version % 10;
			return num.ToString () + "." + num2.ToString ();
		}

		private static List<string> FindPodsWithBitcodeDisabled () {
			List<string> list = new List<string> ();
			foreach (IOSResolver.Pod current in IOSResolver.pods.Values) {
				if (!current.bitcodeEnabled) {
					list.Add (current.name);
				}
			}
			return list;
		}

		public static void AutoInstallCocoapods () {
			IOSResolver.InstallCocoapodsInteractive (false);
		}

		public static void InstallCocoapodsInteractive (bool displayAlreadyInstalled = true) {
			IOSResolver.InstallCocoapods (true, displayAlreadyInstalled);
		}

		private static bool QueryGemInstalled (string gemPackageName, IOSResolver.LogMessageDelegate logMessage = null) {
			logMessage = (logMessage ?? new IOSResolver.LogMessageDelegate (IOSResolver.Log));
			logMessage (string.Format ("Determine whether Ruby Gem {0} is installed", gemPackageName), true, LogLevel.Info);
			string text = string.Format ("list {0} --no-versions", gemPackageName);
			CommandLine.Result result = IOSResolver.RunCommand (IOSResolver.GEM_EXECUTABLE, text, null, false);
			if (result.exitCode == 0) {
				string[] array = result.stdout.Split (new string[] {
					Environment.NewLine
				}, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++) {
					string a = array[i];
					if (a == gemPackageName) {
						logMessage (string.Format ("{0} is installed", gemPackageName), true, LogLevel.Info);
						return true;
					}
				}
			} else {
				logMessage (string.Format ("Unable to determine whether the {0} gem is installed, will attempt to install anyway.\n\n'{1} {2}' failed with error code ({3}):\n{4}\n{5}\n", new object[] {
					gemPackageName,
					IOSResolver.GEM_EXECUTABLE,
					text,
					result.exitCode,
					result.stdout,
					result.stderr
				}), false, LogLevel.Warning);
			}
			return false;
		}

		public static void InstallCocoapods (bool interactive, bool displayAlreadyInstalled = true) {
			IOSResolver.cocoapodsToolsInstallPresent = false;
			IOSResolver.LogMessageDelegate logMessage = null;
			if (interactive) {
				logMessage = new IOSResolver.LogMessageDelegate (IOSResolver.LogToDialog);
			} else {
				logMessage = new IOSResolver.LogMessageDelegate (IOSResolver.Log);
			}
			string podToolPath = IOSResolver.FindPodTool ();
			if (!string.IsNullOrEmpty (podToolPath)) {
				string message = "CocoaPods installation detected " + podToolPath;
				if (displayAlreadyInstalled) {
					logMessage (message, false, LogLevel.Info);
				}
				IOSResolver.cocoapodsToolsInstallPresent = true;
				return;
			}
			AutoResetEvent complete = new AutoResetEvent (false);
			string commonInstallErrorMessage = "It will not be possible to install Cocoapods in the generated Xcode project which will result in link errors when building your application.\n\nFor more information see:\n  https://guides.cocoapods.org/using/getting-started.html\n\n";
			IOSResolver.RunCommand (IOSResolver.GEM_EXECUTABLE, "list", null, false);
			Dictionary<string, List<string>> dictionary = IOSResolver.ReadGemsEnvironment ();
			string text = "--user-install";
			List<string> list;
			if (dictionary != null && dictionary.TryGetValue ("INSTALLATION DIRECTORY", out list)) {
				foreach (string current in list) {
					if (current.IndexOf ("/.rvm/") >= 0) {
						text = "";
						break;
					}
				}
			}
			if (IOSResolver.VerboseLoggingEnabled || ExecutionEnvironment.InBatchMode) {
				text += " --verbose";
			}
			List<IOSResolver.CommandItem> list2 = new List<IOSResolver.CommandItem> ();
			if (!IOSResolver.QueryGemInstalled ("activesupport", logMessage)) {
				list2.Add (new IOSResolver.CommandItem {
					Command = IOSResolver.GEM_EXECUTABLE,
						Arguments = "install activesupport -v 4.2.6 " + text
				});
			}
			list2.Add (new IOSResolver.CommandItem {
				Command = IOSResolver.GEM_EXECUTABLE,
					Arguments = "install cocoapods " + text
			});
			list2.Add (new IOSResolver.CommandItem {
				Command = IOSResolver.POD_EXECUTABLE,
					Arguments = "setup"
			});
			IOSResolver.RunCommandsAsync (list2.ToArray (), delegate (int commandIndex, IOSResolver.CommandItem[] commands, CommandLine.Result result) {
				IOSResolver.CommandItem commandItem = commands[commandIndex];
				commandIndex++;
				if (result.exitCode != 0) {
					logMessage (string.Format ("Failed to install CocoaPods for the current user.\n\n{0}\n'{1} {2}' failed with code ({3}):\n{4}\n\n{5}\n", new object[] {
						commonInstallErrorMessage,
						commandItem.Command,
						commandItem.Arguments,
						result.exitCode,
						result.stdout,
						result.stderr
					}), false, LogLevel.Error);
					complete.Set ();
					return -1;
				}
				if (commandIndex == commands.Length - 1) {
					podToolPath = IOSResolver.FindPodTool ();
					if (string.IsNullOrEmpty (podToolPath)) {
						logMessage (string.Format ("'{0} {1}' succeeded but the {2} tool cannot be found.\n\n{3}\n", new object[] {
							commandItem.Command,
								commandItem.Arguments,
								IOSResolver.POD_EXECUTABLE,
								commonInstallErrorMessage
						}), false, LogLevel.Error);
						complete.Set ();
						return -1;
					}
					commands[commandIndex].Command = podToolPath;
				} else if (commandIndex == commands.Length) {
					complete.Set ();
					logMessage ("CocoaPods tools successfully installed.", false, LogLevel.Info);
					IOSResolver.cocoapodsToolsInstallPresent = true;
				}
				return commandIndex;
			}, "Installing CocoaPods...");
			if (!interactive) {
				complete.WaitOne ();
			}
		}

		public static void OnPosetProcess (string xcodePath, string configPath, object cocoaPodType) {
			IOSResolver.CocoapodsIntegrationMethodPref = (IOSResolver.CocoapodsIntegrationMethod) cocoaPodType;
			IOSResolver.Init (xcodePath, configPath);
			IOSResolver.OnPostProcessRefreshXmlDependencies ();
			IOSResolver.OnPostProcessEnsurePodsInstallation ();
			IOSResolver.OnPostProcessPatchProject ();
			IOSResolver.OnPostProcessGenPodfile ();
			IOSResolver.OnPostProcessInstallPods ();
			IOSResolver.OnPostProcessUpdateProjectDeps ();
		}

		public static void OnPostProcessRefreshXmlDependencies () {
			if (!IOSResolver.CocoapodsIntegrationEnabled) {
				return;
			}
			IOSResolver.RefreshXmlDependencies ();
		}

		public static void OnPostProcessEnsurePodsInstallation () {
			if (!IOSResolver.CocoapodsIntegrationEnabled) {
				return;
			}
			IOSResolver.InstallCocoapods (false, true);
		}

		public static void OnPostProcessPatchProject () {
			if (!IOSResolver.InjectDependencies () || !IOSResolver.PodfileGenerationEnabled || !IOSResolver.CocoapodsProjectIntegrationEnabled || !IOSResolver.cocoapodsToolsInstallPresent) {
				return;
			}
			IOSResolver.PatchProject ();
		}

		internal static void PatchProject () {
			List<string> list = IOSResolver.FindPodsWithBitcodeDisabled ();
			bool flag = list.Count > 0;
			if (flag) {
				IOSResolver.Log ("Bitcode is disabled due to the following CocoaPods (" + string.Join (", ", list.ToArray ()) + ")", false, LogLevel.Warning);
			}
			string projectPath = IOSResolver.GetProjectPath ();
			PBXProject pBXProject = new PBXProject ();
			pBXProject.ReadFromString (File.ReadAllText (projectPath));
			string text = pBXProject.TargetGuidByName (IOSResolver.TARGET_NAME);
			pBXProject.SetBuildProperty (text, "CLANG_ENABLE_MODULES", "YES");
			pBXProject.AddBuildProperty (text, "OTHER_LDFLAGS", "$(inherited)");
			pBXProject.AddBuildProperty (text, "OTHER_CFLAGS", "$(inherited)");
			pBXProject.AddBuildProperty (text, "USER_HEADER_SEARCH_PATHS", "$(inherited)");
			pBXProject.AddBuildProperty (text, "USER_HEADER_SEARCH_PATHS", "$(PROJECT_DIR)/Pods/Headers/Public");
			pBXProject.AddBuildProperty (text, "HEADER_SEARCH_PATHS", "$(PROJECT_DIR)/Pods/Headers/Private");
			pBXProject.AddBuildProperty (text, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
			pBXProject.AddBuildProperty (text, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");
			pBXProject.AddBuildProperty (text, "LIBRARY_SEARCH_PATHS", "$(inherited)");
			pBXProject.AddBuildProperty (text, "OTHER_LDFLAGS", "-ObjC");
			pBXProject.SetBuildProperty (text, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
			if (flag) {
				pBXProject.AddBuildProperty (text, "ENABLE_BITCODE", "NO");
			}
			File.WriteAllText (projectPath, pBXProject.WriteToString ());
		}

		public static void OnPostProcessGenPodfile () {
			if (!IOSResolver.InjectDependencies () || !IOSResolver.PodfileGenerationEnabled) {
				return;
			}
			IOSResolver.GenPodfile ();
		}

		private static string GetPodfilePath (string pathToBuiltProject) {
			return Path.Combine (pathToBuiltProject, "Podfile");
		}

		private static string FindExistingUnityPodfile (string suspectedUnityPodfilePath) {
			if (!File.Exists (suspectedUnityPodfilePath)) {
				return null;
			}
			StreamReader streamReader = new StreamReader (suspectedUnityPodfilePath);
			string text = streamReader.ReadLine ();
			streamReader.Close ();
			if (text == null || text.StartsWith (IOSResolver.PODFILE_GENERATED_COMMENT)) {
				return IOSResolver.FindExistingUnityPodfile (suspectedUnityPodfilePath + "_Unity.backup");
			}
			return suspectedUnityPodfilePath;
		}

		private static void ParseUnityDeps (string unityPodfilePath) {
			IOSResolver.Log ("Parse Unity deps from: " + unityPodfilePath, true, LogLevel.Info);
			StreamReader streamReader = new StreamReader (unityPodfilePath);
			int num = 0;
			List<string> list = new List<string> ();
			string text;
			while ((text = streamReader.ReadLine ()) != null) {
				text = text.Trim ();
				Match match = IOSResolver.PODFILE_SOURCE_REGEX.Match (text);
				if (match.Groups.Count > 1) {
					list.Add (match.Groups[1].Value);
				} else if (text.StartsWith ("target 'Unity-iPhone' do")) {
					num++;
				} else if (num != 0) {
					if (text.EndsWith (" do")) {
						num++;
					} else if (text == "end") {
						num--;
					}
					if (num == 1) {
						Match match2 = IOSResolver.PODFILE_POD_REGEX.Match (text);
						GroupCollection groups = match2.Groups;
						if (groups.Count > 1) {
							string podName = groups["podname"].ToString ();
							string text2 = groups["podversion"].ToString ();
							CaptureCollection captures = groups["propertyname"].Captures;
							CaptureCollection captures2 = groups["propertyvalue"].Captures;
							int count = captures.Count;
							Dictionary<string, string> dictionary = new Dictionary<string, string> ();
							for (int i = 0; i < count; i++) {
								dictionary[captures[i].Value] = captures2[i].Value;
							}
							List<string> sources = list;
							IOSResolver.AddPodInternal (podName, (!string.IsNullOrEmpty (text2)) ? text2 : null, true, null, sources, false, unityPodfilePath, false, dictionary);
						}
					}
				}
			}
			streamReader.Close ();
		}

		private static string GeneratePodfileSourcesSection () {
			List<string> list = new List<string> ();
			HashSet<string> hashSet = new HashSet<string> ();
			int num = 0;
			bool flag;
			do {
				flag = false;
				foreach (IOSResolver.Pod current in IOSResolver.pods.Values) {
					if (num < current.sources.Count) {
						flag = true;
						string text = current.sources[num];
						if (hashSet.Add (text)) {
							list.Add (string.Format ("source '{0}'", text));
						}
					}
				}
				num++;
			}
			while (flag);
			return string.Join ("\n", list.ToArray ()) + "\n";
		}

		public static void GenPodfile () {
			string podfilePath = IOSResolver.GetPodfilePath (xcodePath);
			string text = IOSResolver.FindExistingUnityPodfile (podfilePath);
			IOSResolver.Log (string.Format ("Detected Unity Podfile: {0}", text), true, LogLevel.Info);
			if (text != null) {
				IOSResolver.ParseUnityDeps (text);
				if (podfilePath == text) {
					string text2 = podfilePath + "_Unity.backup";
					if (File.Exists (text2)) {
						File.Delete (text2);
					}
					File.Move (podfilePath, text2);
				}
			}
			IOSResolver.Log (string.Format ("Generating Podfile {0} with {1} integration.", podfilePath, (!IOSResolver.CocoapodsWorkspaceIntegrationEnabled) ? ((!IOSResolver.CocoapodsProjectIntegrationEnabled) ? "no target" : "Xcode project") : "Xcode workspace"), true, LogLevel.Info);
			using (StreamWriter streamWriter = new StreamWriter (podfilePath)) {
				streamWriter.Write (string.Concat (new string[] {
					IOSResolver.GeneratePodfileSourcesSection (),
						(!IOSResolver.CocoapodsProjectIntegrationEnabled) ? "" : "install! 'cocoapods', :integrate_targets => false\n",
						string.Format ("platform :ios, '{0}'\n\n", IOSResolver.TargetSdk),
						"target '",
						IOSResolver.TARGET_NAME,
						"' do\n"
				}));
				foreach (IOSResolver.Pod current in IOSResolver.pods.Values) {
					streamWriter.WriteLine (current.PodFilePodLine);
				}
				streamWriter.WriteLine ("end");
			}
		}

		private static Dictionary<string, List<string>> ReadGemsEnvironment () {
			CommandLine.Result result = IOSResolver.RunCommand (IOSResolver.GEM_EXECUTABLE, "environment", null, false);
			if (result.exitCode != 0) {
				return null;
			}
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>> ();
			int num = 0;
			List<string> list = null;
			char[] separator = new char[] {
				':'
			};
			string[] array = result.stdout.Split (new char[] {
				'\r',
				'\n'
			});
			for (int i = 0; i < array.Length; i++) {
				string text = array[i];
				string text2 = text.Trim ();
				int num2 = text.Length - text2.Length;
				if (num2 < num) {
					list = null;
				}
				if (text2.StartsWith ("- ")) {
					text2 = text2.Substring ("- ".Length).Trim ();
					if (list == null) {
						string[] array2 = text2.Split (separator);
						list = new List<string> ();
						dictionary[array2[0].Trim ()] = list;
						string text3 = (array2.Length != 2) ? null : array2[1].Trim ();
						if (!string.IsNullOrEmpty (text3)) {
							list.Add (text3);
							list = null;
						}
					} else if (num2 >= num) {
						list.Add (text2);
					}
				} else {
					list = null;
				}
				num = num2;
			}
			return dictionary;
		}

		private static string FindPodTool () {
			string[] pOD_SEARCH_PATHS = IOSResolver.POD_SEARCH_PATHS;
			for (int i = 0; i < pOD_SEARCH_PATHS.Length; i++) {
				string path = pOD_SEARCH_PATHS[i];
				string text = Path.Combine (path, IOSResolver.POD_EXECUTABLE);
				IOSResolver.Log ("Searching for CocoaPods tool in " + text, true, LogLevel.Info);
				if (File.Exists (text)) {
					IOSResolver.Log ("Found CocoaPods tool in " + text, true, LogLevel.Info);
					return text;
				}
			}
			IOSResolver.Log ("Querying gems for CocoaPods install path", true, LogLevel.Info);
			Dictionary<string, List<string>> dictionary = IOSResolver.ReadGemsEnvironment ();
			if (dictionary != null) {
				string[] array = new string[] {
					"EXECUTABLE DIRECTORY",
					"GEM PATHS"
				};
				for (int j = 0; j < array.Length; j++) {
					string text2 = array[j];
					List<string> list;
					if (dictionary.TryGetValue (text2, out list)) {
						foreach (string current in list) {
							string path2 = (!(text2 == "EXECUTABLE DIRECTORY")) ? Path.Combine (current, "bin") : current;
							string text3 = Path.Combine (path2, IOSResolver.POD_EXECUTABLE);
							IOSResolver.Log ("Checking gems install path for CocoaPods tool " + text3, true, LogLevel.Info);
							if (File.Exists (text3)) {
								IOSResolver.Log ("Found CocoaPods tool in " + text3, true, LogLevel.Info);
								return text3;
							}
						}
					}
				}
			}
			return null;
		}

		private static void LogCommandLineResult (string command, CommandLine.Result result) {
			IOSResolver.Log (string.Format ("'{0}' completed with code {1}\n\n{2}\n{3}\n", new object[] {
				command,
				result.exitCode,
				result.stdout,
				result.stderr
			}), true, LogLevel.Info);
		}

		private static void RunCommandsAsync (IOSResolver.CommandItem[] commands, IOSResolver.CommandItemCompletionHandler completionDelegate, string summaryText = null) {
			Dictionary<string, string> envVars = new Dictionary<string, string> {
				{
					"LANG",
					(Environment.GetEnvironmentVariable ("LANG") ?? "en_US.UTF-8").Split (new char[] {
						'.'
					}) [0] + ".UTF-8"
				},
				{
					"PATH",
					"/usr/local/bin:" + (Environment.GetEnvironmentVariable ("PATH") ?? "")
				}
			};
			if (!string.IsNullOrEmpty (summaryText)) {
				IOSResolver.Log (summaryText, false, LogLevel.Info);
			}
			int num = 0;
			while (num >= 0 && num < commands.Length) {
				IOSResolver.CommandItem commandItem = commands[num];
				IOSResolver.Log (commandItem.ToString (), true, LogLevel.Info);
				bool podToolExecutionViaShellEnabled = IOSResolver.PodToolExecutionViaShellEnabled;
				CommandLine.Result result = CommandLine.RunViaShell (commandItem.Command, commandItem.Arguments, commandItem.WorkingDirectory, envVars, null, podToolExecutionViaShellEnabled, true);
				IOSResolver.LogCommandLineResult (commandItem.ToString (), result);
				num = completionDelegate (num, commands, result);
			}
		}

		private static void RunCommandAsync (string command, string commandArgs, CommandLine.CompletionHandler completionDelegate, string workingDirectory = null, bool displayDialog = false, string summaryText = null) {
			IOSResolver.RunCommandsAsync (new IOSResolver.CommandItem[] {
				new IOSResolver.CommandItem {
					Command = command,
						Arguments = commandArgs,
						WorkingDirectory = workingDirectory
				}
			}, delegate (int commandIndex, IOSResolver.CommandItem[] commands, CommandLine.Result result) {
				completionDelegate (result);
				return -1;
			}, summaryText);
		}

		private static CommandLine.Result RunCommand (string command, string commandArgs, string workingDirectory = null, bool displayDialog = false) {
			CommandLine.Result result = null;
			AutoResetEvent complete = new AutoResetEvent (false);
			IOSResolver.RunCommandAsync (command, commandArgs, delegate (CommandLine.Result asyncResult) {
				result = asyncResult;
				complete.Set ();
			}, workingDirectory, displayDialog, null);
			complete.WaitOne ();
			return result;
		}

		private static void RunPodCommandAsync (string podArgs, string pathToBuiltProject, CommandLine.CompletionHandler completionDelegate, bool displayDialog = false, string summaryText = null) {
			string text = IOSResolver.FindPodTool ();
			if (string.IsNullOrEmpty (text)) {
				CommandLine.Result result = new CommandLine.Result ();
				result.exitCode = 1;
				result.stderr = string.Format ("'{0}' command not found; unable to generate a usable Xcode project.\n{1}", IOSResolver.POD_EXECUTABLE, "You can install CocoaPods with the Ruby gem package manager:\n > sudo gem install -n /usr/local/bin cocoapods\n > pod setup");
				IOSResolver.Log (result.stderr, false, LogLevel.Error);
				completionDelegate (result);
			}
			IOSResolver.RunCommandAsync (text, podArgs, completionDelegate, pathToBuiltProject, displayDialog, summaryText);
		}

		private static CommandLine.Result RunPodCommand (string podArgs, string pathToBuiltProject, bool displayDialog = false) {
			CommandLine.Result result = null;
			AutoResetEvent complete = new AutoResetEvent (false);
			IOSResolver.RunPodCommandAsync (podArgs, pathToBuiltProject, delegate (CommandLine.Result asyncResult) {
				result = asyncResult;
				complete.Set ();
			}, displayDialog, null);
			complete.WaitOne ();
			return result;
		}

		public static void OnPostProcessInstallPods () {
			string pathToBuiltProject = xcodePath;
			if (!IOSResolver.InjectDependencies () || !IOSResolver.PodfileGenerationEnabled) {
				return;
			}
			if (!IOSResolver.CocoapodsIntegrationEnabled || !IOSResolver.cocoapodsToolsInstallPresent) {
				IOSResolver.Log (string.Format ("Cocoapod installation is disabled.\nIf CocoaPods are not installed in your project it will not link.\n\nThe command '{0} install' must be executed from the {1} directory to generate a Xcode workspace that includes the CocoaPods referenced by {2}.\nFor more information see:\n  https://guides.cocoapods.org/using/using-cocoapods.html\n\n", IOSResolver.POD_EXECUTABLE, pathToBuiltProject, IOSResolver.GetPodfilePath (pathToBuiltProject)), false, LogLevel.Warning);
				return;
			}
			if (IOSResolver.UnityCanLoadWorkspace && IOSResolver.CocoapodsIntegrationMethodPref == IOSResolver.CocoapodsIntegrationMethod.Workspace && IOSResolver.SkipPodInstallWhenUsingWorkspaceIntegration) {
				IOSResolver.Log ("Skipping pod install.", false, LogLevel.Warning);
				return;
			}
			CommandLine.Result result = IOSResolver.RunPodCommand ("--version", pathToBuiltProject, false);
			if (result.exitCode == 0) {
				IOSResolver.podsVersion = result.stdout.Trim ();
			}
			if (result.exitCode != 0 || (!string.IsNullOrEmpty (IOSResolver.podsVersion) && IOSResolver.podsVersion[0] == '0')) {
				IOSResolver.Log (string.Concat (new string[] {
					"Error running CocoaPods. Please ensure you have at least version 1.0.0.  You can install CocoaPods with the Ruby gem package manager:\n > sudo gem install -n /usr/local/bin cocoapods\n > pod setup\n\n'",
					IOSResolver.POD_EXECUTABLE,
					" --version' returned status: ",
					result.exitCode.ToString (),
					"\noutput: ",
					result.stdout,
					"\n\n",
					result.stderr
				}), false, LogLevel.Error);
				return;
			}
			result = IOSResolver.RunPodCommand ("install", pathToBuiltProject, false);
			if (result.exitCode != 0) {
				CommandLine.Result result2 = IOSResolver.RunPodCommand ("repo update", pathToBuiltProject, false);
				bool flag = result2.exitCode == 0;
				CommandLine.Result result3 = IOSResolver.RunPodCommand ("install", pathToBuiltProject, false);
				if (result3.exitCode != 0) {
					IOSResolver.Log (string.Concat (new string[] {
						"iOS framework addition failed due to a CocoaPods installation failure. This will will likely result in an non-functional Xcode project.\n\nAfter the failure, \"pod repo update\" was executed and ",
						(!flag) ? "failed. " : "succeeded. ",
						"\"pod install\" was then attempted again, and still failed. This may be due to a broken CocoaPods installation. See: https://guides.cocoapods.org/using/troubleshooting.html for potential solutions.\n\npod install output:\n\n",
						result.stdout,
						"\n\n",
						result.stderr,
						"\n\n\npod repo update output:\n\n",
						result2.stdout,
						"\n\n",
						result2.stderr
					}), false, LogLevel.Error);
					return;
				}
			}
		}

		internal static List<string> FindFilesWithExtensions (string directory, HashSet<string> extensions) {
			List<string> list = new List<string> ();
			string[] directories = Directory.GetDirectories (directory);
			for (int i = 0; i < directories.Length; i++) {
				string directory2 = directories[i];
				list.AddRange (IOSResolver.FindFilesWithExtensions (directory2, extensions));
			}
			string[] files = Directory.GetFiles (directory);
			for (int j = 0; j < files.Length; j++) {
				string text = files[j];
				string extension = Path.GetExtension (text);
				if (extensions.Contains (extension)) {
					list.Add (text);
				}
			}
			return list;
		}

		public static void OnPostProcessUpdateProjectDeps () {
			if (!IOSResolver.InjectDependencies () || !IOSResolver.PodfileGenerationEnabled || !IOSResolver.CocoapodsProjectIntegrationEnabled || !IOSResolver.cocoapodsToolsInstallPresent) {
				return;
			}
			IOSResolver.UpdateProjectDeps ();
		}

		public static string ExpandPodsVariables (string value, string sourcePodPath) {
			if (!string.IsNullOrEmpty (sourcePodPath)) {
				value = value.Replace ("${PODS_TARGET_SRCROOT}", sourcePodPath);
			}
			return value;
		}

		public static void UpdateProjectDeps () {
			string pathToBuiltProject = xcodePath;
			string text = Path.Combine (pathToBuiltProject, "Pods");
			if (!Directory.Exists (text)) {
				return;
			}
			string path = Path.Combine (pathToBuiltProject, "Unity-iPhone.xcworkspace");
			if (IOSResolver.UnityCanLoadWorkspace && IOSResolver.CocoapodsProjectIntegrationEnabled && Directory.Exists (path)) {
				IOSResolver.Log ("Removing the generated workspace to force Unity to directly load the xcodeproj.\nSince Unity 5.6, Unity can now load workspace files generated from CocoaPods integration, however the IOSResolver Settings are configured to use project level integration. It's recommended that you use workspace integration instead.\nYou can manage this setting from: Assets > Play Services Resolver > iOS Resolver > Settings, using the CocoaPods Integration drop down menu.", false, LogLevel.Warning);
				Directory.Delete (path, true);
			}
			string fullPath = Path.GetFullPath (pathToBuiltProject);
			Directory.CreateDirectory (Path.Combine (pathToBuiltProject, "Frameworks"));
			Directory.CreateDirectory (Path.Combine (pathToBuiltProject, "Resources"));
			string projectPath = IOSResolver.GetProjectPath (pathToBuiltProject);
			PBXProject pBXProject = new PBXProject ();
			pBXProject.ReadFromString (File.ReadAllText (projectPath));
			string text2 = pBXProject.TargetGuidByName (IOSResolver.TARGET_NAME);
			HashSet<string> hashSet = new HashSet<string> ();
			string[] directories = Directory.GetDirectories (text, "*.framework", SearchOption.AllDirectories);
			for (int i = 0; i < directories.Length; i++) {
				string text3 = directories[i];
				IOSResolver.Log (string.Format ("Inspecting framework {0}", text3), true, LogLevel.Info);
				string name = new DirectoryInfo (text3).Name;
				string text4 = Path.Combine ("Frameworks", name);
				string text5 = Path.Combine (pathToBuiltProject, text4);
				if (!File.Exists (Path.Combine (text3, Path.GetFileName (text3).Replace (".framework", "")))) {
					IOSResolver.Log (string.Format ("Ignoring framework {0}", text3), true, LogLevel.Info);
				} else {
					IOSResolver.Log (string.Format ("Moving framework {0} --> {1}", text3, text5), true, LogLevel.Info);
					FileUtils.DeleteExistingFileOrDirectory (text5, true);
					Directory.Move (text3, text5);
					pBXProject.AddFileToBuild (text2, pBXProject.AddFile (text4, text4, PBXSourceTree.Source));
					string[] array = new string[] {
						text5,
						Path.GetDirectoryName (Path.GetDirectoryName (text3))
					};
					for (int j = 0; j < array.Length; j++) {
						string path2 = array[j];
						string text6 = Path.Combine (path2, "Resources");
						IOSResolver.Log (string.Format ("Looking for resources folder {0}", text6), true, LogLevel.Info);
						if (Directory.Exists (text6)) {
							IOSResolver.Log (string.Format ("Found resources {0}", text6), true, LogLevel.Info);
							string[] files = Directory.GetFiles (text6);
							string[] directories2 = Directory.GetDirectories (text6);
							string[] array2 = files;
							for (int k = 0; k < array2.Length; k++) {
								string text7 = array2[k];
								string text8 = Path.Combine ("Resources", Path.GetFileName (text7));
								File.Copy (text7, Path.Combine (pathToBuiltProject, text8), true);
								IOSResolver.Log (string.Format ("Copying resource file {0} --> {1}", text6, Path.Combine (pathToBuiltProject, text8)), true, LogLevel.Info);
								pBXProject.AddFileToBuild (text2, pBXProject.AddFile (text8, text8, PBXSourceTree.Source));
							}
							string[] array3 = directories2;
							for (int l = 0; l < array3.Length; l++) {
								string text9 = array3[l];
								string text10 = Path.Combine ("Resources", new DirectoryInfo (text9).Name);
								string text11 = Path.Combine (pathToBuiltProject, text10);
								FileUtils.DeleteExistingFileOrDirectory (text11, true);
								IOSResolver.Log (string.Format ("Moving resource directory {0} --> {1}", text9, text11), true, LogLevel.Info);
								Directory.Move (text9, text11);
								pBXProject.AddFileToBuild (text2, pBXProject.AddFile (text10, text10, PBXSourceTree.Source));
							}
						}
					}
				}
			}
			string[] files2 = Directory.GetFiles (Path.GetFullPath (text), "lib*.a", SearchOption.AllDirectories);
			for (int m = 0; m < files2.Length; m++) {
				string text12 = files2[m];
				string text13 = text12.Substring (fullPath.Length + 1);
				pBXProject.AddFileToBuild (text2, pBXProject.AddFile (text13, text13, PBXSourceTree.Source));
				string fileName = Path.GetFileName (text13);
				hashSet.Add ("-l" + fileName.Substring ("lib".Length, fileName.Length - ("lib".Length + ".a".Length)));
				pBXProject.AddBuildProperty (text2, "LIBRARY_SEARCH_PATHS", "$(PROJECT_DIR)/" + Path.GetDirectoryName (text13));
			}
			foreach (string current in hashSet) {
				pBXProject.AddBuildProperty (text2, "OTHER_LDFLAGS", current);
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string> ();
			foreach (string current2 in IOSResolver.FindFilesWithExtensions (text, IOSResolver.SOURCE_FILE_EXTENSIONS)) {
				dictionary[current2.Substring (text.Length + 1)] = current2.Substring (pathToBuiltProject.Length + 1);
			}
			string projectPath2 = IOSResolver.GetProjectPath (text, "Pods");
			PBXProject pBXProject2 = null;
			if (File.Exists (projectPath2)) {
				pBXProject2 = new PBXProject ();
				pBXProject2.ReadFromString (File.ReadAllText (projectPath2));
			}
			foreach (KeyValuePair<string, string> current3 in dictionary) {
				string text14 = current3.Value.Substring ("Pods".Length + 1);
				if (pBXProject2 != null && pBXProject2.ContainsFileByRealPath (text14, PBXSourceTree.Source)) {
					IOSResolver.Log ("Adding source file " + text14 + " to Xcode project.", true, LogLevel.Info);
					pBXProject.AddFileToBuild (text2, pBXProject.AddFile (current3.Value, current3.Value, PBXSourceTree.Source));
					pBXProject.UpdateBuildProperty (new string[] {
						text2
					}, "USER_HEADER_SEARCH_PATHS", new string[] {
						"$(SRCROOT)/" + Path.GetDirectoryName (current3.Value)
					}, new string[0]);
				} else {
					IOSResolver.Log ("Skipping adding source file " + text14 + " to Xcode project as it is not part of the pod project", true, LogLevel.Info);
				}
			}
			HashSet<string> hashSet2 = new HashSet<string> ();
			foreach (string current4 in dictionary.Values) {
				hashSet2.Add (current4.Split (new char[] {
					'/',
					'\\'
				}) [1]);
			}
			foreach (string current5 in hashSet2) {
				pBXProject.UpdateBuildProperty (new string[] {
					text2
				}, "USER_HEADER_SEARCH_PATHS", new string[] {
					"$(SRCROOT)/Pods/" + current5
				}, new string[0]);
			}
			IEnumerable<string> enumerable = null;
			PropertyInfo property = pBXProject.GetType ().GetProperty ("BuildConfigNames");
			if (property != null) {
				enumerable = (property.GetValue (null, null) as IEnumerable<string>);
			}
			enumerable = (enumerable ?? IOSResolver.BUILD_CONFIG_NAMES);
			Dictionary<string, string> dictionary2 = new Dictionary<string, string> {
				{
				"Pods-Unity-iPhone.debug.xcconfig",
				"Debug"
				},
				{
				"Pods-Unity-iPhone.release.xcconfig",
				"Release"
				}
			};
			Regex regex = new Regex ("\\s*(\\S+)\\s*=\\s*(.*)");
			List<string> list = IOSResolver.FindFilesWithExtensions (text, new HashSet<string> (new string[] {
				".xcconfig"
			}));
			HashSet<string> hashSet3 = new HashSet<string> {
				"GCC_PREPROCESSOR_DEFINITIONS",
				"HEADER_SEARCH_PATHS",
				"LIBRARY_SEARCH_PATHS",
				"OTHER_CFLAGS",
				"OTHER_LDFLAGS"
			};
			foreach (string current6 in list) {
				string text15 = null;
				if (dictionary2.TryGetValue (Path.GetFileName (current6), out text15)) {
					Dictionary<string, string> dictionary3 = new Dictionary<string, string> ();
					foreach (string current7 in enumerable) {
						if (current7.ToLower ().StartsWith (text15.ToLower ())) {
						string text16 = pBXProject.BuildConfigByName (text2, current7);
						if (!string.IsNullOrEmpty (text16)) {
						dictionary3[text16] = current7;
							}
						}
					}
					if (dictionary3.Count != 0) {
						HashSet<string> hashSet4 = new HashSet<string> (list);
						hashSet4.ExceptWith (new HashSet<string> (dictionary2.Keys));
						hashSet4.Add (current6);
						foreach (string current8 in hashSet4) {
							Dictionary<string, string> dictionary4 = new Dictionary<string, string> ();
							string[] array4 = CommandLine.SplitLines (File.ReadAllText (current8));
							for (int n = 0; n < array4.Length; n++) {
								string text17 = array4[n];
								string text18 = text17.Trim ();
								if (!text18.StartsWith ("//")) {
									if (text18.EndsWith (";")) {
										text18 = text18.Substring (0, text18.Length - 1);
									}
									if (text18.Trim ().Length != 0) {
										if (text18.StartsWith ("#include")) {
											IOSResolver.Log (string.Format ("{0} contains unsupported #include statement '{1}'", current8, text18), false, LogLevel.Warning);
										} else {
											Match match = regex.Match (text18);
											if (!match.Success) {
												IOSResolver.Log (string.Format ("{0} line '{1}' does not contain a variable assignment", current8, text18), false, LogLevel.Warning);
											} else {
												dictionary4[match.Groups[1].Value] = match.Groups[2].Value;
											}
										}
									}
								}
							}
							string text19 = null;
							if (dictionary4.TryGetValue ("OTHER_LDFLAGS", out text19)) {
								List<string> list2 = new List<string> ();
								string[] array5 = text19.Split (new char[0]);
								for (int num = 0; num < array5.Length; num++) {
									string text20 = array5[num];
									if (!text20.StartsWith ("-l") || !hashSet2.Contains (text20.Substring ("-l".Length).Trim (new char[] {
											'"'
										}).Trim (new char[] {
											'\''
										}))) {
										list2.Add (text20);
									}
								}
								dictionary4["OTHER_LDFLAGS"] = string.Join (" ", list2.ToArray ());
							}
							string sourcePodPath = (!(current8 == current6)) ? string.Format ("$(PODS_ROOT)/{0}", Path.GetFileNameWithoutExtension (Path.GetFileName (current8))) : null;
							foreach (KeyValuePair<string, string> current9 in dictionary3) {
								foreach (KeyValuePair<string, string> current10 in dictionary4) {
									if (!(current8 != current6) || hashSet3.Contains (current10.Key)) {
										string text21 = IOSResolver.ExpandPodsVariables (current10.Value, sourcePodPath);
										IOSResolver.Log (string.Format ("From {0} applying build setting '{1} = {2} (expanded as '{3}')' to build config {4} ({5})", new object[] {
											current8.Substring (pathToBuiltProject.Length + 1),
												current10.Key,
												current10.Value,
												text21,
												current9.Value,
												current9.Key
										}), true, LogLevel.Info);
										pBXProject.AddBuildPropertyForConfig (current9.Key, current10.Key, text21);
									}
								}
							}
						}
					}
				}
			}
			if (pBXProject2 != null) {
				string[] directories3 = Directory.GetDirectories (text);
				for (int num2 = 0; num2 < directories3.Length; num2++) {
					string path3 = directories3[num2];
					string fileName2 = Path.GetFileName (path3);
					string text22 = pBXProject2.TargetGuidByName (fileName2);
					IOSResolver.Log (string.Format ("Looking for target: {0} guid: {1}", fileName2, text22 ?? "null"), true, LogLevel.Info);
					if (text22 != null) {
						foreach (KeyValuePair<string, string> current11 in dictionary) {
							string text23 = pBXProject2.FindFileGuidByRealPath (current11.Key);
							if (text23 != null) {
								List<string> compileFlagsForFile = pBXProject2.GetCompileFlagsForFile (text22, text23);
								if (compileFlagsForFile != null) {
									string text24 = pBXProject.FindFileGuidByProjectPath (current11.Value);
									if (text24 == null) {
										IOSResolver.Log ("Unable to find " + current11.Value + " in generated project", false, LogLevel.Warning);
									} else {
										IOSResolver.Log (string.Format ("Setting {0} compile flags to ({1})", current11.Key, string.Join (", ", compileFlagsForFile.ToArray ())), true, LogLevel.Info);
										pBXProject.SetCompileFlagsForFile (text2, text24, compileFlagsForFile);
									}
								}
							}
						}
					}
				}
			} else if (File.Exists (projectPath2 + ".xml")) {
				IOSResolver.Log ("Old CocoaPods installation detected (version: " + IOSResolver.podsVersion + ").  Unable to include source pods, your project will not build.\n\nOlder versions of the pod tool generate xml format Xcode projects which can not be read by Unity's xcodeapi.  To resolve this issue update CocoaPods to at least version 1.1.0\n\nYou can install CocoaPods with the Ruby gem package manager:\n > sudo gem install -n /usr/local/bin cocoapods\n > pod setup", false, LogLevel.Error);
			}
			File.WriteAllText (projectPath, pBXProject.WriteToString ());
		}

		private static void RefreshXmlDependencies () {
			List<string> list = new List<string> ();
			foreach (KeyValuePair<string, IOSResolver.Pod> current in IOSResolver.pods) {
				if (current.Value.fromXmlFile) {
					list.Add (current.Key);
				}
			}
			foreach (string current2 in list) {
				IOSResolver.pods.Remove (current2);
			}
			IOSResolver.xmlDependencies.ReadAll (configPath, IOSResolver.logger);
		}
	}
}