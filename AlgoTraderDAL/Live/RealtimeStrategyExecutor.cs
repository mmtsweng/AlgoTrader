﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Strategies;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL.Live
{
    public class RealtimeStrategyExecutor
    {
        public IStrategy strategy { get; set; }
        public Portfolio portfolio { get; set; }
        public bool isCrypto { get; set; }
        public string symbol { get; set; }

        public event EventHandler<Trade> TradeOccurred;

        public RealtimeStrategyExecutor()
        {
            this.strategy = new SimpleMomentum(true);
            RealtimeAlpacaAPI.Instance.OHLCReceived += OHLCDataReceived;
            RealtimeAlpacaAPI.Instance.TradeCompleted += TradeNotification;
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
                    submittedPrice = ohlc.Open,
                    side = TradeSide.BUY,
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
            this.portfolio.UpdatePortfolio(trade);
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

        public void Start(string symbol, bool isCrypto)
        {
            this.symbol = symbol;
            this.isCrypto = isCrypto;
            RealtimeAlpacaAPI.Instance.CloseAllPositions();
            this.portfolio = RealtimeAlpacaAPI.Instance.GetPortfolio();
            this.strategy.Init();
        }

        public void End(string symbol, bool isCrypto)
        {
            RealtimeAlpacaAPI.Instance.CloseAllPositions();
        }

    }
}
