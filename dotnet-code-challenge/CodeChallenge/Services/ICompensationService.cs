using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    /// <summary>Compensation operations used by the controllers. Implementations also save changes to the database.</summary>
    public interface ICompensationService
    {
        /// <summary>Returns the compensation for this employee id.</summary>
        Task<Compensation> GetByIdAsync(String id);
        /// <summary>Saves a new compensation record and returns it.</summary>
        Task<Compensation> CreateAsync(Compensation compensation);
    }
}

