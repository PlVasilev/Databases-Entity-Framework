using System;
using System.Collections.Generic;
using System.Text;
using AutomapperMyApp.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AutomapperMyApp.Data
{
    public class MyAppContext : DbContext

    {
        public MyAppContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
