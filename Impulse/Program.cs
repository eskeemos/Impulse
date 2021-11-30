using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Impulse
{
    class Program
    {
        private static readonly string app = "impulse";
        static async Task<int> Main()
        {
            NLog.LogManager.Configuration.Variables["fileName"] = $"{app}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";
            NLog.LogManager.Configuration.Variables["archiveFileName"] = $"{app}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{app}.json");

            var config = configBuilder.Build();

            return 0;
        }
    }
}
