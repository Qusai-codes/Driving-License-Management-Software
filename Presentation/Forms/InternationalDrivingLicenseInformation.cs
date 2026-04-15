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
    public partial class InternationalDrivingLicenseInformation : Form
    {
        private int _internationalDrivingLicenseId;

        public InternationalDrivingLicenseInformation(int internationalDrivingLicenseId)
        {
            InitializeComponent();
            _internationalDrivingLicenseId = internationalDrivingLicenseId;
        }

        private void InternationalDrivingLicenseInformation_Load(object sender, EventArgs e)
        {
            internationalDrivingLicenseInformationControl1.InternationalDrivingLicenseId
                = _internationalDrivingLicenseId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
