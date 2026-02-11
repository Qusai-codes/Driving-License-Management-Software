using Business;
using System;
using System.Collections;
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
    public partial class ManagePeopleForm : Form
    {
        

        public ManagePeopleForm()
        {
            InitializeComponent();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            PersonProfileForm personProfileForm = new PersonProfileForm(FormMode.Add);
            personProfileForm.ShowDialog();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ManagePeopleForm_Load(object sender, EventArgs e)
        {
            RefreshPeopleList();
        }

        private DataTable GetPeopleList()
        {
            var list = Person.GetAllPersons();
            var peopleDataTable = PersonDto.ToDataTable(list);
            return peopleDataTable;
        }

        private void RefreshPeopleList()
        {
            try
            {
                DataTable dt = GetPeopleList();
                dgvPeople.DataSource = dt;
                lblNumberOfRecords.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading people: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // TODO: implement the functionality.
        }
    }
}
