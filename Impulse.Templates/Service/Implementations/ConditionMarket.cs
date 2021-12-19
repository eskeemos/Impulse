using Impulse.Shared.Service.Response;

namespace Impulse.Shared.Service.Implementations
{
    public class ConditionMarket : IMarket
    {
        public FundResponse AvailableQuote(int fundPercentage, decimal availableQuote, int precision)
        {
            var result = new FundResponse();

            if(fundPercentage >= 0 && fundPercentage <= 100)
            {
                result.QuoteAssetToTrade = decimal.Round(availableQuote * fundPercentage / 100, precision);
            }
            else
            {
                result.QuoteAssetToTrade = availableQuote;
            }

            return result;
        }
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

        public MarketResponse YesToStopLose(int percentStopLose, decimal storedAvarage, decimal price)
        {
            return new MarketResponse
            {
                IsReadyForMarket = storedAvarage > price
                ? 100 - (price / storedAvarage * 100) >= percentStopLose
                : false,
                PercentChanged = decimal.Round(100 - (price / storedAvarage * 100), 2)
            };
        }

        #endregion
    }
}
