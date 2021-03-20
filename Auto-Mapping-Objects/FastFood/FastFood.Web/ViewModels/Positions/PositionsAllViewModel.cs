using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Positions
{
    public class PositionsAllViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
