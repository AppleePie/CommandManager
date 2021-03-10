using System.Collections.Generic;
using CommandManager.Infrastructure;

namespace CommandManager.Contracts
{
    public interface IResult
    {
        public Result<None> DumpResult(IEnumerable<Result<string>> results);
    }
}