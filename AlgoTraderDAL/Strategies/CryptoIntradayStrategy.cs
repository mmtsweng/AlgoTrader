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
        public int MarubozoPercent { get; set; }
        public int OHLCQueueSize { get; set; }

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
            int parsedVal = 0;

            if (int.TryParse(this.dbParameters["MarubozoPercent"], out parsedVal))
            {
                this.MarubozoPercent = parsedVal;
            }
            else
            {
                this.MarubozoPercent = 95;
            }

            if (int.TryParse(this.dbParameters["OHLCQueueSize"], out parsedVal))
            {
                this.OHLCQueueSize = parsedVal;
            }
            else
            {
                this.OHLCQueueSize = 50;
            }

            try
            {
                throw new Exception("Testing Exception Logging");
            }
            catch (Exception e)
            {
                ErrorLogger.Instance.LogException("CryptoIntradayStrategy.UpdateParameters", e);
            }

        }

        /// <summary>
        /// Process the next Stock iteration
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override Trade Next(OHLC ohlc)
        {
            this.OHLCs.Add(ohlc);
            if (this.OHLCs.Count > this.OHLCQueueSize)
            {
                this.OHLCs.RemoveAt(0);
            }

            return base.Next(ohlc);
        }

        /// <summary>
        /// Figure out if we want to buy
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override bool BuySignal()
        {
            if (this.OHLCs.Count < 20) { return false; }

            IEnumerable<CandleResult> candles = this.OHLCs.GetMarubozu(this.MarubozoPercent);
            if (candles.Last().Match == Match.BullSignal || candles.Last().Match == Match.BullConfirmed)
            {
                return true;
            }
            else
            {
                return false;
            }
                    
        }

        /// <summary>
        /// Figure out if we want to sell
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override bool SellSignal()
        {
            IEnumerable<CandleResult> candles = this.OHLCs.GetMarubozu(this.MarubozoPercent);

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
