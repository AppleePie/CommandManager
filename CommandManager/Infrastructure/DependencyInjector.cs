using Autofac;
using Autofac.Core;
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
            Builder.RegisterType<FileWorker>().As<IWorker>().SingleInstance();

            Builder.RegisterType<Md5Executor>().AsSelf().As<IExecutor>();
            Builder.RegisterType<ConsoleResult>().As<IResult>();
            Builder.RegisterType<HashCommand>().As<ICommand>();

            Builder.RegisterType<PrintFileExecutor>().AsSelf().As<IExecutor>();
            Builder.RegisterType<ConsoleResult>().As<IResult>();
            Builder.RegisterType<PrintCommand>().As<ICommand>();
        }

        public static void RegisterTypes() => Builder.RegisterType<TaskManager>().AsSelf().SingleInstance();

        public static IContainer Build() => Builder.Build();
    }
}