using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using AlgoTraderDAL.Live;
using AlgoTraderDAL.Indicators;
using ScottPlot;

namespace AlgoTrader
{
    public partial class frmRealtimeTrades : Form
    {
        private List<AlgoTraderDAL.Types.OHLC> ohlc = new List<AlgoTraderDAL.Types.OHLC>();
        private List<AlgoTraderDAL.Types.OHLC> ohlcHistory = new List<AlgoTraderDAL.Types.OHLC>();
        private RealtimeStrategyExecutor StrategyExecutor = new RealtimeStrategyExecutor();

        public frmRealtimeTrades()
        {
            InitializeComponent();
            this.txtSymbol.Text = "SOLUSD";
            this.chkCrypto.Checked = true;
        }

        /// <summary>
        /// Start running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            RealtimeAlpacaAPI.Instance.Start(this.txtSymbol.Text, this.chkCrypto.Checked);
            RealtimeAlpacaAPI.Instance.RequestTodayHistoricalTickerData();
            this.StrategyExecutor.Start(this.txtSymbol.Text, this.chkCrypto.Checked);
            UpdateAccountBalance();

            this.btnTest.Enabled = false;
        }

        /// <summary>
        /// Method to reflect the current account balance
        /// </summary>
        private void UpdateAccountBalance()
        {
            decimal balanceDifference = (this.StrategyExecutor.portfolio.availableCash - this.StrategyExecutor.portfolio.startingBalance);
            this.txtAccountBalance.Invoke(new MethodInvoker(delegate {
                this.txtAccountBalance.Text = Math.Round(this.StrategyExecutor.portfolio.availableCash, 2).ToString()
                + " | (" + Math.Round((balanceDifference), 2).ToString()
                + ")";
            }));
            
            if (balanceDifference > 0)
            {
                this.txtAccountBalance.Invoke(new MethodInvoker(delegate { 
                    this.txtAccountBalance.BackColor = Color.LightGreen; 
                }));
            }
            else if(balanceDifference < 0)
            {
                this.txtAccountBalance.Invoke(new MethodInvoker(delegate
                {
                    this.txtAccountBalance.BackColor = Color.LightPink;
                }));
            }
            else
            {
                this.txtAccountBalance.Invoke(new MethodInvoker(delegate
                {
                    this.txtAccountBalance.BackColor = Color.White;
                }));
            }
        }

        /// <summary>
        /// Connect to Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRealtimeTrades_Load(object sender, EventArgs e)
        {
            RealtimeAlpacaAPI.Instance.Init();
            RealtimeAlpacaAPI.Instance.OHLCReceived += OHLCDataReceived;
            RealtimeAlpacaAPI.Instance.OHLCRefresh += OHLCRefreshDataReceived;
            this.StrategyExecutor.TradeOccurred += TradeSignal;
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
            this.ohlcHistory = bars;
            this.pltChart.Invoke((MethodInvoker)delegate { UpdateChart(); });
        }

        public void TradeSignal(object sender, AlgoTraderDAL.Trade trade)
        {
            this.StrategyExecutor.portfolio.trades.Add(trade);
            UpdateAccountBalance();
            this.pltChart.Invoke((MethodInvoker)delegate { UpdateChart(); });
        }

        /// <summary>
        /// Update the chart with current data
        /// </summary>
        private void UpdateChart(bool clearFirst = false)
        {
            ScottPlot.OHLC[] historicalPrices = ohlcHistory.Select(p => new ScottPlot.OHLC(0, 0, 0, 0, 0)
            {
                Open = decimal.ToDouble(p.Open),
                High = decimal.ToDouble(p.High),
                Low = decimal.ToDouble(p.Low),
                Close = decimal.ToDouble(p.Close),
                Volume = decimal.ToDouble(p.Volume),
                TimeSpan = new TimeSpan(0, 1, 0),
                DateTime = p.Timeframe
            }).ToArray();
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

            double[] buytimes = this.StrategyExecutor.portfolio.trades
                .Where(x => x.side == AlgoTraderDAL.TradeSide.BUY)
                .Select(x => x.transactionDateTime.ToOADate()).ToArray();
            double[] buys = this.StrategyExecutor.portfolio.trades
                .Where(x => x.side == AlgoTraderDAL.TradeSide.BUY)
                .Select(x => (double)x.actualPrice).ToArray();
            double[] saletimes = this.StrategyExecutor.portfolio.trades
                .Where(x => x.side == AlgoTraderDAL.TradeSide.SELL)
                .Select(x => x.transactionDateTime.ToOADate()).ToArray();
            double[] sales = this.StrategyExecutor.portfolio.trades
                .Where(x => x.side == AlgoTraderDAL.TradeSide.SELL)
                .Select(x => (double)x.actualPrice).ToArray();

            var plt = new ScottPlot.Plot(800, 600);
            this.pltChart.Plot.Clear();
            var hplot = this.pltChart.Plot.AddCandlesticks(historicalPrices);
            hplot.WickColor = Color.LightGray ;
            hplot.ColorDown = Color.LightGray;
            hplot.ColorUp = Color.LightGray;

            //Add focus chart
            if (this.StrategyExecutor.strategy.focusRange > 0)
            {
                DateTime dtNow = DateTime.Now;
                DateTime dtStart = dtNow.AddMinutes(-(this.StrategyExecutor.strategy.focusRange));
                var vSpan = this.pltChart.Plot.AddHorizontalSpan(dtNow.ToOADate(), dtStart.ToOADate());
                vSpan.DragEnabled = false;
                vSpan.BorderColor = Color.LightSeaGreen;
                vSpan.BorderLineStyle = LineStyle.Dot;
                vSpan.BorderLineWidth = 2;
                vSpan.Color = Color.White;
            }

            foreach(IIndicator ind in this.StrategyExecutor.strategy.Indicators)
            {
                double[] times = ind.History.Select(x => x.DTime.ToOADate()).ToArray();
                double[] val = ind.History.Select(x => decimal.ToDouble((decimal)x.Value)).ToArray();
                if (times.Length > 0 && val.Length == times.Length)
                {
                    this.pltChart.Plot.AddScatter(times, val, Color.Orange, lineWidth: 3, markerSize: 0, lineStyle: LineStyle.Solid);
                }
            }


            if (buys.Length > 0)
            {
                this.pltChart.Plot.AddScatterPoints(buytimes, buys, Color.DarkGreen, markerShape: ScottPlot.MarkerShape.filledTriangleUp, markerSize: 10);
            }
            if (sales.Length > 0)
            {
                this.pltChart.Plot.AddScatterPoints(saletimes, sales, Color.DarkRed, markerShape: ScottPlot.MarkerShape.filledTriangleDown, markerSize: 10);
            }
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
