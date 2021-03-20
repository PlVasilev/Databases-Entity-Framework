using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AutomapperMyApp.Core.Commands.Contracts;
using AutomapperMyApp.Data;
using AutomapperMyApp.ViewModels;
using AutoMapper;

namespace AutomapperMyApp.Core.Commands
{
    public class SetAddressCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public SetAddressCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            int employeeId = int.Parse(inputArgs[0]);
            string address = string.Join(" ", inputArgs.Skip(1));
            var employee = this.context.Employees.Find(employeeId);

            var employeeBirthDayDto = this.mapper.CreateMappedObject<EmployeeAddressDto>(employee);

            employeeBirthDayDto.Address = address;
            employee.Address = address;

            context.Employees.Update(employee);
            context.SaveChanges();

            return "Address Set";
        }
    }
}
