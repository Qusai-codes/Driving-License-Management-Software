namespace Presentation.Forms
{
    partial class IssueDrivingLicenseForFirstTimeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssueDrivingLicenseForFirstTimeForm));
            this.drivingLicenseApplicationInformationControl1 = new Presentation.Controls.DrivingLicenseApplicationInformationControl();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnIssueDrivingLicense = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // drivingLicenseApplicationInformationControl1
            // 
            this.drivingLicenseApplicationInformationControl1.AutoSize = true;
            this.drivingLicenseApplicationInformationControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.drivingLicenseApplicationInformationControl1.Location = new System.Drawing.Point(12, 29);
            this.drivingLicenseApplicationInformationControl1.Name = "drivingLicenseApplicationInformationControl1";
            this.drivingLicenseApplicationInformationControl1.Size = new System.Drawing.Size(804, 411);
            this.drivingLicenseApplicationInformationControl1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 454);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Notes:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(88, 454);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(26, 26);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(130, 454);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(686, 124);
            this.textBox1.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(532, 584);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(139, 38);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnIssueDrivingLicense
            // 
            this.btnIssueDrivingLicense.AutoSize = true;
            this.btnIssueDrivingLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIssueDrivingLicense.Image = ((System.Drawing.Image)(resources.GetObject("btnIssueDrivingLicense.Image")));
            this.btnIssueDrivingLicense.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIssueDrivingLicense.Location = new System.Drawing.Point(677, 584);
            this.btnIssueDrivingLicense.Name = "btnIssueDrivingLicense";
            this.btnIssueDrivingLicense.Size = new System.Drawing.Size(139, 38);
            this.btnIssueDrivingLicense.TabIndex = 5;
            this.btnIssueDrivingLicense.Text = "Issue";
            this.btnIssueDrivingLicense.UseVisualStyleBackColor = true;
            this.btnIssueDrivingLicense.Click += new System.EventHandler(this.btnIssueDrivingLicense_Click);
            // 
            // IssueDrivingLicenseForFirstTimeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 639);
            this.Controls.Add(this.btnIssueDrivingLicense);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.drivingLicenseApplicationInformationControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IssueDrivingLicenseForFirstTimeForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Issue Driving License For The First Time";
            this.Load += new System.EventHandler(this.IssueDrivingLicenseForFirstTimeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.DrivingLicenseApplicationInformationControl drivingLicenseApplicationInformationControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnIssueDrivingLicense;
    }
}