using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.Data.Models
{
    public class Projection
    {
        public int Id { get; set; }
        
        [Required]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Required]
        public int HallId { get; set; }
        public Hall Hall { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
   // Projection
   // •	Id – integer, Primary Key
   // •	MovieId – integer, foreign key(required)
   // •	Movie – the projection’s movie
   // •	HallId – integer, foreign key(required)
   // •	Hall – the projection’s hall
   // •	DateTime - DateTime(required)
   // •	Tickets - collection of type Ticket

}
