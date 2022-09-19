using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Quant.Exchanges.Binance.Converters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Quant.Exchanges.Binance
{
    public class BinanceExchangeQueryClient : IExchangeQueryClient
    {
        private readonly BinanceSocketClient socketClient;
        private readonly BinanceClient client;
        public List<CallResult<UpdateSubscription>> Subs { get; }

        public event Func<string, Ticker, Task>? OnTickerRecieved;

        public BinanceExchangeQueryClient(string? proxyHost, int? proxyPort)
        {
            var staticOptions = new BinanceSocketClientOptions { AutoReconnect = true };
            var options = new BinanceClientOptions();
            if (!string.IsNullOrWhiteSpace(proxyHost) && proxyPort != null)
            {
                var apiProxy = new ApiProxy(proxyHost, proxyPort.Value);
                staticOptions.Proxy = apiProxy;
                options.Proxy = apiProxy;
            }
            socketClient = new BinanceSocketClient(staticOptions);
            client = new BinanceClient(options);
            Subs = new List<CallResult<UpdateSubscription>>();
        }

        public async Task SubTickersAsync(CancellationToken ct)
        {
            await SpotSubAsync(ct);
            await UsdSubAsync(ct);
            await CoinSubAsync(ct);
        }

        public async Task StopSubsAsync()
        {
            await socketClient.UnsubscribeAllAsync();
        }

        void ProcessMessage(IEnumerable<IBinanceTick> binanceTicks, ApiType apiType)
        {
            binanceTicks.AsParallel().ForAll(s =>
            {
                var ticker = TickerConverter.GetTicker(s);
                if (OnTickerRecieved != null)
                {
                    var tag = string.Format($"{ExchangeClientType.Binance}|{apiType}|{ticker.Symbol}");
                    OnTickerRecieved.Invoke(tag, ticker);
                }
            });
        }

        async Task SpotSubAsync(CancellationToken ct)
        {
            var spotSub = await socketClient!.SpotStreams.SubscribeToAllTickerUpdatesAsync(message =>
            {
                if (message != null && message.Data != null)
                {
                    ProcessMessage(message.Data, ApiType.Spot);
                }
            }, ct);
            if (spotSub.Success)
            {
                Subs.Add(spotSub);
            }
        }

        async Task UsdSubAsync(CancellationToken ct)
        {
            var sub = await socketClient!.UsdFuturesStreams.SubscribeToAllTickerUpdatesAsync(message =>
            {
                if (message != null && message.Data != null)
                {
                    ProcessMessage(message.Data, ApiType.Usd);
                }
            }, ct);
            if (sub.Success)
            {
                Subs.Add(sub);
            }
        }

        async Task CoinSubAsync(CancellationToken ct)
        {
            var sub = await socketClient!.CoinFuturesStreams.SubscribeToAllTickerUpdatesAsync(message =>
            {
                if (message != null && message.Data != null)
                {
                    ProcessMessage(message.Data, ApiType.Coin);
                }
            }, ct);
            if (sub.Success)
            {
                Subs.Add(sub);
            }
        }

        public async Task<IEnumerable<Kline>> GetKlinesAsync(string symbol)
        {
            var klines = await client.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol, KlineInterval.OneHour);
            if (klines.Data != null)
            {
                return klines.Data.Select(k => new Kline
                {
                    ClosePrice = k.ClosePrice,
                    HighPrice = k.HighPrice,
                    LowPrice = k.LowPrice,
                    OpenPrice = k.OpenPrice,
                    OpenTime = k.OpenTime,
                    Volume = k.Volume,
                    SourceObject = k
                });
            }
            return new List<Kline>();
        }
    }
}
