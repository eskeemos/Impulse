using Binance.Net;
using Binance.Net.Enums;
using Impulse.Shared.Contexts;
using Impulse.Shared.Domain.Service;
using Impulse.Shared.Domain.Statics;
using Impulse.Shared.Domain.Templates;
using Impulse.Shared.Extensions;
using NLog;
using Quartz;
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
            Strategy strategy = context.JobDetail.JobDataMap["Strategy"] as Strategy;
            StrategyInfo strategyInfo = strategy.StrategiesData.FirstOrDefault(item => item.Id == strategy.ActiveId);

            if (!(strategyInfo is null))
            {
                storage.SetPath(Path.Combine(strategyInfo.StoragePath, $"{strategyInfo.Symbol}.txt"));

                using (var client = new BinanceClient())
                {
                    var ticker = new Ticker(client);
                    var account = await client.General.GetAccountInfoAsync();

                    if(account.Success)
                    {
                        var exchangeInfo = await client.Spot.System.GetExchangeInfoAsync();

                        if(exchangeInfo.Success)
                        {
                            var symbol = exchangeInfo.Data.Symbols.FirstOrDefault((s) => s.Name == strategyInfo.Symbol);

                            if (!(symbol is null) && symbol.Status == SymbolStatus.Trading)
                            {
                                var baseAsset = symbol.BaseAsset;
                                var quoteAsset = symbol.QuoteAsset;

                                var currentPrice = await ticker.GetPrice(strategyInfo);

                                if(currentPrice.Success)
                                {
                                    var price = currentPrice.Result;

                                    var baseA = account.Data.Balances.FirstOrDefault(x => x.Asset == baseAsset).Free;
                                    var quoteA = account.Data.Balances.FirstOrDefault(x => x.Asset == quoteAsset).Free;

                                    logger.Info(LogGenerator.CurrentPrice(strategyInfo, price));

                                    storage.SaveValue(price);

                                    var storedAvg = Average.CountAverage(storage.GetValues(), 8, strategyInfo.Average);

                                    logger.Info(LogGenerator.AveragePrice(strategyInfo, storedAvg));

                                    if(baseA > 0.0m)
                                    {
                                        var sellOrder = market.YesToSell(strategyInfo.Rise, storedAvg, price);

                                        logger.Info(LogGenerator.SellOrder(sellOrder));

                                        if(sellOrder.IsReadyForMarket)
                                        {
                                            logger.Info(LogGenerator.SellOrderReady(price, sellOrder, strategyInfo));

                                            if(strategy.IsNotTestMode)
                                            {
                                                var sellOrderResult = await client.Spot.Order.PlaceOrderAsync(
                                                    strategyInfo.Symbol,
                                                    OrderSide.Sell,
                                                    OrderType.Market,
                                                    quantity: baseA);

                                                if(sellOrderResult.Success)
                                                {
                                                    logger.Info(LogGenerator.SellResultStart(sellOrderResult.Data.OrderId));

                                                    if(sellOrderResult.Data.Fills.AnyAndNotNull())
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

                                    if(quoteA > 0.0m && quoteA > symbol.MinNotionalFilter.MinNotional)
                                    {
                                        var buyOrder = market.YesToBuy(strategyInfo.Drop, storedAvg, price);

                                        logger.Info(LogGenerator.BuyOrder(buyOrder));

                                        if(buyOrder.IsReadyForMarket)
                                        {
                                            logger.Info(LogGenerator.BuyOrderReady(price, buyOrder, strategyInfo));

                                            if (strategy.IsNotTestMode)
                                            {
                                                var buyOrderResult = await client.Spot.Order.PlaceOrderAsync(
                                                    strategyInfo.Symbol,
                                                    OrderSide.Buy, 
                                                    OrderType.Market,
                                                    quoteOrderQuantity: quoteA
                                                    );

                                                if(buyOrderResult.Success)
                                                {
                                                    logger.Info(LogGenerator.BuyResultStart(buyOrderResult.Data.OrderId));

                                                    if(buyOrderResult.Data.Fills.AnyAndNotNull())
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
    }
}