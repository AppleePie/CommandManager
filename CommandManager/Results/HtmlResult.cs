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
        private string RootPath { get; }

        public HtmlResult(string workingPath, string relatedPath)
        {
            this.relatedPath = relatedPath.Replace("%20", " ");
            RootPath = workingPath.Replace("%20", " ");
        }

        public Result<string> DumpResult(string commandName, IEnumerable<Result<string>> results)
        {
            var builder = new StringBuilder();
            builder.Append("<ul>");
            foreach (var result in results)
            {
                result.Then(name =>
                {
                    var fullPath = Path.Combine(RootPath, relatedPath, name);
                    var relatedFolderName = "\\" + relatedPath.Replace(RootPath, "");
                    var fileUrl = RootPath == relatedPath ? "\\" + name : $"{relatedFolderName}\\{name}";
                    return builder.Append(
                        $"<li><a href=\"{(File.Exists(fullPath) ? fileUrl : Path.Combine(relatedFolderName, name))}\">{name}</a></li>");
                });
            }

            builder.Append("</ul>");
            return builder.ToString();
        }
        
    }
}