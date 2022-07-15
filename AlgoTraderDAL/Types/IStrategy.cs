using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTraderDAL.Types
{
    public interface IStrategy
    {
        bool canOpenMultiplePositons { get; set; }
        int openPostions { get; set; }
        Analytics analytics { get; set; }

        void Init();
        Trade Next(OHLC ohlc);
        void Close(OHLC ohlc);

        bool BuySignal(OHLC ohlc);
        bool SellSignal(OHLC ohlc);
        Trade MakeTrade(OHLC ohlc, TradeSide side);
    }
}
