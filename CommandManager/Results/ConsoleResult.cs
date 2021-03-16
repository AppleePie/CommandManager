using System;
using System.Collections.Generic;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Results
{
    public class ConsoleResult : IResult
    {
        public Result<None> DumpResult(IEnumerable<Result<string>> results)
        {
            foreach (var result in results)
                result.Then(Console.WriteLine);
            Console.WriteLine();

            return Result.Ok();
        }
    }
}