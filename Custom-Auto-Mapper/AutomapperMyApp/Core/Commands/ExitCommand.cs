using System;
using AutomapperMyApp.Core.Commands.Contracts;

namespace AutomapperMyApp.Core.Commands
{
   public class ExitCommand : ICommand 
    {
        public string Execute(string[] inputArgs)
        {
            Environment.Exit(0);
            return "";
        }
    }
}
