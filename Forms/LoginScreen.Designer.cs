namespace neoStockMasterv2
{
    partial class LoginScreen
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
            lblCoder = new Label();
            lblUsername = new Label();
            txtId = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            chkRememberID = new CheckBox();
            chkRememberPassword = new CheckBox();
            btnLogin = new Button();
            btnRegister = new Button();
            btnRecovery = new Button();
            btnClear = new Button();
            cmbLanguage = new ComboBox();
            lblVersion = new Label();
            timerMarquee = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // lblCoder
            // 
            lblCoder.AutoSize = true;
            lblCoder.Location = new Point(128, 23);
            lblCoder.Name = "lblCoder";
            lblCoder.Size = new Size(176, 15);
            lblCoder.TabIndex = 0;
            lblCoder.Text = "Çağrım Erdinç ÇAKMAKOĞLU || ";
            lblCoder.Click += lblCoder_Click;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(12, 59);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(77, 15);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Kullanıcı İsmi";
            // 
            // txtId
            // 
            txtId.Location = new Point(106, 56);
            txtId.Name = "txtId";
            txtId.Size = new Size(210, 23);
            txtId.TabIndex = 2;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(12, 90);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(86, 15);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Kullanıcı Şifresi";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(106, 87);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(210, 23);
            txtPassword.TabIndex = 4;
            // 
            // chkRememberID
            // 
            chkRememberID.AutoSize = true;
            chkRememberID.Location = new Point(128, 116);
            chkRememberID.Name = "chkRememberID";
            chkRememberID.Size = new Size(75, 19);
            chkRememberID.TabIndex = 5;
            chkRememberID.Text = "ID Hatırla";
            chkRememberID.UseVisualStyleBackColor = true;
            // 
            // chkRememberPassword
            // 
            chkRememberPassword.AutoSize = true;
            chkRememberPassword.CheckAlign = ContentAlignment.MiddleRight;
            chkRememberPassword.Location = new Point(209, 116);
            chkRememberPassword.Name = "chkRememberPassword";
            chkRememberPassword.RightToLeft = RightToLeft.Yes;
            chkRememberPassword.Size = new Size(87, 19);
            chkRememberPassword.TabIndex = 6;
            chkRememberPassword.Text = "Şifre Hatırla";
            chkRememberPassword.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(106, 141);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(210, 23);
            btnLogin.TabIndex = 7;
            btnLogin.Text = "Giriş Yap";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(106, 170);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(210, 23);
            btnRegister.TabIndex = 8;
            btnRegister.Text = "Kayıt Ol";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // btnRecovery
            // 
            btnRecovery.Location = new Point(106, 199);
            btnRecovery.Name = "btnRecovery";
            btnRecovery.Size = new Size(120, 23);
            btnRecovery.TabIndex = 9;
            btnRecovery.Text = "Bilgilerimi Hatırlat";
            btnRecovery.UseVisualStyleBackColor = true;
            btnRecovery.Click += btnRecovery_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(232, 199);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(84, 23);
            btnClear.TabIndex = 10;
            btnClear.Text = "Temizle";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // cmbLanguage
            // 
            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.Location = new Point(12, 242);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(121, 23);
            cmbLanguage.TabIndex = 11;
            cmbLanguage.SelectedIndexChanged += cmbLanguage_SelectedIndexChanged;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(267, 245);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(13, 15);
            lblVersion.TabIndex = 12;
            lblVersion.Text = "v";
            // 
            // timerMarquee
            // 
            timerMarquee.Tick += timerMarquee_Tick;
            // 
            // LoginScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(331, 277);
            Controls.Add(lblVersion);
            Controls.Add(cmbLanguage);
            Controls.Add(btnClear);
            Controls.Add(btnRecovery);
            Controls.Add(btnRegister);
            Controls.Add(btnLogin);
            Controls.Add(chkRememberPassword);
            Controls.Add(chkRememberID);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtId);
            Controls.Add(lblUsername);
            Controls.Add(lblCoder);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "LoginScreen";
            Text = "LoginScreen";
            FormClosing += LoginScreen_FormClosing;
            Load += LoginScreen_Load;
            KeyDown += LoginScreen_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCoder;
        private Label lblUsername;
        private TextBox txtId;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkRememberID;
        private CheckBox chkRememberPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Button btnRecovery;
        private Button btnClear;
        private ComboBox cmbLanguage;
        private Label lblVersion;
        private System.Windows.Forms.Timer timerMarquee;
    }
}