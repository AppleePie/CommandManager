using System.Collections.Generic;
using CommandManager.Infrastructure;

namespace CommandManager.Contracts
{
    public interface IWorker
    {
        string WorkingPath { get; }
        bool IsRecursive { get; }
        
        public IEnumerable<Result<string>> Process(IExecutor executor);
    }
}