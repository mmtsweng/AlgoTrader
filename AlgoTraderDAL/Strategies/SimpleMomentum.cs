using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL.Strategies
{
    public class SimpleMomentum : AbstractStrategy
    {
        public Queue<decimal> opens { get; set; }
        public Queue<decimal> times { get; set; }
        public Queue<decimal> closes { get; set; }
        public int QueueSize { get; set; }
        internal TrendlineData openTrend { get; set; }
        internal TrendlineData closeTrend { get; set; }

        public SimpleMomentum()
        {
            this.isIntraday = true;
        }

        /// <summary>
        /// Initialize Strategy
        /// </summary>
        public override void Init()
        {
            base.Init();
            this.opens = new Queue<decimal>();
            this.closes = new Queue<decimal>();
            this.times = new Queue<decimal>();
        }

        /// <summary>
        /// Method to parse the db parameters for this strategy
        /// </summary>
        public override void UpdateParameters()
        {
            base.UpdateParameters();
            int parsedQue = 0;
            if (int.TryParse(this.dbParameters["QueueSize"], out parsedQue))
            {
                this.QueueSize = parsedQue;
            }
            else 
            {
                this.QueueSize = 5;
            }
        }

        /// <summary>
        /// Process the next Stock iteration
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override Trade Next(OHLC ohlc)
        {
            UpdateQueues(ohlc);

            if (opens.Count == QueueSize)
            {
                openTrend = Trendline(opens.Select((t, i) => new Tuple<decimal, decimal>(times.ToList()[i], t)));
                closeTrend = Trendline(closes.Select((t, i) => new Tuple<decimal, decimal>(times.ToList()[i], t)));
            }
            return base.Next(ohlc);
        }

        /// <summary>
        /// Method to Keep Queues capped at QueueSize
        /// </summary>
        /// <param name="ohlc"></param>
        private void UpdateQueues(OHLC ohlc)
        {
            times.Enqueue(GetDateAsDecimal(ohlc.Timeframe));
            if (times.Count > this.QueueSize)
            {
                times.Dequeue();
            }
            opens.Enqueue(ohlc.Open);
            if (opens.Count > this.QueueSize)
            {
                opens.Dequeue();
            }
            closes.Enqueue(ohlc.Close);
            if (closes.Count > this.QueueSize)
            {
                closes.Dequeue();
            }
        }

        /// <summary>
        /// Figure out if we want to buy
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override bool BuySignal(OHLC ohlc)
        {
            if (openTrend != null && closeTrend != null)
            {
                return (openTrend.Slope > 0 && closeTrend.Slope > 0);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Figure out if we want to sell
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override bool SellSignal(OHLC ohlc)
        {
            if (openTrend != null && closeTrend != null)
            {
                return (openTrend.Slope < 0 || closeTrend.Slope < 0);
            }
            else
            { 
                return false;
            }
        }

        /// <summary>
        /// Make the actual trade
        /// </summary>
        /// <param name="ohlc"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public override Trade MakeTrade(OHLC ohlc, TradeSide side)
        {
            Trade trade = base.MakeTrade(ohlc, side);

            trade.actualPrice = (ohlc.High + ohlc.Low) / 2;
            trade.submittedPrice = (ohlc.Open + ohlc.Close) / 2;

            return trade;
        }

        /// <summary>
        /// Calculate the Trendline for this set of data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal TrendlineData Trendline(IEnumerable<Tuple<Decimal, Decimal>> data)
        {
            var cachedData = data.ToList();
            var n = cachedData.Count;
            var sumX = cachedData.Sum(x => x.Item1);
            var sumX2 = cachedData.Sum(x => x.Item1 * x.Item1);
            var sumY = cachedData.Sum(x => x.Item2);
            var sumXY = cachedData.Sum(x => x.Item1 * x.Item2);
            TrendlineData results = new TrendlineData();

            //b = (sum(x*y) - sum(x)sum(y)/n)
            //      / (sum(x^2) - sum(x)^2/n)
            results.Slope = (sumXY - ((sumX * sumY) / n))
                        / (sumX2 - (sumX * sumX / n));

            //a = sum(y)/n - b(sum(x)/n)
            results.Intercept = (sumY / n) - (results.Slope * (sumX / n));

            results.Start = GetYValue(cachedData.Min(a => a.Item1), results);
            results.End = GetYValue(cachedData.Max(a => a.Item1), results);
            return results;
        }

        /// <summary>
        /// Get the YValue for the specified X, based on the trendline
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="trend"></param>
        /// <returns></returns>
        internal decimal GetYValue(decimal xValue, TrendlineData trend)
        {
            return trend.Intercept + trend.Slope * xValue;
        }

        /// <summary>
        /// Helper method to convert a date to a decimal to keep trendline types consistent
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal decimal GetDateAsDecimal(DateTime date)
        {
            TimeSpan t = date.Subtract(new DateTime(1990, 1, 1, 0, 0, 0, 0));
            decimal timespan = (decimal)t.TotalSeconds;
            return timespan;
        }
    }

    
    /// <summary>
    /// Helper class to store trendline data
    /// </summary>
    internal class TrendlineData
    {
        internal decimal Slope { get; set; }
        internal decimal Intercept { get; set; }
        internal decimal Start { get; set; }
        internal decimal End { get; set; }
    }
}
