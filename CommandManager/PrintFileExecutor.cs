using System.IO;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class PrintFileExecutor : IExecutor
    {
        public Result<string> Execute(string path) => File.Exists(path)
            ? File.ReadAllText(path)
            : Result.Fail<string>($"File {path} is not found!");
    }
}