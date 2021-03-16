using System;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace EntityFrameworkLAB
{
    class EntityFrameworkLAB
    {
        static void Main(string[] args)
        {
            // Scaffold = automatic code generator !!
            //Install it from Package Manager Console
            //Install-Package Microsoft.EntityFrameworkCore
            //  EF Core is modular – any data providers must be installed too:
            //Install-Package Microsoft.EntityFrameworkCore.SqlServer

            // Database First Model: Setup

            // Scaffolding DbContext from DB with Scaffold-DbContext command in Package Manager Console:
            // Scaffold - DbContext
            //     - DbLoggerCategory.Database.Connection "Server=.;Database=…;Integrated Security=True"
            //     - Provider Microsoft.EntityFrameworkCore.SqlServer
            //     - OutputDir Data
            //Scaffolding requires the following packages beforehand:

            //Reading Data with LINQ Query
            using (var context = new SoftUniEntities())
            {
                var employees = context.Employees
                    .Where(e => e.Department.Name == "Sales")
                    .ToArray(); //IQuerrible
            }

            //Find element by ID
            using (var context = new SoftUniEntities())
            {
                var project = context.Projects.Find(2);
                Console.WriteLine(project.Name);
            }

            //      Creating New Data
            //var project1 = new Project()
            //{
            //    Name = "Judge System",
            //    StartDate = new DateTime(2015, 4, 15),
            //};
            //context.Projects.Add(project1);
            //context.SaveChanges();

            //Cascading Inserts
            //We can also add cascading entities to the database:
            //Employee employee = new Employee();
            //employee.FirstName = "Petya";
            //employee.LastName = "Grozdarska";
            //employee.Projects.Add(new Project { Name = "SoftUni Conf" });
            //softUniEntities.Employees.Add(employee);
            //softUniEntities.SaveChanges();

            //Updating Existing Data
            //Employees employee =
            //    softUniEntities.Employees.First();
            //employees.FirstName = "Alex";
            //context.SaveChanges();

            //Deleting Existing Data
            //Employees employee =
            //    softUniEntities.Employees.First();
            //softUniEntities.Employees.Remove(employee);
            //softUniEntities.SaveChanges();


        }
    }
}
