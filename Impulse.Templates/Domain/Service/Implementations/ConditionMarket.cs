using System;

namespace Impulse.Shared.Domain.Service.Implementations
{
    public class ConditionMarket : IMarket
    {
        #region Implemented functions

        public bool YesToBuy(int priceDrop, decimal storedAvarage, decimal price)
            => (storedAvarage > price) 
                ? 100 - (price / storedAvarage * 100) >= priceDrop
                : false;

        public bool YesToSell(int conditionRise, decimal storedAvarage, decimal price)
        {
            if(price > storedAvarage)
            {
                var part = price / storedAvarage;
                var realRise = decimal.Round(part - Math.Truncate(part), 2) * 100;

                return realRise > conditionRise;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
