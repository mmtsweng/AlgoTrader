using AlgoTraderDAL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoTrader
{
    public partial class frmMain : Form
    {
        private DateTime marketClose;

        public frmMain()
        {
            InitializeComponent();

            //Set Market Close Time
            this.marketClose = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 0, 0);
            tmrMarket.Start();
        }

        private void tmrMarket_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = this.marketClose.Subtract(DateTime.Now);
            if (ts < TimeSpan.Zero)
            {
                lblMarket.Text = "Market Closed";
                lblMarket.BackColor = Color.DarkSlateGray;
                tmrMarket.Stop(); //Market is closed, stop refreshing. Need to manually relaunch every day.
                return;
            }
            else
            {
                if (ts.Hours <= 0)
                {
                    lblMarket.BackColor = Color.DarkOrange; //Power Hour
                }
                else
                {
                    lblMarket.BackColor = Color.DarkGreen;
                }
            }

            this.lblMarket.Text = "Market Open = " + ts.Hours.ToString() + "H : " + ts.Minutes.ToString() + "M : " + ts.Seconds.ToString() + "S ";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmAlpacaData aplaca = new frmAlpacaData();
            aplaca.MdiParent = this;
            aplaca.Show();
        }

        private void btnBackfill_Click(object sender, EventArgs e)
        {
            frmBackfill backfill = new frmBackfill();
            backfill.MdiParent = this;
            backfill.Show();
        }

        private void btnParameters_Click(object sender, EventArgs e)
        {
            frmStrategyParams parameters = new frmStrategyParams();
            parameters.MdiParent = this;
            parameters.Show();
        }

        private void btnLiveTrading_Click(object sender, EventArgs e)
        {
            frmRealtimeTrades realtimeTrades = new frmRealtimeTrades();
            realtimeTrades.MdiParent = this;
            realtimeTrades.Show();
        }
    }
}
