using Impulse.Shared.Domain.Service;
using Impulse.Shared.Domain.Service.Implementations;
using Impulse.Shared.Domain.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Linq;

namespace Impulse.Helpers
{
    public class DependencyProvider
    {
        #region Public

        public static IServiceProvider Get(App app)
        {
            ServiceCollection services = new ServiceCollection();

            #region Logging

            // Create instance
            services.AddLogging(builder =>
            {
                /* Set logger Level and set properties and 
                templates access */
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageProperties = true,
                    CaptureMessageTemplates = true
                });
            });

            #endregion

            // Add transient service
            services.AddTransient<BuyDeepSellHighJob>();

            #region Services

            // Obtain strategy
            var strategy = app.Strategy.StrategiesData.FirstOrDefault(s => s.Id == app.Strategy.ActiveId);

            // TODO
            services.AddTransient<IStorage>(service =>
                new FileStorage(strategy.StoragePath));

            // TODO
            services.AddTransient<ICalculations, CalculationsAvarage>();

            #endregion

            // Return builded service
            return services.BuildServiceProvider();
        }

        #endregion
    }
}
