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

        /// <summary>
        /// Create list of service, configure them and set their permissions
        /// </summary>
        /// <param name="app">App data</param>
        /// <returns>Ready service</returns>
        public static IServiceProvider Get(App app)
        {
            ServiceCollection services = new ServiceCollection();

            var strategy = app.Strategy.StrategiesData.FirstOrDefault(s => s.Id == app.Strategy.ActiveId);

            services.AddLogging(builder =>
            {
                 builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageProperties = true,
                    CaptureMessageTemplates = true
                });
            });

            services.AddTransient<BuyDeepSellHighJob>();
            services.AddTransient<IStorage>(service => new FileStorage(strategy.StoragePath));
            services.AddTransient<IMarket, ConditionMarket>();

            return services.BuildServiceProvider();
        }

        #endregion
    }
}
