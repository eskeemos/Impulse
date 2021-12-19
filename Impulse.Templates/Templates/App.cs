using System.Collections.Generic;

namespace Impulse.Shared.Templates
{
    public class App
    {
        public string Name { get; set; }
        public IEnumerable<Exchange> Exchanges { get; set; }
        public Main Main { get; set; }
    }
}
