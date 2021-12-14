using System.Collections.Generic;

namespace Impulse.Shared.Domain.Service
{
    public interface IMarket
    {
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