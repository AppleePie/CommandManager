using CommandManager.Infrastructure;

namespace CommandManager.Contracts
{
    public interface IExecutor
    {
        public Result<string> Execute(string path);
    }
}