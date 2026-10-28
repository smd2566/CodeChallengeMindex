using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    //new model "Compensation"
    /// <summary>The salary and effective date for one employee. Stored in the compensation database, keyed by EmployeeId.</summary>
    public class Compensation
    {
        //Im matching the UpperCase structure of the Employee model for consistency
        //I added an EmployeeId field as the object needed a Key to function properly, and it made the most sense to have the Employee's id as the key
        [Key] public String EmployeeId { get; set; }

        public Employee Employee { get; set; }
       
        public decimal Salary { get; set; } //decimal rather than float so amounts with cents are stored exactly

        public String EffectiveDate { get; set; } //Making this a String as the JSON Serializer does not support a "DateOnly" datatype

    }
}
