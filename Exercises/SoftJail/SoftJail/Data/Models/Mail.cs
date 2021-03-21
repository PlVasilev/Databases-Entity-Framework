using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace SoftJail.Data.Models
{
   public class Mail
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z0-9\\s]*str.$")]
        public string Address { get; set; }

        public int PrisonerId { get; set; }
        public Prisoner Prisoner { get; set; }
    }
   //Mail
   //•	Id – integer, Primary Key
   //•	Description– text(required)
   //•	Sender – text(required)
   //•	Address – text, consisting only of letters, spaces and numbers, which ends with “ str.” (required) (Example: “62 Muir Hill str.“)
   //•	PrisonerId - integer, foreign key
   //•	Prisoner – the mail's Prisoner (required)

}
