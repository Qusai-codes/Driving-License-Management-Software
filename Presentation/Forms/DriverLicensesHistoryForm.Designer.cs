namespace Presentation.Forms
{
    partial class DriverLicensesHistoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DriverLicensesHistoryForm));
            this.personDetailsWithFilterControl1 = new Presentation.Controls.PersonDetailsWithFilterControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.driverLicensesControl1 = new Presentation.Controls.DriverLicensesControl();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // personDetailsWithFilterControl1
            // 
            this.personDetailsWithFilterControl1.AutoSize = true;
            this.personDetailsWithFilterControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.personDetailsWithFilterControl1.Location = new System.Drawing.Point(251, 106);
            this.personDetailsWithFilterControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.personDetailsWithFilterControl1.Name = "personDetailsWithFilterControl1";
            this.personDetailsWithFilterControl1.PersonId = -1;
            this.personDetailsWithFilterControl1.Size = new System.Drawing.Size(879, 417);
            this.personDetailsWithFilterControl1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(15, 272);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(215, 201);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(385, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 38);
            this.label1.TabIndex = 2;
            this.label1.Text = "License History";
            // 
            // driverLicensesControl1
            // 
            this.driverLicensesControl1.AutoSize = true;
            this.driverLicensesControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.driverLicensesControl1.Location = new System.Drawing.Point(24, 528);
            this.driverLicensesControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.driverLicensesControl1.Name = "driverLicensesControl1";
            this.driverLicensesControl1.Size = new System.Drawing.Size(1106, 339);
            this.driverLicensesControl1.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(985, 874);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(144, 47);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DriverLicensesHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1177, 934);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.driverLicensesControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.personDetailsWithFilterControl1);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DriverLicensesHistoryForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "License History";
            this.Load += new System.EventHandler(this.DriverLicensesHistoryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private Controls.PersonDetailsWithFilterControl personDetailsWithFilterControl1;
        private System.Windows.Forms.Label label1;
        private Controls.DriverLicensesControl driverLicensesControl1;
        private System.Windows.Forms.Button btnClose;
    }
}