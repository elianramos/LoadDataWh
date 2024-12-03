using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using LoadDataWh.interfas;

namespace LoadDataWh.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration configuration; 

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dataService = scope.ServiceProvider.GetRequiredService<IDataServicesWorker>();

                        var result = await dataService.LoadDwh();

                        if (!result.Success)
                        {
                            // registrar en log manejar error
                            _logger.LogError("Error al cargar DWH: {message}", result.Message);

                           
                        }
                    }

                }



                // Configura el delay usando un valor configurado en la configuración
                int delayTime = configuration.GetValue<int>("timerTime");

                // Espera la cantidad de tiempo configurada antes de la siguiente iteración
                await Task.Delay(delayTime, stoppingToken);
            }
        }
    }
}