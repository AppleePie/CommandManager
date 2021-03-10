using CommandManager.Infrastructure;

namespace CommandManager.Contracts
{
    public interface ICommand
    {
        public IExecutor FileExecutor { get; }
        public IFileWorker FileWorker { get; }
        public IResult FileResult { get; }

        public Result<None> Run();
    }
}