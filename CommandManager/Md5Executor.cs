using System;
using System.IO;
using System.Security.Cryptography;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager
{
    public class Md5Executor : IExecutor
    {
        public Result<string> Execute(string filePath) =>
            File.Exists(filePath) 
                ? $"MD5 hash for {filePath}: {CalculateMd5(filePath)}" 
                : Result.Fail<string>($"Error: {filePath} is not exists!");

        private static string CalculateMd5(string filename)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filename);
            
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}