using System.Collections.Generic;

namespace Impulse.Shared.Domain.Templates
{
    public class Strategy
    {
        public int ActiveId { get; set; }
        public string IntervalM { get; set; }
        public IEnumerable<AvailableStrategy> Available { get; set; }
    }
}