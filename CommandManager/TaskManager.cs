using System;
using CommandManager.Contracts;

namespace CommandManager
{
    public class TaskManager
    {
        private ThreadWorker ThreadWorker { get; }
        private ThreadMonitor ThreadMonitor { get; }
        private MyThread[] ThreadPool { get; }
        private readonly int defaultPoolSize = Environment.ProcessorCount - 2;
        public TaskManager(int maxPoolSize = default)
        {
            var poolSize = maxPoolSize > 0 ? maxPoolSize : defaultPoolSize;
            ThreadPool = new MyThread[poolSize];
            
            for (var i = 0; i < poolSize; i++) 
                ThreadPool[i] = new MyThread();
            
            ThreadWorker = new ThreadWorker(ThreadPool);
            ThreadMonitor = new ThreadMonitor(ThreadPool);
            new MyThread().Task = ThreadWorker.ExecuteQueue;
            new MyThread().Task = ThreadMonitor.ExecuteMonitor;
        }

        public void Add(ICommand command)
        {
            lock (ThreadWorker.TasksQueue)
            {
                ThreadWorker.TasksQueue.Enqueue(command);       
            }
        }
    }
}