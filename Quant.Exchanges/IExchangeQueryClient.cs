using CryptoExchange.Net.CommonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quant.Exchanges
{
    public interface IExchangeQueryClient
    {
        Task SubTickersAsync(CancellationToken ct);
        Task<IEnumerable<Kline>> GetKlinesAsync(string symbol);

        event Func<string, Ticker, Task>? OnTickerRecieved;
    }
}
