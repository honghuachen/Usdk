using System.IO;
using System;

namespace Google
{
	internal enum LogLevel
	{
		Debug,
		Verbose,
		Info,
		Warning,
		Error
	}
	
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
					Console.WriteLine (message);
					this.LogToFile(message);
					break;
				case LogLevel.Warning:
					Console.WriteLine ("WARNING: "+ message);
					this.LogToFile("WARNING: " + message);
					break;
				case LogLevel.Error:
					Console.WriteLine ("ERROR: " + message);
					this.LogToFile("ERROR: " + message);
					break;
				}
			}
		}
	}
}
