using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impulse.Helpers
{
    public class DependencyProvider
    {
        public static IServiceProvider Get(IConfigurationRoot config)
        {
            #region Var

            ServiceCollection services = new ServiceCollection();

            #endregion

            #region Operations

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

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;

            #endregion
        }

    }
}
