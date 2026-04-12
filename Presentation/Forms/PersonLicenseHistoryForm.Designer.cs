namespace Presentation.Forms
{
    partial class PersonLicenseHistoryForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.personDetailsWithFilterControl1 = new Presentation.Controls.PersonDetailsWithFilterControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(520, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // personDetailsWithFilterControl1
            // 
            this.personDetailsWithFilterControl1.AutoSize = true;
            this.personDetailsWithFilterControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.personDetailsWithFilterControl1.Location = new System.Drawing.Point(47, 104);
            this.personDetailsWithFilterControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.personDetailsWithFilterControl1.Name = "personDetailsWithFilterControl1";
            this.personDetailsWithFilterControl1.PersonId = -1;
            this.personDetailsWithFilterControl1.Size = new System.Drawing.Size(879, 417);
            this.personDetailsWithFilterControl1.TabIndex = 1;
            // 
            // PersonLicenseHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 596);
            this.Controls.Add(this.personDetailsWithFilterControl1);
            this.Controls.Add(this.label1);
            this.Name = "PersonLicenseHistoryForm";
            this.Text = "PersonLicenseHistoryForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Controls.PersonDetailsWithFilterControl personDetailsWithFilterControl1;
    }
}