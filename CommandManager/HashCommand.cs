using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class HashCommand : ICommand
    {
        public IExecutor FileExecutor { get; }
        public IFileWorker FileWorker { get; }
        public IResult FileResult { get; }

        public HashCommand(IExecutor fileExecutor, IFileWorker fileWorker, IResult fileResult)
        {
            FileWorker = fileWorker;
            FileResult = fileResult;
            FileExecutor = fileExecutor;
        }

        public Result<None> Run() =>
            FileWorker
                .Process(FileExecutor)
                .AsResult()
                .Then(FileResult.DumpResult);
    }
}