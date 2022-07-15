using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;
using AlgoTraderDAL.Strategies;

namespace AlgoTraderDAL.BackTesting
{
    public class BackTester
    {
        private const decimal STARTING_ACCOUNT_BALANCE = 1000.0M;

        public Portfolio portfolio { get; set; }
        public List<OHLC> historicalOHLC { get; set; }
        public IStrategy strategy { get; set; }
        public Analytics analytics { get; set; }

        public BackTester(IStrategy strategy, List<OHLC> history)
        {
            this.portfolio = new Portfolio(STARTING_ACCOUNT_BALANCE);
            this.analytics = new Analytics();
            this.strategy = strategy;
            this.historicalOHLC = history;   
        }

        public void RunBackTest()
        {
            
            this.strategy.Init();
            this.analytics.startDateTime = (historicalOHLC.OrderByDescending(t => t.Timeframe).LastOrDefault()).Timeframe;
            this.analytics.endDateTime = (historicalOHLC.OrderByDescending(t => t.Timeframe).FirstOrDefault()).Timeframe;

            foreach (OHLC ohlc in this.historicalOHLC)
            {
                this.portfolio.UpdatePortfolio(strategy.Next(ohlc));
            }

            strategy.Close(this.historicalOHLC[this.historicalOHLC.Count-1]);

            this.analytics.AnalyzeTrades(STARTING_ACCOUNT_BALANCE, ref this.portfolio.trades);
        }
    }
}
