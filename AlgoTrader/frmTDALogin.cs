using AlgoDAL;
using AlgoTraderDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgoTrader
{
    /*
		<add key="callbackURL" value="https://localhost:8080/" />
		<add key="encodedKey" value="https://auth.tdameritrade.com/auth?response_type=code&amp;redirect_uri={{callbackURL}}&amp;client_id={{consumerKey}}%40AMER.OAUTHAP" />
		<add key="accessToken" value="https://developer.tdameritrade.com/authentication/apis/post/token-0" />
		<add key="ClientSettingsProvider.ServiceUri" value="" />
    */
    public partial class frmTDALogin : Form
    {
        private static readonly HttpClient webClient = new HttpClient();
        private TDAKey settings;

        /// <summary>
        /// This form walks us through the multiple steps required to get an access token and refresh token from TD Ameritrade. Those Tokens are used for all subsequent API Calls
        /// </summary>
        public frmTDALogin()
        {
            InitializeComponent();
            this.btnLogin.Focus();
            settings = TDASettings.Instance.GetDefaultKey();
            updateFieldsFromSettings();
            this.txtConsumerKey.SelectionStart = 0;
            if (!String.IsNullOrEmpty(settings.access_token))
            {
                btnVerifyAPI.Enabled = true;    
                btnDecode.Enabled = true;
            }
            else
            {
                btnVerifyAPI.Enabled = false;
                btnDecode.Enabled = false;
            }
        }

        /// <summary>
        /// Login to TD Ameritrade, using the default browser. It will fail (this is expected), but we need the code in the URL header to determine the next steps
        ///    http://wattcollc.com/TDA/TDAInstr.html has a good walkthrough of the steps, to follow along manually.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string url = settings.encodedKeyURL;
            string callback = txtCallbackURL.Text;
            settings.callbackURL = callback;
            url = url.Replace("{{callbackURL}}", callback);
            url = url.Replace("{{consumerKey}}", this.txtConsumerKey.Text);
            settings.consumerKey = txtConsumerKey.Text;
            settings.encodedKeyURL = url;
            
            Process browser = new Process();
            try
            {
                browser.StartInfo.UseShellExecute = true;
                browser.StartInfo.FileName = url;
                browser.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Open Browser " + ex.Message, "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Once we receive the code/URL from TDAmeritrade, we can move on to the next steps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEAC_TextChanged(object sender, EventArgs e)
        {
            this.btnDecode.Enabled = true;
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            //Get Code from URL
            // Example : https://localhost:8080/?code=PVUbM...
            string searchString = "code=";
            string url = this.txtEAC.Text;
            int ix = url.IndexOf(searchString);

            if (ix == -1)
            {
                MessageBox.Show("Make sure you copy the full URL from the TD Ameritrade site.", "Could not find Code=", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                url = url.Substring(ix + searchString.Length);
            }

            this.txtDAC.Text = WebUtility.UrlDecode(url);
            settings.decodedKey = this.txtDAC.Text;

            //Call API to get Access and Refresh Tokens
            GetAccessTokens(url);
        }

        /// <summary>
        /// Method to set the Access Token and the Refresh Token from the TD Ameritrade API.
        /// </summary>
        /// <param name="url"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void GetAccessTokens(string url)
        {
            var values = new Dictionary<string, string>
            {
                {"grant_type","authorization_code" },
                {"access_type","offline" },
                {"refresh_token",""},
                {"client_id",this.txtConsumerKey.Text},
                {"redirect_uri", settings.callbackURL},
                {"code", this.txtDAC.Text}
            };
            var content = new FormUrlEncodedContent(values);

            // call API Async
            var response = webClient.PostAsync("https://api.tdameritrade.com/v1/oauth2/token", content).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                parseAndProcessAPIResponse(responseString);
            }
        }

        /// <summary>
        /// Method to extract the Access Token and Refresh Token from the response
        /// </summary>
        /// <param name="responseString"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void parseAndProcessAPIResponse(string responseString)
        {
            TDATokens tokens = JsonSerializer.Deserialize<TDATokens>(responseString)!;
            settings.scope = tokens.scope;  
            settings.access_token = tokens.access_token;
            settings.refresh_token = tokens.refresh_token;
            settings.scope = tokens.scope;
            settings.access_expires = tokens.expires_in;
            settings.refresh_expires = tokens.refresh_token_expires_in;
            TDASettings.Instance.UpdateDatabase();

            updateFieldsFromSettings();
            btnVerifyAPI.Enabled = true;    
        }

        private void updateFieldsFromSettings() 
        {
            txtAT.Text = settings.access_token;
            txtRT.Text = settings.refresh_token;
            txtDAC.Text = settings.decodedKey;
            txtEAC.Text = settings.encodedKeyURL;
            txtConsumerKey.Text = settings.consumerKey;
        }

        /// <summary>
        /// Method to retrieve the current $AMC stock price from the TDA API, verifying the access tokens are correct 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerifyAPI_Click(object sender, EventArgs e)
        {
            GetAMCPrice();
        }

        private async void GetAMCPrice()
        {

            using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://api.tdameritrade.com/v1/marketdata/AMC/quotes?apikey=" + settings.consumerKey)))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.access_token);
                var response = await webClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string results = await response.Content.ReadAsStringAsync();
                    Equity AMC = JsonSerializer.Deserialize<Equity>(results);
                    this.txtAMC.Text = "AMC Last Price : " + AMC.AMC.lastPrice.ToString();
                }
                else 
                {
                    MessageBox.Show("Unable to Retrieve AMC Prices because " + response.ReasonPhrase, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
