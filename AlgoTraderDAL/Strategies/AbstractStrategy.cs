using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL.Strategies
{
    public class AbstractStrategy : IStrategy
    {
        public bool canOpenMultiplePositons { get; set; }
        public bool isIntraday { get; set; }
        public bool isSellable { get { return openPostions > 0; } }
        public bool isBuyable { get { return openPostions < maxOpenPositions; } }
        internal bool isLiveTrading { get; set; }
        public int openPostions { get; set; }
        public Analytics analytics { get; set; }
        public virtual Dictionary<string, string> dbParameters { get; set; }
        public int maxOpenPositions { get; set; }

        public AbstractStrategy()
        {
            this.isIntraday = false;
            this.isLiveTrading = false;
            this.maxOpenPositions = 1;
        }

        public virtual bool BuySignal()
        {
            throw new NotImplementedException();
        }

        public virtual Trade Close(OHLC ohlc)
        {
            Trade trade = null;
            if (this.openPostions > 0)
            {
                trade = MakeTrade(ohlc, TradeSide.SELL);
                this.openPostions--;
            }
            return trade;
        }

        public virtual void Init()
        {
            this.canOpenMultiplePositons = false;
            this.openPostions = 0;
            this.analytics = new Analytics();
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

        public virtual Trade MakeTrade(OHLC ohlc, TradeSide side)
        {
            Trade trade = new Trade(false)
            {
                side = side,
                symbol = ohlc.Symbol,
                quantity = 1,
                transactionDateTime = ohlc.Timeframe,
                type = TradeType.MARKET
            };
            return trade;
        }

        public virtual Trade Next(OHLC ohlc)
        {
            Trade trade = new Trade();
            if (!isLiveTrading)
            {                
                if (this.canOpenMultiplePositons || this.openPostions < this.maxOpenPositions)
                {
                    if (this.BuySignal())
                    {
                        trade = MakeTrade(ohlc, TradeSide.BUY);
                        this.openPostions++;
                        return trade;
                    }
                }

                if (this.openPostions > 0)
                {
                    if (this.SellSignal())
                    {
                        trade = MakeTrade(ohlc, TradeSide.SELL);
                        this.openPostions--;
                    }
                }
            }

            return trade;
        }

        public virtual bool SellSignal()
        {
            throw new NotImplementedException();
        }
    }
}
