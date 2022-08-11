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
    public partial class frmErrorLogs : Form
    {
        public List<ErrorLog> logs { get; set; }

        public frmErrorLogs()
        {
            InitializeComponent();
        }

        ///<summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmErrorLogs_Load(object sender, EventArgs e)
        {
            RefreshLogs();
        }

        /// <summary>
        /// User requested a refresh of the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshLogs();

        }

        /// <summary>
        /// Get Logs from the database
        /// </summary>
        private void RefreshLogs()
        {
            using (Entities entities = new Entities())
            {
                logs = entities.ErrorLogs.ToList();
                dgLogs.DataSource = logs;
                this.dgLogs.Columns["Id"].DisplayIndex = 0;
                this.dgLogs.Columns["LogTime"].DisplayIndex = 1;
                this.dgLogs.Columns["Source"].DisplayIndex = 2;
                this.dgLogs.Columns["Source"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.dgLogs.Columns["Message"].DisplayIndex = 3;
                this.dgLogs.Columns["Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgLogs.Refresh();
            }
        }

        private void dgLogs_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.logs.Count > 0 && this.dgLogs.CurrentRow != null)
                {
                    int id = (int)this.dgLogs.CurrentRow.Cells["Id"].Value;
                    ErrorLog log = logs.Where(x => x.Id == id).FirstOrDefault();

                    this.txtMessage.Text = log.Message;
                    this.txtSource.Text = log.Source;
                    this.txtStack.Text = log.StackTrace;
                    this.txtLogTime.Text = log.LogTime.ToString();
                }
                else
                {
                    this.txtMessage.Text = String.Empty;
                    this.txtSource.Text = String.Empty;
                    this.txtStack.Text = String.Empty;
                    this.txtLogTime.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.LogException("frmErrorLogs.dgLogs_SelectionChanged()", ex);
            }
        }

        /// <summary>
        /// Clear All Exceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all the logs in the database?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using(Entities entities = new Entities())
                {
                    entities.ErrorLogs.RemoveRange(entities.ErrorLogs);
                    entities.SaveChanges();
                }
            }
        }
    }
}
