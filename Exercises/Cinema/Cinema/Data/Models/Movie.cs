using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cinema.Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Query;

namespace Cinema.Data.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3),MaxLength(20)]
        public string Title { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Range(1.00,10.00)]
        public double Rating { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Director { get; set; }

        public ICollection<Projection> Projections { get; set; } = new List<Projection>();
    }
   //Movie
   //•	Id – integer, Primary Key
   //•	Title – text with length[3, 20] (required)
   //•	Genre – enumeration of type Genre, with possible values
   //   (Action, Drama, Comedy, Crime, Western, Romance, Documentary, Children, Animation, Musical) (required)
   //•	Duration – TimeSpan(required)
   //•	Rating – double in the range[1, 10] (required)
   //•	Director – text with length[3, 20] (required)
   //•	Projections - collection of type Projection

}
