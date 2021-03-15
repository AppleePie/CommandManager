using System;
using System.Collections.Generic;
using System.Linq;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class TaskManager
    {
        private Queue<ICommand> Tasks { get; }
        public TaskManager(IEnumerable<ICommand> commands) => Tasks = new Queue<ICommand>(commands);

        public void ExecuteAll()
        {
            while (Tasks.Any()) 
                Tasks.Dequeue().Run().OnFail(Console.WriteLine);
        }
    }
}