using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;
using System.Drawing;
using ScottPlot.Plottable;

namespace AlgoTraderDAL.Strategies
{
    public class CryptoBollingerStrategy : AbstractStrategy
    {
        public int LookbackSize { get; set; }
        public int StandardDeviations { get; set; }
        public BollingerBandsResult CurrentBollinger { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CryptoBollingerStrategy()
        {
            base.isIntraday = true;
        }

        ///Overloaded Constructor <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="isLiveTrading"></param>
        public CryptoBollingerStrategy(bool isLiveTrading)
        {
            base.isLiveTrading = isLiveTrading;
            base.isIntraday = true;
        }

        /// <summary>
        /// Method to parse the db parameters for this strategy
        /// </summary>
        public override void UpdateParameters()
        {
            base.UpdateParameters();
            this.LookbackSize = GetStrategyOption("LookbackSize", 20);
            this.StandardDeviations = GetStrategyOption("StandardDeviations", 2);
        }

        /// <summary>
        /// Process the next Stock iteration
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override Trade Next(OHLC ohlc, bool updateQueue)
        {
            UpdateOHLCQueue(ohlc);
            IEnumerable<BollingerBandsResult> bolinger = this.OHLCs.GetBollingerBands(this.LookbackSize, this.StandardDeviations);
            if (bolinger.Count() > 0) { this.CurrentBollinger = bolinger.Last(); }

            Trade trade = base.Next(ohlc, false);

            if (this.OHLCs.Count > this.LookbackSize)
            {
                this.Indicators.Clear();

                double[] times = bolinger.Where(x => x.PercentB != null).Select(x => x.Date.ToOADate()).ToArray();
                double[] bupper = bolinger.Where(x => x.PercentB != null).Select(x => x.UpperBand.GetValueOrDefault()).ToArray();
                double[] blower = bolinger.Where(x => x.PercentB != null).Select(x => x.LowerBand.GetValueOrDefault()).ToArray();
                double[] bsma = bolinger.Where(x => x.PercentB != null).Select(x => x.Sma.GetValueOrDefault()).ToArray();

                ScatterPlot upperPlot = new ScatterPlot(times, bupper);
                upperPlot.Color = Color.MediumSeaGreen;
                upperPlot.LineStyle = ScottPlot.LineStyle.Solid;
                upperPlot.MarkerShape = ScottPlot.MarkerShape.none;

                ScatterPlot lowerPlot = new ScatterPlot(times, blower);
                lowerPlot.Color = Color.MediumOrchid;
                lowerPlot.LineStyle = ScottPlot.LineStyle.Solid;
                lowerPlot.MarkerShape = ScottPlot.MarkerShape.none;

                ScatterPlot smaPlot = new ScatterPlot(times, bsma);
                smaPlot.Color = Color.SkyBlue;
                smaPlot.LineStyle = ScottPlot.LineStyle.Dash;
                smaPlot.MarkerShape = ScottPlot.MarkerShape.none;

                this.Indicators.Add(upperPlot);
                this.Indicators.Add(lowerPlot);
                this.Indicators.Add(smaPlot);

            }


            return trade;
        }

        /// <summary>
        /// Figure out if we want to buy
        /// </summary>
        /// <param name="ohlc"></param>
        /// <returns></returns>
        public override bool BuySignal()
        {
            if (this.CurrentBollinger == null || this.CurrentBollinger.PercentB == null) { return false; }
            OHLC ohlc = this.OHLCs.Last();

            SlopeResult slope = this.OHLCs.GetEma(4).GetSlope(4).Last();
            if (slope.Slope != null && slope.Slope.GetValueOrDefault() < -0.05) 
            { return false; }

            if ((double)ohlc.Low < this.CurrentBollinger.LowerBand.GetValueOrDefault())
            { return true; }
            return false;
        }

        /// <summary>
        /// Figure out if we want to sell
        /// </summary>
        /// <returns></returns>
        public override bool SellSignal()
        {
            if (this.CurrentBollinger == null || this.CurrentBollinger.PercentB == null) { return false; }
            OHLC ohlc = this.OHLCs.Last();

            SlopeResult slope = this.OHLCs.GetEma(4).GetSlope(4).Last();
            if (slope.Slope != null && slope.Slope.GetValueOrDefault() < -0.05) 
            { return true; }

            if ((double)ohlc.High > this.CurrentBollinger.UpperBand.GetValueOrDefault())
            { return true; }
            return false;   
        }

    }
}
