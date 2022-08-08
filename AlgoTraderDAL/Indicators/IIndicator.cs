using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTraderDAL.Types;

namespace AlgoTraderDAL.Indicators
{
    public interface IIndicator
    {
        string Name { get; }
        Queue<IndicatorNode> History { get; set; }

        void AddDataPoint(OHLC ohlc);
    }
}
