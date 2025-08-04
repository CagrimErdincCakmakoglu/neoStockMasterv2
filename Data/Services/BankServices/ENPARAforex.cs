using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class ENPARAforex
    {
        private readonly HttpClient _httpClient;

        public ENPARAforex()
        {
            _httpClient = new HttpClient(); // Web istekleri için HTTP istemcisi
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<(decimal usdBuy, decimal usdSell, decimal euroBuy, decimal euroSell)> GetExchangeRatesAsync()
        {
            try
            {
                // Enpara döviz sayfasını getir
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/enpara");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                // USD alış değerini çek
                var usdBuyStr = ExtractValue(htmlContent, "<span cid=\"1021\" dt=\"bA\"", ">", "</span>");
                var usdBuy = ParseDecimal(usdBuyStr);

                // USD satış değerini çek
                var usdSellStr = ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1021\" dt=\"amount\"", ">", "</span>");
                var usdSell = ParseDecimal(usdSellStr);

                // Euro alış değerini çek
                var euroBuyStr = ExtractValue(htmlContent, "<span cid=\"1031\" dt=\"bA\"", ">", "</span>");
                var euroBuy = ParseDecimal(euroBuyStr);

                // Euro satış değerini çek
                var euroSellStr = ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1031\" dt=\"amount\"", ">", "</span>");
                var euroSell = ParseDecimal(euroSellStr);

                return (usdBuy, usdSell, euroBuy, euroSell);
            }
            catch (Exception ex)
            {
                throw new Exception("Enpara döviz kurları alınırken hata: " + ex.Message);
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

            // Noktayı virgüle çevir (Türkçe format)
            value = value.Replace(".", ",");

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("tr-TR"), out decimal result))
            {
                return result;
            }

            throw new FormatException($"'{value}' değeri decimal olarak çevrilemedi");
        }
    }
}
