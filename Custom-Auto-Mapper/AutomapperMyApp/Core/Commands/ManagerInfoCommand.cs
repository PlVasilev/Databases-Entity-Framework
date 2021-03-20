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
   public class ManagerInfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public ManagerInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            //var allEmployees = context.Employees.ToList();

            int managerId = int.Parse(inputArgs[0]);
            Employee manager = context.Employees.Include(m => m.MangedEmployees).FirstOrDefault(x => x.Id == managerId);

            var ManagerDto = this.mapper.CreateMappedObject<ManagerDto>(manager);
            
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(
                $"{ManagerDto.FirstName} {ManagerDto.LastName} | Employees: {ManagerDto.MangedEmployees.Count}");
            foreach (var dtoMangedEmployee in ManagerDto.MangedEmployees)
            {
                sb.AppendLine(
                    $"    - {dtoMangedEmployee.FirstName} {dtoMangedEmployee.LastName} - ${dtoMangedEmployee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
