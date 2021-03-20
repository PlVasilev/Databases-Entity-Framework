using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using AutomapperMyApp.Core.Commands.Contracts;
using AutomapperMyApp.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AutomapperMyApp.Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private const string Suffix = "Command";
        private readonly IServiceProvider serviceProvider;

        public CommandInterpreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

        }

        public string Read(string[] inputArgs)
        {
            string commandName = inputArgs[0] + Suffix;
            string[] commandParams = inputArgs.Skip(1).ToArray();
            //type
            var type = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(x => x.Name == commandName);

            if (type == null)
            {
                throw new ArgumentNullException("Invalid Command");
            }

            //ctor
            var constructor = type.GetConstructors().FirstOrDefault();

            //ctor params
            var constructorParams = constructor.GetParameters().Select(x => x.ParameterType).ToArray();

            var services = constructorParams.Select(this.serviceProvider.GetService).ToArray();

            //invoke
            var command = (ICommand)constructor.Invoke(services);
            var command2 = (ICommand)Activator.CreateInstance(type, services); // same as previous

            //execute
            string result = command.Execute(commandParams);

            return result;
        }
    }
}
