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
    public partial class ScheduleDrivingTestForm : Form
    {
        private int _localDrivingLicenseApplicationId;
        private TestType.TestTypeId _testTypeId;

        public ScheduleDrivingTestForm(int localDrivingLicenseApplicationId, TestType.TestTypeId testTypeId)
        {
            InitializeComponent();
            _localDrivingLicenseApplicationId = localDrivingLicenseApplicationId;
            _testTypeId = testTypeId;
        }

        private void ScheduleDrivingTestForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // TODO: Implement the logic.
            MessageBox.Show("Data Saved Successfully.", "Saved",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
