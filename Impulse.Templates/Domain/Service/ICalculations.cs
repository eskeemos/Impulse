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
        /// <param name="conditionDrop">Percentage drop</param>
        /// <param name="storedAvarage">Price avarage</param>
        /// <param name="price">Current price</param>
        /// <returns>true of false</returns>
        bool YesToBuy(int conditionDrop, decimal storedAvarage, decimal price);


        /// <summary>
        /// Condition to fulfill to sell
        /// </summary>
        /// <param name="conditionRise">Percentage increase</param>
        /// <param name="storedAvarage">Price avarage</param>
        /// <param name="price">Current price</param>
        /// <returns>true of false</returns>
        bool YesToSell(int conditionRise, decimal storedAvarage, decimal price);
    }
}