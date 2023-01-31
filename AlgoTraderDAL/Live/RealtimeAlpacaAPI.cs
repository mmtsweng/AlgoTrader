using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
        public IAlpacaStreamingClient alpacaStreamingClient { get; set; }
        public IAlpacaCryptoStreamingClient alpacaCryptoStreamingClient { get; set; }
        private IAlpacaDataClient alpacaDataClient { get; set; }
        public IAlpacaCryptoDataClient AlpacaCryptoDataClient { get; set; }
        private IIntervalCalendar marketCalendar { get; set; }
        private IBar lastBarReceived { get; set; }
        public Exception tradeException { get; set; }

        public bool isCrypto { get; set; }
        public event EventHandler<OHLC> OHLCReceived;
        public event EventHandler<List<OHLC>> OHLCRefresh;
        public event EventHandler<AlgoTraderDAL.Trade> TradeCompleted;
        public event EventHandler<Exception> APIException;

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
            this.alpacaStreamingClient = Environments.Paper.GetAlpacaStreamingClient(key);

            this.alpacaCryptoStreamingClient = Environments.Paper.GetAlpacaCryptoStreamingClient(key);
            this.AlpacaCryptoDataClient = Environments.Paper.GetAlpacaCryptoDataClient(key);
            this.isCrypto = false;
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
        public async void Init()
        {
            await GetMarketHours();
            var hours = this.marketCalendar.Trading.ToInterval();
        }

        /// <summary>
        /// Start listening for streaming data
        /// </summary>
        public async void Start(string symbol, bool isCrypto)
        {
            this.symbol = symbol;
            this.isCrypto = isCrypto;
            await SetupSubscriptions();
        }

        /// <summary>
        /// Method to request historical data
        /// </summary>
        public async void RequestTodayHistoricalTickerData()
        {
            await GetTodaysTickerData();
        }

        /// <summary>
        /// Process to submit a trade to Alpaca
        /// </summary>
        /// <param name="trade"></param>
        public async void SubmitTrade(Trade trade)
        {
            this.tradeException = null;
            OrderSide orderside = OrderSide.Buy;
            switch (trade.side)
            {
                case TradeSide.NONE:
                    return;  // Not a valid order
                case TradeSide.BUY:
                    orderside = OrderSide.Buy;
                    break;
                case TradeSide.SELL:
                    orderside = OrderSide.Sell;
                    break;
                default:
                    break;
            }

            try
            {
                OrderQuantity quantity;
                if (trade.dollarQuantity > 0)
                {
                    quantity = OrderQuantity.Notional(trade.dollarQuantity);
                }
                else
                {
                    quantity = OrderQuantity.FromInt64((int)trade.quantity);
                }
                MarketOrder market = orderside.Market(this.symbol, quantity).WithDuration(TimeInForce.Day);
                var order = await alpacaTradingClient.PostOrderAsync(market);   
            }
            catch (Exception e) // What to do if the order fails???
            {
                ErrorLogger.Instance.LogException("RealtimeAlpacaAPI.SubmitTrade()", e);
                APIException?.Invoke(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopListening()
        {
            this.CloseAllPositions();
            var task = Task.Run(async () => { await this.alpacaDataStreamingClient.DisconnectAsync(); return; });
            task = Task.Run(async () => { await this.alpacaCryptoStreamingClient.DisconnectAsync(); return; });
        }

        /// <summary>
        /// Close all existing open positions
        /// </summary>
        public void CloseAllPositions()
        {
            try
            {
                IPosition position = alpacaTradingClient.GetPositionAsync(symbol).GetAwaiter().GetResult();
                if (position.IntegerQuantity > 0)
                {
                    alpacaTradingClient.PostOrderAsync(
                        OrderSide.Sell.Market(symbol, OrderQuantity.Fractional(position.Quantity)).WithDuration(TimeInForce.Gtc))                        
                        .GetAwaiter().GetResult();
                }
            }
            catch (Exception ex) 
            {
                ErrorLogger.Instance.LogException("RealtimeAlpacaAPI.CloseAllPositions()", ex);
                // No position to exit.
            }
        }

        /// <summary>
        /// Get Existing Portfolio information
        /// </summary>
        /// <returns></returns>
        public Portfolio GetPortfolio()
        {
            var account = alpacaTradingClient.GetAccountAsync().GetAwaiter().GetResult();

            Portfolio portfolio = new Portfolio(account.TradableCash);
            portfolio.accountNumber = account.AccountNumber;
            portfolio.accountTradable = (!account.IsTradingBlocked && !account.IsAccountBlocked);

            return portfolio;
        }

        /// <summary>
        /// Method to get Alpaca's open positions on a symbol
        /// </summary>
        /// <param name="symbol"></param>
        public decimal GetOpenPositionCount(string symbol)
        {
            return GetOpenPositionsFromAlpaca(symbol).GetAwaiter().GetResult();
        }

        /*
         * 
         * Private Members Below
         * 
         */

        private async Task GetTodaysTickerData()
        {
            if (this.marketCalendar == null) { return; } //async protection
            List<OHLC> prices = new List<OHLC>();   
            if (this.isCrypto)
            {
                var bars = await AlpacaCryptoDataClient.ListHistoricalBarsAsync(
                    new HistoricalCryptoBarsRequest(this.symbol, DateTime.Now.ToUniversalTime().Date, DateTime.Now.ToUniversalTime(), BarTimeFrame.Minute).WithPageSize(9000).WithExchanges(CryptoExchange.Ftx));
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

        private async Task<decimal> GetOpenPositionsFromAlpaca(string symbol)
        {
            try
            {
                var positions = await this.alpacaTradingClient.GetPositionAsync(symbol);
                return (decimal)positions.Quantity;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("does not exist"))
                { return 0; }
                else
                { return -1; }                
            }
        }

        /// <summary>
        /// Internal async process to run realtime API calls
        /// </summary>
        /// <returns></returns>
        private async Task SetupSubscriptions()
        {
            try
            {
                if (this.isCrypto)
                {
                    //Crypto Trading
                    await this.alpacaCryptoStreamingClient.ConnectAndAuthenticateAsync();
                    await this.alpacaStreamingClient.ConnectAndAuthenticateAsync();
                    this.alpacaStreamingClient.OnTradeUpdate += async t => { ProcessTrade(t); };

                    var subscription = this.alpacaCryptoStreamingClient.GetMinuteBarSubscription(symbol);
                    subscription.Received += bar => { ProcessBar(bar); };
                    await this.alpacaCryptoStreamingClient.SubscribeAsync(subscription);
                }
                else
                {
                    //Securities Trading
                    await this.alpacaDataStreamingClient.ConnectAndAuthenticateAsync();
                    var subscription = this.alpacaDataStreamingClient.GetMinuteBarSubscription(symbol);
                    subscription.Received += bar => { ProcessBar(bar); };
                    await this.alpacaCryptoStreamingClient.SubscribeAsync(subscription);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.LogException("RealtimeAlpacaAPI.SetupSubscriptions()", ex);
            }

        }

        /// <summary>
        /// We have received confirmation of a trade from ALPACA. Process it.
        /// </summary>
        /// <param name="trade"></param>
        private async void ProcessTrade(ITradeUpdate trade)
        {
            if (trade.Event == TradeEvent.Fill)
            {
                TradeSide tradeside = TradeSide.NONE;
                switch (trade.Order.OrderSide)
                {
                    case OrderSide.Buy:
                        tradeside = TradeSide.BUY;
                        break;
                    case OrderSide.Sell:
                        tradeside = TradeSide.SELL;
                        break;
                    default:
                        tradeside = TradeSide.NONE;
                        break;
                }

                Trade completedTrade = new Trade(true)
                {
                    TradeId = trade.Order.OrderId,
                    symbol = trade.Order.Symbol,
                    quantity = trade.Order.Quantity.GetValueOrDefault(),
                    dollarQuantity = trade.Order.Notional.GetValueOrDefault(),
                    transactionDateTime = trade.TimestampUtc.GetValueOrDefault().ToLocalTime(),
                    actualPrice = trade.Order.AverageFillPrice.GetValueOrDefault(),
                    side = tradeside
                };

                TradeCompleted?.Invoke(this, completedTrade);
            }
            
        }

        /// <summary>
        /// We have Received a new bar from ALPACA. Process it
        /// </summary>
        /// <param name="bar"></param>
        private async void ProcessBar(IBar bar)
        {
            if (this.lastBarReceived != null && this.lastBarReceived.TimeUtc.Equals(bar.TimeUtc))
            {
                return;
            }
            else
            {
                this.lastBarReceived = bar;
            }

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
            this.marketCalendar = (await alpacaTradingClient
                .ListIntervalCalendarAsync(new CalendarRequest().WithInterval(DateTime.Today.GetIntervalFromThat())))
                .FirstOrDefault();
        }
    }
}
