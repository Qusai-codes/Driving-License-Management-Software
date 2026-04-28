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
    public partial class DetainLicenseForm : Form
    {
        public DetainLicenseForm()
        {
            InitializeComponent();
            drivingLicenseInformationWithFilterControl1.LicenseSelected += DrivingLicenseInformationWithFilterControl1_LicenseSelected;
        }

        private void DetainLicenseForm_Load(object sender, EventArgs e)
        {

        }

        private void DrivingLicenseInformationWithFilterControl1_LicenseSelected(object sender, Events.LicenseSelectedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            // TODO: complete the implementation.
            DialogResult result = MessageBox.Show("Are you sure you want to detain this license?", 
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            MessageBox.Show("License detained successfully with ID = " + 2, 
                "License Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private void llbShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // TODO: complete the implementation.
        }

        private void llbLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // TODO: complete the implementation.
        }
    }
}
