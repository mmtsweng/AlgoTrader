using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using AlgoTraderDAL.Live;
using AlgoTraderDAL.Types;
using ScottPlot;

namespace AlgoTrader
{
    public partial class frmRealtimeTrades : Form
    {
        private List<AlgoTraderDAL.Types.OHLC> ohlc = new List<AlgoTraderDAL.Types.OHLC>();

        public frmRealtimeTrades()
        {
            InitializeComponent();
            this.txtSymbol.Text = "SPY";
        }

        /// <summary>
        /// Start running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            RealtimeAlpacaAPI.Instance.Start(this.txtSymbol.Text);
            this.btnTest.Enabled = false;
        }

        /// <summary>
        /// Connect to Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRealtimeTrades_Load(object sender, EventArgs e)
        {
            RealtimeAlpacaAPI.Instance.OHLCReceived += OHLCDataReceived;
            RealtimeAlpacaAPI.Instance.OHLCRefresh += OHLCRefreshDataReceived;
        }

        /// <summary>
        /// Process next realtime OHLC data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="bar"></param>
        public void OHLCDataReceived(object sender, AlgoTraderDAL.Types.OHLC bar)
        {
            this.ohlc.Add(bar);
            this.pltChart.Invoke((MethodInvoker)delegate { UpdateChart(); });
        }

        /// <summary>
        /// Get all the bars op to this point
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="bars"></param>
        public void OHLCRefreshDataReceived (object sender, List<AlgoTraderDAL.Types.OHLC> bars)
        {
            this.ohlc = bars;
            this.pltChart.Invoke((MethodInvoker)delegate { UpdateChart(); });
        }

        /// <summary>
        /// Update the chart with current data
        /// </summary>
        private void UpdateChart()
        {
            ScottPlot.OHLC[] prices = ohlc.Select(p => new ScottPlot.OHLC(0, 0, 0, 0, 0)
            {
                Open = decimal.ToDouble(p.Open),
                High = decimal.ToDouble(p.High),
                Low = decimal.ToDouble(p.Low),
                Close = decimal.ToDouble(p.Close),
                Volume = decimal.ToDouble(p.Volume),
                TimeSpan = new TimeSpan(0,1,0),
                DateTime = p.Timeframe
            }).ToArray();

            var plt = new ScottPlot.Plot(800, 600);
            this.pltChart.Plot.Clear();
            var cplot = this.pltChart.Plot.AddCandlesticks(prices);
            this.pltChart.Plot.XAxis.DateTimeFormat(true);
            this.pltChart.Refresh();
        }

        /// <summary>
        /// Stop events when we close this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRealtimeTrades_FormClosing(object sender, FormClosingEventArgs e)
        {
            RealtimeAlpacaAPI.Instance.StopListening();
        }
    }
}
