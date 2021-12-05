using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace Impulse.Helpers
{
    public class DependencyProvider
    {
        #region Public

        public static IServiceProvider Get(IConfigurationRoot config)
        {
            // Create instance
            ServiceCollection services = new ServiceCollection();

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

            // Add transient service
            services.AddTransient<BuyDeepSellHighJob>();
            
            // Return builded service
            return services.BuildServiceProvider();
        }

        #endregion
    }
}
