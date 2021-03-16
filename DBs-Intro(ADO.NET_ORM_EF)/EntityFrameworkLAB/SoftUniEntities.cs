using System;
using Microsoft.EntityFrameworkCore;


namespace EntityFrameworkLAB
{
    public partial class SoftUniEntities : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Department> Departments { get; set; }
    }

}
