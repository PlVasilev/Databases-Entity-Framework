using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Import
{
    [XmlType("Procedure")]
   public class ProceduresDto
    {
        [XmlElement("Vet")]
        [Required]
        [MinLength(3), MaxLength(40)]
        public string Vet { get; set; }

        [XmlElement("Animal")]
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Animal { get; set; }

        [XmlElement("DateTime")]
        [Required]
        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        [Required]
        public AnimalAidDto[] AnimalAids { get; set; }
    }

    [XmlType("AnimalAid")]
    public class AnimalAidDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }
    }
    //<Procedures>
    //<Procedure>
    //<Vet>Niels Bohr</Vet>
    //<Animal>acattee321</Animal>
    //<DateTime>14-01-2016</DateTime>
    //<AnimalAids>
    //<AnimalAid>
    //<Name>Nasal Bordetella</Name>
    //</AnimalAid>
    //<AnimalAid>
    //<Name>Internal Deworming</Name>
    //</AnimalAid>
    //<AnimalAid>
    //<Name>Fecal Test</Name>
    //</AnimalAid>
    //</AnimalAids>
    //</Procedure>

}
