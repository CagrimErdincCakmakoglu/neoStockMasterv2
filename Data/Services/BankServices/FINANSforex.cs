using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class FINANSforex
    {
        private readonly HttpClient _httpClient;

        public FINANSforex()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<Dictionary<string, (decimal BuyRate, decimal SellRate)>> GetExchangeRatesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/finansbank");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                var rates = new Dictionary<string, (decimal, decimal)>();

                // USD
                rates["Dolar"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"788\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"788\" dt=\"amount\"", ">", "</span>"))
                );

                // EUR
                rates["Euro"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"790\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"790\" dt=\"amount\"", ">", "</span>"))
                );

                // GBP
                rates["İngiliz Sterlini"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"791\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"791\" dt=\"amount\"", ">", "</span>"))
                );

                // CHF
                rates["İsviçre Frangı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"795\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"795\" dt=\"amount\"", ">", "</span>"))
                );

                // CAD
                rates["Kanada Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"794\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"794\" dt=\"amount\"", ">", "</span>"))
                );

                // RUB
                rates["Rus Rublesi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"801\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"801\" dt=\"amount\"", ">", "</span>"))
                );

                // AED
                rates["BAE Dirhemi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"792\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"792\" dt=\"amount\"", ">", "</span>"))
                );

                // AUD
                rates["Avustralya Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"793\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"793\" dt=\"amount\"", ">", "</span>"))
                );

                // DKK
                rates["Danimarka Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"797\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"797\" dt=\"amount\"", ">", "</span>"))
                );

                // SEK
                rates["İsveç Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"803\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"803\" dt=\"amount\"", ">", "</span>"))
                );

                // NOK
                rates["Norveç Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"798\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"798\" dt=\"amount\"", ">", "</span>"))
                );

                // ZAR
                rates["Güney Afrika Randı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"804\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"804\" dt=\"amount\"", ">", "</span>"))
                );

                // CNY
                rates["Çin Yuanı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"796\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"796\" dt=\"amount\"", ">", "</span>"))
                );

                // PLN
                rates["Polonya Zlotisi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"799\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"799\" dt=\"amount\"", ">", "</span>"))
                );

                // QAR
                rates["Katar Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"800\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"800\" dt=\"amount\"", ">", "</span>"))
                );

                // SAR
                rates["Suudi Arabistan Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"802\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"802\" dt=\"amount\"", ">", "</span>"))
                );


                return rates;
            }
            catch (Exception ex)
            {
                throw new Exception("Finansbank döviz kurları alınırken hata: " + ex.Message);
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
