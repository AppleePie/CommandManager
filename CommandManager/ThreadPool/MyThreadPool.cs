using System;
using System.Collections.Generic;
using System.Threading;

namespace CommandManager.ThreadPool
{
    public class MyThreadPool : IDisposable
    {
        private Queue<Action> Scheduler { get; }
        private ThreadWorker[] ThreadPool { get; }
        private readonly int defaultPoolSize = Environment.ProcessorCount;
        
        public MyThreadPool(int maxPoolSize = 0)
        {
            Scheduler = new Queue<Action>();
            var poolSize = maxPoolSize > 0 && maxPoolSize < defaultPoolSize ? maxPoolSize : defaultPoolSize;
            ThreadPool = new ThreadWorker[poolSize];
            var threadMonitor = new ThreadMonitor(ThreadPool);

            for (var i = 0; i < poolSize; i++)
                ThreadPool[i] = new ThreadWorker(Scheduler, threadMonitor);
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