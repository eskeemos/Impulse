﻿using Impulse.Helpers;
using Impulse.Shared.Domain.Templates;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using System;
using System.IO;
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

                IJobDetail jobDetail = JobBuilder.Create<BuyDeepSellHighJob>()
                    .WithIdentity("BuyDeepSellHighJob").Build();

                jobDetail.JobDataMap["Strategy"] = app.Strategy;
                jobDetail.JobDataMap["Exchanges"] = app.Exchanges;

                var tBuilder = TriggerBuilder.Create()
                    .WithIdentity("BuyDeepSellHighJobTrigger").StartNow();

                tBuilder.WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5).RepeatForever());

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

        // close #3 ok
        // #03 ok
    }
}
