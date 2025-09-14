using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Data.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace neoStockMasterv2.Forms
{
    public partial class MainMenu : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static User LoggedInUser { get; set; }
        public bool IsAlwaysOnTop => chbTop.Checked;
        UserService _userService = new UserService();

        public MainMenu(User user)
        {
            InitializeComponent();

            LoggedInUser = user;
            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            LanguageService.SetLanguage(user.Language);
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            LanguageService.LanguageChanged += UpdateFormWelcome;
            UpdateFormWelcome();
            UserAccessControl();
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();

            SetSelectedLanguageUI();
            UpdateSelectedLanguage(); // İlk açılışta seçili dili uygula
        }

        private void LanguageService_LanguageChanged()
        {
            UpdateFormTexts();
            UpdateSelectedLanguage(); // Seçili dili güncelle
        }

        private void UpdateFormTexts()
        {
            türkçeToolStripMenuItem.Text = LanguageService.GetString("Türkçe");
            englishToolStripMenuItem.Text = LanguageService.GetString("English2");

            grbMenu.Text = LanguageService.GetString("İşlem Menüsü");
            btnProductManagement.Text = LanguageService.GetString("Ürün İşlemleri");
            btnCalculatePriceAddOrder.Text = LanguageService.GetString("Fiyat Hesaplama - Sipariş Oluşturma");
            btnViewOrdersEditOrders.Text = LanguageService.GetString("Siparişleri Görüntüle - Düzenle");
            btnZReport.Text = LanguageService.GetString("Z Raporu");
            grbUserPanel.Text = LanguageService.GetString("Kullanıcı Paneli");
            btnAboutUser.Text = LanguageService.GetString("Kullanıcı Bilgileri");
            btnLogOut.Text = LanguageService.GetString("Çıkış");
            chbTop.Text = LanguageService.GetString("Uygulamayı Üstte Tut");

            this.Text = LanguageService.GetString("Ana Menü");
        }

        private void UpdateFormWelcome()
        {
            if (LanguageService.CurrentLanguage == "English")
            {
                lblWelcome.Text = $"Dear {LoggedInUser.Name}, welcome.";

            }
            else
            {
                lblWelcome.Text = $"Sayın {LoggedInUser.Name}, hoş geldiniz.";
            }
        }

        private void SetSelectedLanguageUI()
        {
            string userLanguage = LoggedInUser.Language; // Kullanıcının dil bilgisi

            // Seçili dili arayüze uygula
            LanguageService.SetLanguage(userLanguage);

            // Buton arka planlarını güncelle
            if (userLanguage == "Türkçe")
            {
                türkçeToolStripMenuItem.BackColor = Color.LightBlue;
                englishToolStripMenuItem.BackColor = SystemColors.Control;

            }
            else if (userLanguage == "English")
            {
                englishToolStripMenuItem.BackColor = Color.LightBlue;
                türkçeToolStripMenuItem.BackColor = SystemColors.Control;
            }

            // Form üzerindeki yazıları güncelle
            UpdateFormTexts();
        }

        private void UpdateSelectedLanguage()
        {
            string currentLanguage = LanguageService.CurrentLanguage;

            türkçeToolStripMenuItem.Checked = currentLanguage == "Türkçe";
            englishToolStripMenuItem.Checked = currentLanguage == "English";

            türkçeToolStripMenuItem.BackColor = türkçeToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;
            englishToolStripMenuItem.BackColor = englishToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;
        }

        private void türkçeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("Türkçe");
            UpdateSelectedLanguage();
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("English");
            UpdateSelectedLanguage();
        }

        // UYGULAMAYI ÜSTTE TUTMAYA BAŞLANGICI
        private void ToggleAlwaysOnTop()
        {
            bool alwaysOnTop = chbTop.Checked;

            this.TopMost = alwaysOnTop; // MainMenu her zaman önde kalsın

            // Açık olan tüm formları kontrol et ve TopMost ayarlarını yap
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm != this) // MainMenu harici formlar
                {
                    openForm.TopMost = alwaysOnTop;
                }
            }
        }

        private void chbTop_CheckedChanged(object sender, EventArgs e)
        {
            ToggleAlwaysOnTop();
        }
        // UYGULAMAYI ÜSTTE TUTMAYA BİTİŞİ

        private void OpenProductManagementScreen()
        {
            string currentLanguage = LanguageService.CurrentLanguage; // Mevcut dili al

            ProductManagementScreen productManagementScreen = new ProductManagementScreen(currentLanguage);
            productManagementScreen.Owner = this; // MainMenu'yu sahip olarak ayarla (diğer formlar MainMenu'nun önüne geçebilir)

            productManagementScreen.TopMost = chbTop.Checked; // Yeni form da her zaman en önde olsun

            productManagementScreen.FormClosed += (s, e) =>
            {
                // Eğer chbTop seçiliyse, tüm formlar en önde kalmaya devam etsin
                ToggleAlwaysOnTop();
            };

            productManagementScreen.ShowDialog();
        }

        private void OpenUserInfoScreen()
        {
            string currentLanguage = LanguageService.CurrentLanguage; // Mevcut dili al

            UserInfoScreen userInfoScreen = new UserInfoScreen(currentLanguage);
            userInfoScreen.Owner = this; // MainMenu'yu sahip olarak ayarla (diğer formlar MainMenu'nun önüne geçebilir)

            userInfoScreen.TopMost = chbTop.Checked; // Yeni form da her zaman en önde olsun

            userInfoScreen.FormClosed += (s, e) =>
            {
                // Eğer chbTop seçiliyse, tüm formlar en önde kalmaya devam etsin
                ToggleAlwaysOnTop();
            };

            userInfoScreen.ShowDialog();
        }

        private void btnProductManagement_Click(object sender, EventArgs e)
        {
            OpenProductManagementScreen();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Çıkış yapmak istediğinize emin misiniz?",
                "Çıkış Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                _userService.LogOut();

                // Bilgilendirme
                MessageBox.Show("Başarıyla çıkış yaptınız!", "Çıkış Yapıldı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                LoginScreen loginScreen = new LoginScreen();
                loginScreen.Show();
            }
            else
            {
                MessageBox.Show("Çıkış işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAboutUser_Click(object sender, EventArgs e)
        {
            OpenUserInfoScreen();
        }

        public void UserAccessControl()
        {
            if (LoggedInUser.RemainingDays > 0)
            {
                grbMenu.Enabled = true;
            }
            else
            {
                grbMenu.Enabled = false;
                MessageBox.Show("Kullanım süreniz dolmuştur. Lütfen aboneliğinizi yenileyin.",
                                "Erişim Engellendi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }

        private void OpenPricingOrderScreen()
        {
            string currentLanguage = LanguageService.CurrentLanguage; // Mevcut dili al

            PricingOrderScreen pricingOrderScreen = new PricingOrderScreen(currentLanguage);
            pricingOrderScreen.Owner = this; // MainMenu'yu sahip olarak ayarla (diğer formlar MainMenu'nun önüne geçebilir)

            pricingOrderScreen.TopMost = chbTop.Checked; // Yeni form da her zaman en önde olsun

            pricingOrderScreen.FormClosed += (s, e) =>
            {
                // Eğer chbTop seçiliyse, tüm formlar en önde kalmaya devam etsin
                ToggleAlwaysOnTop();
            };

            pricingOrderScreen.ShowDialog();
        }

        private void btnCalculatePriceAddOrder_Click(object sender, EventArgs e)
        {
            OpenPricingOrderScreen();
        }
    }
}
