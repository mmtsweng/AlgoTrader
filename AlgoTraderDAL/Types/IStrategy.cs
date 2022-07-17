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
        bool isIntraday { get; set; }
        int openPostions { get; set; }
        Analytics analytics { get; set; }

        void Init();
        Trade Next(OHLC ohlc);
        Trade Close(OHLC ohlc);

        bool BuySignal(OHLC ohlc);
        bool SellSignal(OHLC ohlc);
        Trade MakeTrade(OHLC ohlc, TradeSide side);
    }
}
