using Business;
using Presentation.Events;
using Presentation.Helpers;
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
        private int _drivingLicenseApplicationId = -1;
        private FormMode _mode;

        public LocalDrivingLicenseForm(int applicationId, FormMode mode)
        {
            InitializeComponent();
            _userId = AppSession.CurrentUserId;
            personDetailsWithFilterControl1.PersonSelected += PersonDetailsWithFilterControl1_PersonSelected;
            _mode = mode;
            if (_mode == FormMode.Edit)
            {
                
                _personId = User.GetPersonId(_userId);

                personDetailsWithFilterControl1.PersonId = _personId;
                _drivingLicenseApplicationId = applicationId;
            }

        }

        private void PersonDetailsWithFilterControl1_PersonSelected(object sender, PersonSavedEventArgs e)
        {
            _personId = (e != null && e.PersonId > 0) ? e.PersonId : -1;
            if (_personId != -1)
            {
                personDetailsWithFilterControl1.EnableEditingOfPersonInfo = true;
            }

        }

        private void LocalDrivingLicenseForm_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpPersonalInfo;
            LoadDrivingLicenseClasses();

            lblApplicationFees.Text = ApplicationType.GetApplicationTypeFees(
                ApplicationType.ApplicationTypeTitle.NewLocalDrivingLicense).ToString();
            lblUserName.Text = User.Find(_userId).UserName;
            lblApplicationDate.Text = DateTime.Now.ToString("d");


            if (_mode == FormMode.Edit)
            {
                // Load application information
                Business.Application application = Business.Application.FindByApplicationId(
                    _drivingLicenseApplicationId);
                lblApplicationDate.Text = application.ApplicationDate.ToString("d");
                // select class

                int localDrivingLicenseApplicationId = LocalDrivingLicenseApplication.
                    GetLocalDrivingLicenseApplicationId(_drivingLicenseApplicationId);
                LocalDrivingLicenseApplication localApp = LocalDrivingLicenseApplication.Find(localDrivingLicenseApplicationId);
                cmbDrivingLicenseClass.SelectedValue = localApp.LicenseClassId;
                cmbDrivingLicenseClass.DropDownStyle = ComboBoxStyle.DropDownList;

                // prevent changing it
                cmbDrivingLicenseClass.Enabled = false;
            }

            SwitchToMode(_mode);
        }

        private void LoadDrivingLicenseClasses()
        {
            int ordinaryDrivingLicenseClassIndex = 2;
            DataTable dt = LicenseClass.GetAllLicenseClasses();
            if (dt != null && dt.Rows.Count > 0)
            {
                cmbDrivingLicenseClass.Items.Clear();
                cmbDrivingLicenseClass.DataSource = dt;
                cmbDrivingLicenseClass.DisplayMember = "ClassName";
                cmbDrivingLicenseClass.ValueMember = "LicenseClassID";
                cmbDrivingLicenseClass.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbDrivingLicenseClass.SelectedIndex = ordinaryDrivingLicenseClassIndex;
            }
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
            // TODO: Refactor this method to account for Adding new driving 
            // license and updating an existing driving license.

            if (_mode == FormMode.Add)
            {
                int licenseClassId;
                if (!TryGetSelectedLicenseClassId(out licenseClassId))
                    return;

                int localAppId, blockingAppId;
                string error;

                bool ok = LocalDrivingLicenseApplication.TryCreateNew(
                    _personId, licenseClassId, _userId,
                    out localAppId, out blockingAppId, out error);

                if (!ok)
                {
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(
                    string.Format("Data Saved Successfully. Local Driving License Application ID = {0}", localAppId),
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblApplicationId.Text = localAppId.ToString();
                SwitchToMode(FormMode.Edit);
            }
            
        }

        private bool TryGetSelectedLicenseClassId(out int licenseClassId)
        {
            licenseClassId = -1;

            if (cmbDrivingLicenseClass.SelectedValue == null ||
                !int.TryParse(cmbDrivingLicenseClass.SelectedValue.ToString(), out licenseClassId))
            {
                MessageBox.Show("Please select a valid driving license class.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
