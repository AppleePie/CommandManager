using CommandManager.Contracts;
using CommandManager.Infrastructure;

namespace CommandManager.Executors
{
    public class First10FibonacciSum : IExecutor
    {
        public Result<string> Execute(string filePath) =>
            $"Sum first 10 Fibonacci elements {FibonacciSequence.FirstFibonacciSumFor(10)}";
    }
}