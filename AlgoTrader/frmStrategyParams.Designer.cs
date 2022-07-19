namespace AlgoTrader
{
    partial class frmStrategyParams
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStrategyParams));
            this.cboParameters = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtParamValue = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cboStrategies = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cboParameters
            // 
            this.cboParameters.FormattingEnabled = true;
            this.cboParameters.Location = new System.Drawing.Point(89, 57);
            this.cboParameters.Name = "cboParameters";
            this.cboParameters.Size = new System.Drawing.Size(217, 23);
            this.cboParameters.TabIndex = 0;
            this.cboParameters.SelectedIndexChanged += new System.EventHandler(this.cboParameters_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Parameter:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Value:";
            // 
            // txtParamValue
            // 
            this.txtParamValue.Location = new System.Drawing.Point(90, 95);
            this.txtParamValue.Name = "txtParamValue";
            this.txtParamValue.Size = new System.Drawing.Size(216, 23);
            this.txtParamValue.TabIndex = 3;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(231, 136);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Strategy:";
            // 
            // cboStrategies
            // 
            this.cboStrategies.FormattingEnabled = true;
            this.cboStrategies.Location = new System.Drawing.Point(89, 18);
            this.cboStrategies.Name = "cboStrategies";
            this.cboStrategies.Size = new System.Drawing.Size(217, 23);
            this.cboStrategies.TabIndex = 5;
            this.cboStrategies.SelectedIndexChanged += new System.EventHandler(this.cboStrategies_SelectedIndexChanged);
            // 
            // frmStrategyParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 174);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboStrategies);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtParamValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboParameters);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmStrategyParams";
            this.Text = "Strategy Parameters";
            this.Load += new System.EventHandler(this.frmStrategyParams_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboParameters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtParamValue;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboStrategies;
    }
}