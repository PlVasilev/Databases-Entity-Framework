using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("sale")]
   public class saleWithAppliedDiscountDto
    {
        [XmlElement("car")]
        public CarsWithAttribDTO CarsWithDistanceDto { get; set; }

        [XmlElement("discount")]
        public decimal SaleDiscount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("price")]
        public decimal CarPrice { get; set; }

        [XmlElement("price-with-discount")]
        public string CarPriceWithDiscount { get; set; }
    }

    [XmlType("car")]
    public class CarsWithAttribDTO
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }
    }

    //<sales>
    //<sale>
    //<car make = "BMW" model="M5 F10" travelled-distance="435603343" />
    //<discount>30.00</discount>
    //<customer-name>Hipolito Lamoreaux</customer-name>
    //<price>707.97</price>
    //<price-with-discount>495.58</price-with-discount>
    //</ExportSaleDiscount>
    //...
    //</sales>

}
