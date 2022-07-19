namespace AlgoTrader
{
    partial class frmAlpacaData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAlpacaData));
            this.label1 = new System.Windows.Forms.Label();
            this.lblTicker = new System.Windows.Forms.Label();
            this.txtTicker = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.btnChart = new System.Windows.Forms.Button();
            this.cmdBacktest = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cboPeriod = new System.Windows.Forms.ComboBox();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblDays = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // lblTicker
            // 
            this.lblTicker.AutoSize = true;
            this.lblTicker.Location = new System.Drawing.Point(12, 14);
            this.lblTicker.Name = "lblTicker";
            this.lblTicker.Size = new System.Drawing.Size(84, 15);
            this.lblTicker.TabIndex = 0;
            this.lblTicker.Text = "Ticker Symbol:";
            // 
            // txtTicker
            // 
            this.txtTicker.Location = new System.Drawing.Point(100, 11);
            this.txtTicker.Name = "txtTicker";
            this.txtTicker.Size = new System.Drawing.Size(64, 23);
            this.txtTicker.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 103);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(82, 23);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(100, 40);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(200, 23);
            this.dtFrom.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "From Date:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "To Date:";
            // 
            // dtTo
            // 
            this.dtTo.Location = new System.Drawing.Point(100, 70);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(200, 23);
            this.dtTo.TabIndex = 5;
            // 
            // dgResults
            // 
            this.dgResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResults.Location = new System.Drawing.Point(12, 136);
            this.dgResults.Name = "dgResults";
            this.dgResults.RowTemplate.Height = 25;
            this.dgResults.Size = new System.Drawing.Size(651, 267);
            this.dgResults.TabIndex = 7;
            // 
            // btnChart
            // 
            this.btnChart.Enabled = false;
            this.btnChart.Location = new System.Drawing.Point(100, 103);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(82, 23);
            this.btnChart.TabIndex = 8;
            this.btnChart.Text = "Show Chart";
            this.btnChart.UseVisualStyleBackColor = true;
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // cmdBacktest
            // 
            this.cmdBacktest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBacktest.Enabled = false;
            this.cmdBacktest.Location = new System.Drawing.Point(562, 107);
            this.cmdBacktest.Name = "cmdBacktest";
            this.cmdBacktest.Size = new System.Drawing.Size(101, 23);
            this.cmdBacktest.TabIndex = 9;
            this.cmdBacktest.Text = "Run Backtest";
            this.cmdBacktest.UseVisualStyleBackColor = true;
            this.cmdBacktest.Click += new System.EventHandler(this.cmdBacktest_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(202, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Period";
            // 
            // cboPeriod
            // 
            this.cboPeriod.FormattingEnabled = true;
            this.cboPeriod.Location = new System.Drawing.Point(249, 11);
            this.cboPeriod.Name = "cboPeriod";
            this.cboPeriod.Size = new System.Drawing.Size(121, 23);
            this.cboPeriod.TabIndex = 11;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevious.Location = new System.Drawing.Point(437, 107);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(38, 23);
            this.btnPrevious.TabIndex = 12;
            this.btnPrevious.Text = "<<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Visible = false;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(518, 107);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(38, 23);
            this.btnNext.TabIndex = 13;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblDays
            // 
            this.lblDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDays.AutoSize = true;
            this.lblDays.Location = new System.Drawing.Point(481, 111);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(32, 15);
            this.lblDays.TabIndex = 14;
            this.lblDays.Text = "Days";
            this.lblDays.Visible = false;
            // 
            // frmAlpacaData
            // 
            this.ClientSize = new System.Drawing.Size(675, 415);
            this.Controls.Add(this.lblDays);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.cboPeriod);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdBacktest);
            this.Controls.Add(this.btnChart);
            this.Controls.Add(this.dgResults);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtTicker);
            this.Controls.Add(this.lblTicker);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAlpacaData";
            this.Load += new System.EventHandler(this.frmAlpacaData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTicker;
        private System.Windows.Forms.TextBox txtTicker;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.Button btnChart;
        private System.Windows.Forms.Button cmdBacktest;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboPeriod;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblDays;
    }
}