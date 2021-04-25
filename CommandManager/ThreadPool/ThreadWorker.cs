using System;
using System.Collections.Generic;
using System.Threading;

namespace CommandManager.ThreadPool
{
    public class ThreadWorker
    {
        public bool IsStopped { get; set; }
        public int ThreadId { get; }
        public bool IsFree { get; private set; }
        private Queue<Action> Scheduler { get; }
        private ThreadMonitor ThreadResult { get; }

        public ThreadWorker(Queue<Action> scheduler, ThreadMonitor threadResult)
        {
            Scheduler = scheduler;
            ThreadResult = threadResult;
            IsFree = true;
            var thread = new Thread(StartWork);

            ThreadId = thread.ManagedThreadId;
            
            thread.Start();
        }

        private void StartWork()
        {
            while (true)
            {
                Action task;
                lock (Scheduler)
                {
                    while (Scheduler.Count == 0)
                    {
                        if (IsStopped)
                            return;
                        Monitor.Wait(Scheduler);
                    }

                    task = Scheduler.Dequeue();
                    IsFree = false;
                    ThreadResult.ExecuteMonitor();
                }
                
                task();

                IsFree = true;
                ThreadResult.ExecuteMonitor();
            }
        }
    }
}