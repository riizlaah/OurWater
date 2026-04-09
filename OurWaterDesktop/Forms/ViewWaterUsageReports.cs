using OurWaterDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace OurWaterDesktop.Forms
{
    public partial class ViewWaterUsageReports : Form
    {
        private readonly OurWater dbc;
        private readonly Dashboard dashb;
        public ViewWaterUsageReports(OurWater ctx, Dashboard dash)
        {
            dbc = ctx;
            dashb = dash;
            InitializeComponent();
            RefreshChart();
        }

        private void RefreshChart()
        {
            var currYear = DateTime.Now.Year;
            waterUsageCharts.ChartAreas.Clear();
            waterUsageCharts.Series.Clear();
            waterUsageCharts.Titles.Clear();
            waterUsageCharts.Titles.Add($"Water Usage Year {currYear}");
            var area = new ChartArea();
            area.AxisX.Title = "Month";
            area.AxisY.Title = "Debit";
            area.AxisY.Interval = 10;
            waterUsageCharts.ChartAreas.Add(area);
            var series = waterUsageCharts.Series.Add("Consumptions Percentage");
            var series2 = waterUsageCharts.Series.Add("Productions Percentage");
            var series3 = waterUsageCharts.Series.Add("Productions Waste Percentage");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "F2";
            series2.ChartType = SeriesChartType.Column;
            series3.ChartType = SeriesChartType.Column;
            series3.IsValueShownAsLabel = true;
            series3.LabelFormat = "F2";
            var line = new StripLine();
            line.BorderColor = Color.Red;
            line.BorderWidth = 1;
            line.BorderDashStyle = ChartDashStyle.Dash;
            line.Interval = 0;
            line.IntervalOffset = 10;
            line.StripWidth = 0;
            area.AxisY.StripLines.Add(line);
            var prods = dbc.ProductionDebitRecords.Where(p => p.date.Year == currYear).GroupBy(p => p.date.Month)
                .Select(p => new { Month = p.Key, Total = p.Sum(r => r.debit) }).ToList();
            var cons = dbc.ConsumptionDebitRecords.Where(c => c.date.Year == currYear && c.status == "Verified").GroupBy(c => c.date.Month)
                .Select(c => new { Month = c.Key, Total = c.Sum(r => r.debit) }).ToList();
            var arr = new List<WaterReport>();
            for(int i = 0; i < cons.Count; i++)
            {
                arr.Add(new WaterReport { Month = cons[i].Month, TotalProdDebit = prods[i].Total, TotalConsDebit = cons[i].Total });
            }
            foreach (var col in arr)
            {
                var date = new DateTime(currYear, col.Month, 1);
                var usedWater = (col.TotalConsDebit / col.TotalProdDebit);
                var availableWater = (1m - usedWater) * 100m;
                series.Points.AddXY(date.ToString("MMM"), usedWater * 100m);
                series2.Points.AddXY(date.ToString("MMM"), 100m);
                series3.Points.AddXY(date.ToString("MMM"), availableWater);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            dashb.Show();
        }
    }
    public class WaterReport
    {
        public int Month { get; set; }
        public decimal TotalProdDebit { get; set; }
        public decimal TotalConsDebit { get; set; }
    }
}
