namespace neoStockMasterv2.Forms
{
    partial class RecoveryScreen
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
            menuStripLanguage = new MenuStrip();
            türkçeToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            grbMail = new GroupBox();
            txtMail = new TextBox();
            grbActivity = new GroupBox();
            btnForgetUsername = new Button();
            btnForgetPassword = new Button();
            menuStripLanguage.SuspendLayout();
            grbMail.SuspendLayout();
            grbActivity.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripLanguage
            // 
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStripLanguage.Location = new Point(0, 0);
            menuStripLanguage.Name = "menuStripLanguage";
            menuStripLanguage.Size = new Size(395, 24);
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
            // grbMail
            // 
            grbMail.Controls.Add(txtMail);
            grbMail.Location = new Point(12, 27);
            grbMail.Name = "grbMail";
            grbMail.Size = new Size(374, 59);
            grbMail.TabIndex = 1;
            grbMail.TabStop = false;
            grbMail.Text = "E-Mail Adresinizi Yazınız";
            // 
            // txtMail
            // 
            txtMail.Location = new Point(6, 22);
            txtMail.Name = "txtMail";
            txtMail.Size = new Size(362, 23);
            txtMail.TabIndex = 0;
            // 
            // grbActivity
            // 
            grbActivity.Controls.Add(btnForgetUsername);
            grbActivity.Location = new Point(12, 92);
            grbActivity.Name = "grbActivity";
            grbActivity.Size = new Size(374, 64);
            grbActivity.TabIndex = 2;
            grbActivity.TabStop = false;
            grbActivity.Text = "İşlemi Seç";
            // 
            // btnForgetUsername
            // 
            btnForgetUsername.Location = new Point(3, 19);
            btnForgetUsername.Name = "btnForgetUsername";
            btnForgetUsername.Size = new Size(178, 34);
            btnForgetUsername.TabIndex = 0;
            btnForgetUsername.Text = "Kullanıcı İsmini Unuttum";
            btnForgetUsername.UseVisualStyleBackColor = true;
            btnForgetUsername.Click += btnForgetUsername_Click;
            // 
            // btnForgetPassword
            // 
            btnForgetPassword.Location = new Point(199, 111);
            btnForgetPassword.Name = "btnForgetPassword";
            btnForgetPassword.Size = new Size(178, 34);
            btnForgetPassword.TabIndex = 1;
            btnForgetPassword.Text = "Şifremi Unuttum";
            btnForgetPassword.UseVisualStyleBackColor = true;
            btnForgetPassword.Click += btnForgetPassword_Click;
            // 
            // RecoveryScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(395, 164);
            Controls.Add(btnForgetPassword);
            Controls.Add(grbActivity);
            Controls.Add(grbMail);
            Controls.Add(menuStripLanguage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStripLanguage;
            MaximizeBox = false;
            Name = "RecoveryScreen";
            Text = "Bilgilerimi Hatırlat";
            menuStripLanguage.ResumeLayout(false);
            menuStripLanguage.PerformLayout();
            grbMail.ResumeLayout(false);
            grbMail.PerformLayout();
            grbActivity.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripLanguage;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private GroupBox grbMail;
        private TextBox txtMail;
        private GroupBox grbActivity;
        private Button btnForgetPassword;
        private Button btnForgetUsername;
    }
}