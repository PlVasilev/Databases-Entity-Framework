using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("PRODUCTS")]
   public class SoldProductsList
    {
        [XmlElement("Product")]
        public List<SoldProductDto> ProductDtos { get; set; } = new List<SoldProductDto>();
    }
}
