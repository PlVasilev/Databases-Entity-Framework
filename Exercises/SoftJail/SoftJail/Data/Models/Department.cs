﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.Data.Models
{
   public class Department
    {
        public int Id { get; set; }

        [MinLength(3),MaxLength(25)]
        public string Name { get; set; }

        public ICollection<Cell> Cells { get; set; } = new List<Cell>();

        public ICollection<Officer> Officers { get; set; } = new List<Officer>();
    }
   // •	Id – integer, Primary Key
   // •	Name – text with min length 3 and max length 25 (required)
   // •	Cells - collection of type Cell

}
