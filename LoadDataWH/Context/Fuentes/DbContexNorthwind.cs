﻿using LoadDataWh.Entities.DwVentas;
using LoadDataWh.Entities.NorthWind;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDataWh.Context.Fuentes
{
    public class DbContexNorthwind: DbContext
    {
        public DbContexNorthwind(DbContextOptions<DbContexNorthwind> dbContext) : base(dbContext)
        {

        }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Shippers> Shippers { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<ClientesAtendidos>ClientesAtendidos { get; set; }
        public DbSet<VistaVentas> VistaVentas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientesAtendidos>(Entities =>
            {
                Entities.HasNoKey()
                         .ToView("ClientesAtendidos","dbo");
            });

            modelBuilder.Entity<VistaVentas>(Entities =>
            {
                Entities.HasNoKey()
                        .ToView("VistaVentas","dbo");
            });

        }

    }
}
