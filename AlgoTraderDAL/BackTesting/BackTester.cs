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
            if (historicalOHLC == null || historicalOHLC.Count == 0) { return; }

            this.strategy.Init();
            this.analytics.startDateTime = (historicalOHLC.OrderByDescending(t => t.Timeframe).LastOrDefault()).Timeframe;
            this.analytics.endDateTime = (historicalOHLC.OrderByDescending(t => t.Timeframe).FirstOrDefault()).Timeframe;
            Trade closingTrade = null;

            if (this.strategy.isIntraday)
            {
                closingTrade = RunIntradayBacktest(true);
            }
            else //Allow Positions to remain open after day closed.
            {
                foreach (OHLC ohlc in this.historicalOHLC)
                {
                    this.portfolio.UpdatePortfolio(strategy.Next(ohlc));
                }
                strategy.Close(this.historicalOHLC[this.historicalOHLC.Count - 1]);
            }

            AnalyzeTrades();
        }

        public void AnalyzeTrades()
        {
            this.analytics.AnalyzeTrades(STARTING_ACCOUNT_BALANCE, ref this.portfolio.trades);
        }

        internal Trade RunIntradayBacktest(bool firstRun)
        {
            if (firstRun)
            {
                this.strategy.Init();                
            }

            Trade closingTrade;
            if (historicalOHLC.Count < 1)
            {
                return null;
            }
            this.portfolio = new Portfolio(STARTING_ACCOUNT_BALANCE);
            OHLC trackingDay = historicalOHLC[0];
            this.analytics.startDateTime = (historicalOHLC.OrderByDescending(t => t.Timeframe).LastOrDefault()).Timeframe;
            this.analytics.endDateTime = (historicalOHLC.OrderByDescending(t => t.Timeframe).FirstOrDefault()).Timeframe;

            foreach (OHLC ohlc in this.historicalOHLC)
            {
                if (trackingDay.Timeframe.Date < ohlc.Timeframe.Date)
                {
                    closingTrade = strategy.Close(this.historicalOHLC[this.historicalOHLC.Count - 1]);
                    if (closingTrade != null) { this.portfolio.UpdatePortfolio(closingTrade); }
                }
                this.portfolio.UpdatePortfolio(strategy.Next(ohlc));
                trackingDay = ohlc;
            }
            closingTrade = strategy.Close(this.historicalOHLC[this.historicalOHLC.Count - 1]);
            if (closingTrade != null) { this.portfolio.UpdatePortfolio(closingTrade); }

            return closingTrade;
        }

        public List<Analytics> RunIntradayBackTest(string ticker, DateTime dtFrom, DateTime dtTo, OHLC_TIMESPAN timespan)
        {
            //Run the backtest once for each day
            bool firstRun = true;
            List<Analytics> analyticslog = new List<Analytics>();
            DateTime calcDate = dtFrom.Date;
            while (calcDate <= dtTo.Date)
            {
                AlgoTraderDAL.AlpacaAPI alpca = AlgoTraderDAL.AlpacaAPI.Instance;
                this.historicalOHLC = alpca.Get_TickerData(ticker, calcDate, calcDate.AddHours(23), timespan);
                this.analytics.Reset();
                this.RunIntradayBacktest(firstRun);
                calcDate = calcDate.AddDays(1);
                firstRun = false;
                this.AnalyzeTrades();
                if (this.analytics.numberOfTrades > 0)
                {
                    analyticslog.Add(this.analytics.Copy());
                }
            }

            return analyticslog;


        }
    }
}
