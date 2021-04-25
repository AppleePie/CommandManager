using System;
using System.Linq;

namespace CommandManager.ThreadPool
{
    public class ThreadMonitor
    {
        private ThreadWorker[] ThreadPool { get; }

        public ThreadMonitor(ThreadWorker[] threadPool) => ThreadPool = threadPool;

        public void ExecuteMonitor()
        {
            lock (ThreadPool) {
                Console.Clear();
                Console.WriteLine($"|{"ID", 3}  |  {"Is Free", 7}  |");
                Console.WriteLine("|{0}|", string.Join("", Enumerable.Repeat('-', 17).ToArray()));
                foreach (var thread in ThreadPool) 
                    Console.WriteLine($"|{thread.ThreadId,3}  |  {thread.IsFree,7}  |");

                Console.WriteLine("|{0}|", string.Join("", Enumerable.Repeat('-', 17).ToArray()));
            }
        }
    }
}