using System;
using System.Collections.Generic;
using System.Threading;

namespace CommandManager.ThreadPool
{
    public class MyThreadPool : IDisposable
    {
        private static MyThreadPool threadPool;
        private Queue<Action> Scheduler { get; }
        private ThreadWorker[] ThreadPool { get; }
        private readonly int defaultPoolSize = Environment.ProcessorCount;
        
        private MyThreadPool(int maxPoolSize = 0)
        {
            Scheduler = new Queue<Action>();
            var poolSize = maxPoolSize > 0 && maxPoolSize < defaultPoolSize ? maxPoolSize : defaultPoolSize;
            ThreadPool = new ThreadWorker[poolSize];
            var threadMonitor = new ThreadMonitor(ThreadPool);

            lock (ThreadPool)
            {
                for (var i = 0; i < poolSize; i++)
                    ThreadPool[i] = new ThreadWorker(Scheduler, threadMonitor);
            }
            threadMonitor.ExecuteMonitor();
        }

        public static MyThreadPool GetInstance()
        {
            threadPool ??= new MyThreadPool();
            return threadPool;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            foreach (var myThread in ThreadPool) 
                myThread.IsStopped = true;
            
            lock (Scheduler)
                Monitor.PulseAll(Scheduler);
        }

        public void Add(Action action)
        {
            lock (Scheduler)
            {
                Scheduler.Enqueue(action);
                Monitor.Pulse(Scheduler);
            }
        }
    }
}