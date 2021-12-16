using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using Impulse.Helpers;
using Impulse.Shared.Domain.Templates;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Impulse
{
    class Program
    {
        #region Variables
        
        private static readonly string appName = "impulse";
        private static readonly string fileName = $"{appName}-{DateTime.UtcNow:ddMMyyyy}.log";
        private static IScheduler scheduler;
        private static ISchedulerFactory schedulerFactory;

        #endregion

        #region Main

        /// <summary>
        /// Obtain all necessary data, configure and plan systematic activity
        /// </summary>
        /// <returns>App</returns>
        static async Task<int> Main()
        {
            NLog.LogManager.Configuration.Variables["fileName"] = fileName;
            NLog.LogManager.Configuration.Variables["archiveFileName"] = fileName;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{appName}.json");

            var config = configBuilder.Build();

            var app = config.Get<App>();

            try
            {
                var serviceProvider = DependencyProvider.Get(app);

                var jobFactory = new JobFactory(serviceProvider);

                schedulerFactory = new StdSchedulerFactory();
                scheduler = await schedulerFactory.GetScheduler();

                scheduler.JobFactory = jobFactory;

                await scheduler.Start();

                var exchange = (app.Exchanges as IList<Exchange>).FirstOrDefault();

                BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                {
                    ApiCredentials = new ApiCredentials(exchange.ApiKey, exchange.ApiSecret)
                });

                IJobDetail jobDetail = JobBuilder.Create<BuyDeepSellHighJob>()
                    .WithIdentity("BuyDeepSellHighJob").Build();

                jobDetail.JobDataMap["Strategy"] = app.Strategy;

                var tBuilder = TriggerBuilder.Create()
                    .WithIdentity("BuyDeepSellHighJobTrigger").StartNow();

                tBuilder.WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(5).RepeatForever());

                var bTrigger = tBuilder.Build();

                await scheduler.ScheduleJob(jobDetail, bTrigger);

                await Task.Delay(TimeSpan.FromSeconds(30));

                Console.ReadKey();
            }
            catch (Exception)
            {
                throw;
            }

            NLog.LogManager.Shutdown();

            return 0;
        }

        #endregion
    }
}
