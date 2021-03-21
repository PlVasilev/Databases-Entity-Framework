using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Cache;
using System.Text;
using System.Xml.Serialization;
using SoftJail.Data.Models.Enums;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
   public class ImportOfficerPrisoners
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(3), MaxLength(30)]       
        public string Name { get; set; }

        [XmlElement("Money")]
        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]           
        public decimal Salary { get; set; }

        [XmlElement("Position")]
        [Required]
        public string Position { get; set; }

        [XmlElement("Weapon")]
        [Required]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public PrisonerDtp[] PrisonerDtp { get; set; }

    }

    [XmlType("Prisoner")]
    public class PrisonerDtp
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }

    //<Officers>
    //<Officer>
    //<Name>Minerva Kitchingman</Name>
    //<Money>2582</Money>
    //<Position>Invalid</Position>
    //<Weapon>ChainRifle</Weapon>
    //<DepartmentId>2</DepartmentId>
    //<Prisoners>
    //<Prisoner id = "15" />
    //
    //</ Prisoners >
    //
    //</ Officer >
}
