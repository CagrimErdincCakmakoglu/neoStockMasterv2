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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            menuStripLanguage = new MenuStrip();
            türkçeToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            bizeUlaşToolStripMenuItem = new ToolStripMenuItem();
            mailGonderToolStripMenuItem = new ToolStripMenuItem();
            teknikToolStripMenuItem = new ToolStripMenuItem();
            teknikDestekToolStripMenuItem1 = new ToolStripMenuItem();
            hataBildirimiToolStripMenuItem = new ToolStripMenuItem();
            satisHesapToolStripMenuItem = new ToolStripMenuItem();
            abonelikVeOdemeToolStripMenuItem = new ToolStripMenuItem();
            fiyatTeklifiToolStripMenuItem = new ToolStripMenuItem();
            hesapBilgiDegisimiToolStripMenuItem = new ToolStripMenuItem();
            geriBildirimToolStripMenuItem = new ToolStripMenuItem();
            sikayetToolStripMenuItem = new ToolStripMenuItem();
            oneriToolStripMenuItem = new ToolStripMenuItem();
            digerToolStripMenuItem = new ToolStripMenuItem();
            genelIletisimToolStripMenuItem = new ToolStripMenuItem();
            tesekkurToolStripMenuItem = new ToolStripMenuItem();
            biziArayinToolStripMenuItem = new ToolStripMenuItem();
            whatsAppIleToolStripMenuItem = new ToolStripMenuItem();
            cagriBaslatToolStripMenuItem = new ToolStripMenuItem();
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
            menuStripLanguage.Items.AddRange(new ToolStripItem[] { türkçeToolStripMenuItem, englishToolStripMenuItem, bizeUlaşToolStripMenuItem });
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
            türkçeToolStripMenuItem.Size = new Size(71, 20);
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
            // bizeUlaşToolStripMenuItem
            // 
            bizeUlaşToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            bizeUlaşToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mailGonderToolStripMenuItem, biziArayinToolStripMenuItem });
            bizeUlaşToolStripMenuItem.Image = (Image)resources.GetObject("bizeUlaşToolStripMenuItem.Image");
            bizeUlaşToolStripMenuItem.Name = "bizeUlaşToolStripMenuItem";
            bizeUlaşToolStripMenuItem.Size = new Size(91, 20);
            bizeUlaşToolStripMenuItem.Text = "Bize Ulaşın";
            // 
            // mailGonderToolStripMenuItem
            // 
            mailGonderToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { teknikToolStripMenuItem, satisHesapToolStripMenuItem, geriBildirimToolStripMenuItem, digerToolStripMenuItem });
            mailGonderToolStripMenuItem.Image = (Image)resources.GetObject("mailGonderToolStripMenuItem.Image");
            mailGonderToolStripMenuItem.Name = "mailGonderToolStripMenuItem";
            mailGonderToolStripMenuItem.Size = new Size(180, 22);
            mailGonderToolStripMenuItem.Text = "Mail Gönder";
            // 
            // teknikToolStripMenuItem
            // 
            teknikToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { teknikDestekToolStripMenuItem1, hataBildirimiToolStripMenuItem });
            teknikToolStripMenuItem.Image = (Image)resources.GetObject("teknikToolStripMenuItem.Image");
            teknikToolStripMenuItem.Name = "teknikToolStripMenuItem";
            teknikToolStripMenuItem.Size = new Size(149, 22);
            teknikToolStripMenuItem.Text = "Teknik";
            // 
            // teknikDestekToolStripMenuItem1
            // 
            teknikDestekToolStripMenuItem1.Image = (Image)resources.GetObject("teknikDestekToolStripMenuItem1.Image");
            teknikDestekToolStripMenuItem1.Name = "teknikDestekToolStripMenuItem1";
            teknikDestekToolStripMenuItem1.Size = new Size(146, 22);
            teknikDestekToolStripMenuItem1.Text = "Teknik Destek";
            teknikDestekToolStripMenuItem1.Click += teknikDestekToolStripMenuItem1_Click;
            // 
            // hataBildirimiToolStripMenuItem
            // 
            hataBildirimiToolStripMenuItem.Image = (Image)resources.GetObject("hataBildirimiToolStripMenuItem.Image");
            hataBildirimiToolStripMenuItem.Name = "hataBildirimiToolStripMenuItem";
            hataBildirimiToolStripMenuItem.Size = new Size(146, 22);
            hataBildirimiToolStripMenuItem.Text = "Hata Bildirimi";
            hataBildirimiToolStripMenuItem.Click += hataBildirimiToolStripMenuItem_Click;
            // 
            // satisHesapToolStripMenuItem
            // 
            satisHesapToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { abonelikVeOdemeToolStripMenuItem, fiyatTeklifiToolStripMenuItem, hesapBilgiDegisimiToolStripMenuItem });
            satisHesapToolStripMenuItem.Image = (Image)resources.GetObject("satisHesapToolStripMenuItem.Image");
            satisHesapToolStripMenuItem.Name = "satisHesapToolStripMenuItem";
            satisHesapToolStripMenuItem.Size = new Size(149, 22);
            satisHesapToolStripMenuItem.Text = "Satış ve Hesap";
            // 
            // abonelikVeOdemeToolStripMenuItem
            // 
            abonelikVeOdemeToolStripMenuItem.Image = (Image)resources.GetObject("abonelikVeOdemeToolStripMenuItem.Image");
            abonelikVeOdemeToolStripMenuItem.Name = "abonelikVeOdemeToolStripMenuItem";
            abonelikVeOdemeToolStripMenuItem.Size = new Size(182, 22);
            abonelikVeOdemeToolStripMenuItem.Text = "Abonelik ve Ödeme";
            abonelikVeOdemeToolStripMenuItem.Click += abonelikVeOdemeToolStripMenuItem_Click;
            // 
            // fiyatTeklifiToolStripMenuItem
            // 
            fiyatTeklifiToolStripMenuItem.Image = (Image)resources.GetObject("fiyatTeklifiToolStripMenuItem.Image");
            fiyatTeklifiToolStripMenuItem.Name = "fiyatTeklifiToolStripMenuItem";
            fiyatTeklifiToolStripMenuItem.Size = new Size(182, 22);
            fiyatTeklifiToolStripMenuItem.Text = "Fiyat Teklifi";
            fiyatTeklifiToolStripMenuItem.Click += fiyatTeklifiToolStripMenuItem_Click;
            // 
            // hesapBilgiDegisimiToolStripMenuItem
            // 
            hesapBilgiDegisimiToolStripMenuItem.Image = (Image)resources.GetObject("hesapBilgiDegisimiToolStripMenuItem.Image");
            hesapBilgiDegisimiToolStripMenuItem.Name = "hesapBilgiDegisimiToolStripMenuItem";
            hesapBilgiDegisimiToolStripMenuItem.Size = new Size(182, 22);
            hesapBilgiDegisimiToolStripMenuItem.Text = "Hesap Bilgi Değişimi";
            hesapBilgiDegisimiToolStripMenuItem.Click += hesapBilgiDegisimiToolStripMenuItem_Click;
            // 
            // geriBildirimToolStripMenuItem
            // 
            geriBildirimToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { sikayetToolStripMenuItem, oneriToolStripMenuItem });
            geriBildirimToolStripMenuItem.Image = (Image)resources.GetObject("geriBildirimToolStripMenuItem.Image");
            geriBildirimToolStripMenuItem.Name = "geriBildirimToolStripMenuItem";
            geriBildirimToolStripMenuItem.Size = new Size(149, 22);
            geriBildirimToolStripMenuItem.Text = "Geri Bildirim";
            // 
            // sikayetToolStripMenuItem
            // 
            sikayetToolStripMenuItem.Image = (Image)resources.GetObject("sikayetToolStripMenuItem.Image");
            sikayetToolStripMenuItem.Name = "sikayetToolStripMenuItem";
            sikayetToolStripMenuItem.Size = new Size(111, 22);
            sikayetToolStripMenuItem.Text = "Şikayet";
            sikayetToolStripMenuItem.Click += sikayetToolStripMenuItem_Click;
            // 
            // oneriToolStripMenuItem
            // 
            oneriToolStripMenuItem.Image = (Image)resources.GetObject("oneriToolStripMenuItem.Image");
            oneriToolStripMenuItem.Name = "oneriToolStripMenuItem";
            oneriToolStripMenuItem.Size = new Size(111, 22);
            oneriToolStripMenuItem.Text = "Öneri";
            oneriToolStripMenuItem.Click += oneriToolStripMenuItem_Click;
            // 
            // digerToolStripMenuItem
            // 
            digerToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { genelIletisimToolStripMenuItem, tesekkurToolStripMenuItem });
            digerToolStripMenuItem.Image = (Image)resources.GetObject("digerToolStripMenuItem.Image");
            digerToolStripMenuItem.Name = "digerToolStripMenuItem";
            digerToolStripMenuItem.Size = new Size(149, 22);
            digerToolStripMenuItem.Text = "Diğer";
            // 
            // genelIletisimToolStripMenuItem
            // 
            genelIletisimToolStripMenuItem.Image = (Image)resources.GetObject("genelIletisimToolStripMenuItem.Image");
            genelIletisimToolStripMenuItem.Name = "genelIletisimToolStripMenuItem";
            genelIletisimToolStripMenuItem.Size = new Size(145, 22);
            genelIletisimToolStripMenuItem.Text = "Genel İletişim";
            genelIletisimToolStripMenuItem.Click += genelIletisimToolStripMenuItem_Click;
            // 
            // tesekkurToolStripMenuItem
            // 
            tesekkurToolStripMenuItem.Image = (Image)resources.GetObject("tesekkurToolStripMenuItem.Image");
            tesekkurToolStripMenuItem.Name = "tesekkurToolStripMenuItem";
            tesekkurToolStripMenuItem.Size = new Size(145, 22);
            tesekkurToolStripMenuItem.Text = "Teşekkürler";
            tesekkurToolStripMenuItem.Click += tesekkurToolStripMenuItem_Click;
            // 
            // biziArayinToolStripMenuItem
            // 
            biziArayinToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { whatsAppIleToolStripMenuItem, cagriBaslatToolStripMenuItem });
            biziArayinToolStripMenuItem.Image = (Image)resources.GetObject("biziArayinToolStripMenuItem.Image");
            biziArayinToolStripMenuItem.Name = "biziArayinToolStripMenuItem";
            biziArayinToolStripMenuItem.Size = new Size(180, 22);
            biziArayinToolStripMenuItem.Text = "Bizi Arayın";
            // 
            // whatsAppIleToolStripMenuItem
            // 
            whatsAppIleToolStripMenuItem.Image = (Image)resources.GetObject("whatsAppIleToolStripMenuItem.Image");
            whatsAppIleToolStripMenuItem.Name = "whatsAppIleToolStripMenuItem";
            whatsAppIleToolStripMenuItem.Size = new Size(180, 22);
            whatsAppIleToolStripMenuItem.Text = "WhatsApp ile";
            whatsAppIleToolStripMenuItem.Click += whatsAppIleToolStripMenuItem_Click;
            // 
            // cagriBaslatToolStripMenuItem
            // 
            cagriBaslatToolStripMenuItem.Image = (Image)resources.GetObject("cagriBaslatToolStripMenuItem.Image");
            cagriBaslatToolStripMenuItem.Name = "cagriBaslatToolStripMenuItem";
            cagriBaslatToolStripMenuItem.Size = new Size(180, 22);
            cagriBaslatToolStripMenuItem.Text = "Çağrı Başlat";
            cagriBaslatToolStripMenuItem.Click += cagriBaslatToolStripMenuItem_Click;
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
            btnZReport.Click += btnZReport_Click;
            // 
            // btnViewOrdersEditOrders
            // 
            btnViewOrdersEditOrders.Location = new Point(3, 111);
            btnViewOrdersEditOrders.Name = "btnViewOrdersEditOrders";
            btnViewOrdersEditOrders.Size = new Size(269, 40);
            btnViewOrdersEditOrders.TabIndex = 2;
            btnViewOrdersEditOrders.Text = "Siparişleri Görüntüle - Düzenle";
            btnViewOrdersEditOrders.UseVisualStyleBackColor = true;
            btnViewOrdersEditOrders.Click += btnViewOrdersEditOrders_Click;
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
            chbTop.Size = new Size(140, 19);
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
        private ToolStripMenuItem bizeUlaşToolStripMenuItem;
        private ToolStripMenuItem mailGonderToolStripMenuItem;
        private ToolStripMenuItem biziArayinToolStripMenuItem;
        private ToolStripMenuItem whatsAppIleToolStripMenuItem;
        private ToolStripMenuItem cagriBaslatToolStripMenuItem;
        private ToolStripMenuItem teknikToolStripMenuItem;
        private ToolStripMenuItem satisHesapToolStripMenuItem;
        private ToolStripMenuItem teknikDestekToolStripMenuItem1;
        private ToolStripMenuItem hataBildirimiToolStripMenuItem;
        private ToolStripMenuItem abonelikVeOdemeToolStripMenuItem;
        private ToolStripMenuItem fiyatTeklifiToolStripMenuItem;
        private ToolStripMenuItem hesapBilgiDegisimiToolStripMenuItem;
        private ToolStripMenuItem geriBildirimToolStripMenuItem;
        private ToolStripMenuItem sikayetToolStripMenuItem;
        private ToolStripMenuItem oneriToolStripMenuItem;
        private ToolStripMenuItem digerToolStripMenuItem;
        private ToolStripMenuItem genelIletisimToolStripMenuItem;
        private ToolStripMenuItem tesekkurToolStripMenuItem;
    }
}