using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL.Indicators
{
    internal class ExponentialMovingAverageIndicator : IIndicator
    {
        private bool isInitialized;
        private readonly int lookback;
        private readonly decimal alpha;
        private decimal previousAverage;

        public decimal Average { get; private set; }
        public decimal Slope { get; private set; }
        public bool SlopeChange { get; private set; }
        public string Name { get { return "ExponentialMovingAverageIndicator"; } }
        public Queue<IndicatorNode> History { get; set; }

        /// <summary>
        /// Method to create an EMA with a lookback history of back
        /// </summary>
        /// <param name="back"></param>
        public ExponentialMovingAverageIndicator(int back)
        {
            lookback = back;
            alpha = (decimal)2.0 / (back + 1);
            this.History = new Queue<IndicatorNode>();
        }

        /// <summary>
        /// Add and queue the next datapoint
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="dt"></param>
        public void AddDataPoint(OHLC ohlc)
        {
            decimal dataPoint = ohlc.GetMedian();

            if (!isInitialized)
            {
                Average = dataPoint;
                Slope = 0;
                previousAverage = Average;
                isInitialized = true;
                this.History.Enqueue(new IndicatorNode() { Value = dataPoint, DTime = ohlc.Timeframe });
                return;
            }

            this.Average = ((dataPoint - previousAverage) * alpha) + previousAverage;
            decimal newSlope = Average - previousAverage;
            this.SlopeChange = ((this.Slope > 0 && newSlope < 0) || (this.Slope < 0 && newSlope > 0));
            this.Slope = newSlope;
            this.History.Enqueue(new IndicatorNode() { Value = this.Average, DTime = ohlc.Timeframe });

            //update previous average
            previousAverage = Average;
        }
        
    }
}
