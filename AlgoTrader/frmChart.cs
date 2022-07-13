using ScottPlot;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoTrader
{
    public partial class frmChart : Form
    {
        public frmChart()
        {
            InitializeComponent();
        }

        public void LoadOHLC(List<AlgoTraderDAL.Types.OHLC> ohlc)
        {
            string filename = Application.StartupPath + @"chart_" + ohlc[0].Symbol;

            ScottPlot.OHLC[] prices = ohlc.Select(p => new ScottPlot.OHLC(0,0,0,0,0)
            { 
                Open = decimal.ToDouble(p.Open),
                High = decimal.ToDouble(p.High),
                Low = decimal.ToDouble(p.Low),
                Close = decimal.ToDouble(p.Close),
                Volume = decimal.ToDouble(p.Volume),
                DateTime = p.Timeframe
            }).ToArray();


            var plt = new ScottPlot.Plot(800, 600);
            var cplot = this.pltChart.Plot.AddOHLCs(prices);
            var bol = cplot.GetBollingerBands(3);
            this.pltChart.Plot.AddScatterLines(bol.xs, bol.sma, Color.DarkBlue, lineStyle: LineStyle.DashDotDot);
            this.pltChart.Plot.AddScatterLines(bol.xs, bol.lower, Color.DarkGreen, lineStyle: LineStyle.Dot);
            this.pltChart.Plot.AddScatterLines(bol.xs, bol.upper, Color.DarkOliveGreen, lineStyle: LineStyle.Dot);
            this.pltChart.Plot.XAxis.DateTimeFormat(true);
            this.pltChart.Refresh();

        }
    }
}
