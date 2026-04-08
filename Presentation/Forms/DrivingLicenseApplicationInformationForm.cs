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
    public partial class DrivingLicenseApplicationInformationForm : Form
    {
        private int _localDrivingLicenseApplicationId;
        public DrivingLicenseApplicationInformationForm(int localDrivingLicenseApplicationId)
        {
            InitializeComponent();
            _localDrivingLicenseApplicationId = localDrivingLicenseApplicationId;
        }

        private void DrivingLicenseApplicationInformationForm_Load(object sender, EventArgs e)
        {
            drivingLicenseApplicationInformationControl1.LocalDrivingLicenseApplicationId =
                _localDrivingLicenseApplicationId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
