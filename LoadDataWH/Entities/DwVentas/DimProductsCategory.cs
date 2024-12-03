using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDataWh.Entities.DwVentas
{  
        [Table("DimProductsCategory", Schema = "dbo")]
    public class DimProductsCategory
        {
            [Key]
            public int ProductID { get; set; }
            public string? ProductName { get; set; }
            public int? CategoryID { get; set; }
            public string? QuantityPerUnit { get; set; }
            public decimal? UnitPrice { get; set; }
            public short? UnitsInStock { get; set; }
            public bool? Discontinued { get; set; }
            public short? UnitsOnOrder { get; set; }
            public short? ReorderLevel { get; set; }
            public string? CategoryName { get; set; }
            public string? DescriptionCategory { get; set; }

        }
    }

