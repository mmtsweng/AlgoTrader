namespace AlgoTrader
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblTrades = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMarket = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrMarket = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLogin = new System.Windows.Forms.ToolStripButton();
            this.btnBackfill = new System.Windows.Forms.ToolStripButton();
            this.btnParameters = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnTransactions = new System.Windows.Forms.ToolStripButton();
            this.btnLiveTrading = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTrades,
            this.lblSpring,
            this.lblMarket});
            this.statusStrip1.Location = new System.Drawing.Point(0, 497);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(933, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblTrades
            // 
            this.lblTrades.Name = "lblTrades";
            this.lblTrades.Size = new System.Drawing.Size(40, 17);
            this.lblTrades.Text = "Trades";
            // 
            // lblSpring
            // 
            this.lblSpring.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblSpring.Name = "lblSpring";
            this.lblSpring.Size = new System.Drawing.Size(832, 17);
            this.lblSpring.Spring = true;
            // 
            // lblMarket
            // 
            this.lblMarket.Name = "lblMarket";
            this.lblMarket.Size = new System.Drawing.Size(44, 17);
            this.lblMarket.Text = "Market";
            // 
            // tmrMarket
            // 
            this.tmrMarket.Interval = 500;
            this.tmrMarket.Tick += new System.EventHandler(this.tmrMarket_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLogin,
            this.btnBackfill,
            this.btnParameters,
            this.toolStripSeparator1,
            this.btnTransactions,
            this.btnLiveTrading});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(933, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnLogin
            // 
            this.btnLogin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLogin.Image = global::AlgoTrader.AlgoResources.Testing;
            this.btnLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(23, 22);
            this.btnLogin.Text = "Run a backtest";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnBackfill
            // 
            this.btnBackfill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBackfill.Image = global::AlgoTrader.AlgoResources.flat_history_icon;
            this.btnBackfill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBackfill.Name = "btnBackfill";
            this.btnBackfill.Size = new System.Drawing.Size(23, 22);
            this.btnBackfill.Text = "Backfill Data";
            this.btnBackfill.ToolTipText = "Backfill data for a Timeframe";
            this.btnBackfill.Click += new System.EventHandler(this.btnBackfill_Click);
            // 
            // btnParameters
            // 
            this.btnParameters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnParameters.Image = global::AlgoTrader.AlgoResources._params;
            this.btnParameters.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnParameters.Name = "btnParameters";
            this.btnParameters.Size = new System.Drawing.Size(23, 22);
            this.btnParameters.Text = "Update Parameters";
            this.btnParameters.ToolTipText = "Strategy Parameters";
            this.btnParameters.Click += new System.EventHandler(this.btnParameters_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnTransactions
            // 
            this.btnTransactions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnTransactions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTransactions.Image = global::AlgoTrader.AlgoResources.transactions;
            this.btnTransactions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTransactions.Name = "btnTransactions";
            this.btnTransactions.Size = new System.Drawing.Size(23, 22);
            this.btnTransactions.Text = "Transactions";
            this.btnTransactions.ToolTipText = "Transaction History";
            this.btnTransactions.Click += new System.EventHandler(this.btnTransactions_Click);
            // 
            // btnLiveTrading
            // 
            this.btnLiveTrading.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnLiveTrading.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLiveTrading.Image = global::AlgoTrader.AlgoResources.Live_trading;
            this.btnLiveTrading.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLiveTrading.Name = "btnLiveTrading";
            this.btnLiveTrading.Size = new System.Drawing.Size(23, 22);
            this.btnLiveTrading.Text = "Realtime Trading";
            this.btnLiveTrading.Click += new System.EventHandler(this.btnLiveTrading_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmMain";
            this.Text = "AlgoTrader";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblTrades;
        private System.Windows.Forms.ToolStripStatusLabel lblSpring;
        private System.Windows.Forms.ToolStripStatusLabel lblMarket;
        private System.Windows.Forms.Timer tmrMarket;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnLogin;
        private System.Windows.Forms.ToolStripButton btnBackfill;
        private System.Windows.Forms.ToolStripButton btnParameters;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnLiveTrading;
        private System.Windows.Forms.ToolStripButton btnTransactions;
    }
}