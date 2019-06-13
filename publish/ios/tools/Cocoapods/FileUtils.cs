using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Google
{
	internal class FileUtils
	{
		internal const string META_EXTENSION = ".meta";

		public static string ProjectDirectory
		{
			get
			{
				return Directory.GetParent(Path.GetFullPath(Application.dataPath)).FullName;
			}
		}

		public static string ProjectTemporaryDirectory
		{
			get
			{
				return Path.Combine(FileUtils.ProjectDirectory, "Temp");
			}
		}

		public static bool DeleteExistingFileOrDirectory(string path, bool includeMetaFiles = true)
		{
			bool result = false;
			if (includeMetaFiles && !path.EndsWith(".meta"))
			{
				result = FileUtils.DeleteExistingFileOrDirectory(path + ".meta", true);
			}
			if (Directory.Exists(path))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				directoryInfo.Attributes &= ~FileAttributes.ReadOnly;
				string[] fileSystemEntries = Directory.GetFileSystemEntries(path);
				for (int i = 0; i < fileSystemEntries.Length; i++)
				{
					string path2 = fileSystemEntries[i];
					FileUtils.DeleteExistingFileOrDirectory(path2, includeMetaFiles);
				}
				Directory.Delete(path);
				result = true;
			}
			else if (File.Exists(path))
			{
				File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
				File.Delete(path);
				result = true;
			}
			return result;
		}

		public static void CopyDirectory(string sourceDir, string targetDir)
		{
			Func<string, string> func = (string path) => Path.Combine(targetDir, path.Substring(sourceDir.Length + 1));
			string[] directories = Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories);
			for (int i = 0; i < directories.Length; i++)
			{
				string arg = directories[i];
				Directory.CreateDirectory(func(arg));
			}
			string[] files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
			for (int j = 0; j < files.Length; j++)
			{
				string text = files[j];
				if (!text.EndsWith(".meta"))
				{
					File.Copy(text, func(text));
				}
			}
		}

		public static string CreateTemporaryDirectory(bool useSystemTempPath = false, int retry = 100)
		{
			string path = (!useSystemTempPath) ? FileUtils.ProjectTemporaryDirectory : Path.GetTempPath();
			while (retry-- > 0)
			{
				string text = Path.Combine(path, Path.GetRandomFileName());
				if (!File.Exists(text))
				{
					Directory.CreateDirectory(text);
					return text;
				}
			}
			return null;
		}

		public static void MoveDirectory(string sourceDir, string targetDir)
		{
			sourceDir = Path.GetFullPath(sourceDir);
			targetDir = Path.GetFullPath(targetDir);
			FileUtils.DeleteExistingFileOrDirectory(targetDir, true);
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				Directory.CreateDirectory(targetDir);
				FileUtils.CopyDirectory(sourceDir, targetDir);
				FileUtils.DeleteExistingFileOrDirectory(sourceDir, true);
			}
			else
			{
				Directory.Move(sourceDir, targetDir);
			}
		}

		public static void MoveFile(string sourceFile, string targetFile)
		{
			sourceFile = Path.GetFullPath(sourceFile);
			targetFile = Path.GetFullPath(targetFile);
			FileUtils.DeleteExistingFileOrDirectory(targetFile, true);
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				File.Copy(sourceFile, targetFile);
				FileUtils.DeleteExistingFileOrDirectory(sourceFile, true);
			}
			else
			{
				File.Move(sourceFile, targetFile);
			}
		}

		public static string NormalizePathSeparators(string path)
		{
			return (path == null) ? null : path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		}

		public static string FindDirectoryByCaseInsensitivePath(string pathToFind)
		{
			string text = ".";
			string[] array = FileUtils.NormalizePathSeparators(pathToFind).Split(new char[]
			{
				Path.DirectorySeparatorChar
			});
			int num = 0;
			while (num < array.Length && text != null)
			{
				string path = text;
				string strA = array[num];
				string b = array[num].ToLowerInvariant();
				text = null;
				List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
				string[] directories = Directory.GetDirectories(path);
				for (int i = 0; i < directories.Length; i++)
				{
					string text2 = directories[i];
					string fileName = Path.GetFileName(text2);
					if (fileName.ToLowerInvariant() == b)
					{
						list.Add(new KeyValuePair<int, string>(Math.Abs(string.CompareOrdinal(strA, fileName)), (num != 0) ? text2 : Path.GetFileName(text2)));
						break;
					}
				}
				if (list.Count == 0)
				{
					break;
				}
				list.Sort((KeyValuePair<int, string> lhs, KeyValuePair<int, string> rhs) => lhs.Key - rhs.Key);
				text = list[0].Value;
				num++;
			}
			return FileUtils.NormalizePathSeparators(text);
		}
	}
}
