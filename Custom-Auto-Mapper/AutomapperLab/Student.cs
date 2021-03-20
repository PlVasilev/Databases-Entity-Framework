using System;
using System.Collections.Generic;
using System.Text;

namespace AutomapperLab
{
   public class Student
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public List<string> StringCourses { get; set; }

        public List<StudentCourse> ClassCourses { get; set; }

        //Not from Automapper
        //public string FullName { get => $"{FirstName} {LastName} from getter"; } // do not map without setter

        //from Automapper
        public string FullName { get; set; }
    }
}
