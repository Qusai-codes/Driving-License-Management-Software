
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Presentation.Events;

namespace Presentation
{
    public partial class PersonDetailsControl : UserControl
    {
        private int _minimumAge = 18;
        private string _imagePath = "";

        public event EventHandler SaveButtonClicked;
        public event EventHandler ClosebuttonClicked;
        public event EventHandler SetImageClicked;
        public event EventHandler RemoveImageClicked;
        public event EventHandler NationalNumberValidated;
        public event EventHandler<ImageSelectedEventArgs> ImageSelected;

        public PersonDetailsControl()
        {
            InitializeComponent();

            DefaultMaleImage = Properties.Resources.person_default_image_male;
            DefaultFemaleImage = Properties.Resources.person_default_image_female;
            picPersonImage.Image = DefaultMaleImage;
            UpdateDateLimits();
        }

        public Image DefaultMaleImage { get; set; }
        public Image DefaultFemaleImage { get; set; }

        public bool RemovePersonImageLinkVisible
        {
            get { return llbRemovePersonImage.Visible; }
            set { llbRemovePersonImage.Visible = value; }
        }

        public int MinimumAge
        {
            get { return _minimumAge; }
            set
            {
                _minimumAge = value;
                UpdateDateLimits();
            }
        }

        public TextBox NationalNumberTextBox
        {
            get { return txtNationalNo; }
        }

        public PictureBox PersonImage
        {
            get { return picPersonImage; }
        }

        public ErrorProvider ErrorProvider
        {
            get { return errorProvider1; }
        }

        public string FirstName
        {
            get { return txtFirstName.Text; }
            set { txtFirstName.Text = value; }
        }
        public string SecondName 
        {
            get { return txtSecondName.Text; }
            set { txtSecondName.Text = value; }
        }

        public string ThirdName
        {
            get { return txtThirdName.Text; }
            set { txtThirdName.Text = value; }
        }
        public string LastName
        {
            get { return txtLastName.Text; }
            set { txtLastName.Text = value; }
        }

        public string NationalNo
        {
            get { return txtNationalNo.Text; }
            set { txtNationalNo.Text = value; }
        }
        public DateTime DateOfBirth
        {
            get { return dtpDateOfBirth.Value; }
            set { dtpDateOfBirth.Value = value; }
        }
        public byte Gender
        {
            get
            {
                if (rdoMale.Checked)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                if (value == 0)
                {
                    rdoMale.Checked = true;
                }
                else
                {
                    rdoFemale.Checked = true;
                }
            }
        }
        public string Address
        {
            get { return txtAddress.Text; }
            set { txtAddress.Text = value; }
        }
        public string Phone
        {
            get { return txtPhone.Text; }
            set { txtPhone.Text = value; }
        }
        public string Email
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }
        [Browsable(false)]
        [Bindable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedCountryId
        {
            get
            {
                if (cmbCountry == null ||
                    cmbCountry.SelectedValue == null ||
                    cmbCountry.SelectedValue == DBNull.Value)
                {
                    return -1;
                }

                int id;
                return int.TryParse(cmbCountry.SelectedValue.ToString(), out id) ? id : -1;
            }
            set
            {
                if (cmbCountry != null && cmbCountry.DataSource != null)
                {
                    cmbCountry.SelectedValue = value;
                }
            }
        }
        public string SelectedCountryName
        {
            get { return cmbCountry.Text; }
        }
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

        public void LoadCountries(DataTable countries)
        {
            cmbCountry.DataSource = countries;
            cmbCountry.DisplayMember = "CountryName";
            cmbCountry.ValueMember = "CountryId";
        }

        private void UpdateDateLimits()
        {
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-_minimumAge);
            dtpDateOfBirth.MinDate = new DateTime(1900, 1, 1);
        }

        public DateTimePicker BirthDatePicker
        {
            get { return dtpDateOfBirth; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ClosebuttonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void llbSetPersonImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                dlg.Title = "Select a Person Image";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ImageSelected?.Invoke(
                        this,
                        new ImageSelectedEventArgs(dlg.FileName));
                }
            }

        }

        private void llbRemovePersonImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RemoveImageClicked?.Invoke(this, EventArgs.Empty);
        }

        private void txtNationalNo_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "National Number is required.");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, "");
            }

            // Raise event for parent form to do duplicate check
            NationalNumberValidated?.Invoke(this, EventArgs.Empty);

        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            errorProvider1.SetError(txtNationalNo, "");
        }

        private void Gender_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMale.Checked)
            {
                picPersonImage.Image = DefaultMaleImage;
            }
            else if (rdoFemale.Checked)
            {
                picPersonImage.Image = DefaultFemaleImage;
            }
        }

        private void txtEmail_Validated(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                // Optional field -> no error
                errorProvider1.SetError(txtEmail, "");
            }
            else if (!IsValidEmail(email))
            {
                // User entered something -> validate it
                errorProvider1.SetError(txtEmail, "Invalid email format");
            }
            else
            {
                // Valid email
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }

        private void txtFirstName_Validated(object sender, EventArgs e)
        {
            ValidateRequiredField(txtFirstName, "First name cannot be empty.");
        }

        private void txtSecondName_Validated(object sender, EventArgs e)
        {
            ValidateRequiredField(txtSecondName, "Second name cannot be empty.");
        }

        private void txtLastName_Validated(object sender, EventArgs e)
        {
            ValidateRequiredField(txtLastName, "Last name cannot be empty.");
        }

        private void txtPhone_Validated(object sender, EventArgs e)
        {
            ValidateRequiredField(txtPhone, "Phone number cannot be empty.");
        }

        private void txtAddress_Validated(object sender, EventArgs e)
        {
            ValidateRequiredField(txtAddress, "Address cannot be empty.");
        }

        private void ValidateRequiredField(TextBox textBox, string message)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                errorProvider1.SetError(textBox, message);
            }
            else
            {
                errorProvider1.SetError(textBox, "");
            }
        }

    }
}
