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
    public partial class AddUserForm : Form
    {
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpPersonInfo;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpLoginInfo;
        }

        private void btnFindPerson_Click(object sender, EventArgs e)
        {
            //Person person = Person.Find(_personId);
            // get the current selected filter option


            // get the value of the text box 

            // call the person class methods to check the person existance
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {

        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
