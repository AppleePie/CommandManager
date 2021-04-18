using System;
using System.Collections.Generic;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Results
{
    public class ConsoleResult : IResult
    {
        public Result<None> DumpResult(string commandName, IEnumerable<Result<string>> results)
        {
            Console.WriteLine($"{commandName} Result:");
            foreach (var result in results)
                result.Then(Console.WriteLine);
            Console.WriteLine();

            return Result.Ok();
        }
    }
}