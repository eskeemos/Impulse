using System.Collections.Generic;
using System.Linq;

namespace Impulse.Shared.Contexts
{
    public class Average
    {
        #region Public

        public static decimal CountAverage(IEnumerable<decimal> values, int last = 0)
        {
            decimal res = (last == 0)
                ? values.Average()
                : values.Count() > last ? values.TakeLast(last).Average() : values.Average();

            return res;
        }

        #endregion
    }
}
