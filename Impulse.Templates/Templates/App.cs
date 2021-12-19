using System.Collections.Generic;

namespace Impulse.Shared.Templates
{
    public class App
    {
        /// <summary>
        /// App name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of exchanges
        /// </summary>
        public IEnumerable<Exchange> Exchanges { get; set; }

        /// <summary>
        /// Strategy
        /// </summary>
        public Strategy Strategy { get; set; }
    }
}
