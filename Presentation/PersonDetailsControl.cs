using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class PersonDetailsControl : UserControl
    {
        private int _minimumAge = 18;

        public PersonDetailsControl()
        {
            InitializeComponent();

            DefaultMaleImage = Properties.Resources.person_default_image_male;
            DefaultFemaleImage = Properties.Resources.person_default_image_female;
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
            // TODO: Implement close functionality
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // TODO: Implement save functionality
        }

        private void llbSetPersonImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // TODO: Implement set person image functionality
        }

        private void llbRemovePersonImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // TODO: Implement remove person image functionality
        }

        private void txtNationalNo_Validated(object sender, EventArgs e)
        {
            // TODO: Implement national number validation functionality

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
    }
}
