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

namespace AlgoTrader
{
    public partial class frmAnalytics : Form
    {
        public frmAnalytics()
        {
            InitializeComponent();
        }

        public void ProcessAnalytics(Analytics analytics)
        {
            this.txtStartDate.Text = analytics.startDateTime.ToShortDateString();
            this.txtEndDate.Text = analytics.endDateTime.ToShortDateString();
            this.txtNumTrades.Text = analytics.numberOfTrades.ToString();

            this.txtWins.Text = analytics.wins.ToString();
            this.txtWinPercent.Text = Math.Round(analytics.winPercent, 1).ToString();
            this.txtMaxWins.Text = analytics.maxConsecutiveWins.ToString();
            this.txtLosses.Text = analytics.losses.ToString();
            this.txtLossPercent.Text = Math.Round(analytics.lossPercent, 1).ToString();
            this.txtMaxLoss.Text = analytics.maxConsecutiveLosses.ToString();

            this.txtNetProfitPercent.Text = Math.Round(analytics.netProfitPercent,1).ToString();
            this.txtNetProfit.Text = analytics.netProfit.ToString();
            this.txtInitCapital.Text = analytics.initialCapital.ToString();
            this.txtFinalCapital.Text = analytics.finalCapital.ToString();
            this.txtMinCapital.Text = analytics.minCapital.ToString();
            this.txtMaxCapital.Text = analytics.maxCapital.ToString();
            this.gpTrade.Text = "Trade Information -- " + analytics.symbol;

            if (analytics.netProfit < 0)
            {
                this.txtNetProfit.BackColor = Color.LightPink;
                this.txtNetProfitPercent.BackColor = Color.LightPink;
            }
            else
            {
                this.txtNetProfit.BackColor = Color.LightGreen;
                this.txtNetProfitPercent.BackColor = Color.LightGreen;
            }

            this.Focus();
        }
    }
}
