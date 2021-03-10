using System.Collections.Generic;
using CommandManager.Infrastructure;

namespace CommandManager.Contracts
{
    public interface IFileWorker
    {
        string WorkingPath { get; }
        bool IsRecursive { get; }
        
        public IEnumerable<Result<string>> Process(IExecutor executor);
    }
}