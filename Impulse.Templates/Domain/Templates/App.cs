using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impulse.Shared.Domain.Templates
{
    public class App
    {
        public string Name { get; set; }
        public IEnumerable<Exchange> Exchanges { get; set; }
        public Strategy Strategy { get; set; }
    }

}
