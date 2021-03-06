using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Executors
{
    public class Md5Executor : IExecutor
    {
        public Result<string> Execute(string filePath)
        {
            if (Directory.Exists(filePath))
                return $"Total MD5 hash for directory {filePath}: {HashToString(ComputeHashForDirectory(filePath))}";
            if (File.Exists(filePath))
                return $"MD5 hash for {filePath}: {HashToString(CalculateMd5(filePath))}";
            return Result.Fail<string>($"Error: {filePath} is not exists!");
        }

        private static string HashToString(byte[] hash) => BitConverter
            .ToString(hash)
            .Replace("-", "")
            .ToLowerInvariant();

        private static byte[] CalculateMd5(string filename)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filename);
            
            return md5.ComputeHash(stream);
        }

        private static byte[] ComputeHashForDirectory(string directoryPath)
        {
            var resultHash = new byte[16];
            foreach (var hash in Directory.EnumerateFiles(directoryPath).Select(CalculateMd5))
            {
                foreach (var i in Enumerable.Range(0, resultHash.Length))
                {
                    resultHash[i] += hash[i];
                }
            }


            return resultHash;
        }
    }
}