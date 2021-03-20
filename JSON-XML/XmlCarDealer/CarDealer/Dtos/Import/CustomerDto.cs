using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Customer")]
   public class CustomerDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("birthDate")]
        public DateTime birthDate { get; set; }

        [XmlElement("isYoungDriver")]
        public bool isYoungDriver { get; set; }
    }
    //<Customers>
    //<Customer>
    //<name>Emmitt Benally</name>
    //<birthDate>1993-11-20T00:00:00</birthDate>
    //<isYoungDriver>true</isYoungDriver>
    //</Customer>
}
