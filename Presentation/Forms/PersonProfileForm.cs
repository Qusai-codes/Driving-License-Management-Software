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

namespace Presentation
{
    public enum FormMode
    {
        Add,
        Edit
    }

    public partial class PersonProfileForm : Form
    {
        private FormMode _mode;
        private int _personId;

        public event EventHandler<PersonEventArgs> PersonAdded;
        

        public PersonProfileForm(FormMode mode, int personId = -1)
        {
            InitializeComponent();
            //personDetailsControl.ImageSelected += 
            _mode = mode;
            _personId = personId;
        }

        private void PersonProfileForm_Load(object sender, EventArgs e)
        {
            if (_mode == FormMode.Add)
            {
                lblTitle.Text = "Add New Person";
                personDetailsControl.RemovePersonImageLinkVisible = false;
                // TODO: create a method to set up the countries combo box.
            }
            else if (_mode == FormMode.Edit)
            {
                lblTitle.Text = "Edit Person";
               
            }
        }

        private void PersonDetailsControl_ImageSelected(object sender, ImageSelectedEventArgs e)
        {
            // TODO: implement the GUID + copy logic
        }
    }
}
