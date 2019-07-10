using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SharpEngine.Library.Threading
{
	public class ThreadManager
	{
		public class ThreadNode
		{
			public int ThreadId;
			public Thread nodeThread;
			public bool IsRunning;

			public virtual void Start()
			{
				nodeThread.Start();
			}
		};

		public class ThreadNodeParam : ThreadNode
		{
			public Object parameters;

			public override void Start()
			{
				nodeThread.Start(this);
			}
		}

		/// <summary>
		/// Minimum amount of time to allow thread to sleep
		/// </summary>
		public static int MinSleepTime = 2;

		/// <summary>
		/// Maximum amount of time to set thread to sleep
		/// </summary>
		public static int MaxSleepTime = 10;

		private static ArrayList regThreads = new ArrayList();
		private static Object _padlock = new Object();

		public static void Close()
		{
			if (regThreads.Count > 0)
			{
				lock (ThreadManager._padlock)
				{
					foreach (ThreadNode node in regThreads)
					{
						node.nodeThread.Abort();
					}
				}
			}
		}

		/// <summary>
		/// Total number of registered threads
		/// </summary>
		private static int TotalThreads
		{
			get
			{
				int cnt = 0;
				if (_masterThread != null)
				{
					cnt++;
				}
				cnt += regThreads.Count;
				return cnt;
			}
		}

		public static int Count
		{
			get
			{
				return TotalThreads;
			}
		}

		public static ThreadNode CreateThread(ThreadStart method)
		{
			ThreadNode node = new ThreadNode()
			{
				ThreadId = TotalThreads,
				IsRunning = ThreadManager.MasterThread.IsRunning,
				nodeThread = new Thread(method)
			};
			lock (ThreadManager._padlock)
			{
				regThreads.Add(node);
			}
			return node;
		}

		public static ThreadNode CreateThread(ParameterizedThreadStart method, Object param)
		{
			// Define thread node that take parameters
			ThreadNodeParam node = new ThreadNodeParam()
			{
				ThreadId = TotalThreads,
				IsRunning = ThreadManager.MasterThread.IsRunning,
				nodeThread = new Thread(method),
				parameters = param
			};
			lock (ThreadManager._padlock)
			{
				regThreads.Add(node);
			}

			return node;
		}

		public static void RemoveThread(ThreadNode node)
		{
			if (regThreads.Contains(node))
			{
				lock (ThreadManager._padlock)
				{
					regThreads.Remove(node);
				}
			}
		}

		private static ThreadNode _masterThread = null;
		/// <summary>
		/// Retrieve master thread node
		/// </summary>
		public static ThreadNode MasterThread
		{
			get
			{
				if (_masterThread == null)
				{
					int threadId = TotalThreads;
					_masterThread = new ThreadNode()
					{
						ThreadId = threadId,
						IsRunning = true
					};
				}

				return _masterThread;
			}
		}

		/// <summary>
		/// Manage thread sleep
		/// </summary>
		/// <param name="time">Amount of time to put thread to sleep in milliseconds</param>
		/// <param name="node">Node thread to sleep</param>
		public static void Sleep(int time, ThreadNode node)
		{
			// Set default sleep time
			int sleepTime = MaxSleepTime;
			// Make sure it evenly divides into time
			while (time % sleepTime != 0)
			{
				// Check if we are getting to fast on sleep time
				if (--sleepTime < MinSleepTime)
				{
					sleepTime = MinSleepTime;
					break;
				}// Endif 
			}// End where time is evenly divided by sleepTime

			// Time to put thread to sleep
			int timeSlept = 0;
			do
			{
				Thread.Sleep(sleepTime);
				timeSlept += sleepTime;
			} while (timeSlept < time && node.IsRunning && _masterThread.IsRunning);

			// Check if master thread is closing down
			if (!_masterThread.IsRunning)
			{
				// Try and shut down all threads
				foreach (ThreadNode n in regThreads)
				{
					// Set all threads running state to closed
					n.IsRunning = false;
				}
			}// Endif masterThread is not running
		}// End method sleep
	}// End class CThreadManager
}
