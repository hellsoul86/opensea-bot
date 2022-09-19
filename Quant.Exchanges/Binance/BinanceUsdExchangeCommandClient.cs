using Binance.Net.Interfaces.Clients.UsdFuturesApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quant.Exchanges.Binance
{
    public class BinanceUsdExchangeCommandClient : IExchangeCommandClient
    {
        public BinanceUsdExchangeCommandClient(IBinanceClientUsdFuturesApi api)
        {
            Api = api;
        }

        public IBinanceClientUsdFuturesApi Api { get; }
    }
}
