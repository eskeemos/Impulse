using System.Collections.Generic;

namespace Impulse.Shared.Domain.Service
{
    public interface ICalculations
    {
        /// <summary>
        /// Count main avarage of all avarages from interval
        /// </summary>
        /// <param name="values">Interval avarange</param>
        /// <returns>Values avarange</returns>
        decimal CountAvarange(IEnumerable<decimal> values);

        /// <summary>
        /// Condition to fulfill to buy
        /// </summary>
        /// <param name="priceDrop">Percentage drop</param>
        /// <returns>true of false</returns>
        bool YesToBuy(int priceDrop);

        /// <summary>
        /// Condition to fulfill to sell
        /// </summary>
        /// <param name="priceRise">Percentage increase</param>
        /// <returns>true of false</returns>
        bool YesToSell(int priceRise);
    }
}
