using CryptoExchange.Net.CommonObjects;
using Microsoft.AspNetCore.SignalR.Client;
using Quant.Exchanges;
using Quant.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quant.StrategyService
{
    public class MarketClient
    {
        private readonly ILogger<MarketClient> logger;
        private readonly IConfiguration configuration;
        private readonly List<IStrategy> strategies;
        private readonly HubConnection hubConnection;

        public MarketClient(ILogger<MarketClient> logger, IConfiguration configuration, List<IStrategy> strategies)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.strategies = strategies;
            hubConnection = new HubConnectionBuilder()
                .WithUrl(configuration.GetSection("MarketHubUrl").Value)
                .WithAutomaticReconnect()
                .Build();
            hubConnection.Reconnected += OnHubConnected;
            MarketClientEventInit();
        }

        public async Task StartAsync(CancellationToken ct)
        {
            await hubConnection.StartAsync(ct);
            await OnHubConnected(null);
        }

        void MarketClientEventInit()
        {
            hubConnection.On<string, Ticker>(nameof(OnTickerChanged), OnTickerChanged);
        }

        async Task OnHubConnected(string? connectionId)
        {
            foreach (var strategy in strategies)
            {
                await hubConnection.SendAsync("OnStrategyConnected", strategy.Tag);
            }

            logger.LogInformation("Strategy service connected.");
        }

        void OnTickerChanged(string tag, Ticker ticker)
        {
            strategies.Where(s => s.Tag == tag).AsParallel().ForAll(s => s.OnTickerChanged(ticker));
        }
    }
}
