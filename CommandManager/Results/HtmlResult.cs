using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Results
{
    public class HtmlResult : IResult
    {
        private readonly string relatedPath;
        private string WorkingPath { get; }

        public HtmlResult(string workingPath, string relatedPath)
        {
            this.relatedPath = relatedPath;
            WorkingPath = workingPath;
        }

        public Result<string> DumpResult(string commandName, IEnumerable<Result<string>> results)
        {
            var builder = new StringBuilder();
            builder.Append("<ul>");
            foreach (var result in results)
            {
                result.Then(path =>
                {
                    var fullPath = Path.Combine(WorkingPath, relatedPath, path);
                    var url = $"/{relatedPath}/{path}";
                    return builder.Append(
                        $"<li><a href=\"{(File.Exists(fullPath) ? url : Path.GetFileName(path))}\">{path}</a></li>");
                });
            }

            builder.Append("</ul>");
            return builder.ToString();
        }
    }
}