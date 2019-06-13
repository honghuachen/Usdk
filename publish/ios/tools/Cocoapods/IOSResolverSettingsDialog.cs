using System;
using UnityEditor;
using UnityEngine;

namespace Google
{
	public class IOSResolverSettingsDialog : EditorWindow
	{
		private class Settings
		{
			internal bool cocoapodsInstallEnabled;

			internal bool podfileGenerationEnabled;

			internal bool podToolExecutionViaShellEnabled;

			internal bool autoPodToolInstallInEditorEnabled;

			internal bool verboseLoggingEnabled;

			internal int cocoapodsIntegrationMenuIndex;

			internal bool useProjectSettings;

			internal Settings()
			{
				this.cocoapodsInstallEnabled = IOSResolver.CocoapodsInstallEnabled;
				this.podfileGenerationEnabled = IOSResolver.PodfileGenerationEnabled;
				this.podToolExecutionViaShellEnabled = IOSResolver.PodToolExecutionViaShellEnabled;
				this.autoPodToolInstallInEditorEnabled = IOSResolver.AutoPodToolInstallInEditorEnabled;
				this.verboseLoggingEnabled = IOSResolver.VerboseLoggingEnabled;
				this.cocoapodsIntegrationMenuIndex = IOSResolverSettingsDialog.FindIndexFromCocoapodsIntegrationMethod(IOSResolver.CocoapodsIntegrationMethodPref);
				this.useProjectSettings = IOSResolver.UseProjectSettings;
			}

			internal void Save()
			{
				IOSResolver.PodfileGenerationEnabled = this.podfileGenerationEnabled;
				IOSResolver.CocoapodsInstallEnabled = this.cocoapodsInstallEnabled;
				IOSResolver.PodToolExecutionViaShellEnabled = this.podToolExecutionViaShellEnabled;
				IOSResolver.AutoPodToolInstallInEditorEnabled = this.autoPodToolInstallInEditorEnabled;
				IOSResolver.VerboseLoggingEnabled = this.verboseLoggingEnabled;
				IOSResolver.CocoapodsIntegrationMethodPref = IOSResolverSettingsDialog.integrationMapping[this.cocoapodsIntegrationMenuIndex];
				IOSResolver.UseProjectSettings = this.useProjectSettings;
			}
		}

		private IOSResolverSettingsDialog.Settings settings;

		private static string[] cocopodsIntegrationStrings = new string[]
		{
			"Xcode Workspace - Add Cocoapods to the Xcode workspace",
			"Xcode Project - Add Cocoapods to the Xcode project",
			"None - Do not integrate Cocoapods."
		};

		private static IOSResolver.CocoapodsIntegrationMethod[] integrationMapping;

		static IOSResolverSettingsDialog()
		{
			// 注意: 此类型已标记为 'beforefieldinit'.
			IOSResolver.CocoapodsIntegrationMethod[] expr_29 = new IOSResolver.CocoapodsIntegrationMethod[3];
			expr_29[0] = IOSResolver.CocoapodsIntegrationMethod.Workspace;
			expr_29[1] = IOSResolver.CocoapodsIntegrationMethod.Project;
			IOSResolverSettingsDialog.integrationMapping = expr_29;
		}

		private static int FindIndexFromCocoapodsIntegrationMethod(IOSResolver.CocoapodsIntegrationMethod enumToFind)
		{
			for (int i = 0; i < IOSResolverSettingsDialog.integrationMapping.Length; i++)
			{
				if (IOSResolverSettingsDialog.integrationMapping[i] == enumToFind)
				{
					return i;
				}
			}
			throw new ArgumentException("Invalid CocoapodsIntegrationMethod.");
		}

		public void Initialize()
		{
			base.minSize = new Vector2(400f, 370f);
			base.position = new Rect((float)(Screen.width / 3), (float)(Screen.height / 3), base.minSize.x, base.minSize.y);
		}

		private void LoadSettings()
		{
			this.settings = new IOSResolverSettingsDialog.Settings();
		}

		public void OnEnable()
		{
			this.LoadSettings();
		}

		public void OnGUI()
		{
			GUI.skin.label.wordWrap = true;
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Podfile Generation", EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.settings.podfileGenerationEnabled = EditorGUILayout.Toggle(this.settings.podfileGenerationEnabled, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.Label("Podfile generation is required to install Cocoapods.  It may be desirable to disable Podfile generation if frameworks are manually included in Unity's generated Xcode project.", new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Cocoapods Integration", EditorStyles.boldLabel, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.settings.cocoapodsIntegrationMenuIndex = EditorGUILayout.Popup(this.settings.cocoapodsIntegrationMenuIndex, IOSResolverSettingsDialog.cocopodsIntegrationStrings, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (IOSResolverSettingsDialog.integrationMapping[this.settings.cocoapodsIntegrationMenuIndex] != IOSResolver.CocoapodsIntegrationMethod.None && !this.settings.podfileGenerationEnabled)
			{
				GUILayout.Label("Cocoapod installation requires Podfile generation to be enabled.", new GUILayoutOption[0]);
			}
			else if (IOSResolverSettingsDialog.integrationMapping[this.settings.cocoapodsIntegrationMenuIndex] == IOSResolver.CocoapodsIntegrationMethod.Workspace)
			{
				GUILayout.Label("Unity Cloud Build and Unity 5.5 and below do not open generated Xcode workspaces so this plugin will fall back to Xcode Project integration in those environments.", new GUILayoutOption[0]);
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Use Shell to Execute Cocoapod Tool", EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.settings.podToolExecutionViaShellEnabled = EditorGUILayout.Toggle(this.settings.podToolExecutionViaShellEnabled, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			if (this.settings.podToolExecutionViaShellEnabled)
			{
				GUILayout.Label("Shell execution is useful when configuration in the shell environment (e.g ~/.profile) is required to execute Cocoapods tools.", new GUILayoutOption[0]);
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Auto Install Cocoapod Tools in Editor", EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.settings.autoPodToolInstallInEditorEnabled = EditorGUILayout.Toggle(this.settings.autoPodToolInstallInEditorEnabled, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			if (this.settings.autoPodToolInstallInEditorEnabled)
			{
				GUILayout.Label("Automatically installs the Cocoapod tool if the editor isn't running in batch mode", new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label("Cocoapod tool installation can be performed via the menu option: Assets > Play Services Resolver > iOS Resolver > Install Cocoapods", new GUILayoutOption[0]);
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Verbose Logging", EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.settings.verboseLoggingEnabled = EditorGUILayout.Toggle(this.settings.verboseLoggingEnabled, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Use project settings", EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.settings.useProjectSettings = EditorGUILayout.Toggle(this.settings.useProjectSettings, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			if (GUILayout.Button("Reset to Defaults", new GUILayoutOption[0]))
			{
				IOSResolverSettingsDialog.Settings settings = new IOSResolverSettingsDialog.Settings();
				IOSResolver.RestoreDefaultSettings();
				this.LoadSettings();
				settings.Save();
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			bool flag = GUILayout.Button("Cancel", new GUILayoutOption[0]);
			bool flag2 = GUILayout.Button("OK", new GUILayoutOption[0]);
			flag |= flag2;
			if (flag2)
			{
				this.settings.Save();
			}
			if (flag)
			{
				base.Close();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
	}
}
