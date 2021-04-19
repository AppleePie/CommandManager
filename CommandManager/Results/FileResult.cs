using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Results
{
    public class FileResult : IResult
    {
        public const string FileResultPath = "Result.txt";
        public Result<string> DumpResult(string commandName, IEnumerable<Result<string>> results)
        {
            var totalResultInfo = new StringBuilder();
            foreach (var result in results) 
                result.Then(totalResultInfo.AppendLine);
            totalResultInfo.AppendLine();
            
            var fullName = $"{commandName}.{FileResultPath}";

            lock (fullName)
            {
                File.WriteAllText(fullName, totalResultInfo.ToString());
            }

            return Result.Ok("");
        }
    }
}