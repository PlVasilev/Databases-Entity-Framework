using System;
using System.Collections.Generic;
using System.Text;
using AutomapperMyApp.Core.Commands.Contracts;
using AutomapperMyApp.Data;
using AutomapperMyApp.Models;
using AutomapperMyApp.Views;
using AutoMapper;

namespace AutomapperMyApp.Core.Commands
{
    public class AddEmployeeCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public AddEmployeeCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            //this.context.Database.EnsureDeleted();
            //this.context.Database.EnsureCreated();

            string firstName = inputArgs[0];
            string lastName = inputArgs[1];
            decimal salary = decimal.Parse(inputArgs[2]);

            //TODO validate

            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Salary = salary
            };

            context.Employees.Add(employee);
            context.SaveChanges();

            var employeeDto = mapper.CreateMappedObject<EmployeeDto>(employee);

            string result =
                $"Registered Successfully {employeeDto.FirstName} {employeeDto.LastName} {employeeDto.Salary}";

            return result;
        }
    }
}
