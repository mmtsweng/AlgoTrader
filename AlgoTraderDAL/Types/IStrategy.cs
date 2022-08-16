using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Plottable;

namespace AlgoTraderDAL.Types
{
    public interface IStrategy
    {
        bool canOpenMultiplePositons { get; set; }
        bool isIntraday { get; set; }
        bool isBuyable { get; }
        bool isSellable { get; }
        int focusRange { get; set; }
        int openPostions { get; set; }
        Analytics analytics { get; set; }
        Dictionary<string, string> dbParameters {get; set;}
        List<IPlottable> Indicators { get; set; }

        void Init();
        void UpdateParameters();

        Trade Next(OHLC ohlc, bool updateQueue = true);
        Trade Close(OHLC ohlc);
        bool BuySignal();
        bool SellSignal();
        Trade MakeTrade(OHLC ohlc, TradeSide side);

    }
}
