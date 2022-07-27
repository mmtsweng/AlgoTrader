using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlgoTraderDAL;

namespace AlgoTrader
{
    public partial class frmTransactions : Form
    {
        public frmTransactions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form Load, set the dt picker to the last date with data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTransactions_Load(object sender, EventArgs e)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    this.dtDate.Value = entities.HistoricalTransactions.OrderBy(t => t.transactionDT).LastOrDefault().transactionDT.GetValueOrDefault();
                }
            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>
        /// User picked a different date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //Get and show individual transactions
                List<HistoricalTransaction> transactions = new List<HistoricalTransaction>();
                using (Entities entities = new Entities())
                {
                    transactions = entities.HistoricalTransactions.Where(
                        t => t.transactionDT.Value.Year == this.dtDate.Value.Year
                        && t.transactionDT.Value.Month == this.dtDate.Value.Month
                        && t.transactionDT.Value.Day == this.dtDate.Value.Day
                        )
                        .OrderBy(o => o.transactionDT)
                        .ToList();
                }
                this.dgTrades.DataSource = transactions;    

                //Show P&L
                decimal profits = transactions.Where(t => t.side.Trim() == "SELL").Select(s => s.actualPrice).Sum().GetValueOrDefault();
                profits -= transactions.Where(t => t.side.Trim() == "BUY").Select(s => s.actualPrice).Sum().GetValueOrDefault();
                this.txtProfits.Text = Math.Round(profits, 2).ToString();
                if (profits > 0)
                { this.txtProfits.BackColor = Color.LightGreen; }
                else if (profits < 0)
                { this.txtProfits.BackColor = Color.LightPink; }
                else
                { this.txtProfits.BackColor = Color.White; }
            }
            catch (Exception ex)
            {
                this.txtProfits.Text = ex.Message;
            }
        }
    }
}
