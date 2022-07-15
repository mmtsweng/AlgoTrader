using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AlgoTraderDAL.BackTesting;
using AlgoTraderDAL.Types;
using AlgoTraderDAL.Strategies;

namespace AlgoTrader
{
#pragma warning disable IDE1006 // Naming Styles
    public partial class frmAlpacaData : Form
#pragma warning restore IDE1006 // Naming Styles
    {
        private List<AlgoTraderDAL.Types.OHLC> ohlcData = new();
        private IStrategy strategy = new SimpleMomentum();

        public frmAlpacaData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set Default Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAlpacaData_Load(object sender, EventArgs e)
        {
            this.txtTicker.Text = "SPY";
            if (this.strategy.isIntraday)
            {
                //Intraday Trading, shorten timeframe
                this.dtFrom.Value = DateTime.Now.AddDays(-3);//3 days ago
                this.dtTo.Value = DateTime.Now.AddDays(-1); //yesterday
            }
            else
            {
                //Multiday trading
                this.dtFrom.Value = DateTime.Now.AddDays(-90);//3 months ago
                this.dtTo.Value = DateTime.Now.AddDays(-1); //yesterday
            }
            this.cboPeriod.DataSource = Enum.GetValues(typeof(OHLC_TIMESPAN)); 
        }

        /// <summary>
        /// Load Data From Data Access Layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            AlgoTraderDAL.AlpacaAPI alpca = AlgoTraderDAL.AlpacaAPI.Instance;

            ohlcData = alpca.Get_TickerData(this.txtTicker.Text, this.dtFrom.Value, this.dtTo.Value, (OHLC_TIMESPAN)cboPeriod.SelectedValue);
            dgResults.DataSource = ohlcData;
            dgResults.Update();
            this.btnChart.Enabled = true;
            this.cmdBacktest.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Show the Chart for this set of tickers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChart_Click(object sender, EventArgs e)
        {
            frmChart chart = new();
            chart.MdiParent = this.MdiParent;
            chart.Text = this.txtTicker.Text;
            chart.LoadOHLC(ohlcData);
            chart.Show();
        }

        /// <summary>
        /// Run a Backtest on the existing Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdBacktest_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            BackTester backtest = new BackTester(strategy, ohlcData);

            frmAnalytics analytics = new frmAnalytics();
            analytics.MdiParent = this.MdiParent;
            analytics.Show();

            backtest.RunBackTest();

            analytics.ProcessAnalytics(backtest.analytics, ref backtest);
            this.Cursor = Cursors.Default;
        }
    }
}
