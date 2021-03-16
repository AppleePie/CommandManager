using System.IO;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Executors
{
    public class PrintFileExecutor : IExecutor
    {
        public Result<string> Execute(string filePath) => File.Exists(filePath)
            ? File.ReadAllText(filePath)
            : Result.Fail<string>($"Error: {filePath} is not exists!");
    }
}