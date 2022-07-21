using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Alpaca.Markets.Extensions;
using Alpaca.Markets;
using AlgoTraderDAL.Types;
using AlgoTraderDAL.Strategies;

namespace AlgoTraderDAL.Live
{

    public class RealtimeAlpacaAPI
    {

        private static readonly Lazy<RealtimeAlpacaAPI> lazy = new Lazy<RealtimeAlpacaAPI>(() => new RealtimeAlpacaAPI());
        public static RealtimeAlpacaAPI Instance { get { return lazy.Value; } } 
        private AlpacaSetting setting { get; set; }
        private IAlpacaTradingClient alpacaTradingClient { get; set; }
        private IAlpacaDataStreamingClient alpacaDataStreamingClient { get; set; }
        public IAlpacaCryptoStreamingClient alpacaCryptoStreamingClient { get; set; }
        private IAlpacaDataClient alpacaDataClient { get; set; }
        public IAlpacaCryptoDataClient AlpacaCryptoDataClient { get; set; }
        private IIntervalCalendar marketCalendar { get; set; }
        public IStrategy strategy { get; set; }
        public bool isCrypto { get; set; }
        public event EventHandler<OHLC> OHLCReceived;
        public event EventHandler<List<OHLC>> OHLCRefresh;
        public string symbol { get; set; }

        /// <summary>
        /// Constructor. Create clients
        /// </summary>
        private RealtimeAlpacaAPI()
        {
            //Initialize Alpaca Trading Clients
            using (Entities entities = new Entities())
            {
                this.setting = entities.AlpacaSettings.FirstOrDefault();
            }
            SecretKey key = new SecretKey(setting.API_KEY, setting.API_SECRET);
            this.alpacaTradingClient = Environments.Paper.GetAlpacaTradingClient(key);
            this.alpacaDataStreamingClient = Environments.Paper.GetAlpacaDataStreamingClient(key);
            this.alpacaDataClient = Environments.Paper.GetAlpacaDataClient(key);

            this.alpacaCryptoStreamingClient = Environments.Paper.GetAlpacaCryptoStreamingClient(key);
            this.AlpacaCryptoDataClient = Environments.Paper.GetAlpacaCryptoDataClient(key);

            this.strategy = new SimpleMomentum();
            this.isCrypto = false;
            this.strategy.Init();

        }

        /// <summary>
        /// Force Dispose on close
        /// </summary>
        ~RealtimeAlpacaAPI()
        {
            alpacaDataClient.Dispose();
            alpacaDataStreamingClient.Dispose();
            alpacaTradingClient.Dispose();
        }

        /// <summary>
        /// Start Realtime tracking request from external
        /// </summary>
        /// <param name="symbol"></param>
        public async void Start(string symbol, bool isCrypto)
        {
            this.symbol = symbol;
            this.isCrypto = isCrypto;
            await Run();
        }

        /// <summary>
        /// Internal async process to run realtime API calls
        /// </summary>
        /// <returns></returns>
        private async Task Run()
        {
            try
            {
                await GetMarketHours();
                var hours = this.marketCalendar.Trading.ToInterval();

                await GetTodaysTickerData();

                if (this.isCrypto)
                {
                    //Crypto Trading
                    await alpacaCryptoStreamingClient.ConnectAndAuthenticateAsync();
                    var subscription = this.alpacaCryptoStreamingClient.GetMinuteBarSubscription(symbol);
                    subscription.Received += async bar => { ProcessBar(bar); };
                    await alpacaCryptoStreamingClient.SubscribeAsync(subscription);
                }
                else
                {
                    //Securities Trading
                    await alpacaDataStreamingClient.ConnectAndAuthenticateAsync();
                    var subscription = alpacaDataStreamingClient.GetMinuteBarSubscription(symbol);
                    subscription.Received += async bar => { ProcessBar(bar); };
                    await this.alpacaCryptoStreamingClient.SubscribeAsync(subscription);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Get the first group of bars since the market has been open.
        /// </summary>
        /// <returns></returns>
        private async Task GetTodaysTickerData()
        {
            List<OHLC> prices = new List<OHLC>();   
            if (this.isCrypto)
            {
                var bars = await AlpacaCryptoDataClient.ListHistoricalBarsAsync(
                    new HistoricalCryptoBarsRequest(this.symbol, BarTimeFrame.Minute, marketCalendar.Trading));
                prices = bars.Items.Select(p => new OHLC()
                {
                    Open = p.Open,
                    High = p.High,
                    Low = p.Low,
                    Close = p.Close,
                    Volume = p.Volume,
                    Timeframe = p.TimeUtc.ToLocalTime()
                }).ToList();
            }
            else
            {
                var bars = await alpacaDataClient.ListHistoricalBarsAsync(
                   new HistoricalBarsRequest(symbol, BarTimeFrame.Minute, marketCalendar.Trading));
                prices = bars.Items.ToList().Select(p => new OHLC()
                {
                    Open = p.Open,
                    High = p.High,
                    Low = p.Low,
                    Close = p.Close,
                    Volume = p.Volume,
                    Timeframe = p.TimeUtc.ToLocalTime()
                }).ToList();
            }

            OHLCRefresh?.Invoke(this, prices);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopListening()
        {
            var task = Task.Run(async () => { await this.alpacaDataStreamingClient.DisconnectAsync(); return; });
        }

        /// <summary>
        /// We have Received a new bar from ALPACA. Process it
        /// </summary>
        /// <param name="bar"></param>
        private async void ProcessBar(IBar bar)
        {
            OHLC ohlc = new OHLC()
            {
                Symbol = bar.Symbol,
                High = bar.High,
                Open = bar.Open,
                Close = bar.Close,
                Low = bar.Low,
                Volume = bar.Volume,
                Timeframe = bar.TimeUtc.ToLocalTime(),
                ticks = OHLC_TIMESPAN.MINUTE
            };

            OHLCReceived?.Invoke(this, ohlc);
        }

        /// <summary>
        /// Helper method to get the current market hours
        /// </summary>
        /// <returns></returns>
        private async Task GetMarketHours()
        {
            marketCalendar = (await alpacaTradingClient
                .ListIntervalCalendarAsync(new CalendarRequest().WithInterval(DateTime.Today.GetIntervalFromThat())))
                .FirstOrDefault();
        }
    }
}
