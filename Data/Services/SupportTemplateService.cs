using neoStockMasterv2.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neoStockMasterv2.Data.Services
{
    public class SupportTemplateService
    {
        public enum SupportType
        {
            TechnicalSupport,
            BugReport,
            Billing,            // Abonelik ve Ödeme
            PriceQuote,         // Fiyat Teklifi
            AccountChange,      // Hesap Bilgi Değişimi
            Complaint,          // Şikayet
            Suggestion,         // Öneri
            GeneralContact,     // Genel İletişim
            ThankYou            // Teşekkür
        }

        public static (string Subject, string Body) GetTemplate(SupportType type, User user, string userMessage = "")
        {
            string currentLang = LanguageService.CurrentLanguage;
            string subject = "";
            string body = "";

            // Sistem Bilgilerini Otomatik Topla
            string systemInfo = $"-------------------------------------\n" +
                                $"[System Info]\n" +
                                $"App: neoStockMaster v2.0\n" +
                                $"User ID: {user.ID} - {user.Name}\n" +
                                $"OS: {Environment.OSVersion}\n" +
                                $"Date: {DateTime.Now:yyyy-MM-dd HH:mm}\n" +
                                $"-------------------------------------\n\n";

            switch (type)
            {
                case SupportType.TechnicalSupport:
                    if (currentLang == "English")
                    {
                        subject = $"[Technical Support] - {user.Name}";
                        body = $"Hello Support Team,\n\nI am experiencing a technical issue with the software.\n\n[User Description]:\n{userMessage}\n\n" + systemInfo;
                    }
                    else
                    {
                        subject = $"[Teknik Destek] - {user.Name}";
                        body = $"Merhaba Destek Ekibi,\n\nProgramın kullanımı sırasında teknik bir sorunla karşılaştım.\n\n[Kullanıcı Açıklaması]:\n{userMessage}\n\n" + systemInfo;
                    }
                    break;

                case SupportType.BugReport:
                    if (currentLang == "English")
                    {
                        subject = $"[Bug Report] - {user.Name}";
                        body = $"Hello,\n\nI want to report a bug in the application.\n\nDetails:\n{userMessage}\n\n" + systemInfo;
                    }
                    else
                    {
                        subject = $"[Hata Bildirimi] - {user.Name}";
                        body = $"Merhaba,\n\nUygulamada bir hata (bug) tespit ettim.\n\nDetaylar:\n{userMessage}\n\n" + systemInfo;
                    }
                    break;

                case SupportType.Billing:
                    if (currentLang == "English")
                    {
                        subject = $"[Subscription & Payment] - {user.Name}";
                        body = $"Hello,\n\nI have a question or issue regarding my subscription/payment.\n\nDetails:\n{userMessage}\n\n" + systemInfo;
                    }
                    else
                    {
                        subject = $"[Abonelik ve Ödeme] - {user.Name}";
                        body = $"Merhaba,\n\nAboneliğim veya ödeme işlemlerim ile ilgili bir durum belirtmek istiyorum.\n\nDetaylar:\n{userMessage}\n\n" + systemInfo;
                    }
                    break;

                case SupportType.PriceQuote:
                    if (currentLang == "English")
                    {
                        subject = $"[Price Quote Request] - {user.Name}";
                        body = $"Hello,\n\nI would like to request a special price quote / plan upgrade.\n\nDetails:\n{userMessage}\n\n" + systemInfo;
                    }
                    else
                    {
                        subject = $"[Fiyat Teklifi Talebi] - {user.Name}";
                        body = $"Merhaba,\n\nKurumsal paket, lisans yenileme veya özel fiyat teklifi almak istiyorum.\n\nDetaylar:\n{userMessage}\n\n" + systemInfo;
                    }
                    break;

                case SupportType.AccountChange:
                    if (currentLang == "English")
                    {
                        subject = $"[Account Information Change] - {user.Name}";
                        body = $"Hello,\n\nI want to update my registration or company account details.\n\nDetails:\n{userMessage}\n\n" + systemInfo;
                    }
                    else
                    {
                        subject = $"[Hesap Bilgi Değişimi] - {user.Name}";
                        body = $"Merhaba,\n\nKayıtlı kullanıcı veya firma bilgilerimin güncellenmesini talep ediyorum.\n\nDetaylar:\n{userMessage}\n\n" + systemInfo;
                    }
                    break;

                case SupportType.Complaint:
                    if (currentLang == "English")
                    {
                        subject = $"[Complaint / Feedback] - {user.Name}";
                        body = $"Hello,\n\nI would like to file a complaint regarding my experience.\n\nDetails:\n{userMessage}\n\n" + systemInfo;
                    }
                    else
                    {
                        subject = $"[Şikayet / Geri Bildirim] - {user.Name}";
                        body = $"Merhaba,\n\nHizmet veya uygulama süreçleriyle ilgili bir şikayetimi/memnuniyetsizliğimi iletmek istiyorum.\n\nDetaylar:\n{userMessage}\n\n" + systemInfo;
                    }
                    break;

                case SupportType.Suggestion:
                    if (currentLang == "English")
                    {
                        subject = $"[Feature Suggestion] - {user.Name}";
                        body = $"Hello,\n\nI have a feature request or a suggestion to improve the application.\n\nDetails:\n{userMessage}\n\n" + systemInfo;
                    }
                    else
                    {
                        subject = $"[Öneri / Özellik Talebi] - {user.Name}";
                        body = $"Merhaba,\n\nUygulamayı geliştirmek adına bir öneri veya yeni özellik talebinde bulunmak istiyorum.\n\nDetaylar:\n{userMessage}\n\n" + systemInfo;
                    }
                    break;

                case SupportType.GeneralContact:
                    if (currentLang == "English")
                    {
                        subject = $"[General Contact] - {user.Name}";
                        body = $"Hello,\n\nI would like to get in touch with you.\n\nDetails:\n{userMessage}\n\n" + systemInfo;
                    }
                    else
                    {
                        subject = $"[Genel İletişim] - {user.Name}";
                        body = $"Merhaba,\n\nSizinle iletişime geçmek istiyorum.\n\nDetaylar:\n{userMessage}\n\n" + systemInfo;
                    }
                    break;

                case SupportType.ThankYou:
                    if (currentLang == "English")
                    {
                        subject = $"[Thank You] - {user.Name}";
                        body = $"Hello,\n\nI would like to share my appreciation.\n\nMessage:\n{userMessage}\n\n" + systemInfo;
                    }
                    else
                    {
                        subject = $"[Teşekkür] - {user.Name}";
                        body = $"Merhaba,\n\nMemnuniyetimi ve teşekkürümü iletmek istiyorum.\n\nMesaj:\n{userMessage}\n\n" + systemInfo;
                    }
                    break;
            }

            return (subject, body);
        }



    }
}