﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alpaca.Markets;
using Alpaca.Markets.Extensions;
using AlgoTraderDAL.Types;


namespace AlgoTraderDAL
{
    public sealed class AlpacaAPI
    {
        private static readonly Lazy<AlpacaAPI> lazy = new Lazy<AlpacaAPI>(() => new AlpacaAPI());
        private readonly AlpacaSetting setting;
        public static AlpacaAPI Instance { get { return lazy.Value; } }

        /// <summary>
        /// Constructor
        /// </summary>
        private AlpacaAPI()
        {
            //Get Keys and properties from database
            Entities entities = new Entities();
            this.setting = entities.AlpacaSettings.FirstOrDefault();
        }

        /// <summary>
        /// Asynchronous Method to retrieve a series of ticker bars (OHLC format)
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private async Task<List<OHLC>> Get_TickerDataAsync(string ticker, DateTime from, DateTime to)
        {
            List<OHLC> result = new List<OHLC>();
            if ((bool)this.setting.PAPER_TRADING)
            {
                var client = Environments.Paper.GetAlpacaDataClient(new SecretKey(this.setting.API_KEY, this.setting.API_SECRET));
                try
                {
                    var page = await client.ListHistoricalBarsAsync(new HistoricalBarsRequest(ticker, from, to, BarTimeFrame.Day)).ConfigureAwait(false);
                    foreach(var bar in page.Items)
                    {
                        OHLC ohlc = new OHLC();
                        ohlc.parseIBar(bar);
                        result.Add(ohlc);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Public Method to retrive Ticker Data from Alpaca
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<OHLC> Get_TickerData(string ticker, DateTime from, DateTime to) 
        {
            List<OHLC> result = new List<OHLC>();
            result = Get_TickerDataAsync(ticker, from, to).GetAwaiter().GetResult();
            return result;
        }
    }
}