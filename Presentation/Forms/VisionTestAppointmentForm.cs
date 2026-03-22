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
            ScheduleDrivingTestForm form = new ScheduleDrivingTestForm(_localDrivingLicenseApplicationId);
            form.ShowDialog();
            RefreshAppointmentsList();
        }

        private DataTable GetAllVisionTestAppointments()
        {
            return TestAppointment.GetAllTestAppointments(_localDrivingLicenseApplicationId, 
                TestType.TestTypeId.Vision);
        }

        private void RefreshAppointmentsList()
        {
            try
            {
                DataTable visionTestAppointments = GetAllVisionTestAppointments();

                dgvVisionTestAppointments.AutoGenerateColumns = true;
                dgvVisionTestAppointments.DataSource = visionTestAppointments;

                FormatDataGridView();
                lblNumberOfRecords.Text = dgvVisionTestAppointments.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading vision test appointments: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            // Setting column headers of data grid view
            SetColumnHeader("TestAppointmentID", "Appointment ID");
            SetColumnHeader("AppointmentDate", "Appointment Date");
            SetColumnHeader("PaidFees", "Paid Fees");
            SetColumnHeader("IsLocked", "Is Locked");
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvVisionTestAppointments.Columns[columnName] != null)
            {
                dgvVisionTestAppointments.Columns[columnName].HeaderText = headerText;
            }
        }
    }
}
