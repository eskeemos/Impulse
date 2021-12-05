using Impulse.Helpers;
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
        #region Var
        
        // App name
        private static readonly string appName = "impulse";
        // Full log file name
        private static readonly string fileName = $"{appName}-{DateTime.UtcNow:ddMMyyyy}.log";
        // Object that schedules units of work
        private static IScheduler scheduler;
        // Provide a mechanism for obtaining IScheduler instances
        private static ISchedulerFactory schedulerFactory;

        #endregion

        #region Action

        static async Task<int> Main()
        {
            // Create(if not exists) and name file;
            NLog.LogManager.Configuration.Variables["fileName"] = fileName;
            NLog.LogManager.Configuration.Variables["archiveFileName"] = fileName;

            // Create settings based on specified file
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{appName}.json");

            // Compile settings 
            var config = configBuilder.Build();

            // Connect object with configs
            var app = config.Get<App>();


            try
            {
                // not yet
                var serviceProvider = DependencyProvider.Get(config);

                schedulerFactory = new StdSchedulerFactory();

                scheduler = await schedulerFactory.GetScheduler();

                await scheduler.Start();



                IJobDetail jobDetail = JobBuilder.Create<BuyDeepSellHighJob>()
                    .WithIdentity("BuyDeepSellHighJob")
                    .Build();

                jobDetail.JobDataMap["Strategy"] = app.Strategy;
                jobDetail.JobDataMap["Exchanges"] = app.Exchanges;

                var tBuilder = TriggerBuilder.Create()
                    .WithIdentity("BuyDeepSellHighJobTrigger")
                    .StartNow();

                tBuilder.WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    //.WithIntervalInMinutes(1)
                    .RepeatForever());

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

        //layout="${date}|${level:uppercase=true}|${logger}| ${message} ${exception}" />
    }
}
