using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Objects.Spot.SpotData;
using CryptoExchange.Net.Objects;
using Impulse.Shared.Contexts;
using Impulse.Shared.Service;
using Impulse.Shared.Statics;
using Impulse.Shared.Templates;
using Impulse.Shared.Extensions;
using NLog;
using Quartz;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Impulse.Helpers
{

    #region Configs

    [DisallowConcurrentExecution]

    #endregion

    public class BuyDeepSellHighJob : IJob
    {

        private static readonly Logger logger = LogManager.GetLogger("IMPULSE");
        private readonly IStorage storage;
        private readonly IMarket market;

        public BuyDeepSellHighJob(IStorage _storage, IMarket _market)
        {
            storage = _storage;
            market = _market;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Main main = context.JobDetail.JobDataMap["Main"] as Main;
                Strategy strategyInfo = main.strategies.FirstOrDefault(item => item.Id == main.ActiveId);

                if(!(strategyInfo is null))
                {
                    storage.SetPath(Path.Combine(strategyInfo.StoragePath, $"{strategyInfo.Symbol}.txt"));

                    using(var client = new BinanceClient())
                    {
                        var ticker = new Ticker(client);
                        var accountInfo = await client.General.GetAccountInfoAsync();

                        if (accountInfo.Success)
                        {
                            var exchangeInfo = await client.Spot.System.GetExchangeInfoAsync();

                            if(exchangeInfo.Success)
                            {
                                var symbol = exchangeInfo.Data.Symbols.FirstOrDefault((s) => s.Name == strategyInfo.Symbol);

                                if(!(symbol is null) && symbol.Status == SymbolStatus.Trading)
                                {
                                    var baseAsset = symbol.BaseAsset;
                                    var quoteAsset = symbol.QuoteAsset;

                                    var currentPrice = await ticker.GetPrice(strategyInfo);

                                    if (currentPrice.Success)
                                    {
                                        var price = currentPrice.Result;
                                        var baseA = accountInfo.Data.Balances.FirstOrDefault(x => x.Asset == baseAsset).Free;
                                        var quoteA = accountInfo.Data.Balances.FirstOrDefault(x => x.Asset == quoteAsset).Free;

                                        quoteA = market.AvailableQuote(strategyInfo.PercentageResourceToPlay, quoteA, symbol.QuoteAssetPrecision).QuoteAssetToTrade;

                                        logger.Info(LogGenerator.GetCurrentPriceInfo(strategyInfo, price));

                                        storage.SaveValue(price);

                                        var storedAvg = Average.CountAverage(storage.GetValues(), 8, strategyInfo.AveragesAmount);

                                        logger.Info(LogGenerator.GetAveragePriceInfo(strategyInfo, storedAvg));

                                        // SELL BOTH
                                        if (baseA > 0.0m && baseA > symbol.MinNotionalFilter.MinNotional)
                                        {
                                            // STOP LOSE
                                            var stopLossOrder = market.YesToStopLose(strategyInfo.PercentageStopLose, storedAvg, price);

                                            logger.Info(LogGenerator.StopLossOrder(stopLossOrder));

                                            if (stopLossOrder.GoodToTrade)
                                            {
                                                logger.Info(LogGenerator.StopLossOrderReady(price, stopLossOrder, strategyInfo));

                                                if (main.TestMode)
                                                {
                                                    WebCallResult<BinancePlacedOrder> stopLossOrderResult = null;

                                                    var quantity = BinanceHelpers.ClampQuantity(symbol.LotSizeFilter.MinQuantity, symbol.LotSizeFilter.MaxQuantity, symbol.LotSizeFilter.StepSize, baseA);

                                                    if (strategyInfo.SellType == 0)
                                                    {
                                                        var minNational = quantity * price;

                                                        if(minNational > symbol.MinNotionalFilter.MinNotional)
                                                        {
                                                            stopLossOrderResult = await client.Spot.Order.PlaceOrderAsync(
                                                                strategyInfo.Symbol,
                                                                OrderSide.Sell,
                                                                OrderType.Market,
                                                                quantity: quantity);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var stopLossPrice = BinanceHelpers.ClampPrice(symbol.PriceFilter.MinPrice, symbol.PriceFilter.MaxPrice, price);
                                                        var minNational = quantity * stopLossPrice;

                                                        if(minNational > symbol.MinNotionalFilter.MinNotional)
                                                        {
                                                            stopLossOrderResult = await client.Spot.Order.PlaceOrderAsync(
                                                            strategyInfo.Symbol,
                                                            OrderSide.Sell,
                                                            OrderType.StopLossLimit,
                                                            quantity: quantity,
                                                            stopPrice: BinanceHelpers.FloorPrice(symbol.PriceFilter.TickSize, stopLossPrice),
                                                            price: BinanceHelpers.FloorPrice(symbol.PriceFilter.TickSize, stopLossPrice),
                                                            timeInForce: TimeInForce.GoodTillCancel);
                                                        }
                                                    }

                                                    if (!(stopLossOrderResult is null))
                                                    {
                                                        if (stopLossOrderResult.Success)
                                                        {
                                                            logger.Info(LogGenerator.StopLossResultStart(stopLossOrderResult.Data.OrderId));

                                                            if (stopLossOrderResult.Data.Fills.AnyAndNotNull())
                                                            {
                                                                foreach (var item in stopLossOrderResult.Data.Fills)
                                                                {
                                                                    logger.Info(LogGenerator.StopLossResult(item));
                                                                }
                                                            }

                                                            logger.Info(LogGenerator.StopLossResultEnd(stopLossOrderResult.Data.OrderId));
                                                        }
                                                        else
                                                        {
                                                            logger.Warn(stopLossOrderResult.Error.Message);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    logger.Info(LogGenerator.StopLossTest);
                                                }
                                            }

                                            // SELLBASE
                                            var sellOrder = market.YesToSell(strategyInfo.PercentagePriceRise, storedAvg, price);

                                            logger.Info(LogGenerator.SellOrderInfo(sellOrder));

                                            if (sellOrder.GoodToTrade)
                                            {
                                                logger.Info(LogGenerator.SellOrderReady(price, sellOrder, strategyInfo));

                                                if (main.TestMode)
                                                {
                                                    var quantity = BinanceHelpers.ClampQuantity(symbol.LotSizeFilter.MinQuantity, symbol.LotSizeFilter.MaxQuantity, symbol.LotSizeFilter.StepSize, quoteA);

                                                    var sellOrderResult = await client.Spot.Order.PlaceOrderAsync(
                                                        strategyInfo.Symbol,
                                                        OrderSide.Sell,
                                                        OrderType.Market,
                                                        quantity: quantity);

                                                    if (sellOrderResult.Success)
                                                    {
                                                        logger.Info(LogGenerator.SellResultStart(sellOrderResult.Data.OrderId));

                                                        if (sellOrderResult.Data.Fills.AnyAndNotNull())
                                                        {
                                                            foreach (var item in sellOrderResult.Data.Fills)
                                                            {
                                                                logger.Info(LogGenerator.SellResult(item));
                                                            }
                                                        }

                                                        logger.Info(LogGenerator.SellResultEnd(sellOrderResult.Data.OrderId));
                                                    }
                                                    else
                                                    {
                                                        logger.Info(sellOrderResult.Error.Message);
                                                    }
                                                }
                                                else
                                                {
                                                    logger.Info(LogGenerator.SellTest);
                                                }
                                            }
                                        }

                                        // BUYORDER
                                        if (quoteA > 0.0m && quoteA > symbol.MinNotionalFilter.MinNotional)
                                        {
                                            var buyOrder = market.YesToBuy(strategyInfo.PercentagePriceDrop, storedAvg, price);

                                            logger.Info(LogGenerator.BuyOrder(buyOrder));

                                            if (buyOrder.GoodToTrade)
                                            {
                                                logger.Info(LogGenerator.BuyOrderReady(price, buyOrder, strategyInfo));

                                                if (main.TestMode)
                                                {
                                                    var quantity = BinanceHelpers.ClampQuantity(symbol.LotSizeFilter.MinQuantity, symbol.LotSizeFilter.MaxQuantity, symbol.LotSizeFilter.StepSize, quoteA);

                                                    var buyOrderResult = await client.Spot.Order.PlaceOrderAsync(
                                                        strategyInfo.Symbol,
                                                        OrderSide.Buy,
                                                        OrderType.Market,
                                                        quoteOrderQuantity: quoteA
                                                        );

                                                    if (buyOrderResult.Success)
                                                    {
                                                        logger.Info(LogGenerator.BuyResultStart(buyOrderResult.Data.OrderId));

                                                        if (buyOrderResult.Data.Fills.AnyAndNotNull())
                                                        {
                                                            foreach (var item in buyOrderResult.Data.Fills)
                                                            {
                                                                logger.Info(LogGenerator.BuyResult(item));
                                                            }
                                                        }

                                                        logger.Info(LogGenerator.BuyResultEnd(buyOrderResult.Data.OrderId));
                                                    }
                                                    else
                                                    {
                                                        logger.Warn(buyOrderResult.Error.Message);
                                                    }
                                                }
                                                else
                                                {
                                                    logger.Info(LogGenerator.BuyTest);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            logger.Warn(LogGenerator.WarnFilterMinNational(quoteAsset, quoteA, symbol.MinNotionalFilter.MinNotional));
                                        }
                                    }
                                    else
                                    {
                                        logger.Warn(currentPrice.Message);
                                    }
                                }
                                else
                                {
                                    logger.Warn(LogGenerator.WarnSymbol(strategyInfo.Symbol));
                                }
                            }
                        }
                        else
                        {
                            logger.Warn(LogGenerator.WarnKeys);
                        }
                    }
                }
                else
                {
                    logger.Warn(LogGenerator.WarnStrategy);
                }
            }
            catch (Exception e)
            {
                logger.Fatal($"{e.Message}");
            }
        }
    }
}