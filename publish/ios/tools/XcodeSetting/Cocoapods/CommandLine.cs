using Google;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace GooglePlayServices
{
	public static class CommandLine
	{
		public class Result
		{
			public string stdout;

			public string stderr;

			public int exitCode;

			public string message;
		}

		public delegate void CompletionHandler(CommandLine.Result result);

		public class StreamData
		{
			public int handle;

			public string text = string.Empty;

			public byte[] data;

			public bool end;

			public static CommandLine.StreamData Empty
			{
				get
				{
					return new CommandLine.StreamData(0, string.Empty, null, false);
				}
			}

			public StreamData(int handle, string text, byte[] data, bool end)
			{
				this.handle = handle;
				this.text = text;
				this.data = data;
				this.end = end;
			}
		}

		public delegate void IOHandler(Process process, StreamWriter stdin, CommandLine.StreamData streamData);

		private class AsyncStreamReader
		{
			public delegate void Handler(CommandLine.StreamData streamData);

			private AutoResetEvent readEvent;

			private int handle;

			private Stream stream;

			private byte[] buffer;

			private volatile bool complete;
			public event CommandLine.AsyncStreamReader.Handler DataReceived;
			// public event CommandLine.AsyncStreamReader.Handler DataReceived
			// {
			// 	add
			// 	{
			// 		CommandLine.AsyncStreamReader.Handler handler = this.DataReceived;
			// 		CommandLine.AsyncStreamReader.Handler handler2;
			// 		do
			// 		{
			// 			handler2 = handler;
			// 			handler = Interlocked.CompareExchange<CommandLine.AsyncStreamReader.Handler>(ref this.DataReceived, (CommandLine.AsyncStreamReader.Handler)Delegate.Combine(handler2, value), handler);
			// 		}
			// 		while (handler != handler2);
			// 	}
			// 	remove
			// 	{
			// 		CommandLine.AsyncStreamReader.Handler handler = this.DataReceived;
			// 		CommandLine.AsyncStreamReader.Handler handler2;
			// 		do
			// 		{
			// 			handler2 = handler;
			// 			handler = Interlocked.CompareExchange<CommandLine.AsyncStreamReader.Handler>(ref this.DataReceived, (CommandLine.AsyncStreamReader.Handler)Delegate.Remove(handler2, value), handler);
			// 		}
			// 		while (handler != handler2);
			// 	}
			// }

			public int Handle
			{
				get
				{
					return this.handle;
				}
			}

			public AsyncStreamReader(int handle, Stream stream, int bufferSize)
			{
				this.readEvent = new AutoResetEvent(false);
				this.handle = handle;
				this.stream = stream;
				this.buffer = new byte[bufferSize];
			}

			public void Start()
			{
				if (!this.complete)
				{
					new Thread(new ThreadStart(this.Read)).Start();
				}
			}

			private void Read()
			{
				while (!this.complete)
				{
					this.stream.BeginRead(this.buffer, 0, this.buffer.Length, delegate(IAsyncResult asyncResult)
					{
						int num = this.stream.EndRead(asyncResult);
						if (!this.complete)
						{
							this.complete = (num == 0);
							if (this.DataReceived != null)
							{
								byte[] array = new byte[num];
								Array.Copy(this.buffer, array, array.Length);
								this.DataReceived(new CommandLine.StreamData(this.handle, Encoding.UTF8.GetString(array), array, this.complete));
							}
						}
						this.readEvent.Set();
					}, null);
					this.readEvent.WaitOne();
				}
			}

			public static CommandLine.AsyncStreamReader[] CreateFromStreams(Stream[] streams, int bufferSize)
			{
				CommandLine.AsyncStreamReader[] array = new CommandLine.AsyncStreamReader[streams.Length];
				for (int i = 0; i < streams.Length; i++)
				{
					array[i] = new CommandLine.AsyncStreamReader(i, streams[i], bufferSize);
				}
				return array;
			}
		}

		private class AsyncStreamReaderMultiplexer
		{
			public delegate void CompletionHandler();

			private AutoResetEvent queuedItem;

			private Queue queue;

			private HashSet<int> activeStreams;
			public event CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler Complete;

			// public event CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler Complete
			// {
			// 	add
			// 	{
			// 		CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler completionHandler = this.Complete;
			// 		CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler completionHandler2;
			// 		do
			// 		{
			// 			completionHandler2 = completionHandler;
			// 			completionHandler = Interlocked.CompareExchange<CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler>(ref this.Complete, (CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler)Delegate.Combine(completionHandler2, value), completionHandler);
			// 		}
			// 		while (completionHandler != completionHandler2);
			// 	}
			// 	remove
			// 	{
			// 		CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler completionHandler = this.Complete;
			// 		CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler completionHandler2;
			// 		do
			// 		{
			// 			completionHandler2 = completionHandler;
			// 			completionHandler = Interlocked.CompareExchange<CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler>(ref this.Complete, (CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler)Delegate.Remove(completionHandler2, value), completionHandler);
			// 		}
			// 		while (completionHandler != completionHandler2);
			// 	}
			// }

			public event CommandLine.AsyncStreamReader.Handler DataReceived;
			// public event CommandLine.AsyncStreamReader.Handler DataReceived
			// {
			// 	add
			// 	{
			// 		CommandLine.AsyncStreamReader.Handler handler = this.DataReceived;
			// 		CommandLine.AsyncStreamReader.Handler handler2;
			// 		do
			// 		{
			// 			handler2 = handler;
			// 			handler = Interlocked.CompareExchange<CommandLine.AsyncStreamReader.Handler>(ref this.DataReceived, (CommandLine.AsyncStreamReader.Handler)Delegate.Combine(handler2, value), handler);
			// 		}
			// 		while (handler != handler2);
			// 	}
			// 	remove
			// 	{
			// 		CommandLine.AsyncStreamReader.Handler handler = this.DataReceived;
			// 		CommandLine.AsyncStreamReader.Handler handler2;
			// 		do
			// 		{
			// 			handler2 = handler;
			// 			handler = Interlocked.CompareExchange<CommandLine.AsyncStreamReader.Handler>(ref this.DataReceived, (CommandLine.AsyncStreamReader.Handler)Delegate.Remove(handler2, value), handler);
			// 		}
			// 		while (handler != handler2);
			// 	}
			// }

			public AsyncStreamReaderMultiplexer(CommandLine.AsyncStreamReader[] readers, CommandLine.AsyncStreamReader.Handler handler, CommandLine.AsyncStreamReaderMultiplexer.CompletionHandler complete = null)
			{
				this.queuedItem = new AutoResetEvent(false);
				this.queue = Queue.Synchronized(new Queue());
				this.activeStreams = new HashSet<int>();
				for (int i = 0; i < readers.Length; i++)
				{
					CommandLine.AsyncStreamReader asyncStreamReader = readers[i];
					asyncStreamReader.DataReceived += new CommandLine.AsyncStreamReader.Handler(this.HandleRead);
					this.activeStreams.Add(asyncStreamReader.Handle);
				}
				this.DataReceived += handler;
				if (complete != null)
				{
					this.Complete += complete;
				}
				new Thread(new ThreadStart(this.PollQueue)).Start();
			}

			public void Shutdown()
			{
				object obj = this.activeStreams;
				lock (obj)
				{
					this.activeStreams.Clear();
				}
				this.queuedItem.Set();
			}

			private void HandleRead(CommandLine.StreamData streamData)
			{
				this.queue.Enqueue(streamData);
				this.queuedItem.Set();
			}

			private void PollQueue()
			{
				while (this.activeStreams.Count > 0)
				{
					this.queuedItem.WaitOne();
					while (this.queue.Count > 0)
					{
						CommandLine.StreamData streamData = (CommandLine.StreamData)this.queue.Dequeue();
						if (streamData.end)
						{
							object obj = this.activeStreams;
							lock (obj)
							{
								this.activeStreams.Remove(streamData.handle);
							}
						}
						if (this.DataReceived != null)
						{
							this.DataReceived(streamData);
						}
					}
				}
				if (this.Complete != null)
				{
					this.Complete();
				}
			}
		}

		public class LineReader
		{
			private Dictionary<int, List<CommandLine.StreamData>> streamDataByHandle = new Dictionary<int, List<CommandLine.StreamData>>();
			public event CommandLine.IOHandler LineHandler;

			// public event CommandLine.IOHandler LineHandler
			// {
			// 	add
			// 	{
			// 		CommandLine.IOHandler iOHandler = this.LineHandler;
			// 		CommandLine.IOHandler iOHandler2;
			// 		do
			// 		{
			// 			iOHandler2 = iOHandler;
			// 			iOHandler = Interlocked.CompareExchange<CommandLine.IOHandler>(ref this.LineHandler, (CommandLine.IOHandler)Delegate.Combine(iOHandler2, value), iOHandler);
			// 		}
			// 		while (iOHandler != iOHandler2);
			// 	}
			// 	remove
			// 	{
			// 		CommandLine.IOHandler iOHandler = this.LineHandler;
			// 		CommandLine.IOHandler iOHandler2;
			// 		do
			// 		{
			// 			iOHandler2 = iOHandler;
			// 			iOHandler = Interlocked.CompareExchange<CommandLine.IOHandler>(ref this.LineHandler, (CommandLine.IOHandler)Delegate.Remove(iOHandler2, value), iOHandler);
			// 		}
			// 		while (iOHandler != iOHandler2);
			// 	}
			// }
			public event CommandLine.IOHandler DataHandler;

			// public event CommandLine.IOHandler DataHandler
			// {
			// 	add
			// 	{
			// 		CommandLine.IOHandler iOHandler = this.DataHandler;
			// 		CommandLine.IOHandler iOHandler2;
			// 		do
			// 		{
			// 			iOHandler2 = iOHandler;
			// 			iOHandler = Interlocked.CompareExchange<CommandLine.IOHandler>(ref this.DataHandler, (CommandLine.IOHandler)Delegate.Combine(iOHandler2, value), iOHandler);
			// 		}
			// 		while (iOHandler != iOHandler2);
			// 	}
			// 	remove
			// 	{
			// 		CommandLine.IOHandler iOHandler = this.DataHandler;
			// 		CommandLine.IOHandler iOHandler2;
			// 		do
			// 		{
			// 			iOHandler2 = iOHandler;
			// 			iOHandler = Interlocked.CompareExchange<CommandLine.IOHandler>(ref this.DataHandler, (CommandLine.IOHandler)Delegate.Remove(iOHandler2, value), iOHandler);
			// 		}
			// 		while (iOHandler != iOHandler2);
			// 	}
			// }

			public LineReader(CommandLine.IOHandler handler = null)
			{
				if (handler != null)
				{
					this.LineHandler += handler;
				}
			}

			public List<CommandLine.StreamData> GetBufferedData(int handle)
			{
				List<CommandLine.StreamData> collection;
				return (!this.streamDataByHandle.TryGetValue(handle, out collection)) ? new List<CommandLine.StreamData>() : new List<CommandLine.StreamData>(collection);
			}

			public void Flush()
			{
				foreach (List<CommandLine.StreamData> current in this.streamDataByHandle.Values)
				{
					current.Clear();
				}
			}

			public static CommandLine.StreamData Aggregate(List<CommandLine.StreamData> dataStream)
			{
				List<string> list = new List<string>();
				int num = 0;
				int handle = 0;
				bool flag = false;
				foreach (CommandLine.StreamData current in dataStream)
				{
					list.Add(current.text);
					num += current.data.Length;
					handle = current.handle;
					flag |= current.end;
				}
				string text = string.Join(string.Empty, list.ToArray());
				byte[] array = new byte[num];
				int num2 = 0;
				foreach (CommandLine.StreamData current2 in dataStream)
				{
					Array.Copy(current2.data, 0, array, num2, current2.data.Length);
					num2 += current2.data.Length;
				}
				return new CommandLine.StreamData(handle, text, array, flag);
			}

			public void AggregateLine(Process process, StreamWriter stdin, CommandLine.StreamData data)
			{
				if (this.DataHandler != null)
				{
					this.DataHandler(process, stdin, data);
				}
				bool flag = false;
				if (data.data != null)
				{
					data.text = data.text.Replace("\r\n", "\n").Replace("\r", "\n");
					List<CommandLine.StreamData> list = this.GetBufferedData(data.handle);
					list.Add(data);
					bool flag2 = false;
					while (!flag2)
					{
						List<CommandLine.StreamData> list2 = new List<CommandLine.StreamData>();
						int num = 0;
						int count = list.Count;
						flag2 = true;
						foreach (CommandLine.StreamData current in list)
						{
							bool flag3 = data.end && ++num == count;
							list2.Add(current);
							int num2 = current.text.Length;
							if (!flag3)
							{
								num2 = current.text.IndexOf("\n");
								if (num2 < 0)
								{
									continue;
								}
								list2.Remove(current);
							}
							CommandLine.StreamData streamData = CommandLine.LineReader.Aggregate(list2);
							list2.Clear();
							if (!flag3)
							{
								CommandLine.StreamData expr_10C = streamData;
								expr_10C.text += current.text.Substring(0, num2 + 1);
								list2.Add(new CommandLine.StreamData(data.handle, current.text.Substring(num2 + 1), current.data, false));
								flag2 = false;
							}
							if (this.LineHandler != null)
							{
								this.LineHandler(process, stdin, streamData);
							}
							flag = true;
						}
						list = list2;
					}
					this.streamDataByHandle[data.handle] = list;
				}
				if (!flag && this.LineHandler != null)
				{
					this.LineHandler(process, stdin, CommandLine.StreamData.Empty);
				}
			}
		}

		public static void RunAsync(string toolPath, string arguments, CommandLine.CompletionHandler completionDelegate, string workingDirectory = null, Dictionary<string, string> envVars = null, CommandLine.IOHandler ioHandler = null)
		{
			Action action = delegate
			{
				CommandLine.Result result = CommandLine.Run(toolPath, arguments, workingDirectory, envVars, ioHandler);
				completionDelegate(result);
			};
			if (ExecutionEnvironment.InBatchMode)
			{
				action();
			}
			else
			{
				Thread thread = new Thread(new ThreadStart(action.Invoke));
				thread.Start();
			}
		}

		public static CommandLine.Result Run(string toolPath, string arguments, string workingDirectory = null, Dictionary<string, string> envVars = null, CommandLine.IOHandler ioHandler = null)
		{
			return CommandLine.RunViaShell(toolPath, arguments, workingDirectory, envVars, ioHandler, false, true);
		}

		public static CommandLine.Result RunViaShell(string toolPath, string arguments, string workingDirectory = null, Dictionary<string, string> envVars = null, CommandLine.IOHandler ioHandler = null, bool useShellExecution = false, bool stdoutRedirectionInShellMode = true)
		{
			Encoding inputEncoding = Console.InputEncoding;
			Encoding outputEncoding = Console.OutputEncoding;
			Console.InputEncoding = Encoding.UTF8;
			Console.OutputEncoding = Encoding.UTF8;
			// if (Application.platform == RuntimePlatform.WindowsEditor && toolPath.Contains("'"))
			// {
			// 	useShellExecution = true;
			// 	stdoutRedirectionInShellMode = true;
			// }
			string text = null;
			string text2 = null;
			if (useShellExecution && stdoutRedirectionInShellMode)
			{
				text = Path.GetTempFileName();
				text2 = Path.GetTempFileName();
				string text3 = toolPath;
				string text4;
				string text5;
				string text6;
				// if (Application.platform == RuntimePlatform.WindowsEditor)
				// {
				// 	text4 = "cmd.exe";
				// 	text5 = "/c \"";
				// 	text6 = "\"";
				// }
				// else
				{
					text4 = "bash";
					text5 = "-l -c '";
					text6 = "'";
					text3 = toolPath.Replace("'", "'\\''");
				}
				arguments = string.Format("{0}\"{1}\" {2} 1> {3} 2> {4}{5}", new object[]
				{
					text5,
					text3,
					arguments,
					text,
					text2,
					text6
				});
				toolPath = text4;
			}
			Process process = new Process();
			process.StartInfo.UseShellExecute = useShellExecution;
			process.StartInfo.Arguments = arguments;
			if (useShellExecution)
			{
				process.StartInfo.CreateNoWindow = false;
				process.StartInfo.RedirectStandardOutput = false;
				process.StartInfo.RedirectStandardError = false;
			}
			else
			{
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				if (envVars != null)
				{
					foreach (KeyValuePair<string, string> current in envVars)
					{
						process.StartInfo.EnvironmentVariables[current.Key] = current.Value;
					}
				}
			}
			process.StartInfo.RedirectStandardInput = (!useShellExecution && ioHandler != null);
			process.StartInfo.FileName = toolPath;
			process.StartInfo.WorkingDirectory = (workingDirectory ?? Environment.CurrentDirectory);
			process.Start();
			if (ioHandler != null)
			{
				ioHandler(process, process.StandardInput, CommandLine.StreamData.Empty);
			}
			List<string>[] stdouterr = new List<string>[]
			{
				new List<string>(),
				new List<string>()
			};
			if (useShellExecution)
			{
				process.WaitForExit();
				if (stdoutRedirectionInShellMode)
				{
					stdouterr[0].Add(File.ReadAllText(text));
					stdouterr[1].Add(File.ReadAllText(text2));
					File.Delete(text);
					File.Delete(text2);
				}
			}
			else
			{
				AutoResetEvent complete = new AutoResetEvent(false);
				CommandLine.AsyncStreamReader[] array = CommandLine.AsyncStreamReader.CreateFromStreams(new Stream[]
				{
					process.StandardOutput.BaseStream,
					process.StandardError.BaseStream
				}, 1);
				new CommandLine.AsyncStreamReaderMultiplexer(array, delegate(CommandLine.StreamData data)
				{
					stdouterr[data.handle].Add(data.text);
					if (ioHandler != null)
					{
						ioHandler(process, process.StandardInput, data);
					}
				}, delegate
				{
					complete.Set();
				});
				CommandLine.AsyncStreamReader[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					CommandLine.AsyncStreamReader asyncStreamReader = array2[i];
					asyncStreamReader.Start();
				}
				process.WaitForExit();
				complete.WaitOne();
			}
			CommandLine.Result result = new CommandLine.Result();
			result.stdout = string.Join(string.Empty, stdouterr[0].ToArray());
			result.stderr = string.Join(string.Empty, stdouterr[1].ToArray());
			result.exitCode = process.ExitCode;
			result.message = CommandLine.FormatResultMessage(toolPath, arguments, result.stdout, result.stderr, result.exitCode);
			Console.InputEncoding = inputEncoding;
			Console.OutputEncoding = outputEncoding;
			return result;
		}

		public static string[] SplitLines(string multilineString)
		{
			return Regex.Split(multilineString, "\r\n|\r|\n");
		}

		private static string FormatResultMessage(string toolPath, string arguments, string stdout, string stderr, int exitCode)
		{
			return string.Format("{0} '{1} {2}'\nstdout:\n{3}\nstderr:\n{4}\nexit code: {5}\n", new object[]
			{
				(exitCode != 0) ? "Failed to run" : "Successfully executed",
				toolPath,
				arguments,
				stdout,
				stderr,
				exitCode
			});
		}
	}
}
