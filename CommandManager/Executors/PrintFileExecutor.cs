using System.IO;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Executors
{
    public class PrintFileExecutor : IExecutor
    {
        public Result<string> Execute(string path) => File.Exists(path)
            ? File.ReadAllText(path)
            : Result.Fail<string>($"Error: {path} is not exists!");
    }
}