﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;
using AlgoTraderDAL.Indicators;

namespace AlgoTraderDAL.Strategies
{
    public class CryptoMomentum : AbstractStrategy
    {
        public Queue<decimal> opens { get; set; }
        public Queue<decimal> times { get; set; }
        public Queue<decimal> closes { get; set; }
        public Queue<decimal> highs { get; set; }
        public Queue<decimal> lows { get; set; }
        public Queue<decimal> avgohlc { get; set; }
        public Queue<decimal> ohlctimes { get; set; }
        public int BuyQueueSize { get; set; }
        public int SellQueueSize { get; set; }
        public int TrendQueueSize { get; set; }
        internal TrendlineData openTrend { get; set; }
        internal TrendlineData closeTrend { get; set; }
        internal TrendlineData highTrend { get; set; }
        internal TrendlineData lowTrend { get; set; }
        internal TrendlineData ohlcTrend { get; set; }
        private decimal stoplossSalePrice { get; set; }
        public bool stoplossSale { get; set; }
        private OHLC recentOHLC { get; set; }
        public decimal slopeFactor { get; set; }

        public CryptoMomentum()
        {
            base.isIntraday = true;
            this.stoplossSalePrice = decimal.MaxValue;
            this.slopeFactor = 0M;
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
            this.highs = new Queue<decimal>();  
            this.lows = new Queue<decimal>();
            this.recentOHLC = new OHLC();
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

            if (int.TryParse(this.dbParameters["EMAPeriod"], out parsedVal))
            {
                this.Indicators.Add(new ExponentialMovingAverageIndicator(parsedVal));
            }
            else
            {
                this.Indicators.Add(new ExponentialMovingAverageIndicator(5));
            }

            decimal parsedDec = 0;
            if (decimal.TryParse(this.dbParameters["EMASlope"], out parsedDec))
            {
                this.slopeFactor = parsedDec;
            }
            else
            {
                this.slopeFactor = 0;
            }

            if (int.TryParse(this.dbParameters["TrendQueueSize"], out parsedVal))
            {
                this.TrendQueueSize = parsedVal;
                this.focusRange = parsedVal;
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
            this.recentOHLC = ohlc;

            UpdateQueues(ohlc);

            if (opens.Count == BuyQueueSize)
            {
                openTrend = Trendline(opens.Select((t, i) => new Tuple<decimal, decimal>(times.ToList()[i], t)));
            }
            if (closes.Count == this.SellQueueSize)
            {
                closeTrend = Trendline(closes.Select((t, i) => new Tuple<decimal, decimal>(times.ToList()[i], t)));
            }
            if (lows.Count == BuyQueueSize)
            {
                highTrend = Trendline(lows.Select((t, i) => new Tuple<decimal, decimal>(times.ToList()[i], t)));
            }
            if (highs.Count == BuyQueueSize)
            {
                lowTrend = Trendline(highs.Select((t, i) => new Tuple<decimal, decimal>(times.ToList()[i], t)));
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
            if (this.BuyQueueSize > this.SellQueueSize)
            {
                if (times.Count > this.BuyQueueSize)
                {
                    times.Dequeue();
                }
            }
            else
            {
                if (times.Count > this.SellQueueSize)
                {
                    times.Dequeue();
                }
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
            highs.Enqueue(ohlc.High);
            if (highs.Count > this.BuyQueueSize)
            {
                highs.Dequeue();
            }
            lows.Enqueue(ohlc.Low);
            if (lows.Count> this.BuyQueueSize)
            {
                lows.Dequeue();
            }

            this.GetIndicatorByName("ExponentialMovingAverageIndicator").AddDataPoint(ohlc);
            decimal median = ohlc.GetMedian();
            avgohlc.Enqueue(median);
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
                //Protection Checks
                if (avgohlc.Count < this.TrendQueueSize) { return false; }
                ExponentialMovingAverageIndicator ema = (ExponentialMovingAverageIndicator)this.GetIndicatorByName("ExponentialMovingAverageIndicator");
                if (ema == null) { return false; }

                //We only check every 10 minutes, on the X3 and X8 times
                int minuteVal = this.recentOHLC.Timeframe.Minute % 10;
                if (minuteVal != 3) { return false; }

                if (ema.Slope > this.slopeFactor) // && ema.SlopeChange &&
                {
                    //Set Stoploss
                    this.stoplossSalePrice = this.recentOHLC.Low;
                    return true;
                }
                else
                {
                    return false;
                }
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

            //Get the indicator
            ExponentialMovingAverageIndicator ema = (ExponentialMovingAverageIndicator)this.GetIndicatorByName("ExponentialMovingAverageIndicator");
            if (ema == null) { return false; }

            if (ema.Slope < 0 || ohlcTrend.Slope < (-this.slopeFactor)) // && ema.SlopeChange 
            {
                //Set Stoploss
                this.stoplossSalePrice = decimal.MaxValue;
                return true;
            }
            else
            {
                return false;
            }
            /* 
            //We only check every 5 minutes, on the X3 and X8 times
            int minuteVal = this.recentOHLC.Timeframe.Minute % 10;

            if (minuteVal == 8 || minuteVal == 3)
            {
                /Check for Stoploss on 3 and 8
                if (this.stoplossSalePrice < decimal.MaxValue) //Alpaca doesn't support stoploss so simulate one
                {
                    if (this.recentOHLC.Low < this.stoplossSalePrice) //  || this.recentOHLC.Open < this.stoplossSalePrice || this.recentOHLC.Close < this.stoplossSalePrice)
                    {
                        this.stoplossSalePrice = decimal.MaxValue;
                        return true;
                    }
                }
            }

            if (minuteVal == 3)
            { 
            
             }
            return false;
             */




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

            try
            {
                results.Slope = (sumXY - ((sumX * sumY) / n))
                    / (sumX2 - (sumX * sumX / n));
                results.Intercept = (sumY / n) - (results.Slope * (sumX / n));
            }
            catch (Exception)
            {
                results.Slope = 0;
                results.Intercept = 0;
            }

            //a = sum(y)/n - b(sum(x)/n)


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
}
