using OurWaterDesktop.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Forms
{
    public partial class ManageFinesForm : Form
    {
        bool editMode = false;
        private readonly MainForm main;
        public ManageFinesForm(MainForm m)
        {
            main = m;
            InitializeComponent();
            Helper.GenerateTableColumns(table1, new[] { "Id", "Start Day", "End Day", "Amount per Day" }, new[] { "Id", "StartDay", "EndDay", "amount" });
            RefreshData();
            ToggleInput(false);
        }

        protected override void OnClosed(EventArgs e)
        {
            main.Show();
        }

        async public Task RefreshData(string searchStr = "")
        {
            var (success, result) = await Helper.JsonReq<object, List<FineRes>>("FineRules");
            if (!success)
            {
                MessageBox.Show(result.message, "Error");
            }
            table1.DataSource = result.data;
        }

        private void ToggleInput(bool enabled, bool clear = false)
        {
            insertBtn.Visible = !enabled;
            editBtn.Visible = !enabled;
            deleteBtn.Visible = !enabled;
            table1.Enabled = !enabled;

            startDay.ReadOnly = !enabled;
            endDay.ReadOnly = !enabled;
            defined.Enabled = enabled;
            continuous.Enabled = enabled;
            amount.ReadOnly = !enabled;

            if (clear)
            {
                startDay.Text = "";
                endDay.Text = "";
                defined.Checked = true;
                amount.Text = "";
            }

            cancelBtn.Visible = enabled;
            saveBtn.Visible = enabled;
        }

        private FineRes? GetSelected()
        {
            if (table1.SelectedCells.Count == 0) return null;
            return table1.SelectedCells[0].OwningRow.DataBoundItem as FineRes;
        }

        private void OnInsert(object sender, EventArgs e)
        {
            editMode = false;
            ToggleInput(true, true);
        }

        private void OnEdit(object sender, EventArgs e)
        {
            editMode = true;
            ToggleInput(true);
        }

        private void OnDelete(object sender, EventArgs e)
        {
            var item = GetSelected();
            if (item == null)
            {
                MessageBox.Show("Please select one row");
                return;
            }
            if (MessageBox.Show($"Are you sure want to delete '{item.startDay} - {item.endDay?.ToString() ?? "..."}'?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Delete(item.id);
            }
        }


        private void OnCancel(object sender, EventArgs e)
        {
            ToggleInput(false, true);
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (startDay.Text.Trim() == "")
            {
                MessageBox.Show("Start Day is required");
                return;
            }
            if (defined.Checked)
            {
                if (endDay.Text.Trim() == "")
                {
                    MessageBox.Show("End Day is required");
                    return;
                }
            }
            if (!decimal.TryParse(amount.Text, out decimal actualAmount))
            {
                MessageBox.Show("Amount not valid");
                return;
            }
            if (editMode)
            {
                var row = GetSelected();
                if (row == null) return;
                Update(row.id);
            }
            else
            {
                Create();
            }
        }


        async private Task Create()
        {
            decimal.TryParse(amount.Text, out decimal actualAmount);
            var (success, result) = await Helper.JsonReq<FineReq, object>($"finerules", new FineReq
            {
                startDay = (int)startDay.Value,
                endDay = defined.Checked ? (int)endDay.Value : null,
                amount = actualAmount
            }, "post");
            if (!success)
            {
                MessageBox.Show(result.message, "Error");
                return;
            }
            ToggleInput(false, true);
            RefreshData();
        }

        async private Task Update(int id)
        {
            decimal.TryParse(amount.Text, out decimal actualAmount);
            var (success, result) = await Helper.JsonReq<FineReq, object>($"finerules/{id}", new FineReq
            {
                startDay = (int)startDay.Value,
                endDay = defined.Checked ? (int)endDay.Value : null,
                amount = actualAmount
            }, "put");
            if (!success)
            {
                MessageBox.Show(result.message, "Error");
                return;
            }
            ToggleInput(false, true);
            RefreshData();
        }

        async private Task Delete(int id)
        {
            var (success, result) = await Helper.JsonReq<object, object>($"finerules/{id}", method: "delete");
            if (!success)
            {
                MessageBox.Show(result.message, "Error");
                return;
            }
            ToggleInput(false, true);
            RefreshData();
        }

        private void OnCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            var row = GetSelected();
            if (row == null) return;
            startDay.Value = row.startDay;
            if (row.endDay.HasValue)
            {
                defined.Checked = true;
                endDay.Value = row.endDay.Value;
            }
            else
            {
                endDay.Text = "";
                continuous.Checked = true;
            }
            amount.Text = row.amount.ToString("F2");
        }

        private void OnCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (e.Value == null)
                {
                    e.Value = "Continuous";
                    e.FormattingApplied = true;
                }
            }
            if (e.ColumnIndex == 3)
            {
                var dec = Convert.ToDecimal(e.Value);
                e.Value = dec.ToString("Rp#,##0;(Rp#,##0);Rp0");
                e.FormattingApplied = true;
            }
        }

        private void OnDefinedCheckStateChanged(object sender, EventArgs e)
        {
            endDay.ReadOnly = !defined.Checked;
        }
    }


    public class FineRes
    {
        public int id { get; set; }
        public int startDay { get; set; }
        public int? endDay { get; set; }
        public decimal amount { get; set; }
    }

    public class FineReq
    {
        public int startDay { get; set; }
        public int? endDay { get; set; }
        public decimal amount { get; set; }
    }

}
