using System.Collections.Generic;

namespace Impulse.Shared.Domain.Templates
{
    public class App
    {
        // App name
        public string Name { get; set; }

        // Exchanges data
        public IEnumerable<Exchange> Exchanges { get; set; }

        // Strategy data
        public Strategy Strategy { get; set; }
    }
}
