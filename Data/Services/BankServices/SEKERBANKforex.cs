using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class SEKERBANKforex
    {
        private readonly HttpClient _httpClient;

        public SEKERBANKforex()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<(decimal usdBuy, decimal usdSell,
                          decimal euroBuy, decimal euroSell,
                          decimal gbpBuy, decimal gbpSell)> GetExchangeRatesAsync()
        {
            try
            {
                // Şekerbank döviz sayfasını getir
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/sekerbank");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                // USD alış-satış (1022)
                var usdBuy = ParseDecimal(ExtractValue(htmlContent, "<span cid=\"1022\" dt=\"bA\"", ">", "</span>"));
                var usdSell = ParseDecimal(ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1022\" dt=\"amount\"", ">", "</span>"));

                // Euro alış-satış (1032)
                var euroBuy = ParseDecimal(ExtractValue(htmlContent, "<span cid=\"1032\" dt=\"bA\"", ">", "</span>"));
                var euroSell = ParseDecimal(ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1032\" dt=\"amount\"", ">", "</span>"));

                // GBP alış-satış (1289)
                var gbpBuy = ParseDecimal(ExtractValue(htmlContent, "<span cid=\"1289\" dt=\"bA\"", ">", "</span>"));
                var gbpSell = ParseDecimal(ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1289\" dt=\"amount\"", ">", "</span>"));

                return (usdBuy, usdSell, euroBuy, euroSell, gbpBuy, gbpSell);
            }
            catch (Exception ex)
            {
                throw new Exception("Şekerbank döviz kurları alınırken hata: " + ex.Message);
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
