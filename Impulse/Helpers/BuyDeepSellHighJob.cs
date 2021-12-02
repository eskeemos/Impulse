using Binance.Net;
using Impulse.Shared.Domain.Templates;
using NLog;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Impulse.Helpers
{
    [DisallowConcurrentExecution]
    public class BuyDeepSellHighJob : IJob
    {
        private static readonly Logger logger = LogManager.GetLogger("IMPULSE");

        public async Task Execute(IJobExecutionContext context)
        {
            var exchange = (context.JobDetail.JobDataMap["Exchanges"] as IList<Exchange>).FirstOrDefault();
            var strategy = context.JobDetail.JobDataMap["Strategy"] as Strategy;
            var activeStrategy = strategy.Available.FirstOrDefault(item => item.Id == strategy.ActiveId);

            using (var client = new BinanceClient())
            {
                var avgPrice = await client.Spot.Market.GetCurrentAvgPriceAsync(activeStrategy.Symbol);

                if (avgPrice.Success)
                {
                    logger.Info($"Average price from last {avgPrice.Data.Minutes}min for {activeStrategy.Symbol} on {exchange.Name} is {avgPrice.Data.Price}");
                }
                else
                {
                    logger.Warn(avgPrice.Error.Message);
                }
            }
        }
    }
}