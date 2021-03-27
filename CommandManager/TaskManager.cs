using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class TaskManager
    {
        private static readonly Dictionary<int, Action> States = new Dictionary<int, Action>();
        private static readonly Dictionary<int, bool> IsFree = new Dictionary<int, bool>();
        private const int DefaultPoolSize = 2;
        private Queue<ICommand> Tasks { get; }
        public TaskManager(IEnumerable<ICommand> commands)
        {
            Tasks = new Queue<ICommand>(commands);
            for (var i = 0; i < DefaultPoolSize; i++)
            {
                var thread = new Thread(Run);
                IsFree[thread.ManagedThreadId] = true;
                thread.Start();
            }
        }

        public void ExecuteAll()
        {
            while (Tasks.Any())
            {
                var firstFreeThread = IsFree.SkipWhile(pair => !pair.Value).First().Key;
                IsFree[firstFreeThread] = false;
                States[firstFreeThread] = Tasks.Dequeue().Run;
            }
        }

        private static void Run()
        {
            while (true)
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                if (States.ContainsKey(threadId))
                {
                    States[threadId]();
                    States.Remove(threadId);
                    IsFree[threadId] = true;
                }
                else
                    Thread.Sleep(1);
            }
        }
    }
}