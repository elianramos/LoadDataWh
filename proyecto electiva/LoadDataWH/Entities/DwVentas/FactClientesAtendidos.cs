using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDataWh.Entities.DwVentas
{
    
        [Table("FactClientesAtendidos", Schema = "dbo")]
        public class FactClientesAtendidos
    {
            [Key]
            public int EmployeeID { get; set; }
            public string? FullName { get; set; }
            public int TotalCustomers { get; set; }

        internal static void Add(FactClientesAtendidos factClientesAtendido)
        {
            throw new NotImplementedException();
        }
    }
    }

