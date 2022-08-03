﻿using System;
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

        public RealtimeStrategyExecutor()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize event listeners, and sanity check
        /// </summary>
        private void Initialize()
        {
            this.strategy = new CryptoMomentum(true);
            RealtimeAlpacaAPI.Instance.OHLCReceived += OHLCDataReceived;
            RealtimeAlpacaAPI.Instance.TradeCompleted += TradeNotification;


            //Setup sanity Check
            this.sanityCheckTimer = new Timer(900000); //15 minutes
            this.sanityCheckTimer.Enabled = true;
            this.sanityCheckTimer.Elapsed += new System.Timers.ElapsedEventHandler(sanityCheck);
            this.sanityCheckTimer.AutoReset = true;
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
            RealtimeAlpacaAPI.Instance.CloseAllPositions();
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
            this.sanityCheckTimer.Elapsed -= new System.Timers.ElapsedEventHandler(sanityCheck);
        }

        /// <summary>
        /// Method to process a new OHLC bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OHLCDataReceived(object sender, OHLC ohlc)
        {
            this.strategy.Next(ohlc);

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

            if (strategy.SellSignal() && strategy.isSellable)
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
        /// If our portfolio doesn't match Alpaca's portfolio, restart.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sanityCheck(object sender, ElapsedEventArgs e)
        {
            if (this.strategy.openPostions != (int)RealtimeAlpacaAPI.Instance.GetOpenPositionCount(this.symbol))
            {
                this.End(this.symbol, this.isCrypto);
                this.Initialize();
                this.Start(this.symbol, this.isCrypto);
            }
        }
    }
}