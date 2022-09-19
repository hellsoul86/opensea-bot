namespace Quant.StrategyService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly MarketClient marketClient;

        public Worker(ILogger<Worker> logger, MarketClient marketClient)
        {
            _logger = logger;
            this.marketClient = marketClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await marketClient.StartAsync(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}