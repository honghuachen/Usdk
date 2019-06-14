using System;
using System.IO;
using UnityEngine;

namespace Google
{
	internal class Logger
	{
		internal LogLevel Level
		{
			get;
			set;
		}

		internal bool Verbose
		{
			get
			{
				return this.Level <= LogLevel.Verbose;
			}
			set
			{
				this.Level = ((!value) ? LogLevel.Info : LogLevel.Verbose);
			}
		}

		internal string LogFilename
		{
			get;
			set;
		}

		internal Logger()
		{
		}

		private void LogToFile(string message)
		{
			if (this.LogFilename != null)
			{
				using (StreamWriter streamWriter = new StreamWriter(this.LogFilename, true))
				{
					streamWriter.WriteLine(message);
				}
			}
		}

		internal virtual void Log(string message, LogLevel level = LogLevel.Info)
		{
			if (level >= this.Level || ExecutionEnvironment.InBatchMode)
			{
				switch (level)
				{
				case LogLevel.Debug:
				case LogLevel.Verbose:
				case LogLevel.Info:
					Debug.Log(message);
					this.LogToFile(message);
					break;
				case LogLevel.Warning:
					Debug.LogWarning(message);
					this.LogToFile("WARNING: " + message);
					break;
				case LogLevel.Error:
					Debug.LogError(message);
					this.LogToFile("ERROR: " + message);
					break;
				}
			}
		}
	}
}
