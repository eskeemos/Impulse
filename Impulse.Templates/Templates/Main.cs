using System.Collections.Generic;

namespace Impulse.Shared.Templates
{
    public class Main
    {
        public int ActiveId { get; set; }
        public int IntervalInMinutes { get; set; }
        public bool TestMode { get; set; }
        public IEnumerable<Strategy> strategies { get; set; }
    }
}