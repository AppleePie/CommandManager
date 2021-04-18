using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Commands
{
    public class HashCommand : ICommand
    {
        public IExecutor Executor { get; }
        public IWorker Worker { get; }
        public IResult Result { get; }

        public HashCommand(IExecutor executor, IWorker worker, IResult fileResult)
        {
            Worker = worker;
            Result = fileResult;
            Executor = executor;
        }

        public void Run() => Result
            .DumpResult(
                nameof(HashCommand),
                Worker.Process(Executor)
            );
    }
}