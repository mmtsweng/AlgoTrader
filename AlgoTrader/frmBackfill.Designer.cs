namespace AlgoTrader
{
    partial class frmBackfill
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBackfill));
            this.cboPeriod = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.txtTicker = new System.Windows.Forms.TextBox();
            this.lblTicker = new System.Windows.Forms.Label();
            this.prgProgess = new System.Windows.Forms.ProgressBar();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboPeriod
            // 
            this.cboPeriod.FormattingEnabled = true;
            this.cboPeriod.Location = new System.Drawing.Point(249, 6);
            this.cboPeriod.Name = "cboPeriod";
            this.cboPeriod.Size = new System.Drawing.Size(121, 23);
            this.cboPeriod.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(202, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "Period";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "To Date:";
            // 
            // dtTo
            // 
            this.dtTo.Location = new System.Drawing.Point(100, 65);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(200, 23);
            this.dtTo.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "From Date:";
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(100, 35);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(200, 23);
            this.dtFrom.TabIndex = 14;
            // 
            // txtTicker
            // 
            this.txtTicker.Location = new System.Drawing.Point(100, 6);
            this.txtTicker.Name = "txtTicker";
            this.txtTicker.Size = new System.Drawing.Size(64, 23);
            this.txtTicker.TabIndex = 13;
            // 
            // lblTicker
            // 
            this.lblTicker.AutoSize = true;
            this.lblTicker.Location = new System.Drawing.Point(12, 9);
            this.lblTicker.Name = "lblTicker";
            this.lblTicker.Size = new System.Drawing.Size(84, 15);
            this.lblTicker.TabIndex = 12;
            this.lblTicker.Text = "Ticker Symbol:";
            // 
            // prgProgess
            // 
            this.prgProgess.Location = new System.Drawing.Point(12, 121);
            this.prgProgess.Name = "prgProgess";
            this.prgProgess.Size = new System.Drawing.Size(417, 23);
            this.prgProgess.TabIndex = 20;
            this.prgProgess.Visible = false;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(354, 92);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 21;
            this.btnLoad.Text = "Backfill";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(12, 100);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(38, 15);
            this.lblProgress.TabIndex = 22;
            this.lblProgress.Text = "X of Y";
            this.lblProgress.Visible = false;
            // 
            // frmBackfill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 154);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.prgProgess);
            this.Controls.Add(this.cboPeriod);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.txtTicker);
            this.Controls.Add(this.lblTicker);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBackfill";
            this.Text = "frmBackfill";
            this.Load += new System.EventHandler(this.frmBackfill_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboPeriod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.TextBox txtTicker;
        private System.Windows.Forms.Label lblTicker;
        private System.Windows.Forms.ProgressBar prgProgess;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblProgress;
    }
}