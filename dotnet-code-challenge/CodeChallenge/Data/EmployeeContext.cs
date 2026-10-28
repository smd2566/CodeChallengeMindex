using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    /// <summary>Entity Framework context for the employee database (an in-memory database in this project).</summary>
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }

        /// <summary>All employees. The key is EmployeeId, found by EF convention (see <see cref="Employee"/>).</summary>
        public DbSet<Employee> Employees { get; set; }
    }
}
