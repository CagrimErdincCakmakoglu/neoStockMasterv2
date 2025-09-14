using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Table; // EPPlus tablolar için
using System.IO;

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

        public void SendOrderWithExcelAttachment(
    string recipientEmail,
    string recipientName,
    decimal totalAmount,
    DataGridView dgwOrderDetails,
    ListView lwDisc,
    ListView lwTotal,
    ListView lwDiscList,
    ListView lwTax,
    ComboBox cmbDisc,
    NumericUpDown nmrDisc,
    ComboBox cmbSCT,
    ComboBox cmbVAT,
    NumericUpDown nmrCargo,
    TextBox txtCustomerName,
    MaskedTextBox mskPhoneNo,
    DateTime createdDate,
    string userLanguage,
    string userPassword
)
        {
            try
            {
                bool isTurkish = userLanguage?.Equals("Türkçe", StringComparison.OrdinalIgnoreCase) ?? true;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    // === Sipariş İçeriği ===
                    var wsOrderDetails = package.Workbook.Worksheets.Add(isTurkish ? "Sipariş İçeriği" : "Order Content");
                    ExportDataGridViewToWorksheet(dgwOrderDetails, wsOrderDetails);

                    // === İndirim Tablosu ===
                    var wsDisc = package.Workbook.Worksheets.Add(isTurkish ? "İndirim Tablosu" : "Discount Table");
                    ExportListViewToWorksheet(lwDisc, wsDisc);

                    // === Toplam Tutar ===
                    var wsTotal = package.Workbook.Worksheets.Add(isTurkish ? "Toplam Tutar" : "Total Amount");
                    ExportListViewToWorksheet(lwTotal, wsTotal);

                    // === Ek İndirim ===
                    var wsDiscList = package.Workbook.Worksheets.Add(isTurkish ? "Ek İndirim" : "Extra Discount");
                    ExportListViewToWorksheet(lwDiscList, wsDiscList);

                    // === Vergiler ===
                    var wsTax = package.Workbook.Worksheets.Add(isTurkish ? "Vergiler" : "Taxes");
                    ExportListViewToWorksheet(lwTax, wsTax);

                    // === Bilgiler ===
                    var wsInfo = package.Workbook.Worksheets.Add(isTurkish ? "Bilgiler" : "Info");
                    wsInfo.Cells[1, 1].Value = isTurkish ? "Müşteri Adı" : "Customer Name";
                    wsInfo.Cells[1, 2].Value = txtCustomerName.Text;

                    wsInfo.Cells[2, 1].Value = isTurkish ? "Telefon" : "Phone";
                    wsInfo.Cells[2, 2].Value = mskPhoneNo.Text;

                    // İndirim satırı
                    string discountText = cmbDisc.Text;
                    if (nmrDisc.Value > 0)
                        discountText += $" +{nmrDisc.Value}";
                    wsInfo.Cells[3, 1].Value = isTurkish ? "İndirim" : "Discount";
                    wsInfo.Cells[3, 2].Value = discountText;

                    wsInfo.Cells[4, 1].Value = isTurkish ? "ÖTV" : "SCT";
                    wsInfo.Cells[4, 2].Value = cmbSCT.Text;

                    wsInfo.Cells[5, 1].Value = isTurkish ? "KDV" : "VAT";
                    wsInfo.Cells[5, 2].Value = cmbVAT.Text;

                    wsInfo.Cells[6, 1].Value = isTurkish ? "Kargo" : "Cargo";
                    wsInfo.Cells[6, 2].Value = nmrCargo.Value;

                    wsInfo.Cells[7, 1].Value = isTurkish ? "Oluşturulma Tarihi" : "Created Date";
                    wsInfo.Cells[7, 2].Value = createdDate.ToString("dd.MM.yyyy HH:mm");

                    // === Otomatik kolon genişliği + hücre koruma ===
                    foreach (var ws in package.Workbook.Worksheets)
                    {
                        if (ws.Dimension != null)
                        {
                            // Kolon genişliği
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                            for (int col = ws.Dimension.Start.Column; col <= ws.Dimension.End.Column; col++)
                                ws.Column(col).BestFit = true;

                            // Tüm hücreleri kilitle
                            ws.Cells[ws.Dimension.Address].Style.Locked = true;

                            // Sayfa koruması
                            ws.Protection.IsProtected = true;
                            ws.Protection.SetPassword(userPassword);

                            // Kullanıcıya sadece okuma ve seçim izni ver
                            ws.Protection.AllowSelectLockedCells = true;
                            ws.Protection.AllowSelectUnlockedCells = true;
                            ws.Protection.AllowEditObject = false;
                            ws.Protection.AllowEditScenarios = false;
                            ws.Protection.AllowFormatCells = false;
                            ws.Protection.AllowFormatColumns = false;
                            ws.Protection.AllowFormatRows = false;
                            ws.Protection.AllowInsertColumns = false;
                            ws.Protection.AllowInsertRows = false;
                            ws.Protection.AllowDeleteColumns = false;
                            ws.Protection.AllowDeleteRows = false;
                        }
                    }

                    // === Workbook koruması (sayfa ekleme/silme engeli) ===
                    package.Workbook.Protection.SetPassword(userPassword);
                    package.Workbook.Protection.LockStructure = true;

                    // Belleğe kaydet
                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);
                        stream.Position = 0;

                        // === Mail başlığı ===
                        string subject = isTurkish
                            ? $"{txtCustomerName.Text} Siparişi Alındı!"
                            : $"{txtCustomerName.Text} Order Received!";

                        // === Mail gövdesi (lwTotal’den dinamik) ===
                        List<string> amounts = new List<string>();
                        foreach (ListViewItem item in lwTotal.Items)
                        {
                            if (item.SubItems.Count >= 2)
                            {
                                string currency = item.SubItems[0].Text;
                                string value = item.SubItems[1].Text;

                                if (!string.IsNullOrWhiteSpace(currency) && !string.IsNullOrWhiteSpace(value))
                                {
                                    amounts.Add($"{value} {currency}");
                                }
                            }
                        }

                        string joinedAmounts = "";
                        if (amounts.Count == 1)
                        {
                            joinedAmounts = amounts[0];
                        }
                        else if (amounts.Count == 2)
                        {
                            joinedAmounts = $"{amounts[0]} ve {amounts[1]}";
                        }
                        else if (amounts.Count > 2)
                        {
                            joinedAmounts = string.Join(", ", amounts.Take(amounts.Count - 1))
                                            + " ve " + amounts.Last();
                        }

                        string body = isTurkish
                            ? $"Merhaba {recipientName},\n\n{joinedAmounts} tutarındaki siparişiniz sisteme kayıt edilmiştir."
                            : $"Hello {recipientName},\n\nYour order of {joinedAmounts} has been recorded in the system.";

                        // === Mail oluşturma ===
                        MailMessage mailMessage = new MailMessage
                        {
                            From = new MailAddress(senderEmail, "StockMaster"),
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = false
                        };
                        mailMessage.To.Add(new MailAddress(recipientEmail));

                        // Excel dosya adı
                        string excelName = isTurkish
                            ? $"{txtCustomerName.Text} Sipariş Detayları.xlsx"
                            : $"{txtCustomerName.Text} Order Details.xlsx";

                        mailMessage.Attachments.Add(new Attachment(stream, excelName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));

                        using (SmtpClient smtpClient = new SmtpClient(smtpClientAddress, smtpPort)
                        {
                            Credentials = new NetworkCredential(senderEmail, senderPassword),
                            EnableSsl = true
                        })
                        {
                            smtpClient.Send(mailMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"E-posta gönderme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // --- Yardımcılar ---
        private void ExportDataGridViewToWorksheet(DataGridView dgv, ExcelWorksheet ws)
        {
            int col = 1;
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                ws.Cells[1, col].Value = column.HeaderText;
                col++;
            }

            int row = 2;
            foreach (DataGridViewRow dgRow in dgv.Rows)
            {
                if (dgRow.IsNewRow) continue;
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    ws.Cells[row, i + 1].Value = dgRow.Cells[i].Value?.ToString();
                }
                row++;
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();
        }

        private void ExportListViewToWorksheet(ListView lv, ExcelWorksheet ws)
        {
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                ws.Cells[1, i + 1].Value = lv.Columns[i].Text;
            }

            for (int i = 0; i < lv.Items.Count; i++)
            {
                var item = lv.Items[i];
                for (int j = 0; j < item.SubItems.Count; j++)
                {
                    ws.Cells[i + 2, j + 1].Value = item.SubItems[j].Text;
                }
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();
        }
    }
}
