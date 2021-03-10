using System;
using System.IO;
using System.Linq;
using CommandManager;
using CommandManager.Contracts;
using CommandManager.Infrastructure;
using NUnit.Framework;
using FluentAssertions;
using FakeItEasy;

namespace CommandManagerTests
{
    [TestFixture]
    public class FileWorker_Should
    {
        private IExecutor executor;
        private readonly string workingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "TestDir");
        private const string FileName = "NewFile.txt";

        [OneTimeSetUp]
        public void SetUp()
        {
            executor = A.Fake<IExecutor>();
            A.CallTo(() => executor.Execute(null)).Returns(Result.Ok("Done!"));
        }

        [Test]
        public void Process_IsNotRecursive_OneResult() =>
            new FileWorker(workingPath, false)
                .Process(executor)
                .Count()
                .Should()
                .Be(1);

        [Test]
        public void Process_IsRecursive_OneResult() =>
            new FileWorker(workingPath, true)
                .Process(executor)
                .Count()
                .Should()
                .Be(2);

        [Test]
        public void Process_Md5Executor_OneCorrectResult()
        {
            var exec = new Md5Executor();
            var results = new FileWorker(workingPath, false).Process(exec).ToList();
            results.Count.Should().Be(1);

            Action callResult = () => results.First().GetValueOrThrow();

            callResult.Should().NotThrow();
            results
                .First()
                .GetValueOrThrow()
                .Should()
                .Contain($"MD5 hash for {Path.Combine(workingPath, FileName)}:");
        }
        
        [Test]
        public void Process_Md5Executor_SeveralCorrectResult()
        {
            var exec = new Md5Executor();
            var results = new FileWorker(workingPath, true).Process(exec).ToList();
            results.Count.Should().Be(2);

            Action callResult = () => results.First().GetValueOrThrow();

            callResult.Should().NotThrow();
            results
                .ForEach(r => r.GetValueOrThrow()
                                        .Should()
                                        .Contain($"MD5 hash for {workingPath}")
                );
        }
    }
}