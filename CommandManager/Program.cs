using System;
using System.Collections.Generic;
using Autofac;
using CommandManager.Contracts;
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
            
            var taskManager = container.Resolve<TaskManager>();
            var commands = container.Resolve<IEnumerable<ICommand>>();
            foreach (var command in commands) 
                taskManager.Add(command);
        }
    }
}