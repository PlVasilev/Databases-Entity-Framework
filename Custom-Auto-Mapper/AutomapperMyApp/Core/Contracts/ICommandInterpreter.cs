using System;
using System.Collections.Generic;
using System.Text;

namespace AutomapperMyApp.Core.Contracts
{
   public interface ICommandInterpreter
   {
       string Read(string[] inputArgs);
   }
}
