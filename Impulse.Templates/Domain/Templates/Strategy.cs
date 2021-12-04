using System.Collections.Generic;

namespace Impulse.Shared.Domain.Templates
{
    public class Strategy
    {
        public int ActiveId { get; set; }
        public int IntervalInMinutes { get; set; }
        public IEnumerable<AvailableStrategy> Available { get; set; }
    }
}