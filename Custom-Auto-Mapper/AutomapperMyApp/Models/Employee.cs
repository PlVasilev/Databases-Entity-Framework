using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutomapperMyApp.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Range(typeof(decimal), "0,00","1000000000000000" )]
        public decimal Salary { get; set; }

        public DateTime? BirthDay { get; set; }

        public string Address { get; set; }

        public int? ManagerId { get; set; }
        public  Employee Manager { get; set; }

        public List<Employee> MangedEmployees { get; set; } = new List<Employee>();
    }
}
