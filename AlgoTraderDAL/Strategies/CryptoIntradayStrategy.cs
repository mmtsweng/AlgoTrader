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

        public int MarubozoPercent { get; set; }


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
        /// Method to parse the db parameters for this strategy
        /// </summary>
        public override void UpdateParameters()
        {
            base.UpdateParameters();

            this.MarubozoPercent = GetStrategyOption("MarubozoPercent", 95);
            this.OHLCQueueSize = GetStrategyOption("OHLCQueueSize", 50);
        }

        /// <summary>
        /// Figure out if we want to buy
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override bool BuySignal()
        {
            if (this.OHLCs.Count < 20) { return false; } //warmup

            IEnumerable<CandleResult> candles = this.OHLCs.GetMarubozu(this.MarubozoPercent);
            SlopeResult slope = null;
            try
            {
                IEnumerable<VwmaResult> vwema = this.OHLCs.GetVwma(60);
                IEnumerable<SlopeResult> slopeVals = vwema.GetSlope(17);
                slope = slopeVals.Last();
                if (slope.Line == null) { return false; }
            }
            catch (Exception ex)
            {
                return false;
            }

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
            if (this.OHLCs.Count < 20) { return false; } //Warmup

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
    }
}
