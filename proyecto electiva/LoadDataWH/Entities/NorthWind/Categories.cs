using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDataWh.Entities.NorthWind
{

        [Table("Categories", Schema = "dbo")]
        public class Categories
        {
            [Key]
            public int CategoryID { get; set; }
            public string? CategoryName { get; set; }
            public string? Description { get; set; }
        }
    }

