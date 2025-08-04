using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class DENIZBANKforex
    {
        private readonly HttpClient _httpClient;

        public DENIZBANKforex()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<(decimal usdBuy, decimal usdSell, decimal euroBuy, decimal euroSell)> GetExchangeRatesAsync()
        {
            try
            {
                // DenizBank döviz sayfasını getir
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/denizbank");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                // USD alış değerini çek (1019)
                var usdBuyStr = ExtractValue(htmlContent, "<span cid=\"1019\" dt=\"bA\"", ">", "</span>");
                var usdBuy = ParseDecimal(usdBuyStr);

                // USD satış değerini çek (1019)
                var usdSellStr = ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1019\" dt=\"amount\"", ">", "</span>");
                var usdSell = ParseDecimal(usdSellStr);

                // Euro alış değerini çek (1029)
                var euroBuyStr = ExtractValue(htmlContent, "<span cid=\"1029\" dt=\"bA\"", ">", "</span>");
                var euroBuy = ParseDecimal(euroBuyStr);

                // Euro satış değerini çek (1029)
                var euroSellStr = ExtractValue(htmlContent, "<span itemprop=\"price\" cid=\"1029\" dt=\"amount\"", ">", "</span>");
                var euroSell = ParseDecimal(euroSellStr);

                return (usdBuy, usdSell, euroBuy, euroSell);
            }
            catch (Exception ex)
            {
                throw new Exception("DenizBank döviz kurları alınırken hata: " + ex.Message);
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
