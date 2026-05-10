using OurWaterDesktop.Views;
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
    public partial class ViewWaterUsageForm : Form
    {
        private readonly MainForm mainForm;
        public ViewWaterUsageForm(MainForm m)
        {
            mainForm = m;
            InitializeComponent();
            yearInp.Maximum = decimal.MaxValue;
            yearInp.Value = DateTime.Today.Year;
        }

        async private void RefreshChart()
        {
            var year = yearInp.Value;
            waterUsageChart.ChartAreas.Clear();
            waterUsageChart.Series.Clear();
            waterUsageChart.Titles.Clear();
            waterUsageChart.Titles.Add($"Water Usage in {year}");
            var area = new ChartArea { AxisX = new Axis { Title = "Month" }, AxisY = new Axis { Title = "Debit Percentage", Interval = 10 } };
            waterUsageChart.ChartAreas.Add(area);
            var consPercentage = waterUsageChart.Series.Add("Consumptions Percentage");
            consPercentage.ChartType = SeriesChartType.Column;
            consPercentage.IsValueShownAsLabel = true;
            consPercentage.LabelFormat = "F2";
            var prodPercentage = waterUsageChart.Series.Add("Productions Percentage");
            prodPercentage.ChartType = SeriesChartType.Column;
            prodPercentage.LabelFormat = "F2";
            var prodWastePercentage = waterUsageChart.Series.Add("Productions Waste Percentage");
            prodWastePercentage.ChartType = SeriesChartType.Column;
            prodWastePercentage.LabelFormat = "F2";
            prodWastePercentage.IsValueShownAsLabel = true; ;
            var line = new StripLine { BorderColor = Color.Red, BorderWidth = 1, BorderDashStyle = ChartDashStyle.Dash, Interval = 0, IntervalOffset = 10, StripWidth = 0 };
            area.AxisY.StripLines.Add(line);
            var (success, result) = await Helper.JsonReq<object, List<MonthlyWaterUsage>>($"WaterUsages?year={year}");
            if(!success || result.data == null)
            {
                MessageBox.Show(result.message, "Error");
                return;
            }
            foreach(var col in result.data)
            {
                consPercentage.Points.AddXY(col.monthName, col.consDebitPercentage);
                prodPercentage.Points.AddXY(col.monthName, 100m);
                prodWastePercentage.Points.AddXY(col.monthName, col.remainingWaterPercentage);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            mainForm.Show();
        }

        private void OnYearChanged(object sender, EventArgs e)
        {
            RefreshChart();
        }
    }


    public class MonthlyWaterUsage
    {
        public string monthName { get; set; } = null!;
        public int monthNumber { get; set; }
        public float totalProdDebit { get; set; }
        public float totalConsDebit { get; set; }
        public float remainingWater { get; set; }
        public float remainingWaterPercentage { get; set; }
        public float consDebitPercentage { get; set; }
    }

}
