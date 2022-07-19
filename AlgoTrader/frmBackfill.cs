using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlgoTraderDAL.Types;
using AlgoTraderDAL;

namespace AlgoTrader
{
    public partial class frmBackfill : Form
    {
        public frmBackfill()
        {
            InitializeComponent();
        }

        private void frmBackfill_Load(object sender, EventArgs e)
        {
            this.txtTicker.Text = "SPY";
            this.cboPeriod.DataSource = Enum.GetValues(typeof(OHLC_TIMESPAN));
            this.dtFrom.Value = DateTime.Now.AddDays(-15);   //.AddYears(-2);//Past 2 years' data
            this.dtTo.Value = DateTime.Now.AddDays(-1); //yesterday
        }

        /// <summary>
        /// Method to Backfill the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                List<DateTime?> backfillDates = AlpacaAPI.Instance.GetDatesFromRange(dtFrom.Value, dtTo.Value);
                prgProgess.Minimum = 0;
                prgProgess.Maximum = backfillDates.Count;
                prgProgess.Visible = true;
                lblProgress.Text = "";
                lblProgress.Visible = true;

                for (int i = 0; i < backfillDates.Count; i++)
                {
                    lblProgress.Text = (i + 1).ToString() + " of " + backfillDates.Count.ToString();
                    List<OHLC> ohlcData = new List<OHLC>();
                    prgProgess.Value = i + 1;
                    AlpacaAPI.Instance.RemoveExistingHistoricalOHLCs(backfillDates[i].Value.Date, backfillDates[i].Value.Date.AddHours(23), (OHLC_TIMESPAN)cboPeriod.SelectedValue);
                    ohlcData = AlpacaAPI.Instance.Get_TickerData(txtTicker.Text, backfillDates[i].Value.Date, backfillDates[i].Value.Date.AddHours(23), (OHLC_TIMESPAN)cboPeriod.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
