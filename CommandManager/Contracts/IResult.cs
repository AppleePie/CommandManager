using System.Collections.Generic;
using CommandManager.Infrastructure;

namespace CommandManager.Contracts
{
    public interface IResult
    {
        public Result<string> DumpResult(string commandName, IEnumerable<Result<string>> results);
    }
}