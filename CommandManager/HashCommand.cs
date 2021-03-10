using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class HashCommand : ICommand
    {
        public IExecutor Executor { get; }
        public IWorker Worker { get; }
        public IResult FileResult { get; }

        public HashCommand(IExecutor executor, IWorker worker, IResult fileResult)
        {
            Worker = worker;
            FileResult = fileResult;
            Executor = executor;
        }

        public Result<None> Run() =>
            Worker
                .Process(Executor)
                .AsResult()
                .Then(FileResult.DumpResult);
    }
}