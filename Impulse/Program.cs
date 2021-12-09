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
                // Set up configured service
                var serviceProvider = DependencyProvider.Get(app);

                // Create instance of new job
                var jobFactory = new JobFactory(serviceProvider);

                /* Create instance */
                schedulerFactory = new StdSchedulerFactory();
                scheduler = await schedulerFactory.GetScheduler();

                // TODO
                scheduler.JobFactory = jobFactory;

                // Start scheduler 
                await scheduler.Start();

                // Create job
                IJobDetail jobDetail = JobBuilder.Create<BuyDeepSellHighJob>()
                    .WithIdentity("BuyDeepSellHighJob")
                    .Build();

                /* Set up job data */
                jobDetail.JobDataMap["Strategy"] = app.Strategy;
                jobDetail.JobDataMap["Exchanges"] = app.Exchanges;

                // Create trigger
                var tBuilder = TriggerBuilder.Create()
                    .WithIdentity("BuyDeepSellHighJobTrigger")
                    .StartNow();

                // Set up trigger
                tBuilder.WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    //.WithIntervalInMinutes(app.Strategy.IntervalInMinutes)
                    .RepeatForever());

                // Compile trigger
                var bTrigger = tBuilder.Build();

                // Job schedule
                await scheduler.ScheduleJob(jobDetail, bTrigger);

                // Delay 
                await Task.Delay(TimeSpan.FromSeconds(30));

                // ReadKey
                Console.ReadKey();
            }
            catch (Exception)
            {
                throw;
            }

            // Close nlog
            NLog.LogManager.Shutdown();

            return 0;
        }

        #endregion
    }
}
