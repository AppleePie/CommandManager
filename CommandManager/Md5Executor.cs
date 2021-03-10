using System;
using System.Diagnostics;
using System.IO;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class Md5Executor : IExecutor
    {
        public Result<string> Execute(string filePath)
        {
            if (!File.Exists(filePath))
                return Result.Fail<string>($"Error: {filePath} is not exists!");

            var process = new Process
            {
                StartInfo =
                {
                    FileName = "certutil",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Arguments = $@"-hashfile {filePath} MD5"
                }
            };

            process.Start();
            process.WaitForExit();
            var result = process.StandardOutput.ReadToEnd();

            return !result.Contains("ERROR")
                ? $"MD5 hash for {filePath}: {ExtractMd5Hash(result)}" 
                : Result.Fail<string>($"Exception was thrown trying process MD5-hash for {filePath}");
        }

        private static string ExtractMd5Hash(string result) => result.Split(Environment.NewLine)[1];
    }
}