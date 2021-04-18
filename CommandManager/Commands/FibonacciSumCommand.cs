using System;
using System.Linq;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Commands
{
    public class FibonacciSumCommand : ICommand
    {
        public FibonacciSumCommand(IExecutor executor, IResult result)
        {
            Executor = executor;
            Result = result;
        }

        public IExecutor Executor { get; }
        public IWorker Worker { get; } = null;
        public IResult Result { get; }

        public void Run() => 
            Result.DumpResult(nameof(FibonacciSumCommand),Enumerable.Repeat(Executor.Execute(string.Empty), 1));
    }
}