﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlgoTraderDAL.Types;
using AlgoTraderDAL.BackTesting;

namespace AlgoTrader
{
    public partial class frmAnalytics : Form
    {
        public frmAnalytics()
        {
            InitializeComponent();
        }

        public void ProcessAnalytics(Analytics analytics, ref BackTester backtest)
        {
            this.txtStartDate.Text = analytics.startDateTime.ToShortDateString();
            this.txtEndDate.Text = analytics.endDateTime.ToShortDateString();
            this.txtNumTrades.Text = analytics.numberOfTrades.ToString();

            this.txtWins.Text = analytics.wins.ToString();
            this.txtWinPercent.Text = Math.Round(analytics.winPercent, 1).ToString();
            this.txtMaxWins.Text = analytics.maxConsecutiveWins.ToString();
            this.txtLosses.Text = analytics.losses.ToString();
            this.txtLossPercent.Text = Math.Round(analytics.lossPercent, 1).ToString();
            this.txtMaxLoss.Text = analytics.maxConsecutiveLosses.ToString();

            this.txtNetProfitPercent.Text = Math.Round(analytics.netProfitPercent,1).ToString();
            this.txtNetProfit.Text = analytics.netProfit.ToString();
            this.txtInitCapital.Text = analytics.initialCapital.ToString();
            this.txtFinalCapital.Text = analytics.finalCapital.ToString();
            this.txtMinCapital.Text = analytics.minCapital.ToString();
            this.txtMaxCapital.Text = analytics.maxCapital.ToString();
            this.gpTrade.Text = "Trade Information -- " + analytics.symbol;

            if (analytics.netProfit < 0)
            {
                this.txtNetProfit.BackColor = Color.LightPink;
                this.txtNetProfitPercent.BackColor = Color.LightPink;
            }
            else
            {
                this.txtNetProfit.BackColor = Color.LightGreen;
                this.txtNetProfitPercent.BackColor = Color.LightGreen;
            }
            RefreshChartData(ref analytics, ref backtest);
            this.Focus();
        }

        public void RefreshChartData(ref Analytics analytics, ref BackTester backtest)
        {

            ScottPlot.OHLC[] prices = backtest.historicalOHLC.Select(p => new ScottPlot.OHLC(0, 0, 0, 0, 0)
            {
                Open = decimal.ToDouble(p.Open),
                High = decimal.ToDouble(p.High),
                Low = decimal.ToDouble(p.Low),
                Close = decimal.ToDouble(p.Close),
                Volume = decimal.ToDouble(p.Volume),
                DateTime = p.Timeframe
            }).ToArray();

            double[] buytimes = backtest.portfolio.trades.Where(x => x.side == AlgoTraderDAL.TradeSide.BUY).Select(x => x.transactionDateTime.ToOADate()).ToArray();
            double[] buys = backtest.portfolio.trades.Where(x => x.side == AlgoTraderDAL.TradeSide.BUY).Select(x => (double)x.actualPrice).ToArray();
            double[] losingtimes = backtest.analytics.losingTrades.Select(x => x.transactionDateTime.ToOADate()).ToArray();
            double[] losingsells = backtest.analytics.losingTrades.Select(x => (double)x.actualPrice).ToArray();
            double[] winningtimes = backtest.analytics.winningTrades.Select(x => x.transactionDateTime.ToOADate()).ToArray();
            double[] winningsells = backtest.analytics.winningTrades.Select(x => (double)x.actualPrice).ToArray();

            var plt = new ScottPlot.Plot(800, 600);
            if (!backtest.strategy.isIntraday)
            {
                var cplot = this.pltResults.Plot.AddOHLCs(prices);
                cplot.ColorUp = Color.LightGreen;
                cplot.ColorDown = Color.LightPink;
                cplot.WickColor = Color.DarkGray;
            }
            this.pltResults.Plot.AddScatterPoints(buytimes, buys, Color.DarkBlue,markerShape: ScottPlot.MarkerShape.filledCircle, markerSize:10);
            this.pltResults.Plot.AddScatterPoints(winningtimes, winningsells, Color.DarkGreen, markerShape: ScottPlot.MarkerShape.filledTriangleUp, markerSize: 10);
            this.pltResults.Plot.AddScatterPoints(losingtimes, losingsells, Color.DarkRed, markerShape: ScottPlot.MarkerShape.filledTriangleDown, markerSize: 10);

            this.pltResults.Plot.XAxis.DateTimeFormat(true);
            this.pltResults.Refresh();

        }
    }
}
