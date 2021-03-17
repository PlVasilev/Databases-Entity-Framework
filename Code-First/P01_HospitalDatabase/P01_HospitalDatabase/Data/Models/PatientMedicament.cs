using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    public class PatientMedicament
    {
        [Key]
        public int PatientId { get; set; }

        public int MedicamentId { get; set; }

        [Required]
        public Patient Patient { get; set; }

        [Required]
        public Medicament Medicament { get; set; }
    }
}
