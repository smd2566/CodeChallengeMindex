using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    /// <summary>Persistence operations for compensation records. Add stages a change; call SaveAsync to commit it.</summary>
    public interface ICompensationRepository
    {
        /// <summary>Returns the compensation for this employee id.</summary>
        Task<Compensation> GetByIdAsync(String id);
        /// <summary>Stages a new compensation for insertion.</summary>
        Compensation Add(Compensation compensation);
        /// <summary>Commits staged changes.</summary>
        Task SaveAsync();
    }
}