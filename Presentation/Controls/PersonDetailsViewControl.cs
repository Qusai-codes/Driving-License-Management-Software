using Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Controls
{
    public partial class PersonDetailsViewControl : UserControl
    {
        public event EventHandler EditPersonInfoClicked;

        private DateTime _dateOfBirth;
        private readonly string _defaultTextPlaceholder = "[?????]";
        private Image _defaultPersonImage;

        public PersonDetailsViewControl()
        {
            InitializeComponent();
            _defaultPersonImage = picPersonImage.Image;
        }

        public int PersonId
        {
            get { return int.TryParse(lblPersonId.Text, out int id) ? id : -1; }
            set { lblPersonId.Text = value.ToString(); }
        }

        public string FullName
        {
            get { return lblFullName.Text; }
            set { lblFullName.Text = value; }
        }

        public string NationalNo
        {
            get { return lblNationalNo.Text; }
            set { lblNationalNo.Text = value; }
        }

        public string Gender
        {
            get { return lblGender.Text; }
            set 
            {
                lblGender.Text = value;
                string gender = value.ToLower();
                if (gender.Equals("male"))
                {
                    picGender.Image = Resources.male_icon;
                }
                else if (gender.Equals("female"))
                {
                    picGender.Image = Resources.female_icon;
                }
            }
        }

        public string Address
        {
            get { return lblAddress.Text; }
            set { lblAddress.Text = value; }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set 
            { 
                _dateOfBirth = value;
                lblDateOfBirth.Text = value.ToString("dd/MM/yyyy"); 
            }
        }

        public string Phone
        {
            get { return lblPhone.Text; }
            set { lblPhone.Text = value; }
        }

        public string Country
        {
            get { return lblCountry.Text; }
            set { lblCountry.Text = value; }
        }

        public string Email
        {
            get { return lblEmail.Text; }
            set { lblEmail.Text = value; }
        }

        public PictureBox PersonImage
        {
            get { return picPersonImage; }
        }

        public bool EnableEditingOfPersonInfo
        {
            set { llbEditPersonInfo.Enabled = value; }
        }

        public void ResetView()
        {
            lblPersonId.Text = _defaultTextPlaceholder;
            lblFullName.Text = _defaultTextPlaceholder;
            lblNationalNo.Text = _defaultTextPlaceholder;
            lblGender.Text = _defaultTextPlaceholder;
            lblEmail.Text = _defaultTextPlaceholder;
            lblAddress.Text = _defaultTextPlaceholder;
            lblDateOfBirth.Text = _defaultTextPlaceholder;
            lblPhone.Text = _defaultTextPlaceholder;
            lblCountry.Text = _defaultTextPlaceholder;

            _dateOfBirth = DateTime.MinValue;

            picGender.Image = null;
            picPersonImage.Image = _defaultPersonImage;

            llbEditPersonInfo.LinkVisited = false;
        }

        private void llbEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EditPersonInfoClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
