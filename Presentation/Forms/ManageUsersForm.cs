using Business;
using Contracts.DTOs;
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
            PopulateFilterComboBox();
            RefreshUsersList();
        }

        private void cmbFilterUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DataTable GetUsersList()
        {
            List<UserDto> list = User.GetAllUsers();
            DataTable usersDataTable = UserDto.ToDataTable(list);
            return usersDataTable;
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
                var persons = Person.GetAllPersons();
                var personLookup = persons.ToDictionary(
                    p => p.PersonId,
                    p => BuildFullName(p.FirstName, p.SecondName, p.ThirdName, p.LastName));

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
            AddUserForm form = new AddUserForm();
            // subscribe to form events here
            form.ShowDialog();
            RefreshUsersList();
        }

        private void PopulateFilterComboBox()
        {
            cmbFilterUsers.Items.Clear();
            cmbFilterUsers.Items.Add("None");
            cmbFilterUsers.Items.Add("User ID");
            cmbFilterUsers.Items.Add("Person ID");
            cmbFilterUsers.Items.Add("Full Name");
            cmbFilterUsers.Items.Add("UserName");
            cmbFilterUsers.Items.Add("Is Active");

            cmbFilterUsers.SelectedIndex = 0;
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

        private void HideColumn(string columnName)
        {
            if (dgvUsers.Columns[columnName] != null)
            {
                dgvUsers.Columns[columnName].Visible = false;
            }
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvUsers.Columns[columnName] != null)
            {
                dgvUsers.Columns[columnName].HeaderText = headerText;
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allUsersData == null)
                return;

            var selected = cmbFilterUsers.SelectedItem as string;
            var filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || string.IsNullOrEmpty(selected) || selected == "None")
            {
                dgvUsers.DataSource = _allUsersData;
                lblNumberOfRecords.Text = dgvUsers.Rows.Count.ToString();
                return;
            }

            string column = GetColumnNameFromDisplayName(selected);
            if (string.IsNullOrEmpty(column) || !_allUsersData.Columns.Contains(column))
            {
                dgvUsers.DataSource = _allUsersData;
                lblNumberOfRecords.Text = dgvUsers.Rows.Count.ToString();
                return;
            }

            DataView view = new DataView(_allUsersData);
            if (_allUsersData.Columns[column].DataType == typeof(string))
            {
                view.RowFilter = string.Format("{0} LIKE '%{1}%'", column.Replace("'", "''"), filterText.Replace("'", "''"));
            }
            else if (_allUsersData.Columns[column].DataType == typeof(int))
            {
                int num;
                if (int.TryParse(filterText, out num))
                {
                    view.RowFilter = string.Format("{0} = {1}", column, num);
                }
            }
            else if (_allUsersData.Columns[column].DataType == typeof(bool))
            {
                bool val;
                if (bool.TryParse(filterText, out val))
                {
                    view.RowFilter = string.Format("{0} = {1}", column, val ? "true" : "false");
                }
            }

            dgvUsers.DataSource = view;
            lblNumberOfRecords.Text = dgvUsers.Rows.Count.ToString();
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
    }
}
