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
    public partial class VisionTestAppointmentForm : Form
    {
        private int _localDrivingLicenseApplicationId;

        public VisionTestAppointmentForm(int localDrivingLicenseApplicationId)
        {
            InitializeComponent();
            _localDrivingLicenseApplicationId = localDrivingLicenseApplicationId;
        }

        private void VisionTestAppointmentForm_Load(object sender, EventArgs e)
        {
            drivingLicenseApplicationInformationControl1.LocalDrivingLicenseApplicationId =
                _localDrivingLicenseApplicationId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            // TODO: Complete the logic.
        }

        private void RefreshApplicationsList()
        {
            // TODO: Complete the logic.
        }

        private void FormatDataGridView()
        {
            // TODO: Complete the logic.
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            // TODO: Complete the logic.
        }
    }
}
