using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTraderDAL.Types
{
    public class Analytics
    {
        public string symbol { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public decimal winPercent
        {
            get
            {
                if (wins == 0) { return 0; }
                else { return ((decimal)wins / ((decimal)numberOfTrades / 2)) * 100; };
            }
        }
        public decimal lossPercent
        {
            get
            {
                if (wins == 0) { return 0; }
                else { return ((decimal)losses / ((decimal)numberOfTrades / 2)) * 100; };
            }
        }   
        public decimal initialCapital { get; set; }
        public decimal finalCapital { get; set; }
        public decimal maxCapital { get; set; }
        public decimal minCapital { get; set; }
        public decimal netProfitPercent { get
            {
                if (finalCapital == 0) { return 0; }
                else
                {
                    return ((finalCapital - initialCapital) / ((initialCapital + finalCapital) / 2) * 100);
                }
            }
        }
        public decimal netProfit { get { return this.finalCapital - this.initialCapital; } }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public int numberOfTrades { get; set; }
        public int maxConsecutiveWins { get; set; }
        public int maxConsecutiveLosses { get; set; }
        public List<Trade> winningTrades { get; set; }
        public List<Trade> losingTrades { get; set; }


        public Analytics()
        {
            Reset();
            this.startDateTime = DateTime.MinValue;
            this.endDateTime = DateTime.MaxValue;
        }

        public void Reset()
        {
            this.wins = 0;
            this.losses = 0;
            this.initialCapital = 0;
            this.finalCapital = 0;
            this.maxCapital = 0;
            this.minCapital = 0;
            this.numberOfTrades = 0;
            this.maxConsecutiveWins = 0;
            this.maxConsecutiveLosses = 0;
            this.winningTrades = new List<Trade>();
            this.losingTrades = new List<Trade>();
        }

        public void AnalyzeTrades (decimal accountbalance, ref List<Trade> trades)
        {
            if  (trades.Count == 0) { return; }
            int winCount = 0;
            int lossCount = 0;
            List<Trade> runningPortfolio = new List<Trade>();

            this.symbol = trades[0].symbol;
            this.initialCapital = accountbalance;
            this.finalCapital = accountbalance;
            this.minCapital = accountbalance;
            this.maxCapital = accountbalance;
            this.numberOfTrades = trades.Count;

            // Loop through trades
            foreach (Trade trade in trades)
            {
                decimal tradecost = trade.quantity * trade.actualPrice;
                if (trade.side == TradeSide.BUY)
                {
                    this.finalCapital -= trade.quantity * trade.actualPrice;
                    runningPortfolio.Add(trade);
                }
                else
                {
                    int buytradeidx = -1;
                    //Find buy order for this sell
                    for (var i = runningPortfolio.Count - 1; i >= 0; i--)
                    {
                        if (runningPortfolio[i].symbol == trade.symbol)
                        {
                            buytradeidx = i;
                            break;
                        }
                    }

                    decimal buytradecost = runningPortfolio[buytradeidx].quantity * runningPortfolio[buytradeidx].actualPrice;

                    if (buytradeidx != -1)
                    {
                        this.finalCapital += buytradecost;
                        if (tradecost > buytradecost)
                        {
                            //winning trade
                            winCount++;
                            lossCount = 0;
                            this.wins++;
                            if (winCount > maxConsecutiveWins) { maxConsecutiveWins = winCount; }
                            this.winningTrades.Add(trade);

                            this.finalCapital += (tradecost - buytradecost); 
                            if (this.finalCapital > this.maxCapital) { this.maxCapital = this.finalCapital; }
                        }
                        else 
                        {
                            //losing trade
                            lossCount++;
                            winCount = 0;
                            this.losses++;
                            if (lossCount > maxConsecutiveLosses) { maxConsecutiveLosses = lossCount; }
                            this.losingTrades.Add(trade);

                            this.finalCapital += (tradecost - buytradecost);
                            if (this.finalCapital < this.minCapital) { this.minCapital = this.finalCapital; }
                        }

                        runningPortfolio.RemoveAt(buytradeidx); 
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to get a copy of this object 
        ///    (used to store lists of analytics, since references update previous runs)
        /// </summary>
        /// <returns></returns>
        public Analytics Copy()
        {
            Analytics copy = new Analytics();
            var props = typeof(Analytics).GetProperties();
            var fields = typeof(Analytics).GetFields();
            foreach (var item in props)
            {
                if (item.CanWrite)
                {
                    item.SetValue(copy, item.GetValue(this));
                }
            }
            foreach (var item in fields)
            {
                item.SetValue(copy, item.GetValue(this));
            }
            return copy;
        }
    }
}
