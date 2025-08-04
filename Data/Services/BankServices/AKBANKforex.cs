using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class AKBANKforex
    {
        private readonly HttpClient _httpClient;

        public AKBANKforex()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<(decimal usdBuy, decimal usdSell)> GetExchangeRatesAsync()
        {
            try
            {
                // Akbank döviz sayfasını getir
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/akbank");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                // USD alış değerini çek
                var usdBuyStr = ExtractValue(htmlContent, "<span cid=\"822\" dt=\"bA\"", ">", "</span>");
                var usdBuy = ParseDecimal(usdBuyStr);

                // USD satış değerini çek
                var usdSellStr = ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"822\" dt=\"amount\"", ">", "</span>");
                var usdSell = ParseDecimal(usdSellStr);

                return (usdBuy, usdSell);
            }
            catch (Exception ex)
            {
                throw new Exception("Akbank döviz kurları alınırken hata: " + ex.Message);
            }
        }

        private string ExtractValue(string html, string startMarker, string prefix, string suffix)
        {
            int startIndex = html.IndexOf(startMarker);
            if (startIndex == -1) return null;

            int prefixIndex = html.IndexOf(prefix, startIndex);
            if (prefixIndex == -1) return null;

            int valueStart = prefixIndex + prefix.Length;
            int suffixIndex = html.IndexOf(suffix, valueStart);
            if (suffixIndex == -1) return null;

            return html.Substring(valueStart, suffixIndex - valueStart).Trim();
        }

        private decimal ParseDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Değer boş olamaz");

            value = value.Replace(".", ",");

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("tr-TR"), out decimal result))
            {
                return result;
            }

            throw new FormatException($"'{value}' değeri decimal olarak çevrilemedi");
        }
    }
}
