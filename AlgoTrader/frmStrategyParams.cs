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
    public partial class frmStrategyParams : Form
    {
        public frmStrategyParams()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initial Load -- get strategies with parameters from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmStrategyParams_Load(object sender, EventArgs e)
        {
            using (Entities entities = new Entities())
            {
                List<string> strategies = entities.StrategyOptions
                    .Select(s => s.StrategyName)
                    .Distinct()
                    .ToList();
                this.cboStrategies.DataSource = strategies; 
            }
        }

        /// <summary>
        /// User has selected a Strategy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboStrategies_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strategyName = cboStrategies.SelectedItem.ToString();    

            using(Entities entities = new Entities())
            {
                List<string> parameters = entities.StrategyOptions
                    .Where(o => o.StrategyName == strategyName)
                    .Select(s => s.Parameter)
                    .ToList();
                this.cboParameters.DataSource = parameters; 
            }

        }

        /// <summary>
        /// User has selected the parameter to update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (Entities entities = new Entities())
            {
                string strategyName = cboStrategies.SelectedItem.ToString();
                string parameter = cboParameters.SelectedItem.ToString();
                string dataValue = entities.StrategyOptions
                    .Where(o => o.StrategyName == strategyName
                        && o.Parameter == parameter)
                    .Select(s => s.Value)
                    .FirstOrDefault();
                this.txtParamValue.Text = dataValue;
            }
        }

        /// <summary>
        /// User wants to update the database with the new values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string strategyName = cboStrategies.SelectedItem.ToString();
                string parameter = cboParameters.SelectedItem.ToString();
                using (Entities entities = new Entities())
                {
                    StrategyOption optionObj = entities.StrategyOptions
                        .Where(o => o.StrategyName == strategyName
                            && o.Parameter == parameter)
                        .FirstOrDefault();
                    optionObj.Value = txtParamValue.Text;
                    entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }
    }
}
