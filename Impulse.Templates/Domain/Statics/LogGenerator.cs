using Binance.Net.Objects.Spot.SpotData;
using Impulse.Shared.Domain.Service.Response;
using Impulse.Shared.Domain.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impulse.Shared.Domain.Statics
{
    public class LogGenerator
    {
        // TODO
        public static string CurrentPrice(StrategyInfo strategyInfo, decimal price)
            => $"Current price for {strategyInfo.Symbol} is {price}";

        // TODO
        public static string AveragePrice(StrategyInfo strategyInfo, decimal storedPriceAverage)
            => $"Average price for last {strategyInfo.Average} is {storedPriceAverage}";

        // TODO
        public static string SellOrder(MarketResponse marketResponse)
            => $"Sell order ({marketResponse.IsReadyForMarket}), price change ({marketResponse.PercentChanged}%)";

        // TODO
        public static string SellOrderReady(decimal price, MarketResponse marketResponse, StrategyInfo strategyInfo)
            => $"Price ({price}) increased ({marketResponse.PercentChanged}%) > ({strategyInfo.Rise}%), selling {strategyInfo.Symbol}";

        // TODO
        public static string SellTest
            => "Sold in test mode";

        // TODO
        public static string BuyOrder(MarketResponse marketResponse)
            => $"Buy order ({marketResponse.IsReadyForMarket}), price change ({marketResponse.PercentChanged})%";

        // TODO
        public static string BuyOrderReady(decimal price, MarketResponse marketResponse, StrategyInfo strategyInfo)
            => $"Price ({price}) dropped ({marketResponse.PercentChanged}%) < ({strategyInfo.Drop}%), buying {strategyInfo.Symbol}";

        // TODO
        public static string BuyTest
            => "Buy in test mode";

        // TODO
        public static string BuyResultStart(long orderId)
            => $"Start order ({orderId})";

        // TODO
        public static string BuyResult(BinanceOrderTrade item)
            => $"Order filled with Quantity ({item.Quantity}), Price ({item.Price}, Commision ({item.Commission} {item.CommissionAsset}))";

        // TODO
        public static string BuyResultEnd(long orderId)
            => $"End order ({orderId})";

        // TODO
        public static string WarnSymbol(string symbol)
            => $"Provided symbol ({symbol}) is not supported";

        // TODO
        public static string WarnKeys
            => $"Apikey/secret is not correct";

        // TODO
        public static string WarnStrategy
            => $"Provided strategy is not correct";

        // TODO
        public static string WarnFilterMinNational(string quoteAsset, decimal availableQuote, decimal min)
            => $"Not enough {quoteAsset} ({availableQuote}), needed at least ({min})";
    }
}
