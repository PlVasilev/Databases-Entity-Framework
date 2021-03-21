using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace PetClinic.Models
{
   public class AnimalAid
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3),MaxLength(30)]
        public string Name { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        public ICollection<ProcedureAnimalAid> AnimalAidProcedures { get; set; } = new List<ProcedureAnimalAid>();
    }

   //AnimalAid
   //-	Id – integer, Primary Key
   //-	Name – text with min length 3 and max length 30 (required, unique)
   //-	Price – decimal (non-negative, minimum value: 0.01, required)
   //-	AnimalAidProcedures – collection of type ProcedureAnimalAid

}
