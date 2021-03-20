using System.ComponentModel.DataAnnotations;

namespace FastFood.Web.ViewModels.Items
{
    public class CreateItemInputModel
    {
        [Required]
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "1000000000")]
        public decimal Price { get; set; }

        public string CategoryIdName { get; set; }

        
    }
}
