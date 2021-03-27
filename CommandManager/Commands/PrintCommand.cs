using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Commands
{
    public class PrintCommand : ICommand
    {
        public IExecutor Executor { get; }
        public IWorker Worker { get; }
        public IResult Result { get; }

        public PrintCommand(IExecutor executor, IWorker worker, IResult fileResult)
        {
            Executor = executor;
            Worker = worker;
            Result = fileResult;
        }

        public void Run() => Worker
            .Process(Executor)
            .AsResult()
            .Then(Result.DumpResult);
    }
}