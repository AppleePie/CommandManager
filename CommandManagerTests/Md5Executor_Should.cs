using System;
using System.Linq;
using CommandManager;
using FluentAssertions;
using NUnit.Framework;

namespace CommandManagerTests
{
    public class Md5Executor_Should
    {
        private Md5Executor executor;
        private const string CorrectFile = "TestFile.txt";
        private const string AnotherCorrectFile = "AnotherTestFile.txt";
        private const string IncorrectFile = "NOT_EXISTING_FILE.txt";

        [OneTimeSetUp]
        public void Initialize() => executor = new Md5Executor();

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
        public void Execute_CorrectFile_CorrectMd5Hash() =>
            ExtractHashCodeFromResult(CorrectFile).Should().Be("41c5c5b41da1e3615640934634618f19");

        [Test]
        public void Execute_SameFile_SameResult() =>
            ExtractHashCodeFromResult(CorrectFile).Should().Be(ExtractHashCodeFromResult(CorrectFile));

        [Test]
        public void Execute_DifferentFiles_DifferentResults() =>
            ExtractHashCodeFromResult(CorrectFile).Should().NotBe(ExtractHashCodeFromResult(AnotherCorrectFile));

        private string ExtractHashCodeFromResult(string fileName) =>
            executor
                .Execute(fileName)
                .GetValueOrThrow()
                .Split(": ")
                .Last();
    }
}