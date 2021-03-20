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
    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public ListEmployeesOlderThanCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            int age = int.Parse(inputArgs[0]);
            var employees = context.Employees
                .Include(x => x.Manager)
                .Where(x => DateTime.Now.Year - x.BirthDay.Value.Year > age)
                .OrderByDescending(x => x.Salary).ToList();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < employees.Count; i++)
            {
                var empDto = this.mapper.CreateMappedObject<EmployeesOlderThanDto>(employees[i]);
                string managerName = "[no manager]";
                if (empDto.Manager != null)
                {
                    managerName = empDto.Manager.LastName;
                }
                sb.AppendLine($"{empDto.FirstName} {empDto.LastName} ${empDto.Salary:f2} - Manager: {managerName}");

            }

            return sb.ToString().TrimEnd();
        }

    }
}
