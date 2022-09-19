using CryptoExchange.Net.CommonObjects;
using Microsoft.AspNetCore.SignalR;
using Quant.Exchanges;

namespace Quant.DataApi.Hubs
{
    public class MarketHub : Hub
    {
        public List<IExchangeQueryClient> Exchanges { get; }

        public MarketHub(List<IExchangeQueryClient> exchanges)
        {
            var ct = new CancellationToken();
            Exchanges = exchanges;
            Exchanges.ForEach(e =>
            {
                e.SubTickersAsync(ct);
                e.OnTickerRecieved += OnTickerRecieved;
            });
        }

        public async Task OnStrategyConnected(string tag)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tag);
        }

        async Task OnTickerRecieved(string tag, Ticker ticker)
        {
            await Clients.Group(tag).SendAsync("OnTickerChanged",tag, ticker);
        }
    }
}
