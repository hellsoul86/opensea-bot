using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quant.Strategies
{
    public class StrategyFactory
    {
        private readonly ILogger<StrategyFactory> logger;

        public StrategyFactory(ILogger<StrategyFactory> logger)
        {
            this.logger = logger;
        }

        public List<IStrategy> GetStrategies()
        {
            return new List<IStrategy>() 
            { 
                new DemoStrategy(logger, "BTCUSDT"), 
                new DemoStrategy(logger, "ETHUSDT"), 
                new DemoStrategy(logger, "BNBUSDT") 
            };
        }
    }
}
