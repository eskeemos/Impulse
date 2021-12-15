using System.Collections.Generic;

namespace Impulse.Shared.Domain.Templates
{
    public class Strategy
    {
        // TODO
        public int ActiveId { get; set; }

        // TODO
        public int IntervalInMinutes { get; set; }

        // TODO
        public IEnumerable<StrategyInfo> StrategiesData { get; set; }

        // TODO
        public int TestMode { get; set; }

        // TODO
        public bool IsNotTestMode => TestMode == 0;

    }
}