using Binance.Net.Interfaces;
using Binance.Net.Objects.Spot.MarketData;
using Impulse.Shared.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impulse.Shared.Contexts
{
    public class Ticker
    {
        #region Variables

        private IBinanceClient binanceClient;

        #endregion

        #region Constructor

        public Ticker(IBinanceClient _binanceClient)
        {
            binanceClient = _binanceClient;
        }

        #endregion

        #region MyRegion

        public async Task<TickerResponse> GetPrice(StrategyInfo strategyInfo)
        {
            var result = new TickerResponse();

            if (strategyInfo.Ticker == 0)
            {
                var response = await binanceClient.Spot.Market.GetPriceAsync(strategyInfo.Symbol);

                if(response.Success)
                {
                    result.setResult(response.Data.Price);
                }
                else
                {
                    result.Message = response.Error.Message;
                }
            }
            else if (strategyInfo.Ticker == 1)
            {
                var response = await binanceClient.Spot.Market.GetCurrentAvgPriceAsync(strategyInfo.Symbol);

                if (response.Success)
                {
                    result.setResult(response.Data.Price);
                }
                else
                {
                    result.Message = response.Error.Message;
                }
            }
            else
            {
                result.Message = "Bad Ticker Definition";
            }

            return result;
        }

        public class TickerResponse
        {
            public decimal Result { get; private set; }
            public void setResult(decimal result)
            {
                Result = result;
            }
            public string Message { get; set; }
            public bool Success => string.IsNullOrWhiteSpace(Message);
        }
        #endregion
    }
}
