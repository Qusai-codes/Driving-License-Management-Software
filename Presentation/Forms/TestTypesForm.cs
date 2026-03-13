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
    public partial class TestTypesForm : Form
    {
        public TestTypesForm()
        {
            InitializeComponent();
        }

        private void TestTypesForm_Load(object sender, EventArgs e)
        {
            RefreshTestTypesList();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTestTypes.CurrentRow != null)
            {
                int id = (int)dgvTestTypes.CurrentRow.Cells["TestTypeID"].Value;
                UpdateTestTypeForm form = new UpdateTestTypeForm(id);
                form.ShowDialog();
                RefreshTestTypesList();
            }
        }

        private void RefreshTestTypesList()
        {
            try
            {
                DataTable dt = GetTestTypesList();
                dgvTestTypes.DataSource = dt;

                FormatDataGridView();
                lblNumberOfRecords.Text = dgvTestTypes.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading test types: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            // Set headers
            SetColumnHeader("TestTypeID", "ID");
            SetColumnHeader("TestTypeTitle", "Title");
            SetColumnHeader("TestTypeDescription", "Description");
            SetColumnHeader("TestTypeFees", "Fees");

            if (dgvTestTypes.Columns["TestTypeTitle"] != null)
            {
                dgvTestTypes.Columns["TestTypeTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private DataTable GetTestTypesList()
        {
            return TestType.GetAllTestTypes();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvTestTypes.Columns[columnName] != null)
            {
                dgvTestTypes.Columns[columnName].HeaderText = headerText;
            }
        }
    }
}
