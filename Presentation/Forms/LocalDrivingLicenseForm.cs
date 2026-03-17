using Business;
using Presentation.Events;
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
    public partial class LocalDrivingLicenseForm : Form
    {
        private int _userId = -1;
        private int _personId = -1;
        private FormMode _mode;

        public LocalDrivingLicenseForm(FormMode mode, int userId)
        {
            InitializeComponent();
            personDetailsWithFilterControl1.PersonSelected += PersonDetailsWithFilterControl1_PersonSelected;
            _mode = mode;
            if (_mode == FormMode.Edit)
            {
                _userId = userId;
                _personId = User.GetPersonId(_userId);

                personDetailsWithFilterControl1.PersonId = _personId;
            }

        }

        private void PersonDetailsWithFilterControl1_PersonSelected(object sender, PersonSavedEventArgs e)
        {
            _personId = (e != null && e.PersonId > 0) ? e.PersonId : -1;
            //_personId != -1 ? btnSave

        }

        private void LocalDrivingLicenseForm_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpPersonalInfo;
            LoadDrivingLicenseClasses();
            LoadApplicationTypes();
            lblApplicationDate.Text = DateTime.Now.ToString("d");
            lblApplicationFees.Text = ApplicationType.GetApplicationTypeFees(
                ApplicationType.ApplicationTypeTitle.NewLocalDrivingLicense).ToString();
            SwitchToMode(_mode);
        }

        private void LoadDrivingLicenseClasses()
        {
            int ordinaryDrivingLicenseIndex = 2;
            string[] licenseclassNames = LicenseClass.GetAllLicenseClassNames();
            if (licenseclassNames != null && licenseclassNames.Length > 0)
            {
                cmbDrivingLicenseClass.Items.Clear();
                cmbDrivingLicenseClass.Items.AddRange(licenseclassNames);
                cmbDrivingLicenseClass.SelectedIndex = ordinaryDrivingLicenseIndex;
            }
        }

        private void LoadApplicationTypes()
        {
            DataTable dt = ApplicationType.GetAllApplicationTypes();

            cmbApplicationType.DataSource = dt;
            cmbApplicationType.DisplayMember = "ApplicationTypeTitle";
            cmbApplicationType.ValueMember = "ApplicationTypeID";
            cmbApplicationType.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void SwitchToMode(FormMode mode)
        {
            _mode = mode;


            if (_mode == FormMode.Add)
            {
                this.Text = "New Local Driving License Application";
                lblFormTitle.Text = "New Local Driving License Application";
                personDetailsWithFilterControl1.EnableEditingOfPersonInfo = false;
                btnSave.Enabled = false;
            }
            else if (_mode == FormMode.Edit)
            {
                this.Text = "Update Local Driving License Application";
                lblFormTitle.Text = "Update Local Driving License Application";
                personDetailsWithFilterControl1.PersonFilter.Enabled = false;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_personId == -1 && _mode == FormMode.Add)
            {
                MessageBox.Show("Please select a person.",
                    "Select Peron", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tabControl1.SelectedTab = tpApplicationInfo;
            btnSave.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void tpApplicationInfo_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }
    }
}
