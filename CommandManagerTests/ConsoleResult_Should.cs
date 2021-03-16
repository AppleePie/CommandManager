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
    public class ConsoleResult_Should
    {
        private IExecutor executor;
        private ConsoleResult resulter = new ConsoleResult();

        [OneTimeSetUp]
        public void SetUp()
        {
            executor = A.Fake<IExecutor>();
            A.CallTo(() => executor.Execute("test")).Returns(Result.Ok("Done!"));
        }
        
        [Test]
        public void DumpResult_EmptyInput_EmptyOutput()
        {
            const string tempFile = "test.txt";
            var so = new StreamWriter(tempFile) {AutoFlush = true};
            
            Console.SetOut(so);
            resulter.DumpResult(Enumerable.Empty<Result<string>>());
            so.Close();

            File.ReadAllText(tempFile).Should().Be(Environment.NewLine);
            File.Delete(tempFile);
        }

        [Test]
        public void DumpResult_CorrectInput_CorrectResultInConsole()
        {
            const string tempFile = "test.txt";
            var so = new StreamWriter(tempFile) {AutoFlush = true};
            
            Console.SetOut(so);
            resulter.DumpResult(Enumerable.Repeat(executor.Execute("test"), 1));
            so.Close();

            File.ReadAllText(tempFile).Should().Be("Done!" + Environment.NewLine + Environment.NewLine);
            File.Delete(tempFile);
        }
    }
}