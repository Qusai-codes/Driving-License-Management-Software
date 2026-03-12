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
    public partial class ManageApplicationTypesForm : Form
    {
        public ManageApplicationTypesForm()
        {
            InitializeComponent();
        }

        private void ManageApplicationTypesForm_Load(object sender, EventArgs e)
        {
            RefreshApplicationTypesList();
        }

        private DataTable GetApplicationTypesList()
        {
            return ApplicationType.GetAllApplicationTypes();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplicationTypes.CurrentRow == null)
            {
                MessageBox.Show("Please select an application type to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // logic here.

            RefreshApplicationTypesList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RefreshApplicationTypesList()
        {
            try
            {
                DataTable dt = GetApplicationTypesList();
                dgvApplicationTypes.DataSource = dt;

                FormatDataGridView();
                lblNumberOfRecords.Text = dgvApplicationTypes.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading application types: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            // Set headers
            SetColumnHeader("ApplicationTypeID", "ID");
            SetColumnHeader("ApplicationTypeTitle", "Title");
            SetColumnHeader("ApplicationFees", "Fees");

            if (dgvApplicationTypes.Columns["ApplicationTypeTitle"] != null)
            {
                dgvApplicationTypes.Columns["ApplicationTypeTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
                
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvApplicationTypes.Columns[columnName] != null)
            {
                dgvApplicationTypes.Columns[columnName].HeaderText = headerText;
            }
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplicationTypes.CurrentRow != null)
            {
                int id = (int)dgvApplicationTypes.CurrentRow.Cells["ApplicationTypeID"].Value;
                UpdateApplicationTypeForm form = new UpdateApplicationTypeForm(id);
                form.ShowDialog();
                RefreshApplicationTypesList();
            }
        }
    }
}
