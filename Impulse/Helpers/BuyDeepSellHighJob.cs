using Binance.Net;
using Impulse.Shared.Domain.Service;
using Impulse.Shared.Domain.Templates;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Impulse.Helpers
{

    #region Configs

    [DisallowConcurrentExecution]

    #endregion

    public class BuyDeepSellHighJob : IJob
    {
        #region Variables

        private static readonly Logger logger = LogManager.GetLogger("IMPULSE");
        private readonly IStorage storage;
        private readonly ICalculations calculations;

        #endregion

        #region Constructor

        /// <summary>
        /// Set up variables
        /// </summary>
        /// <param name="_storage">Storage referention</param>
        /// <param name="_calculations">Calculations referention</param>
        public BuyDeepSellHighJob(IStorage _storage, ICalculations _calculations)
        {
            storage = _storage;
            calculations = _calculations;
        }

        #endregion

        #region Public

        /// <summary>
        /// Using the binance client causes data register into targets
        /// </summary>
        /// <param name="context">Task context</param>
        /// <returns>Logger act</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            Exchange exchange = (context.JobDetail.JobDataMap["Exchanges"] as IList<Exchange>).FirstOrDefault();
            Strategy strategy = context.JobDetail.JobDataMap["Strategy"] as Strategy;
            StrategyInfo activeStrategy = strategy.StrategiesData.FirstOrDefault(item => item.Id == strategy.ActiveId);

            using (var client = new BinanceClient())
            {
                var avgPrice = await client.Spot.Market.GetCurrentAvgPriceAsync(activeStrategy.Symbol);

                if (avgPrice.Success)
                {
                    storage.SaveValue(avgPrice.Data.Price);

                    logger.Info($"IM[{avgPrice.Data.Minutes}]|CP[{activeStrategy.Symbol}]|PN[{exchange.Name}]|PR[{avgPrice.Data.Price}]");
                    logger.Info($"AVG PRICE : {Math.Round(calculations.CountAvarange(storage.GetValues()), 4)}");
                }
                else
                {
                    logger.Warn(avgPrice.Error.Message);
                }
            }
        }

        #endregion
    }

}