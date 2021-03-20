using System;
using System.Collections.Generic;
using System.Text;
using AutomapperMyApp.Core.Commands.Contracts;
using AutomapperMyApp.Data;
using AutoMapper;

namespace AutomapperMyApp.Core.Commands
{
   public class SetManagerCommand : ICommand
    {
        private readonly MyAppContext context;

        public SetManagerCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
        }

        public string Execute(string[] inputArgs)
        {
            int employeeId = int.Parse(inputArgs[0]);
            int managerId = int.Parse(inputArgs[1]);

            var employee = this.context.Employees.Find(employeeId);
            var manager = this.context.Employees.Find(managerId);

            employee.Manager = manager;

            context.SaveChanges();
            return "Command completed";
        }
    }
}
