using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    public class UsersWithUserDto
    {
        [XmlElement("User")]
        public List<ExportUserWithAgeDto> Users { get; set; }
    }
}
