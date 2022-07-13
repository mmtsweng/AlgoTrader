using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgoTrader
{
#pragma warning disable IDE1006 // Naming Styles
    public partial class frmAlpacaData : Form
#pragma warning restore IDE1006 // Naming Styles
    {
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
            this.dtFrom.Value = DateTime.Now.AddDays(-30);//a month ago
            this.dtTo.Value = DateTime.Now.AddDays(-1); //yesterday
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
            List<AlgoTraderDAL.Types.OHLC> results = alpca.Get_TickerData(this.txtTicker.Text, this.dtFrom.Value, this.dtTo.Value);
            dgResults.DataSource = results;
            dgResults.Update();
            this.Cursor = Cursors.Default;
        }
    }
}
