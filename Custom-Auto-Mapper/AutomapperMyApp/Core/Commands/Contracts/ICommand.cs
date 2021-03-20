using System;
using System.Collections.Generic;
using System.Text;

namespace AutomapperMyApp.Core.Commands.Contracts
{
   public interface ICommand
   {
       string Execute(string[] inputArgs);
   }
}
