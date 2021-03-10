using CommandManager.Infrastructure;

namespace CommandManager.Contracts
{
    public interface ICommand
    {
        public IExecutor FileExecutor { get; }
        public IWorker Worker { get; }
        public IResult FileResult { get; }

        public Result<None> Run();
    }
}