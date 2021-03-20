using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Employees
{
    public class EmployeesAllViewModel
    {
        [Required]
        public string Name { get; set; }

        [Range(18,65)]
        public int Age { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Position { get; set; }
    }
}
