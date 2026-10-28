using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    /// <summary>
    /// Data-access layer for compensation records, wrapping <see cref="CompensationContext"/>.
    /// Like the employee repository, changes are only persisted once <see cref="SaveAsync"/> is called.
    /// </summary>
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        /// <summary>Stages a new compensation (and its nested Employee) for insertion. Its EmployeeId is the key, so a duplicate id fails on save. Requires SaveAsync to persist.</summary>
        public Compensation Add(Compensation compensation)
        {
            //The compensation object has the Employee field filled out. However, upon being added to the DbSet of Compensations within the
            //CompensationContext oject, the Employee field becomes null. Every other field is transferred properly, and I am not sure why
            //the Employee field is not. I would love to touch base and learn about how this could be fixed. (2024 comment)

            //Resolved: Add was never the problem. The Employee is saved along with the compensation, but it was not loaded when
            //the compensation was read back. The fix is the .Include(c => c.Employee) in GetByIdAsync.

            _compensationContext.Compensations.Add(compensation); //compensation.Employee = A filled out Employee Object
            return compensation;
        }

        /// <summary>Returns the compensation for the given employee id with its Employee loaded, or null if there is none (callers rely on null to produce a 404).</summary>
        public Task<Compensation> GetByIdAsync(string id)
        {
            //Include tells EF to also load the related Employee. Without it, a freshly queried Compensation only has its own
            //columns filled in and Employee comes back null. FirstOrDefault returns null when no compensation matches,
            //which lets the controller respond with a 404.
            return _compensationContext.Compensations
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(c => c.EmployeeId == id);
        }

        /// <summary>Commits all staged changes to the database.</summary>
        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}

