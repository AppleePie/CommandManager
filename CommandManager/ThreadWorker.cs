using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommandManager.Contracts;

namespace CommandManager
{
    public class ThreadWorker
    {
        public Queue<ICommand> TasksQueue { get; }
        private MyThread[] ThreadPool { get; }

        public ThreadWorker(MyThread[] threadPool)
        {
            TasksQueue = new Queue<ICommand>();
            ThreadPool = threadPool;
        }

        public void ExecuteQueue()
        {
            while (true)
            {
                var freeThreads = ThreadPool.Where(t => t.IsFree).ToArray();

                foreach (var thread in freeThreads)
                {
                    if (!TasksQueue.Any())
                        break;
                
                    thread.Task = TasksQueue.Dequeue().Run;
                }
                
                Thread.Sleep(1);
            }
        }
    }
}