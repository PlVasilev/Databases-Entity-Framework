using System;
using System.Collections.Generic;
using System.Text;
using AutomapperMyApp.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AutomapperMyApp.Core
{
   public class Engine : IEngine
   {
       private readonly IServiceProvider provider;

        public Engine(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public void Run()
        {


            while (true)
            {
                string[] inputArgs = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    var commandInterpreter = this.provider.GetService<ICommandInterpreter>();
                    string result = commandInterpreter.Read(inputArgs);

                    Console.WriteLine(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
