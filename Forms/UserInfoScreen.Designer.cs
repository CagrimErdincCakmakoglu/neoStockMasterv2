namespace neoStockMasterv2.Forms
{
    partial class UserInfoScreen
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
            components = new System.ComponentModel.Container();
            menuStripLanguage = new MenuStrip();
            türkçeToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            grbAboutUser = new GroupBox();
            btnUpdate = new Button();
            btnClear = new Button();
            chbPassword = new CheckBox();
            txtPassword = new TextBox();
            chbMail = new CheckBox();
            txtMail = new TextBox();
            chbUsername = new CheckBox();
            txtUsername = new TextBox();
            lblPassword = new Label();
            lblMail = new Label();
            lblUsername = new Label();
            grbCargo = new GroupBox();
            rdbOther = new RadioButton();
            rdbCompanies = new RadioButton();
            btnCargoUpdate = new Button();
            btnCargoClear = new Button();
            txtCargoOther = new TextBox();
            lblOther = new Label();
            cmbCompanies = new ComboBox();
            lblCompanies = new Label();
            txtCargo = new TextBox();
            lblCargo = new Label();
            grbDetailsOfUser = new GroupBox();
            btnLanguageUpdate = new Button();
            cmbLanguage = new ComboBox();
            lblLanguage = new Label();
            nmrSinceRegistration = new NumericUpDown();
            txtDateTimeOfRegister = new TextBox();
            lblSinceRegistration = new Label();
            lblDateTimeRegistration = new Label();
            dgwIPHistory = new DataGridView();
            grbHistoryOfActivity = new GroupBox();
            dgwLogs = new DataGridView();
            grbIPHistory = new GroupBox();
            lblSerial = new Label();
            btnSerialClear = new Button();
            txtSerialKey = new TextBox();
            lblRemainingDays = new Label();
            btnSerialConfirm = new Button();
            nmrRemainingDays = new NumericUpDown();
            grbSerial = new GroupBox();
            timerMailScroll = new System.Windows.Forms.Timer(components);
            timerUsernameScroll2 = new System.Windows.Forms.Timer(components);
            menuStripLanguage.SuspendLayout();
            grbAboutUser.SuspendLayout();
            grbCargo.SuspendLayout();
            grbDetailsOfUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrSinceRegistration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgwIPHistory).BeginInit();
            grbHistoryOfActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwLogs).BeginInit();
            grbIPHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nmrRemainingDays).BeginInit();
            grbSerial.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripLanguage
            // 
            menuStripLanguage.BackColor = SystemColors.MenuBar;
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStripLanguage.Location = new Point(0, 0);
            menuStripLanguage.Name = "menuStripLanguage";
            menuStripLanguage.Size = new Size(616, 24);
            menuStripLanguage.TabIndex = 0;
            menuStripLanguage.Text = "menuStripLanguage";
            // 
            // türkçeToolStripMenuItem
            // 
            türkçeToolStripMenuItem.Image = Languages.Turkish.TurkFlag;
            türkçeToolStripMenuItem.Name = "türkçeToolStripMenuItem";
            türkçeToolStripMenuItem.Size = new Size(70, 20);
            türkçeToolStripMenuItem.Text = "Türkçe";
            türkçeToolStripMenuItem.Click += türkçeToolStripMenuItem_Click;
            // 
            // englishToolStripMenuItem
            // 
            englishToolStripMenuItem.Image = Languages.English.EngFlag;
            englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            englishToolStripMenuItem.Size = new Size(73, 20);
            englishToolStripMenuItem.Text = "English";
            englishToolStripMenuItem.Click += englishToolStripMenuItem_Click;
            // 
            // grbAboutUser
            // 
            grbAboutUser.Controls.Add(btnUpdate);
            grbAboutUser.Controls.Add(btnClear);
            grbAboutUser.Controls.Add(chbPassword);
            grbAboutUser.Controls.Add(txtPassword);
            grbAboutUser.Controls.Add(chbMail);
            grbAboutUser.Controls.Add(txtMail);
            grbAboutUser.Controls.Add(chbUsername);
            grbAboutUser.Controls.Add(txtUsername);
            grbAboutUser.Controls.Add(lblPassword);
            grbAboutUser.Controls.Add(lblMail);
            grbAboutUser.Controls.Add(lblUsername);
            grbAboutUser.Location = new Point(12, 27);
            grbAboutUser.Name = "grbAboutUser";
            grbAboutUser.Size = new Size(281, 151);
            grbAboutUser.TabIndex = 1;
            grbAboutUser.TabStop = false;
            grbAboutUser.Text = "Kullanıcı Bilgileri";
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(173, 114);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(75, 23);
            btnUpdate.TabIndex = 9;
            btnUpdate.Text = "Güncelle";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(88, 114);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 2;
            btnClear.Text = "Temizle";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // chbPassword
            // 
            chbPassword.AutoSize = true;
            chbPassword.Location = new Point(254, 89);
            chbPassword.Name = "chbPassword";
            chbPassword.Size = new Size(15, 14);
            chbPassword.TabIndex = 8;
            chbPassword.UseVisualStyleBackColor = true;
            chbPassword.CheckedChanged += chbPassword_CheckedChanged;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(88, 85);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(160, 23);
            txtPassword.TabIndex = 7;
            // 
            // chbMail
            // 
            chbMail.AutoSize = true;
            chbMail.Location = new Point(254, 60);
            chbMail.Name = "chbMail";
            chbMail.Size = new Size(15, 14);
            chbMail.TabIndex = 6;
            chbMail.UseVisualStyleBackColor = true;
            chbMail.CheckedChanged += chbMail_CheckedChanged;
            // 
            // txtMail
            // 
            txtMail.Location = new Point(88, 56);
            txtMail.Name = "txtMail";
            txtMail.ReadOnly = true;
            txtMail.Size = new Size(160, 23);
            txtMail.TabIndex = 5;
            // 
            // chbUsername
            // 
            chbUsername.AutoSize = true;
            chbUsername.Location = new Point(254, 31);
            chbUsername.Name = "chbUsername";
            chbUsername.Size = new Size(15, 14);
            chbUsername.TabIndex = 4;
            chbUsername.UseVisualStyleBackColor = true;
            chbUsername.CheckedChanged += chbUsername_CheckedChanged;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(88, 27);
            txtUsername.Name = "txtUsername";
            txtUsername.ReadOnly = true;
            txtUsername.Size = new Size(160, 23);
            txtUsername.TabIndex = 3;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(17, 88);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(30, 15);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Şifre";
            // 
            // lblMail
            // 
            lblMail.AutoSize = true;
            lblMail.Location = new Point(17, 59);
            lblMail.Name = "lblMail";
            lblMail.Size = new Size(30, 15);
            lblMail.TabIndex = 1;
            lblMail.Text = "Mail";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(17, 30);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(29, 15);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "İsim";
            // 
            // grbCargo
            // 
            grbCargo.Controls.Add(rdbOther);
            grbCargo.Controls.Add(rdbCompanies);
            grbCargo.Controls.Add(btnCargoUpdate);
            grbCargo.Controls.Add(btnCargoClear);
            grbCargo.Controls.Add(txtCargoOther);
            grbCargo.Controls.Add(lblOther);
            grbCargo.Controls.Add(cmbCompanies);
            grbCargo.Controls.Add(lblCompanies);
            grbCargo.Controls.Add(txtCargo);
            grbCargo.Controls.Add(lblCargo);
            grbCargo.Location = new Point(12, 184);
            grbCargo.Name = "grbCargo";
            grbCargo.Size = new Size(281, 140);
            grbCargo.TabIndex = 10;
            grbCargo.TabStop = false;
            grbCargo.Text = "Anlaşmalı Kargo Şirketi";
            // 
            // rdbOther
            // 
            rdbOther.AutoSize = true;
            rdbOther.Location = new Point(222, -3);
            rdbOther.Name = "rdbOther";
            rdbOther.Size = new Size(53, 19);
            rdbOther.TabIndex = 12;
            rdbOther.TabStop = true;
            rdbOther.Text = "Diğer";
            rdbOther.UseVisualStyleBackColor = true;
            rdbOther.CheckedChanged += rdbOther_CheckedChanged;
            // 
            // rdbCompanies
            // 
            rdbCompanies.AutoSize = true;
            rdbCompanies.Location = new Point(155, -3);
            rdbCompanies.Name = "rdbCompanies";
            rdbCompanies.Size = new Size(67, 19);
            rdbCompanies.TabIndex = 10;
            rdbCompanies.TabStop = true;
            rdbCompanies.Text = "Şirketler";
            rdbCompanies.UseVisualStyleBackColor = true;
            rdbCompanies.CheckedChanged += rdbCompanies_CheckedChanged;
            // 
            // btnCargoUpdate
            // 
            btnCargoUpdate.Location = new Point(173, 109);
            btnCargoUpdate.Name = "btnCargoUpdate";
            btnCargoUpdate.Size = new Size(75, 23);
            btnCargoUpdate.TabIndex = 11;
            btnCargoUpdate.Text = "Güncelle";
            btnCargoUpdate.UseVisualStyleBackColor = true;
            btnCargoUpdate.Click += btnCargoUpdate_Click;
            // 
            // btnCargoClear
            // 
            btnCargoClear.Location = new Point(88, 109);
            btnCargoClear.Name = "btnCargoClear";
            btnCargoClear.Size = new Size(75, 23);
            btnCargoClear.TabIndex = 10;
            btnCargoClear.Text = "Temizle";
            btnCargoClear.UseVisualStyleBackColor = true;
            btnCargoClear.Click += btnCargoClear_Click;
            // 
            // txtCargoOther
            // 
            txtCargoOther.Location = new Point(88, 80);
            txtCargoOther.Name = "txtCargoOther";
            txtCargoOther.Size = new Size(181, 23);
            txtCargoOther.TabIndex = 9;
            // 
            // lblOther
            // 
            lblOther.AutoSize = true;
            lblOther.Location = new Point(17, 83);
            lblOther.Name = "lblOther";
            lblOther.Size = new Size(35, 15);
            lblOther.TabIndex = 8;
            lblOther.Text = "Diğer";
            // 
            // cmbCompanies
            // 
            cmbCompanies.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCompanies.FormattingEnabled = true;
            cmbCompanies.Location = new Point(88, 51);
            cmbCompanies.Name = "cmbCompanies";
            cmbCompanies.Size = new Size(181, 23);
            cmbCompanies.TabIndex = 7;
            // 
            // lblCompanies
            // 
            lblCompanies.AutoSize = true;
            lblCompanies.Location = new Point(17, 57);
            lblCompanies.Name = "lblCompanies";
            lblCompanies.Size = new Size(49, 15);
            lblCompanies.TabIndex = 6;
            lblCompanies.Text = "Şirketler";
            // 
            // txtCargo
            // 
            txtCargo.Location = new Point(88, 22);
            txtCargo.Name = "txtCargo";
            txtCargo.Size = new Size(181, 23);
            txtCargo.TabIndex = 5;
            // 
            // lblCargo
            // 
            lblCargo.AutoSize = true;
            lblCargo.Location = new Point(17, 28);
            lblCargo.Name = "lblCargo";
            lblCargo.Size = new Size(38, 15);
            lblCargo.TabIndex = 0;
            lblCargo.Text = "Kargo";
            // 
            // grbDetailsOfUser
            // 
            grbDetailsOfUser.Controls.Add(btnLanguageUpdate);
            grbDetailsOfUser.Controls.Add(cmbLanguage);
            grbDetailsOfUser.Controls.Add(lblLanguage);
            grbDetailsOfUser.Controls.Add(nmrSinceRegistration);
            grbDetailsOfUser.Controls.Add(txtDateTimeOfRegister);
            grbDetailsOfUser.Controls.Add(lblSinceRegistration);
            grbDetailsOfUser.Controls.Add(lblDateTimeRegistration);
            grbDetailsOfUser.Location = new Point(299, 27);
            grbDetailsOfUser.Name = "grbDetailsOfUser";
            grbDetailsOfUser.Size = new Size(310, 151);
            grbDetailsOfUser.TabIndex = 11;
            grbDetailsOfUser.TabStop = false;
            grbDetailsOfUser.Text = "Kullanıcı Detayları";
            // 
            // btnLanguageUpdate
            // 
            btnLanguageUpdate.Location = new Point(121, 122);
            btnLanguageUpdate.Name = "btnLanguageUpdate";
            btnLanguageUpdate.Size = new Size(183, 23);
            btnLanguageUpdate.TabIndex = 19;
            btnLanguageUpdate.Text = "Güncelle";
            btnLanguageUpdate.UseVisualStyleBackColor = true;
            btnLanguageUpdate.Click += btnLanguageUpdate_Click;
            // 
            // cmbLanguage
            // 
            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.Location = new Point(122, 88);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(182, 23);
            cmbLanguage.TabIndex = 15;
            // 
            // lblLanguage
            // 
            lblLanguage.AutoSize = true;
            lblLanguage.Location = new Point(6, 91);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new Size(21, 15);
            lblLanguage.TabIndex = 14;
            lblLanguage.Text = "Dil";
            // 
            // nmrSinceRegistration
            // 
            nmrSinceRegistration.Location = new Point(122, 59);
            nmrSinceRegistration.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrSinceRegistration.Name = "nmrSinceRegistration";
            nmrSinceRegistration.Size = new Size(182, 23);
            nmrSinceRegistration.TabIndex = 3;
            // 
            // txtDateTimeOfRegister
            // 
            txtDateTimeOfRegister.Location = new Point(122, 30);
            txtDateTimeOfRegister.Name = "txtDateTimeOfRegister";
            txtDateTimeOfRegister.ReadOnly = true;
            txtDateTimeOfRegister.Size = new Size(182, 23);
            txtDateTimeOfRegister.TabIndex = 2;
            // 
            // lblSinceRegistration
            // 
            lblSinceRegistration.AutoSize = true;
            lblSinceRegistration.Location = new Point(6, 61);
            lblSinceRegistration.Name = "lblSinceRegistration";
            lblSinceRegistration.Size = new Size(96, 15);
            lblSinceRegistration.TabIndex = 1;
            lblSinceRegistration.Text = "Kayıtlı Gün Sayısı";
            // 
            // lblDateTimeRegistration
            // 
            lblDateTimeRegistration.AutoSize = true;
            lblDateTimeRegistration.Location = new Point(6, 33);
            lblDateTimeRegistration.Name = "lblDateTimeRegistration";
            lblDateTimeRegistration.Size = new Size(64, 15);
            lblDateTimeRegistration.TabIndex = 0;
            lblDateTimeRegistration.Text = "Kayıt Tarihi";
            // 
            // dgwIPHistory
            // 
            dgwIPHistory.AllowUserToAddRows = false;
            dgwIPHistory.AllowUserToDeleteRows = false;
            dgwIPHistory.AllowUserToResizeColumns = false;
            dgwIPHistory.AllowUserToResizeRows = false;
            dgwIPHistory.BorderStyle = BorderStyle.Fixed3D;
            dgwIPHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwIPHistory.Location = new Point(6, 19);
            dgwIPHistory.Name = "dgwIPHistory";
            dgwIPHistory.ReadOnly = true;
            dgwIPHistory.Size = new Size(298, 177);
            dgwIPHistory.TabIndex = 5;
            // 
            // grbHistoryOfActivity
            // 
            grbHistoryOfActivity.Controls.Add(dgwLogs);
            grbHistoryOfActivity.Location = new Point(12, 330);
            grbHistoryOfActivity.Name = "grbHistoryOfActivity";
            grbHistoryOfActivity.Size = new Size(281, 202);
            grbHistoryOfActivity.TabIndex = 12;
            grbHistoryOfActivity.TabStop = false;
            grbHistoryOfActivity.Text = "Etkinlik Geçmişi";
            // 
            // dgwLogs
            // 
            dgwLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwLogs.Location = new Point(3, 19);
            dgwLogs.Name = "dgwLogs";
            dgwLogs.Size = new Size(272, 177);
            dgwLogs.TabIndex = 0;
            // 
            // grbIPHistory
            // 
            grbIPHistory.Controls.Add(dgwIPHistory);
            grbIPHistory.Location = new Point(299, 330);
            grbIPHistory.Name = "grbIPHistory";
            grbIPHistory.Size = new Size(310, 202);
            grbIPHistory.TabIndex = 13;
            grbIPHistory.TabStop = false;
            grbIPHistory.Text = "IP Geçmişi";
            // 
            // lblSerial
            // 
            lblSerial.AutoSize = true;
            lblSerial.Location = new Point(110, 57);
            lblSerial.Name = "lblSerial";
            lblSerial.Size = new Size(57, 15);
            lblSerial.TabIndex = 1;
            lblSerial.Text = "Serial Key";
            // 
            // btnSerialClear
            // 
            btnSerialClear.Location = new Point(80, 109);
            btnSerialClear.Name = "btnSerialClear";
            btnSerialClear.Size = new Size(75, 23);
            btnSerialClear.TabIndex = 20;
            btnSerialClear.Text = "Temizle";
            btnSerialClear.UseVisualStyleBackColor = true;
            btnSerialClear.Click += btnSerialClear_Click;
            // 
            // txtSerialKey
            // 
            txtSerialKey.Location = new Point(24, 75);
            txtSerialKey.Name = "txtSerialKey";
            txtSerialKey.Size = new Size(269, 23);
            txtSerialKey.TabIndex = 18;
            txtSerialKey.TextAlign = HorizontalAlignment.Center;
            // 
            // lblRemainingDays
            // 
            lblRemainingDays.AutoSize = true;
            lblRemainingDays.Location = new Point(24, 28);
            lblRemainingDays.Name = "lblRemainingDays";
            lblRemainingDays.Size = new Size(93, 15);
            lblRemainingDays.TabIndex = 16;
            lblRemainingDays.Text = "Kalan Gün Sayısı";
            // 
            // btnSerialConfirm
            // 
            btnSerialConfirm.Location = new Point(161, 109);
            btnSerialConfirm.Name = "btnSerialConfirm";
            btnSerialConfirm.Size = new Size(75, 23);
            btnSerialConfirm.TabIndex = 21;
            btnSerialConfirm.Text = "Onayla";
            btnSerialConfirm.UseVisualStyleBackColor = true;
            btnSerialConfirm.Click += btnSerialConfirm_Click;
            // 
            // nmrRemainingDays
            // 
            nmrRemainingDays.Location = new Point(134, 26);
            nmrRemainingDays.Maximum = new decimal(new int[] { 1661992959, 1808227885, 5, 0 });
            nmrRemainingDays.Name = "nmrRemainingDays";
            nmrRemainingDays.Size = new Size(153, 23);
            nmrRemainingDays.TabIndex = 17;
            // 
            // grbSerial
            // 
            grbSerial.Controls.Add(nmrRemainingDays);
            grbSerial.Controls.Add(btnSerialConfirm);
            grbSerial.Controls.Add(lblRemainingDays);
            grbSerial.Controls.Add(txtSerialKey);
            grbSerial.Controls.Add(btnSerialClear);
            grbSerial.Controls.Add(lblSerial);
            grbSerial.Location = new Point(299, 184);
            grbSerial.Name = "grbSerial";
            grbSerial.Size = new Size(310, 140);
            grbSerial.TabIndex = 22;
            grbSerial.TabStop = false;
            grbSerial.Text = "Kullanım Hakları";
            // 
            // timerMailScroll
            // 
            timerMailScroll.Interval = 150;
            timerMailScroll.Tick += timerMailScroll_Tick;
            // 
            // timerUsernameScroll2
            // 
            timerUsernameScroll2.Interval = 150;
            timerUsernameScroll2.Tick += timerUsernameScroll2_Tick;
            // 
            // UserInfoScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(616, 539);
            Controls.Add(grbSerial);
            Controls.Add(grbIPHistory);
            Controls.Add(grbHistoryOfActivity);
            Controls.Add(grbDetailsOfUser);
            Controls.Add(grbCargo);
            Controls.Add(grbAboutUser);
            Controls.Add(menuStripLanguage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStripLanguage;
            MaximizeBox = false;
            Name = "UserInfoScreen";
            Text = "Kullanıcı Bilgileri";
            Load += UserInfoScreen_Load;
            menuStripLanguage.ResumeLayout(false);
            menuStripLanguage.PerformLayout();
            grbAboutUser.ResumeLayout(false);
            grbAboutUser.PerformLayout();
            grbCargo.ResumeLayout(false);
            grbCargo.PerformLayout();
            grbDetailsOfUser.ResumeLayout(false);
            grbDetailsOfUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nmrSinceRegistration).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgwIPHistory).EndInit();
            grbHistoryOfActivity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwLogs).EndInit();
            grbIPHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nmrRemainingDays).EndInit();
            grbSerial.ResumeLayout(false);
            grbSerial.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripLanguage;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private GroupBox grbAboutUser;
        private Label lblPassword;
        private Label lblMail;
        private Label lblUsername;
        private CheckBox chbUsername;
        private TextBox txtUsername;
        private TextBox txtMail;
        private CheckBox chbPassword;
        private TextBox txtPassword;
        private CheckBox chbMail;
        private Button btnUpdate;
        private Button btnClear;
        private GroupBox grbCargo;
        private Label lblCargo;
        private Label lblOther;
        private ComboBox cmbCompanies;
        private Label lblCompanies;
        private TextBox txtCargo;
        private TextBox txtCargoOther;
        private Button btnCargoUpdate;
        private Button btnCargoClear;
        private GroupBox grbDetailsOfUser;
        private GroupBox grbHistoryOfActivity;
        private Label lblDateTimeRegistration;
        private Label lblSinceRegistration;
        private DataGridView dgwIPHistory;
        private NumericUpDown nmrSinceRegistration;
        private TextBox txtDateTimeOfRegister;
        private DataGridView dgwLogs;
        private GroupBox grbIPHistory;
        private ComboBox cmbLanguage;
        private Label lblLanguage;
        private Button btnLanguageUpdate;
        private RadioButton rdbOther;
        private RadioButton rdbCompanies;
        private Label lblSerial;
        private Button btnSerialClear;
        private TextBox txtSerialKey;
        private Label lblRemainingDays;
        private Button btnSerialConfirm;
        private NumericUpDown nmrRemainingDays;
        private GroupBox grbSerial;
        private System.Windows.Forms.Timer timerMailScroll;
        private System.Windows.Forms.Timer timerUsernameScroll;
        private System.Windows.Forms.Timer timerUsernameScroll2;
    }
}