using System;
using System.Collections.Generic;
using System.Text;
using AutomapperMyApp.Views;

namespace AutomapperMyApp.ViewModels
{
    public class ManagerDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<EmployeeDto> MangedEmployees { get; set; } = new List<EmployeeDto>();
    }
}
