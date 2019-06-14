using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Google
{
	[InitializeOnLoad]
	internal class RunOnMainThread
	{
		private class ScheduledJob
		{
			private static Dictionary<int, RunOnMainThread.ScheduledJob> scheduledJobs = new Dictionary<int, RunOnMainThread.ScheduledJob>();

			private static int nextJobId = 1;

			private Action Job;

			private int JobId;

			private double DelayInMilliseconds;

			private DateTime scheduledTime = DateTime.Now;

			public static int Schedule(Action job, double delayInMilliseconds)
			{
				object obj = RunOnMainThread.ScheduledJob.scheduledJobs;
				RunOnMainThread.ScheduledJob scheduledJob;
				lock (obj)
				{
					scheduledJob = new RunOnMainThread.ScheduledJob
					{
						Job = job,
						JobId = RunOnMainThread.ScheduledJob.nextJobId,
						DelayInMilliseconds = (!ExecutionEnvironment.InBatchMode) ? delayInMilliseconds : 0.0
					};
					RunOnMainThread.ScheduledJob.scheduledJobs[RunOnMainThread.ScheduledJob.nextJobId++] = scheduledJob;
					if (RunOnMainThread.ScheduledJob.nextJobId == 0)
					{
						RunOnMainThread.ScheduledJob.nextJobId++;
					}
				}
				RunOnMainThread.PollOnUpdateUntilComplete(new Func<bool>(scheduledJob.PollUntilExecutionTime));
				return scheduledJob.JobId;
			}

			public static void Cancel(int jobId)
			{
				object obj = RunOnMainThread.ScheduledJob.scheduledJobs;
				lock (obj)
				{
					RunOnMainThread.ScheduledJob scheduledJob;
					if (RunOnMainThread.ScheduledJob.scheduledJobs.TryGetValue(jobId, out scheduledJob))
					{
						scheduledJob.Dequeue();
					}
				}
			}

			private Action Dequeue()
			{
				object obj = RunOnMainThread.ScheduledJob.scheduledJobs;
				Action job;
				lock (obj)
				{
					RunOnMainThread.ScheduledJob.scheduledJobs.Remove(this.JobId);
					this.JobId = 0;
					job = this.Job;
					this.Job = null;
				}
				return job;
			}

			public bool PollUntilExecutionTime()
			{
				if (DateTime.Now.Subtract(this.scheduledTime).TotalMilliseconds < this.DelayInMilliseconds)
				{
					return false;
				}
				Action action = this.Dequeue();
				if (action != null)
				{
					action();
				}
				return true;
			}
		}

		private static int mainThreadId;

		private static Queue<Action> jobs;

		private static List<Func<bool>> pollingJobs;

		private static List<Func<bool>> completePollingJobs;

		private static bool runningJobs;

		[CompilerGenerated]
		private static EditorApplication.CallbackFunction callbackFunction;

		public static event EditorApplication.CallbackFunction OnUpdate
		{
			add
			{
				RunOnMainThread.AddOnUpdateCallback(value);
			}
			remove
			{
				RunOnMainThread.RemoveOnUpdateCallback(value);
			}
		}

		private static bool OnMainThread
		{
			get
			{
				return RunOnMainThread.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		public static bool ExecuteNow
		{
			get
			{
				return ExecutionEnvironment.InBatchMode && !RunOnMainThread.runningJobs;
			}
		}

		static RunOnMainThread()
		{
			RunOnMainThread.jobs = new Queue<Action>();
			RunOnMainThread.pollingJobs = new List<Func<bool>>();
			RunOnMainThread.completePollingJobs = new List<Func<bool>>();
			RunOnMainThread.runningJobs = false;
			RunOnMainThread.mainThreadId = Thread.CurrentThread.ManagedThreadId;
			if (!ExecutionEnvironment.InBatchMode)
			{
				if (RunOnMainThread.callbackFunction == null)
				{
					RunOnMainThread.callbackFunction = new EditorApplication.CallbackFunction(RunOnMainThread.ExecuteAll);
				}
				RunOnMainThread.OnUpdate += RunOnMainThread.callbackFunction;
			}
		}

		private static void AddOnUpdateCallback(EditorApplication.CallbackFunction callback)
		{
			RunOnMainThread.Run(delegate
			{
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, callback);
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, callback);
				if (ExecutionEnvironment.InBatchMode)
				{
					callback();
				}
			}, true);
		}

		private static void RemoveOnUpdateCallback(EditorApplication.CallbackFunction callback)
		{
			RunOnMainThread.Run(delegate
			{
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, callback);
			}, true);
		}

		private static void RunAction(Action action)
		{
			RunOnMainThread.runningJobs = true;
			try
			{
				action();
			}
			finally
			{
				RunOnMainThread.runningJobs = false;
			}
		}

		public static void PollOnUpdateUntilComplete(Func<bool> condition)
		{
			object obj = RunOnMainThread.pollingJobs;
			lock (obj)
			{
				RunOnMainThread.pollingJobs.Add(condition);
			}
			if (RunOnMainThread.ExecuteNow && RunOnMainThread.OnMainThread)
			{
				RunOnMainThread.RunAction(delegate
				{
					while (true)
					{
						RunOnMainThread.ExecuteAll();
						object obj2 = RunOnMainThread.pollingJobs;
						lock (obj2)
						{
							if (RunOnMainThread.pollingJobs.Count == 0)
							{
								break;
							}
						}
						Thread.Sleep(100);
					}
				});
			}
		}

		private static int ExecutePollingJobs()
		{
			bool flag = false;
			int num = 0;
			int num2;
			while (true)
			{
				object obj = RunOnMainThread.pollingJobs;
				Func<bool> func;
				lock (obj)
				{
					num2 = RunOnMainThread.pollingJobs.Count;
					if (num >= num2)
					{
						break;
					}
					func = RunOnMainThread.pollingJobs[num];
				}
				bool flag2 = false;
				try
				{
					flag2 = func();
				}
				catch (Exception ex)
				{
					flag2 = true;
					Debug.LogError(string.Format("Stopped polling job due to exception: {0}", ex.ToString()));
				}
				if (flag2)
				{
					RunOnMainThread.completePollingJobs.Add(func);
					flag = true;
				}
				num++;
			}
			if (flag)
			{
				object obj2 = RunOnMainThread.pollingJobs;
				lock (obj2)
				{
					foreach (Func<bool> current in RunOnMainThread.completePollingJobs)
					{
						if (RunOnMainThread.pollingJobs.Remove(current))
						{
							num2--;
						}
					}
				}
				RunOnMainThread.completePollingJobs.Clear();
			}
			return num2;
		}

		public static int Schedule(Action job, double delayInMilliseconds)
		{
			return RunOnMainThread.ScheduledJob.Schedule(job, delayInMilliseconds);
		}

		public static void Cancel(int jobId)
		{
			RunOnMainThread.ScheduledJob.Cancel(jobId);
		}

		public static void Run(Action job, bool runNow = true)
		{
			object obj = RunOnMainThread.jobs;
			bool flag;
			lock (obj)
			{
				flag = (RunOnMainThread.jobs.Count == 0);
				RunOnMainThread.jobs.Enqueue(job);
			}
			if ((flag || RunOnMainThread.ExecuteNow) && runNow && RunOnMainThread.OnMainThread)
			{
				RunOnMainThread.ExecuteAll();
			}
		}

		private static bool ExecuteNext()
		{
			Action action = null;
			object obj = RunOnMainThread.jobs;
			lock (obj)
			{
				if (RunOnMainThread.jobs.Count > 0)
				{
					action = RunOnMainThread.jobs.Dequeue();
				}
			}
			if (action == null)
			{
				return false;
			}
			try
			{
				action();
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("Job failed with exception: {0}", ex.ToString()));
			}
			return true;
		}

		public static bool TryExecuteAll()
		{
			if (RunOnMainThread.OnMainThread)
			{
				RunOnMainThread.ExecuteAll();
				return true;
			}
			return false;
		}

		private static void ExecuteAll()
		{
			if (!RunOnMainThread.OnMainThread)
			{
				Debug.LogError("ExecuteAll must be executed from the main thread.");
				return;
			}
			RunOnMainThread.RunAction(delegate
			{
				while (RunOnMainThread.ExecuteNext())
				{
				}
				int num;
				do
				{
					num = RunOnMainThread.ExecutePollingJobs();
				}
				while (num > 0 && ExecutionEnvironment.InBatchMode);
			});
		}
	}
}
