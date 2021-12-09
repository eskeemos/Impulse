using System.Collections.Generic;

namespace Impulse.Shared.Domain.Service
{
    public interface ICalculations
    {
        // Count avarange
        decimal CountAvarange(IEnumerable<decimal> values);

        // Buy condition
        bool YesToBuy(int priceDropPercentage);

        // Sell condition
        bool YesToSell(int priceRisePercentage);
    }
}
