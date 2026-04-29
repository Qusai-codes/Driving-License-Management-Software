using Business;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Presentation.Events;

namespace Presentation
{
    public partial class ManagePeopleForm : Form
    {
        private Dictionary<int, string> _countriesCache;
        private DataTable _allPeopleData;

        public ManagePeopleForm()
        {
            InitializeComponent();
            LoadCountriesCache();
        }

        private void LoadCountriesCache()
        {
            DataTable countries = Country.GetAllCountries();
            _countriesCache = countries.AsEnumerable().ToDictionary(
                row => row.Field<int>("CountryId"),
                row => row.Field<string>("CountryName"));
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            int addedPersonId = -1;

            using (PersonProfileForm personProfileForm = new PersonProfileForm(FormMode.Add))
            {
                EventHandler<PersonSavedEventArgs> onPersonSaved = (s, args) =>
                {
                    addedPersonId = args.PersonId;
                };

                personProfileForm.PersonSaved += onPersonSaved;
                personProfileForm.ShowDialog();
                personProfileForm.PersonSaved -= onPersonSaved;
            }

            RefreshPeopleList();

            if (addedPersonId != -1)
            {
                SelectPersonInGrid(addedPersonId);
            }
        }

        private void SelectPersonInGrid(int personId)
        {
            foreach (DataGridViewRow row in dgvPeople.Rows)
            {
                if (row.Cells["PersonID"].Value != null &&
                    (int)row.Cells["PersonID"].Value == personId)
                {
                    // Clear previous selection
                    dgvPeople.ClearSelection();

                    // Select the row
                    row.Selected = true;

                    // Scroll to make it visible
                    dgvPeople.FirstDisplayedScrollingRowIndex = row.Index;

                    break;
                }
            }
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ManagePeopleForm_Load(object sender, EventArgs e)
        {
            // Subscribe to CellFormatting event for Gender display
            dgvPeople.CellFormatting += dgvPeople_CellFormatting;

            // Populate filter combo box
            PopulateFilterComboBox();

            RefreshPeopleList();
        }

        private void dgvPeople_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if this is the Gender column
            if (dgvPeople.Columns[e.ColumnIndex].Name == "Gender" && e.Value != null)
            {
                byte gender = (byte)e.Value;
                e.Value = gender == 0 ? "Male" : "Female";
                e.FormattingApplied = true;
            }

            if (dgvPeople.Columns[e.ColumnIndex].Name == "NationalityCountryId" && e.Value != null)
            {
                int countryId = (int)e.Value;

                if (_countriesCache.TryGetValue(countryId, out string countryName))
                {
                    e.Value = countryName;
                    e.FormattingApplied = true;
                }
            }
        }

        private DataTable GetPeopleList()
        {
            return Person.GetAllPersons();
        }

        private void RefreshPeopleList()
        {
            try
            {
                DataTable dt = GetPeopleList();
                _allPeopleData = dt; // Store the full dataset
                dgvPeople.DataSource = _allPeopleData;
                FormatDataGridView();

                // Reapply filter if one is active
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading people: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            // Setting column headers of data grid view
            SetColumnHeader("PersonId", "Person ID");
            SetColumnHeader("NationalNo", "National No.");
            SetColumnHeader("FirstName", "First Name");
            SetColumnHeader("SecondName", "Second Name");
            SetColumnHeader("ThirdName", "ThirdName");
            SetColumnHeader("LastName", "Last Name");
            SetColumnHeader("DateOfBirth", "Date Of Birth");
            SetColumnHeader("NationalityCountryId", "Nationality");

            // Hiding unwanted columns
            HideColumn("Address");
            HideColumn("ImagePath");
        }

        private void HideColumn(string columnName)
        {
            if (dgvPeople.Columns[columnName] != null)
            {
                dgvPeople.Columns[columnName].Visible = false;
            }
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvPeople.Columns[columnName] != null)
            {
                dgvPeople.Columns[columnName].HeaderText = headerText;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow == null)
            {
                MessageBox.Show("Please select a person to view details.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int personId = (int)dgvPeople.CurrentRow.Cells["PersonID"].Value;
            PersonDetailsForm personDetailsForm = new PersonDetailsForm(personId);
            personDetailsForm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dgvPeople.CurrentRow == null)
            {
                MessageBox.Show("Please select a person to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int personId = (int)dgvPeople.CurrentRow.Cells["PersonID"].Value;

            PersonProfileForm personProfileForm = new PersonProfileForm(FormMode.Edit, personId);
            personProfileForm.ShowDialog();

            // Refresh after form closes
            RefreshPeopleList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow == null)
            {
                MessageBox.Show("Please select a person to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int personId = (int)dgvPeople.CurrentRow.Cells["PersonID"].Value;
            // TODO: create a method to check if delete person is acceptable
            if (User.IsUserExistByPersonId(personId))
            {
                MessageBox.Show("Person was not deleted because it has data linked to it.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string message = string.Format("Are you sure you want to delete Person [{0}]",
                personId);

            DialogResult result = MessageBox.Show(message, "Confirm Delete", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                if (Person.Delete(personId))
                {
                    MessageBox.Show("Person Deleted Successfully.", "Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to Delete Person.", "Fail",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }

            RefreshPeopleList();
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

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.SelectedItem == null)
            {
                return;
            }

            string selectedFilter = cmbFilter.SelectedItem.ToString();

            if (selectedFilter == "None")
            {
                // Hide textbox and show all data
                txtFilterValue.Visible = false;
                txtFilterValue.Clear();
                ApplyFilter();
            }
            else
            {
                // Show textbox for filtering
                txtFilterValue.Visible = true;
                txtFilterValue.Clear();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allPeopleData == null)
                return;

            string selectedFilter = cmbFilter.SelectedItem?.ToString();
            string filterValue = txtFilterValue.Text.Trim();

            // Create a DataView for filtering
            DataView dv = _allPeopleData.DefaultView;

            // If "None" or empty filter value, show all data
            if (selectedFilter == "None" || string.IsNullOrEmpty(filterValue))
            {
                dv.RowFilter = ""; // Clear the filter
                lblNumberOfRecords.Text = _allPeopleData.Rows.Count.ToString();
                return;
            }

            // Map display names to actual column names
            string columnName = GetColumnNameFromDisplayName(selectedFilter);

            if (string.IsNullOrEmpty(columnName))
            {
                dv.RowFilter = ""; // Clear the filter
                lblNumberOfRecords.Text = _allPeopleData.Rows.Count.ToString();
                return;
            }

            // Build filter expression based on column type
            if (columnName == "PersonId")
            {
                // Numeric filter
                if (int.TryParse(filterValue, out int numericValue))
                {
                    dv.RowFilter = $"{columnName} = {numericValue}";
                }
                else
                {
                    dv.RowFilter = "1=0"; // No results if invalid number
                }
            }
            else if (columnName == "Gender")
            {
                // Gender filter - accept "Male", "Female", "0", "1"
                if (filterValue.Equals("Male", StringComparison.OrdinalIgnoreCase) || filterValue == "0")
                {
                    dv.RowFilter = "Gender = 0";
                }
                else if (filterValue.Equals("Female", StringComparison.OrdinalIgnoreCase) || filterValue == "1")
                {
                    dv.RowFilter = "Gender = 1";
                }
                else
                {
                    dv.RowFilter = "Gender = -1"; // No results
                }
            }
            else if (columnName == "NationalityCountryId")
            {
                // Filter by country name
                var matchingCountries = _countriesCache
                    .Where(c => c.Value.IndexOf(filterValue, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(c => c.Key)
                    .ToList();

                if (matchingCountries.Any())
                {
                    // Use IN operator for cleaner syntax
                    string countryIds = string.Join(", ", matchingCountries);
                    dv.RowFilter = $"NationalityCountryId IN ({countryIds})";
                }
                else
                {
                    dv.RowFilter = "1=0"; // No results
                }
            }
            else
            {
                // String filter (contains)
                dv.RowFilter = $"{columnName} LIKE '%{filterValue.Replace("'", "''")}%'";
            }

            // Update record count based on filtered view
            lblNumberOfRecords.Text = dv.Count.ToString();
        }

        private string GetColumnNameFromDisplayName(string displayName)
        {
            switch (displayName)
            {
                case "Person ID":
                    return "PersonId";
                case "National No.":
                    return "NationalNo";
                case "First Name":
                    return "FirstName";
                case "Second Name":
                    return "SecondName";
                case "Third Name":
                    return "ThirdName";
                case "Last Name":
                    return "LastName";
                case "Nationality":
                    return "NationalityCountryId";
                case "Gender":
                    return "Gender";
                case "Phone":
                    return "Phone";
                case "Email":
                    return "Email";
                default:
                    return null;
            }
        }

        private void PopulateFilterComboBox()
        {
            cmbFilter.Items.Clear();
            cmbFilter.Items.Add("None");
            cmbFilter.Items.Add("Person ID");
            cmbFilter.Items.Add("National No.");
            cmbFilter.Items.Add("First Name");
            cmbFilter.Items.Add("Second Name");
            cmbFilter.Items.Add("Third Name");
            cmbFilter.Items.Add("Last Name");
            cmbFilter.Items.Add("Nationality");
            cmbFilter.Items.Add("Gender");
            cmbFilter.Items.Add("Phone");
            cmbFilter.Items.Add("Email");

            cmbFilter.SelectedIndex = 0; // Default to "None"
        }

    }
}
