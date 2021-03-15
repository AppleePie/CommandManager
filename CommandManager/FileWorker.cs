using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class FileWorker : IWorker
    {
        private string WorkingPath { get; }
        private bool IsRecursive { get; }

        public FileWorker(string workingPath, bool isRecursive)
        {
            WorkingPath = workingPath;
            IsRecursive = isRecursive;
        }

        public IEnumerable<Result<string>> Process(IExecutor executor)
        {
            var searchOption = IsRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            return Directory
                .EnumerateFiles(WorkingPath, "*", searchOption)
                .Select(executor.Execute)
                .Append(executor.Execute(WorkingPath));
        }
    }
}