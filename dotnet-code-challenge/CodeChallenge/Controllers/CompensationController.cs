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
    /// <summary>
    /// HTTP endpoints for the Task 2 requirement (api/compensation): store and retrieve an employee's salary and effective date.
    /// </summary>
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        /// <summary>
        /// POST api/compensation - saves a compensation record. The record is keyed by its EmployeeId, so the caller must
        /// supply it along with the nested Employee (400 otherwise). Returns 409 if a compensation already exists for that
        /// employee, or 201 with a Location header pointing at the GET endpoint.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCompensation([FromBody] Compensation compensation)
        {
            if (compensation == null || compensation.Employee == null || String.IsNullOrEmpty(compensation.EmployeeId))
                return BadRequest("A compensation requires an employeeId and a nested employee.");

            _logger.LogDebug($"Received compensation create request for '{compensation.Employee.FirstName} {compensation.Employee.LastName}'");

            //EmployeeId is the key, so saving a second compensation for the same employee would fail inside EF
            if (await _compensationService.GetByIdAsync(compensation.EmployeeId) != null)
                return Conflict($"A compensation already exists for employee '{compensation.EmployeeId}'.");

            await _compensationService.CreateAsync(compensation);

            return CreatedAtRoute("getCompensationById", new { employeeId = compensation.EmployeeId }, compensation);
        }

        /// <summary>
        /// GET api/compensation/{employeeId} - returns the stored compensation for that employee, including the nested
        /// Employee, or 404 if no compensation exists for that id.
        /// </summary>
        [HttpGet("{employeeId}", Name = "getCompensationById")]
        public async Task<IActionResult> GetCompensationById(String employeeId)
        {
            _logger.LogDebug($"Received compensation get request for '{employeeId}'");

            var compensation = await _compensationService.GetByIdAsync(employeeId);

            if (compensation == null)
                return NotFound();

            
            return Ok(compensation);
        }
    }
}
