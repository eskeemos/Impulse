using System;
using System.Collections.Generic;
using System.Linq;

namespace Impulse.Shared.Domain.Service.Implementations
{
    public class CalculationsAvarage : ICalculations
    {
        #region Implemented functions

        public decimal CountAvarange(IEnumerable<decimal> values)
        {
            return values.Average();
        }

        public bool YesToBuy(int priceDrop)
        {
            throw new NotImplementedException();
        }

        public bool YesToSell(int priceRise)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
