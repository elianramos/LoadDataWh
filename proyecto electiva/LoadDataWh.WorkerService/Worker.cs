using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using LoadDataWh.interfas;

namespace LoadDataWh.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dataService = scope.ServiceProvider.GetRequiredService<IDataServicesWorker>();
                    var result = await dataService.LoadDwh();
                }

                await Task.Delay(100000, stoppingToken);
            }
        }
    }
}