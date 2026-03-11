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

        public string UserId
        {
            get { return lblUserId.Text; }
            set { lblUserId.Text = value; }
        }

        public string UserName
        {
            get { return lblUsername.Text; }
            set { lblUsername.Text = value; }
        }

        public string IsActive
        {
            get { return lblIsActive.Text; }
            set { lblIsActive.Text = value; }
        }

        public PersonDetailsViewControl PersonDetailsControl
        {
            get { return personDetailsViewControl1; }
        }

    }
}
