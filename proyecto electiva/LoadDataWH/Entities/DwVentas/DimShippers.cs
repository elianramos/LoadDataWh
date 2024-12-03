using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDataWh.Entities.DwVentas
{

    
        [Table("DimShippers", Schema = "dbo")]
        public class DimShippers
        {
            [Key]
            public int ShipperID { get; set; }
            public string? CompanyName { get; set; }
            public string? Phone { get; set; }
        }
    
}
