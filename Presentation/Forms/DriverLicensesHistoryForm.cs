using Business;
using Presentation.Controls;
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
    public partial class DriverLicensesHistoryForm : Form
    {
        private int _driverId;

        public DriverLicensesHistoryForm(int driverId)
        {
            InitializeComponent();
            _driverId = driverId;
        }

        private void DriverLicensesHistoryForm_Load(object sender, EventArgs e)
        {
            Driver driver = Driver.FindByDriverId(_driverId);
            if (driver == null)
            {
                this.Close();
            }

            driverLicensesControl1.DriverId = _driverId;


            personDetailsWithFilterControl1.PersonFilter.Enabled = false;
            personDetailsWithFilterControl1.PersonId = driver.PersonId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
