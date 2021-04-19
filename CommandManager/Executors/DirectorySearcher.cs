using System.IO;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Executors
{
    public class DirectorySearcher : IExecutor
    {
        public Result<string> Execute(string path) => Directory.Exists(path) ? path : "";
    }
}