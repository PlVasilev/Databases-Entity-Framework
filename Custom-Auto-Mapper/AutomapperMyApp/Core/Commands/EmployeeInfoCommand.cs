using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomapperMyApp.Core.Commands.Contracts;
using AutomapperMyApp.Data;
using AutomapperMyApp.Models;
using AutomapperMyApp.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AutomapperMyApp.Core.Commands
{
   public class EmployeeInfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public EmployeeInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
    
            int employeeId = int.Parse(inputArgs[0]);
            Employee employee = context.Employees.Include(m => m.MangedEmployees).FirstOrDefault(x => x.Id == employeeId);

            var employeeDto = this.mapper.CreateMappedObject<EmployeeInfoDto>(employee);

            return $"{employeeDto.Id} - {employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:f2}";
        }
    }
}
