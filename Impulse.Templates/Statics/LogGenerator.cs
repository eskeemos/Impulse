using Binance.Net.Objects.Spot.SpotData;
using Impulse.Shared.Service.Response;
using Impulse.Shared.Templates;

namespace Impulse.Shared.Statics
{
    public class LogGenerator
    {
        public static string GetCurrentPriceInfo(Strategy strategy, decimal price)
            => $"Current price for {strategy.Symbol} is {price}";
        public static string GetAveragePriceInfo(Strategy strategy, decimal storedAverage)
            => $"Average price for {strategy.Symbol} is {storedAverage}, from last {strategy.AveragesAmount} logs";
        public static string SellOrderInfo(MarketResponse response)
            => $"Sell order [{response.GoodToTrade}], price change ({response.PercentChanged}%)";
        public static string SellOrderReady(decimal price, MarketResponse marketResponse, Strategy strategyInfo)
            => $"Price ({price}) increased ({marketResponse.PercentChanged}%) > ({strategyInfo.PercentagePriceRise}%), selling {strategyInfo.Symbol}";
        public static string SellTest
            => "Sold in test mode";
        public static string SellResultStart(long orderId)
            => $"Start selling order ({orderId})";
        public static string SellResult(BinanceOrderTrade item)
            => $"Order filled with Quantity ({item.Quantity}), Price ({item.Price}, Commision ({item.Commission} {item.CommissionAsset}))";
        public static string SellResultEnd(long orderId)
            => $"End selling order ({orderId})";
        public static string BuyOrder(MarketResponse marketResponse)
            => $"Buy order ({marketResponse.GoodToTrade}), price change ({marketResponse.PercentChanged})%";
        public static string BuyOrderReady(decimal price, MarketResponse marketResponse, Strategy strategyInfo)
            => $"Price ({price}) dropped ({marketResponse.PercentChanged}%) < ({strategyInfo.PercentagePriceDrop}%), buying {strategyInfo.Symbol}";
        public static string BuyTest
            => "Buy in test mode";
        public static string BuyResultStart(long orderId)
            => $"Start buying order ({orderId})";
        public static string BuyResult(BinanceOrderTrade item)
            => $"Order filled with Quantity ({item.Quantity}), Price ({item.Price}, Commision ({item.Commission} {item.CommissionAsset}))";
        public static string BuyResultEnd(long orderId)
            => $"End buying order ({orderId})";
        public static string WarnSymbol(string symbol)
            => $"Provided symbol ({symbol}) is not supported";
        public static string WarnKeys
            => $"Apikey/secret is not correct";
        public static string WarnStrategy
            => $"Provided strategy is not correct";
        public static string WarnFilterMinNational(string quoteAsset, decimal availableQuote, decimal min)
            => $"Not enough {quoteAsset} ({availableQuote}), needed at least ({min})"; 
        public static string StopLossOrder(MarketResponse marketResponse)
            => $"Stop lose order ({marketResponse.GoodToTrade}), price change ({marketResponse.PercentChanged})";
        public static string StopLossOrderReady(decimal price, MarketResponse marketResponse, Strategy strategy)
            => $"Price ({price}) dropped ({marketResponse.PercentChanged}%) > ({strategy.PercentageStopLose}%), selling all {strategy.Symbol}";
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
