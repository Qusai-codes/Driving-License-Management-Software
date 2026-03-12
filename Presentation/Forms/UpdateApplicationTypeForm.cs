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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Presentation.Forms
{
    public partial class UpdateApplicationTypeForm : Form
    {
        private int _applicationTypeId;
        private ApplicationType _applicationType;

        public UpdateApplicationTypeForm(int applicationTypeId)
        {
            InitializeComponent();
            _applicationTypeId = applicationTypeId;
        }

        private void UpdateApplicationTypeForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            _applicationType = ApplicationType.Find(_applicationTypeId);
            if (_applicationType == null)
            {
                MessageBox.Show($"No Application Type with ID = {_applicationTypeId}.",
                    "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblApplicationTypeId.Text = _applicationTypeId.ToString();
            txtApplicationTypeTitle.Text = _applicationType.Title;
            txtApplicationTypeFees.Text = _applicationType.Fees.ToString();
        }

        private void txtApplicationTypeFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits and control keys
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && (sender as System.Windows.Forms.TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _applicationType.Title = txtApplicationTypeTitle.Text;
            _applicationType.Fees = decimal.Parse(txtApplicationTypeFees.Text);
            if (_applicationType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to update application type.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtApplicationTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApplicationTypeTitle.Text))
            {
                errorProvider1.SetError(txtApplicationTypeTitle, "Application type title cannot be blank.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtApplicationTypeTitle, "");
            }
        }
    }
}
