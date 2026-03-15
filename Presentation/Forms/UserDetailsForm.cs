using Business;
using Presentation.Controls;
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
    public partial class UserDetailsForm : Form
    {
        private int _userId;
        public UserDetailsForm(int userId)
        {
            InitializeComponent();
            
            _userId = userId;
            LoadUserData();
        }

        private void UserDetailsForm_Load(object sender, EventArgs e)
        {
        }

        private void LoadUserData()
        {
            userDetailsControl1.UserId = _userId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
