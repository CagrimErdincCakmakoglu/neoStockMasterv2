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

        // Destek hattı: başında + olmadan, ülke koduyla, boşluksuz yaz (ör. 905321234567)
        private const string SupportPhone = "905XXXXXXXXX";

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
            bizeUlaşToolStripMenuItem.Text = LanguageService.GetString("Bize Ulaşın");
            mailGonderToolStripMenuItem.Text = LanguageService.GetString("Mail Gönder");
            biziArayinToolStripMenuItem.Text = LanguageService.GetString("Bizi Arayın");
            whatsAppIleToolStripMenuItem.Text = LanguageService.GetString("Whatsapp ile");
            cagriBaslatToolStripMenuItem.Text = LanguageService.GetString("Çağrı Başlat");
            teknikToolStripMenuItem.Text = LanguageService.GetString("Teknik");
            satisHesapToolStripMenuItem.Text = LanguageService.GetString("Satış ve Hesap");
            geriBildirimToolStripMenuItem.Text = LanguageService.GetString("Geri Bildirim");
            digerToolStripMenuItem.Text = LanguageService.GetString("Diğer");
            teknikDestekToolStripMenuItem1.Text = LanguageService.GetString("Teknik Destek");
            hataBildirimiToolStripMenuItem.Text = LanguageService.GetString("Hata Bildirimi");
            abonelikVeOdemeToolStripMenuItem.Text = LanguageService.GetString("Abonelik ve Ödeme");
            fiyatTeklifiToolStripMenuItem.Text = LanguageService.GetString("Fiyat Teklifi");
            hesapBilgiDegisimiToolStripMenuItem.Text = LanguageService.GetString("Hesap Bilgi Değişimi");
            sikayetToolStripMenuItem.Text = LanguageService.GetString("Şikayet");
            oneriToolStripMenuItem.Text = LanguageService.GetString("Öneri");
            genelIletisimToolStripMenuItem.Text = LanguageService.GetString("Genel İletişim");
            tesekkurToolStripMenuItem.Text = LanguageService.GetString("Teşekkür");

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

                    // Eğer form TotalTreeViewForm ise ekstra olarak öne getirilmeyi zorlayalım
                    if (openForm.Name == "TotalTreeViewForm" && alwaysOnTop)
                    {
                        openForm.BringToFront();
                    }
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
            var result = MessageBox.Show(
                LanguageService.T("Çıkış yapmak istediğinize emin misiniz?", "Are you sure you want to log out?"),
                LanguageService.T("Çıkış Onayı", "Log Out"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            _userService.LogOut();
            MessageBox.Show(LanguageService.T("Başarıyla çıkış yaptınız!", "You have logged out successfully!"),
                LanguageService.T("Çıkış Yapıldı", "Logged Out"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
        }

        private void btnAboutUser_Click(object sender, EventArgs e)
        {
            OpenUserInfoScreen();
        }

        public void UserAccessControl()
        {
            if (LoggedInUser.DaysLeft > 0)
            {
                grbMenu.Enabled = true;
            }
            else
            {
                grbMenu.Enabled = false;
                MessageBox.Show(
                    LanguageService.T("Kullanım süreniz dolmuştur. Lütfen aboneliğinizi yenileyin.",
                                      "Your subscription has expired. Please renew your subscription."),
                    LanguageService.T("Erişim Engellendi", "Access Denied"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void OpenOrderViewEditScreen()
        {
            string currentLanguage = LanguageService.CurrentLanguage; // Mevcut dili al

            // Formu oluştururken mevcut dili gönder
            OrderViewEditScreen orderViewEditScreen = new OrderViewEditScreen(currentLanguage);
            orderViewEditScreen.Owner = this; // MainMenu formunu sahip olarak ayarla

            // Eğer "Her Zaman Üstte" seçiliyse, bu form da öyle olsun
            orderViewEditScreen.TopMost = chbTop.Checked;

            orderViewEditScreen.FormClosed += (s, e) =>
            {
                // Form kapandığında da kontrol et, üstte kalma özelliğini koru
                ToggleAlwaysOnTop();
            };

            // Modal şekilde aç
            orderViewEditScreen.ShowDialog();
        }

        private void btnViewOrdersEditOrders_Click(object sender, EventArgs e)
        {
            OpenOrderViewEditScreen();
        }

        private void OpenZReportScreen()
        {
            string currentLanguage = LanguageService.CurrentLanguage; // Mevcut dili al

            // Formu oluştururken mevcut dili gönder
            ZReportScreen zReportScreen = new ZReportScreen(currentLanguage);
            zReportScreen.Owner = this; // MainMenu formunu sahip olarak ayarla

            // Eğer "Her Zaman Üstte" seçiliyse, bu form da öyle olsun
            zReportScreen.TopMost = chbTop.Checked;

            zReportScreen.FormClosed += (s, e) =>
            {
                // Form kapandığında da kontrol et, üstte kalma özelliğini koru
                ToggleAlwaysOnTop();
            };

            zReportScreen.ShowDialog();
        }

        private void btnZReport_Click(object sender, EventArgs e)
        {
            OpenZReportScreen();
        }

        private void teknikDestekToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenSupportPrompt(SupportTemplateService.SupportType.TechnicalSupport);
        }



        private void hataBildirimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSupportPrompt(SupportTemplateService.SupportType.BugReport);
        }

        private void abonelikVeOdemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSupportPrompt(SupportTemplateService.SupportType.Billing);
        }

        private void fiyatTeklifiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSupportPrompt(SupportTemplateService.SupportType.PriceQuote);
        }

        private void hesapBilgiDegisimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSupportPrompt(SupportTemplateService.SupportType.AccountChange);
        }

        private void sikayetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSupportPrompt(SupportTemplateService.SupportType.Complaint);
        }

        private void oneriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSupportPrompt(SupportTemplateService.SupportType.Suggestion);
        }

        private void ExecuteMailAction(string subject, string body)
        {
            try
            {
                string mailTo = "stockmasterapp@gmail.com";
                string mailtoUri = $"mailto:{mailTo}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(mailtoUri) { UseShellExecute = true });
            }
            catch (Exception)
            {
                MessageBox.Show(
                    LanguageService.CurrentLanguage == "English"
                        ? "Default mail client could not be opened. Copying text to clipboard..."
                        : "Varsayılan mail istemcisi açılamadı. Metin panoya kopyalandı...",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
                Clipboard.SetText($"Subject: {subject}\n\n{body}");
            }
        }

        private void OpenSupportPrompt(SupportTemplateService.SupportType supportType)
        {
            bool isEng = LanguageService.CurrentLanguage == "English";

            string formTitle = "";
            string lblText = "";

            switch (supportType)
            {
                case SupportTemplateService.SupportType.TechnicalSupport:
                    formTitle = LanguageService.GetString("Teknik Destek");
                    lblText = isEng ? "Please describe the technical problem you are experiencing:" : "Lütfen yaşadığınız teknik sorunu detaylıca açıklayınız:";
                    break;
                case SupportTemplateService.SupportType.BugReport:
                    formTitle = LanguageService.GetString("Hata Bildirimi");
                    lblText = isEng ? "Please explain the bug/error you found step by step:" : "Lütfen karşılaştığınız hatayı (bug) adım adım açıklayınız:";
                    break;
                case SupportTemplateService.SupportType.Billing:
                    formTitle = LanguageService.GetString("Abonelik ve Ödeme");
                    lblText = isEng ? "Please describe your subscription or payment issue:" : "Lütfen abonelik veya ödeme ile ilgili sorunuzu/sorununuzu açıklayınız:";
                    break;
                case SupportTemplateService.SupportType.PriceQuote:
                    formTitle = LanguageService.GetString("Fiyat Teklifi");
                    lblText = isEng ? "Please specify your request for a price quote or upgrade:" : "Lütfen almak istediğiniz fiyat teklifi veya paket yükseltme detaylarını belirtiniz:";
                    break;
                case SupportTemplateService.SupportType.AccountChange:
                    formTitle = LanguageService.GetString("Hesap Bilgi Değişimi");
                    lblText = isEng ? "Please state the account information you want to change:" : "Lütfen değiştirmek istediğiniz hesap veya firma bilgilerinizi yazınız:";
                    break;
                case SupportTemplateService.SupportType.Complaint:
                    formTitle = LanguageService.GetString("Şikayet");
                    lblText = isEng ? "Please specify the issue or dissatisfaction you experienced:" : "Lütfen yaşadığınız sorunu veya memnuniyetsizliği belirtiniz:";
                    break;
                case SupportTemplateService.SupportType.Suggestion:
                    formTitle = LanguageService.GetString("Öneri");
                    lblText = isEng ? "Please share your ideas or feature requests to improve the app:" : "Lütfen uygulamayı geliştirmemize yardımcı olacak fikir veya önerilerinizi paylaşınız:";
                    break;
                case SupportTemplateService.SupportType.GeneralContact:
                    formTitle = LanguageService.GetString("Genel İletişim");
                    lblText = isEng ? "Please write the topic you want to contact us about:" : "Lütfen bizimle iletişime geçmek istediğiniz konuyu yazınız:";
                    break;
                case SupportTemplateService.SupportType.ThankYou:
                    formTitle = LanguageService.GetString("Teşekkür");
                    lblText = isEng ? "We would love to hear your feedback and appreciation:" : "Bizimle paylaşmak istediğiniz güzel mesajınızı ve memnuniyetinizi yazabilirsiniz:";
                    break;
            }

            // Dinamik form oluşturma (metin uzunluğuna göre kendini ayarlar)
            const int margin = 20;
            const int contentWidth = 460;

            Form prompt = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = formTitle,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(24, 24, 36),
                ForeColor = Color.White,
                TopMost = this.TopMost
            };

            Label lblTitle = new Label()
            {
                Left = margin,
                Top = 15,
                AutoSize = false,
                Width = contentWidth,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = lblText
            };
            // Metin kaç satır tutuyorsa o kadar yükseklik ver
            lblTitle.Height = lblTitle.GetPreferredSize(new Size(contentWidth, 0)).Height;

            TextBox txtMessage = new TextBox()
            {
                Left = margin,
                Top = lblTitle.Bottom + 10,
                Width = contentWidth,
                Height = 180,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(36, 36, 52),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnSubmit = new Button()
            {
                Text = isEng ? "Prepare Mail" : "E-Posta Hazırla",
                Width = 120,
                Height = 35,
                Top = txtMessage.Bottom + 15,
                DialogResult = DialogResult.OK,
                FlatStyle = FlatStyle.Flat
            };
            btnSubmit.Left = margin + contentWidth - btnSubmit.Width;
            btnSubmit.FlatAppearance.BorderColor = Color.LightBlue;
            btnSubmit.BackColor = Color.FromArgb(45, 60, 85);

            Button btnCancel = new Button()
            {
                Text = isEng ? "Cancel" : "İptal",
                Width = 100,
                Height = 35,
                Top = btnSubmit.Top,
                DialogResult = DialogResult.Cancel,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Left = btnSubmit.Left - btnCancel.Width - 10;
            btnCancel.FlatAppearance.BorderColor = Color.Gray;

            // Pencere boyutu içeriğe göre
            prompt.ClientSize = new Size(contentWidth + margin * 2, btnSubmit.Bottom + 15);

            prompt.Controls.Add(lblTitle);
            prompt.Controls.Add(txtMessage);
            prompt.Controls.Add(btnCancel);
            prompt.Controls.Add(btnSubmit);
            prompt.AcceptButton = btnSubmit;
            prompt.CancelButton = btnCancel;

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                string userMessage = txtMessage.Text.Trim();

                if (string.IsNullOrEmpty(userMessage))
                {
                    MessageBox.Show(
                        isEng ? "Please do not leave the description empty." : "Lütfen açıklama alanını boş bırakmayınız.",
                        isEng ? "Warning" : "Uyarı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var template = SupportTemplateService.GetTemplate(supportType, LoggedInUser, userMessage);
                ExecuteMailAction(template.Subject, template.Body);
            }
        }

        private void genelIletisimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSupportPrompt(SupportTemplateService.SupportType.GeneralContact);
        }

        private void tesekkurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSupportPrompt(SupportTemplateService.SupportType.ThankYou);
        }

        private void whatsAppIleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isEng = LanguageService.CurrentLanguage == "English";

            string userName = LoggedInUser?.Name ?? "-";
            string message = isEng
                ? $"Hello, I am contacting you via StockMaster. Username: {userName}"
                : $"Merhaba, StockMaster üzerinden ulaşıyorum. Kullanıcı adım: {userName}";

            string url = $"https://wa.me/{SupportPhone}?text={Uri.EscapeDataString(message)}";
            OpenContactLink(url, isEng);
        }

        private void cagriBaslatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isEng = LanguageService.CurrentLanguage == "English";

            var confirm = MessageBox.Show(
                isEng ? $"Call our support line?\n\n+{FormatPhone(SupportPhone)}"
                      : $"Destek hattımız aransın mı?\n\n+{FormatPhone(SupportPhone)}",
                isEng ? "Call Us" : "Bizi Arayın",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            OpenContactLink($"tel:+{SupportPhone}", isEng);
        }

        // Linki varsayılan uygulamayla açar; açılamazsa numarayı panoya kopyalar
        private void OpenContactLink(string uri, bool isEng)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(uri) { UseShellExecute = true });
            }
            catch (Exception)
            {
                Clipboard.SetText("+" + SupportPhone);
                MessageBox.Show(
                    isEng ? $"No app was found to open this. The number has been copied to the clipboard:\n\n+{FormatPhone(SupportPhone)}"
                          : $"Bunu açacak bir uygulama bulunamadı. Numara panoya kopyalandı:\n\n+{FormatPhone(SupportPhone)}",
                    isEng ? "Info" : "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // 905321234567 → 90 532 123 45 67
        private static string FormatPhone(string digits) =>
            digits.Length == 12
                ? $"{digits[..2]} {digits.Substring(2, 3)} {digits.Substring(5, 3)} {digits.Substring(8, 2)} {digits.Substring(10, 2)}"
                : digits;
    }
}
