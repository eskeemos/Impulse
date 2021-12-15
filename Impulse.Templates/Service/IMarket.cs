using Impulse.Shared.Domain.Service.Response;

namespace Impulse.Shared.Domain.Service
{
    public interface IMarket
    {
        // TODO
        MarketResponse YesToBuy(int conditionDrop, decimal storedAvarage, decimal price);


        // TODO
        MarketResponse YesToSell(int conditionRise, decimal storedAvarage, decimal price);
    }
}