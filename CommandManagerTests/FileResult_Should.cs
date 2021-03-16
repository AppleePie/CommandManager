using System;
using System.IO;
using System.Linq;
using CommandManager;
using CommandManager.Contracts;
using CommandManager.Infrastructure;
using CommandManager.Results;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace CommandManagerTests
{
    
    [TestFixture]
    public class FileResult_Should
    {
        private IExecutor executor;
        private readonly FileResult resulter = new FileResult();

        [SetUp]
        public void SetUp()
        {
            executor = A.Fake<IExecutor>();
            A.CallTo(() => executor.Execute("test")).Returns(Result.Ok("Done!"));
        }

        [Test]
        public void DumpResult_EmptyResults_EmptyOutput()
        {
            resulter.DumpResult(Enumerable.Empty<Result<string>>());
            File.ReadAllText(FileResult.FileResultPath).Should().Be(Environment.NewLine);
            File.Delete(FileResult.FileResultPath);
        }

        [Test]
        public void DumpResult_RunCommand_CorrectResultInFile()
        {
            resulter.DumpResult(Enumerable.Repeat(executor.Execute("test"), 1));
            File.ReadAllText(FileResult.FileResultPath).Should().Be("Done!" + Environment.NewLine + Environment.NewLine);
            File.Delete(FileResult.FileResultPath);
        }
    }
}