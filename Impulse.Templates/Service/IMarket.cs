using Impulse.Shared.Service.Response;

namespace Impulse.Shared.Service
{
    public interface IMarket
    {
        MarketResponse YesToBuy(int conditionDrop, decimal storedAvarage, decimal price);
        MarketResponse YesToSell(int conditionRise, decimal storedAvarage, decimal price);
        MarketResponse YesToStopLose(int percentStopLose, decimal storedAvarage, decimal price);
        FundResponse AvailableQuote(int fundPercentage, decimal availableQuote, int precision);
    }
}