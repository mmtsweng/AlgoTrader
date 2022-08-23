using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Strategies;
using AlgoTraderDAL.Types;
using System.Timers;

namespace AlgoTraderDAL.Live
{
    public class RealtimeStrategyExecutor
    {
        public IStrategy strategy { get; set; }
        public Portfolio portfolio { get; set; }
        public Timer sanityCheckTimer { get; set; }
        public bool isCrypto { get; set; }
        public string symbol { get; set; }

        public event EventHandler<Trade> TradeOccurred;
        public event EventHandler<Exception> TradeFailure;

        public RealtimeStrategyExecutor()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize event listeners, and sanity check
        /// </summary>
        private void Initialize()
        {
            this.strategy = new CryptoBollingerStrategy(true);
        }

        /// <summary>
        /// Start Running the Strategy
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="isCrypto"></param>
        public void Start(string symbol, bool isCrypto)
        {

            this.symbol = symbol;
            this.isCrypto = isCrypto;
            RealtimeAlpacaAPI.Instance.Start(symbol, isCrypto);
            RealtimeAlpacaAPI.Instance.CloseAllPositions();

            System.Threading.Thread.Sleep(1000); // Wait a second.

            //Put some cash in the account for commission fees
            RealtimeAlpacaAPI.Instance.SubmitTrade(new Trade()
            {
                quantity = 1,
                side = TradeSide.BUY,
                symbol = symbol
            });

            RealtimeAlpacaAPI.Instance.OHLCReceived += OHLCDataReceived;
            RealtimeAlpacaAPI.Instance.TradeCompleted += TradeNotification;
            RealtimeAlpacaAPI.Instance.APIException += APIExceptionNotification;

            this.portfolio = RealtimeAlpacaAPI.Instance.GetPortfolio();
            this.strategy.Init();          
        }

        /// <summary>
        /// Stop the strategy, and reset
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="isCrypto"></param>
        public void End(string symbol, bool isCrypto)
        {
            RealtimeAlpacaAPI.Instance.CloseAllPositions();

            RealtimeAlpacaAPI.Instance.OHLCReceived -= OHLCDataReceived;
            RealtimeAlpacaAPI.Instance.TradeCompleted -= TradeNotification;
            RealtimeAlpacaAPI.Instance.APIException -= APIExceptionNotification;
        }

        /// <summary>
        /// Method to process a new OHLC bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OHLCDataReceived(object sender, OHLC ohlc)
        {
            this.strategy.Next(ohlc, true);
            
            if (strategy.BuySignal() && strategy.isBuyable)
            {
                RealtimeAlpacaAPI.Instance.SubmitTrade(new Trade()
                {
                    quantity = 1,
                    submittedPrice = Decimal.Multiply(ohlc.Open, 3M),
                    side = TradeSide.BUY,
                    stopLossPrice = ohlc.Low,
                    symbol = symbol
                });
            }
            else if (strategy.SellSignal() && strategy.isSellable)
            {
                RealtimeAlpacaAPI.Instance.SubmitTrade(new Trade()
                {
                    quantity = 1,
                    submittedPrice = ohlc.Open,
                    side = TradeSide.SELL,
                    symbol = symbol
                });
            }
        }

        /// <summary>
        /// API Notified us of a trade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="trade"></param>
        private void TradeNotification(object sender, Trade trade)
        {
            this.portfolio.UpdatePortfolio(trade, true);
            if (trade.side == TradeSide.BUY)
            {
                strategy.openPostions++;
            }
            else
            {
                strategy.openPostions--;
            }
            TradeOccurred?.Invoke(this, trade);
        }

        /// <summary>
        /// API notified us that there was an error. Log it and try restarting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void APIExceptionNotification(object sender, Exception e)
        {
            ErrorLogger.Instance.LogException("RealtimeStrategyExecutor.APIExceptionNotification()", e);
            TradeFailure?.Invoke(this, RealtimeAlpacaAPI.Instance.tradeException);

            //Try restarting?
            End(this.symbol, this.isCrypto);
            Start(this.symbol, this.isCrypto);
        }

    }
}
