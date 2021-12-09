using System;
using System.Collections.Generic;
using System.Linq;

namespace Impulse.Shared.Domain.Service.Implementations
{
    public class CalculationsAvarage : ICalculations
    {
        #region Public

        /* return avarange of values */
        public decimal CountAvarange(IEnumerable<decimal> values)
        {
            return values.Average();
        }

        // Buy condition
        public bool YesToBuy(int priceDropPercentage)
        {
            throw new NotImplementedException();
        }

        // Sell condition
        public bool YesToSell(int priceRisePercentage)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
