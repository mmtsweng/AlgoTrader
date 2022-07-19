namespace AlgoTrader
{
    partial class frmChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChart));
            this.pltChart = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // pltChart
            // 
            this.pltChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pltChart.Location = new System.Drawing.Point(13, 12);
            this.pltChart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pltChart.Name = "pltChart";
            this.pltChart.Size = new System.Drawing.Size(894, 423);
            this.pltChart.TabIndex = 0;
            // 
            // frmChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 447);
            this.Controls.Add(this.pltChart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmChart";
            this.Text = "frmChart";
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.FormsPlot pltChart;
    }
}