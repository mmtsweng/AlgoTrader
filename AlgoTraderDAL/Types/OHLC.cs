using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpaca.Markets;
using Skender.Stock.Indicators;

namespace AlgoTraderDAL.Types
{
    public enum OHLC_TIMESPAN 
    {
        DAY,
        HOUR,
        MINUTE
    }

    public class OHLC : Skender.Stock.Indicators.IQuote
    {
        DateTime Skender.Stock.Indicators.IQuote.Date => Timeframe;
        public string Symbol { get; set; }
        public DateTime Timeframe { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public OHLC_TIMESPAN ticks { get; set; }

        public void parseIBar(IBar bardata)
        {
            this.Open = bardata.Open;
            this.Close = bardata.Close;
            this.High = bardata.High;
            this.Low = bardata.Low;
            this.Symbol = bardata.Symbol;
            this.Timeframe = bardata.TimeUtc;
            this.Volume = bardata.Volume;
        }

        /// <summary>
        /// Helper method to convert ticks to a timespan
        ///     Used for charting
        /// </summary>
        /// <returns></returns>
        public TimeSpan TimespanFromOHLCTimespan()
        {
            TimeSpan tspan = new TimeSpan(1, 0, 0, 0);
            switch (ticks)
            {
                case OHLC_TIMESPAN.HOUR:
                    tspan = new TimeSpan(1, 0, 0);
                    break;
                case OHLC_TIMESPAN.MINUTE:
                    tspan = new TimeSpan(0, 1, 0);
                    break;
                default:
                    break;
            }

            return tspan;
        }

        /// <summary>
        /// Method to return the median value of this OHLC instance
        /// </summary>
        /// <returns></returns>
        public decimal GetMedian()
        {
            List<decimal> medianCalc = new List<decimal>();
            medianCalc.Add(this.Open);
            medianCalc.Add(this.Close);
            medianCalc.Add(this.High);
            medianCalc.Add(this.Low);
            return CalculateMedian(medianCalc.ToArray());
        }

        /// <summary>
        /// Method to return the median value of an array of numbers
        /// </summary>
        /// <param name="sourceNumbers"></param>
        /// <returns></returns>
        private decimal CalculateMedian(decimal[] sourceNumbers)
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
