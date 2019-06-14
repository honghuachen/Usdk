using System;
using System.Globalization;

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
	}
}
