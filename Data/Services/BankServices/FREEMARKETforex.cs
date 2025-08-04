using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace neoStockMasterv2.Data.Services.BankServices
{
    public class FREEMARKETforex
    {
        private readonly HttpClient _httpClient;

        public FREEMARKETforex()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<Dictionary<string, (decimal BuyRate, decimal SellRate)>> GetExchangeRatesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://canlidoviz.com/doviz-kurlari");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();

                var rates = new Dictionary<string, (decimal, decimal)>();

                // USD
                rates["Dolar"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"1\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"1\" dt=\"amount\"", ">", "</span>"))
                );

                // EUR
                rates["Euro"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"50\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"50\" dt=\"amount\"", ">", "</span>"))
                );

                // GBP
                rates["İngiliz Sterlini"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"100\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"100\" dt=\"amount\"", ">", "</span>"))
                );

                // CHF
                rates["İsviçre Frangı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"51\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"51\" dt=\"amount\"", ">", "</span>"))
                );

                // CAD
                rates["Kanada Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"56\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"56\" dt=\"amount\"", ">", "</span>"))
                );

                // RUB
                rates["Rus Rublesi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"97\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"97\" dt=\"amount\"", ">", "</span>"))
                );

                // AED
                rates["BAE Dirhemi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"53\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"53\" dt=\"amount\"", ">", "</span>"))
                );

                // AUD
                rates["Avustralya Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"102\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"102\" dt=\"amount\"", ">", "</span>"))
                );

                // DKK
                rates["Danimarka Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"54\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"54\" dt=\"amount\"", ">", "</span>"))
                );

                // SEK
                rates["İsveç Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"60\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"60\" dt=\"amount\"", ">", "</span>"))
                );

                // NOK
                rates["Norveç Kronu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"99\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"99\" dt=\"amount\"", ">", "</span>"))
                );

                // JPY
                rates["100 Japon Yeni"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"57\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"57\" dt=\"amount\"", ">", "</span>"))
                );

                // KWD
                rates["Kuveyt Dinarı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"104\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"104\" dt=\"amount\"", ">", "</span>"))
                );

                // ZAR
                rates["Güney Afrika Randı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"59\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"59\" dt=\"amount\"", ">", "</span>"))
                );

                // ALL
                rates["Arnavutluk Leki"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"112\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"112\" dt=\"amount\"", ">", "</span>"))
                );

                // ARS
                rates["Arjantin Pesosu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"73\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"73\" dt=\"amount\"", ">", "</span>"))
                );

                // AZN
                rates["Azerbaycan Manatı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"75\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"75\" dt=\"amount\"", ">", "</span>"))
                );

                // BAM
                rates["Bosna-Hersek Markı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"82\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"82\" dt=\"amount\"", ">", "</span>"))
                );

                // BGN
                rates["Bulgar Levası"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"71\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"71\" dt=\"amount\"", ">", "</span>"))
                );

                // BHD
                rates["Bahreyn Dinarı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"64\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"64\" dt=\"amount\"", ">", "</span>"))
                );

                // BRL
                rates["Brezilya Reali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"74\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"74\" dt=\"amount\"", ">", "</span>"))
                );

                // BYR
                rates["Belarus Rublesi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"109\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"109\" dt=\"amount\"", ">", "</span>"))
                );

                // CLP
                rates["Şili Pesosu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"76\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"76\" dt=\"amount\"", ">", "</span>"))
                );

                // CNY
                rates["Çin Yuanı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"107\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"107\" dt=\"amount\"", ">", "</span>"))
                );

                // COP
                rates["Kolombiya Pesosu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"114\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"114\" dt=\"amount\"", ">", "</span>"))
                );

                // CRC
                rates["Kosta Rika Kolonu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"79\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"79\" dt=\"amount\"", ">", "</span>"))
                );

                // CSK
                rates["Çek Korunası"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"69\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"69\" dt=\"amount\"", ">", "</span>"))
                );

                // DVZSP1
                rates["Sepet Kur"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"783\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"783\" dt=\"amount\"", ">", "</span>"))
                );

                // DZD
                rates["Cezayir Dinarı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"88\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"88\" dt=\"amount\"", ">", "</span>"))
                );

                // EGP
                rates["Mısır Lirası"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"111\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"111\" dt=\"amount\"", ">", "</span>"))
                );

                // GEL
                rates["Gürcistan Larisi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"162\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"162\" dt=\"amount\"", ">", "</span>"))
                );

                // HKD
                rates["Hong Kong Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"80\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"80\" dt=\"amount\"", ">", "</span>"))
                );

                // HRK
                rates["Hırvat Kunası"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"116\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"116\" dt=\"amount\"", ">", "</span>"))
                );

                // HUF
                rates["Macar Forinti"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"108\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"108\" dt=\"amount\"", ">", "</span>"))
                );

                // IDR
                rates["Endonezya Rupiahi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"105\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"105\" dt=\"amount\"", ">", "</span>"))
                );

                // ILS
                rates["İsrail Şekeli"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"63\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"63\" dt=\"amount\"", ">", "</span>"))
                );

                // INR
                rates["Hindistan Rupisi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"103\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"103\" dt=\"amount\"", ">", "</span>"))
                );

                // IQD
                rates["Irak Dinarı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"106\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"106\" dt=\"amount\"", ">", "</span>"))
                );

                // IRR
                rates["İran Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"68\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"68\" dt=\"amount\"", ">", "</span>"))
                );

                // ISK
                rates["İzlanda Kronası"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"83\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"83\" dt=\"amount\"", ">", "</span>"))
                );

                // JOD
                rates["Ürdün Dinarı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"92\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"92\" dt=\"amount\"", ">", "</span>"))
                );

                // KRW
                rates["Güney Kore Wonu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"113\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"113\" dt=\"amount\"", ">", "</span>"))
                );

                // KZT
                rates["Kazak Tengesi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"85\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"85\" dt=\"amount\"", ">", "</span>"))
                );

                // LBP
                rates["Lübnan Lirası"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"117\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"117\" dt=\"amount\"", ">", "</span>"))
                );

                // LKR
                rates["Sri Lanka Rupisi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"87\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"87\" dt=\"amount\"", ">", "</span>"))
                );

                // LTL
                rates["Litvanya Litası"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"95\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"95\" dt=\"amount\"", ">", "</span>"))
                );

                // LVL
                rates["Letonya Latsı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"115\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"115\" dt=\"amount\"", ">", "</span>"))
                );

                // LYD
                rates["Libya Dinarı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"101\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"101\" dt=\"amount\"", ">", "</span>"))
                );

                // MAD
                rates["Fas Dirhemi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"89\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"89\" dt=\"amount\"", ">", "</span>"))
                );

                // MDL
                rates["Moldovya Leusu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"10\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"10\" dt=\"amount\"", ">", "</span>"))
                );

                // MKD
                rates["Makedon Dinarı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"21\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"21\" dt=\"amount\"", ">", "</span>"))
                );

                // MXN
                rates["Meksika Pesosu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"65\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"65\" dt=\"amount\"", ">", "</span>"))
                );

                // MYR
                rates["Malezya Ringgiti"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"3\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"3\" dt=\"amount\"", ">", "</span>"))
                );

                // NZD
                rates["Yeni Zelanda Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"67\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"67\" dt=\"amount\"", ">", "</span>"))
                );

                // OMR
                rates["Umman Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"2\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"2\" dt=\"amount\"", ">", "</span>"))
                );

                // PEN
                rates["Peru İnti"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"13\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"13\" dt=\"amount\"", ">", "</span>"))
                );

                // PHP
                rates["Filipinler Pesosu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"4\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"4\" dt=\"amount\"", ">", "</span>"))
                );

                // PKR
                rates["Pakistan Rupisi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"29\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"29\" dt=\"amount\"", ">", "</span>"))
                );

                // PLN
                rates["Polonya Zlotisi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"110\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"110\" dt=\"amount\"", ">", "</span>"))
                );

                // QAR
                rates["Katar Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"5\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"5\" dt=\"amount\"", ">", "</span>"))
                );

                // RON
                rates["Romanya Leyi"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"77\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"77\" dt=\"amount\"", ">", "</span>"))
                );

                // RSD
                rates["Sırbistan Dinarı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"7\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"7\" dt=\"amount\"", ">", "</span>"))
                );

                // SAR
                rates["Suudi Arabistan Riyali"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"61\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"61\" dt=\"amount\"", ">", "</span>"))
                );

                // SGD
                rates["Singapur Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"17\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"17\" dt=\"amount\"", ">", "</span>"))
                );

                // SYP
                rates["Suriye Lirası"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"6\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"6\" dt=\"amount\"", ">", "</span>"))
                );

                // THB
                rates["Tayland Bahtı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"39\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"39\" dt=\"amount\"", ">", "</span>"))
                );

                // TND
                rates["Tunus Dinarı"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"85\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"85\" dt=\"amount\"", ">", "</span>"))
                );

                // TWD
                rates["Yeni Tayvan Doları"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"9\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"9\" dt=\"amount\"", ">", "</span>"))
                );

                // UAH
                rates["Ukrayna Grivnası"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"8\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"8\" dt=\"amount\"", ">", "</span>"))
                );

                // UYU
                rates["Uruguay Pesosu"] = (
                    DecimalCevir(DegerCikar(htmlContent, "<span cid=\"25\" dt=\"bA\"", ">", "</span>")),
                    DecimalCevir(DegerCikar(htmlContent, "<span itemprop=\"price\" cid=\"25\" dt=\"amount\"", ">", "</span>"))
                );


                return rates;
            }
            catch (Exception ex)
            {
                throw new Exception("Serbest Piyasa döviz kurları alınırken hata: " + ex.Message);
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
