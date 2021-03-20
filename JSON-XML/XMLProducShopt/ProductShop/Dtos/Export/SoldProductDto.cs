using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Product")]
    public class SoldProductDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }

    [XmlType("User")]
    public class UserDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("soldProducts")]
        public SoldProducts SoldProducts { get; set; }
    }

    [XmlType("soldProducts")]
    public class SoldProducts
    {
        //[XmlElement("count")]
        //public int Count { get; set; }

        [XmlElement("Product")]
        public List<SoldProductDto> ProductDtos { get; set; } = new List<SoldProductDto>();
    }


    //<User>
    //<firstName>Almire</firstName>
    //<lastName>Ainslee</lastName>
    //<soldProducts>
    //<Product>
    //<name>olio activ mouthwash</name>
    //<price>206.06</price>
    //</Product>
    //<Product>
    //<name>Acnezzol Base</name>
    //<price>710.6</price>
    //</Product>
    //<Product>
    //<name>ENALAPRIL MALEATE</name>
    //<price>210.42</price>
    //</Product>
    //</soldProducts>
    //</User>...
}
