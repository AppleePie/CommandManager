using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Results
{
    public class FileResult : IResult
    {
        public static string FileResultPath = "CommandResult.txt";
        public Result<None> DumpResult(IEnumerable<Result<string>> results)
        {
            var totalResultInfo = new StringBuilder();
            foreach (var result in results) 
                result.Then(totalResultInfo.AppendLine);
            totalResultInfo.AppendLine();
            
            File.WriteAllText(Path.Combine("CommandResult", FileResultPath), totalResultInfo.ToString());
            Console.WriteLine(File.ReadAllText(Path.Combine("CommandResult", FileResultPath)));

            Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "/CommandResult/").ToList().ForEach(Console.WriteLine);

            return Result.Ok();
        }
    }
}