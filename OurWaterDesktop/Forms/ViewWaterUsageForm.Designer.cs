namespace OurWaterDesktop.Forms
{
    partial class ViewWaterUsageForm
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
            panel1 = new Panel();
            waterUsageChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            panel2 = new Panel();
            yearInp = new NumericUpDown();
            label1 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)waterUsageChart).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)yearInp).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(waterUsageChart);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(12);
            panel1.Size = new Size(800, 450);
            panel1.TabIndex = 1;
            // 
            // waterUsageChart
            // 
            chartArea1.Name = "ChartArea1";
            waterUsageChart.ChartAreas.Add(chartArea1);
            waterUsageChart.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            waterUsageChart.Legends.Add(legend1);
            waterUsageChart.Location = new Point(12, 63);
            waterUsageChart.Name = "waterUsageChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            waterUsageChart.Series.Add(series1);
            waterUsageChart.Size = new Size(776, 375);
            waterUsageChart.TabIndex = 1;
            waterUsageChart.Text = "chart1";
            // 
            // panel2
            // 
            panel2.Controls.Add(yearInp);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(12, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(776, 51);
            panel2.TabIndex = 0;
            // 
            // yearInp
            // 
            yearInp.Location = new Point(75, 13);
            yearInp.Name = "yearInp";
            yearInp.Size = new Size(150, 27);
            yearInp.TabIndex = 1;
            yearInp.ValueChanged += OnYearChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 15);
            label1.Name = "label1";
            label1.Size = new Size(44, 20);
            label1.TabIndex = 0;
            label1.Text = "Year :";
            // 
            // ViewWaterUsageForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Name = "ViewWaterUsageForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "View Water Usage";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)waterUsageChart).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)yearInp).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart waterUsageChart;
        private Panel panel2;
        private NumericUpDown yearInp;
        private Label label1;
    }
}