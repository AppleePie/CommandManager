using Autofac;
using CommandManager.Contracts;

namespace CommandManager.Infrastructure
{
    public static class DependencyInjector
    {
        private static readonly ContainerBuilder Builder = new ContainerBuilder();

        public static void RegisterArguments(string directoryPath, bool isRecursive)
        {
            Builder.Register(context => directoryPath).SingleInstance();
            Builder.Register(context => isRecursive);
        }

        public static void RegisterContractImplementations()
        {
            Builder.RegisterType<Md5Executor>().As<IExecutor>();
            Builder.RegisterType<FileWorker>().As<IFileWorker>();
            Builder.RegisterType<ConsoleFileResult>().As<IResult>();
            Builder.RegisterType<HashCommand>().As<ICommand>();
        }

        public static void RegisterTypes() => Builder.RegisterType<TaskManager>().AsSelf().SingleInstance();

        public static IContainer Build() => Builder.Build();
    }
}