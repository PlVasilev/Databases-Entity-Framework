using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AutomapperMyApp.Core.Commands.Contracts;
using AutomapperMyApp.Data;
using AutomapperMyApp.Models;
using AutomapperMyApp.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AutomapperMyApp.Core.Commands
{
    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public EmployeePersonalInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {

            int employeeId = int.Parse(inputArgs[0]);
            Employee employee = context.Employees.Include(m => m.MangedEmployees).FirstOrDefault(x => x.Id == employeeId);

            var employeeDto = this.mapper.CreateMappedObject<EmployeePersonalInfoDto>(employee);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(
                $"{employeeDto.Id} - {employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:f2}");
            if (employeeDto.BirthDay != null)
            {
                sb.AppendLine($"Birthday: {employeeDto.BirthDay:dd-MM-yyyy}");
            }
            if (employeeDto.Address != null)
            {
                sb.AppendLine($"Address: {employeeDto.Address}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
