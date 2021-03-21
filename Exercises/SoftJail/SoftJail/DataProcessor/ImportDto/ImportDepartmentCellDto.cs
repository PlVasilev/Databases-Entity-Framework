using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
   public class ImportDepartmentCellDto
    {
        [MinLength(3), MaxLength(25)]
        public string Name { get; set; }

        public CellsDto[] Cells { get; set; }
    }

    public class CellsDto
    {
        [Range(1, 1000)]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }
    }
}
