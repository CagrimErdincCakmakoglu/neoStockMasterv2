using System.Net;
using System.Net.Mail;
using OfficeOpenXml;

namespace neoStockMasterv2.Data.Services
{
    public class EmailService
    {
        private readonly MailConfig _config = MailConfig.Current;

        // ── Ortak yardımcılar ──
        private MailMessage CreateMessage(string to, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_config.SenderEmail, "StockMaster"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            message.To.Add(new MailAddress(to));
            return message;
        }

        private async Task SendAsync(MailMessage message)
        {
            using var smtp = new SmtpClient(_config.SmtpHost, _config.SmtpPort)
            {
                Credentials = new NetworkCredential(_config.SenderEmail, _config.AppPassword),
                EnableSsl = true
            };
            await smtp.SendMailAsync(message);
        }

        // ── Kullanıcı mailleri ──
        public async Task SendWelcomeEmailAsync(string recipientEmail, string recipientName, string verificationCode)
        {
            string body = $"Merhaba {recipientName},\n\n" +
                          "Uygulamaya kayıt olduğunuz için teşekkür ederiz! Sizleri aramızda görmekten mutluluk duyuyoruz.\n\n" +
                          "Hesabınızı doğrulamak için aşağıdaki onay kodunu kullanabilirsiniz:\n\n" +
                          $"{verificationCode}\n\n" +
                          "Bu kodu kullanarak hesabınızı aktif hale getirebilirsiniz.\n\n" +
                          "En iyi dileklerimle,\n\n[Stock Master]";

            using var message = CreateMessage(recipientEmail, "Hoş Geldiniz!", body);
            await SendAsync(message);
        }

        public async Task SendTemporaryPasswordEmailAsync(string recipientEmail, string temporaryPassword)
        {
            string body = "Merhaba,\n\n" +
                          $"Şifre sıfırlama talebiniz alındı. Geçici şifreniz: {temporaryPassword}\n\n" +
                          "Giriş yaptıktan sonra Kullanıcı Bilgileri ekranından şifrenizi değiştirin.\n" +
                          "Bu talebi siz yapmadıysanız lütfen bizimle iletişime geçin.";

            using var message = CreateMessage(recipientEmail, "Şifre Sıfırlama", body);
            await SendAsync(message);
        }

        public async Task SendForgotUsernameEmailAsync(string recipientEmail, string username)
        {
            string body = $"Merhaba,\n\nKullanıcı adınız: {username}\n\nİyi günler dileriz!";

            using var message = CreateMessage(recipientEmail, "Kullanıcı Adı Hatırlatma", body);
            await SendAsync(message);
        }

        public async Task SendPasswordChangedEmailAsync(string recipientEmail, string recipientName)
        {
            string body = $"Merhaba {recipientName},\n\n" +
                          "Şifreniz başarıyla değiştirilmiştir. Eğer bu işlemi siz yapmadıysanız, lütfen hemen bizimle iletişime geçin.\n\n" +
                          "En iyi dileklerimle,\n\n[Stock Master]";

            using var message = CreateMessage(recipientEmail, "Şifre Değişikliği Bilgilendirmesi", body);
            await SendAsync(message);
        }

        // ── Sipariş maili (Excel ekli) ──
        public async Task SendOrderWithExcelAttachmentAsync(
            string recipientEmail,
            string recipientName,
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
            string userLanguage)
        {
            bool isTurkish = userLanguage?.Equals("Türkçe", StringComparison.OrdinalIgnoreCase) ?? true;
            string customerName = txtCustomerName.Text;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();

            ExportDataGridViewToWorksheet(dgwOrderDetails, package.Workbook.Worksheets.Add(isTurkish ? "Sipariş İçeriği" : "Order Content"));
            ExportListViewToWorksheet(lwDisc, package.Workbook.Worksheets.Add(isTurkish ? "İndirim Tablosu" : "Discount Table"));
            ExportListViewToWorksheet(lwTotal, package.Workbook.Worksheets.Add(isTurkish ? "Toplam Tutar" : "Total Amount"));
            ExportListViewToWorksheet(lwDiscList, package.Workbook.Worksheets.Add(isTurkish ? "Ek İndirim" : "Extra Discount"));
            ExportListViewToWorksheet(lwTax, package.Workbook.Worksheets.Add(isTurkish ? "Vergiler" : "Taxes"));

            var wsInfo = package.Workbook.Worksheets.Add(isTurkish ? "Bilgiler" : "Info");
            string discountText = cmbDisc.Text + (nmrDisc.Value > 0 ? $" +{nmrDisc.Value}" : "");
            object[,] info =
            {
                { isTurkish ? "Müşteri Adı" : "Customer Name", customerName },
                { isTurkish ? "Telefon" : "Phone", mskPhoneNo.Text },
                { isTurkish ? "İndirim" : "Discount", discountText },
                { isTurkish ? "ÖTV" : "SCT", cmbSCT.Text },
                { isTurkish ? "KDV" : "VAT", cmbVAT.Text },
                { isTurkish ? "Kargo" : "Cargo", nmrCargo.Value },
                { isTurkish ? "Oluşturulma Tarihi" : "Created Date", createdDate.ToString("dd.MM.yyyy HH:mm") }
            };
            for (int r = 0; r < info.GetLength(0); r++)
            {
                wsInfo.Cells[r + 1, 1].Value = info[r, 0];
                wsInfo.Cells[r + 1, 2].Value = info[r, 1];
            }

            // Sayfaları rastgele bir parolayla kilitle (kullanıcının giriş şifresi ASLA kullanılmaz)
            string protectionPassword = Guid.NewGuid().ToString("N");
            foreach (var ws in package.Workbook.Worksheets)
            {
                if (ws.Dimension == null) continue;
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells[ws.Dimension.Address].Style.Locked = true;
                ws.Protection.IsProtected = true;
                ws.Protection.SetPassword(protectionPassword);
                ws.Protection.AllowSelectLockedCells = true;
                ws.Protection.AllowSelectUnlockedCells = true;
            }
            package.Workbook.Protection.SetPassword(protectionPassword);
            package.Workbook.Protection.LockStructure = true;

            // Mail gövdesi: lwTotal'daki tutarlar
            var amounts = lwTotal.Items.Cast<ListViewItem>()
                .Where(i => i.SubItems.Count >= 2 && !string.IsNullOrWhiteSpace(i.SubItems[0].Text) && !string.IsNullOrWhiteSpace(i.SubItems[1].Text))
                .Select(i => $"{i.SubItems[1].Text} {i.SubItems[0].Text}")
                .ToList();

            string joiner = isTurkish ? " ve " : " and ";
            string joinedAmounts = amounts.Count <= 1
                ? string.Join("", amounts)
                : string.Join(", ", amounts.Take(amounts.Count - 1)) + joiner + amounts.Last();

            string subject = isTurkish ? $"{customerName} Siparişi Alındı!" : $"{customerName} Order Received!";
            string body = isTurkish
                ? $"Merhaba {recipientName},\n\n{joinedAmounts} tutarındaki siparişiniz sisteme kayıt edilmiştir."
                : $"Hello {recipientName},\n\nYour order of {joinedAmounts} has been recorded in the system.";
            string excelName = isTurkish ? $"{customerName} Sipariş Detayları.xlsx" : $"{customerName} Order Details.xlsx";

            using var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            using var message = CreateMessage(recipientEmail, subject, body);
            message.Attachments.Add(new Attachment(stream, excelName,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
            await SendAsync(message);
        }

        private static void ExportDataGridViewToWorksheet(DataGridView dgv, ExcelWorksheet ws)
        {
            for (int c = 0; c < dgv.Columns.Count; c++)
                ws.Cells[1, c + 1].Value = dgv.Columns[c].HeaderText;

            int row = 2;
            foreach (DataGridViewRow dgRow in dgv.Rows)
            {
                if (dgRow.IsNewRow) continue;
                for (int c = 0; c < dgv.Columns.Count; c++)
                    ws.Cells[row, c + 1].Value = dgRow.Cells[c].Value?.ToString();
                row++;
            }
        }

        private static void ExportListViewToWorksheet(ListView lv, ExcelWorksheet ws)
        {
            for (int c = 0; c < lv.Columns.Count; c++)
                ws.Cells[1, c + 1].Value = lv.Columns[c].Text;

            for (int i = 0; i < lv.Items.Count; i++)
                for (int j = 0; j < lv.Items[i].SubItems.Count; j++)
                    ws.Cells[i + 2, j + 1].Value = lv.Items[i].SubItems[j].Text;
        }

        public async Task SendExcelPasswordEmailAsync(string recipientEmail, string recipientName, string fileName, string password)
        {
            string body = $"Merhaba {recipientName},\n\n" +
                          "StockMaster'dan bir Excel dosyası dışa aktarıldı.\n\n" +
                          $"Dosya adı: {fileName}\n" +
                          $"Tarih: {DateTime.Now:dd.MM.yyyy HH:mm}\n" +
                          $"Kilit şifresi: {password}\n\n" +
                          "Kilidi açmak için Excel'de: Gözden Geçir → Sayfa Korumasını Kaldır → bu şifreyi girin.\n\n" +
                          "Bu işlemi siz yapmadıysanız lütfen kontrol edin.\n\n[Stock Master]";

            using var message = CreateMessage(recipientEmail, $"Excel Şifresi: {fileName}", body);
            await SendAsync(message);
        }
    }
}