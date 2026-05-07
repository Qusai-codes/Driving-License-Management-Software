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

namespace Presentation.Controls
{
    public partial class PersonDetailsWithFilterControl : UserControl
    {
        private int _personId = -1;

        // Notify parent when a person is selected/found.
        public event EventHandler<PersonSavedEventArgs> PersonSelected;

        public PersonDetailsWithFilterControl()
        {
            InitializeComponent();
            SetupFilterCombo();
        }

        public GroupBox PersonFilter
        {
            get { return grpFilterPerson; }
        }

        public int PersonId
        {
            get { return _personId; }
            set
            {
                _personId = value;

                if (_personId > 0)
                {
                    personDetailsViewControl1.PersonId = _personId;
                }
                else
                {
                    personDetailsViewControl1.ResetView();
                }
            }
        }

        public bool EnableEditingOfPersonInfo
        {
            set { personDetailsViewControl1.EditPersonInfoLinkLabel.Enabled = value; }
        }

        private void SetupFilterCombo()
        {
            cmbFilter.Items.Clear();
            cmbFilter.Items.AddRange(new object[] { "National No.", "Person ID" });
            cmbFilter.SelectedIndex = 0;
        }

        private void btnFindPerson_Click(object sender, EventArgs e)
        {
            string selectedFilter = cmbFilter.SelectedItem?.ToString();
            string filterValue = txtFilterValue.Text.Trim();

            if (string.IsNullOrWhiteSpace(filterValue))
            {
                MessageBox.Show("Please enter a value to search.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Person person = null;

            if (selectedFilter == "National No.")
            {
                person = Person.Find(filterValue);

                if (person == null)
                {
                    MessageBox.Show($"No Person with National No. = {filterValue}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (selectedFilter == "Person ID")
            {
                int personId;
                if (!int.TryParse(filterValue, out personId))
                {
                    MessageBox.Show("Person ID must be numeric.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                person = Person.Find(personId);
                if (person == null)
                {
                    MessageBox.Show($"No Person with Person ID = {filterValue}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (person == null)
            {
                _personId = -1;
                personDetailsViewControl1.ResetView();
                return;
            }
            _personId = person.PersonId;
            RaisePersonSelected(_personId);
            PersonId = _personId;
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            using (PersonProfileForm personProfileForm = new PersonProfileForm())
            {
                EventHandler<PersonSavedEventArgs> onPersonSaved = (s, args) =>
                {
                    _personId = args.PersonId;
                };

                personProfileForm.PersonSaved += onPersonSaved;
                personProfileForm.ShowDialog();
                personProfileForm.PersonSaved -= onPersonSaved;
            }

            if (_personId != -1)
            {
                RaisePersonSelected(_personId);
                PersonId = _personId;
            }
        }

        private void RaisePersonSelected(int personId)
        {
            PersonSelected?.Invoke(this, new PersonSavedEventArgs(personId));
        }
    }
}
