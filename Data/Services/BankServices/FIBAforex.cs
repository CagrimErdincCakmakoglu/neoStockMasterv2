using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class FIBAforex
    {
        private readonly HttpClient _httpClient;

        public FIBAforex()
        {
            _httpClient = new HttpClient(); // Web istekleri için HTTP istemcisi
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        // Fibabanka'dan döviz kurlarını çeken asenkron metot
        public async Task<(decimal usdAlis, decimal usdSatis)> GetExchangeRatesAsync()
        {
            try
            {
                // Fibabanka döviz sayfasını getir
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari/fibabanka");
                response.EnsureSuccessStatusCode(); // Hata durumunda exception fırlat
                var htmlContent = await response.Content.ReadAsStringAsync();

                // USD alış değerini çek (HTML'den parse et)
                var usdAlisStr = DegerCikar(htmlContent, "<span cid=\"1296\" dt=\"bA\"", ">", "</span>");
                var usdAlis = DecimalCevir(usdAlisStr);

                // USD satış değerini çek (HTML'den parse et)
                var usdSatisStr = DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1296\" dt=\"amount\"", ">", "</span>");
                var usdSatis = DecimalCevir(usdSatisStr);

                return (usdAlis, usdSatis);
            }
            catch (Exception ex)
            {
                throw new Exception("Fibabanka döviz kurları alınırken hata: " + ex.Message);
            }
        }

        // HTML içinden değer çıkaran yardımcı metot
        private string DegerCikar(string html, string baslangicIsareti, string onEk, string sonEk)
        {
            int baslangicIndex = html.IndexOf(baslangicIsareti);
            if (baslangicIndex == -1) return null;

            int onEkIndex = html.IndexOf(onEk, baslangicIndex);
            if (onEkIndex == -1) return null;

            int degerBaslangic = onEkIndex + onEk.Length;
            int sonEkIndex = html.IndexOf(sonEk, degerBaslangic);
            if (sonEkIndex == -1) return null;

            return html.Substring(degerBaslangic, sonEkIndex - degerBaslangic);
        }

        // String'i decimal'e çeviren yardımcı metot
        private decimal DecimalCevir(string deger)
        {
            if (string.IsNullOrEmpty(deger))
                throw new ArgumentException("Değer boş olamaz");

            // Noktayı virgüle çevir (Türkçe format)
            deger = deger.Replace(".", ",");

            if (decimal.TryParse(deger, NumberStyles.Any, CultureInfo.GetCultureInfo("tr-TR"), out decimal sonuc))
            {
                return sonuc;
            }

            throw new FormatException($"'{deger}' değeri decimal olarak çevrilemedi");
        }
    }

}

