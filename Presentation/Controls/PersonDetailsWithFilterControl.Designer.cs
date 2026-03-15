namespace Presentation.Controls
{
    partial class PersonDetailsWithFilterControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonDetailsWithFilterControl));
            this.grpFilterPerson = new System.Windows.Forms.GroupBox();
            this.btnFindPerson = new System.Windows.Forms.Button();
            this.btnAddPerson = new System.Windows.Forms.Button();
            this.txtFilterValue = new System.Windows.Forms.TextBox();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.personDetailsViewControl1 = new Presentation.Controls.PersonDetailsViewControl();
            this.grpFilterPerson.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpFilterPerson
            // 
            this.grpFilterPerson.Controls.Add(this.btnFindPerson);
            this.grpFilterPerson.Controls.Add(this.btnAddPerson);
            this.grpFilterPerson.Controls.Add(this.txtFilterValue);
            this.grpFilterPerson.Controls.Add(this.cmbFilter);
            this.grpFilterPerson.Controls.Add(this.label2);
            this.grpFilterPerson.Location = new System.Drawing.Point(3, 3);
            this.grpFilterPerson.Name = "grpFilterPerson";
            this.grpFilterPerson.Size = new System.Drawing.Size(653, 62);
            this.grpFilterPerson.TabIndex = 3;
            this.grpFilterPerson.TabStop = false;
            this.grpFilterPerson.Text = "Filter";
            // 
            // btnFindPerson
            // 
            this.btnFindPerson.AutoSize = true;
            this.btnFindPerson.ImageIndex = 1;
            this.btnFindPerson.ImageList = this.imageList1;
            this.btnFindPerson.Location = new System.Drawing.Point(432, 13);
            this.btnFindPerson.Name = "btnFindPerson";
            this.btnFindPerson.Size = new System.Drawing.Size(40, 38);
            this.btnFindPerson.TabIndex = 4;
            this.btnFindPerson.UseVisualStyleBackColor = true;
            this.btnFindPerson.Click += new System.EventHandler(this.btnFindPerson_Click);
            // 
            // btnAddPerson
            // 
            this.btnAddPerson.AutoSize = true;
            this.btnAddPerson.ImageIndex = 0;
            this.btnAddPerson.ImageList = this.imageList1;
            this.btnAddPerson.Location = new System.Drawing.Point(478, 13);
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(40, 38);
            this.btnAddPerson.TabIndex = 3;
            this.btnAddPerson.UseVisualStyleBackColor = true;
            this.btnAddPerson.Click += new System.EventHandler(this.btnAddPerson_Click);
            // 
            // txtFilterValue
            // 
            this.txtFilterValue.Location = new System.Drawing.Point(251, 23);
            this.txtFilterValue.Name = "txtFilterValue";
            this.txtFilterValue.Size = new System.Drawing.Size(162, 20);
            this.txtFilterValue.TabIndex = 2;
            // 
            // cmbFilter
            // 
            this.cmbFilter.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Location = new System.Drawing.Point(80, 23);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(162, 21);
            this.cmbFilter.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Find By:";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "administrator(6).png");
            this.imageList1.Images.SetKeyName(1, "administrator(7).png");
            // 
            // personDetailsViewControl1
            // 
            this.personDetailsViewControl1.Address = "[?????]";
            this.personDetailsViewControl1.Country = "[?????]";
            this.personDetailsViewControl1.DateOfBirth = new System.DateTime(((long)(0)));
            this.personDetailsViewControl1.Email = "[?????]";
            this.personDetailsViewControl1.FullName = "[?????]";
            this.personDetailsViewControl1.Gender = "[?????]";
            this.personDetailsViewControl1.Location = new System.Drawing.Point(3, 70);
            this.personDetailsViewControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.personDetailsViewControl1.Name = "personDetailsViewControl1";
            this.personDetailsViewControl1.NationalNo = "[?????]";
            this.personDetailsViewControl1.PersonId = -1;
            this.personDetailsViewControl1.Phone = "[?????]";
            this.personDetailsViewControl1.Size = new System.Drawing.Size(653, 267);
            this.personDetailsViewControl1.TabIndex = 0;
            // 
            // PersonDetailsWithFilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.grpFilterPerson);
            this.Controls.Add(this.personDetailsViewControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "PersonDetailsWithFilterControl";
            this.Size = new System.Drawing.Size(659, 339);
            this.grpFilterPerson.ResumeLayout(false);
            this.grpFilterPerson.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PersonDetailsViewControl personDetailsViewControl1;
        private System.Windows.Forms.GroupBox grpFilterPerson;
        private System.Windows.Forms.Button btnFindPerson;
        private System.Windows.Forms.Button btnAddPerson;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList1;
    }
}
