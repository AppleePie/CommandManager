using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class FileResult : IResult
    {
        public Result<None> DumpResult(IEnumerable<Result<string>> results)
        {
            var totalResultInfo = new StringBuilder();
            foreach (var result in results) 
                result.Then(totalResultInfo.AppendLine);
            
            File.WriteAllText("CommandResult.txt", totalResultInfo.ToString());

            return Result.Ok();
        }
    }
}