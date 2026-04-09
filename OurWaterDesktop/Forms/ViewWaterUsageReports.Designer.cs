namespace OurWaterDesktop.Forms
{
    partial class ViewWaterUsageReports
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.waterUsageCharts = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.waterUsageCharts)).BeginInit();
            this.SuspendLayout();
            // 
            // waterUsageCharts
            // 
            chartArea1.Name = "ChartArea1";
            this.waterUsageCharts.ChartAreas.Add(chartArea1);
            this.waterUsageCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.waterUsageCharts.Legends.Add(legend1);
            this.waterUsageCharts.Location = new System.Drawing.Point(12, 12);
            this.waterUsageCharts.Name = "waterUsageCharts";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.waterUsageCharts.Series.Add(series1);
            this.waterUsageCharts.Size = new System.Drawing.Size(1064, 518);
            this.waterUsageCharts.TabIndex = 0;
            this.waterUsageCharts.Text = "chart1";
            // 
            // ViewWaterUsageReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 542);
            this.Controls.Add(this.waterUsageCharts);
            this.Name = "ViewWaterUsageReports";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewWaterUsageReports";
            ((System.ComponentModel.ISupportInitialize)(this.waterUsageCharts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart waterUsageCharts;
    }
}