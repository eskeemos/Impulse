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
    public class DepedencyProvider
    {
        public static IServiceProvider Get(IConfigurationRoot config)
        {
            var services = new ServiceCollection();

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

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;

        }
    }
}
