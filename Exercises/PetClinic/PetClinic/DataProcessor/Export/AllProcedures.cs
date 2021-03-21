using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Export
{
    [XmlType("Procedure")]
   public class AllProcedures
    {
        [XmlElement("Passport")]
        public string Passport { get; set; }

        [XmlElement("OwnerNumber")]
        public string OwnerNumber { get; set; }

        [XmlElement("DateTime")]
        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        public AnimalAidDtoExport[] AnimalAids { get; set; }

        [XmlElement("TotalPrice")]
        public decimal TotalPrice { get; set; }
    }

    [XmlType("AnimalAid")]
    public class AnimalAidDtoExport
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
    //<Procedures>
    //<Procedure>
    //<Passport>acattee321</Passport>
    //<OwnerNumber>0887446123</OwnerNumber>
    //<DateTime>14-01-2016</DateTime>
    //<AnimalAids>
    //<AnimalAid>
    //<Name>Internal Deworming</Name>
    //<Price>8.00</Price>
    //</AnimalAid>
    //<AnimalAid>
    //<Name>Fecal Test</Name>
    //<Price>7.50</Price>
    //</AnimalAid>
    //<AnimalAid>
    //<Name>Nasal Bordetella</Name>
    //<Price>5.60</Price>
    //</AnimalAid>
    //</AnimalAids>
    //<TotalPrice>21.10</TotalPrice>
    //</Procedure>

}
