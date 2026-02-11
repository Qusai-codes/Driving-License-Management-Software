using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Contracts.DTOs;

namespace Presentation
{
    public partial class PersonDetailsForm : Form
    {
        private PersonDto _person;
        public PersonDetailsForm(PersonDto person)
        {
            InitializeComponent();
            _person = person;
            // TODO: call the method to return person information from the business layer
        }

        private void llbEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PersonProfileForm form = new PersonProfileForm(FormMode.Edit, _person.PersonId);
            form.ShowDialog();
        }
    }
}
