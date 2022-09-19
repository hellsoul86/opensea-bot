using Quant.Strategies;
using Quant.StrategyService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging();
        services.AddHostedService<Worker>();
        services.AddSingleton<MarketClient>();
        services.AddSingleton<StrategyFactory>();
        services.AddSingleton<List<IStrategy>>(provider => provider.GetService<StrategyFactory>()!.GetStrategies());
    })
    .Build();

await host.RunAsync();
