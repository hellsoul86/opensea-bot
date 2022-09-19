using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Configuration;
using Quant.Exchanges.Binance;
using Quant.Exchanges.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quant.Exchanges
{
    public class ExchangeClientFactory
    {
        public static IExchangeCommandClient? GetExchange(Account account)
        {
            var typeString = "Exchanges.{0}.{0}ExchangeClient";
            var type = Type.GetType(string.Format(typeString, account.ClientType.ToString()));
            if (type != null)
            {
                var exchangeClient = Activator.CreateInstance(type, account);
                if (exchangeClient != null)
                {
                    return (IExchangeCommandClient)exchangeClient;
                }
            }
            return default;
        }

        public static List<IExchangeQueryClient> GetQueryExchanges(IConfiguration configuration)
        {
            var exchanges = new List<IExchangeQueryClient>();
            string? proxyHost = null;
            int? proxyPort = null;
            try
            {
                proxyHost = configuration.GetSection("ProxyHost").Value;
                proxyPort = Convert.ToInt32(configuration.GetSection("ProxyPort").Value);
            }
            catch { }
            exchanges.Add(new BinanceExchangeQueryClient(proxyHost, proxyPort));
            return exchanges;
        }
    }
}