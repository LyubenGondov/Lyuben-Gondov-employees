using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkEmployees.Models
{
    public class EmployeeFileData
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public string DateFrom { get; set; }
        [Required]
        public string DateTo { get; set; }

    }
}
