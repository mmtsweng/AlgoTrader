using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL
{
    public class Portfolio
    {
        public List<Trade> trades = new List<Trade>();

        public string accountNumber { get; set; }
        public decimal startingBalance { get; set; }
        public decimal accountBalance { get; set; }
        public bool accountTradable { get; set; }
        public decimal amountAvailableToTrade { get
            {
                decimal runningTotal = accountBalance;
                foreach(Trade trade in trades)
                    if (trade.active)
                    {
                        runningTotal -= trade.actualPrice * trade.quantity;
                    }
                return runningTotal;
            }}

        /// <summary>
        /// Construtor - requires a starting balance
        /// </summary>
        /// <param name="startingBalance"></param>
        public Portfolio(decimal startingBalance)
        {
            this.startingBalance = startingBalance; 
        }

        internal void UpdatePortfolio(Trade trade)
        {
            switch(trade.side)
            {
                case TradeSide.NONE:
                    return;
                case TradeSide.BUY:
                    this.trades.Add(trade);
                    accountBalance -= trade.actualPrice;
                    return;
                case TradeSide.SELL:
                    this.trades.Add(trade);
                    accountBalance += trade.actualPrice;
                    return;
            }         
        }
    }
}
