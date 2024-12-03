using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDataWh.Entities.DwVentas
{

        [Table("DimEmployees", Schema = "dbo")]
        public class DimEmployees
        {
            [Key]
            public int EmployeeID { get; set; }
            public string? FullName { get; set; }
            public string? Title { get; set; }
            public string? City { get; set; }
            public string? Region { get; set; }
            public string? Country { get; set; }
            public string? HomePhone { get; set; }
            public string? Extension { get; set; }
        }
    }


