namespace Presentation.Forms
{
    partial class DrivingLicenseApplicationInformationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrivingLicenseApplicationInformationForm));
            this.drivingLicenseApplicationInformationControl1 = new Presentation.Controls.DrivingLicenseApplicationInformationControl();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // drivingLicenseApplicationInformationControl1
            // 
            this.drivingLicenseApplicationInformationControl1.AutoSize = true;
            this.drivingLicenseApplicationInformationControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.drivingLicenseApplicationInformationControl1.Location = new System.Drawing.Point(14, 14);
            this.drivingLicenseApplicationInformationControl1.Name = "drivingLicenseApplicationInformationControl1";
            this.drivingLicenseApplicationInformationControl1.Size = new System.Drawing.Size(804, 411);
            this.drivingLicenseApplicationInformationControl1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(347, 429);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(139, 38);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DrivingLicenseApplicationInformationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(831, 476);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.drivingLicenseApplicationInformationControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DrivingLicenseApplicationInformationForm";
            this.ShowIcon = false;
            this.Text = "Driving License Application Details";
            this.Load += new System.EventHandler(this.DrivingLicenseApplicationInformationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.DrivingLicenseApplicationInformationControl drivingLicenseApplicationInformationControl1;
        private System.Windows.Forms.Button btnClose;
    }
}