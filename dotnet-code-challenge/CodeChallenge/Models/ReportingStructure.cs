using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    //new model "ReportingStructure"
    /// <summary>
    /// API response shape for the reporting structure endpoint. It is calculated per request and never stored in a database.
    /// NumberOfReports counts all direct and indirect reports beneath Employee.
    /// </summary>
    public class ReportingStructure
    {
        //Im matching the UpperCase structure of the Employee model for consistency
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }
        
    }
}
