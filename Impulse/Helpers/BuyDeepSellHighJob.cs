using Binance.Net;
using Impulse.Shared.Contexts;
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
        private readonly IMarket market;

        #endregion

        #region Constructor

        /// <summary>
        /// Set up variables
        /// </summary>
        /// <param name="_storage">Storage referention</param>
        /// <param name="_calculations">Calculations referention</param>
        public BuyDeepSellHighJob(IStorage _storage, IMarket _market)
        {
            storage = _storage;
            market = _market;
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
            StrategyInfo strategyInfo = strategy.StrategiesData.FirstOrDefault(item => item.Id == strategy.ActiveId);

            using (var client = new BinanceClient())
            {
                var ticker = new Ticker(client);

                var avgPrice = await ticker.GetPrice(strategyInfo);

                if (avgPrice.Success)
                {
                    var price = avgPrice.Result;

                    storage.SaveValue(price);

                    var storedAvarage = Average.CountAverage(storage.GetValues(), strategyInfo.Average);

                    logger.Info($"IM[{strategy.IntervalInMinutes}]|SN[{strategyInfo.Symbol}]|PN[{exchange.Name}]|PR[{price}]");
                    logger.Info($"Average price for last {strategyInfo.Average} is {Math.Round(storedAvarage, 2)}");
                    var buyCondition = market.YesToBuy(strategyInfo.Rise, storedAvarage, price);

                    if(buyCondition)
                    {

                    }

                }
                else
                {
                    logger.Warn(avgPrice.Message);
                }
            }
        }

        #endregion
    }

}