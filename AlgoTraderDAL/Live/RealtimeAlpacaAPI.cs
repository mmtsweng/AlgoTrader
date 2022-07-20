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
        private IAlpacaDataClient alpacaDataClient { get; set; }
        private IIntervalCalendar marketCalendar { get; set; }
        public IStrategy strategy { get; set; }
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
            alpacaTradingClient = Environments.Paper.GetAlpacaTradingClient(new SecretKey(setting.API_KEY, setting.API_SECRET));
            alpacaDataStreamingClient = Environments.Paper.GetAlpacaDataStreamingClient(new SecretKey(setting.API_KEY, setting.API_SECRET));
            alpacaDataClient = Environments.Paper.GetAlpacaDataClient(new SecretKey(setting.API_KEY, setting.API_SECRET));

            this.strategy = new SimpleMomentum();
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
        public async void Start(string symbol)
        {
            this.symbol = symbol;
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
                await alpacaDataStreamingClient.ConnectAndAuthenticateAsync();
                await GetMarketHours();
                var hours = this.marketCalendar.Trading.ToInterval();

                // await GetTodaysTickerData();

                //Connect to Alpaca's websocket and listen for price updates
                var subscription = alpacaDataStreamingClient.GetMinuteBarSubscription(symbol);
                subscription.Received += async bar => { ProcessBar(bar); };
                await alpacaDataStreamingClient.SubscribeAsync(subscription);
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
            var bars = await alpacaDataClient.ListHistoricalBarsAsync(
               new HistoricalBarsRequest(symbol, BarTimeFrame.Minute, marketCalendar.Trading));
            List<OHLC> prices = bars.Items.ToList().Select(p => new OHLC()
            {
                Open = p.Open,
                High = p.High,
                Low = p.Low,
                Close = p.Close,
                Volume = p.Volume,
                Timeframe = p.TimeUtc.ToLocalTime()
            }).ToList();

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
