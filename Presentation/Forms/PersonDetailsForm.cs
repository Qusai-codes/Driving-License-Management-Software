using Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Business;

namespace Presentation
{
    public partial class PersonDetailsForm : Form
    {
        private int _personId;

        public PersonDetailsForm(int personId)
        {
            InitializeComponent();
            _personId = personId;
            personDetailsViewControl1.PersonId = personId;

        }

        private void PersonDetailsForm_Load(object sender, EventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
