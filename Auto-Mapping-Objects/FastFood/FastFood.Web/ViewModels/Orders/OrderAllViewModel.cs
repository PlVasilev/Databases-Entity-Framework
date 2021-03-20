using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Orders
{
    public class OrderAllViewModel
    {
        public int OrderId { get; set; }

        [Required]
        public string Customer { get; set; }

        [Required]
        public string Employee { get; set; }

        [Required]
        public string DateTime { get; set; }
    }
}
