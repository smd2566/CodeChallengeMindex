using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    /// <summary>Persistence operations for employees. Add/Remove stage changes; call SaveAsync to commit them.</summary>
    public interface IEmployeeRepository
    {
        /// <summary>Returns the employee with this id, or null if not found.</summary>
        Task<Employee> GetByIdAsync(String id);
        /// <summary>Stages a new employee for insertion and assigns it a new id.</summary>
        Employee Add(Employee employee);
        /// <summary>Stages an existing employee for deletion.</summary>
        Employee Remove(Employee employee);
        /// <summary>Commits staged changes.</summary>
        Task SaveAsync();
    }
}