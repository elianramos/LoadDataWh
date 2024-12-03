using LoadDataWh.Context.Destino;
using LoadDataWh.Context.Fuentes;
using LoadDataWh.Entities.DwVentas;
using LoadDataWh.interfas;
using LoadDataWh.Result;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LoadDataWH.Services
{ 
        public class DataServicesWorker : IDataServicesWorker
        {
            private readonly DbContextDwhNorthwind dbContextDwhNorthwind;
            private readonly DbContexNorthwind dbContexNorthwind;
            public DataServicesWorker(DbContextDwhNorthwind dbContextDwhNorthwind, DbContexNorthwind dbContexNorthwind)
            {
                this.dbContextDwhNorthwind = dbContextDwhNorthwind;
                this.dbContexNorthwind = dbContexNorthwind;
            }
            public async Task<OperactionResult> LoadDwh()
            {
                OperactionResult result = new OperactionResult();
                try
                {
               
                    await LoadDimCustomers();
                    await LoadDimEmployees();
                    await LoadDimProductsCategory();
                    await LoadDimShippers();
                    await LoadDimDate();
                    await LoadFactClientesAtendidos();
                    await LoadFactVistaVentas();
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = $"Error cargando el DWH. {ex.Message}";
                }
                return result;
            }

        private async Task<OperactionResult> LoadFactClientesAtendidos()
        {
            OperactionResult result = new OperactionResult();

            try
            {
               
                var customServed = await dbContexNorthwind.ClientesAtendidos
                    .AsNoTracking()
                    .ToListAsync();

                int[] cust = customServed.Select(c => c.EmployeeID).ToArray();



                if (cust.Any())
                {
                    await dbContextDwhNorthwind.FactClientesAtendidos
                        .Where(o => cust.Contains(o.EmployeeID))
                        .ExecuteDeleteAsync();
                }

               
                var employees = await dbContextDwhNorthwind.DimEmployees
                    .Where(e => cust.Contains(e.EmployeeID))
                    .ToDictionaryAsync(e => e.EmployeeID);

                List<FactClientesAtendidos> factClientesAtendidos = new List<FactClientesAtendidos>();

                foreach (var item in customServed)
                {
                    if (employees.TryGetValue(item.EmployeeID, out var employee))
                    {
                        var factClientesAtendido = new FactClientesAtendidos
                        {
                            EmployeeID = employee.EmployeeID,
                            FullName = item.FullName ?? string.Empty, 
                            TotalCustomers = item.TotalCustomers
                        };

                        factClientesAtendidos.Add(factClientesAtendido);
                    }
                }

                
                await dbContextDwhNorthwind.FactClientesAtendidos.AddRangeAsync(factClientesAtendidos);
                await dbContextDwhNorthwind.SaveChangesAsync();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando FactClientesAtendidos: {ex.Message}";
            }

            return result;
        }



        private async Task<OperactionResult> LoadFactVistaVentas()
            {
                OperactionResult result = new OperactionResult();

                try
                {
                    var order = await dbContexNorthwind.VistaVentas.AsNoTracking().ToListAsync();

                   
                    int[] orde = order.Select(o => o.OrderID).ToArray();

                    if (orde.Any())
                    {
                        await dbContextDwhNorthwind.FactVistaVentas
                            .Where(o => orde.Contains(o.OrderID))
                           .AsNoTracking().ExecuteDeleteAsync();
                    }

                    List<FactVistaVentas> factSal = new List<FactVistaVentas>();


                  
                    foreach (var item in order)
                    {
                        var customer = await dbContextDwhNorthwind.DimCustomers
                            .SingleOrDefaultAsync(e => e.CustomerID == item.CustomerID);
                        var employee = await dbContextDwhNorthwind.DimEmployees
                            .SingleOrDefaultAsync(e => e.EmployeeID == item.EmployeeID);
                        var product = await dbContextDwhNorthwind.DimProductsCategory
                            .SingleOrDefaultAsync(e => e.ProductID == item.ProductID);
                        var shipper = await dbContextDwhNorthwind.DimShippers
                            .SingleOrDefaultAsync(e => e.ShipperID == item.ShipperID);
                        
                        var date = await dbContextDwhNorthwind.DimDate.SingleOrDefaultAsync(e => e.Fecha == item.OrderDate);


                        if (employee != null && customer != null && product != null && shipper != null && date != null)
                        {
                        FactVistaVentas factVistaVentas = new FactVistaVentas()
                            {
                                OrderID = item.OrderID,
                                CustomerID = customer.CustomerID,
                                CompanyName = customer.CompanyName,
                                EmployeeID = employee.EmployeeID,
                                FullName = employee.FullName,
                                ShipperID = shipper.ShipperID,
                                ShipperCompanyName = shipper.CompanyName,
                                ProductID = product.ProductID,
                                ProductName = product.ProductName,
                                ProductCount = item.ProductCount,
                                ShipCity = item.ShipCity,
                                OrderDate = date.Fecha,
                                Year = item.Year,
                                Month = item.Month,
                                TotalOrders = item.TotalOrders,
                                TotalSold = item.TotalSold
                            };

                            factSal.Add(factVistaVentas);
                        }
                    }
                  
                    await dbContextDwhNorthwind.FactVistaVentas.AddRangeAsync(factSal);
                    await dbContextDwhNorthwind.SaveChangesAsync();
                    result.Success = true;


                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = $"Error cargando el FactSales. {ex.Message}";
                }

                return result;
            }


            private async Task<OperactionResult> LoadDimDate()
            {
                OperactionResult result = new OperactionResult();

                try
                {
                var dimDates = await dbContexNorthwind.Orders
               .Where(order => order.OrderDate != null)
               .OrderBy(order => order.OrderDate)
               .Select(order => new DimDate
               {
                   Fecha = order.OrderDate,
                   NombreFecha = order.OrderDate.ToString("dd/MM/yyyy"),
                   Dia = order.OrderDate.Day,
                   Mes = order.OrderDate.Month,
                   Año = order.OrderDate.Year,
                   NombreDia = order.OrderDate.ToString("dddd"),
                   NombreMeses = order.OrderDate.ToString("MMMM"),
                   NombreAño = order.OrderDate.ToString("yyyy"),

         }).Distinct()
                .ToListAsync();

                    

                // Limpiar la tabla DimCity antes de insertar nuevos datos
                    await dbContextDwhNorthwind.Database.ExecuteSqlRawAsync("TRUNCATE TABLE DimDate");
               

                   
                    await dbContextDwhNorthwind.DimDate.AddRangeAsync(dimDates);
                    await dbContextDwhNorthwind.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = $"Error cargando la DimDate. {ex.Message}";
                }
                return result;
            }

            private async Task<OperactionResult> LoadDimShippers()
            {
                OperactionResult operaction = new OperactionResult();
                try
                {

                    var Shipper = await dbContexNorthwind.Shippers.AsNoTracking().Select(shipper => new DimShippers()
                    {
                        
                        ShipperID = shipper.ShipperID,
                        CompanyName = shipper.CompanyName,
                        Phone = shipper.Phone

                    }).ToListAsync();

              
                    int[] shipper = Shipper.Select(ship => ship.ShipperID).ToArray();

                    if (shipper.Any())
                    {
                        await dbContextDwhNorthwind.DimShippers.Where(s => shipper.Contains(s.ShipperID)).AsNoTracking().ExecuteDeleteAsync();
                    }

                    
                    await dbContextDwhNorthwind.DimShippers.AddRangeAsync(Shipper);
                    await dbContextDwhNorthwind.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    operaction.Success = false;
                    operaction.Message = $"Error cargando la DimShippers. {ex.Message}";
                }

                return operaction;

            }

            private async Task<OperactionResult> LoadDimProductsCategory()
            {
                OperactionResult operaction = new OperactionResult();

                try
                {
                    var product = (from Products in dbContexNorthwind.Products
                                   join categories in dbContexNorthwind.Categories
                                   on Products.CategoryID equals categories.CategoryID
                                   select new DimProductsCategory()
                                   {
                                       ProductID = Products.ProductID,
                                       ProductName = Products.ProductName,
                                       CategoryID = Products.CategoryID,
                                       QuantityPerUnit = Products.QuantityPerUnit,
                                       UnitPrice = Products.UnitPrice,
                                       UnitsInStock = Products.UnitsInStock,
                                       UnitsOnOrder = Products.UnitsOnOrder,
                                       ReorderLevel = Products.ReorderLevel,
                                       Discontinued = Products.Discontinued,
                                       CategoryName = categories.CategoryName,
                                       DescriptionCategory = categories.Description

                                   }).AsNoTracking().ToList();

                 
                    int[] produc = product.Select(p => p.ProductID).ToArray();

                    if (produc.Any())
                    {
                        await dbContextDwhNorthwind.DimProductsCategory.Where(p => produc.Contains(p.ProductID)).AsNoTracking().ExecuteDeleteAsync();
                    }

                   
                    await dbContextDwhNorthwind.DimProductsCategory.AddRangeAsync(product);
                    await dbContextDwhNorthwind.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    operaction.Success = false;
                    operaction.Message = $"Error cargando la DimProductsCategory. {ex.Message}";
                }

                return operaction;
            }

            private async Task<OperactionResult> LoadDimCustomers()
            {
                OperactionResult operaction = new OperactionResult();
                try
                {
                    
                

                    var customer = await dbContexNorthwind.Customers.AsNoTracking().Select(customer => new DimCustomers()
                    {
                        
                        CustomerID = customer.CustomerID,
                        CompanyName = customer.CompanyName,
                        ContactName = customer.ContactName,
                        ContactTitle = customer.ContactTitle,
                        Address = customer.Address,
                        City = customer.City,
                        Region = customer.Region,
                        PostalCode = customer.PostalCode,
                        Country = customer.Country,
                        Phone = customer.Phone,
                        Fax = customer.Fax

                    }).ToListAsync();

                    
                    string[] cust = customer.Select(c => c.CustomerID).ToArray();

                    if (cust.Any())
                    {
                        await dbContextDwhNorthwind.DimCustomers.Where(c => cust.Contains(c.CustomerID)).AsNoTracking().ExecuteDeleteAsync();
                    }

                    
                    await dbContextDwhNorthwind.DimCustomers.AddRangeAsync(customer);
                    await dbContextDwhNorthwind.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    operaction.Success = false;
                    operaction.Message = $"Error cargando la DimCustomers. {ex.Message}";
                }
                return operaction;
            }


            private async Task<OperactionResult> LoadDimEmployees()
            {
                OperactionResult operaction = new OperactionResult();
                try
                {


                    var employees = await dbContexNorthwind.Employees.AsNoTracking().Select(employee => new DimEmployees()
                    {
                        
                        EmployeeID = employee.EmployeeID,
                        FullName = string.Concat(employee.FirstName, " ", employee.LastName),
                        Title = employee.Title,
                        City = employee.City,
                        Region = employee.Region,
                        Country = employee.Country,
                        HomePhone = employee.HomePhone,
                        Extension = employee.Extension

                    }).ToListAsync();

                    
                    int[] emple = employees.Select(e => e.EmployeeID).ToArray();

                    if (emple.Any())
                    {
                        await dbContextDwhNorthwind.DimEmployees.Where(e => emple.Contains(e.EmployeeID)).AsNoTracking().ExecuteDeleteAsync();
                    }


                    
                    await dbContextDwhNorthwind.DimEmployees.AddRangeAsync(employees);
                    await dbContextDwhNorthwind.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    operaction.Success = false;
                    operaction.Message = $"Error cargando la DimEmployees. {ex.Message}";
                }

                return operaction;



            }
        }
    }