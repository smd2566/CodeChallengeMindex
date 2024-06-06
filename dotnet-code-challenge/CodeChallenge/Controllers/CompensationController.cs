using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.Employee.FirstName} {compensation.Employee.LastName}'");

            _compensationService.Create(compensation);

            return CreatedAtRoute("getCompensationById", new { employeeId = compensation.Employee.EmployeeId }, compensation);
        }

        [HttpGet("{employeeId}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String employeeId)
        {
            _logger.LogDebug($"Received compensation get request for '{employeeId}'");

            var compensation = _compensationService.GetById(employeeId); //this line returns the employee as null

            if (compensation == null)
                return NotFound();

            
            return Ok(compensation);
        }
    }
}
