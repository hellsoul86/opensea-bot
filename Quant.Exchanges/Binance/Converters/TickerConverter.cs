using Binance.Net.Interfaces;
using CryptoExchange.Net.CommonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quant.Exchanges.Binance.Converters
{
    public class TickerConverter
    {
        public static Ticker GetTicker(IBinanceTick binanceTicker)
        {
            return new Ticker()
            {
                HighPrice = binanceTicker.HighPrice,
                LastPrice = binanceTicker.LastPrice,
                LowPrice = binanceTicker.LowPrice,
                Symbol = binanceTicker.Symbol,
                Volume = binanceTicker.Volume
            };
        }
    }
}
