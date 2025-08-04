using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Data.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace neoStockMasterv2.Forms
{
    public partial class UserInfoScreen : Form
    {
        private int mailScrollIndex = 0;
        private string originalMailText = "";

        private string originalUsernameText;
        private int scrollIndex = 0;

        public static User LoggedInUser { get; set; }
        private readonly string senderEmail = "stockmasterapp@gmail.com";
        private readonly string senderPassword = "bfbi cpom gikz azjx";
        UserService userService = new UserService();



        public UserInfoScreen(string selectedLanguage)
        {
            InitializeComponent();

            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            LanguageService.SetLanguage(LanguageService.CurrentLanguage);
        }

        private void LanguageService_LanguageChanged()
        {
            UpdateFormTexts();
            LoadLoginHistory(LoggedInUser);
            FillingMethod();
        }

        private void UpdateFormTexts()
        {
            türkçeToolStripMenuItem.Text = LanguageService.GetString("Türkçe");
            englishToolStripMenuItem.Text = LanguageService.GetString("English2");

            grbAboutUser.Text = LanguageService.GetString("Kullanıcı Bilgileri");
            lblUsername.Text = LanguageService.GetString("İsim");
            lblMail.Text = LanguageService.GetString("Mail");
            lblPassword.Text = LanguageService.GetString("Şifre");
            btnClear.Text = LanguageService.GetString("Temizle");
            btnUpdate.Text = LanguageService.GetString("Güncelle");
            grbCargo.Text = LanguageService.GetString("Anlaşmalı Kargo Şirketi");
            lblCargo.Text = LanguageService.GetString("Kargo");
            lblCompanies.Text = LanguageService.GetString("Şirketler");
            lblOther.Text = LanguageService.GetString("Diğer");
            btnCargoClear.Text = LanguageService.GetString("Temizle");
            btnCargoUpdate.Text = LanguageService.GetString("Güncelle");
            grbHistoryOfActivity.Text = LanguageService.GetString("Etkinlik Geçmişi");
            grbDetailsOfUser.Text = LanguageService.GetString("Kullanıcı Detayları");
            lblDateTimeRegistration.Text = LanguageService.GetString("Kayıt Tarihi");
            lblSinceRegistration.Text = LanguageService.GetString("Kayıtlı Gün Sayısı");
            lblLanguage.Text = LanguageService.GetString("Dil");
            lblRemainingDays.Text = LanguageService.GetString("Kalan Gün Sayısı");
            grbIPHistory.Text = LanguageService.GetString("IP Geçmişi");
            btnLanguageUpdate.Text = LanguageService.GetString("Güncelle");
            grbSerial.Text = LanguageService.GetString("Kullanım Hakları");
            btnSerialClear.Text = LanguageService.GetString("Temizle");
            btnSerialConfirm.Text = LanguageService.GetString("Onayla");


            this.Text = LanguageService.GetString("Kullanıcı Bilgileri");

            UpdateSelectedLanguage();
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

        private void UserInfoScreen_Load(object sender, EventArgs e)
        {

            LanguageService.LanguageChanged += UpdateLanguage;
            UpdateLanguage();
            DisableUserInputs();

            if (LoggedInUser.Language == "Turkish" || LoggedInUser.Language == "Türkçe")
            {
                cmbLanguage.SelectedIndex = 0;
            }
            else if (LoggedInUser.Language == "İngilizce" || LoggedInUser.Language == "English")
            {
                cmbLanguage.SelectedIndex = 1;
            }

            //FillingMethod();
            FillCargoCompanies();
            SetUsernameVisibility(false);
            SetMailVisibility(false);
            SetPasswordVisibility(false);
            StartMailTextScrollIfNeeded();
            InitializeUsernameScrollTimer();

            rdbCompanies.Checked = true;
        }


        private void UpdateLanguage()
        {
            cmbLanguage.Items.Clear();

            if (LanguageService.CurrentLanguage == "English")
            {
                cmbLanguage.Items.Add("Turkish");
                cmbLanguage.Items.Add("English");
            }
            else // Türkçe ise
            {
                cmbLanguage.Items.Add("Türkçe");
                cmbLanguage.Items.Add("İngilizce");
            }

            // Mevcut kullanıcı dili neyse ona göre index seç
            if (LoggedInUser.Language == "Turkish" || LoggedInUser.Language == "Türkçe")
                cmbLanguage.SelectedIndex = 0;
            else if (LoggedInUser.Language == "English" || LoggedInUser.Language == "İngilizce")
                cmbLanguage.SelectedIndex = 1;
        }

        private void FillingMethod()
        {
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // Kullanıcı bilgilerini doldur
            txtUsername.Text = LoggedInUser.Name;
            txtMail.Text = LoggedInUser.Email;

            // Tarih formatını dil seçimine göre ayarla
            string registrationDateFormat = isTurkish ? "dd-MM-yyyy" : "MM/dd/yyyy";
            txtDateTimeOfRegister.Text = LoggedInUser.RegistrationDate.ToString(registrationDateFormat);

            // Kayıtlı gün sayısını hesapla
            TimeSpan difference = DateTime.Now.Date - LoggedInUser.RegistrationDate.Date;
            int registeredDays = difference.Days;
            nmrSinceRegistration.Value = registeredDays;
            LoggedInUser.UsageRemainingDays = registeredDays;

            // Kalan günleri ve diğer bilgileri doldur
            nmrRemainingDays.Value = LoggedInUser.RemainingDays;
            txtCargo.Text = LoggedInUser.Cargo;
        }

        private void DisableUserInputs()
        {
            txtUsername.Enabled = false;
            txtMail.Enabled = false;
            nmrSinceRegistration.Enabled = false;
            nmrRemainingDays.Enabled = false;
            txtDateTimeOfRegister.Enabled = false;
            txtCargo.Enabled = false;
        }

        private void FillCargoCompanies()
        {
            List<string> cargoCompanies = new List<string>()
            {
                "PTT Kargo",
                "Yurtiçi Kargo",
                "MNG Kargo",
                "Aras Kargo",
                "Sürat Kargo",
                "UPS Türkiye",
                "DHL Express Türkiye",
                "FedEx Türkiye",
                "HepsiJET",
                "Trendyol Express",
                "Kolay Gelsin",
                "Kargoist",
                "Getir Lojistik",
                "N11 Lojistik",
                "Amazon Türkiye Lojistik",
                "Ekol Lojistik",
                "Ceva Lojistik",
                "Borusan Lojistik"
            };

            cmbCompanies.Items.Clear();
            cmbCompanies.Items.AddRange(cargoCompanies.ToArray());

            if (cmbCompanies.Items.Count > 0)
                cmbCompanies.SelectedIndex = 0;
        }

        private void SetUsernameVisibility(bool visible)
        {
            txtUsername.UseSystemPasswordChar = !visible;
        }

        private void SetMailVisibility(bool visible)
        {
            txtMail.UseSystemPasswordChar = !visible;
        }

        private void SetPasswordVisibility(bool visible)
        {
            txtPassword.UseSystemPasswordChar = !visible;
        }

        private void chbMail_CheckedChanged(object sender, EventArgs e)
        {
            if (chbMail.Checked)
            {
                txtMail.UseSystemPasswordChar = false;
            }
            else
            {
                txtMail.UseSystemPasswordChar = true;
            }
        }

        private void chbPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chbPassword.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void chbUsername_CheckedChanged(object sender, EventArgs e)
        {
            if (chbUsername.Checked)
            {
                txtUsername.UseSystemPasswordChar = false;
            }
            else
            {
                txtUsername.UseSystemPasswordChar = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPassword.Clear();
        }

        private void rdbCompanies_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCompanies.Checked)
            {
                cmbCompanies.Enabled = true;
                cmbCompanies.SelectedIndex = 0;
                txtCargoOther.Enabled = false;
                txtCargoOther.Text = "";

                FillCargoCompanies();
            }
        }

        private void rdbOther_CheckedChanged(object sender, EventArgs e)
        {

            if (rdbOther.Checked)
            {
                cmbCompanies.Enabled = false;
                cmbCompanies.Items.Clear();
                cmbCompanies.Items.Add("- Diğer -");
                cmbCompanies.SelectedIndex = 0;
                txtCargoOther.Enabled = true;
            }
            else
            {
                cmbCompanies.Enabled = true;
                FillCargoCompanies();
                cmbCompanies.SelectedIndex = 0;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Kullanıcı adı ve mevcut şifreyi almak
            string currentUsername = LoggedInUser.Name; // Mevcut giriş yapan kullanıcının adı
            string currentPassword = LoggedInUser.Password; // Kullanıcının girdiği mevcut şifre
            string newPassword = txtPassword.Text; // Kullanıcının girdiği yeni şifre

            // Eski şifre ile yeni şifre aynı mı kontrolü
            if (currentPassword == newPassword)
            {
                MessageBox.Show("Yeni şifre, mevcut şifreyle aynı olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Şifre değiştirme işlemi için kullanıcıdan onay alma
            DialogResult confirmation = MessageBox.Show("Şifrenizi değiştirmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                // Kullanıcı "Hayır" dediğinde işlem iptal edilir
                return;
            }

            // Kullanıcı hizmetinden şifre değiştirme işlemini başlat
            string result = userService.ChangePassword(currentUsername, currentPassword, newPassword);

            // Sonuç mesajını göstermek
            MessageBox.Show(result);

            // Eğer şifre başarıyla değiştirildiyse, text boxları temizle
            if (result == "Şifreniz başarıyla değiştirildi!")
            {
                try
                {
                    // E-posta gönderme işlemi
                    EmailService emailService = new EmailService(senderEmail, senderPassword);
                    emailService.SendPasswordChangedEmail(LoggedInUser.Email, LoggedInUser.Name);
                }
                catch (Exception ex)
                {
                    // E-posta gönderiminde hata oluştuysa kullanıcıyı bilgilendir
                    MessageBox.Show($"E-posta gönderilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Kullanıcıyı oturumdan çıkart
                userService.LogOut();

                Application.Restart();

                this.Close();
            }
        }

        private void btnCargoClear_Click(object sender, EventArgs e)
        {
            txtCargoOther.Clear();
        }

        private void StartMailTextScrollIfNeeded()
        {
            originalMailText = txtMail.Text;

            // Eğer metin, TextBox genişliğinden uzunsa kaydırma başlat
            if (TextRenderer.MeasureText(originalMailText, txtMail.Font).Width > txtMail.Width)
            {
                timerMailScroll.Start();
            }
        }

        private void InitializeUsernameScrollTimer()
        {
            originalUsernameText = txtUsername.Text;

            // Eğer metin, TextBox genişliğinden uzunsa kaydırma başlat
            if (TextRenderer.MeasureText(originalUsernameText, txtUsername.Font).Width > txtUsername.Width)
            {
                timerUsernameScroll2.Start();
            }
        }


        private void timerMailScroll_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(originalMailText)) return;

            // Metni baştan sona doğru kaydır
            if (mailScrollIndex >= originalMailText.Length)
            {
                mailScrollIndex = 0;
            }

            string scrolled = originalMailText.Substring(mailScrollIndex) + "   " + originalMailText.Substring(0, mailScrollIndex);
            txtMail.Text = scrolled;

            mailScrollIndex++;
        }

        private void timerUsernameScroll2_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(originalUsernameText)) return;

            // Metni baştan sona doğru kaydır
            if (scrollIndex >= originalUsernameText.Length)
            {
                scrollIndex = 0;
            }

            string scrolled = originalUsernameText.Substring(scrollIndex) + "   " + originalUsernameText.Substring(0, scrollIndex);
            txtUsername.Text = scrolled;

            scrollIndex++;
        }


        private void btnCargoUpdate_Click(object sender, EventArgs e)
        {
            string selectedCargo = "";

            if (rdbCompanies.Checked && cmbCompanies.SelectedItem != null)
            {
                selectedCargo = cmbCompanies.SelectedItem.ToString();
            }
            else if (rdbOther.Checked && !string.IsNullOrWhiteSpace(txtCargoOther.Text))
            {
                selectedCargo = txtCargoOther.Text.Trim();
            }

            if (string.IsNullOrEmpty(selectedCargo))
            {
                MessageBox.Show("Lütfen geçerli bir kargo bilgisi girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıya değişikliği onaylat
            DialogResult result = MessageBox.Show("Kargo bilginizi güncellemek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            // Kullanıcının kargo bilgisi güncelleniyor
            LoggedInUser.Cargo = selectedCargo;

            // Veritabanı ya da dosya sistemine kayıt
            bool updateResult = userService.UpdateCargoCompany(LoggedInUser);

            if (updateResult)
            {
                MessageBox.Show("Kargo bilginiz başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCargo.Text = selectedCargo;
            }
            else
            {
                MessageBox.Show("Kargo bilgisi güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLanguageUpdate_Click(object sender, EventArgs e)
        {
            string selectedLanguage = cmbLanguage.SelectedIndex == 0 ? "Türkçe" : "English";

            DialogResult result = MessageBox.Show(
                $"Dil tercihinizi \"{selectedLanguage}\" olarak değiştirmek istediğinize emin misiniz?\nUygulama yeniden başlatılacaktır.",
                "Dil Güncelleme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                if (UserService.LoggedInUser != null)
                {
                    // LoggedInUser içindeki dili güncelle
                    UserService.LoggedInUser.Language = selectedLanguage;

                    // users.json'daki eşleşen kullanıcıyı bulup güncelle
                    var allUsers = userService.GetAllUsers();
                    var existingUser = allUsers.FirstOrDefault(u =>
                        u.Email.Equals(UserService.LoggedInUser.Email, StringComparison.OrdinalIgnoreCase));

                    if (existingUser != null)
                    {
                        existingUser.Language = selectedLanguage;
                        userService.SaveAllUsers(allUsers);

                        MessageBox.Show("Dil tercihiniz güncellendi. Uygulama yeniden başlatılıyor...", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Restart();
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı bulunamadı. Güncelleme başarısız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Aktif kullanıcı bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Dil güncelleme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadLoginHistory(User user)
        {
            // Yeni bir tablo oluştur
            DataTable table = new DataTable();

            // Dil kontrolü
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // Sütunları ekle
            table.Columns.Add(isTurkish ? "IP" : "IP");
            table.Columns.Add(isTurkish ? "Giriş Tarihi" : "Login Date");

            foreach (var record in user.LoginHistory)
            {
                string formattedDate = isTurkish
                    ? record.LoginDate.ToString("dd.MM.yyyy - HH:mm")
                    : record.LoginDate.ToString("MM/dd/yyyy - hh:mm tt", new CultureInfo("en-US"));

                table.Rows.Add(record.IP, formattedDate);
            }

            // DataGridView'e bağla
            dgwIPHistory.DataSource = table;

            // Otomatik sütun genişliği ayarı (isteğe bağlı)
            dgwIPHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dgwIPHistory.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgwIPHistory.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void btnSerialClear_Click(object sender, EventArgs e)
        {
            txtSerialKey.Clear();
        }

        private void btnSerialConfirm_Click(object sender, EventArgs e)
        {
            string serialKey = txtSerialKey.Text.Trim().ToUpper();

            int daysToAdd = serialKey switch
            {
                "TRIAL7" => 7,
                "FULL30" => 30,
                "VIP90" => 90,
                _ => 0
            };

            if (daysToAdd == 0)
            {
                MessageBox.Show("Geçersiz serial key!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Onay sorusu
            DialogResult result = MessageBox.Show(
                $"Bu serial key {daysToAdd} gün ekleyecektir.\nOnaylıyor musunuz?",
                "Serial Key Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
            {
                MessageBox.Show("İşlem iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Gün ekle
            UserService.LoggedInUser.RemainingDays += daysToAdd;

            // Kullanıcıyı JSON dosyasında da güncelle
            var allUsers = userService.GetAllUsers();

            var userToUpdate = allUsers.FirstOrDefault(u => u.Email.Equals(UserService.LoggedInUser.Email, StringComparison.OrdinalIgnoreCase));
            if (userToUpdate != null)
            {
                userToUpdate.RemainingDays = UserService.LoggedInUser.RemainingDays;
                userService.SaveAllUsers(allUsers);

                nmrRemainingDays.Value = UserService.LoggedInUser.RemainingDays;

                MessageBox.Show($"{daysToAdd} gün başarıyla eklendi!\nYeni kalan süreniz: {userToUpdate.RemainingDays} gün", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 👉 MainMenu formuna erişim kontrolü tetikle
                if (this.Owner is MainMenu mainMenu)
                {
                    mainMenu.UserAccessControl();
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
