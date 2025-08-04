using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class ZIRAATforex
    {
        private readonly HttpClient _httpClient;

        public ZIRAATforex()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<Dictionary<string, (decimal BuyRate, decimal SellRate)>> GetExchangeRatesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/ziraat-bankasi");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                var rates = new Dictionary<string, (decimal, decimal)>();

                // USD
                rates["Dolar"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"264\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"264\" dt=\"amount\"", ">", "</span>"))
                );

                // EUR
                rates["Euro"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"894\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"894\" dt=\"amount\"", ">", "</span>"))
                );

                // GBP
                rates["İngiliz Sterlini"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"896\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"896\" dt=\"amount\"", ">", "</span>"))
                );

                // CHF
                rates["İsviçre Frangı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"902\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"902\" dt=\"amount\"", ">", "</span>"))
                );

                // CAD
                rates["Kanada Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"899\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"899\" dt=\"amount\"", ">", "</span>"))
                );

                // RUB
                rates["Rus Rublesi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"901\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"901\" dt=\"amount\"", ">", "</span>"))
                );

                // AUD
                rates["Avustralya Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"897\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"897\" dt=\"amount\"", ">", "</span>"))
                );

                // DKK
                rates["Danimarka Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"895\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"895\" dt=\"amount\"", ">", "</span>"))
                );

                // SEK
                rates["İsveç Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"900\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"900\" dt=\"amount\"", ">", "</span>"))
                );

                // NOK
                rates["Norveç Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"898\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"898\" dt=\"amount\"", ">", "</span>"))
                );

                // JPY (100)
                rates["100 Japon Yeni"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1286\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1286\" dt=\"amount\"", ">", "</span>"))
                );

                // KWD
                rates["Kuveyt Dinarı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1284\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1284\" dt=\"amount\"", ">", "</span>"))
                );

                // CNY
                rates["Çin Yuanı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1285\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1285\" dt=\"amount\"", ">", "</span>"))
                );

                // QAR
                rates["Katar Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1287\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1287\" dt=\"amount\"", ">", "</span>"))
                );

                // SAR
                rates["Suudi Arabistan Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"903\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"903\" dt=\"amount\"", ">", "</span>"))
                );

                return rates;
            }
            catch (Exception ex)
            {
                throw new Exception("Ziraat Bankası döviz kurları alınırken hata: " + ex.Message);
            }
        }

        private string DegerCikar(string html, string baslangicIsareti, string onEk, string sonEk)
        {
            int baslangicIndex = html.IndexOf(baslangicIsareti);
            if (baslangicIndex == -1) return null;

            int onEkIndex = html.IndexOf(onEk, baslangicIndex);
            if (onEkIndex == -1) return null;

            int degerBaslangic = onEkIndex + onEk.Length;
            int sonEkIndex = html.IndexOf(sonEk, degerBaslangic);
            if (sonEkIndex == -1) return null;

            return html.Substring(degerBaslangic, sonEkIndex - degerBaslangic).Trim();
        }

        private decimal DecimalCevir(string deger)
        {
            if (string.IsNullOrEmpty(deger))
                throw new ArgumentException("Değer boş olamaz");

            deger = deger.Replace(".", ",");

            if (decimal.TryParse(deger, NumberStyles.Any, CultureInfo.GetCultureInfo("tr-TR"), out decimal sonuc))
            {
                return sonuc;
            }

            throw new FormatException($"'{deger}' değeri decimal olarak çevrilemedi");
        }
    }
}
