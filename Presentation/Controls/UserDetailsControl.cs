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
    public partial class UserDetailsControl : UserControl
    {
        public UserDetailsControl()
        {
            InitializeComponent();
        }

        public int UserId
        {
            set 
            {
                LoadUserDate(value);
            }
        }

        private void LoadUserDate(int userId)
        {
            var user = User.Find(userId);
            if (user != null)
            {
                lblUserId.Text = user.UserId.ToString();
                lblUsername.Text = user.UserName;
                lblIsActive.Text = user.IsActive ? "Yes" : "No";

                // To load person data.
                personDetailsViewControl1.PersonId = user.PersonId;
            }
        }

    }
}
