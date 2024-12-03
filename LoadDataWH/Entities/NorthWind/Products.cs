using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDataWh.Entities.NorthWind

    {
    [Table("Products", Schema = "dbo")]
    public class Products
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

    }
}

