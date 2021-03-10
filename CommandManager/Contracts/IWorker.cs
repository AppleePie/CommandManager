using System.Collections.Generic;
using CommandManager.Infrastructure;

namespace CommandManager.Contracts
{
    public interface IWorker
    {
        public IEnumerable<Result<string>> Process(IExecutor executor);
    }
}