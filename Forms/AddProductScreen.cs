using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Data.Services;
using neoStockMasterv2.Data.Services.BankServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace neoStockMasterv2.Forms
{
    public partial class AddProductScreen : Form
    {
        private ProductService _productService = new ProductService();
        public event EventHandler? ProductAdded;

        private List<ComboBox> forexComboBoxes;
        private bool isUpdatingComboBoxes = false;
        private Image originalSwapImage;

        private Dictionary<string, string> currencySymbols = new Dictionary<string, string>
        {
            { "Dolar", "$" },
            { "Euro", "€" },
            { "İngiliz Sterlini", "£" },
            { "İsviçre Frangı", "CHF" },
            { "Kanada Doları", "C$" },
            { "Rus Rublesi", "₽" },
            { "BAE Dirhemi", "AED" },
            { "Avustralya Doları", "A$" },
            { "Danimarka Kronu", "kr" },
            { "İsveç Kronu", "kr" },
            { "Norveç Kronu", "kr" },
            { "100 Japon Yeni", "¥" },
            { "Kuveyt Dinarı", "KWD" },
            { "Güney Afrika Randı", "R" },
            { "Arnavutluk Leki", "Lek" },
            { "Arjantin Pesosu", "$" },
            { "Azerbaycan Manatı", "₼" },
            { "Bosna-Hersek Markı", "KM" },
            { "Bulgar Levası", "лв" },
            { "Bahreyn Dinarı", "BHD" },
            { "Brezilya Reali", "R$" },
            { "Belarus Rublesi", "Br" },
            { "Şili Pesosu", "$" },
            { "Çin Yuanı", "¥" },
            { "Kolombiya Pesosu", "$" },
            { "Kosta Rika Kolonu", "₡" },
            { "Çek Korunası", "Kč" },
            { "Sepet Kur", "-" },
            { "Cezayir Dinarı", "DZD" },
            { "Mısır Lirası", "EGP" },
            { "Gürcistan Larisi", "₾" },
            { "Hong Kong Doları", "HK$" },
            { "Hırvat Kunası", "kn" },
            { "Macar Forinti", "Ft" },
            { "Endonezya Rupiahi", "Rp" },
            { "İsrail Şekeli", "₪" },
            { "Hindistan Rupisi", "₹" },
            { "Irak Dinarı", "IQD" },
            { "İran Riyali", "IRR" },
            { "İzlanda Kronası", "ISK" },
            { "Ürdün Dinarı", "JOD" },
            { "Güney Kore Wonu", "₩" },
            { "Kazan Tengesi", "₸" },
            { "Lübnan Lirası", "LBP" },
            { "Sri Lanka Rupisi", "LKR" },
            { "Litvanya Litası", "Lt" },
            { "Letonya Latsı", "Ls" },
            { "Libya Dinarı", "LYD" },
            { "Fas Dirhemi", "MAD" },
            { "Moldovya Leusu", "MDL" },
            { "Makedon Dinarı", "MKD" },
            { "Meksika Pesosu", "$" },
            { "Malezya Ringgiti", "RM" },
            { "Yeni Zelanda Doları", "NZ$" },
            { "Umman Riyali", "OMR" },
            { "Peru İnti", "S/." },
            { "Filipinler Pesosu", "₱" },
            { "Pakistan Rupisi", "PKR" },
            { "Polonya Zlotisi", "zł" },
            { "Katar Riyali", "QAR" },
            { "Romanya Leyi", "RON" },
            { "Sırbistan Dinarı", "RSD" },
            { "Suudi Arabistan Riyali", "SAR" },
            { "Singapur Doları", "S$" },
            { "Suriye Lirası", "SYP" },
            { "Tayland Bahtı", "฿" },
            { "Tunus Dinarı", "TND" },
            { "Yeni Tayvan Doları", "NT$" },
            { "Ukrayna Grivnası", "₴" },
            { "Uruguay Pesosu", "$U" }
        };

        List<string> allCurrencies = new List<string>
        {
            "Dolar", "Euro", "İngiliz Sterlini", "İsviçre Frangı", "Kanada Doları", "Rus Rublesi", "BAE Dirhemi", "Avustralya Doları",
            "Danimarka Kronu", "İsveç Kronu", "Norveç Kronu", "100 Japon Yeni", "Kuveyt Dinarı", "Güney Afrika Randı", "Arnavutluk Leki",
            "Arjantin Pesosu", "Azerbaycan Manatı", "Bosna-Hersek Markı", "Bulgar Levası", "Bahreyn Dinarı", "Brezilya Reali", "Belarus Rublesi",
            "Şili Pesosu", "Çin Yuanı", "Kolombiya Pesosu", "Kosta Rika Kolonu", "Çek Korunası", "Sepet Kur", "Cezayir Dinarı", "Mısır Lirası",
            "Gürcistan Larisi", "Hong Kong Doları", "Hırvat Kunası", "Macar Forinti", "Endonezya Rupiahi", "İsrail Şekeli", "Hindistan Rupisi",
            "Irak Dinarı", "İran Riyali", "İzlanda Kronası", "Ürdün Dinarı", "Güney Kore Wonu", "Kazan Tengesi", "Lübnan Lirası", "Sri Lanka Rupisi",
            "Litvanya Litası", "Letonya Latsı", "Libya Dinarı", "Fas Dirhemi", "Moldovya Leusu", "Makedon Dinarı", "Meksika Pesosu", "Malezya Ringgiti",
            "Yeni Zelanda Doları", "Umman Riyali", "Peru İnti", "Filipinler Pesosu", "Pakistan Rupisi", "Polonya Zlotisi", "Katar Riyali",
            "Romanya Leyi", "Sırbistan Dinarı", "Suudi Arabistan Riyali", "Singapur Doları", "Suriye Lirası", "Tayland Bahtı", "Tunus Dinarı",
            "Yeni Tayvan Doları", "Ukrayna Grivnası", "Uruguay Pesosu"
        };

        public static User LoggedInUser { get; set; }

        public AddProductScreen(string selectedLanguage)
        {
            InitializeComponent();

            LanguageService.SetLanguage(selectedLanguage);
            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            UpdateFormTexts();

            // Para Sembolleri Doldurma
            FillCurrencyComboBoxes();
            cmbForex1st.Items.AddRange(allCurrencies.ToArray());
            cmbForex2nd.Items.AddRange(allCurrencies.ToArray());
            cmbForex3rd.Items.AddRange(allCurrencies.ToArray());
            cmbForex4th.Items.AddRange(allCurrencies.ToArray());
            cmbForex5th.Items.AddRange(allCurrencies.ToArray());

            cmbForex1st.SelectedIndexChanged += (s, e) => UpdateCurrencySymbol(cmbForex1st, cmbSymbol1st);
            cmbForex2nd.SelectedIndexChanged += (s, e) => UpdateCurrencySymbol(cmbForex2nd, cmbSymbol2nd);
            cmbForex3rd.SelectedIndexChanged += (s, e) => UpdateCurrencySymbol(cmbForex3rd, cmbSymbol3rd);
            cmbForex4th.SelectedIndexChanged += (s, e) => UpdateCurrencySymbol(cmbForex4th, cmbSymbol4th);
            cmbForex5th.SelectedIndexChanged += (s, e) => UpdateCurrencySymbol(cmbForex5th, cmbSymbol5th);
            // Para Sembolleri Doldurma Bitiş

        }

        private async void AddProductScreen_Load(object sender, EventArgs e)
        {
            ConfigureForexUI();
            FillCurrencyProductComboBoxes();
            SetPersistentToolTip(pbSwap);

            // Combobox'ların event'lerini bağla
            cmbForex1st.SelectedIndexChanged += ForexComboBox_SelectedIndexChanged;
            cmbForex2nd.SelectedIndexChanged += ForexComboBox_SelectedIndexChanged;
            cmbForex3rd.SelectedIndexChanged += ForexComboBox_SelectedIndexChanged;
            cmbForex4th.SelectedIndexChanged += ForexComboBox_SelectedIndexChanged;
            cmbForex5th.SelectedIndexChanged += ForexComboBox_SelectedIndexChanged;


            cmbBanks.SelectedIndex = 0;

            // Form yüklenirken pbSwap'ın orijinal görselini kaydediyoruz
            originalSwapImage = pbSwap.Image;
        }

        private void ConfigureForexUI()
        {

            forexComboBoxes = new List<ComboBox>
            {
                cmbForex1st, cmbForex2nd, cmbForex3rd, cmbForex4th, cmbForex5th
            };

            foreach (var comboBox in forexComboBoxes)
            {
                comboBox.SelectedIndexChanged += ForexComboBox_SelectedIndexChanged;
            }

            LoadBanks();
            DisableExchangeControls();
            UpdateCrossRatesGroupVisibility(); // İlk durumu kontrol et
        }

        private void UpdateCrossRatesGroupVisibility()
        {
            int filledCount = forexComboBoxes.Count(cb => cb.SelectedItem != null && !string.IsNullOrEmpty(cb.SelectedItem.ToString()));
            grbCrossRates.Enabled = filledCount >= 2;

            // Eğer grbCrossRates disable edildiyse cmbFromMoney seçimini temizle
            if (!grbCrossRates.Enabled)
            {
                cmbFromMoney.SelectedIndex = -1;
                cmbFromMoney.Text = "";
                cmbFromMoney.Items.Clear();
            }
        }

        private void LanguageService_LanguageChanged()
        {
            UpdateFormTexts();
        }

        private void UpdateFormTexts()
        {
            türkçeToolStripMenuItem.Text = LanguageService.GetString("Türkçe");
            englishToolStripMenuItem.Text = LanguageService.GetString("English2");

            grbProduct.Text = LanguageService.GetString("Ürün Bilgileri");
            lblName.Text = LanguageService.GetString("İsim");
            lblCost.Text = LanguageService.GetString("Maliyet");
            lblPrice.Text = LanguageService.GetString("Fiyat");
            lblStock.Text = LanguageService.GetString("Stok");
            lblPercent.Text = LanguageService.GetString("Kâr Yüzdesi");
            grbExchangeInfo.Text = LanguageService.GetString("Döviz Bilgisi");
            lblBuying.Text = LanguageService.GetString("Alış");
            lblSelling.Text = LanguageService.GetString("Satış");
            btnClear.Text = LanguageService.GetString("Temizle");
            btnAdd.Text = LanguageService.GetString("Ekle");
            grbCrossRates.Text = LanguageService.GetString("Çarpraz Kurlar");
            btnRefresh.Text = LanguageService.GetString("Yenile");
            grbFrom.Text = LanguageService.GetString("Çevrilen");
            grbTo.Text = LanguageService.GetString("Çevrilmek İstenen");
            lblFromMoney.Text = LanguageService.GetString("Para Birimi");
            lblToMoney.Text = LanguageService.GetString("Para Birimi");
            lblFromMoneyUnite.Text = LanguageService.GetString("Birim Miktarı");
            lblToMoneyUnite.Text = LanguageService.GetString("Birim Miktarı");

            this.Text = LanguageService.GetString("Ürün Ekle");

            UpdateSelectedLanguage();
        }

        private void UpdateSelectedLanguage()
        {
            string currentLanguage = LanguageService.CurrentLanguage;

            türkçeToolStripMenuItem.Checked = currentLanguage == "Türkçe";
            englishToolStripMenuItem.Checked = currentLanguage == "English";

            türkçeToolStripMenuItem.BackColor = türkçeToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;
            englishToolStripMenuItem.BackColor = englishToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;

            // ToolTip güncellemesi
            SetPersistentToolTip(pbSwap);
        }

        private void türkçeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("Türkçe");
            UpdateSelectedLanguage();
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LanguageService.SetLanguage("English");
            UpdateSelectedLanguage();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            nmrCost.Value = 0.00M;
            nmrPrice.Value = 0.00M;
            nmrStock.Value = 0;
            nmrPercent.Value = 0;
        }

        private void FillCurrencyProductComboBoxes()
        {
            List<string> sortedCurrencies = new List<string>(allCurrencies);
            sortedCurrencies.Sort(); // Alfabetik sıralama

            // Türk Lirası'nı listenin başına ekle
            sortedCurrencies.Insert(0, "Türk Lirası");

            // ComboBox'ları doldur
            cmbCost.DataSource = new List<string>(sortedCurrencies);
            cmbPrice.DataSource = new List<string>(sortedCurrencies);

            // DropDown genişliğini ayarla
            SetComboBoxDropDownWidth(cmbCost);
            SetComboBoxDropDownWidth(cmbPrice);
            SetComboBoxDropDownWidth(cmbForex1st);
            SetComboBoxDropDownWidth(cmbForex2nd);
            SetComboBoxDropDownWidth(cmbForex3rd);
            SetComboBoxDropDownWidth(cmbForex4th);
            SetComboBoxDropDownWidth(cmbForex5th);

            cmbCost.SelectedIndex = 0;
            cmbPrice.SelectedIndex = 0;
        }

        private void SetComboBoxDropDownWidth(ComboBox comboBox)
        {
            // Graphics objesi oluştur
            using (Graphics g = comboBox.CreateGraphics())
            {
                // Font bilgisini al
                Font font = comboBox.Font;

                // En uzun metnin genişliğini bul
                int maxWidth = 0;
                foreach (string item in comboBox.Items)
                {
                    int itemWidth = (int)g.MeasureString(item, font).Width;
                    if (itemWidth > maxWidth)
                    {
                        maxWidth = itemWidth;
                    }
                }

                // ScrollBar genişliği için biraz padding ekle (yaklaşık 20 piksel)
                comboBox.DropDownWidth = maxWidth + 20;
            }
        }


        private void LoadBanks()
        {
            List<string> banks = new List<string>
            {
                "SERBEST PİYASA",
                "KAPALI ÇARŞI",
                "ZİRAAT BANKASI",
                "YAPI KREDİ",
                "VAKIFBANK",
                "TEB",
                "ŞEKERBANK",
                "KUVEYT TÜRK",
                "İŞ BANKASI",
                "ING BANK",
                "HSBC",
                "HALKBANK",
                "GARANTİ BANKASI",
                "FİNANSBANK",
                "DENİZBANK",
                "ALBARAKA",
                "AKBANK",
                "MERKEZ BANKASI",
                "ENPARA",
                "FİBABANKA"
            };

            cmbBanks.Items.Clear();
            cmbBanks.Items.AddRange(banks.ToArray());
            cmbBanks.SelectedIndex = 0;
        }

        private void DisableExchangeControls()
        {
            foreach (Control control in grbExchangeInfo.Controls)
            {
                if (control is NumericUpDown nud)
                {
                    nud.Enabled = false;
                }
            }

            cmbSymbol1st.Enabled = false;
            cmbSymbol2nd.Enabled = false;
            cmbSymbol3rd.Enabled = false;
            cmbSymbol4th.Enabled = false;
            cmbSymbol5th.Enabled = false;
        }

        private async void cmbBanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                isUpdatingComboBoxes = true; // Güncelleme bayrağını aktif et

                // Önce temel işlemleri yap
                nmrFromMoneyUnite.Value = 0.00m;
                nmrToMoneyUnite.Value = 0.00m;

                // Event'leri geçici olarak devre dışı bırak
                foreach (var cb in forexComboBoxes)
                    cb.SelectedIndexChanged -= ForexComboBox_SelectedIndexChanged;

                // Önce tüm değerleri sıfırla (her banka değişiminde)
                ResetExchangeInfoNumericUpDowns();

                // Seçili banka yoksa işlemi sonlandır
                if (cmbBanks.SelectedItem == null || string.IsNullOrEmpty(cmbBanks.SelectedItem.ToString()))
                {
                    // Event'leri yeniden bağla
                    foreach (var cb in forexComboBoxes)
                        cb.SelectedIndexChanged += ForexComboBox_SelectedIndexChanged;
                    return;
                }

                List<string> bankCurrencies = GetCurrenciesForSelectedBank();

                // ComboBox'ları temizle ve para birimlerini yükle
                for (int i = 0; i < forexComboBoxes.Count; i++)
                {
                    forexComboBoxes[i].Items.Clear();
                    forexComboBoxes[i].Items.AddRange(bankCurrencies.ToArray());
                    forexComboBoxes[i].SelectedItem = null;
                }

                // Normal para birimi atamasını yap (tüm bankalar için)
                for (int i = 0; i < forexComboBoxes.Count && i < bankCurrencies.Count; i++)
                {
                    forexComboBoxes[i].SelectedItem = bankCurrencies[i];
                }

                // SADECE HSBC seçilmişse özel işlemleri uygula
                if (cmbBanks.SelectedItem.ToString() == "HSBC")
                {
                    var hsbcService = new HSBCforex();
                    var rates = await hsbcService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuying;
                    nmrForexSelling1st.Value = rates.usdSelling;
                    nmrForexBuying2nd.Value = rates.euroBuying;
                    nmrForexSelling2nd.Value = rates.euroSelling;
                }

                // FİBABANKA seçilmişse
                else if (cmbBanks.SelectedItem.ToString() == "FİBABANKA")
                {
                    var fibaService = new FIBAforex();
                    var rates = await fibaService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdAlis;
                    nmrForexSelling1st.Value = rates.usdSatis;
                }

                // ENPARA seçilmişse
                else if (cmbBanks.SelectedItem.ToString() == "ENPARA")
                {
                    var enparaService = new ENPARAforex();
                    var rates = await enparaService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "MERKEZ BANKASI")
                {
                    var merkezService = new MERKEZforex();
                    var rates = await merkezService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "AKBANK")
                {
                    var akbankService = new AKBANKforex();
                    var rates = await akbankService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "ALBARAKA")
                {
                    var albarakaService = new ALBARAKAforex();
                    var rates = await albarakaService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                    nmrForexBuying3rd.Value = rates.gbpBuy;
                    nmrForexSelling3rd.Value = rates.gbpSell;
                    nmrForexBuying4th.Value = rates.sarBuy;
                    nmrForexSelling4th.Value = rates.sarSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "DENİZBANK")
                {
                    var denizbankService = new DENIZBANKforex();
                    var rates = await denizbankService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "HALKBANK")
                {
                    var halkbankService = new HALKBANKforex();
                    var rates = await halkbankService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "ING BANK")
                {
                    var ingService = new INGforex();
                    var rates = await ingService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "İŞ BANKASI")
                {
                    var isBankService = new ISBANKASIforex();
                    var rates = await isBankService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "ŞEKERBANK")
                {
                    var sekerBankService = new SEKERBANKforex();
                    var rates = await sekerBankService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                    nmrForexBuying3rd.Value = rates.gbpBuy;
                    nmrForexSelling3rd.Value = rates.gbpSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "TEB")
                {
                    var tebService = new TEBforex();
                    var rates = await tebService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                    nmrForexBuying3rd.Value = rates.gbpBuy;
                    nmrForexSelling3rd.Value = rates.gbpSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "VAKIFBANK")
                {
                    var vakifbankService = new VAKIFBANKforex();
                    var rates = await vakifbankService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "YAPI KREDİ")
                {
                    var yapiKrediService = new YAPIKREDIforex();
                    var rates = await yapiKrediService.GetExchangeRatesAsync();

                    nmrForexBuying1st.Value = rates.usdBuy;
                    nmrForexSelling1st.Value = rates.usdSell;
                    nmrForexBuying2nd.Value = rates.euroBuy;
                    nmrForexSelling2nd.Value = rates.euroSell;
                }

                else if (cmbBanks.SelectedItem.ToString() == "GARANTİ BANKASI")
                {
                    var garantiService = new GARANTIforex();
                    var rates = await garantiService.GetExchangeRatesAsync();

                    // Get the selected currencies from each combobox
                    var currency1 = cmbForex1st.SelectedItem?.ToString();
                    var currency2 = cmbForex2nd.SelectedItem?.ToString();
                    var currency3 = cmbForex3rd.SelectedItem?.ToString();
                    var currency4 = cmbForex4th.SelectedItem?.ToString();
                    var currency5 = cmbForex5th.SelectedItem?.ToString();

                    // Set values for each numeric up down based on the selected currencies
                    if (!string.IsNullOrEmpty(currency1) && rates.ContainsKey(currency1))
                    {
                        nmrForexBuying1st.Value = rates[currency1].BuyRate;
                        nmrForexSelling1st.Value = rates[currency1].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency2) && rates.ContainsKey(currency2))
                    {
                        nmrForexBuying2nd.Value = rates[currency2].BuyRate;
                        nmrForexSelling2nd.Value = rates[currency2].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency3) && rates.ContainsKey(currency3))
                    {
                        nmrForexBuying3rd.Value = rates[currency3].BuyRate;
                        nmrForexSelling3rd.Value = rates[currency3].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency4) && rates.ContainsKey(currency4))
                    {
                        nmrForexBuying4th.Value = rates[currency4].BuyRate;
                        nmrForexSelling4th.Value = rates[currency4].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency5) && rates.ContainsKey(currency5))
                    {
                        nmrForexBuying5th.Value = rates[currency5].BuyRate;
                        nmrForexSelling5th.Value = rates[currency5].SellRate;
                    }
                }

                else if (cmbBanks.SelectedItem.ToString() == "ZİRAAT BANKASI")
                {
                    var ziraatService = new ZIRAATforex();
                    var rates = await ziraatService.GetExchangeRatesAsync();

                    // Get the selected currencies from each combobox
                    var currency1 = cmbForex1st.SelectedItem?.ToString();
                    var currency2 = cmbForex2nd.SelectedItem?.ToString();
                    var currency3 = cmbForex3rd.SelectedItem?.ToString();
                    var currency4 = cmbForex4th.SelectedItem?.ToString();
                    var currency5 = cmbForex5th.SelectedItem?.ToString();

                    // Set values for each numeric up down based on the selected currencies
                    if (!string.IsNullOrEmpty(currency1) && rates.ContainsKey(currency1))
                    {
                        nmrForexBuying1st.Value = rates[currency1].BuyRate;
                        nmrForexSelling1st.Value = rates[currency1].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency2) && rates.ContainsKey(currency2))
                    {
                        nmrForexBuying2nd.Value = rates[currency2].BuyRate;
                        nmrForexSelling2nd.Value = rates[currency2].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency3) && rates.ContainsKey(currency3))
                    {
                        nmrForexBuying3rd.Value = rates[currency3].BuyRate;
                        nmrForexSelling3rd.Value = rates[currency3].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency4) && rates.ContainsKey(currency4))
                    {
                        nmrForexBuying4th.Value = rates[currency4].BuyRate;
                        nmrForexSelling4th.Value = rates[currency4].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency5) && rates.ContainsKey(currency5))
                    {
                        nmrForexBuying5th.Value = rates[currency5].BuyRate;
                        nmrForexSelling5th.Value = rates[currency5].SellRate;
                    }
                }

                else if (cmbBanks.SelectedItem.ToString() == "FİNANSBANK")
                {
                    var finansBankService = new FINANSforex();
                    var rates = await finansBankService.GetExchangeRatesAsync();

                    // Get the selected currencies from each combobox
                    var currency1 = cmbForex1st.SelectedItem?.ToString();
                    var currency2 = cmbForex2nd.SelectedItem?.ToString();
                    var currency3 = cmbForex3rd.SelectedItem?.ToString();
                    var currency4 = cmbForex4th.SelectedItem?.ToString();
                    var currency5 = cmbForex5th.SelectedItem?.ToString();

                    // Set values for each numeric up down based on the selected currencies
                    if (!string.IsNullOrEmpty(currency1) && rates.ContainsKey(currency1))
                    {
                        nmrForexBuying1st.Value = rates[currency1].BuyRate;
                        nmrForexSelling1st.Value = rates[currency1].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency2) && rates.ContainsKey(currency2))
                    {
                        nmrForexBuying2nd.Value = rates[currency2].BuyRate;
                        nmrForexSelling2nd.Value = rates[currency2].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency3) && rates.ContainsKey(currency3))
                    {
                        nmrForexBuying3rd.Value = rates[currency3].BuyRate;
                        nmrForexSelling3rd.Value = rates[currency3].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency4) && rates.ContainsKey(currency4))
                    {
                        nmrForexBuying4th.Value = rates[currency4].BuyRate;
                        nmrForexSelling4th.Value = rates[currency4].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency5) && rates.ContainsKey(currency5))
                    {
                        nmrForexBuying5th.Value = rates[currency5].BuyRate;
                        nmrForexSelling5th.Value = rates[currency5].SellRate;
                    }
                }

                else if (cmbBanks.SelectedItem.ToString() == "KAPALI ÇARŞI")
                {
                    var grandBazaarService = new GRANDBAZAARforex();
                    var rates = await grandBazaarService.GetExchangeRatesAsync();

                    // Get the selected currencies from each combobox
                    var currency1 = cmbForex1st.SelectedItem?.ToString();
                    var currency2 = cmbForex2nd.SelectedItem?.ToString();
                    var currency3 = cmbForex3rd.SelectedItem?.ToString();
                    var currency4 = cmbForex4th.SelectedItem?.ToString();
                    var currency5 = cmbForex5th.SelectedItem?.ToString();

                    // Set values for each numeric up down based on the selected currencies
                    if (!string.IsNullOrEmpty(currency1) && rates.ContainsKey(currency1))
                    {
                        nmrForexBuying1st.Value = rates[currency1].BuyRate;
                        nmrForexSelling1st.Value = rates[currency1].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency2) && rates.ContainsKey(currency2))
                    {
                        nmrForexBuying2nd.Value = rates[currency2].BuyRate;
                        nmrForexSelling2nd.Value = rates[currency2].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency3) && rates.ContainsKey(currency3))
                    {
                        nmrForexBuying3rd.Value = rates[currency3].BuyRate;
                        nmrForexSelling3rd.Value = rates[currency3].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency4) && rates.ContainsKey(currency4))
                    {
                        nmrForexBuying4th.Value = rates[currency4].BuyRate;
                        nmrForexSelling4th.Value = rates[currency4].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency5) && rates.ContainsKey(currency5))
                    {
                        nmrForexBuying5th.Value = rates[currency5].BuyRate;
                        nmrForexSelling5th.Value = rates[currency5].SellRate;
                    }
                }

                else if (cmbBanks.SelectedItem.ToString() == "KUVEYT TÜRK")
                {
                    var kuveytTurkService = new KUVEYTTURKforex();
                    var rates = await kuveytTurkService.GetExchangeRatesAsync();

                    // Get the selected currencies from each combobox
                    var currency1 = cmbForex1st.SelectedItem?.ToString();
                    var currency2 = cmbForex2nd.SelectedItem?.ToString();
                    var currency3 = cmbForex3rd.SelectedItem?.ToString();
                    var currency4 = cmbForex4th.SelectedItem?.ToString();
                    var currency5 = cmbForex5th.SelectedItem?.ToString();

                    // Set values for each numeric up down based on the selected currencies
                    if (!string.IsNullOrEmpty(currency1) && rates.ContainsKey(currency1))
                    {
                        nmrForexBuying1st.Value = rates[currency1].BuyRate;
                        nmrForexSelling1st.Value = rates[currency1].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency2) && rates.ContainsKey(currency2))
                    {
                        nmrForexBuying2nd.Value = rates[currency2].BuyRate;
                        nmrForexSelling2nd.Value = rates[currency2].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency3) && rates.ContainsKey(currency3))
                    {
                        nmrForexBuying3rd.Value = rates[currency3].BuyRate;
                        nmrForexSelling3rd.Value = rates[currency3].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency4) && rates.ContainsKey(currency4))
                    {
                        nmrForexBuying4th.Value = rates[currency4].BuyRate;
                        nmrForexSelling4th.Value = rates[currency4].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency5) && rates.ContainsKey(currency5))
                    {
                        nmrForexBuying5th.Value = rates[currency5].BuyRate;
                        nmrForexSelling5th.Value = rates[currency5].SellRate;
                    }
                }

                else if (cmbBanks.SelectedItem.ToString() == "SERBEST PİYASA")
                {
                    var freeMarketService = new FREEMARKETforex();
                    var rates = await freeMarketService.GetExchangeRatesAsync();

                    // Get the selected currencies from each combobox
                    var currency1 = cmbForex1st.SelectedItem?.ToString();
                    var currency2 = cmbForex2nd.SelectedItem?.ToString();
                    var currency3 = cmbForex3rd.SelectedItem?.ToString();
                    var currency4 = cmbForex4th.SelectedItem?.ToString();
                    var currency5 = cmbForex5th.SelectedItem?.ToString();

                    // Set values for each numeric up down based on the selected currencies
                    if (!string.IsNullOrEmpty(currency1) && rates.ContainsKey(currency1))
                    {
                        nmrForexBuying1st.Value = rates[currency1].BuyRate;
                        nmrForexSelling1st.Value = rates[currency1].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency2) && rates.ContainsKey(currency2))
                    {
                        nmrForexBuying2nd.Value = rates[currency2].BuyRate;
                        nmrForexSelling2nd.Value = rates[currency2].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency3) && rates.ContainsKey(currency3))
                    {
                        nmrForexBuying3rd.Value = rates[currency3].BuyRate;
                        nmrForexSelling3rd.Value = rates[currency3].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency4) && rates.ContainsKey(currency4))
                    {
                        nmrForexBuying4th.Value = rates[currency4].BuyRate;
                        nmrForexSelling4th.Value = rates[currency4].SellRate;
                    }

                    if (!string.IsNullOrEmpty(currency5) && rates.ContainsKey(currency5))
                    {
                        nmrForexBuying5th.Value = rates[currency5].BuyRate;
                        nmrForexSelling5th.Value = rates[currency5].SellRate;
                    }
                }


                // Event'leri yeniden bağla
                foreach (var cb in forexComboBoxes)
                    cb.SelectedIndexChanged += ForexComboBox_SelectedIndexChanged;

                UpdateAllForexComboBoxes();
                UpdateFromToMoneyComboBoxes();
                UpdateCrossRatesGroupVisibility();

                // Eğer Cross Rates disable ise FromMoney'i temizle
                if (!grbCrossRates.Enabled)
                {
                    cmbFromMoney.SelectedIndex = -1;
                    cmbFromMoney.Text = "";
                    cmbFromMoney.Items.Clear();

                    pbSwap.Enabled = false;
                    pbSwap.Image = SetImageOpacity(originalSwapImage, 0.4f); // Solgun yap
                }
                else
                {
                    pbSwap.Enabled = true;
                    pbSwap.Image = originalSwapImage; // Orijinaline geri dön
                }


            }

            catch (Exception ex)
            {
                MessageBox.Show($"İşlem sırasında hata oluştu: {ex.Message}", "Hata",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isUpdatingComboBoxes = false; // Güncelleme bayrağını pasif et
            }
        }

        private Image SetImageOpacity(Image image, float opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity; // 0.0f şeffaf, 1.0f normal
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }


        private void UpdateFromToMoneyComboBoxes()
        {
            try
            {
                // Mevcut seçimleri al
                var currentFrom = cmbFromMoney.SelectedItem?.ToString();
                var currentTo = cmbToMoney.SelectedItem?.ToString();

                // Bankanın desteklediği tüm dövizleri al
                var bankCurrencies = GetCurrenciesForSelectedBank();

                // Önce her iki combobox'ı da temizle
                cmbFromMoney.Items.Clear();
                cmbToMoney.Items.Clear();

                // Tüm dövizleri From combobox'ına ekle
                cmbFromMoney.Items.AddRange(bankCurrencies.ToArray());

                // From combobox'ında seçim yap
                if (currentFrom != null && cmbFromMoney.Items.Contains(currentFrom))
                {
                    cmbFromMoney.SelectedItem = currentFrom;
                }
                else if (cmbFromMoney.Items.Count > 0)
                {
                    cmbFromMoney.SelectedIndex = 0;
                }

                // To combobox'ını güncelle
                UpdateToComboBoxBasedOnFromSelection();

                // Eğer To'da bir seçim varsa, From'dan kaldır
                if (cmbToMoney.SelectedItem != null)
                {
                    UpdateFromComboBoxBasedOnToSelection();
                }

                cmbFromMoney_SelectedIndexChanged(cmbFromMoney, EventArgs.Empty);
                cmbToMoney_SelectedIndexChanged(cmbToMoney, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Döviz listesi güncellenirken hata: {ex.Message}", "Hata",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateFromComboBoxBasedOnToSelection()
        {
            var currentFrom = cmbFromMoney.SelectedItem?.ToString();
            cmbFromMoney.Items.Clear();

            if (cmbToMoney.SelectedItem != null)
            {
                string selectedTo = cmbToMoney.SelectedItem.ToString();
                var bankCurrencies = GetCurrenciesForSelectedBank();

                // To'da seçili olmayanları ekle
                foreach (var currency in bankCurrencies)
                {
                    if (currency != selectedTo)
                    {
                        cmbFromMoney.Items.Add(currency);
                    }
                }

                // Önceki seçimi koru veya ilkini seç
                if (currentFrom != null && cmbFromMoney.Items.Contains(currentFrom))
                {
                    cmbFromMoney.SelectedItem = currentFrom;
                }
                else if (cmbFromMoney.Items.Count > 0)
                {
                    cmbFromMoney.SelectedIndex = 0;
                }
            }
        }

        private void UpdateToComboBoxBasedOnFromSelection()
        {
            var currentTo = cmbToMoney.SelectedItem?.ToString();
            cmbToMoney.Items.Clear();

            if (cmbFromMoney.SelectedItem != null)
            {
                string selectedFrom = cmbFromMoney.SelectedItem.ToString();
                var bankCurrencies = GetCurrenciesForSelectedBank();

                // From'da seçili olmayanları ekle
                foreach (var currency in bankCurrencies)
                {
                    if (currency != selectedFrom)
                    {
                        cmbToMoney.Items.Add(currency);
                    }
                }

                // Önceki seçimi koru veya ilkini seç
                if (currentTo != null && cmbToMoney.Items.Contains(currentTo))
                {
                    cmbToMoney.SelectedItem = currentTo;
                }
                else if (cmbToMoney.Items.Count > 0)
                {
                    cmbToMoney.SelectedIndex = 0;
                }
            }
        }



        // grbExchangeInfo içindeki tüm NumericUpDown'ları sıfırlayan yardımcı metot
        private void ResetExchangeInfoNumericUpDowns()
        {
            // Belirli numeric updown'ları doğrudan sıfırla
            nmrForexBuying1st.Value = 0;
            nmrForexSelling1st.Value = 0;
            nmrForexBuying2nd.Value = 0;
            nmrForexSelling2nd.Value = 0;
            nmrForexBuying3rd.Value = 0;
            nmrForexSelling3rd.Value = 0;
            nmrForexBuying4th.Value = 0;
            nmrForexSelling4th.Value = 0;
            nmrForexBuying5th.Value = 0;
            nmrForexSelling5th.Value = 0;
        }

        private List<string> GetCurrenciesForSelectedBank()
        {
            string selectedBank = cmbBanks.SelectedItem?.ToString();

            cmbForex2nd.Enabled = true;
            cmbForex3rd.Enabled = true;
            cmbForex4th.Enabled = true;
            cmbForex5th.Enabled = true;

            bool disableExtraForex = selectedBank == "YAPI KREDİ" || selectedBank == "VAKIFBANK" || selectedBank == "TEB" || selectedBank == "ŞEKERBANK" || selectedBank == "İŞ BANKASI" || selectedBank == "ING BANK" || selectedBank == "HSBC" || selectedBank == "HALKBANK" || selectedBank == "DENİZBANK" || selectedBank == "AKBANK" || selectedBank == "MERKEZ BANKASI" || selectedBank == "ENPARA" || selectedBank == "FİBABANKA";
            cmbForex3rd.Enabled = !disableExtraForex;
            cmbForex4th.Enabled = !disableExtraForex;
            cmbForex5th.Enabled = !disableExtraForex;

            if (selectedBank == "ALBARAKA")
            {
                cmbForex5th.Enabled = false;
            }
            else
            {
                cmbForex5th.Enabled = !disableExtraForex;
            }



            if (selectedBank == "KAPALI ÇARŞI")
            {
                return new List<string>
        {
            "Dolar", "Euro", "İngiliz Sterlini", "İsviçre Frangı", "Kanada Doları",
            "Rus Rublesi", "BAE Dirhemi", "Avustralya Doları", "Danimarka Kronu",
            "İsveç Kronu", "Norveç Kronu", "100 Japon Yeni", "Bulgar Levası",
            "Çin Yuanı", "Katar Riyali", "Suudi Arabistan Riyali"
        };
            }

            if (selectedBank == "ZİRAAT BANKASI")
            {
                return new List<string>
        {
            "Dolar", "Euro", "İngiliz Sterlini", "İsviçre Frangı", "Kanada Doları",
            "Rus Rublesi", "Avustralya Doları", "Danimarka Kronu", "İsveç Kronu",
            "Norveç Kronu", "100 Japon Yeni", "Kuveyt Dinarı", "Çin Yuanı",
            "Katar Riyali", "Suudi Arabistan Riyali"
        };
            }

            if (selectedBank == "YAPI KREDİ")
            {
                return new List<string>
        {
            "Dolar", "Euro"
        };
            }

            if (selectedBank == "VAKIFBANK")
            {
                return new List<string>
        {
            "Dolar", "Euro"
        };
            }

            if (selectedBank == "TEB")
            {
                cmbForex3rd.Enabled = true;

                return new List<string>
        {
            "Dolar", "Euro", "İngiliz Sterlini"
        };
            }

            if (selectedBank == "ŞEKERBANK")
            {
                cmbForex3rd.Enabled = true;

                return new List<string>
        {
            "Dolar", "Euro", "İngiliz Sterlini"
        };
            }

            if (selectedBank == "KUVEYT TÜRK")
            {
                return new List<string>
        {
            "Dolar", "Euro", "İngiliz Sterlini", "İsviçre Frangı", "Kanada Doları", "Rus Rublesi", "BAE Dirhemi", "Avustralya Doları", "Danimarka Kronu", "İsveç Kronu", "Norveç Kronu", "Kuveyt Dinarı", "Bahreyn Dinarı", "Çin Yuanı", "Malezya Ringgiti", "Katar Riyali", "Suudi Arabistan Riyali"
        };
            }

            if (selectedBank == "İŞ BANKASI")
            {
                return new List<string>
        {
            "Dolar", "Euro"
        };
            }

            if (selectedBank == "ING BANK")
            {
                return new List<string>
        {
            "Dolar", "Euro"
        };
            }

            if (selectedBank == "HSBC")
            {
                return new List<string>
        {
            "Dolar", "Euro"
        };
            }

            if (selectedBank == "HALKBANK")
            {
                return new List<string>
        {
            "Dolar", "Euro"
        };
            }

            if (selectedBank == "GARANTİ BANKASI")
            {
                return new List<string>
        {
            "Dolar", "Euro", "İngiliz Sterlini", "İsviçre Frangı", "Kanada Doları", "Rus Rublesi", "Avustralya Doları", "Danimarka Kronu", "İsveç Kronu", "Norveç Kronu", "100 Japon Yeni", "Çin Yuanı", "Suudi Arabistan Riyali"
        };
            }

            if (selectedBank == "FİNANSBANK")
            {
                return new List<string>
        {
            "Dolar", "Euro", "İngiliz Sterlini", "İsviçre Frangı", "Kanada Doları", "Rus Rublesi", "BAE Dirhemi", "Avustralya Doları", "Danimarka Kronu", "İsveç Kronu", "Norveç Kronu", "Güney Afrika Randı", "Çin Yuanı", "Polonya Zlotisi", "Katar Riyali", "Suudi Arabistan Riyali"
        };
            }

            if (selectedBank == "DENİZBANK")
            {
                return new List<string>
        {
            "Dolar", "Euro"
        };
            }

            if (selectedBank == "ALBARAKA")
            {
                return new List<string>
        {
            "Dolar", "Euro", "İngiliz Sterlini", "İsviçre Frangı"
        };
            }

            if (selectedBank == "AKBANK")
            {
                cmbForex2nd.Enabled = false;

                return new List<string>
        {
            "Dolar"
        };
            }

            if (selectedBank == "MERKEZ BANKASI")
            {
                return new List<string>
        {
            "Dolar", "Euro"
        };
            }

            if (selectedBank == "ENPARA")
            {
                return new List<string>
        {
            "Dolar", "Euro"
        };
            }

            if (selectedBank == "FİBABANKA")
            {
                cmbForex2nd.Enabled = false;

                return new List<string>
        {
            "Dolar"
        };
            }

            return allCurrencies.ToList(); // Diğer bankalar için tam liste
        }

        private async void ForexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingComboBoxes) return;
            UpdateCrossRatesGroupVisibility();
            UpdateFromToMoneyComboBoxes();

            try
            {
                isUpdatingComboBoxes = true;

                // Hangi banka seçili olduğunu kontrol et
                if (cmbBanks.SelectedItem == null) return;

                string selectedBank = cmbBanks.SelectedItem.ToString();

                // Sadece Garanti Bankası için işlem yap
                if (selectedBank == "GARANTİ BANKASI")
                {
                    var service = new GARANTIforex();
                    var rates = await service.GetExchangeRatesAsync();

                    // Hangi combobox'ın değiştiğini tespit et
                    var changedComboBox = sender as ComboBox;

                    if (changedComboBox != null)
                    {
                        // Değişen combobox'a göre ilgili numericUpDown'ları güncelle
                        if (changedComboBox == cmbForex1st)
                        {
                            UpdateCurrencyValues(cmbForex1st, nmrForexBuying1st, nmrForexSelling1st, rates);
                        }
                        else if (changedComboBox == cmbForex2nd)
                        {
                            UpdateCurrencyValues(cmbForex2nd, nmrForexBuying2nd, nmrForexSelling2nd, rates);
                        }
                        else if (changedComboBox == cmbForex3rd)
                        {
                            UpdateCurrencyValues(cmbForex3rd, nmrForexBuying3rd, nmrForexSelling3rd, rates);
                        }
                        else if (changedComboBox == cmbForex4th)
                        {
                            UpdateCurrencyValues(cmbForex4th, nmrForexBuying4th, nmrForexSelling4th, rates);
                        }
                        else if (changedComboBox == cmbForex5th)
                        {
                            UpdateCurrencyValues(cmbForex5th, nmrForexBuying5th, nmrForexSelling5th, rates);
                        }
                    }
                }

                else if (selectedBank == "ZİRAAT BANKASI")
                {
                    var service = new ZIRAATforex();
                    var rates = await service.GetExchangeRatesAsync();

                    // Hangi combobox'ın değiştiğini tespit et
                    var changedComboBox = sender as ComboBox;

                    if (changedComboBox != null)
                    {
                        // Değişen combobox'a göre ilgili numericUpDown'ları güncelle
                        if (changedComboBox == cmbForex1st)
                        {
                            UpdateCurrencyValues(cmbForex1st, nmrForexBuying1st, nmrForexSelling1st, rates);
                        }
                        else if (changedComboBox == cmbForex2nd)
                        {
                            UpdateCurrencyValues(cmbForex2nd, nmrForexBuying2nd, nmrForexSelling2nd, rates);
                        }
                        else if (changedComboBox == cmbForex3rd)
                        {
                            UpdateCurrencyValues(cmbForex3rd, nmrForexBuying3rd, nmrForexSelling3rd, rates);
                        }
                        else if (changedComboBox == cmbForex4th)
                        {
                            UpdateCurrencyValues(cmbForex4th, nmrForexBuying4th, nmrForexSelling4th, rates);
                        }
                        else if (changedComboBox == cmbForex5th)
                        {
                            UpdateCurrencyValues(cmbForex5th, nmrForexBuying5th, nmrForexSelling5th, rates);
                        }
                    }
                }

                else if (selectedBank == "FİNANSBANK")
                {
                    var service = new FINANSforex();
                    var rates = await service.GetExchangeRatesAsync();

                    // Hangi combobox'ın değiştiğini tespit et
                    var changedComboBox = sender as ComboBox;

                    if (changedComboBox != null)
                    {
                        // Değişen combobox'a göre ilgili numericUpDown'ları güncelle
                        if (changedComboBox == cmbForex1st)
                        {
                            UpdateCurrencyValues(cmbForex1st, nmrForexBuying1st, nmrForexSelling1st, rates);
                        }
                        else if (changedComboBox == cmbForex2nd)
                        {
                            UpdateCurrencyValues(cmbForex2nd, nmrForexBuying2nd, nmrForexSelling2nd, rates);
                        }
                        else if (changedComboBox == cmbForex3rd)
                        {
                            UpdateCurrencyValues(cmbForex3rd, nmrForexBuying3rd, nmrForexSelling3rd, rates);
                        }
                        else if (changedComboBox == cmbForex4th)
                        {
                            UpdateCurrencyValues(cmbForex4th, nmrForexBuying4th, nmrForexSelling4th, rates);
                        }
                        else if (changedComboBox == cmbForex5th)
                        {
                            UpdateCurrencyValues(cmbForex5th, nmrForexBuying5th, nmrForexSelling5th, rates);
                        }
                    }
                }

                else if (selectedBank == "KAPALI ÇARŞI")
                {
                    var service = new GRANDBAZAARforex();
                    var rates = await service.GetExchangeRatesAsync();

                    // Hangi combobox'ın değiştiğini tespit et
                    var changedComboBox = sender as ComboBox;

                    if (changedComboBox != null)
                    {
                        // Değişen combobox'a göre ilgili numericUpDown'ları güncelle
                        if (changedComboBox == cmbForex1st)
                        {
                            UpdateCurrencyValues(cmbForex1st, nmrForexBuying1st, nmrForexSelling1st, rates);
                        }
                        else if (changedComboBox == cmbForex2nd)
                        {
                            UpdateCurrencyValues(cmbForex2nd, nmrForexBuying2nd, nmrForexSelling2nd, rates);
                        }
                        else if (changedComboBox == cmbForex3rd)
                        {
                            UpdateCurrencyValues(cmbForex3rd, nmrForexBuying3rd, nmrForexSelling3rd, rates);
                        }
                        else if (changedComboBox == cmbForex4th)
                        {
                            UpdateCurrencyValues(cmbForex4th, nmrForexBuying4th, nmrForexSelling4th, rates);
                        }
                        else if (changedComboBox == cmbForex5th)
                        {
                            UpdateCurrencyValues(cmbForex5th, nmrForexBuying5th, nmrForexSelling5th, rates);
                        }
                    }
                }

                else if (selectedBank == "SERBEST PİYASA")
                {
                    var service = new FREEMARKETforex();
                    var rates = await service.GetExchangeRatesAsync();

                    // Hangi combobox'ın değiştiğini tespit et
                    var changedComboBox = sender as ComboBox;

                    if (changedComboBox != null)
                    {
                        // Değişen combobox'a göre ilgili numericUpDown'ları güncelle
                        if (changedComboBox == cmbForex1st)
                        {
                            UpdateCurrencyValues(cmbForex1st, nmrForexBuying1st, nmrForexSelling1st, rates);
                        }
                        else if (changedComboBox == cmbForex2nd)
                        {
                            UpdateCurrencyValues(cmbForex2nd, nmrForexBuying2nd, nmrForexSelling2nd, rates);
                        }
                        else if (changedComboBox == cmbForex3rd)
                        {
                            UpdateCurrencyValues(cmbForex3rd, nmrForexBuying3rd, nmrForexSelling3rd, rates);
                        }
                        else if (changedComboBox == cmbForex4th)
                        {
                            UpdateCurrencyValues(cmbForex4th, nmrForexBuying4th, nmrForexSelling4th, rates);
                        }
                        else if (changedComboBox == cmbForex5th)
                        {
                            UpdateCurrencyValues(cmbForex5th, nmrForexBuying5th, nmrForexSelling5th, rates);
                        }
                    }
                }

                else if (selectedBank == "KUVEYT TÜRK")
                {
                    var service = new KUVEYTTURKforex();
                    var rates = await service.GetExchangeRatesAsync();

                    // Hangi combobox'ın değiştiğini tespit et
                    var changedComboBox = sender as ComboBox;

                    if (changedComboBox != null)
                    {
                        // Değişen combobox'a göre ilgili numericUpDown'ları güncelle
                        if (changedComboBox == cmbForex1st)
                        {
                            UpdateCurrencyValues(cmbForex1st, nmrForexBuying1st, nmrForexSelling1st, rates);
                        }
                        else if (changedComboBox == cmbForex2nd)
                        {
                            UpdateCurrencyValues(cmbForex2nd, nmrForexBuying2nd, nmrForexSelling2nd, rates);
                        }
                        else if (changedComboBox == cmbForex3rd)
                        {
                            UpdateCurrencyValues(cmbForex3rd, nmrForexBuying3rd, nmrForexSelling3rd, rates);
                        }
                        else if (changedComboBox == cmbForex4th)
                        {
                            UpdateCurrencyValues(cmbForex4th, nmrForexBuying4th, nmrForexSelling4th, rates);
                        }
                        else if (changedComboBox == cmbForex5th)
                        {
                            UpdateCurrencyValues(cmbForex5th, nmrForexBuying5th, nmrForexSelling5th, rates);
                        }
                    }
                }
                UpdateCrossRatesGroupVisibility();
                UpdateFromToMoneyComboBoxes();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Döviz kurları güncellenirken hata: {ex.Message}", "Hata",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            finally
            {
                isUpdatingComboBoxes = false;
            }

            UpdateAllForexComboBoxes();
        }

        private void UpdateCurrencyValues(ComboBox comboBox, NumericUpDown buyingControl,
                                NumericUpDown sellingControl,
                                Dictionary<string, (decimal buy, decimal sell)> rates)
        {
            try
            {
                if (comboBox.SelectedItem != null && rates != null)
                {
                    var currency = comboBox.SelectedItem.ToString();

                    // Eğer seçilen döviz için kur bilgisi varsa
                    if (rates.ContainsKey(currency))
                    {
                        buyingControl.Value = rates[currency].buy;
                        sellingControl.Value = rates[currency].sell;
                    }
                    else
                    {
                        // Kur bilgisi yoksa sıfırla
                        buyingControl.Value = 0;
                        sellingControl.Value = 0;

                        // İsteğe bağlı: Kullanıcıyı bilgilendir
                        MessageBox.Show($"{currency} döviz kuru bulunamadı.", "Bilgi",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                // Hata durumunda sıfırla
                buyingControl.Value = 0;
                sellingControl.Value = 0;
            }
        }

        private void UpdateAllForexComboBoxes()
        {
            isUpdatingComboBoxes = true;

            // Mevcut seçili para birimlerini listele
            var selectedValues = forexComboBoxes
                .Where(cb => cb.SelectedItem != null)
                .Select(cb => cb.SelectedItem.ToString())
                .ToList();

            foreach (var comboBox in forexComboBoxes)
            {
                var currentSelection = comboBox.SelectedItem?.ToString();

                // Uygun para birimlerini bul: Diğer seçimlerin dışında kalanlar
                var availableCurrencies = GetCurrenciesForSelectedBank()
                    .Where(currency => !selectedValues.Contains(currency) || currency == currentSelection)
                    .ToList();

                comboBox.SelectedIndexChanged -= ForexComboBox_SelectedIndexChanged;

                comboBox.Items.Clear();
                comboBox.Items.AddRange(availableCurrencies.ToArray());

                if (currentSelection != null && availableCurrencies.Contains(currentSelection))
                {
                    comboBox.SelectedItem = currentSelection;
                }
                else
                {
                    comboBox.SelectedItem = null;
                    ClearSymbolComboBox(comboBox);
                }

                comboBox.SelectedIndexChanged += ForexComboBox_SelectedIndexChanged;
            }

            isUpdatingComboBoxes = false;
        }

        private void ClearSymbolComboBox(ComboBox forexComboBox)
        {
            if (forexComboBox == cmbForex1st)
            {
                cmbSymbol1st.Items.Clear();
                cmbSymbol1st.Text = "";
            }
            else if (forexComboBox == cmbForex2nd)
            {
                cmbSymbol2nd.Items.Clear();
                cmbSymbol2nd.Text = "";
            }
            else if (forexComboBox == cmbForex3rd)
            {
                cmbSymbol3rd.Items.Clear();
                cmbSymbol3rd.Text = "";
            }
            else if (forexComboBox == cmbForex4th)
            {
                cmbSymbol4th.Items.Clear();
                cmbSymbol4th.Text = "";
            }
            else if (forexComboBox == cmbForex5th)
            {
                cmbSymbol5th.Items.Clear();
                cmbSymbol5th.Text = "";
            }
        }

        private void UpdateCurrencySymbol(ComboBox currencyCombo, ComboBox symbolCombo)
        {
            if (currencyCombo.SelectedItem == null || string.IsNullOrWhiteSpace(currencyCombo.SelectedItem.ToString()))
            {
                // If currency is not selected or empty, clear the symbol ComboBox
                symbolCombo.Items.Clear();
                symbolCombo.Text = "";
                return;
            }

            string selectedCurrency = currencyCombo.SelectedItem.ToString();
            if (currencySymbols.TryGetValue(selectedCurrency, out string symbol))
            {
                symbolCombo.Items.Clear();
                symbolCombo.Items.Add(symbol);
                symbolCombo.SelectedIndex = 0;
            }
            else
            {
                symbolCombo.Items.Clear();
                symbolCombo.Text = "";
            }
        }

        private void FillCurrencyComboBoxes()
        {
            List<string> allCurrencies = new List<string>
    {
        "Dolar", "Euro", "İngiliz Sterlini", "İsviçre Frangı", "Kanada Doları", "Rus Rublesi", "BAE Dirhemi", "Avustralya Doları",
        "Danimarka Kronu", "İsveç Kronu", "Norveç Kronu", "100 Japon Yeni", "Kuveyt Dinarı", "Güney Afrika Randı", "Arnavutluk Leki",
        "Arjantin Pesosu", "Azerbaycan Manatı", "Bosna-Hersek Markı", "Bulgar Levası", "Bahreyn Dinarı", "Brezilya Reali", "Belarus Rublesi",
        "Şili Pesosu", "Çin Yuanı", "Kolombiya Pesosu", "Kosta Rika Kolonu", "Çek Korunası", "Sepet Kur", "Cezayir Dinarı", "Mısır Lirası",
        "Gürcistan Larisi", "Hong Kong Doları", "Hırvat Kunası", "Macar Forinti", "Endonezya Rupiahi", "İsrail Şekeli", "Hindistan Rupisi",
        "Irak Dinarı", "İran Riyali", "İzlanda Kronası", "Ürdün Dinarı", "Güney Kore Wonu", "Kazan Tengesi", "Lübnan Lirası", "Sri Lanka Rupisi",
        "Litvanya Litası", "Letonya Latsı", "Libya Dinarı", "Fas Dirhemi", "Moldovya Leusu", "Makedon Dinarı", "Meksika Pesosu", "Malezya Ringgiti",
        "Yeni Zelanda Doları", "Umman Riyali", "Peru İnti", "Filipinler Pesosu", "Pakistan Rupisi", "Polonya Zlotisi", "Katar Riyali",
        "Romanya Leyi", "Sırbistan Dinarı", "Suudi Arabistan Riyali", "Singapur Doları", "Suriye Lirası", "Tayland Bahtı", "Tunus Dinarı",
        "Yeni Tayvan Doları", "Ukrayna Grivnası", "Uruguay Pesosu"
    };

            cmbForex1st.Items.AddRange(allCurrencies.ToArray());
            cmbForex2nd.Items.AddRange(allCurrencies.ToArray());
            cmbForex3rd.Items.AddRange(allCurrencies.ToArray());
            cmbForex4th.Items.AddRange(allCurrencies.ToArray());
            cmbForex5th.Items.AddRange(allCurrencies.ToArray());
        }

        private void cmbFromMoney_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingComboBoxes) return;

            try
            {
                isUpdatingComboBoxes = true;
                UpdateToComboBoxBasedOnFromSelection();
            }
            finally
            {
                isUpdatingComboBoxes = false;
            }
        }

        private void cmbToMoney_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingComboBoxes) return;

            try
            {
                isUpdatingComboBoxes = true;
                UpdateFromComboBoxBasedOnToSelection();
            }
            finally
            {
                isUpdatingComboBoxes = false;
            }
        }

        private void SetPersistentToolTip(PictureBox pictureBox, int delay = 1000)
        {
            string currentLanguage = LanguageService.CurrentLanguage;
            string text = currentLanguage == "Türkçe" ? "Buraya tıklanması halinde döviz değerleri yer değiştirir" : "Clicking here will swap the currency values";

            ttSwap.InitialDelay = delay;
            ttSwap.ReshowDelay = delay;
            ttSwap.AutoPopDelay = int.MaxValue;
            ttSwap.ShowAlways = true;

            ttSwap.SetToolTip(pictureBox, text);
        }

        private void pbSwap_MouseEnter(object sender, EventArgs e)
        {
            int growAmount = 1;

            pbSwap.Size = new Size(pbSwap.Width + growAmount * 2, pbSwap.Height + growAmount * 2);
            pbSwap.Location = new Point(pbSwap.Location.X - growAmount, pbSwap.Location.Y - growAmount);
        }

        private void pbSwap_MouseLeave(object sender, EventArgs e)
        {
            int shrinkAmount = 1;

            pbSwap.Size = new Size(pbSwap.Width - shrinkAmount * 2, pbSwap.Height - shrinkAmount * 2);
            pbSwap.Location = new Point(pbSwap.Location.X + shrinkAmount, pbSwap.Location.Y + shrinkAmount);
        }

        private void pbSwap_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Seçili para birimlerini değiştirmek istediğinize emin misiniz?",
        "Onay",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            if (result == DialogResult.Yes)
            {
                try
                {
                    isUpdatingComboBoxes = true;

                    // Mevcut seçimleri sakla
                    var fromSelected = cmbFromMoney.SelectedItem?.ToString();
                    var toSelected = cmbToMoney.SelectedItem?.ToString();

                    if (string.IsNullOrEmpty(fromSelected)) fromSelected = "";
                    if (string.IsNullOrEmpty(toSelected)) toSelected = "";

                    // FromMoney'i güncelle (toSelected'i ekleyip fromSelected'i çıkar)
                    var fromItems = new List<string>();
                    foreach (var item in cmbFromMoney.Items)
                    {
                        if (item.ToString() != fromSelected)
                            fromItems.Add(item.ToString());
                    }
                    if (!string.IsNullOrEmpty(toSelected) && !fromItems.Contains(toSelected))
                        fromItems.Add(toSelected);

                    cmbFromMoney.Items.Clear();
                    cmbFromMoney.Items.AddRange(fromItems.ToArray());
                    if (!string.IsNullOrEmpty(toSelected))
                        cmbFromMoney.SelectedItem = toSelected;

                    // ToMoney'i güncelle (fromSelected'i ekleyip toSelected'i çıkar)
                    var toItems = new List<string>();
                    foreach (var item in cmbToMoney.Items)
                    {
                        if (item.ToString() != toSelected)
                            toItems.Add(item.ToString());
                    }
                    if (!string.IsNullOrEmpty(fromSelected) && !toItems.Contains(fromSelected))
                        toItems.Add(fromSelected);

                    cmbToMoney.Items.Clear();
                    cmbToMoney.Items.AddRange(toItems.ToArray());
                    if (!string.IsNullOrEmpty(fromSelected))
                        cmbToMoney.SelectedItem = fromSelected;

                    // Numeric değerleri sıfırla
                    nmrFromMoneyUnite.Value = 0.00m;
                    nmrToMoneyUnite.Value = 0.00m;
                }
                finally
                {
                    isUpdatingComboBoxes = false;
                }
            }

            else
            {
                // Hiçbir şey yapma
            }
        }

        private void cmbFromMoney_DropDown(object sender, EventArgs e)
        {
            SetComboBoxDropDownWidth(cmbFromMoney);
        }

        private void cmbToMoney_DropDown(object sender, EventArgs e)
        {
            SetComboBoxDropDownWidth(cmbToMoney);
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshRatesAsync();
        }


        private async Task RefreshRatesAsync()
        {
            if (cmbBanks.SelectedItem == null)
            {
                MessageBox.Show("Lütfen önce bir banka seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedBank = cmbBanks.SelectedItem.ToString();

            try
            {
                Dictionary<string, (decimal buy, decimal sell)> rates = null;

                switch (selectedBank)
                {
                    case "YAPI KREDİ":
                        {
                            var service = new YAPIKREDIforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) }
                    };
                            break;
                        }
                    case "MERKEZ BANKASI":
                        {
                            var service = new MERKEZforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) }
                    };
                            break;
                        }
                    case "ENPARA":
                        {
                            var service = new ENPARAforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) }
                    };
                            break;
                        }
                    case "AKBANK":
                        {
                            var service = new AKBANKforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) }
                    };
                            break;
                        }
                    case "FİBABANKA":
                        {
                            var service = new FIBAforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdAlis, tupleRates.usdSatis) }
                    };
                            break;
                        }
                    case "ING BANK":
                        {
                            var service = new INGforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) }
                    };
                            break;
                        }
                    case "ALBARAKA":
                        {
                            var service = new ALBARAKAforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) },
                        { "İngiliz Sterlini", (tupleRates.gbpBuy, tupleRates.gbpSell) },
                        { "İsviçre Frangı", (tupleRates.sarBuy, tupleRates.sarSell) }
                    };
                            break;
                        }
                    case "DENİZBANK":
                        {
                            var service = new DENIZBANKforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) }
                    };
                            break;
                        }
                    case "HALKBANK":
                        {
                            var service = new HALKBANKforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) }
                    };
                            break;
                        }
                    case "HSBC":
                        {
                            var service = new HSBCforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuying, tupleRates.usdSelling) },
                        { "Euro", (tupleRates.euroBuying, tupleRates.usdSelling) }
                    };
                            break;
                        }
                    case "SERBEST PİYASA":
                        {
                            var service = new FREEMARKETforex();
                            rates = await service.GetExchangeRatesAsync();
                            break;
                        }
                    case "KUVEYT TÜRK":
                        {
                            var service = new KUVEYTTURKforex();
                            rates = await service.GetExchangeRatesAsync();
                            break;
                        }
                    case "KAPALI ÇARŞI":
                        {
                            var service = new GRANDBAZAARforex();
                            rates = await service.GetExchangeRatesAsync();
                            break;
                        }
                    case "VAKIFBANK":
                        {
                            var service = new VAKIFBANKforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) }
                    };
                            break;
                        }
                    case "İŞ BANKASI":
                        {
                            var service = new ISBANKASIforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) }
                    };
                            break;
                        }
                    case "TEB":
                        {
                            var service = new TEBforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) },
                        { "İngiliz Sterlini", (tupleRates.gbpBuy, tupleRates.gbpSell) }
                    };
                            break;
                        }
                    case "ŞEKERBANK":
                        {
                            var service = new SEKERBANKforex();
                            var tupleRates = await service.GetExchangeRatesAsync();
                            rates = new Dictionary<string, (decimal buy, decimal sell)>
                    {
                        { "Dolar", (tupleRates.usdBuy, tupleRates.usdSell) },
                        { "Euro", (tupleRates.euroBuy, tupleRates.euroSell) },
                        { "İngiliz Sterlini", (tupleRates.gbpBuy, tupleRates.gbpSell) }
                    };
                            break;
                        }
                    case "GARANTİ BANKASI":
                        {
                            var service = new GARANTIforex();
                            rates = await service.GetExchangeRatesAsync();
                            break;
                        }
                    case "ZİRAAT BANKASI":
                        {
                            var service = new ZIRAATforex();
                            rates = await service.GetExchangeRatesAsync();
                            break;
                        }
                    case "FİNANSBANK":
                        {
                            var service = new FINANSforex();
                            rates = await service.GetExchangeRatesAsync();
                            break;
                        }
                    // Buraya diğer bankaları ekleyebilirsin
                    default:
                        {
                            MessageBox.Show("Seçili banka için döviz bilgisi bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                }

                // 1'den 5'e kadar olan alanları güncelle
                UpdateCurrencyValues(cmbForex1st, nmrForexBuying1st, nmrForexSelling1st, rates);
                UpdateCurrencyValues(cmbForex2nd, nmrForexBuying2nd, nmrForexSelling2nd, rates);
                UpdateCurrencyValues(cmbForex3rd, nmrForexBuying3rd, nmrForexSelling3rd, rates);
                UpdateCurrencyValues(cmbForex4th, nmrForexBuying4th, nmrForexSelling4th, rates);
                UpdateCurrencyValues(cmbForex5th, nmrForexBuying5th, nmrForexSelling5th, rates);

                MessageBox.Show($"{selectedBank} döviz değerleri başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // İlk hesaplama (From -> To)
                //CalculateConversion(true); DİKKAT ET

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Döviz kurları güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ÜRÜN EKLEME İŞLEMLERİ BAŞLANGIÇ
        private void UpdateProfitPercent(bool fromPercentChange = false)
        {
            if (cmbCost.SelectedItem == null || cmbPrice.SelectedItem == null)
                return;

            string costCurrency = cmbCost.SelectedItem.ToString();
            string priceCurrency = cmbPrice.SelectedItem.ToString();

            decimal cost = nmrCost.Value;

            if (costCurrency == priceCurrency)
            {
                nmrPercent.Enabled = true;

                if (fromPercentChange)
                {
                    // Kullanıcı kar yüzdesini değiştirmiş → fiyat hesapla
                    decimal percent = nmrPercent.Value;
                    decimal newPrice = cost + (cost * percent / 100);
                    nmrPrice.Value = Math.Round(newPrice, 2);
                }
                else
                {
                    // Kullanıcı fiyat veya maliyet değiştirmiş → kar yüzdesi hesapla
                    decimal price = nmrPrice.Value;
                    if (cost > 0)
                    {
                        decimal profitPercent = ((price - cost) / cost) * 100;
                        nmrPercent.Value = (decimal)Math.Round(profitPercent, 2);
                    }
                    else
                    {
                        nmrPercent.Value = 0;
                    }
                }
            }
            else
            {
                nmrPercent.Enabled = false;
                nmrPercent.Value = 0;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Lütfen ürün adını giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtName.Text.Trim();
            decimal cost = nmrCost.Value;
            string costCurrency = cmbCost.SelectedItem.ToString();
            decimal price = nmrPrice.Value;
            string priceCurrency = cmbPrice.SelectedItem.ToString();
            int stock = (int)nmrStock.Value;
            string addedBy = LoggedInUser?.Name ?? "Unknown";

            // Onay mesajı oluştur
            string confirmationMessage = $"Ürün Bilgileri:\n\n" +
                                         $"Ad: {name}\n" +
                                         $"Maliyet: {cost} {costCurrency}\n" +
                                         $"Satış: {price} {priceCurrency}\n" +
                                         $"Stok: {stock}\n" +
                                         $"Bu ürünü eklemek istiyor musunuz?";

            var result = MessageBox.Show(confirmationMessage, "Ürün Ekleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var product = new Product
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = name,
                    Cost = cost,
                    CostCurrency = costCurrency,
                    Price = price,
                    PriceCurrency = priceCurrency,
                    Stock = stock,
                    AddedBy = addedBy,
                    DateAdded = DateTime.Now
                };

                _productService.AddProduct(product);
                btnClear.PerformClick(); // btnClear tıklanmış gibi yap

                ProductAdded?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Ürün ekleme işlemi iptal edildi.", "İptal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void nmrCost_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateProfitPercent();
        }

        private void cmbCost_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProfitPercent();
        }

        private void cmbPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProfitPercent();
        }

        private void nmrCost_ValueChanged(object sender, EventArgs e)
        {
            UpdateProfitPercent();
        }

        private void nmrPrice_ValueChanged(object sender, EventArgs e)
        {
            UpdateProfitPercent();
        }

        private void nmrPrice_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateProfitPercent();
        }

        private void nmrPercent_ValueChanged(object sender, EventArgs e)
        {
            if (nmrPercent.Enabled)
                UpdateProfitPercent(true);
        }

        private void nmrPercent_KeyUp(object sender, KeyEventArgs e)
        {
            if (nmrPercent.Enabled)
                UpdateProfitPercent(true);
        }

        // ÜRÜN EKLEME İŞLEMLERİ BİTİŞ
    }
}
