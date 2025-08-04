using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class GARANTIforex
    {
        private readonly HttpClient _httpClient;

        public GARANTIforex()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<Dictionary<string, (decimal BuyRate, decimal SellRate)>> GetExchangeRatesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/garanti-bankasi");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                var rates = new Dictionary<string, (decimal, decimal)>();

                // USD
                rates["Dolar"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"805\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"805\" dt=\"amount\"", ">", "</span>"))
                );

                // EUR
                rates["Euro"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"807\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"807\" dt=\"amount\"", ">", "</span>"))
                );

                // GBP
                rates["İngiliz Sterlini"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"808\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"808\" dt=\"amount\"", ">", "</span>"))
                );

                // CHF
                rates["İsviçre Frangı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"809\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"809\" dt=\"amount\"", ">", "</span>"))
                );

                // CAD
                rates["Kanada Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"811\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"811\" dt=\"amount\"", ">", "</span>"))
                );

                // RUB
                rates["Rus Rublesi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"816\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"816\" dt=\"amount\"", ">", "</span>"))
                );

                // AUD
                rates["Avustralya Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"810\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"810\" dt=\"amount\"", ">", "</span>"))
                );

                // DKK
                rates["Danimarka Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"813\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"813\" dt=\"amount\"", ">", "</span>"))
                );

                // SEK
                rates["İsveç Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"818\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"818\" dt=\"amount\"", ">", "</span>"))
                );

                // NOK
                rates["Norveç Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"815\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"815\" dt=\"amount\"", ">", "</span>"))
                );

                // JPY (100)
                rates["100 Japon Yeni"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"814\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"814\" dt=\"amount\"", ">", "</span>"))
                );

                // CNY
                rates["Çin Yuanı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"812\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"812\" dt=\"amount\"", ">", "</span>"))
                );

                // SAR
                rates["Suudi Arabistan Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"817\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"817\" dt=\"amount\"", ">", "</span>"))
                );

                return rates;
            }
            catch (Exception ex)
            {
                throw new Exception("Garanti Bankası döviz kurları alınırken hata: " + ex.Message);
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
