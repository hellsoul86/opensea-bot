using CryptoExchange.Net.CommonObjects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quant.Exchanges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quant.Strategies
{
    public class DemoStrategy : IStrategy
    {
        private readonly ILogger logger;

        private readonly ExchangeClientType exchangeClientType;
        private readonly ApiType apiType;
        private readonly string symbol;
        public string Tag { get { return string.Format($"{exchangeClientType}|{apiType}|{symbol}"); } }

        public DemoStrategy(ILogger logger, string symbol)
        {
            this.logger = logger;
            exchangeClientType = ExchangeClientType.Binance;
            apiType = ApiType.Usd;
            this.symbol = symbol;
        }

        public void OnTickerChanged(Ticker ticker)
        {
            logger.LogInformation($"Ticker: {JsonConvert.SerializeObject(ticker)}");
        }
    }
}
