using System;
using System.CommandLine.DragonFruit;
using Autofac;
using CommandManager.Infrastructure;

namespace CommandManager
{
    class Program
    {
        /// <summary>
        /// Runs a certain pool of commands on all files in the directory
        /// </summary>
        /// <param name="directoryPath">Directory for running</param>
        /// <param name="isRecursive" alias="r">Flag for applying commands to subdirectories</param>
        static void Main(string directoryPath, bool isRecursive)
        {
            if (directoryPath is null)
            {
                Console.WriteLine("Directory not found. Use default: TestDirectory");
                directoryPath = "TestDirectory";
            }

            DependencyInjector.RegisterArguments(directoryPath, isRecursive);
            DependencyInjector.RegisterContractImplementations();
            DependencyInjector.RegisterTypes();

            using var container = DependencyInjector.Build();
            container.Resolve<TaskManager>().ExecuteAll();
        }
    }
}