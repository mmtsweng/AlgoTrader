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
        public decimal winPercent { get
            {
                return ((decimal)wins / ((decimal)numberOfTrades / 2)) * 100;
            }}
        public decimal lossPercent { get
            { 
                return ((decimal)losses / ((decimal)numberOfTrades /2))* 100;
            }}
        public decimal initialCapital { get; set; }
        public decimal finalCapital { get; set; }
        public decimal maxCapital { get; set; }
        public decimal minCapital { get; set; }
        public decimal netProfitPercent { get
            {
                return ((finalCapital - initialCapital) / ((initialCapital + finalCapital)/2) * 100);
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
            this.wins = 0;
            this.losses = 0;
            this.initialCapital = 0;
            this.finalCapital = 0;
            this.maxCapital = 0;
            this.minCapital = 0;
            this.startDateTime = DateTime.MinValue;
            this.endDateTime = DateTime.MaxValue;
            this.numberOfTrades = 0;
            this.maxConsecutiveWins = 0;
            this.maxConsecutiveLosses = 0;
            this.winningTrades = new List<Trade>();
            this.losingTrades = new List<Trade>();
        }

        public void AnalyzeTrades (decimal accountbalance, ref List<Trade> trades)
        {
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
                            if (winCount > maxConsecutiveWins) { maxConsecutiveWins = winCount; }
                            this.wins++;
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

                            this.finalCapital += (tradecost-buytradecost);
                            if (this.finalCapital < this.minCapital) { this.minCapital = this.finalCapital; }
                        }

                        runningPortfolio.RemoveAt(buytradeidx); 
                    }
                }
            }
        }
    }
}
