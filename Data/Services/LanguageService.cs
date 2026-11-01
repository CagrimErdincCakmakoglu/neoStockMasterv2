using System;
using System.Resources;

namespace neoStockMasterv2.Data.Services
{
    public class LanguageService
    {
        // Her GetString çağrısında yeniden oluşturmak yerine bir kez oluşturulur
        private static readonly ResourceManager _turkish =
            new ResourceManager("neoStockMasterv2.Languages.Turkish", typeof(LanguageService).Assembly);
        private static readonly ResourceManager _english =
            new ResourceManager("neoStockMasterv2.Languages.English", typeof(LanguageService).Assembly);

        public static string CurrentLanguage { get; private set; } = "Türkçe";
        public static event Action LanguageChanged;  // Dil değiştiğinde tetiklenecek event

        public static bool IsEnglish => CurrentLanguage != "Türkçe";

        public static void SetLanguage(string language)
        {
            CurrentLanguage = language;
            LanguageChanged?.Invoke();
        }

        public static string GetString(string key) =>
            (IsEnglish ? _english : _turkish).GetString(key);

        /// <summary>Kısa çeviri: T("Kaydedildi", "Saved")</summary>
        public static string T(string tr, string en) => IsEnglish ? en : tr;
    }
}