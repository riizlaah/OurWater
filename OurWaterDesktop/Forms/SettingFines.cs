using OurWaterDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Forms
{
    public partial class SettingFines : Form
    {
        private readonly OurWater dbc;
        private readonly Dashboard dashb;
        bool editing = false;
        public SettingFines(OurWater ctx, Dashboard dash)
        {
            dbc = ctx;
            dashb = dash;
            InitializeComponent();
            Helper.BindTableColumns(table1, new[] { "Id", "Day After Deadline", "Fine Amount" }, new[] { "id", "dayAfterDeadline", "fineAmount" });
            RefreshData();
            enableInputs(false);
        }

        private void RefreshData(string search = "")
        {
            table1.DataSource = dbc.FineRules.ToList();
        }

        private void onCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            var fineRule = GetSelected();
            if (fineRule == null) return;
            dayAfterDeadline.Text = fineRule.dayAfterDeadline.ToString();
            fineAmount.Text = fineRule.fineAmount.ToString();
            edit.Show();
            delete.Show();
        }

        private FineRule GetSelected()
        {
            if (table1.SelectedRows.Count == 0) return null;
            return table1.SelectedRows[0].DataBoundItem as FineRule;
        }

        private void onCreate(object sender, EventArgs e)
        {
            enableInputs(true);
        }

        private void enableInputs(bool enabled)
        {
            dayAfterDeadline.ReadOnly = !enabled;
            fineAmount.ReadOnly = !enabled;

            save.Visible = enabled;
            cancel.Visible = enabled;
            table1.Enabled = !enabled;

            edit.Visible = !enabled;
            delete.Visible = !enabled;
        }

        private void onCancel(object sender, EventArgs e)
        {
            enableInputs(false);
        }

        private void onSave(object sender, EventArgs e)
        {
            if(!int.TryParse(dayAfterDeadline.Text, out int dayAfterD))
            {
                MessageBox.Show("Day after deadline not valid");
                return;
            }
            if (!decimal.TryParse(fineAmount.Text, out decimal fAmount))
            {
                MessageBox.Show("Fine Amount not valid");
                return;
            }
            if (editing)
            {
                var fr2 = GetSelected();
                if (fr2 == null) return;
                var fr = dbc.FineRules.Find(fr2.id);
                fr.dayAfterDeadline = dayAfterD;
                fr.fineAmount = fAmount;
                dbc.SaveChanges();
            } else
            {
                dbc.FineRules.Add(new FineRule { dayAfterDeadline = dayAfterD, fineAmount = fAmount });
                dbc.SaveChanges();
            }
            enableInputs(false);
            RefreshData();
            editing = false;
        }

        private void onDelete(object sender, EventArgs e)
        {
            var data = GetSelected();
            if (data == null) return;
            if(MessageBox.Show($"Are you sure want to delete '{data.dayAfterDeadline} day'?", "Confirmation", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                dbc.FineRules.Remove(data);
                dbc.SaveChanges();
                RefreshData();
            }
        }

        private void onEdit(object sender, EventArgs e)
        {
            var user = GetSelected();
            if (user == null) return;
            editing = true;
            enableInputs(true);
        }

        protected override void OnClosed(EventArgs e)
        {
            dashb.Show();
        }
    }
}
