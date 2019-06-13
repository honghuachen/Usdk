using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GooglePlayServices
{
	public class TextAreaDialog : EditorWindow
	{
		public delegate void ButtonClicked(TextAreaDialog dialog);

		public TextAreaDialog.ButtonClicked buttonClicked;

		public bool modal = true;

		public string summaryText = string.Empty;

		public string yesText = string.Empty;

		public string noText = string.Empty;

		public string bodyText = string.Empty;

		public bool result;

		private bool yesNoClicked;

		public Vector2 scrollPosition;

		public static TextAreaDialog CreateTextAreaDialog(string title)
		{
			TextAreaDialog textAreaDialog = (TextAreaDialog)EditorWindow.GetWindow(typeof(TextAreaDialog), true, title, true);
			textAreaDialog.Initialize();
			return textAreaDialog;
		}

		public virtual void Initialize()
		{
			this.yesText = string.Empty;
			this.noText = string.Empty;
			this.summaryText = string.Empty;
			this.bodyText = string.Empty;
			this.result = false;
			this.yesNoClicked = false;
			this.scrollPosition = new Vector2(0f, 0f);
			base.minSize = new Vector2(300f, 200f);
			base.position = new Rect((float)(Screen.width / 3), (float)(Screen.height / 3), base.minSize.x * 2f, base.minSize.y * 2f);
		}

		protected virtual void OnGUI()
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label(this.summaryText, EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, new GUILayoutOption[0]);
			int i = 0;
			List<string> list = new List<string>();
			while (i < this.bodyText.Length)
			{
				int num = 5000;
				num = ((i + num < this.bodyText.Length) ? num : (this.bodyText.Length - i));
				list.Add(this.bodyText.Substring(i, num));
				i += num;
			}
			foreach (string current in list)
			{
				float height = EditorStyles.wordWrappedLabel.CalcHeight(new GUIContent(current), base.position.width);
				EditorGUILayout.SelectableLabel(current, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
				{
					GUILayout.Height(height)
				});
			}
			GUILayout.EndScrollView();
			bool flag = false;
			bool flag2 = false;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (this.yesText != string.Empty)
			{
				flag = GUILayout.Button(this.yesText, new GUILayoutOption[0]);
			}
			if (this.noText != string.Empty)
			{
				flag2 = GUILayout.Button(this.noText, new GUILayoutOption[0]);
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			if (flag || flag2)
			{
				this.yesNoClicked = true;
				if (flag)
				{
					this.result = true;
				}
				else if (flag2)
				{
					this.result = false;
				}
				if (this.buttonClicked != null)
				{
					this.buttonClicked(this);
				}
			}
		}

		protected virtual void OnLostFocus()
		{
			if (this.modal)
			{
				base.Focus();
			}
		}

		protected virtual void OnDestroy()
		{
			if (!this.yesNoClicked)
			{
				this.result = false;
				if (this.buttonClicked != null)
				{
					this.buttonClicked(this);
				}
			}
		}
	}
}
