using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Items
{
    public class ItemsAllViewModels
    {
        [Required]
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "1000000000")]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }
    }
}
