using CryptoExchange.Net.CommonObjects;

namespace Quant.Strategies
{
    public interface IStrategy
    {
        string Tag { get; }

        void OnTickerChanged(Ticker ticker);
    }
}