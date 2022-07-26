namespace AlgoTrader
{
    partial class frmRealtimeTrades
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRealtimeTrades));
            this.btnTest = new System.Windows.Forms.Button();
            this.pltChart = new ScottPlot.FormsPlot();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.chkCrypto = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(581, 14);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "Run";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // pltChart
            // 
            this.pltChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pltChart.Location = new System.Drawing.Point(12, 37);
            this.pltChart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pltChart.Name = "pltChart";
            this.pltChart.Size = new System.Drawing.Size(664, 295);
            this.pltChart.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Symbol:";
            // 
            // txtSymbol
            // 
            this.txtSymbol.Location = new System.Drawing.Point(71, 11);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(100, 23);
            this.txtSymbol.TabIndex = 3;
            // 
            // chkCrypto
            // 
            this.chkCrypto.AutoSize = true;
            this.chkCrypto.Checked = true;
            this.chkCrypto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCrypto.Location = new System.Drawing.Point(177, 14);
            this.chkCrypto.Name = "chkCrypto";
            this.chkCrypto.Size = new System.Drawing.Size(78, 19);
            this.chkCrypto.TabIndex = 4;
            this.chkCrypto.Text = "Is Crypto?";
            this.chkCrypto.UseVisualStyleBackColor = true;
            // 
            // frmRealtimeTrades
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 335);
            this.Controls.Add(this.chkCrypto);
            this.Controls.Add(this.txtSymbol);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pltChart);
            this.Controls.Add(this.btnTest);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRealtimeTrades";
            this.Text = "Realtime Trading";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRealtimeTrades_FormClosing);
            this.Load += new System.EventHandler(this.frmRealtimeTrades_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTest;
        private ScottPlot.FormsPlot pltChart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSymbol;
        private System.Windows.Forms.CheckBox chkCrypto;
    }
}