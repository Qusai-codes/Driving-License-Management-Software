namespace Presentation.Forms
{
    partial class ReplaceLostOrDamagedDrivingLicenseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplaceLostOrDamagedDrivingLicenseForm));
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.llbShowLicensesHistory = new System.Windows.Forms.LinkLabel();
            this.llbShowNewLicenseInfo = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnIssueReplacement = new System.Windows.Forms.Button();
            this.grpReplacementReason = new System.Windows.Forms.GroupBox();
            this.rdoLostLicense = new System.Windows.Forms.RadioButton();
            this.rdoDamagedLicense = new System.Windows.Forms.RadioButton();
            this.drivingLicenseInformationWithFilterControl1 = new Presentation.Controls.DrivingLicenseInformationWithFilterControl();
            this.grbApplicationInfo = new System.Windows.Forms.GroupBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblCreatedByUserName = new System.Windows.Forms.Label();
            this.lblOldLicenseId = new System.Windows.Forms.Label();
            this.lblReplacedLicenseId = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblApplicationFees = new System.Windows.Forms.Label();
            this.lblApplicationDate = new System.Windows.Forms.Label();
            this.lblLicenseReplacementApplicationId = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpReplacementReason.SuspendLayout();
            this.grbApplicationInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormTitle.ForeColor = System.Drawing.Color.DarkRed;
            this.lblFormTitle.Location = new System.Drawing.Point(235, 22);
            this.lblFormTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(478, 31);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "Replacement For Damaged License";
            // 
            // llbShowLicensesHistory
            // 
            this.llbShowLicensesHistory.AutoSize = true;
            this.llbShowLicensesHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llbShowLicensesHistory.Location = new System.Drawing.Point(92, 869);
            this.llbShowLicensesHistory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llbShowLicensesHistory.Name = "llbShowLicensesHistory";
            this.llbShowLicensesHistory.Size = new System.Drawing.Size(160, 18);
            this.llbShowLicensesHistory.TabIndex = 1;
            this.llbShowLicensesHistory.TabStop = true;
            this.llbShowLicensesHistory.Text = "Show Licenses History";
            this.llbShowLicensesHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbShowLicensesHistory_LinkClicked);
            // 
            // llbShowNewLicenseInfo
            // 
            this.llbShowNewLicenseInfo.AutoSize = true;
            this.llbShowNewLicenseInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llbShowNewLicenseInfo.Location = new System.Drawing.Point(273, 869);
            this.llbShowNewLicenseInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llbShowNewLicenseInfo.Name = "llbShowNewLicenseInfo";
            this.llbShowNewLicenseInfo.Size = new System.Drawing.Size(163, 18);
            this.llbShowNewLicenseInfo.TabIndex = 2;
            this.llbShowNewLicenseInfo.TabStop = true;
            this.llbShowNewLicenseInfo.Text = "Show New License Info";
            this.llbShowNewLicenseInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbShowNewLicenseInfo_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(529, 852);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(141, 47);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnIssueReplacement
            // 
            this.btnIssueReplacement.AutoSize = true;
            this.btnIssueReplacement.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnIssueReplacement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIssueReplacement.Image = ((System.Drawing.Image)(resources.GetObject("btnIssueReplacement.Image")));
            this.btnIssueReplacement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIssueReplacement.Location = new System.Drawing.Point(691, 852);
            this.btnIssueReplacement.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnIssueReplacement.Name = "btnIssueReplacement";
            this.btnIssueReplacement.Size = new System.Drawing.Size(260, 47);
            this.btnIssueReplacement.TabIndex = 4;
            this.btnIssueReplacement.Text = "Issue Replacement";
            this.btnIssueReplacement.UseVisualStyleBackColor = true;
            this.btnIssueReplacement.Click += new System.EventHandler(this.btnIssueReplacement_Click);
            // 
            // grpReplacementReason
            // 
            this.grpReplacementReason.Controls.Add(this.rdoLostLicense);
            this.grpReplacementReason.Controls.Add(this.rdoDamagedLicense);
            this.grpReplacementReason.Location = new System.Drawing.Point(727, 64);
            this.grpReplacementReason.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpReplacementReason.Name = "grpReplacementReason";
            this.grpReplacementReason.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpReplacementReason.Size = new System.Drawing.Size(224, 101);
            this.grpReplacementReason.TabIndex = 5;
            this.grpReplacementReason.TabStop = false;
            this.grpReplacementReason.Text = "Replacement For";
            // 
            // rdoLostLicense
            // 
            this.rdoLostLicense.AutoSize = true;
            this.rdoLostLicense.Location = new System.Drawing.Point(8, 52);
            this.rdoLostLicense.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoLostLicense.Name = "rdoLostLicense";
            this.rdoLostLicense.Size = new System.Drawing.Size(103, 20);
            this.rdoLostLicense.TabIndex = 1;
            this.rdoLostLicense.Text = "Lost License";
            this.rdoLostLicense.UseVisualStyleBackColor = true;
            this.rdoLostLicense.CheckedChanged += new System.EventHandler(this.rdoLostLicense_CheckedChanged);
            // 
            // rdoDamagedLicense
            // 
            this.rdoDamagedLicense.AutoSize = true;
            this.rdoDamagedLicense.Checked = true;
            this.rdoDamagedLicense.Location = new System.Drawing.Point(8, 23);
            this.rdoDamagedLicense.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoDamagedLicense.Name = "rdoDamagedLicense";
            this.rdoDamagedLicense.Size = new System.Drawing.Size(132, 20);
            this.rdoDamagedLicense.TabIndex = 0;
            this.rdoDamagedLicense.TabStop = true;
            this.rdoDamagedLicense.Text = "Damaged Licese";
            this.rdoDamagedLicense.UseVisualStyleBackColor = true;
            this.rdoDamagedLicense.CheckedChanged += new System.EventHandler(this.rdoDamagedLicense_CheckedChanged);
            // 
            // drivingLicenseInformationWithFilterControl1
            // 
            this.drivingLicenseInformationWithFilterControl1.AutoSize = true;
            this.drivingLicenseInformationWithFilterControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.drivingLicenseInformationWithFilterControl1.Location = new System.Drawing.Point(33, 171);
            this.drivingLicenseInformationWithFilterControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.drivingLicenseInformationWithFilterControl1.Name = "drivingLicenseInformationWithFilterControl1";
            this.drivingLicenseInformationWithFilterControl1.Size = new System.Drawing.Size(918, 485);
            this.drivingLicenseInformationWithFilterControl1.TabIndex = 6;
            // 
            // grbApplicationInfo
            // 
            this.grbApplicationInfo.Controls.Add(this.pictureBox6);
            this.grbApplicationInfo.Controls.Add(this.pictureBox5);
            this.grbApplicationInfo.Controls.Add(this.pictureBox4);
            this.grbApplicationInfo.Controls.Add(this.pictureBox3);
            this.grbApplicationInfo.Controls.Add(this.pictureBox2);
            this.grbApplicationInfo.Controls.Add(this.pictureBox1);
            this.grbApplicationInfo.Controls.Add(this.lblCreatedByUserName);
            this.grbApplicationInfo.Controls.Add(this.lblOldLicenseId);
            this.grbApplicationInfo.Controls.Add(this.lblReplacedLicenseId);
            this.grbApplicationInfo.Controls.Add(this.label7);
            this.grbApplicationInfo.Controls.Add(this.label8);
            this.grbApplicationInfo.Controls.Add(this.label9);
            this.grbApplicationInfo.Controls.Add(this.lblApplicationFees);
            this.grbApplicationInfo.Controls.Add(this.lblApplicationDate);
            this.grbApplicationInfo.Controls.Add(this.lblLicenseReplacementApplicationId);
            this.grbApplicationInfo.Controls.Add(this.label3);
            this.grbApplicationInfo.Controls.Add(this.label2);
            this.grbApplicationInfo.Controls.Add(this.label1);
            this.grbApplicationInfo.Location = new System.Drawing.Point(35, 662);
            this.grbApplicationInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbApplicationInfo.Name = "grbApplicationInfo";
            this.grbApplicationInfo.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbApplicationInfo.Size = new System.Drawing.Size(917, 169);
            this.grbApplicationInfo.TabIndex = 7;
            this.grbApplicationInfo.TabStop = false;
            this.grbApplicationInfo.Text = "Application Info for License Replacement";
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(620, 113);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(35, 32);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox6.TabIndex = 17;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(620, 71);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(35, 32);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 16;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(620, 32);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(35, 32);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 15;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(180, 113);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(35, 32);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(180, 71);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(35, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(180, 31);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // lblCreatedByUserName
            // 
            this.lblCreatedByUserName.AutoSize = true;
            this.lblCreatedByUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedByUserName.Location = new System.Drawing.Point(663, 122);
            this.lblCreatedByUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCreatedByUserName.Name = "lblCreatedByUserName";
            this.lblCreatedByUserName.Size = new System.Drawing.Size(61, 17);
            this.lblCreatedByUserName.TabIndex = 11;
            this.lblCreatedByUserName.Text = "label10";
            // 
            // lblOldLicenseId
            // 
            this.lblOldLicenseId.AutoSize = true;
            this.lblOldLicenseId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOldLicenseId.Location = new System.Drawing.Point(663, 80);
            this.lblOldLicenseId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOldLicenseId.Name = "lblOldLicenseId";
            this.lblOldLicenseId.Size = new System.Drawing.Size(45, 17);
            this.lblOldLicenseId.TabIndex = 10;
            this.lblOldLicenseId.Text = "[???]";
            // 
            // lblReplacedLicenseId
            // 
            this.lblReplacedLicenseId.AutoSize = true;
            this.lblReplacedLicenseId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReplacedLicenseId.Location = new System.Drawing.Point(663, 39);
            this.lblReplacedLicenseId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReplacedLicenseId.Name = "lblReplacedLicenseId";
            this.lblReplacedLicenseId.Size = new System.Drawing.Size(45, 17);
            this.lblReplacedLicenseId.TabIndex = 9;
            this.lblReplacedLicenseId.Text = "[???]";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(439, 122);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 17);
            this.label7.TabIndex = 8;
            this.label7.Text = "Created By:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(439, 80);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "Old License ID:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(439, 39);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(162, 17);
            this.label9.TabIndex = 6;
            this.label9.Text = "Replaced License ID:";
            // 
            // lblApplicationFees
            // 
            this.lblApplicationFees.AutoSize = true;
            this.lblApplicationFees.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationFees.Location = new System.Drawing.Point(223, 122);
            this.lblApplicationFees.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApplicationFees.Name = "lblApplicationFees";
            this.lblApplicationFees.Size = new System.Drawing.Size(52, 17);
            this.lblApplicationFees.TabIndex = 5;
            this.lblApplicationFees.Text = "label4";
            // 
            // lblApplicationDate
            // 
            this.lblApplicationDate.AutoSize = true;
            this.lblApplicationDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationDate.Location = new System.Drawing.Point(223, 80);
            this.lblApplicationDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApplicationDate.Name = "lblApplicationDate";
            this.lblApplicationDate.Size = new System.Drawing.Size(52, 17);
            this.lblApplicationDate.TabIndex = 4;
            this.lblApplicationDate.Text = "label5";
            // 
            // lblLicenseReplacementApplicationId
            // 
            this.lblLicenseReplacementApplicationId.AutoSize = true;
            this.lblLicenseReplacementApplicationId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicenseReplacementApplicationId.Location = new System.Drawing.Point(223, 39);
            this.lblLicenseReplacementApplicationId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLicenseReplacementApplicationId.Name = "lblLicenseReplacementApplicationId";
            this.lblLicenseReplacementApplicationId.Size = new System.Drawing.Size(45, 17);
            this.lblLicenseReplacementApplicationId.TabIndex = 3;
            this.lblLicenseReplacementApplicationId.Text = "[???]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 122);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Application Fees:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Application Date:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "L.R.Application ID:";
            // 
            // ReplaceLostOrDamagedDrivingLicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(976, 913);
            this.Controls.Add(this.grbApplicationInfo);
            this.Controls.Add(this.drivingLicenseInformationWithFilterControl1);
            this.Controls.Add(this.grpReplacementReason);
            this.Controls.Add(this.btnIssueReplacement);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.llbShowNewLicenseInfo);
            this.Controls.Add(this.llbShowLicensesHistory);
            this.Controls.Add(this.lblFormTitle);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ReplaceLostOrDamagedDrivingLicenseForm";
            this.Text = "[???]";
            this.Load += new System.EventHandler(this.ReplaceLostOrDamagedDrivingLicenseForm_Load);
            this.grpReplacementReason.ResumeLayout(false);
            this.grpReplacementReason.PerformLayout();
            this.grbApplicationInfo.ResumeLayout(false);
            this.grbApplicationInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.LinkLabel llbShowLicensesHistory;
        private System.Windows.Forms.LinkLabel llbShowNewLicenseInfo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnIssueReplacement;
        private System.Windows.Forms.GroupBox grpReplacementReason;
        private System.Windows.Forms.RadioButton rdoLostLicense;
        private System.Windows.Forms.RadioButton rdoDamagedLicense;
        private Controls.DrivingLicenseInformationWithFilterControl drivingLicenseInformationWithFilterControl1;
        private System.Windows.Forms.GroupBox grbApplicationInfo;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblCreatedByUserName;
        private System.Windows.Forms.Label lblOldLicenseId;
        private System.Windows.Forms.Label lblReplacedLicenseId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblApplicationFees;
        private System.Windows.Forms.Label lblApplicationDate;
        private System.Windows.Forms.Label lblLicenseReplacementApplicationId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}