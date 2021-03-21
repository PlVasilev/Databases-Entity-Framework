using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor
{
    [XmlType("Prisoner")]
   public class PrisonersInboxDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; }

        [XmlArray("EncryptedMessages")]
        public EncryptedMessages[] EncryptedMessages { get; set; }
    }

    [XmlType("Message")]
    public class EncryptedMessages
    {
        [XmlElement("Description")]
        public string Description { get; set; }
    }
    //<Prisoner>
    //<Id>2</Id>
    //<Name>Diana Ebbs</Name>
    //<IncarcerationDate>1963-08-21</IncarcerationDate>
    //<EncryptedMessages>
    //<Message>
    //<Description>.kcab draeh ton evah llits I dna  , skeew 2 tuoba ni si esaeler mubla ehT .dnuoranrut rof skeew 6-4 sekat ynapmoc DC eht dias yllanigiro eH.gnitiaw llits ma I</Description>
    //</Message>
    //<Message>
    //<Description>.emit ruoy ekat ot uoy ekil lliw ew dna krow ruoy ekil I.hsur on emit ruoy ekat , enif si tahT</Description>
    //</Message>
    //</EncryptedMessages>
    //</Prisoner>

}
