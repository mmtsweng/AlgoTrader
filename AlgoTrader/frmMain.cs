using AlgoTraderDAL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoTrader
{
    public partial class frmMain : Form
    {
        private DateTime marketClose;

        public frmMain()
        {
            InitializeComponent();

            //Set Market Close Time
            this.marketClose = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 0, 0);
            tmrMarket.Start();

            //Get
            TDAKey key = TDASettings.Instance.GetDefaultKey();
            if (String.IsNullOrEmpty(key.UserName))
            {
                string newUser = String.Empty;
                if (InputBox.Show("Set Username", "No User was found. Set your Username to continue", ref newUser) == DialogResult.OK)
                {
                    key.UserName = newUser;
                    key.encodedKeyURL = @"https://auth.tdameritrade.com/auth?response_type=code&redirect_uri={{callbackURL}}&client_id={{consumerKey}}%40AMER.OAUTHAP";
                    key.accessTokenURL = @"https://developer.tdameritrade.com/authentication/apis/post/token-0";
                    key.last_update = DateTime.Now;
                    TDASettings.Instance.UpdateDatabase();
                }
                else
                {
                    //Exit if they decide not to add a User
                    Close();
                }
            }
        }

        private void tmrMarket_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = this.marketClose.Subtract(DateTime.Now);
            if (ts < TimeSpan.Zero)
            {
                lblMarket.Text = "Market Closed";
                lblMarket.BackColor = Color.DarkSlateGray;
                tmrMarket.Stop(); //Market is closed, stop refreshing. Need to manually relaunch every day.
                return;
            }
            else
            {
                if (ts.Hours <= 0)
                {
                    lblMarket.BackColor = Color.DarkOrange; //Power Hour
                }
                else
                {
                    lblMarket.BackColor = Color.DarkGreen;
                }
            }

            this.lblMarket.Text = "Market Open = " + ts.Hours.ToString() + "H : " + ts.Minutes.ToString() + "M : " + ts.Seconds.ToString() + "S ";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmTDALogin TDA = new frmTDALogin();
            TDA.MdiParent = this;
            TDA.Show();
        }
    }
}
