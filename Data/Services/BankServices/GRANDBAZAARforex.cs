using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class GRANDBAZAARforex
    {
        private readonly HttpClient _httpClient;

        public GRANDBAZAARforex()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<Dictionary<string, (decimal BuyRate, decimal SellRate)>> GetExchangeRatesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/kapali-carsi");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                var rates = new Dictionary<string, (decimal, decimal)>();

                

                // USD
                rates["Dolar"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1114\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1114\" dt=\"amount\"", ">", "</span>"))
                );

                // EUR
                rates["Euro"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1194\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1194\" dt=\"amount\"", ">", "</span>"))
                );

                // GBP
                rates["İngiliz Sterlini"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1195\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1195\" dt=\"amount\"", ">", "</span>"))
                );

                // CHF
                rates["İsviçre Frangı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1199\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1199\" dt=\"amount\"", ">", "</span>"))
                );

                // CAD
                rates["Kanada Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1204\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1204\" dt=\"amount\"", ">", "</span>"))
                );

                // RUB
                rates["Rus Rublesi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1206\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1206\" dt=\"amount\"", ">", "</span>"))
                );

                // AED
                rates["BAE Dirhemi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1208\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1208\" dt=\"amount\"", ">", "</span>"))
                );

                // AUD
                rates["Avustralya Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1203\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1203\" dt=\"amount\"", ">", "</span>"))
                );

                // DKK
                rates["Danimarka Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1200\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1200\" dt=\"amount\"", ">", "</span>"))
                );

                // SEK
                rates["İsveç Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1201\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1201\" dt=\"amount\"", ">", "</span>"))
                );

                // NOK
                rates["Norveç Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1202\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1202\" dt=\"amount\"", ">", "</span>"))
                );

                // // JPY (100)
                rates["100 Japon Yeni"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1198\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1198\" dt=\"amount\"", ">", "</span>"))
                );

                // BGN
                rates["Bulgar Levası"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1207\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1207\" dt=\"amount\"", ">", "</span>"))
                );

                // CNY
                rates["Çin Yuanı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1210\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1210\" dt=\"amount\"", ">", "</span>"))
                );

                // QAR
                rates["Katar Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1209\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1209\" dt=\"amount\"", ">", "</span>"))
                );

                // SAR
                rates["Suudi Arabistan Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1205\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1205\" dt=\"amount\"", ">", "</span>"))
                );


                return rates;
            }
            catch (Exception ex)
            {
                throw new Exception("Kapalı Çarşı döviz kurları alınırken hata: " + ex.Message);
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
