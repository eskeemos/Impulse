using System.Collections.Generic;

namespace Impulse.Shared.Domain.Templates
{
    public class Strategy
    {
        // Id of current Strategy
        public int ActiveId { get; set; }

        // Interval of a new register
        public int IntervalInMinutes { get; set; }

        // Iteracja konkretnych strategii
        public IEnumerable<StrategyData> StrategiesData { get; set; }
    }
}