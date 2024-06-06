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
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            //The compensation object has the Employee field filled out. However, upon being added to the DbSet of Compensations within the
            //CompensationContext oject, the Employee field becomes null. Every other field is transferred properly, and I am not sure why
            //the Employee field is not. I would love to touch base and learn about how this could be fixed.

            _compensationContext.Compensations.Add(compensation); //compensation.Employee = A filled out Employee Object
            return compensation;
        }

        public Compensation GetById(string id)
        {

            var returnedCompensation = new Compensation();

            foreach (var compensation in _compensationContext.Compensations) //compensation.Employee immediately becomes null, even before any of this code is executed
            {
                if (compensation.EmployeeId == id)
                {
                    returnedCompensation = compensation; //The other values are returned properly. It is only compensation.Employee that is null.

                }

            }

            return returnedCompensation;
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}

