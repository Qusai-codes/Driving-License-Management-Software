using Business;
using System;
using System.Data;
using System.Windows.Forms;

namespace Presentation.Forms
{
    public partial class ManageDriversForm : Form
    {
        private DataTable _allDriversData = null;

        public ManageDriversForm()
        {
            InitializeComponent();
        }

        private void ManageDriversForm_Load(object sender, EventArgs e)
        {
            SetupFilterComboBox();
            SetUpDataGridView();
            txtFilterValue.Visible = false;
        }

        private void SetupFilterComboBox()
        {
            cmbFilter.Items.Clear();
            string[] applicationsFilterOption = new string[] {
                "None", "Driver ID", "Person ID", "National No.",
                "Full Name"
            };
            cmbFilter.Items.AddRange(applicationsFilterOption);
            cmbFilter.SelectedIndex = 0;
        }

        private void SetUpDataGridView()
        {
            try
            {
                _allDriversData = GetAllDriversData();

                dgvDrivers.AutoGenerateColumns = true;
                dgvDrivers.DataSource = _allDriversData;

                FormatDataGridView();
                ApplyFilter();
                lblNumberOfRecords.Text = dgvDrivers.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading drivers: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            SetColumnHeader(dgvDrivers, "DriverID", "Driver ID");
            SetColumnHeader(dgvDrivers, "PersonID", "Person ID");
            SetColumnHeader(dgvDrivers, "NationalNo", "National No.");
            SetColumnHeader(dgvDrivers, "FullName", "Full Name");
            SetColumnHeader(dgvDrivers, "CreatedDate", "Date");
            SetColumnHeader(dgvDrivers, "ActiveLicenses", "Active Licenses");
        }

        private void SetColumnHeader(DataGridView dgv, string columnName, string headerText)
        {
            if (dgv.Columns[columnName] != null)
            {
                dgv.Columns[columnName].HeaderText = headerText;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbFilter.SelectedItem as string;

            if (string.IsNullOrEmpty(selected) || selected == "None")
            {
                txtFilterValue.Visible = false;
                txtFilterValue.Clear();
            }
            else
            {
                txtFilterValue.Visible = true;
                txtFilterValue.Clear();
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allDriversData == null)
                return;

            DataView dv = _allDriversData.DefaultView;

            string selected = cmbFilter.SelectedItem as string;
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(selected) || selected == "None")
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            if (string.IsNullOrEmpty(filterText))
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            string column = GetColumnNameFromDisplayName(selected);

            if (string.IsNullOrEmpty(column) || !_allDriversData.Columns.Contains(column))
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            Type colType = _allDriversData.Columns[column].DataType;

            if (colType == typeof(string))
            {
                dv.RowFilter = string.Format("{0} LIKE '%{1}%'", column, filterText.Replace("'", "''"));
            }
            else if (colType == typeof(int))
            {
                int num;
                dv.RowFilter = int.TryParse(filterText, out num)
                    ? string.Format("{0} = {1}", column, num)
                    : "1=0";
            }
            else
            {
                dv.RowFilter = "";
            }

            lblNumberOfRecords.Text = dv.Count.ToString();
        }

        private string GetColumnNameFromDisplayName(string displayName)
        {
            switch (displayName)
            {
                case "Driver ID":
                    return "DriverID";
                case "Person ID":
                    return "PersonID";
                case "National No.":
                    return "NationalNo";
                case "Full Name":
                    return "FullName";
                default:
                    return null;
            }
        }

        private DataTable GetAllDriversData()
        {
            return Driver.GetAllDrivers();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsNumericFilterSelected())
                return;

            bool isControl = char.IsControl(e.KeyChar);
            bool isDigit = char.IsDigit(e.KeyChar);

            e.Handled = !(isControl || isDigit);
        }

        private bool IsNumericFilterSelected()
        {
            string selected = cmbFilter.SelectedItem as string;
            return selected == "Driver ID" || selected == "Person ID";
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvDrivers.CurrentRow.Cells["PersonID"].Value;
            PersonDetailsForm frm = new PersonDetailsForm(PersonID);
            frm.ShowDialog();
            SetUpDataGridView();
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet.");
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDrivers.CurrentRow == null)
                return;

            int driverId = (int)dgvDrivers.CurrentRow.Cells["DriverID"].Value;

            DriverLicensesHistoryForm form = new DriverLicensesHistoryForm(driverId);
            form.ShowDialog();
            SetUpDataGridView();
        }
    }
}