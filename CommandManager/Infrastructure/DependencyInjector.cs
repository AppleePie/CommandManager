using Autofac;
using CommandManager.Commands;
using CommandManager.Contracts;
using CommandManager.Executors;
using CommandManager.Results;

namespace CommandManager.Infrastructure
{
    public static class DependencyInjector
    {
        private static readonly ContainerBuilder Builder = new ContainerBuilder();

        public static void RegisterArguments(string directoryPath, bool isRecursive)
        {
            Builder.Register(context => directoryPath).SingleInstance();
            Builder.Register(context => isRecursive).SingleInstance();
        }

        public static void RegisterContractImplementations()
        {
            Builder.RegisterType<FileWorker>().As<IWorker>().SingleInstance();

            Builder.RegisterType<Md5Executor>().AsSelf().As<IExecutor>();
            Builder.RegisterType<FileResult>().AsSelf().As<IResult>();
            Builder
                .Register(context => new HashCommand(
                        context.Resolve<Md5Executor>(),
                        context.Resolve<IWorker>(),
                        context.Resolve<FileResult>()
                    )
                )
                .As<ICommand>();

            Builder.RegisterType<PrintFileExecutor>().AsSelf().As<IExecutor>();
            Builder.RegisterType<ConsoleResult>().AsSelf().As<IResult>();
            Builder.Register(context => new PrintCommand(
                        context.Resolve<PrintFileExecutor>(),
                        context.Resolve<IWorker>(),
                        context.Resolve<FileResult>()
                    )
                )
                .As<ICommand>();
        }

        public static void RegisterTypes() => Builder.RegisterType<TaskManager>().AsSelf().SingleInstance();

        public static IContainer Build() => Builder.Build();
    }
}