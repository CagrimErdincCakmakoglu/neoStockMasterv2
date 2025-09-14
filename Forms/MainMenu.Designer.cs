namespace neoStockMasterv2.Forms
{
    partial class MainMenu
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
            grbMenu = new GroupBox();
            btnZReport = new Button();
            btnViewOrdersEditOrders = new Button();
            btnCalculatePriceAddOrder = new Button();
            btnProductManagement = new Button();
            grbUserPanel = new GroupBox();
            chbTop = new CheckBox();
            btnLogOut = new Button();
            btnAboutUser = new Button();
            lblWelcome = new Label();
            menuStripLanguage.SuspendLayout();
            grbMenu.SuspendLayout();
            grbUserPanel.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripLanguage
            // 
            menuStripLanguage.BackColor = SystemColors.MenuBar;
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem });
            menuStripLanguage.Location = new Point(0, 0);
            menuStripLanguage.Name = "menuStripLanguage";
            menuStripLanguage.Size = new Size(302, 24);
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
            // grbMenu
            // 
            grbMenu.Controls.Add(btnZReport);
            grbMenu.Controls.Add(btnViewOrdersEditOrders);
            grbMenu.Controls.Add(btnCalculatePriceAddOrder);
            grbMenu.Controls.Add(btnProductManagement);
            grbMenu.Location = new Point(12, 61);
            grbMenu.Name = "grbMenu";
            grbMenu.Size = new Size(278, 206);
            grbMenu.TabIndex = 1;
            grbMenu.TabStop = false;
            grbMenu.Text = "İşlem Menüsü";
            // 
            // btnZReport
            // 
            btnZReport.Location = new Point(3, 157);
            btnZReport.Name = "btnZReport";
            btnZReport.Size = new Size(269, 40);
            btnZReport.TabIndex = 3;
            btnZReport.Text = "Z Raporu";
            btnZReport.UseVisualStyleBackColor = true;
            // 
            // btnViewOrdersEditOrders
            // 
            btnViewOrdersEditOrders.Location = new Point(3, 111);
            btnViewOrdersEditOrders.Name = "btnViewOrdersEditOrders";
            btnViewOrdersEditOrders.Size = new Size(269, 40);
            btnViewOrdersEditOrders.TabIndex = 2;
            btnViewOrdersEditOrders.Text = "Siparişleri Görüntüle - Düzenle";
            btnViewOrdersEditOrders.UseVisualStyleBackColor = true;
            // 
            // btnCalculatePriceAddOrder
            // 
            btnCalculatePriceAddOrder.Location = new Point(3, 65);
            btnCalculatePriceAddOrder.Name = "btnCalculatePriceAddOrder";
            btnCalculatePriceAddOrder.Size = new Size(269, 40);
            btnCalculatePriceAddOrder.TabIndex = 1;
            btnCalculatePriceAddOrder.Text = "Fiyat Hesaplama - Sipariş Oluşturma";
            btnCalculatePriceAddOrder.UseVisualStyleBackColor = true;
            btnCalculatePriceAddOrder.Click += btnCalculatePriceAddOrder_Click;
            // 
            // btnProductManagement
            // 
            btnProductManagement.Location = new Point(3, 19);
            btnProductManagement.Name = "btnProductManagement";
            btnProductManagement.Size = new Size(269, 40);
            btnProductManagement.TabIndex = 0;
            btnProductManagement.Text = "Ürün İşlemleri";
            btnProductManagement.UseVisualStyleBackColor = true;
            btnProductManagement.Click += btnProductManagement_Click;
            // 
            // grbUserPanel
            // 
            grbUserPanel.Controls.Add(chbTop);
            grbUserPanel.Controls.Add(btnLogOut);
            grbUserPanel.Controls.Add(btnAboutUser);
            grbUserPanel.Location = new Point(12, 273);
            grbUserPanel.Name = "grbUserPanel";
            grbUserPanel.Size = new Size(278, 94);
            grbUserPanel.TabIndex = 2;
            grbUserPanel.TabStop = false;
            grbUserPanel.Text = "Kullanıcı Paneli";
            // 
            // chbTop
            // 
            chbTop.AutoSize = true;
            chbTop.Location = new Point(84, 63);
            chbTop.Name = "chbTop";
            chbTop.Size = new Size(139, 19);
            chbTop.TabIndex = 2;
            chbTop.Text = "Uygulamayı Üstte Tut";
            chbTop.UseVisualStyleBackColor = true;
            chbTop.CheckedChanged += chbTop_CheckedChanged;
            // 
            // btnLogOut
            // 
            btnLogOut.Location = new Point(169, 17);
            btnLogOut.Name = "btnLogOut";
            btnLogOut.Size = new Size(103, 40);
            btnLogOut.TabIndex = 1;
            btnLogOut.Text = "Çıkış";
            btnLogOut.UseVisualStyleBackColor = true;
            btnLogOut.Click += btnLogOut_Click;
            // 
            // btnAboutUser
            // 
            btnAboutUser.Location = new Point(3, 17);
            btnAboutUser.Name = "btnAboutUser";
            btnAboutUser.Size = new Size(160, 40);
            btnAboutUser.TabIndex = 0;
            btnAboutUser.Text = "Kullanıcı Bilgileri";
            btnAboutUser.UseVisualStyleBackColor = true;
            btnAboutUser.Click += btnAboutUser_Click;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(12, 33);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(57, 15);
            lblWelcome.TabIndex = 3;
            lblWelcome.Text = "Welcome";
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(302, 378);
            Controls.Add(lblWelcome);
            Controls.Add(grbUserPanel);
            Controls.Add(grbMenu);
            Controls.Add(menuStripLanguage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStripLanguage;
            MaximizeBox = false;
            Name = "MainMenu";
            Text = "Ana Menü";
            FormClosing += MainMenu_FormClosing;
            Load += MainMenu_Load;
            menuStripLanguage.ResumeLayout(false);
            menuStripLanguage.PerformLayout();
            grbMenu.ResumeLayout(false);
            grbUserPanel.ResumeLayout(false);
            grbUserPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripLanguage;
        private ToolStripMenuItem türkçeToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private GroupBox grbMenu;
        private GroupBox grbUserPanel;
        private Label lblWelcome;
        private Button btnZReport;
        private Button btnViewOrdersEditOrders;
        private Button btnCalculatePriceAddOrder;
        private Button btnProductManagement;
        private Button btnLogOut;
        private Button btnAboutUser;
        public CheckBox chbTop;
    }
}