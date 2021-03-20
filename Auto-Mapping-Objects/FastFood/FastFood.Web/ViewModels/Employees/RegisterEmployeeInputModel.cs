using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Employees
{
    public class RegisterEmployeeInputModel
    {
        [Required]
        public string Name { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }


        //public int PositionId { get; set; }

        public string PositionName { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
