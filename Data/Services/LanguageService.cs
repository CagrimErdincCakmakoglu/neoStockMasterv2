using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace neoStockMasterv2.Data.Services
{
    public class LanguageService
    {
        public static string CurrentLanguage { get; private set; } = "Türkçe";
        public static event Action LanguageChanged;  // Dil değiştiğinde tetiklenecek event

        public static void SetLanguage(string language)
        {
            CurrentLanguage = language;
            LanguageChanged?.Invoke();  // Event tetikleniyor
        }

        public static string GetString(string key)
        {
            //Üye Girişi Yapılmadan Önce İşlemleri Buradan Yapıyor
            if (CurrentLanguage == "Türkçe")
            {
                return new ResourceManager("neoStockMasterv2.Languages.Turkish", typeof(LanguageService).Assembly).GetString(key);
            }
            else
            {
                return new ResourceManager("neoStockMasterv2.Languages.English", typeof(LanguageService).Assembly).GetString(key);
            }
        }
    }
}
