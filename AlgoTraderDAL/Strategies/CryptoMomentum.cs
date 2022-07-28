using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL.Strategies
{
    public class CryptoMomentum : AbstractStrategy
    {
        public Queue<decimal> opens { get; set; }
        public Queue<decimal> times { get; set; }
        public Queue<decimal> closes { get; set; }
        public Queue<decimal> avgohlc { get; set; }
        public Queue<decimal> ohlctimes { get; set; }
        public int BuyQueueSize { get; set; }
        public int SellQueueSize { get; set; }
        public int TrendQueueSize { get; set; }
        internal TrendlineData openTrend { get; set; }
        internal TrendlineData closeTrend { get; set; }
        internal TrendlineData ohlcTrend { get; set; }

        public CryptoMomentum()
        {
            base.isIntraday = true;
        }

        public CryptoMomentum(bool isLiveTrading)
        {
            base.isLiveTrading = isLiveTrading;
            base.isIntraday = true;
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
            this.avgohlc = new Queue<decimal>();
            this.ohlctimes = new Queue<decimal>();
        }

        /// <summary>
        /// Method to parse the db parameters for this strategy
        /// </summary>
        public override void UpdateParameters()
        {
            base.UpdateParameters();
            int parsedVal = 0;
            if (int.TryParse(this.dbParameters["BuyQueueSize"], out parsedVal))
            {
                this.BuyQueueSize = parsedVal;
            }
            else 
            {
                this.BuyQueueSize = 5;
            }

            if (int.TryParse(this.dbParameters["SellQueueSize"], out parsedVal))
            {
                this.SellQueueSize = parsedVal;
            }
            else
            {
                this.SellQueueSize = 5;
            }

            if (int.TryParse(this.dbParameters["TrendQueueSize"], out parsedVal))
            {
                this.TrendQueueSize = parsedVal;
            }
            else
            {
                this.TrendQueueSize = 12;
            }

            if (int.TryParse(this.dbParameters["MaxOpenPositions"], out parsedVal))
            {
                this.maxOpenPositions = parsedVal;
            }
            else
            {
                this.maxOpenPositions = 1;
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

            if (opens.Count == BuyQueueSize)
            {
                openTrend = Trendline(opens.Select((t, i) => new Tuple<decimal, decimal>(times.ToList()[i], t)));
            }
            if (closes.Count == this.SellQueueSize)
            {
                closeTrend = Trendline(closes.Select((t, i) => new Tuple<decimal, decimal>(times.ToList()[i], t)));
            }
            if (avgohlc.Count > 1)
            {
                ohlcTrend = Trendline(avgohlc.Select((t, i) => new Tuple<decimal, decimal>(ohlctimes.ToList()[i], t)));
            }
            return base.Next(ohlc);
        }

        /// <summary>
        /// Method to Keep Queues capped at QueueSize
        /// </summary>
        /// <param name="ohlc"></param>
        private void UpdateQueues(OHLC ohlc)
        {
            decimal dt = GetDateAsDecimal(ohlc.Timeframe);

            times.Enqueue(dt);
            if (times.Count > this.BuyQueueSize)
            {
                times.Dequeue();
            }
            opens.Enqueue(ohlc.Open);
            if (opens.Count > this.BuyQueueSize)
            {
                opens.Dequeue();
            }
            closes.Enqueue(ohlc.Close);
            if (closes.Count > this.SellQueueSize)
            {
                closes.Dequeue();
            }

            List<decimal> medianCalc = new List<decimal>();
            medianCalc.Add(ohlc.Open);
            medianCalc.Add(ohlc.Close);
            medianCalc.Add(ohlc.High);
            medianCalc.Add(ohlc.Low);
            avgohlc.Enqueue(GetMedian(medianCalc.ToArray()));
            ohlctimes.Enqueue(dt);
            if (avgohlc.Count > this.TrendQueueSize)
            {
                avgohlc.Dequeue();
                ohlctimes.Dequeue();
            }
        }

        /// <summary>
        /// Figure out if we want to buy
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override bool BuySignal()
        {
            if (openTrend != null && closeTrend != null && ohlcTrend != null)
            {
                return (ohlcTrend.Slope > 0 && openTrend.Slope > 0 && closeTrend.Slope > 0);
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
        public override bool SellSignal()
        {
            if (openTrend != null && closeTrend != null & ohlcTrend != null)
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

        /// <summary>
        /// Method to return the median value of an array of numbers
        /// </summary>
        /// <param name="sourceNumbers"></param>
        /// <returns></returns>
        internal decimal GetMedian(decimal[] sourceNumbers)
        {
            //make sure the list is sorted, but use a new array
            decimal[] sortedPNumbers = (decimal[])sourceNumbers.Clone();
            Array.Sort(sortedPNumbers);

            //get the median
            int size = sortedPNumbers.Length;
            int mid = size / 2;
            decimal median = (size % 2 != 0) ? (decimal)sortedPNumbers[mid] : ((decimal)sortedPNumbers[mid] + (decimal)sortedPNumbers[mid - 1]) / 2;
            return median;
        }
    }
}
