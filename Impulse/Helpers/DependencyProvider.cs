using Impulse.Shared.Domain.Service;
using Impulse.Shared.Service;
using Impulse.Shared.Service.Implementations;
using Impulse.Shared.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

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
            services.AddTransient<IStorage, FileStorage>();
            services.AddTransient<IMarket, ConditionMarket>();

            return services.BuildServiceProvider();
        }

        #endregion
    }
}
