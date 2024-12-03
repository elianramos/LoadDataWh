using LoadDataWh.Context.Fuentes;
using LoadDataWh.interfas;
using LoadDataWH.Services;
using LoadDataWh.WorkerService;
using Microsoft.EntityFrameworkCore;
using LoadDataWh.Context.Destino;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContextPool<DbContextDwhNorthwind>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DbNorthwindConnection")));

                services.AddDbContextPool<DbContexNorthwind>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("NorthwindConnection")));



                // Otros servicios
                services.AddScoped<IDataServicesWorker, DataServicesWorker>();
                services.AddHostedService<Worker>();
            });

}