using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.Data.Models
{
   public class Cell
    {
        public int Id { get; set; }

        [Range(1,1000)]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Prisoner> Prisoners { get; set; } = new List<Prisoner>();
    }

    //Cell
    //•	Id – integer, Primary Key
    //•	CellNumber – integer in the range[1, 1000] (required)
    //•	HasWindow – bool (required)
    //•	DepartmentId - integer, foreign key
    //•	Department – the cell's department (required)
    //•	Prisoners - collection of type Prisoner

}
