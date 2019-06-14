using System;
using System.Globalization;
using UnityEngine;

namespace Google
{
	internal class ExecutionEnvironment
	{
		private const float DEFAULT_UNITY_VERSION_MAJOR_MINOR = 5.4f;

		private static float unityVersionMajorMinor = -1f;

		public static bool InBatchMode
		{
			get
			{
				return Environment.CommandLine.Contains("-batchmode");
			}
		}

		public static float VersionMajorMinor
		{
			get
			{
				if (ExecutionEnvironment.unityVersionMajorMinor >= 0f)
				{
					return ExecutionEnvironment.unityVersionMajorMinor;
				}
				float result = 5.4f;
				string unityVersion = Application.unityVersion;
				if (!string.IsNullOrEmpty(unityVersion))
				{
					int num = unityVersion.IndexOf('.');
					if (num > 0 && unityVersion.Length > num + 1 && !float.TryParse(unityVersion.Substring(0, num + 2), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
					{
						result = 5.4f;
					}
				}
				ExecutionEnvironment.unityVersionMajorMinor = result;
				return result;
			}
		}
	}
}
