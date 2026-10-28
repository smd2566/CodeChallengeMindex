using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    /// <summary>Entity Framework context for the compensation database, kept separate from <see cref="EmployeeContext"/>.</summary>
    public class CompensationContext : DbContext
    {
        public CompensationContext(DbContextOptions<CompensationContext> options) : base(options)
        {

        }

        /// <summary>All compensation records, keyed by EmployeeId (one compensation per employee).</summary>
        public DbSet<Compensation> Compensations { get; set; }
    }
}

