namespace AlgoTrader
{
    partial class frmTDALogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTDALogin));
            this.Keys = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCallbackURL = new System.Windows.Forms.TextBox();
            this.txtAMC = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnRegenerateToken = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnVerifyAPI = new System.Windows.Forms.Button();
            this.txtRT = new System.Windows.Forms.TextBox();
            this.txtAT = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDAC = new System.Windows.Forms.TextBox();
            this.btnDecode = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEAC = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtConsumerKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Keys.SuspendLayout();
            this.SuspendLayout();
            // 
            // Keys
            // 
            this.Keys.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Keys.Controls.Add(this.label10);
            this.Keys.Controls.Add(this.txtCallbackURL);
            this.Keys.Controls.Add(this.txtAMC);
            this.Keys.Controls.Add(this.label9);
            this.Keys.Controls.Add(this.btnRegenerateToken);
            this.Keys.Controls.Add(this.label8);
            this.Keys.Controls.Add(this.btnVerifyAPI);
            this.Keys.Controls.Add(this.txtRT);
            this.Keys.Controls.Add(this.txtAT);
            this.Keys.Controls.Add(this.label7);
            this.Keys.Controls.Add(this.label6);
            this.Keys.Controls.Add(this.label5);
            this.Keys.Controls.Add(this.txtDAC);
            this.Keys.Controls.Add(this.btnDecode);
            this.Keys.Controls.Add(this.label4);
            this.Keys.Controls.Add(this.txtEAC);
            this.Keys.Controls.Add(this.label3);
            this.Keys.Controls.Add(this.label2);
            this.Keys.Controls.Add(this.btnLogin);
            this.Keys.Controls.Add(this.txtConsumerKey);
            this.Keys.Controls.Add(this.label1);
            this.Keys.Location = new System.Drawing.Point(15, 15);
            this.Keys.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Keys.Name = "Keys";
            this.Keys.Size = new System.Drawing.Size(362, 598);
            this.Keys.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 47);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(108, 15);
            this.label10.TabIndex = 21;
            this.label10.Text = "...and Callback URL";
            // 
            // txtCallbackURL
            // 
            this.txtCallbackURL.Location = new System.Drawing.Point(30, 65);
            this.txtCallbackURL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCallbackURL.Name = "txtCallbackURL";
            this.txtCallbackURL.Size = new System.Drawing.Size(296, 23);
            this.txtCallbackURL.TabIndex = 20;
            this.txtCallbackURL.Text = "https://localhost:8080/";
            // 
            // txtAMC
            // 
            this.txtAMC.Location = new System.Drawing.Point(30, 505);
            this.txtAMC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtAMC.Multiline = true;
            this.txtAMC.Name = "txtAMC";
            this.txtAMC.ReadOnly = true;
            this.txtAMC.Size = new System.Drawing.Size(296, 22);
            this.txtAMC.TabIndex = 19;
            this.txtAMC.Text = resources.GetString("txtAMC.Text");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 545);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(177, 15);
            this.label9.TabIndex = 18;
            this.label9.Text = "Step 7: Regenerate Access Token";
            // 
            // btnRegenerateToken
            // 
            this.btnRegenerateToken.Enabled = false;
            this.btnRegenerateToken.Location = new System.Drawing.Point(29, 563);
            this.btnRegenerateToken.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRegenerateToken.Name = "btnRegenerateToken";
            this.btnRegenerateToken.Size = new System.Drawing.Size(113, 27);
            this.btnRegenerateToken.TabIndex = 17;
            this.btnRegenerateToken.Text = "Regenerate";
            this.btnRegenerateToken.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 453);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 15);
            this.label8.TabIndex = 16;
            this.label8.Text = "Step 6: Verify API Access";
            // 
            // btnVerifyAPI
            // 
            this.btnVerifyAPI.Enabled = false;
            this.btnVerifyAPI.Location = new System.Drawing.Point(29, 472);
            this.btnVerifyAPI.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnVerifyAPI.Name = "btnVerifyAPI";
            this.btnVerifyAPI.Size = new System.Drawing.Size(113, 27);
            this.btnVerifyAPI.TabIndex = 15;
            this.btnVerifyAPI.Text = "Verify API";
            this.btnVerifyAPI.UseVisualStyleBackColor = true;
            this.btnVerifyAPI.Click += new System.EventHandler(this.btnVerifyAPI_Click);
            // 
            // txtRT
            // 
            this.txtRT.Location = new System.Drawing.Point(30, 406);
            this.txtRT.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtRT.Name = "txtRT";
            this.txtRT.ReadOnly = true;
            this.txtRT.Size = new System.Drawing.Size(296, 23);
            this.txtRT.TabIndex = 14;
            // 
            // txtAT
            // 
            this.txtAT.Location = new System.Drawing.Point(29, 361);
            this.txtAT.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtAT.Name = "txtAT";
            this.txtAT.ReadOnly = true;
            this.txtAT.Size = new System.Drawing.Size(296, 23);
            this.txtAT.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 388);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 15);
            this.label7.TabIndex = 12;
            this.label7.Text = "Refresh Token";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 343);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "Access Token";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 316);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(259, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Step 5: Retrieve Refresh Token and Access Token";
            // 
            // txtDAC
            // 
            this.txtDAC.Location = new System.Drawing.Point(29, 275);
            this.txtDAC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDAC.Name = "txtDAC";
            this.txtDAC.ReadOnly = true;
            this.txtDAC.Size = new System.Drawing.Size(296, 23);
            this.txtDAC.TabIndex = 9;
            // 
            // btnDecode
            // 
            this.btnDecode.Location = new System.Drawing.Point(29, 241);
            this.btnDecode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(113, 27);
            this.btnDecode.TabIndex = 8;
            this.btnDecode.Text = "Decode";
            this.btnDecode.UseVisualStyleBackColor = true;
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 223);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(186, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Step 4: Decode the Access Tokens ";
            // 
            // txtEAC
            // 
            this.txtEAC.Location = new System.Drawing.Point(30, 185);
            this.txtEAC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtEAC.Name = "txtEAC";
            this.txtEAC.Size = new System.Drawing.Size(296, 23);
            this.txtEAC.TabIndex = 6;
            this.txtEAC.Text = resources.GetString("txtEAC.Text");
            this.txtEAC.TextChanged += new System.EventHandler(this.txtEAC_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 166);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(261, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Step 3: Paste URL from resulting error page here:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 104);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Step 2: Log in to TD Ameritrade";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(30, 122);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(113, 27);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Login to TDA";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtConsumerKey
            // 
            this.txtConsumerKey.Location = new System.Drawing.Point(30, 21);
            this.txtConsumerKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtConsumerKey.Name = "txtConsumerKey";
            this.txtConsumerKey.Size = new System.Drawing.Size(296, 23);
            this.txtConsumerKey.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Step 1: Enter Consumer Key From Developer App";
            // 
            // frmTDALogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 627);
            this.Controls.Add(this.Keys);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmTDALogin";
            this.Text = "TD Ameritrade Login";
            this.Keys.ResumeLayout(false);
            this.Keys.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Keys;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtConsumerKey;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDAC;
        private System.Windows.Forms.Button btnDecode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEAC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnVerifyAPI;
        private System.Windows.Forms.TextBox txtRT;
        private System.Windows.Forms.TextBox txtAT;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnRegenerateToken;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAMC;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtCallbackURL;
    }
}