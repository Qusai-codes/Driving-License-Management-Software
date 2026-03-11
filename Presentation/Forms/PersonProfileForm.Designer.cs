namespace Presentation
{
    partial class PersonProfileForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonProfileForm));
            this.label1 = new System.Windows.Forms.Label();
            this.lblPersonId = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.personDetailsControl = new Presentation.PersonDetailsControl();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Person ID:";
            // 
            // lblPersonId
            // 
            this.lblPersonId.AutoSize = true;
            this.lblPersonId.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPersonId.Location = new System.Drawing.Point(73, 57);
            this.lblPersonId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPersonId.Name = "lblPersonId";
            this.lblPersonId.Size = new System.Drawing.Size(30, 13);
            this.lblPersonId.TabIndex = 2;
            this.lblPersonId.Text = "N/A";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTitle.Location = new System.Drawing.Point(268, 30);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(195, 29);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Add New Person";
            // 
            // personDetailsControl
            // 
            this.personDetailsControl.Address = "";
            this.personDetailsControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.personDetailsControl.DateOfBirth = new System.DateTime(2008, 3, 11, 0, 0, 0, 0);
            this.personDetailsControl.DefaultFemaleImage = ((System.Drawing.Image)(resources.GetObject("personDetailsControl.DefaultFemaleImage")));
            this.personDetailsControl.DefaultMaleImage = ((System.Drawing.Image)(resources.GetObject("personDetailsControl.DefaultMaleImage")));
            this.personDetailsControl.Email = "";
            this.personDetailsControl.FirstName = "";
            this.personDetailsControl.Gender = ((byte)(0));
            this.personDetailsControl.ImagePath = "";
            this.personDetailsControl.LastName = "";
            this.personDetailsControl.Location = new System.Drawing.Point(9, 85);
            this.personDetailsControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.personDetailsControl.MinimumAge = 18;
            this.personDetailsControl.Name = "personDetailsControl";
            this.personDetailsControl.NationalNo = "";
            this.personDetailsControl.Phone = "";
            this.personDetailsControl.RemovePersonImageLinkVisible = true;
            this.personDetailsControl.SecondName = "";
            this.personDetailsControl.Size = new System.Drawing.Size(700, 311);
            this.personDetailsControl.TabIndex = 0;
            this.personDetailsControl.ThirdName = "";
            this.personDetailsControl.Load += new System.EventHandler(this.personDetailsControl_Load);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // PersonProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 405);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblPersonId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.personDetailsControl);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "PersonProfileForm";
            this.Text = "Add / Edit Person Info.";
            this.Load += new System.EventHandler(this.PersonProfileForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PersonDetailsControl personDetailsControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPersonId;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}