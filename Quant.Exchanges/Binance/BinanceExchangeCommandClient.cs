using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Objects;
using Quant.Exchanges.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quant.Exchanges.Binance
{
    public class BinanceExchangeCommandClient : IExchangeCommandClient
    {
        public Account Account { get; }
        private readonly BinanceClient client;

        public BinanceExchangeCommandClient(Account account, ApiProxy? apiProxy = default)
        {
            Account = account;
            var options = new BinanceClientOptions { ApiCredentials = new ApiCredentials(account.ApiKey, account.ApiSecret) };
            if (apiProxy != null)
            {
                options.Proxy = apiProxy;
            }
            client = new BinanceClient(options);
        }

    }
}
