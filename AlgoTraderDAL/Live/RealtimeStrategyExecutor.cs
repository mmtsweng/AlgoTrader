using System;
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
        public bool isCrypto { get; set; }
        public string symbol { get; set; }

        public event EventHandler<AlgoTraderDAL.Trade> TradeOccurred;

        public RealtimeStrategyExecutor()
        {
            this.strategy = new SimpleMomentum();
        }

        public async void Start(string symbol, bool isCrypto)
        {
            this.symbol = symbol;
            this.isCrypto = isCrypto;
            this.strategy.Init();
            this.strategy = new SimpleMomentum();
        }
    }
}
