using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Net.Http;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class HSBCforex
    {
        private readonly HttpClient _httpClient;

        public HSBCforex()
        {
            _httpClient = new HttpClient(); // Web istekleri için HTTP istemcisi
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        // Döviz kurlarını getiren asenkron metot
        public async Task<(decimal usdBuying, decimal usdSelling, decimal euroBuying, decimal euroSelling)> GetExchangeRatesAsync()
        {
            try
            {
                // HSBC döviz sayfasını getir
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/hsbc");
                response.EnsureSuccessStatusCode(); // Başarısız istekte hata fırlat
                var htmlContent = await response.Content.ReadAsStringAsync();

                // USD alış değerini çıkar (1. değer)
                var usdBuyingStr = ExtractValue(htmlContent, "<span cid=\"1025\" dt=\"bA\"", ">", "</span>");
                var usdBuying = ParseDecimal(usdBuyingStr);

                // USD satış değerini çıkar (1. değer)
                var usdSellingStr = ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1025\" dt=\"amount\"", ">", "</span>");
                var usdSelling = ParseDecimal(usdSellingStr);

                // Euro alış değerini çıkar (2. değer)
                var euroBuyingStr = ExtractValue(htmlContent, "<span cid=\"1035\" dt=\"bA\"", ">", "</span>");
                var euroBuying = ParseDecimal(euroBuyingStr);

                // Euro satış değerini çıkar (2. değer)
                var euroSellingStr = ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1035\" dt=\"amount\"", ">", "</span>");
                var euroSelling = ParseDecimal(euroSellingStr);

                return (usdBuying, usdSelling, euroBuying, euroSelling);
            }
            catch (Exception ex)
            {
                // Hataları yönet veya logla
                throw new Exception("HSBC döviz kurları alınırken hata: " + ex.Message);
            }
        }

        // HTML içinden değer çıkaran yardımcı metot
        private string ExtractValue(string html, string startMarker, string prefix, string suffix)
        {
            int startIndex = html.IndexOf(startMarker);
            if (startIndex == -1) return null;

            int prefixIndex = html.IndexOf(prefix, startIndex);
            if (prefixIndex == -1) return null;

            int valueStart = prefixIndex + prefix.Length;
            int suffixIndex = html.IndexOf(suffix, valueStart);
            if (suffixIndex == -1) return null;

            return html.Substring(valueStart, suffixIndex - valueStart);
        }

        // String'i decimal'e çeviren yardımcı metot
        private decimal ParseDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Değer boş olamaz");

            // Web sitesindeki noktayı virgüle çevir (Türk formatı)
            value = value.Replace(".", ",");

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("tr-TR"), out decimal result))
            {
                return result;
            }

            throw new FormatException($"'{value}' değeri decimal olarak parse edilemedi");
        }
    }
}


