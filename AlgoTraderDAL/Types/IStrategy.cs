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

        bool isBuyable { get; }

        bool isSellable { get; }   

        int openPostions { get; set; }
        Analytics analytics { get; set; }
        Dictionary<string, string> dbParameters {get; set;}

        void Init();
        void UpdateParameters();

        Trade Next(OHLC ohlc);
        Trade Close(OHLC ohlc);
        bool BuySignal();
        bool SellSignal();
        Trade MakeTrade(OHLC ohlc, TradeSide side);
    }
}
