using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    /// <summary>
    /// A person at the company. DirectReports forms the org-chart tree: each entry is itself an Employee
    /// with their own DirectReports. EF treats EmployeeId as the primary key by naming convention.
    /// </summary>
    public class Employee
    {
        public String EmployeeId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Position { get; set; }
        public String Department { get; set; }
        public List<Employee> DirectReports { get; set; }
    }
}
