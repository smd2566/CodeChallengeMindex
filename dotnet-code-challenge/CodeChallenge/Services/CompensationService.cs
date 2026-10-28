using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    /// <summary>
    /// Business logic for compensation records. Mirrors <see cref="EmployeeService"/>: validates input,
    /// then delegates storage to <see cref="ICompensationRepository"/>.
    /// </summary>
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        /// <summary>Adds the compensation to the database and saves. Null input is ignored.</summary>
        public async Task<Compensation> CreateAsync(Compensation compensation)
        {
            if (compensation != null)
            {
                _compensationRepository.Add(compensation);
                await _compensationRepository.SaveAsync();
            }

            return compensation;
        }

        /// <summary>Looks up the compensation for an employee id. Returns null for a null/empty id or when no compensation matches.</summary>
        public async Task<Compensation> GetByIdAsync(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return await _compensationRepository.GetByIdAsync(id);
            }

            return null;
        }
    }
}
