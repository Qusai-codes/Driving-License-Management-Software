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
    public partial class DriverLicenseInformationForm : Form
    {
        private int _driverId;

        public DriverLicenseInformationForm(int driverId)
        {
            InitializeComponent();
            _driverId = driverId;
        }

        private void DriverLicenseInformationForm_Load(object sender, EventArgs e)
        {
            drivingLicenseInformationControl1.DriverId = _driverId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
