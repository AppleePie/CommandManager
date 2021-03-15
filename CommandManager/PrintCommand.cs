using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class PrintCommand : ICommand
    {
        public IExecutor Executor { get; }
        public IWorker Worker { get; }
        public IResult Result { get; }

        public PrintCommand(PrintFileExecutor executor, IWorker worker, IResult fileResult)
        {
            Executor = executor;
            Worker = worker;
            Result = new FileResult();
        }

        public Result<None> Run() => Worker
            .Process(Executor)
            .AsResult()
            .Then(Result.DumpResult);
    }
}