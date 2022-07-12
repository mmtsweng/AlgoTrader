using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTraderDAL
{
       
    public class Equity
    {
        public AMC AMC { get; set; }
    }

    public class AMC
    {
        public string assetType { get; set; }
        public string assetMainType { get; set; }
        public string cusip { get; set; }
        public string symbol { get; set; }
        public string description { get; set; }
        public double bidPrice { get; set; }
        public int bidSize { get; set; }
        public string bidId { get; set; }
        public double askPrice { get; set; }
        public int askSize { get; set; }
        public string askId { get; set; }
        public double lastPrice { get; set; }
        public int lastSize { get; set; }
        public string lastId { get; set; }
        public double openPrice { get; set; }
        public double highPrice { get; set; }
        public double lowPrice { get; set; }
        public string bidTick { get; set; }
        public double closePrice { get; set; }
        public double netChange { get; set; }
        public int totalVolume { get; set; }
        public long quoteTimeInLong { get; set; }
        public long tradeTimeInLong { get; set; }
        public double mark { get; set; }
        public string exchange { get; set; }
        public string exchangeName { get; set; }
        public bool marginable { get; set; }
        public bool shortable { get; set; }
        public double volatility { get; set; }
        public int digits { get; set; }
        public double _52WkHigh { get; set; }
        public double _52WkLow { get; set; }
        public double nAV { get; set; }
        public double peRatio { get; set; }
        public double divAmount { get; set; }
        public double divYield { get; set; }
        public string divDate { get; set; }
        public string securityStatus { get; set; }
        public double regularMarketLastPrice { get; set; }
        public int regularMarketLastSize { get; set; }
        public double regularMarketNetChange { get; set; }
        public long regularMarketTradeTimeInLong { get; set; }
        public double netPercentChangeInDouble { get; set; }
        public double markChangeInDouble { get; set; }
        public double markPercentChangeInDouble { get; set; }
        public double regularMarketPercentChangeInDouble { get; set; }
        public bool delayed { get; set; }
        public bool realtimeEntitled { get; set; }
    }
}
