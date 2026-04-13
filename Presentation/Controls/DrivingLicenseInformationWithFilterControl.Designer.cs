namespace Presentation.Controls
{
    partial class DrivingLicenseInformationWithFilterControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrivingLicenseInformationWithFilterControl));
            this.grbFilter = new System.Windows.Forms.GroupBox();
            this.btnFindLicense = new System.Windows.Forms.Button();
            this.txtDrivingLicenseId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.drivingLicenseInformationControl1 = new Presentation.Controls.DrivingLicenseInformationControl();
            this.grbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbFilter
            // 
            this.grbFilter.Controls.Add(this.btnFindLicense);
            this.grbFilter.Controls.Add(this.txtDrivingLicenseId);
            this.grbFilter.Controls.Add(this.label1);
            this.grbFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbFilter.Location = new System.Drawing.Point(3, 3);
            this.grbFilter.Name = "grbFilter";
            this.grbFilter.Size = new System.Drawing.Size(563, 100);
            this.grbFilter.TabIndex = 1;
            this.grbFilter.TabStop = false;
            this.grbFilter.Text = "Filter";
            // 
            // btnFindLicense
            // 
            this.btnFindLicense.Image = ((System.Drawing.Image)(resources.GetObject("btnFindLicense.Image")));
            this.btnFindLicense.Location = new System.Drawing.Point(487, 29);
            this.btnFindLicense.Name = "btnFindLicense";
            this.btnFindLicense.Size = new System.Drawing.Size(64, 55);
            this.btnFindLicense.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnFindLicense, "Find License");
            this.btnFindLicense.UseVisualStyleBackColor = true;
            this.btnFindLicense.Click += new System.EventHandler(this.btnFindLicense_Click);
            // 
            // txtDrivingLicenseId
            // 
            this.txtDrivingLicenseId.Location = new System.Drawing.Point(137, 46);
            this.txtDrivingLicenseId.Name = "txtDrivingLicenseId";
            this.txtDrivingLicenseId.Size = new System.Drawing.Size(311, 24);
            this.txtDrivingLicenseId.TabIndex = 1;
            this.txtDrivingLicenseId.TextChanged += new System.EventHandler(this.txtDrivingLicenseId_TextChanged);
            this.txtDrivingLicenseId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDrivingLicenseId_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "License ID:";
            // 
            // drivingLicenseInformationControl1
            // 
            this.drivingLicenseInformationControl1.AutoSize = true;
            this.drivingLicenseInformationControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.drivingLicenseInformationControl1.Location = new System.Drawing.Point(3, 110);
            this.drivingLicenseInformationControl1.Margin = new System.Windows.Forms.Padding(4);
            this.drivingLicenseInformationControl1.Name = "drivingLicenseInformationControl1";
            this.drivingLicenseInformationControl1.Size = new System.Drawing.Size(911, 371);
            this.drivingLicenseInformationControl1.TabIndex = 0;
            // 
            // DrivingLicenseInformationWithFilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.grbFilter);
            this.Controls.Add(this.drivingLicenseInformationControl1);
            this.Name = "DrivingLicenseInformationWithFilterControl";
            this.Size = new System.Drawing.Size(918, 485);
            this.grbFilter.ResumeLayout(false);
            this.grbFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DrivingLicenseInformationControl drivingLicenseInformationControl1;
        private System.Windows.Forms.GroupBox grbFilter;
        private System.Windows.Forms.Button btnFindLicense;
        private System.Windows.Forms.TextBox txtDrivingLicenseId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
