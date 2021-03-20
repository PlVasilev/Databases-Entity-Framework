using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("soldProducts")]
    public class ExportSoldProductsWIthCount
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlElement("products")]
        public SoldProductsList ProductDtos { get; set; }
    }

    //[XmlType("soldProducts")]
    //public class SoldProducts
    //{
    //    [XmlElement("Product")]
    //    public List<SoldProductDto> ProductDtos { get; set; } = new List<SoldProductDto>();
    //}
}
