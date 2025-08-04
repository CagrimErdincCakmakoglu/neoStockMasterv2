using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class ALBARAKAforex
    {
        private readonly HttpClient _httpClient;

        public ALBARAKAforex()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<(decimal usdBuy, decimal usdSell,
                          decimal euroBuy, decimal euroSell,
                          decimal gbpBuy, decimal gbpSell,
                          decimal sarBuy, decimal sarSell)> GetExchangeRatesAsync()
        {
            try
            {
                // Albaraka döviz sayfasını getir
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/albaraka");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                // USD alış-satış
                var usdBuy = ParseDecimal(ExtractValue(htmlContent, "<span cid=\"1113\" dt=\"bA\"", ">", "</span>"));
                var usdSell = ParseDecimal(ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1113\" dt=\"amount\"", ">", "</span>"));

                // Euro alış-satış
                var euroBuy = ParseDecimal(ExtractValue(htmlContent, "<span cid=\"1328\" dt=\"bA\"", ">", "</span>"));
                var euroSell = ParseDecimal(ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1328\" dt=\"amount\"", ">", "</span>"));

                // GBP alış-satış
                var gbpBuy = ParseDecimal(ExtractValue(htmlContent, "<span cid=\"1329\" dt=\"bA\"", ">", "</span>"));
                var gbpSell = ParseDecimal(ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1329\" dt=\"amount\"", ">", "</span>"));

                // SAR alış-satış
                var sarBuy = ParseDecimal(ExtractValue(htmlContent, "<span cid=\"1330\" dt=\"bA\"", ">", "</span>"));
                var sarSell = ParseDecimal(ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1330\" dt=\"amount\"", ">", "</span>"));

                return (usdBuy, usdSell, euroBuy, euroSell, gbpBuy, gbpSell, sarBuy, sarSell);
            }
            catch (Exception ex)
            {
                throw new Exception("Albaraka döviz kurları alınırken hata: " + ex.Message);
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
