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
    public partial class TakingDrivingTestForm : Form
    {
        private int _testAppointmentId;

        public TakingDrivingTestForm(int testAppointmentId)
        {
            InitializeComponent();

            _testAppointmentId = testAppointmentId;
        }

        private void TakingDrivingTestForm_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
