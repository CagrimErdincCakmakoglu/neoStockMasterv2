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
    public partial class RegisterScreen : Form
    {
        private string selectedLanguage;
        private UserService _userService;
        private CheckBox chbShowPassword;
        private readonly ToolTip _ttPassword = new ToolTip();

        public RegisterScreen(string selectedLanguage)
        {
            InitializeComponent();

            // Şifre gizli başlasın + göster/gizle kutusu
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.Width = 180;

            chbShowPassword = new CheckBox
            {
                AutoSize = true,
                Location = new Point(txtPassword.Right + 6, txtPassword.Top + 4)
            };
            chbShowPassword.CheckedChanged += (s, e) =>
                txtPassword.UseSystemPasswordChar = !chbShowPassword.Checked;
            grbUserInfo.Controls.Add(chbShowPassword);

            this.selectedLanguage = selectedLanguage;

            cmbLanguage.Items.Add("Türkçe");
            cmbLanguage.Items.Add("English");

            cmbLanguage.SelectedIndex = selectedLanguage == "Türkçe" ? 0 : 1;


            LanguageService.LanguageChanged += UpdateLanguage;
            this.FormClosed += (s, e) => LanguageService.LanguageChanged -= UpdateLanguage;
            UpdateLanguage();

            _userService = new UserService();
        }

        private void UpdateLanguage()
        {
            lblName.Text = LanguageService.GetString("İsim");
            lblPassword.Text = LanguageService.GetString("Şifre");
            lblMail.Text = LanguageService.GetString("Mail");
            lblLanguage.Text = LanguageService.GetString("Dil");
            btnClear.Text = LanguageService.GetString("Temizle");
            btnRegister.Text = LanguageService.GetString("Kayıt Ol");
            grbUserInfo.Text = LanguageService.GetString("Kullanıcı Bilgileri");
            grbVerification.Text = LanguageService.GetString("Kullanıcı Onaylama");
            lblVerificationCode.Text = LanguageService.GetString("Kod");
            btnVerificationClear.Text = LanguageService.GetString("Temizle");
            btnVerificationConfirm.Text = LanguageService.GetString("Onayla");
            lblVerificationName.Text = LanguageService.GetString("İsim");
            llContract.Text = LanguageService.GetString("Kullanıcı Sözleşmesini okudum, kabul ediyorum.");

            this.Text = LanguageService.GetString("Kayıt Ekranı");

            _ttPassword.SetToolTip(chbShowPassword, LanguageService.T("Şifreyi göster", "Show password"));
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtMail.Clear();
            chbContract.Checked = false;
            chbShowPassword.Checked = false;
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedLanguage = cmbLanguage.SelectedItem.ToString();
            LanguageService.SetLanguage(selectedLanguage);

            // Dil değiştiğinde, LoginScreen formundaki cmbLanguage'ı da güncelleyeceğiz.
            var loginScreen = Application.OpenForms["LoginScreen"] as LoginScreen;
            if (loginScreen != null)
            {
                loginScreen.UpdateLanguageComboBox(selectedLanguage);
            }
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            if (!chbContract.Checked)
            {
                // Dil servisinden anahtarları çekmeye çalışıyoruz
                string rawMsg = LanguageService.GetString("SözleşmeOnayUyarısı");
                string rawTitle = LanguageService.GetString("SözleşmeOnayBaşlığı");

                string warningMsg;
                string warningTitle;

                // Mevcut seçili dile göre kurumsal mesajları ve başlıkları belirliyoruz
                if (cmbLanguage.Text == "English")
                {
                    warningMsg = !string.IsNullOrEmpty(rawMsg) ? rawMsg : "You must read and accept the End-User License Agreement (EULA) to proceed with registration.";
                    warningTitle = !string.IsNullOrEmpty(rawTitle) ? rawTitle : "Action Required";
                }
                else // Varsayılan veya Türkçe seçeneği
                {
                    warningMsg = !string.IsNullOrEmpty(rawMsg) ? rawMsg : "Devam edebilmek için Kullanıcı Sözleşmesi ve Hizmet Şartları'nı okuyup onaylamanız gerekmektedir.";
                    warningTitle = !string.IsNullOrEmpty(rawTitle) ? rawTitle : "Sözleşme Onayı Gerekli";
                }

                MessageBox.Show(warningMsg, warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıdan alınan bilgiler
            string name = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string email = txtMail.Text.Trim();
            string language = cmbLanguage.Text;
            string errorTitle = LanguageService.T("Hata", "Error");

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(LanguageService.T("İsim boş bırakılamaz.", "Name cannot be empty."), errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(LanguageService.T("Şifre boş bırakılamaz.", "Password cannot be empty."), errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show(LanguageService.T("Şifre en az 6 karakter olmalıdır.", "Password must be at least 6 characters."), errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show(LanguageService.T("Mail adresi boş bırakılamaz.", "E-mail address cannot be empty."), errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                _ = new System.Net.Mail.MailAddress(email);   // format kontrolü
            }
            catch (FormatException)
            {
                MessageBox.Show(LanguageService.T("Geçersiz mail adresi formatı. Lütfen doğru bir mail adresi giriniz.", "Invalid e-mail format. Please enter a valid e-mail address."),
                    errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnRegister.Enabled = false;   // mail giderken çift tıklamayı engelle

            try
            {
                User newUser = new User(name, email, password, language);
                string resultMessage = await _userService.RegisterUserAsync(newUser);
                MessageBox.Show(resultMessage);
            }
            finally
            {
                btnRegister.Enabled = true;
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            txtVerificationName.Text = txtUsername.Text;
        }

        private void RegisterScreen_Load(object sender, EventArgs e)
        {
            txtVerificationName.Enabled = false;
        }

        private void btnVerificationClear_Click(object sender, EventArgs e)
        {
            txtVerificationCode.Clear();
        }

        private void VerifyAccountForUser()
        {
            string userName = txtVerificationName.Text.Trim();
            string verificationCodeEntered = txtVerificationCode.Text.Trim();

            List<User> users = _userService.GetAllUsers();
            User user = users.FirstOrDefault(u => u.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                MessageBox.Show(LanguageService.T("Kullanıcı bulunamadı. Lütfen doğru adı girin.", "User not found. Please enter the correct name."));
                return;
            }

            if (string.IsNullOrWhiteSpace(verificationCodeEntered))
            {
                MessageBox.Show(LanguageService.T("Doğrulama kodu boş olamaz.", "Verification code cannot be empty."));
                return;
            }

            if (user.VerificationCode == verificationCodeEntered)
            {
                user.IsVerified = true;
                _userService.SaveAllUsers(users);

                MessageBox.Show(LanguageService.T($"{userName} kullanıcısı başarıyla doğrulandı!", $"User {userName} has been verified successfully!"));
            }
            else
            {
                MessageBox.Show(LanguageService.T("Doğrulama kodu yanlış.", "Wrong verification code."));
            }
        }

        private void btnVerificationConfirm_Click(object sender, EventArgs e)
        {
            VerifyAccountForUser();
        }

        private void llContract_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Dil servisinden veriyi çekiyoruz
            bool isEnglish = cmbLanguage.Text == "English";

            string rawTitle = isEnglish
                ? LanguageService.GetString("UserAgreement")   // dil dosyasında "User Agreement" değeri
                : LanguageService.GetString("Kullanıcı Sözleşmesi");

            string rawText = LanguageService.GetString("UserAgreementText");

            string agreementTitle = !string.IsNullOrEmpty(rawTitle)
                ? rawTitle
                : (isEnglish ? "User Agreement" : "Kullanıcı Sözleşmesi");

            string agreementText = !string.IsNullOrEmpty(rawText)
                ? rawText
                : LanguageService.T("Sözleşme metni yüklenemedi. Lütfen sistem yöneticisi ile iletişime geçin.",
                    "The agreement text could not be loaded. Please contact the system administrator.");
            // 1. Ana Formun Modernize Edilmesi
            Form agreementForm = new Form();
            agreementForm.Text = agreementTitle;
            agreementForm.Size = new Size(550, 500);
            agreementForm.StartPosition = FormStartPosition.CenterParent;
            agreementForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            agreementForm.MaximizeBox = false;
            agreementForm.MinimizeBox = false;
            agreementForm.BackColor = Color.FromArgb(245, 247, 250); // Açık modern gri/mavi arka plan

            // 2. Üst Banner (Header) Alanı
            Panel pnlHeader = new Panel();
            pnlHeader.Height = 60;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.BackColor = Color.FromArgb(41, 56, 78); // Kurumsal Koyu Lacivert/Griton
            pnlHeader.Padding = new Padding(20, 0, 0, 0);

            Label lblHeaderTitle = new Label();
            lblHeaderTitle.Text = agreementTitle.ToUpper();
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            lblHeaderTitle.AutoSize = false;
            lblHeaderTitle.Dock = DockStyle.Fill;
            lblHeaderTitle.TextAlign = ContentAlignment.MiddleLeft;
            pnlHeader.Controls.Add(lblHeaderTitle);

            // 3. Alt Buton Alanı (Footer)
            Panel pnlFooter = new Panel();
            pnlFooter.Height = 60;
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.BackColor = Color.FromArgb(235, 238, 243);

            Button btnClose = new Button();
            btnClose.Text = LanguageService.GetString("Anladım, Kapat"); // Veya "Kapat"
            btnClose.Size = new Size(130, 36);
            btnClose.Location = new Point(agreementForm.Width - 160, 12);
            btnClose.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.BackColor = Color.FromArgb(0, 122, 255); // Modern iOS/Windows Mavisi
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0; // Kenarlıkları kaldırarak flat görünüm sağladık
            btnClose.Cursor = Cursors.Hand;
            btnClose.DialogResult = DialogResult.OK; // Tıklanınca formu kapatması için
            pnlFooter.Controls.Add(btnClose);

            // 4. Metin Alanı İçin İç Panel (Padding/Boşluk Amacıyla)
            Panel pnlContent = new Panel();
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Padding = new Padding(20, 20, 20, 20); // Metnin kenarlara yapışmasını engeller

            // RichTextBox kullanarak daha temiz bir metin görünümü ve modern kenarlıklar elde ediyoruz
            RichTextBox rtbAgreement = new RichTextBox();
            rtbAgreement.ReadOnly = true;
            rtbAgreement.Dock = DockStyle.Fill;
            rtbAgreement.Text = agreementText;
            rtbAgreement.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            rtbAgreement.ForeColor = Color.FromArgb(45, 55, 72); // Koyu gri metin rengi (Gözü yormaz)
            rtbAgreement.BackColor = Color.White;
            rtbAgreement.BorderStyle = BorderStyle.None; // Çirkin 3D kenarlıkları kaldırıyoruz
            rtbAgreement.ScrollBars = RichTextBoxScrollBars.Vertical;

            // Metin paneline RichTextBox'ı ekliyoruz
            pnlContent.Controls.Add(rtbAgreement);

            // 5. Kontrolleri Forma Sırasıyla Ekleme
            agreementForm.Controls.Add(pnlContent);
            agreementForm.Controls.Add(pnlHeader);
            agreementForm.Controls.Add(pnlFooter);

            // Formu göster
            agreementForm.ShowDialog();
        }
    }
}
