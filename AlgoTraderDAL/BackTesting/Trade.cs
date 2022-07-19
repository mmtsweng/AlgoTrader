using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTraderDAL
{
    public enum TradeSide
    {
        NONE,
        BUY,
        SELL
    }

    public enum TradeType
    {
        MARKET,
        LIMIT, 
        STOP, 
        STOP_LIMIT,
        TRAILING_STOP
    }

    public class Trade
    {
        public bool liveTrade { get; set; }
        public string symbol { get; set; }
        public string clientOrderID { get; set; }
        public bool active { get; set; }
        public decimal actualPrice { get; set; }
        public decimal submittedPrice { get; set; }
        public decimal limitPrice { get; set; }
        public decimal stopLossPrice { get; set; }
        public int quantity { get; set; }
        public DateTime transactionDateTime { get; set; }
        public TradeType type { get; set; }
        public TradeSide side { get; set; }


        public Trade() : this(true)
        { 
        }

        public Trade(bool liveTrade)
        {
            this.side = TradeSide.NONE;
            this.liveTrade = liveTrade;
            this.active = false;
        }
    }
}
