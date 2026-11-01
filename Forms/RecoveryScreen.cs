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
    public partial class RecoveryScreen : Form
    {
        public event Action<string> LanguageChanged;
        string selectedLanguage;

        public RecoveryScreen(string language)
        {
            InitializeComponent();

            selectedLanguage = language;
            LanguageService.SetLanguage(selectedLanguage);

            if (selectedLanguage == "Türkçe")
            {
                menuStripLanguage.Items[0].PerformClick(); // 1. öğeyi seçili yap
            }
            else if (selectedLanguage == "English")
            {
                menuStripLanguage.Items[1].PerformClick(); // 2. öğeyi seçili yap
            }

            LanguageService.LanguageChanged += UpdateLanguage;
            this.FormClosed += (s, e) => LanguageService.LanguageChanged -= UpdateLanguage;
            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            grbMail.Text = LanguageService.GetString("E-Mail Adresinizi Yazınız");
            grbActivity.Text = LanguageService.GetString("İşlemi Seç");
            btnForgetUsername.Text = LanguageService.GetString("Kullanıcı İsmini Hatırlat");
            btnForgetPassword.Text = LanguageService.GetString("Şifremi Hatırlat");

            this.Text = LanguageService.GetString("Bilgilerimi Hatırlat");
        }

        private void türkçeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("Türkçe");
            türkçeToolStripMenuItem.BackColor = Color.LightBlue;
            englishToolStripMenuItem.BackColor = SystemColors.Control;

            LanguageChanged?.Invoke("Türkçe");
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("English");
            englishToolStripMenuItem.BackColor = Color.LightBlue;
            türkçeToolStripMenuItem.BackColor = SystemColors.Control;

            LanguageChanged?.Invoke("English");
        }

        private async void btnForgetUsername_Click(object sender, EventArgs e)
        {
            string email = GetValidatedEmail();
            if (email == null) return;

            var user = new UserService().FindUserByEmail(email);
            if (user == null)
            {
                MessageBox.Show(LanguageService.T("Bu e-posta adresine ait kullanıcı bulunamadı.", "No user was found for this e-mail address."),
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnForgetUsername.Enabled = false;
            try
            {
                await new EmailService().SendForgotUsernameEmailAsync(email, user.Name);
                MessageBox.Show(LanguageService.T("Kullanıcı adınız e-posta adresinize gönderildi.", "Your username has been sent to your e-mail address."),
                    LanguageService.T("Bilgi", "Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(LanguageService.T("E-posta gönderilemedi: ", "The e-mail could not be sent: ") + ex.Message,
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnForgetUsername.Enabled = true;
            }
        }

        private async void btnForgetPassword_Click(object sender, EventArgs e)
        {
            string email = GetValidatedEmail();
            if (email == null) return;

            var confirm = MessageBox.Show(
                LanguageService.T(
                    "Hesabınız için yeni bir geçici şifre oluşturulup e-posta adresinize gönderilecek. Devam edilsin mi?",
                    "A new temporary password will be created for your account and sent to your e-mail address. Continue?"),
                LanguageService.T("Şifre Sıfırlama", "Password Reset"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            btnForgetPassword.Enabled = false;
            try
            {
                var (success, message) = await new UserService().ResetPasswordAsync(email);
                MessageBox.Show(message,
                    success ? LanguageService.T("Bilgi", "Info") : LanguageService.T("Hata", "Error"),
                    MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(LanguageService.T("E-posta gönderilemedi, şifreniz değişmedi: ", "The e-mail could not be sent, your password was not changed: ") + ex.Message,
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnForgetPassword.Enabled = true;
            }
        }

        private string GetValidatedEmail()
        {
            if (!CheckMailField()) return null;

            try
            {
                return new System.Net.Mail.MailAddress(txtMail.Text.Trim()).Address;
            }
            catch (FormatException)
            {
                MessageBox.Show(LanguageService.T("Geçersiz mail adresi formatı. Lütfen doğru bir mail adresi giriniz.", "Invalid e-mail format. Please enter a valid e-mail address."),
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private bool CheckMailField()
        {
            if (string.IsNullOrWhiteSpace(txtMail.Text))
            {
                MessageBox.Show(LanguageService.T("Mail alanı boş bırakılamaz.", "The e-mail field cannot be empty."),
                    LanguageService.T("Uyarı", "Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMail.Focus();
                return false;
            }
            return true;
        }
    }
}
