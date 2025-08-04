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
        private EmailService emailService;
        private UserService _userService;
        public string enteredUserName = "";

        public RegisterScreen(string selectedLanguage)
        {
            InitializeComponent();

            this.selectedLanguage = selectedLanguage;

            cmbLanguage.Items.Add("Türkçe");
            cmbLanguage.Items.Add("English");

            cmbLanguage.SelectedIndex = selectedLanguage == "Türkçe" ? 0 : 1;


            LanguageService.LanguageChanged += UpdateLanguage;
            UpdateLanguage();

            emailService = new EmailService("stockmasterapp@gmail.com", "bfbi cpom gikz azjx");
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

            this.Text = LanguageService.GetString("Kayıt Ekranı");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtMail.Clear();
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan alınan bilgiler
            string name = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string email = txtMail.Text.Trim();
            string language = cmbLanguage.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("İsim boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Şifre boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Şifre en az 6 karakter olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Mail adresi boş bırakılamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);

                User newUser = new User(name, email, password, language);

                string resultMessage = _userService.RegisterUser(newUser);

                MessageBox.Show(resultMessage);

            }
            catch (FormatException)
            {
                MessageBox.Show("Geçersiz mail adresi formatı. Lütfen doğru bir mail adresi giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
            // Kullanıcı adı ve doğrulama kodunu al
            string userName = txtVerificationName.Text.Trim();
            string verificationCodeEntered = txtVerificationCode.Text.Trim();

            // Kullanıcıyı veritabanından veya JSON listesinden bul
            List<User> users = _userService.GetAllUsers();
            User user = users.FirstOrDefault(u => u.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                MessageBox.Show("Kullanıcı bulunamadı. Lütfen doğru adı girin.");
                return;
            }

            if (string.IsNullOrWhiteSpace(verificationCodeEntered))
            {
                MessageBox.Show("Doğrulama kodu boş olamaz.");
                return;
            }

            // VerificationCode ile kullanıcının girdiği kodu karşılaştır
            if (user.VerificationCode == verificationCodeEntered)
            {
                user.IsVerified = true;  // ✅ Kullanıcıyı doğrula

                // Değişiklikleri kaydet
                _userService.SaveAllUsers(users);

                MessageBox.Show($"{userName} kullanıcısı başarıyla doğrulandı!");

            }
            else
            {
                MessageBox.Show("Doğrulama kodu yanlış.");
            }
        }

        private void btnVerificationConfirm_Click(object sender, EventArgs e)
        {
            VerifyAccountForUser();
        }
    }
}
