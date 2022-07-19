namespace AlgoTrader
{
    partial class frmAnalytics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAnalytics));
            this.gpTrade = new System.Windows.Forms.GroupBox();
            this.txtNumTrades = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtEndDate = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStartDate = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMaxLoss = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLossPercent = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLosses = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMaxWins = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWinPercent = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWins = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtNetProfitPercent = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtNetProfit = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtMaxCapital = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMinCapital = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtFinalCapital = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtInitCapital = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.pltResults = new ScottPlot.FormsPlot();
            this.gpTrade.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpTrade
            // 
            this.gpTrade.Controls.Add(this.txtNumTrades);
            this.gpTrade.Controls.Add(this.label9);
            this.gpTrade.Controls.Add(this.txtEndDate);
            this.gpTrade.Controls.Add(this.label8);
            this.gpTrade.Controls.Add(this.txtStartDate);
            this.gpTrade.Controls.Add(this.label7);
            this.gpTrade.Location = new System.Drawing.Point(12, 12);
            this.gpTrade.Name = "gpTrade";
            this.gpTrade.Size = new System.Drawing.Size(859, 67);
            this.gpTrade.TabIndex = 18;
            this.gpTrade.TabStop = false;
            this.gpTrade.Text = "Trade Information";
            // 
            // txtNumTrades
            // 
            this.txtNumTrades.Location = new System.Drawing.Point(744, 22);
            this.txtNumTrades.Name = "txtNumTrades";
            this.txtNumTrades.Size = new System.Drawing.Size(77, 23);
            this.txtNumTrades.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(637, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 15);
            this.label9.TabIndex = 22;
            this.label9.Text = "Number of Trades";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndDate
            // 
            this.txtEndDate.Location = new System.Drawing.Point(413, 22);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Size = new System.Drawing.Size(77, 23);
            this.txtEndDate.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(353, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 15);
            this.label8.TabIndex = 20;
            this.label8.Text = "End Date";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStartDate
            // 
            this.txtStartDate.Location = new System.Drawing.Point(110, 22);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(77, 23);
            this.txtStartDate.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(46, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "Start Date";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMaxLoss);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtLossPercent);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtLosses);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtMaxWins);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtWinPercent);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtWins);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(859, 87);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Wins && Losses";
            // 
            // txtMaxLoss
            // 
            this.txtMaxLoss.Location = new System.Drawing.Point(744, 51);
            this.txtMaxLoss.Name = "txtMaxLoss";
            this.txtMaxLoss.Size = new System.Drawing.Size(77, 23);
            this.txtMaxLoss.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(603, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 15);
            this.label4.TabIndex = 22;
            this.label4.Text = "Max Consecutive Losses";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLossPercent
            // 
            this.txtLossPercent.Location = new System.Drawing.Point(413, 49);
            this.txtLossPercent.Name = "txtLossPercent";
            this.txtLossPercent.Size = new System.Drawing.Size(77, 23);
            this.txtLossPercent.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(334, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 15);
            this.label5.TabIndex = 20;
            this.label5.Text = "Loss Percent";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLosses
            // 
            this.txtLosses.Location = new System.Drawing.Point(110, 54);
            this.txtLosses.Name = "txtLosses";
            this.txtLosses.Size = new System.Drawing.Size(77, 23);
            this.txtLosses.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 15);
            this.label6.TabIndex = 18;
            this.label6.Text = "Losses";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMaxWins
            // 
            this.txtMaxWins.Location = new System.Drawing.Point(744, 22);
            this.txtMaxWins.Name = "txtMaxWins";
            this.txtMaxWins.Size = new System.Drawing.Size(77, 23);
            this.txtMaxWins.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(611, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "Max Consecutive Wins";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWinPercent
            // 
            this.txtWinPercent.Location = new System.Drawing.Point(413, 20);
            this.txtWinPercent.Name = "txtWinPercent";
            this.txtWinPercent.Size = new System.Drawing.Size(77, 23);
            this.txtWinPercent.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(336, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Win Percent";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWins
            // 
            this.txtWins.Location = new System.Drawing.Point(110, 25);
            this.txtWins.Name = "txtWins";
            this.txtWins.Size = new System.Drawing.Size(77, 23);
            this.txtWins.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "Wins";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtNetProfitPercent);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.txtNetProfit);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtMaxCapital);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtMinCapital);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtFinalCapital);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.txtInitCapital);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Location = new System.Drawing.Point(16, 186);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(855, 98);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Financials";
            // 
            // txtNetProfitPercent
            // 
            this.txtNetProfitPercent.Location = new System.Drawing.Point(106, 56);
            this.txtNetProfitPercent.Name = "txtNetProfitPercent";
            this.txtNetProfitPercent.Size = new System.Drawing.Size(77, 23);
            this.txtNetProfitPercent.TabIndex = 33;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(-1, 64);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(101, 15);
            this.label14.TabIndex = 32;
            this.label14.Text = "Net Profit Percent";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNetProfit
            // 
            this.txtNetProfit.Location = new System.Drawing.Point(106, 27);
            this.txtNetProfit.Name = "txtNetProfit";
            this.txtNetProfit.Size = new System.Drawing.Size(77, 23);
            this.txtNetProfit.TabIndex = 31;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(42, 35);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 15);
            this.label15.TabIndex = 30;
            this.label15.Text = "Net Profit";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMaxCapital
            // 
            this.txtMaxCapital.Location = new System.Drawing.Point(740, 51);
            this.txtMaxCapital.Name = "txtMaxCapital";
            this.txtMaxCapital.Size = new System.Drawing.Size(77, 23);
            this.txtMaxCapital.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(632, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 15);
            this.label10.TabIndex = 28;
            this.label10.Text = "Maximum Capital";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMinCapital
            // 
            this.txtMinCapital.Location = new System.Drawing.Point(410, 56);
            this.txtMinCapital.Name = "txtMinCapital";
            this.txtMinCapital.Size = new System.Drawing.Size(77, 23);
            this.txtMinCapital.TabIndex = 27;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(304, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 15);
            this.label11.TabIndex = 26;
            this.label11.Text = "Minimum Capital";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFinalCapital
            // 
            this.txtFinalCapital.Location = new System.Drawing.Point(740, 22);
            this.txtFinalCapital.Name = "txtFinalCapital";
            this.txtFinalCapital.Size = new System.Drawing.Size(77, 23);
            this.txtFinalCapital.TabIndex = 25;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(662, 30);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 15);
            this.label12.TabIndex = 24;
            this.label12.Text = "Final Capital";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtInitCapital
            // 
            this.txtInitCapital.Location = new System.Drawing.Point(410, 27);
            this.txtInitCapital.Name = "txtInitCapital";
            this.txtInitCapital.Size = new System.Drawing.Size(77, 23);
            this.txtInitCapital.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(327, 35);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 15);
            this.label13.TabIndex = 22;
            this.label13.Text = "Initial Capital";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pltResults
            // 
            this.pltResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pltResults.Location = new System.Drawing.Point(12, 290);
            this.pltResults.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pltResults.Name = "pltResults";
            this.pltResults.Size = new System.Drawing.Size(877, 306);
            this.pltResults.TabIndex = 21;
            // 
            // frmAnalytics
            // 
            this.ClientSize = new System.Drawing.Size(904, 608);
            this.Controls.Add(this.pltResults);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gpTrade);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAnalytics";
            this.Text = "Backtest Results";
            this.gpTrade.ResumeLayout(false);
            this.gpTrade.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpTrade;
        private System.Windows.Forms.TextBox txtNumTrades;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtEndDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStartDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtMaxLoss;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLossPercent;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLosses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMaxWins;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWinPercent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWins;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtNetProfitPercent;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtNetProfit;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtMaxCapital;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMinCapital;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtFinalCapital;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtInitCapital;
        private System.Windows.Forms.Label label13;
        private ScottPlot.FormsPlot pltResults;
    }
}