using Impulse.Helpers;
using Impulse.Shared.Domain.Templates;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Impulse
{
    class Program
    {
        private static readonly string appName = "impulse";
        private static readonly string fileName = $"{appName}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";
        static async Task<int> Main()
        {
            
            NLog.LogManager.Configuration.Variables["fileName"] = fileName;
            NLog.LogManager.Configuration.Variables["archiveFileName"] = fileName;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{appName}.json");

            var config = configBuilder.Build();

            var app = config.Get<App>();

            try
            {
                var serviceProvider = DepedencyProvider.Get(config);
            }
            catch (Exception)
            {

                throw;
            }

            return 0;
        }
    }
}
