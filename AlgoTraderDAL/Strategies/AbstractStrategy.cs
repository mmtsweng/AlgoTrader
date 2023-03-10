using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Plottable;
using AlgoTraderDAL.Types;
using ScottPlot;
using Polly.Fallback;
using System.Windows.Forms;

namespace AlgoTraderDAL.Strategies
{
    public class AbstractStrategy : IStrategy
    {
        public bool canOpenMultiplePositons { get; set; }
        public bool isIntraday { get; set; }
        public bool isSellable { get { return openPostions > 0; } }
        public bool isBuyable { get { return openPostions < maxOpenPositions; } }
        internal int OHLCQueueSize { get; set; }
        internal List<AlgoTraderDAL.Types.OHLC> OHLCs { get; set; }
        public int focusRange { get; set; }
        internal bool isLiveTrading { get; set; }
        public int openPostions { get; set; }
        public Analytics analytics { get; set; }
        public virtual Dictionary<string, string> dbParameters { get; set; }
        public virtual List<IPlottable> Indicators { get; set; }
        public int maxOpenPositions { get; set; }
        private Trade lastBuy { get; set; }

        public AbstractStrategy()
        {
            this.isIntraday = false;
            this.isLiveTrading = false;
            this.maxOpenPositions = 1;
            this.focusRange = 0;
            this.Indicators = new List<IPlottable>();
        }

        public virtual bool BuySignal()
        {
            throw new NotImplementedException();
        }

        public virtual Trade Close(AlgoTraderDAL.Types.OHLC ohlc)
        {
            Trade trade = null;
            if (this.openPostions > 0)
            {
                trade = MakeTrade(ohlc, TradeSide.SELL);
                this.openPostions--;
                this.lastBuy = null;
            }
            return trade;
        }

        public virtual void Init()
        {
            this.canOpenMultiplePositons = false;
            this.openPostions = 0;
            this.analytics = new Analytics();
            this.OHLCs = new List<AlgoTraderDAL.Types.OHLC>();
            this.OHLCQueueSize = 200;
            this.lastBuy = null;
            GetParametersFromDatabase();
        }

        internal void GetParametersFromDatabase()
        {
            using (Entities entities = new Entities())
            {
                string strategyName = this.GetType().Name;
                this.dbParameters = entities.StrategyOptions
                    .Where(s => s.StrategyName == strategyName)
                    .ToDictionary(d => d.Parameter, d => d.Value);
            }
            UpdateParameters();
        }

        public virtual void UpdateParameters()
        {
        }

        public virtual Trade MakeTrade(AlgoTraderDAL.Types.OHLC ohlc, TradeSide side)
        {
            Trade trade = new Trade(false)
            {
                side = side,
                symbol = ohlc.Symbol,
                transactionDateTime = ohlc.Timeframe,
                type = TradeType.MARKET
            };

            if (!isLiveTrading) //Backtesting Calculation
            {
                trade = BacktestTradeCalculations(ohlc, side, trade);
            }

            return trade;
        }

        /// <summary>
        /// Method to update the OHLC queue
        /// </summary>
        /// <param name="ohlc"></param>
        internal void UpdateOHLCQueue(AlgoTraderDAL.Types.OHLC ohlc)
        {
            this.OHLCs.Add(ohlc);
            if (this.OHLCs.Count > this.OHLCQueueSize)
            {
                this.OHLCs.RemoveAt(0);
            }
        }

        public virtual Trade Next(AlgoTraderDAL.Types.OHLC ohlc, bool updateQueue = true)
        {
            if (updateQueue) { UpdateOHLCQueue(ohlc); }
            return new Trade();
        }

        public virtual bool SellSignal()
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        /// Get Strategy Options
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal int GetStrategyOption (string optionName, int defaultValue)
        {
            int parsedVal = 0;

            if (int.TryParse(this.dbParameters[optionName], out parsedVal))
            {
                return parsedVal;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get Strategy Options
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal double GetStrategyOption(string optionName, double defaultValue)
        {
            double parsedVal = 0;

            if (double.TryParse(this.dbParameters[optionName], out parsedVal))
            {
                return parsedVal;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get Strategy Options
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal string GetStrategyOption(string optionName, string defaultValue)
        {
            string parsedVal = String.Empty;

            if (this.dbParameters[optionName] != null)
            {
                return parsedVal;
            }
            else
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// Method to calculate values of a trade during a backtest
        /// </summary>
        /// <param name="ohlc"></param>
        /// <param name="side"></param>
        /// <param name="trade"></param>
        private Trade BacktestTradeCalculations(Types.OHLC ohlc, TradeSide side, Trade trade)
        {
            if (side == TradeSide.BUY)
            {
                if (this.openPostions >= this.maxOpenPositions) { return null; }
                this.openPostions++;
                if (trade.dollarQuantity > 0)
                {
                    trade.quantity = (trade.dollarQuantity / ohlc.Open);
                    trade.actualPrice = trade.dollarQuantity;
                }
                else
                {
                    trade.actualPrice = ohlc.Open;
                }
                this.lastBuy = trade;
            }
            else //TradeSide.SELL
            {
                if (this.openPostions <= 0) { return null; }
                this.openPostions--;
                if (trade.dollarQuantity > 0 && lastBuy != null)
                {
                    trade.quantity = lastBuy.quantity;  
                    trade.actualPrice = ohlc.Open * lastBuy.quantity;
                }
                else
                {
                    trade.actualPrice = ohlc.Open;
                }
                lastBuy = null;
            }

            trade.submittedPrice = trade.actualPrice;

            return trade;
        }

    }
}
