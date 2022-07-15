using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL.Strategies
{
    public class RandomBuySell : IStrategy
    {
        public int buyRate { get; set; }
        public int sellRate { get; set; }
        public bool canOpenMultiplePositons { get; set; }
        public int openPostions { get; set; }
        public Analytics analytics { get; set; }

        public void Init()
        {
            this.buyRate = 2; //simulate 20% of the time
            this.sellRate = 5; //simulate 50% the time
            this.canOpenMultiplePositons = false;
            this.openPostions = 0;  
            this.analytics = new Analytics();
        }

        public Trade Next(OHLC ohlc)
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

        public void Close(OHLC ohlc)
        {
            if (this.openPostions > 0)
            {
                MakeTrade(ohlc, TradeSide.SELL);
            }
        }

        public bool BuySignal(OHLC ohlc)
        {
            int buyRnd = new Random().Next(10) + 1;
            return (buyRnd <= this.buyRate);
        }

        public bool SellSignal(OHLC ohlc)
        {
            int sellRnd = new Random().Next(10) + 1;
            return (sellRnd <= this.sellRate);
        }

        public Trade MakeTrade(OHLC ohlc, TradeSide side)
        {
            Random random = new Random();
            Trade trade = new Trade(false)
            {
                submittedPrice = random.Next(Convert.ToInt32(ohlc.Low), Convert.ToInt32(ohlc.High)),
                side = side,
                symbol = ohlc.Symbol,
                quantity = 1,
                transactionDateTime = ohlc.Timeframe,
                type = TradeType.MARKET
            };

            if (side == TradeSide.BUY)
            {
                trade.actualPrice = random.Next(Convert.ToInt32(trade.submittedPrice), Convert.ToInt32(ohlc.High));
            }
            else
            {
                trade.actualPrice = random.Next(Convert.ToInt32(ohlc.Low), Convert.ToInt32(trade.submittedPrice));
            }

            return trade;
        }
    }
}
