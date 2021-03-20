using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AutomapperMyApp.Models;
using AutomapperMyApp.Views;

namespace AutomapperMyApp.ViewModels
{
    public class EmployeesOlderThanDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime BirthDay { get; set; }

        public EmployeeDto Manager { get; set; }
    }
}
