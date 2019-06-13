using Google;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor;

namespace GooglePlayServices
{
	internal class CommandLineDialog : TextAreaDialog
	{
		public class ProgressReporter : CommandLine.LineReader
		{
			public int maxProgressLines;

			private Queue textQueue;

			private volatile int linesReported;

			private volatile CommandLine.Result result;

			private Logger logger;
			public event CommandLine.CompletionHandler Complete;

			// public event CommandLine.CompletionHandler Complete
			// {
			// 	add
			// 	{
			// 		CommandLine.CompletionHandler completionHandler = this.Complete;
			// 		CommandLine.CompletionHandler completionHandler2;
			// 		do
			// 		{
			// 			completionHandler2 = completionHandler;
			// 			completionHandler = Interlocked.CompareExchange<CommandLine.CompletionHandler>(ref this.Complete, (CommandLine.CompletionHandler)Delegate.Combine(completionHandler2, value), completionHandler);
			// 		}
			// 		while (completionHandler != completionHandler2);
			// 	}
			// 	remove
			// 	{
			// 		CommandLine.CompletionHandler completionHandler = this.Complete;
			// 		CommandLine.CompletionHandler completionHandler2;
			// 		do
			// 		{
			// 			completionHandler2 = completionHandler;
			// 			completionHandler = Interlocked.CompareExchange<CommandLine.CompletionHandler>(ref this.Complete, (CommandLine.CompletionHandler)Delegate.Remove(completionHandler2, value), completionHandler);
			// 		}
			// 		while (completionHandler != completionHandler2);
			// 	}
			// }

			public ProgressReporter(Logger logger) : base(null)
			{
				this.textQueue = Queue.Synchronized(new Queue());
				this.maxProgressLines = 0;
				this.linesReported = 0;
				base.LineHandler += new CommandLine.IOHandler(this.CommandLineIOHandler);
				this.logger = logger;
				this.Complete = null;
			}

			private int CountLines(string str)
			{
				return str.Split(new char[]
				{
					'\n',
					'\r'
				}).Length - 1;
			}

			private void CommandLineIOHandler(Process process, StreamWriter stdin, CommandLine.StreamData data)
			{
				if (process.HasExited || data.data == null)
				{
					return;
				}
				if (data.handle == 0)
				{
					this.linesReported += this.CountLines(data.text);
				}
				string @string = Encoding.UTF8.GetString(data.data);
				this.textQueue.Enqueue(@string);
				string[] array = CommandLine.SplitLines(@string);
				for (int i = 0; i < array.Length; i++)
				{
					string message = array[i];
					this.logger.Log(message, LogLevel.Verbose);
				}
			}

			public void CommandLineToolCompletion(CommandLine.Result result)
			{
				this.logger.Log(string.Format("Command completed: {0}", result.message), LogLevel.Verbose);
				this.result = result;
				this.SignalComplete();
			}

			private void SignalComplete()
			{
				if (this.Complete != null)
				{
					this.Complete(this.result);
					this.Complete = null;
				}
			}

			public void Update(CommandLineDialog window)
			{
				if (this.textQueue.Count > 0)
				{
					List<string> list = new List<string>();
					while (this.textQueue.Count > 0)
					{
						list.Add((string)this.textQueue.Dequeue());
					}
					string text = window.bodyText + string.Join(string.Empty, list.ToArray());
					while (true)
					{
						int num = text.LastIndexOf("\r");
						if (num < 0 || text.Substring(num, 1) == "\n")
						{
							break;
						}
						string str = string.Empty;
						int num2 = text.LastIndexOf("\n", num, num);
						if (num2 >= 0)
						{
							str = text.Substring(0, num2 + 1);
						}
						text = str + text.Substring(num + 1);
					}
					window.bodyText = text;
					if (window.autoScrollToBottom)
					{
						window.scrollPosition.y = float.PositiveInfinity;
					}
					window.Repaint();
				}
				if (this.maxProgressLines > 0)
				{
					window.progress = (float)this.linesReported / (float)this.maxProgressLines;
				}
				if (this.result != null)
				{
					window.progressTitle = string.Empty;
					this.SignalComplete();
				}
			}
		}

		public delegate void UpdateDelegate(CommandLineDialog window);

		public volatile float progress;

		public string progressTitle;

		public string progressSummary;

		public volatile bool autoScrollToBottom;

		public Logger logger = new Logger();

		private bool progressBarVisible;
		public event CommandLineDialog.UpdateDelegate UpdateEvent;

		// public event CommandLineDialog.UpdateDelegate UpdateEvent
		// {
		// 	add
		// 	{
		// 		CommandLineDialog.UpdateDelegate updateDelegate = this.UpdateEvent;
		// 		CommandLineDialog.UpdateDelegate updateDelegate2;
		// 		do
		// 		{
		// 			updateDelegate2 = updateDelegate;
		// 			updateDelegate = Interlocked.CompareExchange<CommandLineDialog.UpdateDelegate>(ref this.UpdateEvent, (CommandLineDialog.UpdateDelegate)Delegate.Combine(updateDelegate2, value), updateDelegate);
		// 		}
		// 		while (updateDelegate != updateDelegate2);
		// 	}
		// 	remove
		// 	{
		// 		CommandLineDialog.UpdateDelegate updateDelegate = this.UpdateEvent;
		// 		CommandLineDialog.UpdateDelegate updateDelegate2;
		// 		do
		// 		{
		// 			updateDelegate2 = updateDelegate;
		// 			updateDelegate = Interlocked.CompareExchange<CommandLineDialog.UpdateDelegate>(ref this.UpdateEvent, (CommandLineDialog.UpdateDelegate)Delegate.Remove(updateDelegate2, value), updateDelegate);
		// 		}
		// 		while (updateDelegate != updateDelegate2);
		// 	}
		// }

		public static CommandLineDialog CreateCommandLineDialog(string title)
		{
			CommandLineDialog commandLineDialog = (!ExecutionEnvironment.InBatchMode) ? ((CommandLineDialog)EditorWindow.GetWindow(typeof(CommandLineDialog), true, title)) : new CommandLineDialog();
			commandLineDialog.Initialize();
			return commandLineDialog;
		}

		public new void Show()
		{
			this.Show(false);
		}

		public new void Show(bool immediateDisplay)
		{
			if (!ExecutionEnvironment.InBatchMode)
			{
				base.Show(immediateDisplay);
			}
		}

		public new void Repaint()
		{
			if (!ExecutionEnvironment.InBatchMode)
			{
				base.Repaint();
			}
		}

		public new void Close()
		{
			if (!ExecutionEnvironment.InBatchMode)
			{
				base.Close();
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			this.progress = 0f;
			this.progressTitle = string.Empty;
			this.progressSummary = string.Empty;
			this.UpdateEvent = null;
			this.progressBarVisible = false;
			this.autoScrollToBottom = false;
		}

		public void RunAsync(string toolPath, string arguments, CommandLine.CompletionHandler completionDelegate, string workingDirectory = null, Dictionary<string, string> envVars = null, CommandLine.IOHandler ioHandler = null, int maxProgressLines = 0)
		{
			CommandLineDialog.ProgressReporter reporter = new CommandLineDialog.ProgressReporter(this.logger);
			reporter.maxProgressLines = maxProgressLines;
			this.UpdateEvent += new CommandLineDialog.UpdateDelegate(reporter.Update);
			reporter.Complete += completionDelegate;
			reporter.DataHandler += ioHandler;
			CommandLine.CompletionHandler value = delegate(CommandLine.Result unusedResult)
			{
				this.UpdateEvent -= new CommandLineDialog.UpdateDelegate(reporter.Update);
			};
			reporter.Complete += value;
			this.logger.Log(string.Format("Executing command: {0} {1}", toolPath, arguments), LogLevel.Verbose);
			CommandLine.RunAsync(toolPath, arguments, new CommandLine.CompletionHandler(reporter.CommandLineToolCompletion), workingDirectory, envVars, new CommandLine.IOHandler(reporter.AggregateLine));
		}

		protected virtual void Update()
		{
			if (this.UpdateEvent != null)
			{
				this.UpdateEvent(this);
			}
			if (this.progressTitle != string.Empty)
			{
				this.progressBarVisible = true;
				EditorUtility.DisplayProgressBar(this.progressTitle, this.progressSummary, this.progress);
			}
			else if (this.progressBarVisible)
			{
				this.progressBarVisible = false;
				EditorUtility.ClearProgressBar();
			}
		}

		protected override void OnDestroy()
		{
			if (this.progressBarVisible)
			{
				EditorUtility.ClearProgressBar();
			}
			base.OnDestroy();
		}
	}
}
