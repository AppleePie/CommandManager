using System;
using System.IO;
using CommandManager;
using CommandManager.Executors;
using FluentAssertions;
using NUnit.Framework;

namespace CommandManagerTests
{
    [TestFixture]
    public class PrintFileExecutor_Should
    {
        private PrintFileExecutor executor = new PrintFileExecutor();
        private const string CorrectFile = "TestFile.txt";
        private const string IncorrectFile = "NOT_EXISTING_FILE.txt";

        [Test]
        public void Execute_FileNotExists_Error()
        {
            Action callExecution = () => executor.Execute(IncorrectFile).GetValueOrThrow();
            callExecution
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage($"Error: {IncorrectFile} is not exists!");
        }

        [Test]
        public void Execute_CorrectFile_CorrectPrinting() => executor.Execute(CorrectFile).GetValueOrThrow().Should()
            .Be(File.ReadAllText(CorrectFile));
    }
}