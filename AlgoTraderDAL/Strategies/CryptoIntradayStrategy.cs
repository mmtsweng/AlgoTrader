using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skender.Stock.Indicators;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL.Strategies
{
    public class CryptoIntradayStrategy : AbstractStrategy
    {
        public List<OHLC> OHLCs { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CryptoIntradayStrategy()
        {
            base.isIntraday = true;
        }

        ///Overloaded Constructor <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="isLiveTrading"></param>
        public CryptoIntradayStrategy(bool isLiveTrading)
        {
            base.isLiveTrading = isLiveTrading;
            base.isIntraday = true;
        }

        /// <summary>
        /// Initialize Strategy
        /// </summary>
        public override void Init()
        {
            base.Init();

            this.OHLCs = new List<OHLC>();
        }

        /// <summary>
        /// Method to parse the db parameters for this strategy
        /// </summary>
        public override void UpdateParameters()
        {
            base.UpdateParameters();
        }

        /// <summary>
        /// Process the next Stock iteration
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override Trade Next(OHLC ohlc)
        {
            this.OHLCs.Add(ohlc);
            return base.Next(ohlc);
        }

        /// <summary>
        /// Figure out if we want to buy
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override bool BuySignal()
        {
            IEnumerable<SuperTrendResult> trend = this.OHLCs.GetSuperTrend(10, 3);
            if (trend != null && trend.LastOrDefault().SuperTrend != null)
            {
                int idx = trend.Count();
                if (trend.ElementAt(idx - 2).UpperBand == null)
                {
                    if (trend.ElementAt(idx - 1).LowerBand > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Figure out if we want to sell
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override bool SellSignal()
        {
            IEnumerable<CandleResult> candles = this.OHLCs.GetMarubozu(80);
            if (candles.Last().Match == Match.BearSignal || candles.Last().Match == Match.BearConfirmed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Make the actual trade
        /// </summary>
        /// <param name="ohlc"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public override Trade MakeTrade(OHLC ohlc, TradeSide side)
        {
            Trade trade = base.MakeTrade(ohlc, side);

            trade.actualPrice = ohlc.Low;
            trade.submittedPrice = trade.actualPrice;

            return trade;
        }

    }
}
