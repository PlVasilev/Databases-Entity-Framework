using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Sale")]
   public class SaleDto
    {
        [XmlElement("carId")]
        public int carId { get; set; }

        [XmlElement("customerId")]
        public int customerId { get; set; }

        [XmlElement("discount")]
        public decimal discount { get; set; }
    }
   //<Sales>
   //<Sale>
   //<carId>105</carId>
   //<customerId>30</customerId>
   //<discount>30</discount>
   //</Sale>
}
