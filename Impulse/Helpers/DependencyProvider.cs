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
            // Create instance of serviceCollection
            ServiceCollection services = new ServiceCollection();

            // Add logging service 
            services.AddLogging(builder =>
            {
                // Set logger level, properties and templates access
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageProperties = true,
                    CaptureMessageTemplates = true
                });
            });

            // Add transient service
            services.AddTransient<BuyDeepSellHighJob>();

            // Obtain current strategy
            var strategy = app.Strategy.StrategiesData.FirstOrDefault(s => s.Id == app.Strategy.ActiveId);

            // TODO
            services.AddTransient<BuyDeepSellHighJob>();
            // TODO
            services.AddTransient<IStorage>(service => new FileStorage(strategy.StoragePath));
            // TODO
            services.AddTransient<ICalculations, CalculationsAvarage>();

            // Return ready services
            return services.BuildServiceProvider();
        }

        #endregion
    }
}
