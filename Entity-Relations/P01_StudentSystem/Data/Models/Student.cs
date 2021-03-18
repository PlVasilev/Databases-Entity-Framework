using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(10),MinLength(10)]
        //[Column(TypeName = "CHAR(10)")]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }
       
        public DateTime? Birthday  { get; set; }

        public ICollection<Homework> HomeworkSubmissions { get; set; } = new HashSet<Homework>();

        public ICollection<StudentCourse> CourseEnrollments { get; set; } = new HashSet<StudentCourse>();
    }
}
