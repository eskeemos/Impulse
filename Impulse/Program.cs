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
        #region Variables
        
        // Application name
        private static readonly string appName = "impulse";
        // Names for files generated dynamically 
        private static readonly string fileName = $"{appName}-{DateTime.UtcNow:ddMMyyyy}.log";
        // Interface that schedules units of work
        private static IScheduler scheduler;
        // Provides obtains an IScheduler instance
        private static ISchedulerFactory schedulerFactory;

        #endregion

        static async Task<int> Main()
        {
            /* Create if not exists nlog file and set naming */
            NLog.LogManager.Configuration.Variables["fileName"] = fileName;
            NLog.LogManager.Configuration.Variables["archiveFileName"] = fileName;

            /* Create config settings based on file */
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{appName}.json");

            // Resources compilation
            var config = configBuilder.Build();

            // Connect object with configs
            var app = config.Get<App>();

            
            try
            {
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
                
                var bTrigger = tBuilder.Build();

                await scheduler.ScheduleJob(jobDetail, bTrigger);

                await Task.Delay(TimeSpan.FromSeconds(30));

                Console.ReadKey();
            }
            catch (Exception)
            {
                throw;
            }
            

            // Dispose all targets and close  logging
            NLog.LogManager.Shutdown();

            return 0;
        }
    }
}
