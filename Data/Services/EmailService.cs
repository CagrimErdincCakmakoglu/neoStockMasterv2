using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace neoStockMasterv2.Data.Services
{
    public class EmailService
    {
        private readonly OrderService _orderService;
        private readonly UserService _userService;
        private readonly string smtpClientAddress = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string senderEmail = "stockmasterapp@gmail.com";
        private readonly string senderPassword = "bfbi cpom gikz azjx";

        public EmailService(string email, string password)
        {
            senderEmail = email;
            senderPassword = password;
            _orderService = new OrderService();
        }

        public void SendWelcomeEmail(string recipientEmail, string recipientName, string verificationCode)
        {
            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, "StockMaster"),
                    Subject = "Hoş Geldiniz!",
                    Body = $"Merhaba {recipientName},\n\n" +
                           $"Uygulamaya kayıt olduğunuz için teşekkür ederiz! Sizleri aramızda görmekten mutluluk duyuyoruz.\n\n" +
                           $"Hesabınızı doğrulamak için aşağıdaki onay kodunu kullanabilirsiniz:\n\n" +
                           $"**{verificationCode}**\n\n" +
                           $"Bu kodu kullanarak hesabınızı aktif hale getirebilirsiniz.\n\n" +
                           $"En iyi dileklerimle,\n\n[Stock Master]",
                    IsBodyHtml = false // HTML içeriği kullanacaksanız true yapın
                };
                mailMessage.To.Add(new MailAddress(recipientEmail));

                // SMTP istemcisi ayarları
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                })
                {
                    smtpClient.Send(mailMessage);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"E-posta gönderme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void SendForgotPasswordEmail(string recipientEmail, string userPassword)
        {
            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, "StockMaster"),
                    Subject = "Şifre Hatırlatma",
                    Body = $"Merhaba,\n\nUnuttuğunuz şifreniz: {userPassword}\n\nGüvenliğiniz için bu şifreyi kimseyle paylaşmayın.",
                    IsBodyHtml = false
                };
                mailMessage.To.Add(new MailAddress(recipientEmail));

                using (SmtpClient smtpClient = new SmtpClient(smtpClientAddress, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                })
                {
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"E-posta gönderme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        public void SendForgotUsernameEmail(string recipientEmail, string username)
        {
            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, "StockMaster"),
                    Subject = "Kullanıcı Adı Hatırlatma",
                    Body = $"Merhaba,\n\nKullanıcı adınız: {username}\n\nİyi günler dileriz!",
                    IsBodyHtml = false
                };
                mailMessage.To.Add(new MailAddress(recipientEmail));

                using (SmtpClient smtpClient = new SmtpClient(smtpClientAddress, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                })
                {
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"E-posta gönderme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void SendPasswordChangedEmail(string recipientEmail, string recipientName)
        {
            try
            {
                // Mail mesajı oluşturma
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = "Şifre Değişikliği Bilgilendirmesi",
                    Body = $"Merhaba {recipientName},\n\nŞifreniz başarıyla değiştirilmiştir. Eğer bu işlemi siz yapmadıysanız, lütfen hemen bizimle iletişime geçin.\n\nEn iyi dileklerimle,\n\n[Stock Master]",
                    IsBodyHtml = false // HTML içeriği kullanacaksanız true yapın
                };
                mailMessage.To.Add(new MailAddress(recipientEmail));

                // SMTP istemcisi ayarları
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                })
                {
                    // Mail gönderme
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"E-posta gönderme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SendOrderAddedEmail(string recipientEmail, string recipientName, string recipientPhone, decimal totalAmount, List<string> orderDetails)
        {
            try
            {
                // Sipariş içeriği metnini oluşturma
                StringBuilder orderDetailsBuilder = new StringBuilder();
                foreach (var item in orderDetails)
                {
                    orderDetailsBuilder.AppendLine($"- {item}");
                }

                // Mail mesajı oluşturma
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = "Sipariş Eklendi!",
                    Body = $"Merhaba,\n\n" +
                           $"Siparişiniz başarıyla alınmıştır. İşte siparişinizin detayları:\n\n" +
                           $"Adı: {recipientName}\n" +
                           $"Telefon: {recipientPhone}\n" +
                           $"Ödenecek Tutar: {totalAmount:C2}\n\n" +
                           $"Sipariş İçeriği:\n{orderDetailsBuilder.ToString()}\n\n" +
                           "En iyi dileklerimle,\n\n[Stock Master]",
                    IsBodyHtml = false // HTML içeriği kullanmak isterseniz true yapın
                };

                mailMessage.To.Add(new MailAddress(recipientEmail));

                // SMTP istemcisi ayarları
                using (SmtpClient smtpClient = new SmtpClient(smtpClientAddress, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                })
                {
                    // Mail gönderme
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"E-posta gönderme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
