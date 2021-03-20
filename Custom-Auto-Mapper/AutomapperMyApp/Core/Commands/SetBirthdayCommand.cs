using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AutomapperMyApp.Core.Commands.Contracts;
using AutomapperMyApp.Data;
using AutomapperMyApp.ViewModels;
using AutoMapper;

namespace AutomapperMyApp.Core.Commands
{
    public class SetBirthdayCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public SetBirthdayCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] inputArgs)
        {
            int employeeId = int.Parse(inputArgs[0]);
            DateTime birthDay = DateTime.ParseExact(inputArgs[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var employee = this.context.Employees.Find(employeeId);

            var employeeBirthDayDto = this.mapper.CreateMappedObject<EmployeeBirthDayDto>(employee);

            employeeBirthDayDto.BirthDay = birthDay;
            employee.BirthDay = birthDay;

            context.Employees.Update(employee);
            context.SaveChanges();

            return "Birthday Set";
        }
    }
}
