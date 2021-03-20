using System.ComponentModel.DataAnnotations;
using FastFood.Models;

namespace FastFood.Web.ViewModels.Orders
{
    public class CreateOrderInputModel
    {
        [Required]
        public string Customer { get; set; }

        public string ItemName { get; set; }

        public string EmployeeName { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string OrderType { get; set; }
    }
}
