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
            this.label1.Location = new System.Drawing.Point(12, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Person ID:";
            // 
            // lblPersonId
            // 
            this.lblPersonId.AutoSize = true;
            this.lblPersonId.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPersonId.Location = new System.Drawing.Point(97, 70);
            this.lblPersonId.Name = "lblPersonId";
            this.lblPersonId.Size = new System.Drawing.Size(33, 16);
            this.lblPersonId.TabIndex = 2;
            this.lblPersonId.Text = "N/A";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTitle.Location = new System.Drawing.Point(358, 37);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(241, 36);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Add New Person";
            // 
            // personDetailsControl
            // 
            this.personDetailsControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.personDetailsControl.DefaultFemaleImage = ((System.Drawing.Image)(resources.GetObject("personDetailsControl.DefaultFemaleImage")));
            this.personDetailsControl.DefaultMaleImage = ((System.Drawing.Image)(resources.GetObject("personDetailsControl.DefaultMaleImage")));
            this.personDetailsControl.Location = new System.Drawing.Point(12, 105);
            this.personDetailsControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.personDetailsControl.MinimumAge = 18;
            this.personDetailsControl.Name = "personDetailsControl";
            this.personDetailsControl.RemovePersonImageLinkVisible = true;
            this.personDetailsControl.Size = new System.Drawing.Size(933, 382);
            this.personDetailsControl.TabIndex = 0;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // PersonProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 498);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblPersonId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.personDetailsControl);
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