using Binance.Net;
using Impulse.Shared.Domain.Templates;
using NLog;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Impulse.Helpers
{

    // Disallow executing several instances concurrently
    [DisallowConcurrentExecution]
    public class BuyDeepSellHighJob : IJob
    {
        #region Var

            // Get loggers 'IMPULSE' from config file
            private static readonly Logger logger = LogManager.GetLogger("IMPULSE");

        #endregion

        #region Public

        public async Task Execute(IJobExecutionContext context)
        {
            /* Get exchange class data */
            Exchange exchange = (context.JobDetail.JobDataMap["Exchanges"] as IList<Exchange>).FirstOrDefault();
            Strategy strategy = context.JobDetail.JobDataMap["Strategy"] as Strategy;
            StrategyData activeStrategy = strategy.StrategiesData.FirstOrDefault(item => item.Id == strategy.ActiveId);

            // Do action using binance
            using (var client = new BinanceClient())
            {
                // Get avarange price of specified symbol
                var avgPrice = await client.Spot.Market.GetCurrentAvgPriceAsync(activeStrategy.Symbol);

                if (avgPrice.Success)
                {
                    // Customized message transferred into logger
                    logger.Info($"IM[{avgPrice.Data.Minutes}]|CP[{activeStrategy.Symbol}]|PN[{exchange.Name}]|PR[{avgPrice.Data.Price}]");
                }
                else
                {
                    // Exception message
                    logger.Warn(avgPrice.Error.Message);
                }
            }
        }

        #endregion
    }

}