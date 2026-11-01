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
        UserService userService = new UserService();



        public UserInfoScreen(string selectedLanguage)
        {
            InitializeComponent();
            dgwLogs.CellDoubleClick += dgwLogs_CellDoubleClick;

            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            LanguageService.SetLanguage(LanguageService.CurrentLanguage);

            this.FormClosed += (s, e) =>
            {
                LanguageService.LanguageChanged -= LanguageService_LanguageChanged;
                LanguageService.LanguageChanged -= UpdateLanguage;
                timerMailScroll.Stop();
                timerUsernameScroll2.Stop();
            };
        }

        private void LanguageService_LanguageChanged()
        {
            UpdateFormTexts();
            LoadLoginHistory(LoggedInUser);
            LoadActivityHistory();
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

            FillCargoCompanies();
            SetUsernameVisibility(false);
            SetMailVisibility(false);
            SetPasswordVisibility(false);
            StartMailTextScrollIfNeeded();
            InitializeUsernameScrollTimer();

            rdbCompanies.Checked = true;

            try
            {
                LoadActivityHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(LanguageService.T("Etkinlik geçmişi yüklenemedi: ", "Activity history could not be loaded: ") + ex.Message,
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            nmrRemainingDays.Value = LoggedInUser.DaysLeft;

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
                cmbCompanies.Items.Add(LanguageService.T("- Diğer -", "- Other -"));
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

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            bool tr = LanguageService.CurrentLanguage == "Türkçe";
            string newPassword = txtPassword.Text.Trim();

            if (newPassword.Length < 6)
            {
                MessageBox.Show(tr ? "Şifre en az 6 karakter olmalıdır." : "Password must be at least 6 characters.",
                    tr ? "Uyarı" : "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(tr ? "Şifrenizi değiştirmek istediğinize emin misiniz?" : "Are you sure you want to change your password?",
                    tr ? "Onay" : "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var (success, message) = userService.ChangePassword(LoggedInUser.Name, newPassword);
            MessageBox.Show(message, success ? (tr ? "Bilgi" : "Info") : (tr ? "Uyarı" : "Warning"),
                MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (!success) return;

            btnUpdate.Enabled = false;
            try
            {
                await new EmailService().SendPasswordChangedEmailAsync(LoggedInUser.Email, LoggedInUser.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show((tr ? "Bilgilendirme e-postası gönderilemedi: " : "Notification e-mail could not be sent: ") + ex.Message,
                    tr ? "Hata" : "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            userService.LogOut();
            Application.Restart();
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
                selectedCargo = cmbCompanies.SelectedItem.ToString();
            else if (rdbOther.Checked && !string.IsNullOrWhiteSpace(txtCargoOther.Text))
                selectedCargo = txtCargoOther.Text.Trim();

            if (string.IsNullOrEmpty(selectedCargo))
            {
                MessageBox.Show(LanguageService.T("Lütfen geçerli bir kargo bilgisi girin.", "Please enter a valid cargo company."),
                    LanguageService.T("Uyarı", "Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(LanguageService.T("Kargo bilginizi güncellemek istediğinize emin misiniz?", "Are you sure you want to update your cargo company?"),
                    LanguageService.T("Onay", "Confirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            LoggedInUser.Cargo = selectedCargo;

            if (userService.UpdateCargoCompany(LoggedInUser))
            {
                MessageBox.Show(LanguageService.T("Kargo bilginiz başarıyla güncellendi.", "Your cargo company has been updated."),
                    LanguageService.T("Bilgi", "Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCargo.Text = selectedCargo;
            }
            else
            {
                MessageBox.Show(LanguageService.T("Kargo bilgisi güncellenemedi.", "The cargo company could not be updated."),
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLanguageUpdate_Click(object sender, EventArgs e)
        {
            string selectedLanguage = cmbLanguage.SelectedIndex == 0 ? "Türkçe" : "English";

            var result = MessageBox.Show(
                LanguageService.T(
                    $"Dil tercihinizi \"{selectedLanguage}\" olarak değiştirmek istediğinize emin misiniz?\nUygulama yeniden başlatılacaktır.",
                    $"Are you sure you want to change your language preference to \"{selectedLanguage}\"?\nThe application will restart."),
                LanguageService.T("Dil Güncelleme Onayı", "Language Update"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            if (UserService.LoggedInUser == null)
            {
                MessageBox.Show(LanguageService.T("Aktif kullanıcı bulunamadı!", "No active user found!"),
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UserService.LoggedInUser.Language = selectedLanguage;

            var allUsers = userService.GetAllUsers();
            var existingUser = allUsers.FirstOrDefault(u =>
                u.Email.Equals(UserService.LoggedInUser.Email, StringComparison.OrdinalIgnoreCase));

            if (existingUser == null)
            {
                MessageBox.Show(LanguageService.T("Kullanıcı bulunamadı. Güncelleme başarısız.", "User not found. Update failed."),
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            existingUser.Language = selectedLanguage;
            userService.SaveAllUsers(allUsers);
            ActivityLogService.Log(ActivityType.LanguageChanged, new Dictionary<string, string> { ["Language"] = selectedLanguage });

            MessageBox.Show(LanguageService.T("Dil tercihiniz güncellendi. Uygulama yeniden başlatılıyor...", "Your language preference has been updated. Restarting..."),
                LanguageService.T("Başarılı", "Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
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
            string serialKey = txtSerialKey.Text;
            int daysToAdd = UserService.GetSerialKeyDays(serialKey);

            if (daysToAdd == 0)
            {
                MessageBox.Show(LanguageService.T("Geçersiz serial key!", "Invalid serial key!"),
    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(LanguageService.T($"Bu serial key {daysToAdd} gün ekleyecektir.\nOnaylıyor musunuz?",
                                                  $"This serial key will add {daysToAdd} days.\nDo you confirm?"),
                    LanguageService.T("Serial Key Onayı", "Serial Key Confirmation"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var (success, message) = userService.ApplySerialKey(serialKey);
            MessageBox.Show(message, success ? "Başarılı" : "Hata", MessageBoxButtons.OK,
                success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (!success) return;

            nmrRemainingDays.Value = LoggedInUser.DaysLeft;
            txtSerialKey.Clear();

            if (this.Owner is MainMenu mainMenu)
                mainMenu.UserAccessControl();
        }

        // ── Etkinlik Geçmişi ──
        private static string FormatActivityDate(DateTime date, bool isTurkish) =>
            isTurkish
                ? date.ToString("dd.MM.yyyy HH:mm")
                : date.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"));

        private void LoadActivityHistory()
        {
            if (LoggedInUser == null) return;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
            var activities = ActivityLogService.GetForUser(LoggedInUser.Name);

            dgwLogs.Rows.Clear();
            dgwLogs.Columns.Clear();

            dgwLogs.ReadOnly = true;
            dgwLogs.AllowUserToAddRows = false;
            dgwLogs.AllowUserToDeleteRows = false;
            dgwLogs.AllowUserToResizeRows = false;
            dgwLogs.RowHeadersVisible = false;
            dgwLogs.MultiSelect = false;
            dgwLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgwLogs.Columns.Add("Date", isTurkish ? "Tarih" : "Date");
            dgwLogs.Columns.Add("Activity", isTurkish ? "İşlem" : "Activity");
            dgwLogs.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgwLogs.Columns["Activity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (var activity in activities)
            {
                int rowIndex = dgwLogs.Rows.Add(
                    FormatActivityDate(activity.Date, isTurkish),
                    ActivityText.Summary(activity, isTurkish));

                dgwLogs.Rows[rowIndex].Tag = activity;   // çift tıklamada detay için
                dgwLogs.Rows[rowIndex].Cells["Activity"].ToolTipText = isTurkish
                    ? "Detaylar için çift tıklayın"
                    : "Double-click for details";
            }
        }

        private void dgwLogs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;   // başlık satırına tıklandıysa

            if (dgwLogs.Rows[e.RowIndex].Tag is ActivityLog activity)
                ShowActivityDetails(activity);
        }

        private void ShowActivityDetails(ActivityLog activity)
        {
            bool tr = LanguageService.CurrentLanguage == "Türkçe";

            using var form = new Form
            {
                Text = ActivityText.Title(activity.Type, tr),
                Size = new Size(480, 360),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                TopMost = this.TopMost
            };

            var lvDetails = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                ShowItemToolTips = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable
            };
            lvDetails.Columns.Add(tr ? "Alan" : "Field", 150);
            lvDetails.Columns.Add(tr ? "Değer" : "Value", 300);

            void AddRow(string field, string value)
            {
                var item = new ListViewItem(field) { ToolTipText = value };
                item.SubItems.Add(value);
                lvDetails.Items.Add(item);
            }

            AddRow(tr ? "İşlem" : "Activity", ActivityText.Title(activity.Type, tr));
            AddRow(tr ? "Tarih" : "Date", FormatActivityDate(activity.Date, tr));
            AddRow(tr ? "Kullanıcı" : "User", activity.UserName);

            foreach (var entry in activity.Data)
                AddRow(ActivityText.FieldLabel(entry.Key, tr), entry.Value);

            var btnClose = new Button
            {
                Text = tr ? "Kapat" : "Close",
                Dock = DockStyle.Bottom,
                Height = 34,
                DialogResult = DialogResult.OK
            };

            // Sıra önemli: önce Fill, sonra Bottom eklenir
            form.Controls.Add(lvDetails);
            form.Controls.Add(btnClose);
            form.AcceptButton = btnClose;

            form.ShowDialog(this);
        }
    }
    
}
