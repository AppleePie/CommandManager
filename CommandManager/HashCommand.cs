using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class HashCommand : ICommand
    {
        public IExecutor FileExecutor { get; }
        public IWorker Worker { get; }
        public IResult FileResult { get; }

        public HashCommand(IExecutor fileExecutor, IWorker worker, IResult fileResult)
        {
            Worker = worker;
            FileResult = fileResult;
            FileExecutor = fileExecutor;
        }

        public Result<None> Run() =>
            Worker
                .Process(FileExecutor)
                .AsResult()
                .Then(FileResult.DumpResult);
    }
}