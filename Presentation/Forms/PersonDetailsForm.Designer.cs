namespace Presentation
{
    partial class PersonDetailsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonDetailsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.personDetailsViewControl1 = new Presentation.Controls.PersonDetailsViewControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(362, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Person Details";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "close(2).png");
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.ImageIndex = 0;
            this.btnClose.ImageList = this.imageList1;
            this.btnClose.Location = new System.Drawing.Point(752, 422);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(145, 48);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // personDetailsViewControl1
            // 
            this.personDetailsViewControl1.Address = "[?????]";
            this.personDetailsViewControl1.Country = "[?????]";
            this.personDetailsViewControl1.DateOfBirth = new System.DateTime(((long)(0)));
            this.personDetailsViewControl1.Email = "[?????]";
            this.personDetailsViewControl1.FullName = "[?????]";
            this.personDetailsViewControl1.Gender = "[?????]";
            this.personDetailsViewControl1.Location = new System.Drawing.Point(26, 89);
            this.personDetailsViewControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.personDetailsViewControl1.Name = "personDetailsViewControl1";
            this.personDetailsViewControl1.NationalNo = "[?????]";
            this.personDetailsViewControl1.PersonId = -1;
            this.personDetailsViewControl1.Phone = "[?????]";
            this.personDetailsViewControl1.Size = new System.Drawing.Size(871, 329);
            this.personDetailsViewControl1.TabIndex = 3;
            // 
            // PersonDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 498);
            this.Controls.Add(this.personDetailsViewControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PersonDetailsForm";
            this.Text = "Person Details";
            this.Load += new System.EventHandler(this.PersonDetailsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ImageList imageList1;
        private Controls.PersonDetailsViewControl personDetailsViewControl1;
    }
}