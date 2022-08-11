using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL
{
    public class Portfolio
    {
        public List<Trade> trades = new List<Trade>();
        public string accountNumber { get; set; }
        public decimal startingBalance { get; set; }
        public decimal positionBalance { get; set; }
        public decimal portfolioBalance { get 
            {
                return availableCash + positionBalance;
            }}    
        public decimal availableCash { get; set; }
        public bool accountTradable { get; set; }

        /// <summary>
        /// Construtor - requires a starting balance
        /// </summary>
        /// <param name="startingBalance"></param>
        public Portfolio(decimal startingBalance)
        {
            this.startingBalance = startingBalance; 
            this.availableCash = startingBalance;
            this.positionBalance = 0;
        }

        /// <summary>
        /// Method to update the portfolio with the supplied trade
        /// </summary>
        /// <param name="trade"></param>
        internal void UpdatePortfolio(Trade trade, bool liveTrading)
        {
            if (liveTrading)
            {
                SaveTransactionToDatabase(trade);
            }

            decimal tradeValue = (trade.actualPrice * trade.quantity);
            switch (trade.side)
            {
                case TradeSide.NONE:
                    return;
                case TradeSide.BUY:
                    this.trades.Add(trade);
                    positionBalance += tradeValue;
                    availableCash -= tradeValue;
                    return;
                case TradeSide.SELL:
                    this.trades.Add(trade);
                    positionBalance -= tradeValue;
                    availableCash += tradeValue;
                    return;
            }
        }

        /// <summary>
        /// Store this transaction int the database
        /// </summary>
        /// <param name="trade"></param>
        private static void SaveTransactionToDatabase(Trade trade)
        {
            try
            {
                using (Entities entities = new Entities())
                {
                    HistoricalTransaction transaction = new HistoricalTransaction()
                    {
                        actualPrice = trade.actualPrice,
                        symbol = trade.symbol,
                        alpacaID = trade.TradeId.ToString(),
                        dollarquantity = trade.dollarQuantity,
                        quantity = (int)trade.quantity,
                        transactionDT = trade.transactionDateTime,
                        side = trade.side.ToString(),
                        type = trade.type.ToString()
                    };

                    entities.HistoricalTransactions.Add(transaction);
                    entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.LogException("Portfolio.SaveTransactionToDatabase()", ex);
            }
        }
    }
}
