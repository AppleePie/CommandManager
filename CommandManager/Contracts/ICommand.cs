using CommandManager.Infrastructure;

namespace CommandManager.Contracts
{
    public interface ICommand
    {
        public IExecutor Executor { get; }
        public IWorker Worker { get; }
        public IResult Result { get; }

        public Result<None> Run();
    }
}