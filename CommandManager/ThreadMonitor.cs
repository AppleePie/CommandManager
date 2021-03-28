using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CommandManager
{
    public class ThreadMonitor
    {
        private MyThread[] ThreadPool { get; }
        private HashSet<string> Log { get; }
        public ThreadMonitor(MyThread[] threadPool)
        {
            ThreadPool = threadPool;
            Log = new HashSet<string>();
        }

        public void ExecuteMonitor()
        {
            Console.WriteLine("ThreadId    TaskName        Status");
            while (true)
            {
                foreach (var thread in ThreadPool)
                {
                    var methodName = thread.Task != null ? thread.Task.Target.GetType().Name : "Free";
                    if (Log.Add($"{thread.ThreadId}\t\t{methodName}\t{thread.IsFree}"))
                        Console.WriteLine($"{thread.ThreadId}\t\t{methodName}\t{thread.IsFree}");
                }
                Thread.Sleep(1);
            }
        }
    }
}