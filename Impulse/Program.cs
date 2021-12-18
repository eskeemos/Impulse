using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using Impulse.Helpers;
using Impulse.Shared.Domain.Templates;
using Microsoft.Extensions.Configuration;
using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace Impulse
{
    class Program
    {
        

        private static readonly string appName = "impulse";
        private static readonly string fileName = $"{appName}-{DateTime.UtcNow:ddMMyyyy}.log";
        private static IScheduler scheduler;
        private static ISchedulerFactory schedulerFactory;
        private static readonly Logger logger = LogManager.GetLogger("IMPULSE");

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]

        static async Task<int> Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

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

                if(exchange.IsInTestMode)
                {
                    logger.Warn($"No ApiKey or ApiSecret provided, be sure to set 'testmode' = 1 in impulse.json file");
                }
                else
                {
                    BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                    {
                        ApiCredentials = new ApiCredentials(exchange.ApiKey, exchange.ApiSecret)
                    });
                }

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
            catch (Exception e)
            {
                logger.Fatal($"{e.Message}");
            }

            NLog.LogManager.Shutdown();

            return 0;
        }

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            logger.Fatal($"{e.Message}");
            logger.Fatal($"{args.IsTerminating}");
        }
    }
}
