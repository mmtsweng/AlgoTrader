using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AlgoTraderDAL.BackTesting;
using AlgoTraderDAL.Types;
using AlgoTraderDAL.Strategies;
using System.Drawing;

namespace AlgoTrader
{
    public partial class frmAlpacaData : Form
    {
        private IStrategy strategy = new CryptoIntradayStrategy();
        public DateTime intradateTime { get; set; }
        public frmAnalytics analyticsForm { get; set; }
        public BackTester backtest { get; set; }
        public Analytics[] analyticsLog { get; set; }
        public int analyticsLogIdx { get; set; }

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
            this.txtTicker.Text = ":SOLUSD";
            this.intradateTime = DateTime.Now;
            this.cboPeriod.DataSource = Enum.GetValues(typeof(OHLC_TIMESPAN));
            this.backtest = new BackTester(strategy, new List<OHLC>());
            this.analyticsLog = Array.Empty<Analytics>();
            this.analyticsLogIdx = 0;

            DateTime now = DateTime.Today.Date;

            if (this.strategy.isIntraday)
            {
                //Intraday Trading, shorten timeframe
                this.dtFrom.Value = now.AddDays(-7);//7 days ago
                this.dtTo.Value = now.AddDays(-1); //yesterday
                this.cboPeriod.SelectedIndex = 2;
            }
            else
            {
                //Multiday trading
                this.dtFrom.Value = now.AddDays(-90);//3 months ago
                this.dtTo.Value = now.AddDays(-1); //yesterday
            }
        }

        /// <summary>
        /// Load Data From Data Access Layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (this.strategy.isIntraday)
            {
                this.intradateTime = this.dtFrom.Value;
                this.btnPrevious.Enabled = false;
                LoadDataFromAPI(this.txtTicker.Text, this.dtFrom.Value.Date, this.dtFrom.Value.Date.AddHours(23), (OHLC_TIMESPAN)cboPeriod.SelectedValue);
            }
            else
            {
                LoadDataFromAPI(this.txtTicker.Text, this.dtFrom.Value.Date, this.dtTo.Value.Date, (OHLC_TIMESPAN)cboPeriod.SelectedValue);
            }
        }

        /// <summary>
        /// Method to call the Alpaca API for market data
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="period"></param>
        private void LoadDataFromAPI(string ticker, DateTime from, DateTime to, OHLC_TIMESPAN period)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                AlgoTraderDAL.AlpacaAPI alpca = AlgoTraderDAL.AlpacaAPI.Instance;

                this.backtest.historicalOHLC = alpca.Get_TickerData(ticker, from, to, period);
                this.dgResults.DataSource = this.backtest.historicalOHLC;
                this.dgResults.Update();
                this.btnChart.Enabled = true;
                this.cmdBacktest.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

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
            chart.LoadOHLC(this.backtest.historicalOHLC);
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
            this.btnNext.Visible = true;
            this.lblDays.Visible = true;
            this.btnPrevious.Visible = true;

            if (this.backtest.strategy.isIntraday)
            {
                this.Cursor = Cursors.WaitCursor;
                this.btnChart.Enabled = false;
                this.analyticsLog = this.backtest.RunIntradayBackTest(this.txtTicker.Text, this.dtFrom.Value, this.dtTo.Value, (OHLC_TIMESPAN)cboPeriod.SelectedValue).ToArray();
                if (this.analyticsLog.Length > 0)
                {
                    DisplayIntradayAnalytics(analyticsLog);
                    DisplayAnalyticsData(this.backtest, analyticsLog[analyticsLogIdx]);
                }
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.backtest.RunBackTest();
                DisplayAnalyticsData(this.backtest);
            }
            this.Cursor = Cursors.Default;
        }

        private void DisplayIntradayAnalytics(Analytics[] results)
        {
            DataGridViewCellStyle winstyle = new DataGridViewCellStyle();
            winstyle.BackColor = Color.LightGreen;
            DataGridViewCellStyle lossStyle = new DataGridViewCellStyle();
            lossStyle.BackColor = Color.LightPink;

            this.dgResults.DataSource = results;
            this.dgResults.Columns["startDateTime"].DisplayIndex = 0;
            this.dgResults.Columns["endDateTime"].DisplayIndex = 1;
            this.dgResults.Columns["netProfit"].DisplayIndex = 2;
            this.dgResults.Columns["netProfitPercent"].DisplayIndex = 3;
            this.dgResults.Columns["numberOfTrades"].DisplayIndex = 4;

            foreach (DataGridViewColumn column in this.dgResults.Columns)
            {
                if (column.Name.Contains("netProfit"))
                {
                    foreach (DataGridViewRow row in this.dgResults.Rows)
                    {
                        DataGridViewCell cell = this.dgResults.Rows[row.Index].Cells[column.Index];
                        if ((decimal)cell.Value > 0)
                        {
                            this.dgResults.Rows[row.Index].Cells[column.Index].Style = winstyle;
                        }
                        else
                        {
                            this.dgResults.Rows[row.Index].Cells[column.Index].Style = lossStyle;
                        }
                    }
                }
            }

            this.dgResults.Refresh();
        }

        /// <summary>
        /// Method for Displaying a Single Analytics object's data
        /// </summary>
        /// <param name="backtest"></param>
        private void DisplayAnalyticsData(BackTester backtest, Analytics analytics = null)
        {
            if (analytics == null) { analytics = backtest.analytics; }

            CreateAnalyticsFormIfNeeded();
            this.analyticsForm.Show();
            this.analyticsForm.ProcessBacktest(ref backtest, analytics);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            Analytics analytics = analyticsLog[analyticsLogIdx];
            CreateAnalyticsFormIfNeeded();
            analyticsLogIdx--;
            this.analyticsForm.ProcessAnalytics(analyticsLog[analyticsLogIdx]);
            this.backtest.historicalOHLC = AlgoTraderDAL.AlpacaAPI.Instance.Get_TickerData(analytics.symbol, analytics.startDateTime, analytics.endDateTime, (OHLC_TIMESPAN)cboPeriod.SelectedValue);
            DisplayAnalyticsData(this.backtest, analyticsLog[analyticsLogIdx]);
            if (analyticsLogIdx == 0)
            {
                this.btnPrevious.Enabled = false;
            }
            this.btnNext.Enabled = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Analytics analytics = analyticsLog[analyticsLogIdx];
            CreateAnalyticsFormIfNeeded();
            analyticsLogIdx++;
            this.analyticsForm.ProcessAnalytics(analytics);
            this.backtest.historicalOHLC = AlgoTraderDAL.AlpacaAPI.Instance.Get_TickerData(analytics.symbol, analytics.startDateTime, analytics.endDateTime, (OHLC_TIMESPAN)cboPeriod.SelectedValue);
            DisplayAnalyticsData(this.backtest, analytics);
            if (analyticsLogIdx == analyticsLog.Length)
            {
                this.btnNext.Enabled = false;
            }
            this.btnPrevious.Enabled = true;
        }

        /// <summary>
        /// Helper Method to Create an Analytics form if the old one was closed
        /// </summary>
        private void CreateAnalyticsFormIfNeeded()
        {
            if (this.analyticsForm == null || this.analyticsForm.Disposing)
            {
                this.analyticsForm = new frmAnalytics();
                this.analyticsForm.MdiParent = this.MdiParent;
            }
        }
    }
}
