using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL.Strategies
{
    public class RandomBuySell : AbstractStrategy
    {
        public int buyRate { get; set; }
        public int sellRate { get; set; }

        public override void Init()
        {
            this.buyRate = 2; //simulate 20% of the time
            this.sellRate = 5; //simulate 50% the time
            base.Init();
        }

        public override bool BuySignal()
        {
            int buyRnd = new Random().Next(10) + 1;
            return (buyRnd <= this.buyRate);
        }

        public override bool SellSignal()
        {
            int sellRnd = new Random().Next(10) + 1;
            return (sellRnd <= this.sellRate);
        }

        public override Trade MakeTrade(OHLC ohlc, TradeSide side)
        {
            Trade trade = base.MakeTrade(ohlc, side);

            Random random = new Random();
            trade.submittedPrice = random.Next(Convert.ToInt32(ohlc.Low), Convert.ToInt32(ohlc.High));                

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
