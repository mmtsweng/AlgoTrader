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
        private IAlpacaStreamingClient alpacaStreamingClient { get; set; }
        private IAlpacaDataStreamingClient alpacaDataClient { get; set; }
        private IIntervalCalendar marketCalendar { get; set; }
        public IStrategy strategy { get; set; }

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
            alpacaDataClient = Environments.Paper.GetAlpacaDataStreamingClient(new SecretKey(setting.API_KEY, setting.API_SECRET));
            alpacaStreamingClient = Environments.Paper.GetAlpacaStreamingClient(new SecretKey(setting.API_KEY, setting.API_SECRET));

            this.strategy = new SimpleMomentum();
            this.strategy.Init();

        }

        /// <summary>
        /// Force Dispose on close
        /// </summary>
        ~RealtimeAlpacaAPI()
        {
            alpacaDataClient.Dispose();
            alpacaStreamingClient.Dispose();
            alpacaTradingClient.Dispose();
        }

        public async void Start()
        {
            await Run();
        }

        private async Task Run()
        {
            try
            {
                await alpacaDataClient.ConnectAndAuthenticateAsync();
                await GetMarketHours();
                var hours = this.marketCalendar.Trading.ToInterval();
                var subscription = alpacaDataClient.GetMinuteBarSubscription("SPY");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private async Task GetMarketHours()
        {
            marketCalendar = (await alpacaTradingClient
                .ListIntervalCalendarAsync(new CalendarRequest().WithInterval(DateTime.Today.GetIntervalFromThat())))
                .FirstOrDefault();
        }
    }
}
