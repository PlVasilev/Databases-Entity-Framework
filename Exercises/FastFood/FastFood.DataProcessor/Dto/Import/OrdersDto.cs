using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Import
{
    [XmlType("Order")]
   public class OrdersDto
    {
        [XmlElement("Customer")]
        [Required]
        public string CustomerName { get; set; }

        [XmlElement("Employee")]
        [MinLength(3), MaxLength(30)]
        public string EmployeeName { get; set; }

        [XmlElement("DateTime")]
        [Required]
        public string DateTime { get; set; }

        [XmlElement("Type")]
        [Required]
        public string Type { get; set; }

        [XmlArray("Items")]
        public ItemBought[] Items { get; set; }
    }

    [XmlType("Item")]
    public class ItemBought
    {
        [XmlElement("Name")]
        [MinLength(3), MaxLength(30)]
        public string ItemName { get; set; }

        [XmlElement("Quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
    //<Orders>
    //<Order>
    //<Customer>Garry</Customer>
    //<Employee>Maxwell Shanahan</Employee>
    //<DateTime>21/08/2017 13:22</DateTime>
    //<Type>ForHere</Type>
    //<Items>
    //<Item>
    //<Name>Quarter Pounder</Name>
    //<Quantity>2</Quantity>
    //</Item>
    //<Item>
    //<Name>Premium chicken sandwich</Name>
    //<Quantity>2</Quantity>
    //</Item>
    //<Item>
    //<Name>Chicken Tenders</Name>
    //<Quantity>4</Quantity>
    //</Item>
    //<Item>
    //<Name>Just Lettuce</Name>
    //<Quantity>4</Quantity>
    //</Item>
    //</Items>
    //</Order>

}
