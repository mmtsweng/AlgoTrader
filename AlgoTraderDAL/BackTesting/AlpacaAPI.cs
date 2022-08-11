using System;
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
        private async Task<List<OHLC>> Get_TickerDataAsync(string ticker, DateTime from, DateTime to, OHLC_TIMESPAN ticks)
        {
            List<OHLC> result = new List<OHLC>();

            //Standare Equity Call
            BarTimeFrame btf = ConvertOHLCToBar(ticks);

            if ((bool)this.setting.PAPER_TRADING)
            {
                var client = Environments.Paper.GetAlpacaDataClient(new SecretKey(this.setting.API_KEY, this.setting.API_SECRET));
                try
                {
                    var req = new HistoricalBarsRequest(ticker, DateTime.SpecifyKind(from, DateTimeKind.Utc), DateTime.SpecifyKind(to, DateTimeKind.Utc), btf).WithPageSize(9000);
                    var page = await client.ListHistoricalBarsAsync(req).ConfigureAwait(false);
                    foreach (var bar in page.Items)
                    {
                        OHLC ohlc = new OHLC();
                        ohlc.parseIBar(bar);
                        ohlc.ticks = ticks;
                        result.Add(ohlc);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Instance.LogException("AlpacaAPI.Get_TickerDataAsync()", ex);
                }
            }
            

            return result;
        }

        private async Task<List<OHLC>> Get_CryptoTickerDataAsync(string ticker, DateTime from, DateTime to, OHLC_TIMESPAN ticks)
        {
            BarTimeFrame btf = ConvertOHLCToBar(ticks);

            List<OHLC> result = new List<OHLC>();
            if ((bool)this.setting.PAPER_TRADING)
            {
                var client = Environments.Paper.GetAlpacaCryptoDataClient(new SecretKey(this.setting.API_KEY, this.setting.API_SECRET));
                try
                {
                    var req = new HistoricalCryptoBarsRequest(ticker, DateTime.SpecifyKind(from, DateTimeKind.Utc), DateTime.SpecifyKind(to, DateTimeKind.Utc), btf).WithPageSize(9000).WithExchanges(CryptoExchange.Ftx);
                    var page = await client.ListHistoricalBarsAsync(req).ConfigureAwait(false);
                    foreach (var bar in page.Items)
                    {
                        OHLC ohlc = new OHLC();
                        ohlc.parseIBar(bar);
                        ohlc.ticks = ticks;
                        result.Add(ohlc);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.Instance.LogException("AlpacaAPI.Get_CryptoTickerDataAsync()", ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Public Method to retrive Ticker Data from the local database if it exists
        ///     If not, query Alpaca for the data
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<OHLC> Get_TickerData(string ticker, DateTime from, DateTime to, OHLC_TIMESPAN ticks) 
        {
            List<OHLC> result = new List<OHLC>();
            bool hasDBData = false;

            using (Entities entities = new Entities())
            {
                var logs = entities.HistoricalLogs.Where(
                        lg => lg.DataDate == from.Date
                        && lg.TickResolution == (int)ticks).Count();
                if (logs > 0)
                {
                    hasDBData = true;
                    result = entities.HistoricalOHLCs.Where(
                        x => x.Timeframe >= from
                        && x.Timeframe <= to
                        && x.Timespan == (int)ticks
                        ).Select(
                            o => new OHLC()
                            {
                                Symbol = o.Symbol,
                                Open = (decimal)o.Open,
                                High = (decimal)o.High,
                                Low= (decimal)o.Low,
                                Close = (decimal)o.Close,
                                Volume = (decimal)o.Volume,
                                Timeframe = (DateTime)o.Timeframe,
                                ticks = (OHLC_TIMESPAN)o.Timespan
                            }).ToList();
                }                
            }

            //If we didn't get database data, query API for the data
            if (!hasDBData)
            {
                if (ticker.Contains(":"))
                {
                    //Crypto Call
                    result = Get_CryptoTickerDataAsync(ticker.Substring(ticker.IndexOf(':') + 1), from, to, ticks).GetAwaiter().GetResult();
                }
                else
                {
                    result = Get_TickerDataAsync(ticker, from, to, ticks).GetAwaiter().GetResult();
                }
                SaveBackfill(ticker, (int)ticks, from, to, result);
            }
            return result;
        }

        /// <summary>
        /// Helper method to convert OHLC_TIMESPAN objects to Alpaca Specific BarTimeframes
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        private static BarTimeFrame ConvertOHLCToBar(OHLC_TIMESPAN ticks)
        {
            BarTimeFrame btf = BarTimeFrame.Day;
            switch (ticks)
            {
                case OHLC_TIMESPAN.HOUR:
                    btf = BarTimeFrame.Hour;
                    break;
                case OHLC_TIMESPAN.MINUTE:
                    btf = BarTimeFrame.Minute;
                    break;
                case OHLC_TIMESPAN.DAY:
                    btf = BarTimeFrame.Day;
                    break;
                default:
                    btf = BarTimeFrame.Day;
                    break;
            }
            return btf;
        }

        /// <summary>
        /// Method to store the historical OHLC data for a particular date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ohlcData"></param>
        public void SaveBackfill(string symbol, int tickResolution, DateTime fromDate, DateTime toDate, List<OHLC> ohlcData)
        {
            using (Entities entities = new Entities())
            {

                //Store the log dates and ticks
                List<DateTime?> dates = GetDatesFromRange(fromDate, toDate);
                foreach (DateTime date in dates)
                {
                    if (entities.HistoricalLogs.Where(
                        x => x.DataDate >= fromDate
                        && x.DataDate <= toDate
                        && x.TickResolution == tickResolution).Count() == 0)
                    {
                        HistoricalLog log = new HistoricalLog()
                        {
                            DataDate = date.Date,
                            Symbol = symbol,
                            TickResolution = tickResolution
                        };
                        entities.HistoricalLogs.Add(log);
                    }
                }              

                //Add new items
                if (ohlcData.Count > 0)
                {
                    List<HistoricalOHLC> ohlc = ohlcData.Select(
                    x => new HistoricalOHLC()
                    {
                        Symbol = symbol,
                        Open = (decimal?)x.Open,
                        Close = (decimal?)x.Close,
                        High = (decimal?)x.High,
                        Low = (decimal?)x.Low,
                        Volume = (long?)x.Volume,
                        Timeframe = (DateTime?)x.Timeframe,
                        Timespan = tickResolution

                    }).ToList();
                    entities.HistoricalOHLCs.AddRange(ohlc);
                }
                entities.SaveChanges();
            }

            return;
        }

        /// <summary>
        /// Clear out old data in preparation for a backfill
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="ticks"></param>
        public void RemoveExistingHistoricalOHLCs(DateTime fromDate, DateTime toDate, OHLC_TIMESPAN ticks)
        {

            Entities entities = new Entities();

            //Remove previously saved OHLC Items
            DateTime from = fromDate.Date;
            DateTime to = toDate.Date;
            var remove = entities.HistoricalOHLCs
                .Where(r =>
                    r.Timeframe >= fromDate
                    && r.Timeframe <= toDate
                    && r.Timespan == (int)ticks).ToList();
            if (remove.Count > 0)
            {
                entities.HistoricalOHLCs.RemoveRange(remove);
            }

            //Remove previously saved Log Entries
            var removelog = entities.HistoricalLogs
                .Where(l =>
                     l.DataDate >= fromDate
                    && l.DataDate <= toDate
                    && l.TickResolution == (int)ticks
                ).ToList();
            if (removelog.Count > 0)
            {
                entities.HistoricalLogs.RemoveRange(removelog);
            }

            entities.SaveChanges();
        }

        /// <summary>
        /// Helper method to get all dates between a start and end date to iterate through
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <returns></returns>
        public List<DateTime?> GetDatesFromRange(DateTime dtFrom, DateTime dtTo)
        {
            DateTime startDate = dtFrom.Date;
            DateTime endDate = dtTo.Date;
            List<DateTime?> dates = Enumerable
                .Range(0, int.MaxValue)
                .Select(index => new DateTime?(startDate.AddDays(index)))
                .TakeWhile(date => date <= endDate)
                .ToList();

            return dates;
        }

    }
}
