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
        public int openPostions { get; set; }
        public Analytics analytics { get; set; }

        public virtual bool BuySignal(OHLC ohlc)
        {
            throw new NotImplementedException();
        }

        public virtual void Close(OHLC ohlc)
        {
            if (this.openPostions > 0)
            {
                MakeTrade(ohlc, TradeSide.SELL);
            }
        }

        public virtual void Init()
        {
            this.canOpenMultiplePositons = false;
            this.openPostions = 0;
            this.analytics = new Analytics();
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
            if (this.canOpenMultiplePositons || this.openPostions < 1)
            {
                if (this.BuySignal(ohlc))
                {
                    trade = MakeTrade(ohlc, TradeSide.BUY);
                    this.openPostions++;
                    return trade;
                }
            }

            if (this.openPostions > 0)
            {
                if (this.SellSignal(ohlc))
                {
                    trade = MakeTrade(ohlc, TradeSide.SELL);
                    this.openPostions--;
                }
            }

            return trade;
        }

        public virtual bool SellSignal(OHLC ohlc)
        {
            throw new NotImplementedException();
        }
    }
}
