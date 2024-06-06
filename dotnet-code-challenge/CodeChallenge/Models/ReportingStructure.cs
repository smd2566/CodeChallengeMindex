using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    //new model "ReportingStructure"
    public class ReportingStructure
    {
        //Im matching the UpperCase structure of the Employee model for consistency
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }
        
    }
}
