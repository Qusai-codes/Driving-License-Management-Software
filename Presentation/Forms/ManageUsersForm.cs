using Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Forms
{
    public partial class ManageUsersForm : Form
    {

        private DataTable _allUsersData;

        public ManageUsersForm()
        {
            InitializeComponent();
        }

        private void ManageUsersForm_Load(object sender, EventArgs e)
        {
            SetupFilterComboBox();
            SetupIsActiveFilterCombo();
            RefreshUsersList();
        }

        private void SetupIsActiveFilterCombo()
        {
            cmbIsActiveFilter.Items.Clear();
            cmbIsActiveFilter.Items.AddRange(new object[] { "All", "Yes", "No" });
            cmbIsActiveFilter.SelectedIndex = 0;
            cmbIsActiveFilter.Visible = false;
        }

        private void SetupFilterComboBox()
        {
            cmbFilterUsers.Items.Clear();
            string[] personFilterOptions = new string[] {
                "None", "User ID", "Person ID", "Full Name",
                "UserName", "Is Active"
            };
            cmbFilterUsers.Items.AddRange(personFilterOptions);
            cmbFilterUsers.SelectedIndex = 0;
        }

        private void cmbIsActiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void cmbFilterUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbFilterUsers.SelectedItem as string;
            if (string.IsNullOrEmpty(selected) || selected == "None")
            {
                txtFilterValue.Visible = false;
                cmbIsActiveFilter.Visible = false;
                txtFilterValue.Clear();
            }
            else if (selected == "Is Active")
            {
                txtFilterValue.Visible = false;
                txtFilterValue.Clear();
                cmbIsActiveFilter.Visible = true;
            }
            else
            {
                cmbIsActiveFilter.Visible = false;
                txtFilterValue.Visible = true;
                txtFilterValue.Clear();
            }

            ApplyFilter();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DataTable GetUsersList()
        {
            return User.GetAllUsers();
        }

        private void RefreshUsersList()
        {
            try
            {
                DataTable dt = GetUsersList();

                // Add FullName column
                if (!dt.Columns.Contains("FullName"))
                {
                    dt.Columns.Add("FullName", typeof(string));
                }

                // Build a lookup for PersonId -> FullName
                DataTable persons = Person.GetAllPersons();
                Dictionary<int, string> personLookup = persons.AsEnumerable().ToDictionary(
                    row => row.Field<int>("PersonId"),
                    row => BuildFullName(
                        Convert.ToString(row["FirstName"]),
                        Convert.ToString(row["SecondName"]),
                        row["ThirdName"] == DBNull.Value ? null : row["ThirdName"].ToString(),
                        Convert.ToString(row["LastName"])));

                foreach (DataRow row in dt.Rows)
                {
                    int personId = row.Field<int>("PersonId");
                    string fullName;
                    if (personLookup.TryGetValue(personId, out fullName))
                    {
                        row["FullName"] = fullName;
                    }
                    else
                    {
                        row["FullName"] = "(not found)";
                    }
                }

                _allUsersData = dt;
                dgvUsers.AutoGenerateColumns = true;
                dgvUsers.DataSource = _allUsersData;

                FormatDataGridView();
                ApplyFilter();
                lblNumberOfRecords.Text = dgvUsers.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string BuildFullName(string first, string second, string third, string last)
        {
            return string.Join(" ",
                new[] { first, second, third, last }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            AddEditUserForm form = new AddEditUserForm();
            form.ShowDialog();
            RefreshUsersList();
        }

        private void FormatDataGridView()
        {
            // Force checkbox column for IsActive if needed
            if (dgvUsers.Columns["IsActive"] != null && !(dgvUsers.Columns["IsActive"] is DataGridViewCheckBoxColumn))
            {
                int displayIndex = dgvUsers.Columns["IsActive"].DisplayIndex;
                dgvUsers.Columns.Remove("IsActive");
                var chk = new DataGridViewCheckBoxColumn
                {
                    Name = "IsActive",
                    DataPropertyName = "IsActive",
                    HeaderText = "Is Active",
                    ThreeState = false
                };
                dgvUsers.Columns.Insert(displayIndex, chk);
            }

            // Set headers
            SetColumnHeader("UserId", "User ID");
            SetColumnHeader("PersonId", "Person ID");
            SetColumnHeader("FullName", "Full Name");
            SetColumnHeader("UserName", "User Name");
            SetColumnHeader("IsActive", "Is Active");

            // Set display order
            SetDisplayIndex("UserId", 0);
            SetDisplayIndex("PersonId", 1);
            SetDisplayIndex("FullName", 2);
            SetDisplayIndex("UserName", 3);
            SetDisplayIndex("IsActive", 4);

            // Sizing: make FullName fill remaining space, others auto-size to content/header
            if (dgvUsers.Columns["FullName"] != null)
            {
                dgvUsers.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvUsers.Columns["UserId"] != null)
                dgvUsers.Columns["UserId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            if (dgvUsers.Columns["PersonId"] != null)
                dgvUsers.Columns["PersonId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            if (dgvUsers.Columns["UserName"] != null)
                dgvUsers.Columns["UserName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            if (dgvUsers.Columns["IsActive"] != null)
                dgvUsers.Columns["IsActive"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Adjust sizes based on current data
            dgvUsers.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void SetDisplayIndex(string columnName, int displayIndex)
        {
            if (dgvUsers.Columns[columnName] != null)
            {
                dgvUsers.Columns[columnName].DisplayIndex = displayIndex;
            }
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvUsers.Columns[columnName] != null)
            {
                dgvUsers.Columns[columnName].HeaderText = headerText;
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            string selected = cmbFilterUsers.SelectedItem as string;
            bool numeric = selected == "User ID" || selected == "Person ID";

            if (!numeric)
                return;

            bool isControl = char.IsControl(e.KeyChar);
            bool isDigit = char.IsDigit(e.KeyChar);

            e.Handled = !(isControl || isDigit);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allUsersData == null)
                return;

            DataView dv = _allUsersData.DefaultView;

            string selected = cmbFilterUsers.SelectedItem as string;
            string filterText = txtFilterValue.Text.Trim();

            // Reset filter if "None"
            if (string.IsNullOrEmpty(selected) || selected == "None")
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            // Special case: IsActive (uses combo box)
            if (selected == "Is Active")
            {
                string choice = cmbIsActiveFilter.SelectedItem as string;

                if (string.IsNullOrEmpty(choice) || choice == "All")
                {
                    dv.RowFilter = "";
                }
                else
                {
                    bool isActive = choice == "Yes";
                    dv.RowFilter = $"IsActive = {isActive.ToString().ToLower()}";
                }

                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            // Other filters use textbox
            if (string.IsNullOrEmpty(filterText))
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            string column = GetColumnNameFromDisplayName(selected);

            if (string.IsNullOrEmpty(column) || !_allUsersData.Columns.Contains(column))
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            // Build filter based on column type
            Type colType = _allUsersData.Columns[column].DataType;

            if (colType == typeof(string))
            {
                // safe, correct SQL‑style filter expression
                dv.RowFilter = $"{column} LIKE '%{filterText.Replace("'", "''")}%'";
            }
            else if (colType == typeof(int))
            {
                if (int.TryParse(filterText, out int num))
                    dv.RowFilter = $"{column} = {num}";
                else
                    dv.RowFilter = "1=0";
            }

            lblNumberOfRecords.Text = dv.Count.ToString();
        }

        private string GetColumnNameFromDisplayName(string displayName)
        {
            switch (displayName)
            {
                case "User ID":
                    return "UserId";
                case "Person ID":
                    return "PersonId";
                case "Full Name":
                    return "FullName";
                case "UserName":
                    return "UserName";
                case "Is Active":
                    return "IsActive";
                default:
                    return null;
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = (int)dgvUsers.CurrentRow.Cells["UserId"].Value;

            UserDetailsForm form = new UserDetailsForm(userId);
            form.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = (int)dgvUsers.CurrentRow.Cells["UserId"].Value;

            AddEditUserForm form = new AddEditUserForm(userId);
            form.ShowDialog();
            RefreshUsersList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int userId = -1;
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            userId = (int)dgvUsers.CurrentRow.Cells["UserId"].Value;

            // TODO: Add logic to check for data integrity before deleting the user.
            //if (!User.CanDeleteUser(userId))
            //{
            //    MessageBox.Show("User is not deleted due to data connected to it.",
            //        "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            string message = string.Format("Are you sure you want to delete User [{0}]",
                userId);

            DialogResult result = MessageBox.Show(message, "Confirm Delete", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                if (User.Delete(userId))
                {
                    MessageBox.Show("User Deleted Successfully.", "Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to Delete User.", "Fail",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }

            RefreshUsersList();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePasswordForm form = new ChangePasswordForm(Helpers.AppSession.CurrentUserId);
            form.ShowDialog();
            RefreshUsersList();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is Not Implemented Yet!",
                "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is Not Implemented Yet!",
                "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private int GetUserIdFromDataGridView()
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            int userId = (int)dgvUsers.CurrentRow.Cells["UserId"].Value;
            return userId;
        }
    }
}
