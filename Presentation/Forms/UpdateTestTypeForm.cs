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
    public partial class UpdateTestTypeForm : Form
    {
        private int _testTypeId;
        private TestType _testType;

        public UpdateTestTypeForm(int testTypeId)
        {
            InitializeComponent();
            _testTypeId = testTypeId;
        }

        private void UpdateTestTypeForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            _testType = TestType.Find(_testTypeId);
            if (_testType == null)
            {
                MessageBox.Show($"No Test Type with ID = {_testTypeId}.",
                    "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblTestTypeId.Text = _testTypeId.ToString();
            txtTitle.Text = _testType.Title;
            txtDescription.Text = _testType.Description;
            txtFees.Text = _testType.Fees.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _testType.Title = txtTitle.Text;
            _testType.Description = txtDescription.Text;
            _testType.Fees = decimal.Parse(txtFees.Text);
            if (_testType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to update test type.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits and control keys
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                errorProvider1.SetError(txtTitle, "This field cannot be blank.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtTitle, "");
            }
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                errorProvider1.SetError(txtDescription, "This field cannot be blank.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtDescription, "");
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFees.Text))
            {
                errorProvider1.SetError(txtFees, "This field cannot be blank.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtFees, "");
            }
        }
    }
}
