using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProductShop.Views.Dtos
{
    public class UserSalesDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonProperty(PropertyName = "soldProducts")]
        public List<SoldProductDto> SoldProduct { get; set; } = new List<SoldProductDto>();
    }
}
