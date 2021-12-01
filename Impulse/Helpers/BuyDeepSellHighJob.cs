using Binance.Net;
using Impulse.Shared.Domain.Templates;
using Microsoft.Extensions.Logging;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                //var avgPrice = await client.Spot.
            }


        }
    }
}
