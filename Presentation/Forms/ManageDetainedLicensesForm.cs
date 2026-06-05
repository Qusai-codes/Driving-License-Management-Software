using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Business;

namespace Presentation.Forms
{
    public partial class ManageDetainedLicensesForm : Form
    {
        private DataTable _allDetainedLicenses = null;

        public ManageDetainedLicensesForm()
        {
            InitializeComponent();
        }

        private void ManageDetainedLicensesForm_Load(object sender, EventArgs e)
        {
            SetupFilterComboBox();
            SetupIsReleasedComboBox();

            SetUpDataGridView();
            txtFilterValue.Visible = false;
            cmbIsLicenseReleased.Visible = false;
        }

        private void SetupFilterComboBox()
        {
            cmbFilter.Items.Clear();
            string[] filterOptions = new string[] {
                "None", "Detain ID", "Is Released", "National No.",
                "Full Name", "Release Application ID"
            };
            cmbFilter.Items.AddRange(filterOptions);
            cmbFilter.SelectedIndex = 0;
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void SetupIsReleasedComboBox()
        {
            cmbIsLicenseReleased.Items.Clear();
            cmbIsLicenseReleased.Items.AddRange(new[] { "Yes", "No" });
            cmbIsLicenseReleased.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIsLicenseReleased.SelectedIndex = 0;
            cmbIsLicenseReleased.SelectedIndexChanged += cmbIsLicenseReleased_SelectedIndexChanged;
        }

        private void SetUpDataGridView()
        {
            try
            {
                _allDetainedLicenses = GetAllDetainedLicenses();

                dgvDetainedLicenses.AutoGenerateColumns = true;
                dgvDetainedLicenses.DataSource = _allDetainedLicenses;

                FormatDataGridView();
                ApplyFilter();
                lblNumberOfRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading detained licenses: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetAllDetainedLicenses()
        {
            return DetainedLicense.GetAllDetainedLicenses();
        }

        private void ApplyFilter()
        {
            if (_allDetainedLicenses == null)
                return;

            DataView dv = _allDetainedLicenses.DefaultView;
            string selected = cmbFilter.SelectedItem as string;

            if (string.IsNullOrEmpty(selected) || selected == "None")
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            if (selected == "Is Released")
            {
                if (cmbIsLicenseReleased.SelectedItem == null)
                {
                    dv.RowFilter = "";
                }
                else
                {
                    bool isReleased = cmbIsLicenseReleased.SelectedItem.ToString() == "Yes";
                    dv.RowFilter = string.Format("IsReleased = {0}", isReleased ? "true" : "false");
                }

                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            string filterText = txtFilterValue.Text.Trim();
            if (string.IsNullOrEmpty(filterText))
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            string column = GetColumnNameFromDisplayName(selected);
            if (string.IsNullOrEmpty(column) || !_allDetainedLicenses.Columns.Contains(column))
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            Type colType = _allDetainedLicenses.Columns[column].DataType;

            if (colType == typeof(string))
            {
                dv.RowFilter = string.Format("{0} LIKE '%{1}%'", column, filterText.Replace("'", "''"));
            }
            else if (colType == typeof(int))
            {
                int number;
                dv.RowFilter = int.TryParse(filterText, out number)
                    ? string.Format("{0} = {1}", column, number)
                    : "1=0";
            }
            else if (colType == typeof(decimal))
            {
                decimal number;
                dv.RowFilter = decimal.TryParse(filterText, out number)
                    ? string.Format("{0} = {1}", column, number.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    : "1=0";
            }
            else if (colType == typeof(DateTime))
            {
                dv.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '%{1}%'", column, filterText.Replace("'", "''"));
            }
            else
            {
                dv.RowFilter = "";
            }

            lblNumberOfRecords.Text = dv.Count.ToString();
        }

        private void FormatDataGridView()
        {
            // Set headers
            SetColumnHeader(dgvDetainedLicenses, "DetainID", "Detain ID");
            SetColumnHeader(dgvDetainedLicenses, "LicenseID", "License ID");
            SetColumnHeader(dgvDetainedLicenses, "DetainDate", "Detain Date");
            SetColumnHeader(dgvDetainedLicenses, "IsReleased", "Is Released");
            SetColumnHeader(dgvDetainedLicenses, "FineFees", "Fine Fees");
            SetColumnHeader(dgvDetainedLicenses, "ReleaseDate", "Release Date");
            SetColumnHeader(dgvDetainedLicenses, "NationalNo", "National No.");
            SetColumnHeader(dgvDetainedLicenses, "FullName", "Full Name");
            SetColumnHeader(dgvDetainedLicenses, "ReleaseApplicationID", "Release Application ID");

            // Set display order
            dgvDetainedLicenses.Columns["DetainID"].DisplayIndex = 0;
            dgvDetainedLicenses.Columns["LicenseID"].DisplayIndex = 1;
            dgvDetainedLicenses.Columns["DetainDate"].DisplayIndex = 2;
            dgvDetainedLicenses.Columns["IsReleased"].DisplayIndex = 3;
            dgvDetainedLicenses.Columns["FineFees"].DisplayIndex = 4;
            dgvDetainedLicenses.Columns["ReleaseDate"].DisplayIndex = 5;
            dgvDetainedLicenses.Columns["NationalNo"].DisplayIndex = 6;
            dgvDetainedLicenses.Columns["FullName"].DisplayIndex = 7;
            dgvDetainedLicenses.Columns["ReleaseApplicationID"].DisplayIndex = 8;
        }

        private void SetColumnHeader(DataGridView dgv, string columnName, string headerText)
        {
            if (dgv.Columns[columnName] != null)
            {
                dgv.Columns[columnName].HeaderText = headerText;
            }
        }

        private string GetColumnNameFromDisplayName(string displayName)
        {
            switch (displayName)
            {
                case "Detain ID":
                    return "DetainID";
                case "Is Released":
                    return "IsReleased";
                case "National No.":
                    return "NationalNo";
                case "Full Name":
                    return "FullName";
                case "Release Application ID":
                    return "ReleaseApplicationID";
                default:
                    return null;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbFilter.SelectedItem as string;

            if (string.IsNullOrEmpty(selected) || selected == "None")
            {
                txtFilterValue.Visible = false;
                txtFilterValue.Clear();
                cmbIsLicenseReleased.Visible = false;
            }
            else if (selected == "Is Released")
            {
                txtFilterValue.Visible = false;
                txtFilterValue.Clear();
                cmbIsLicenseReleased.Visible = true;
                cmbIsLicenseReleased.SelectedIndex = 0;
            }
            else
            {
                cmbIsLicenseReleased.Visible = false;
                txtFilterValue.Visible = true;
                txtFilterValue.Clear();
            }

            ApplyFilter();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void cmbIsLicenseReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDetainedLicenses.CurrentRow == null)
                return;

            string nationalNo = dgvDetainedLicenses.CurrentRow.Cells["NationalNo"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(nationalNo))
                return;

            Person person = Person.Find(nationalNo);
            if (person == null)
            {
                MessageBox.Show("Person not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PersonDetailsForm form = new PersonDetailsForm(person.PersonId);
            form.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDetainedLicenses.CurrentRow == null)
                return;

            int licenseId = (int)dgvDetainedLicenses.CurrentRow.Cells["LicenseID"].Value;
            DriverLicenseInformationForm form = new DriverLicenseInformationForm(licenseId);
            form.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDetainedLicenses.CurrentRow == null)
                return;

            int licenseId = (int)dgvDetainedLicenses.CurrentRow.Cells["LicenseID"].Value;
            int driverId = Business.License.GetDriverIdByLicenseId(licenseId);

            if (driverId == -1)
            {
                MessageBox.Show("Driver not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DriverLicensesHistoryForm form = new DriverLicensesHistoryForm(driverId);
            form.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDetainedLicenses.CurrentRow == null)
                return;

            int licenseId = (int)dgvDetainedLicenses.CurrentRow.Cells["LicenseID"].Value;

            ReleaseDetainedLicenseForm form = new ReleaseDetainedLicenseForm(licenseId);
            form.ShowDialog();
            SetUpDataGridView();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvDetainedLicenses.CurrentRow == null)
            {
                showPersonDetailsToolStripMenuItem.Enabled = false;
                showLicenseDetailsToolStripMenuItem.Enabled = false;
                showPersonLicenseHistoryToolStripMenuItem.Enabled = false;
                releaseDetainedLicenseToolStripMenuItem.Enabled = false;
                return;
            }

            showPersonDetailsToolStripMenuItem.Enabled = true;
            showLicenseDetailsToolStripMenuItem.Enabled = true;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = true;

            int licenseId = (int)dgvDetainedLicenses.CurrentRow.Cells["LicenseID"].Value;

            bool isReleased = false;
            object releasedObj = dgvDetainedLicenses.CurrentRow.Cells["IsReleased"].Value;
            if (releasedObj != null && releasedObj != DBNull.Value)
                bool.TryParse(releasedObj.ToString(), out isReleased);

            bool isCurrentlyDetained = DetainedLicense.IsLicenseDetained(licenseId);

            releaseDetainedLicenseToolStripMenuItem.Enabled = isCurrentlyDetained && !isReleased;
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsNumericFilterSelected())
            {
                return;
            }

            bool isControl = char.IsControl(e.KeyChar);
            bool isDigit = char.IsDigit(e.KeyChar);

            e.Handled = !(isControl || isDigit);
        }

        private bool IsNumericFilterSelected()
        {
            string selected = cmbFilter.SelectedItem as string;
            return selected == "Detain ID" || selected == "Release Application ID";
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicenseForm form = new ReleaseDetainedLicenseForm();
            form.ShowDialog();
            SetUpDataGridView();
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            DetainLicenseForm form = new DetainLicenseForm();
            form.ShowDialog();
            SetUpDataGridView();
        }
    }
}
