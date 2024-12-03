using LoadDataWh.Entities.DwVentas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDataWh.Context.Destino
{
    public class DbContextDwhNorthwind : DbContext
    {

        public DbContextDwhNorthwind(DbContextOptions<DbContextDwhNorthwind> dbContext) : base(dbContext)
        {

        }
        public DbSet<DimCustomers> DimCustomers { get; set; }
        public DbSet<DimEmployees> DimEmployees { get; set; }
        public DbSet<DimProductsCategory> DimProductsCategory { get; set; }
        public DbSet<DimShippers> DimShippers { get; set; }
        public DbSet<DimDate> DimDate { get; set; }

        public DbSet<FactClientesAtendidos> FactClientesAtendidos { get; set; }
        public DbSet<FactVistaVentas> FactVistaVentas { get; set; }


    }
}

