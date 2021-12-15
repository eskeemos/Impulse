using Impulse.Shared.Domain.Service.Response;

namespace Impulse.Shared.Domain.Service.Implementations
{
    public class ConditionMarket : IMarket
    {
        #region Implemented functions

        public MarketResponse YesToBuy(int priceDrop, decimal storedAvarage, decimal price)
        {
            return new MarketResponse
            {
                IsReadyForMarket = storedAvarage > price
                ? 100 - (price / storedAvarage * 100) >= priceDrop
                : false,
                PercentChanged = decimal.Round(100 - (price / storedAvarage * 100), 2)
            };
        }

        public MarketResponse YesToSell(int conditionRise, decimal storedAvarage, decimal price)
        {
            return new MarketResponse
            {
                IsReadyForMarket = price > storedAvarage
                ? 100 - ((storedAvarage * 100) / price) >= conditionRise
                : false,
                PercentChanged = decimal.Round(100 - ((storedAvarage * 100) / price), 2)
            };
        }

        #endregion
    }
}
