using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace AutomapperLab
{
   public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> StringCourses { get; set; }

        public List<PersonCourse> ClassCourses { get; set; }

        public int Age { get; set; }
    }
}
