using System.IO;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Executors
{
    public class HtmlFileHandler : IExecutor
    {
        public Result<string> Execute(string path) => File.Exists(path) 
            ? Path.GetFileName(path)
            : new DirectoryInfo(path).Name;
    }
}