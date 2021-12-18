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
        public static string CurrentPrice(StrategyInfo strategyInfo, decimal price, decimal availableQuote)
            => $"Current price for {strategyInfo.Symbol} is {price}. Available to trade: {availableQuote} ({strategyInfo.FundPercentage}%)";

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
        public static string SellResultStart(long orderId)
            => $"Start selling order ({orderId})";

        // TODO
        public static string SellResult(BinanceOrderTrade item)
            => $"Order filled with Quantity ({item.Quantity}), Price ({item.Price}, Commision ({item.Commission} {item.CommissionAsset}))";

        // TODO
        public static string SellResultEnd(long orderId)
            => $"End selling order ({orderId})";

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
            => $"Start buying order ({orderId})";

        // TODO
        public static string BuyResult(BinanceOrderTrade item)
            => $"Order filled with Quantity ({item.Quantity}), Price ({item.Price}, Commision ({item.Commission} {item.CommissionAsset}))";

        // TODO
        public static string BuyResultEnd(long orderId)
            => $"End buying order ({orderId})";

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

        public static string StopLossOrder(MarketResponse marketResponse)
            => $"Stop lose order ({marketResponse.IsReadyForMarket}), price change ({marketResponse.PercentChanged})";

        public static string StopLossOrderReady(decimal price, MarketResponse marketResponse, StrategyInfo strategy)
            => $"Price ({price}) dropped ({marketResponse.PercentChanged}%) > ({strategy.StopLosePercentageDown}%), selling all {strategy.Symbol}";

        public static string StopLossTest 
            => "Stop lose in test mode";

        public static string StopLossResultStart(long orderId)
            => $"(Stop lose) start selling order ({orderId})";

        public static string StopLossResult(BinanceOrderTrade item)
            => $"Order filled with quantity ({item.Quantity}), price ({item.Price}), commision ({item.Commission} {item.CommissionAsset})";

        public static string StopLossResultEnd(long orderId)
            => $"(Stop lose) end selling order ({orderId})";
    }
}
