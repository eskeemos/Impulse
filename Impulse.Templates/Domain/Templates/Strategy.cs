using System.Collections.Generic;

namespace Impulse.Shared.Domain.Templates
{
    public class Strategy
    {
        /// <summary>
        /// Strategy id
        /// </summary>
        public int ActiveId { get; set; }

        /// <summary>
        /// Register interval
        /// </summary>
        public int IntervalInMinutes { get; set; }

        /// <summary>
        /// Collection of strategies info
        /// </summary>
        public IEnumerable<StrategyInfo> StrategiesData { get; set; }
    }
}