using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Data.Services;
using neoStockMasterv2.Data.Services.BankServices;
using neoStockMasterv2.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace neoStockMasterv2.Forms
{
    public partial class PricingOrderScreen : Form
    {
        public static User LoggedInUser { get; set; }
        private int previousCargoIndex = -1;
        private ProductService _productService;
        TextBox editingControl;
        private ComboBox currencyComboBox = null;
        private bool isExpanded = false;    // Başlangıçta kapalı varsayıyoruz
        private int originalHeight;         // GroupBox'ın orijinal yüksekliği

        private ListView hoveredListView; // Hangi ListView'de hover olduğunu tutar
        private bool isHovering = false;

        // --- lwDisc kolon indeksleri (SubItems) ---
        const int COL_CHECKBOX = 0;     // 0: Checkbox (ListViewItem.Checkbox alanı, otomatik)
        const int COL_NAME = 1;         // 1: Ürün Adı
        const int COL_TOTAL = 2;        // 2: Tutar (başlangıçta indirimsiz, indirim uygulandığında güncellenir)
        const int COL_CURRENCY = 3;     // 3: Para Birimi
        const int COL_DISC_STATUS = 4;  // 4: İndirim Durumu (örn: %10)
        const int COL_DISC_AMOUNT = 5;  // 5: İndirim Miktarı (hesaplanan TL/Euro vb. değer)
        const int COL_SCT = 6;          // 6: ÖTV Durumu
        const int COL_VAT = 7;          // 7: KDV Durumu 

        private ToolTip ttAll = new ToolTip();

        public PricingOrderScreen(string currentLanguage)
        {
            InitializeComponent();
            _productService = new ProductService();

            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            LanguageService.SetLanguage(LanguageService.CurrentLanguage);

            ProductService.OnProductsChanged += LoadUserProducts; // Ürün değişince yenile

            // Tutar değiştikçe vergilerin otomaik SEhesaplanması için event bağlantıları
            cmbDisc.SelectedIndexChanged += async (s, e) => await ApplyDiscountAndTaxes();
            nmrDisc.ValueChanged += async (s, e) => await ApplyDiscountAndTaxes();
            cmbSCT.SelectedIndexChanged += async (s, e) => await ApplyDiscountAndTaxes();
            cmbVAT.SelectedIndexChanged += async (s, e) => await ApplyDiscountAndTaxes();

            this.Height = 555; // Form başlangıç yüksekliği grbCustomerMsg daraltılmış geldiği için
        }

        private void PricingOrderScreen_Load(object sender, EventArgs e)
        {
            FillComboBoxes();

            InitializeProductGrid();
            InitializeOrderDetailsGrid();

            SetModernHeaderStyle(dgwProducts);
            SetModernHeaderStyle(dgwOrderDetails);

            InitializeLwDisc();

            LoadUserProducts();
            ApplyColumnLockStyles();

            // Event'leri tanımla dgwOrderDetails için lwDisc tablosuna düşürme
            dgwOrderDetails.CellValueChanged += dgwOrderDetails_Changed;
            dgwOrderDetails.RowsRemoved += dgwOrderDetails_Changed;
            dgwOrderDetails.RowsAdded += dgwOrderDetails_Changed;

            UpdateListViewFromDGV();
            ResetDiscountControls(); // Form açıldığında kontroller kapalı ve sıfırlanmış olsun

            originalHeight = grbCustomerMsg.Height;
            grbCustomerMsg.Height = 38; // Başlangıçta sadece başlık görünsün

            UpdateMessage();
            isWhatsApp();

            InitTotalListViewTotal(); // Toplam ListView'ı başlat
            InitTotalListViewDisc(); // İndirim ListView'ı başlat
            InitTotalListViewTax(); // Vergi ListView'ı başlat

            InitializeHoverTooltips(); // Form üzerindeki bütün lw'lere tooltip ekle
            MakeRichTextBoxUnselectable(rchMessage);
        }

        private void LanguageService_LanguageChanged()
        {
            UpdateFormTexts();
            FillComboBoxes();
            UpdatePlaceholders();
            InitializeProductGrid();
            InitializeOrderDetailsGrid();
            LoadUserProducts();
            InitializeLwDisc();
            UpdateMessage();
        }

        private void UpdateFormTexts()
        {
            türkçeToolStripMenuItem.Text = LanguageService.GetString("Türkçe");
            englishToolStripMenuItem.Text = LanguageService.GetString("English2");

            grbPriceDetails.Text = LanguageService.GetString("Fiyat Detayları");
            lblTotalPrice.Text = LanguageService.GetString("Toplam Tutar");
            lblTotalDisc.Text = LanguageService.GetString("Toplam İndirim");
            lblTotalTax.Text = LanguageService.GetString("Vergi");
            grbDisc.Text = LanguageService.GetString("İndirim");
            grbTax.Text = LanguageService.GetString("Vergi");
            grbCustomerMsg.Text = LanguageService.GetString("Müşteri Bilgilendirme Metni");
            btnClear.Text = LanguageService.GetString("Temizle");
            btnCopy.Text = LanguageService.GetString("Kopyala");
            btnWhatsapp.Text = LanguageService.GetString("WhatsApp'ı Aç");
            grbOrderDetails.Text = LanguageService.GetString("Sipariş Detayları");
            btnAddOrder.Text = LanguageService.GetString("Sipariş Ekle");
            chbLockPrice.Text = LanguageService.GetString("Fiyatları Kilitle");
            chbLockForex.Text = LanguageService.GetString("Para Birimini Kilitle");
            grbTable.Text = LanguageService.GetString("Tablo Kontrolleri");
            btnTotal.Text = LanguageService.GetString("Toplam Tutar'ı Genişlet");
            btnDisc.Text = LanguageService.GetString("Toplam İndirim'i Genişlet");
            btnTax.Text = LanguageService.GetString("VergiyiGenişlet");
            btnAll.Text = LanguageService.GetString("Bütün Tabloları Genişlet");

            this.Text = LanguageService.GetString("Fiyat Hesaplama - Sipariş Oluşturma");

            UpdateSelectedLanguage();
        }

        private void UpdateSelectedLanguage()
        {
            string currentLanguage = LanguageService.CurrentLanguage;

            türkçeToolStripMenuItem.Checked = currentLanguage == "Türkçe";
            englishToolStripMenuItem.Checked = currentLanguage == "English";

            türkçeToolStripMenuItem.BackColor = türkçeToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;
            englishToolStripMenuItem.BackColor = englishToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;
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

        private void FillComboBoxes()
        {
            cmbDisc.Items.Clear();
            cmbSCT.Items.Clear();
            cmbVAT.Items.Clear();
            cmbPayment.Items.Clear();
            cmbOrderStatus.Items.Clear();
            cmbCargo.Items.Clear();

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
            nmrDisc.Value = 0;
            nmrCargo.Value = 0;

            // İndirim (Discount)
            if (isTurkish)
            {
                cmbDisc.Items.Add("İndirim Yapılmıyor");
                for (int i = 5; i <= 50; i += 5)
                {
                    cmbDisc.Items.Add($"%{i} İndirim Yap");
                }
            }
            else
            {
                cmbDisc.Items.Add("No Discount");
                for (int i = 5; i <= 50; i += 5)
                {
                    cmbDisc.Items.Add($"Apply {i}% Discount");
                }
            }
            cmbDisc.SelectedIndex = 0;

            // ÖTV (SCT)
            if (isTurkish)
            {
                cmbSCT.Items.Add("ÖTV Yok");
            }
            else
            {
                cmbSCT.Items.Add("No Excise Tax");
            }

            int[] sctRates = { 20, 37, 45, 50, 60, 63, 67, 80, 100, 110, 150, 220 };
            foreach (var rate in sctRates)
            {
                if (isTurkish)
                    cmbSCT.Items.Add($"%{rate} ÖTV ekle");
                else
                    cmbSCT.Items.Add($"Add {rate}% SCT");
            }
            cmbSCT.SelectedIndex = 0;

            // KDV (VAT)
            if (isTurkish)
            {
                cmbVAT.Items.Add("KDV Yok");
            }
            else
            {
                cmbVAT.Items.Add("No VAT");
            }

            int[] vatRates = { 1, 8, 10, 18, 20 };
            foreach (var rate in vatRates)
            {
                if (isTurkish)
                    cmbVAT.Items.Add($"%{rate} KDV ekle");
                else
                    cmbVAT.Items.Add($"Add {rate}% VAT");
            }
            cmbVAT.SelectedIndex = 0;

            // Ödeme Durumu (Payment Status)
            if (isTurkish)
            {
                cmbPayment.Items.Add("Ödeme Alınmadı");
                cmbPayment.Items.Add("Ödeme Alındı");
            }
            else
            {
                cmbPayment.Items.Add("Payment Not Received");
                cmbPayment.Items.Add("Payment Received");
            }
            cmbPayment.SelectedIndex = 0;

            // Sipariş Durumu (Order Status)
            if (isTurkish)
            {
                cmbOrderStatus.Items.Add("Siparişe Başlanmadı");
                cmbOrderStatus.Items.Add("Hazırlanıyor");
                cmbOrderStatus.Items.Add("Kargoya Verilecek");
                cmbOrderStatus.Items.Add("Kargolandı");
                cmbOrderStatus.Items.Add("Bitti");
            }
            else
            {
                cmbOrderStatus.Items.Add("Not Started");
                cmbOrderStatus.Items.Add("In Preparation");
                cmbOrderStatus.Items.Add("To Be Shipped");
                cmbOrderStatus.Items.Add("Shipped");
                cmbOrderStatus.Items.Add("Completed");
            }

            cmbOrderStatus.SelectedIndex = 0;

            // Kargo (Cargo)
            cmbCargo.SelectedIndexChanged -= cmbCargo_SelectedIndexChanged; // Çift bağlanmayı önlemek için
            cmbCargo.SelectedIndexChanged += cmbCargo_SelectedIndexChanged;

            if (string.IsNullOrWhiteSpace(LoggedInUser.Cargo))
            {
                cmbCargo.Items.Add(isTurkish ? "Diğer" : "Other");
                cmbCargo.SelectedIndex = 0;
                txtCargo.Enabled = true;
            }
            else
            {
                cmbCargo.Items.Add(LoggedInUser.Cargo);
                cmbCargo.Items.Add(isTurkish ? "Diğer" : "Other");
                cmbCargo.SelectedIndex = 0;
                txtCargo.Enabled = false;
            }

            //lwTotal ve lwlDiscList içerisini temizleme
            lwTotal.Items.Clear();
            lwDiscList.Items.Clear();
            lwTax.Items.Clear();
        }

        // cmbCargo seçimi değiştiğinde çalışacak olay
        private void cmbCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //previousCargoIndex = cmbCargo.SelectedIndex;
            int statusIndex = cmbOrderStatus.SelectedIndex;

            if (statusIndex == 3 || statusIndex == 4)
            {
                txtCargo.Enabled = (cmbCargo.SelectedIndex == 1);
            }
            else
            {
                // Duruma göre enable ayarla, gerekirse
                if (cmbCargo.SelectedIndex == 0 && cmbCargo.Items[0].ToString() == LoggedInUser.Cargo)
                    txtCargo.Enabled = false;
                else
                    txtCargo.Enabled = true;
            }

            if (previousCargoIndex == 1 && cmbCargo.SelectedIndex == 0)
            {
                // Placeholder'ı doğrudan ata (SetPlaceholder yerine)
                bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
                txtCargo.Text = isTurkish ? "Diğer Kargo Şirketinin Adını Yazınız" : "Enter Other Cargo Company Name";
            }

            previousCargoIndex = cmbCargo.SelectedIndex;
        }

        private void UpdatePlaceholders()
        {
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            txtCargo.Text = "";
            txtCargoTracker.Text = "";
            txtCustomerName.Text = "";
            mskPhoneNo.Text = "";

            // txtCargo
            string cargoPlaceholder = isTurkish ? "Diğer Kargo Şirketinin Adını Yazınız" : "Enter Other Cargo Company Name";
            if (string.IsNullOrWhiteSpace(txtCargo.Text) ||
                txtCargo.Text == "Diğer Kargo Şirketinin Adını Yazınız" ||
                txtCargo.Text == "Enter Other Cargo Company Name")
            {
                txtCargo.Text = cargoPlaceholder;
            }

            // txtCargoTracker
            string trackerPlaceholder = isTurkish ? "Kargo Takip Numarasını Yazınız" : "Enter Cargo Tracking Number";
            if (string.IsNullOrWhiteSpace(txtCargoTracker.Text) ||
                txtCargoTracker.Text == "Kargo Takip Numarasını Yazınız" ||
                txtCargoTracker.Text == "Enter Cargo Tracking Number")
            {
                txtCargoTracker.Text = trackerPlaceholder;
            }

            // txtCustomerName
            string customerPlaceholder = isTurkish ? "Müşteri İsmini Yazınız" : "Enter Customer Name";
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                txtCustomerName.Text == "Müşteri İsmini Yazınız" ||
                txtCustomerName.Text == "Enter Customer Name")
            {
                txtCustomerName.Text = customerPlaceholder;
            }
        }

        private void SetPlaceholder(TextBox textBox, string trPlaceholder, string enPlaceholder, bool isLeave)
        {
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
            string placeholder = isTurkish ? trPlaceholder : enPlaceholder;

            if (isLeave)
            {
                // Burada placeholder'ı kesin olarak ata, text ne olursa olsun
                if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == placeholder)
                {
                    textBox.Text = placeholder;
                }
            }
            else
            {
                // Enter Event -> Placeholder ise temizle
                if (textBox.Text == placeholder)
                    textBox.Text = "";
            }
        }

        private void txtCargo_Enter(object sender, EventArgs e)
            => SetPlaceholder(txtCargo, "Diğer Kargo Şirketinin Adını Yazınız", "Enter Other Cargo Company Name", false);

        private void txtCargo_Leave(object sender, EventArgs e)
            => SetPlaceholder(txtCargo, "Diğer Kargo Şirketinin Adını Yazınız", "Enter Other Cargo Company Name", true);


        private void txtCargoTracker_Enter(object sender, EventArgs e)
            => SetPlaceholder(txtCargoTracker, "Kargo Takip Numarasını Yazınız", "Enter Cargo Tracking Number", false);

        private void txtCargoTracker_Leave(object sender, EventArgs e)
            => SetPlaceholder(txtCargoTracker, "Kargo Takip Numarasını Yazınız", "Enter Cargo Tracking Number", true);


        private void txtCustomerName_Enter(object sender, EventArgs e)
            => SetPlaceholder(txtCustomerName, "Müşteri İsmini Yazınız", "Enter Customer Name", false);

        private void txtCustomerName_Leave(object sender, EventArgs e)
            => SetPlaceholder(txtCustomerName, "Müşteri İsmini Yazınız", "Enter Customer Name", true);

        private void cmbPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPayment.SelectedIndex == 0) // 1. seçenek
            {
                // Otomatik seçimler (sadece item varsa uygula)
                if (cmbOrderStatus.Items.Count > 0)
                    cmbOrderStatus.SelectedIndex = 0; // 1. seçenek

                if (cmbCargo.Items.Count > 0)
                    cmbCargo.SelectedIndex = 0;       // 1. seçenek

                // TextBox ve MaskedTextBox’ları orijinal placeholderlara döndür
                UpdatePlaceholders();

                cmbOrderStatus.Enabled = false;
                cmbCargo.Enabled = false;
                txtCargo.Enabled = false;
                txtCargoTracker.Enabled = false;
                txtCustomerName.Enabled = false;
                mskPhoneNo.Enabled = false;
                btnAddOrder.Enabled = false;
            }
            else if (cmbPayment.SelectedIndex == 1) // 2. seçenek
            {
                cmbOrderStatus.Enabled = true;
                cmbCargo.Enabled = false;
                txtCargo.Enabled = false;
                txtCargoTracker.Enabled = false;
                txtCustomerName.Enabled = false;
                mskPhoneNo.Enabled = false;
                btnAddOrder.Enabled = false;

                // cmbOrderStatus durumu kontrol edilsin
                UpdateOrderStatusControls();
            }
        }

        private void cmbOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOrderStatusControls();

            // Ek kural: Eğer 1., 2. veya 3. seçenek seçildiyse
            if (cmbOrderStatus.SelectedIndex == 0 ||
                cmbOrderStatus.SelectedIndex == 1 ||
                cmbOrderStatus.SelectedIndex == 2)
            {
                if (cmbCargo.Items.Count > 0)
                    cmbCargo.SelectedIndex = 0;  // Kargo 1. seçenek
                UpdatePlaceholders();        // Cargo ve Tracker’ı sıfırla
            }
        }

        private void UpdateOrderStatusControls()
        {
            // Eğer cmbPayment 2. seçenek değilse burayı boş geçebiliriz
            if (cmbPayment.SelectedIndex != 1)
                return;

            int statusIndex = cmbOrderStatus.SelectedIndex;

            if (statusIndex == 0 || statusIndex == 1 || statusIndex == 2) // 1,2,3. seçenekler
            {
                cmbCargo.Enabled = false;
                txtCargo.Enabled = false;
                txtCargoTracker.Enabled = false;

                txtCustomerName.Enabled = true;
                mskPhoneNo.Enabled = true;
                btnAddOrder.Enabled = true;
            }
            else if (statusIndex == 3 || statusIndex == 4) // 4 ve 5. seçenekler
            {
                cmbCargo.Enabled = true;
                txtCargoTracker.Enabled = true;
                txtCustomerName.Enabled = true;
                mskPhoneNo.Enabled = true;
                btnAddOrder.Enabled = true;

                // cmbCargo 2. seçenek seçili ise txtCargo enable true, değilse false
                if (cmbCargo.SelectedIndex == 1) // 2. seçenek (index 1)
                {
                    txtCargo.Enabled = true;
                }
                else
                {
                    txtCargo.Enabled = false;
                }
            }
        }

        private void InitializeProductGrid() // 1. TABLOYA BAŞLIKLARIN YÜKLENMESİ
        {
            bool isEnglish = LanguageService.CurrentLanguage == "English";

            dgwProducts.Columns.Clear();

            dgwProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                HeaderText = isEnglish ? "Name" : "İsim",
                ReadOnly = true,
                Width = 112
            });

            dgwProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Cost",
                HeaderText = isEnglish ? "Cost" : "Maliyet",
                ReadOnly = true,
                Width = 80
            });

            dgwProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "CostCurrency",
                HeaderText = isEnglish ? "Cost Currency" : "Maliyet Para Birimi",
                ReadOnly = true,
                Width = 115
            });

            dgwProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "StockQuantity",
                HeaderText = isEnglish ? "Stock Quantity" : "Stok Adeti",
                ReadOnly = true,
                Width = 80
            });

            dgwProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Price",
                HeaderText = isEnglish ? "Price" : "Fiyat",
                ReadOnly = chbLockPrice.Checked, // checkbox durumuna göre
                Width = 80
            });

            // Para birimleri listesi
            var currencies = new List<string>
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

            // Alfabetik sırala
            currencies.Sort();

            // Türk Lirası en başa ekle
            currencies.Insert(0, "Türk Lirası");

            // === ComboBoxColumn oluştur ===
            var currencyColumn = new DataGridViewComboBoxColumn()
            {
                Name = "PriceCurrency",
                HeaderText = isEnglish ? "Price Currency" : "Fiyat Para Birimi",
                ReadOnly = chbLockForex.Checked,
                Width = 150,
                DropDownWidth = 180,
                FlatStyle = FlatStyle.Flat

            };

            currencyColumn.Items.AddRange(currencies.ToArray());

            dgwProducts.Columns.Add(currencyColumn);

            dgwProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "OrderQuantity",
                HeaderText = isEnglish ? "Order Quantity" : "Sipariş Adeti",
                ReadOnly = false, // Kullanıcı girebilsin diye false
                Width = 94
            });
        }

        private void SetModernHeaderStyle(DataGridView dgv) // BAŞLIKLARIN DAHA MODERN GÖRÜNÜMDE OLMASI İÇİN KULLANILAN METOD
        {
            var headerStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(245, 245, 245),   // Çok açık gri (neredeyse beyaz)
                ForeColor = Color.FromArgb(50, 50, 50),     // Koyu gri (siyah değil)
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                WrapMode = DataGridViewTriState.False
            };

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle = headerStyle;

            dgv.ColumnHeadersHeight = 30;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            // İnce gri sınırlar
            dgv.GridColor = Color.FromArgb(220, 220, 220);

            // Satır seçiminde açık renk vurgusu
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 201); // Açık yeşil ton
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Satır arka planı beyaz
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250); // Çok hafif gri alternance
        }

        private void chbLockPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (dgwProducts.Columns.Contains("Price"))
            {
                dgwProducts.Columns["Price"].ReadOnly = chbLockPrice.Checked;
            }

            ApplyColumnLockStyles();
        }

        private void chbLockForex_CheckedChanged(object sender, EventArgs e)
        {
            if (dgwProducts.Columns.Contains("PriceCurrency"))
            {
                dgwProducts.Columns["PriceCurrency"].ReadOnly = chbLockForex.Checked;
            }

            ApplyColumnLockStyles();
        }

        private void LoadUserProducts() // GİRİŞ YAPAN KULLANICININ ÜRÜNLERİN YÜKLENMESİ
        {
            var products = _productService.GetProductsByLoggedInUser();
            dgwProducts.Rows.Clear(); // Önce tabloyu temizle

            foreach (var product in products)
            {
                dgwProducts.Rows.Add(
                    product.Name,                // 1. sütun: Ürün adı
                    product.Cost,                // 2. sütun: Maliyet
                    product.CostCurrency,        // 3. sütun: Stok
                    product.Stock,               // 4. sütun: Maliyet para birimi
                    product.Price,               // 5. sütun: Satış fiyatı
                    product.PriceCurrency,       // 6. sütun: Satış para birimi
                    0                            // Sipariş adeti başlangıçta 0
                );
            }
        }

        private void dgwProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (editingControl != null)
            {
                editingControl.TextChanged -= ProductCell_TextChanged;
                editingControl.Leave -= EditingControl_Leave;
                editingControl.KeyPress -= NumericTextBox_KeyPress;
                editingControl = null;
            }

            if (currencyComboBox != null)
            {
                currencyComboBox.SelectedIndexChanged -= CurrencyComboBox_SelectedIndexChanged;
                currencyComboBox = null;
            }

            if (e.Control is TextBox tb && dgwProducts.CurrentCell != null)
            {
                string colName = dgwProducts.CurrentCell.OwningColumn.Name;

                if (colName == "Price" || colName == "OrderQuantity" || colName == "PriceCurrency")
                {
                    if (colName == "Price" || colName == "OrderQuantity")
                    {
                        tb.KeyPress -= NumericTextBox_KeyPress;
                        tb.KeyPress += NumericTextBox_KeyPress;
                    }

                    editingControl = tb;
                    editingControl.TextChanged += ProductCell_TextChanged;
                    editingControl.Leave += EditingControl_Leave;
                }
            }
            else if (e.Control is ComboBox combo && dgwProducts.CurrentCell != null)
            {
                string colName = dgwProducts.CurrentCell.OwningColumn.Name;

                if (colName == "PriceCurrency")
                {
                    currencyComboBox = combo;
                    currencyComboBox.SelectedIndexChanged += CurrencyComboBox_SelectedIndexChanged;
                }
            }
        }

        private void ProductCell_TextChanged(object sender, EventArgs e)
        {
            if (dgwProducts.CurrentCell == null) return;

            string colName = dgwProducts.CurrentCell.OwningColumn.Name;
            if (colName != "Price" && colName != "OrderQuantity") return; // PriceCurrency'i buradan çıkardık

            var tb = sender as TextBox;
            int rowIndex = dgwProducts.CurrentCell.RowIndex;
            var row = dgwProducts.Rows[rowIndex];
            string productName = row.Cells["Name"].Value?.ToString() ?? "";

            decimal price = ParseDecimalFlexible(row.Cells["Price"].Value?.ToString());
            int qty = 0;
            string currency = row.Cells["PriceCurrency"].Value?.ToString() ?? "";

            if (colName == "Price")
            {
                price = ParseDecimalFlexible(tb.Text);
                int.TryParse(row.Cells["OrderQuantity"].Value?.ToString(), out qty);
            }
            else if (colName == "OrderQuantity")
            {
                int.TryParse(tb.Text, out qty);
                price = ParseDecimalFlexible(row.Cells["Price"].Value?.ToString());
            }

            var existingRow = dgwOrderDetails.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => r.Cells["Name"].Value?.ToString() == productName);

            if (qty > 0)
            {
                decimal total = price * qty;
                if (existingRow != null)
                {
                    existingRow.Cells["Price"].Value = price;
                    existingRow.Cells["PriceCurrency"].Value = currency;
                    existingRow.Cells["OrderQuantity"].Value = qty;
                    existingRow.Cells["Total"].Value = total;
                }
                else
                {
                    dgwOrderDetails.Rows.Add(productName, price, currency, qty, total);
                }
            }
            else
            {
                if (existingRow != null)
                    dgwOrderDetails.Rows.Remove(existingRow);
            }
        }

        // Düzenleme kontrolü ayrılırken (başka yere tıklayınca) boşsa 0 yaz ve commit et
        private void EditingControl_Leave(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null || dgwProducts.CurrentCell == null) return;

            string colName = dgwProducts.CurrentCell.OwningColumn.Name;

            if (colName == "OrderQuantity" && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "0"; // bu TextChanged'i tetikleyecek ve orderDetails güncellenecek
            }

            // hücreye commit et
            dgwProducts.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        // CellEndEdit: final commit sonrası kesin sync (yedek)
        private void dgwProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var colName = dgwProducts.Columns[e.ColumnIndex].Name;
            if (colName == "OrderQuantity")
            {
                var cell = dgwProducts.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    cell.Value = 0;
            }

            if (dgwProducts.Columns[e.ColumnIndex].Name == "OrderQuantity") // EK ADIM BABACIM
            {
                UpdateMessage(); // Mesajı güncelle
            }
        }

        // esnek decimal parse (kültüre göre deneyip fallback yapar)
        private decimal ParseDecimalFlexible(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;

            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out var v)) return v;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v)) return v;

            // nokta/virgül dönüşümleri deneyelim
            var replaced = s.Replace('.', ',');
            if (decimal.TryParse(replaced, NumberStyles.Any, CultureInfo.CurrentCulture, out v)) return v;
            replaced = s.Replace(',', '.');
            if (decimal.TryParse(replaced, NumberStyles.Any, CultureInfo.InvariantCulture, out v)) return v;

            return 0m;
        }

        private void OrderQuantity_TextChanged(object sender, EventArgs e)
        {
            if (dgwProducts.CurrentCell == null) return;
            if (dgwProducts.CurrentCell.OwningColumn.Name != "OrderQuantity") return;

            var row = dgwProducts.Rows[dgwProducts.CurrentCell.RowIndex];

            string productName = row.Cells["Name"].Value?.ToString();
            string priceCurrency = row.Cells["PriceCurrency"].Value?.ToString();

            decimal.TryParse(row.Cells["Price"].Value?.ToString(), out decimal price);
            int.TryParse(((TextBox)sender).Text, out int orderQty);

            if (orderQty > 0)
            {
                decimal total = price * orderQty;

                var existingRow = dgwOrderDetails.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => r.Cells["Name"].Value?.ToString() == productName);

                if (existingRow != null)
                {
                    existingRow.Cells["Price"].Value = price;
                    existingRow.Cells["PriceCurrency"].Value = priceCurrency;
                    existingRow.Cells["OrderQuantity"].Value = orderQty;
                    existingRow.Cells["Total"].Value = total;
                }
                else
                {
                    dgwOrderDetails.Rows.Add(productName, price, priceCurrency, orderQty, total);
                }
            }
            else
            {
                var existingRow = dgwOrderDetails.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => r.Cells["Name"].Value?.ToString() == productName);

                if (existingRow != null)
                {
                    dgwOrderDetails.Rows.Remove(existingRow);
                }
            }
        }

        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Rakam, virgül, nokta ve kontrol tuşları dışında engelle
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            // Virgül veya nokta sadece bir kere olabilir
            if ((e.KeyChar == ',' || e.KeyChar == '.') && (tb.Text.Contains(',') || tb.Text.Contains('.')))
            {
                e.Handled = true;
                return;
            }

            // Virgülden sonra maksimum 2 hane
            if ((tb.Text.Contains(',') || tb.Text.Contains('.')) && char.IsDigit(e.KeyChar))
            {
                int separatorIndex = tb.Text.IndexOf(',') >= 0 ? tb.Text.IndexOf(',') : tb.Text.IndexOf('.');
                if (tb.SelectionStart > separatorIndex)
                {
                    string afterSeparator = tb.Text.Substring(separatorIndex + 1);
                    if (afterSeparator.Length >= 2 && tb.SelectionLength == 0)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void ApplyColumnLockStyles() // TİKLANMIŞ SÜTUNLARIN RENKLERİNİN UYGULANMASI
        {
            foreach (DataGridViewColumn column in dgwProducts.Columns)
            {
                // Önce tüm sütunları varsayılan renge döndür
                column.DefaultCellStyle.BackColor = Color.White;
                column.DefaultCellStyle.ForeColor = Color.Black;
            }

            // Price sütunu kilitliyse açık gri renge boya
            if (chbLockPrice.Checked && dgwProducts.Columns.Contains("Price"))
            {
                var priceColumn = dgwProducts.Columns["Price"];
                priceColumn.ReadOnly = true;
                priceColumn.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // Açık gri
                priceColumn.DefaultCellStyle.ForeColor = Color.FromArgb(100, 100, 100); // Koyu gri
            }
            else if (dgwProducts.Columns.Contains("Price"))
            {
                dgwProducts.Columns["Price"].ReadOnly = false;
            }

            // PriceCurrency sütunu kilitliyse açık gri renge boya
            if (chbLockForex.Checked && dgwProducts.Columns.Contains("PriceCurrency"))
            {
                var currencyColumn = dgwProducts.Columns["PriceCurrency"];
                currencyColumn.ReadOnly = true;
                currencyColumn.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // Açık gri
                currencyColumn.DefaultCellStyle.ForeColor = Color.FromArgb(100, 100, 100); // Koyu gri
            }
            else if (dgwProducts.Columns.Contains("PriceCurrency"))
            {
                dgwProducts.Columns["PriceCurrency"].ReadOnly = false;
            }
        }

        private void InitializeOrderDetailsGrid() // 2. SİPARİŞ DETAYLARI TABLOSUNA BAŞLIKLARIN YÜKLENMESİ
        {
            bool isEnglish = LanguageService.CurrentLanguage == "English";

            dgwOrderDetails.Columns.Clear();

            dgwOrderDetails.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                HeaderText = isEnglish ? "Name" : "İsmi",
                ReadOnly = true,
                Width = 80
            });

            dgwOrderDetails.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Price",
                HeaderText = isEnglish ? "Price" : "Fiyat",
                ReadOnly = true,
                Width = 80
            });

            dgwOrderDetails.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "PriceCurrency",
                HeaderText = isEnglish ? "Price Currency" : "Fiyat Para Birimi",
                ReadOnly = true,
                Width = 115
            });

            dgwOrderDetails.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "OrderQuantity",
                HeaderText = isEnglish ? "Order Quantity" : "Sipariş Adeti",
                ReadOnly = true, // Sipariş detay tablosunda kullanıcı girmesin
                Width = 80
            });

            dgwOrderDetails.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Total",
                HeaderText = isEnglish ? "Total" : "Tutar",
                ReadOnly = true,
                Width = 80
            });

        }

        private void dgwProducts_KeyUp(object sender, KeyEventArgs e)
        {
            if (dgwProducts.CurrentCell == null) return;

            string colName = dgwProducts.CurrentCell.OwningColumn.Name;
            int rowIndex = dgwProducts.CurrentCell.RowIndex;

            // Sadece PriceCurrency sütununda ve bir değer girildiğinde
            if (colName == "PriceCurrency" && rowIndex >= 0)
            {
                var row = dgwProducts.Rows[rowIndex];
                string productName = row.Cells["Name"].Value?.ToString();
                string newCurrency = row.Cells["PriceCurrency"].Value?.ToString();

                if (!string.IsNullOrEmpty(productName) && !string.IsNullOrEmpty(newCurrency))
                {
                    // OrderDetails tablosunda ilgili ürünü bul ve güncelle
                    foreach (DataGridViewRow detailRow in dgwOrderDetails.Rows)
                    {
                        if (detailRow.Cells["Name"].Value?.ToString() == productName)
                        {
                            detailRow.Cells["PriceCurrency"].Value = newCurrency;
                            break;
                        }
                    }
                }
            }
        }

        private void dgwProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e) // ATAMA
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = dgwProducts.Columns[e.ColumnIndex].Name;

            // Sadece PriceCurrency sütunu için işlem yap
            if (colName == "PriceCurrency")
            {
                var row = dgwProducts.Rows[e.RowIndex];
                string productName = row.Cells["Name"].Value?.ToString();
                string newCurrency = row.Cells["PriceCurrency"].Value?.ToString();
                string oldCurrency = "";

                // Eski para birimini al
                var existingRow = dgwOrderDetails.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => r.Cells["Name"].Value?.ToString() == productName);

                if (existingRow != null)
                {
                    oldCurrency = existingRow.Cells["PriceCurrency"].Value?.ToString();
                }

                // Eğer para birimi gerçekten değiştiyse ve eski bir değer varsa onay iste
                if (!string.IsNullOrEmpty(oldCurrency) && newCurrency != oldCurrency)
                {
                    bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
                    string message = isTurkish ?
                        $"Para birimini {oldCurrency} yerine {newCurrency} olarak değiştirmek istediğinize emin misiniz?" :
                        $"Are you sure you want to change the currency from {oldCurrency} to {newCurrency}?";

                    string title = isTurkish ? "Para Birimi Değişikliği" : "Currency Change";

                    var result = MessageBox.Show(message, title,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        // Değişikliği geri al
                        dgwProducts.Rows[e.RowIndex].Cells["PriceCurrency"].Value = oldCurrency;
                        return;
                    }
                }

                // OrderDetails tablosunda ilgili ürünü bul ve güncelle
                if (!string.IsNullOrEmpty(productName))
                {
                    foreach (DataGridViewRow detailRow in dgwOrderDetails.Rows)
                    {
                        if (detailRow.Cells["Name"].Value?.ToString() == productName)
                        {
                            detailRow.Cells["PriceCurrency"].Value = newCurrency;
                            break;
                        }
                    }
                }
            }
        }

        private void CurrencyComboBox_SelectedIndexChanged(object sender, EventArgs e) // PARA BİRİMİ DEĞİŞTİRME İŞLEMİ ONAY EKRANI
        {
            if (dgwProducts.CurrentCell == null || currencyComboBox == null) return;

            int rowIndex = dgwProducts.CurrentCell.RowIndex;
            var row = dgwProducts.Rows[rowIndex];
            string productName = row.Cells["Name"].Value?.ToString();
            string newCurrency = currencyComboBox.SelectedItem?.ToString();

            // Eski para birimini al (eğer varsa)
            string oldCurrency = "";
            var existingRow = dgwOrderDetails.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => r.Cells["Name"].Value?.ToString() == productName);

            if (existingRow != null)
            {
                oldCurrency = existingRow.Cells["PriceCurrency"].Value?.ToString();
            }

            // Eğer para birimi gerçekten değiştiyse ve eski bir değer varsa onay iste
            if (!string.IsNullOrEmpty(oldCurrency))
            {
                bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
                string message = isTurkish ?
                    $"Para birimini {oldCurrency} yerine {newCurrency} olarak değiştirmek istediğinize emin misiniz?" :
                    $"Are you sure you want to change the currency from {oldCurrency} to {newCurrency}?";

                string title = isTurkish ? "Para Birimi Değişikliği" : "Currency Change";

                var result = MessageBox.Show(message, title,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    // Değişikliği geri al
                    currencyComboBox.SelectedItem = oldCurrency;
                    return;
                }
            }

            // OrderDetails tablosunda ilgili ürünü bul ve güncelle
            if (!string.IsNullOrEmpty(productName) && !string.IsNullOrEmpty(newCurrency))
            {
                foreach (DataGridViewRow detailRow in dgwOrderDetails.Rows)
                {
                    if (detailRow.Cells["Name"].Value?.ToString() == productName)
                    {
                        detailRow.Cells["PriceCurrency"].Value = newCurrency;
                        break;
                    }
                }
            }
        }

        private void InitializeLwDisc() // List View Sütun Başlıklarının Yüklenmesi
        {
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            lwDisc.CheckBoxes = true;
            lwDisc.View = View.Details;
            lwDisc.FullRowSelect = true;

            lwDisc.Columns.Clear();

            lwDisc.Columns.Add("", 30); // Checkbox boş alan

            if (isTurkish)
            {
                lwDisc.Columns.Add("Ürün Adı", 90);
                lwDisc.Columns.Add("Tutar", 70);
                lwDisc.Columns.Add("Para Birimi", 100);
                lwDisc.Columns.Add("İndirim Durumu", 115);
                lwDisc.Columns.Add("İndirim", 70);
                lwDisc.Columns.Add("ÖTV Durumu", 100);
                lwDisc.Columns.Add("KDV Durumu", 100);
            }
            else
            {
                lwDisc.Columns.Add("Product Name", 90);
                lwDisc.Columns.Add("Amount", 80);
                lwDisc.Columns.Add("Currency", 90);
                lwDisc.Columns.Add("Discount Status", 115);
                lwDisc.Columns.Add("Discount", 70);
                lwDisc.Columns.Add("SCT Status", 100);
                lwDisc.Columns.Add("VAT Status", 100);
            }
        }

        private void lwDisc_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true; // Kullanıcının genişlik değiştirmesini iptal et
            e.NewWidth = lwDisc.Columns[e.ColumnIndex].Width; // Mevcut genişliği koru
        }

        private void dgwOrderDetails_Changed(object sender, EventArgs e)
        {
            UpdateListViewFromDGV();
            UpdateTotalsFromDisc(); // ListView'den toplamları güncelle
            UpdateDiscsFromDisc();
            ResetTax();
            UpdateMessage();
        }

        private void UpdateListViewFromDGV()
        {
            lwDisc.Items.Clear();

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            foreach (DataGridViewRow row in dgwOrderDetails.Rows)
            {
                if (row.IsNewRow) continue;

                string urunAdi = row.Cells["Name"].Value?.ToString();
                string tutarStr = row.Cells["Total"].Value?.ToString();
                string paraBirimi = row.Cells["PriceCurrency"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(urunAdi)) continue;

                if (!TryParseDecimalFlexible(tutarStr, out decimal tutar))
                    tutar = 0;

                var item = new ListViewItem("");                 // 0: (checkbox alanı)
                item.SubItems.Add(urunAdi);                      // 1: Ürün Adı
                item.SubItems.Add(tutar.ToString("F2"));         // 2: Tutar (başlangıçta indirimsiz göster)
                item.SubItems.Add(paraBirimi);                   // 3: Para Birimi

                if (isTurkish)
                {
                    item.SubItems.Add("İndirim Yapılmıyor");     // 4: İndirim Durumu No Discount
                    item.SubItems.Add("0,00");                   // 5: İndirim Miktarı
                    item.SubItems.Add("ÖTV Yok");                // 6: ÖTV No Excise Tax
                    item.SubItems.Add("KDV Yok");                // 7: KDV No VAT
                }
                else
                {
                    item.SubItems.Add("No Discount");            // 4: İndirim Durumu 
                    item.SubItems.Add("0.00");                   // 5: İndirim Miktarı
                    item.SubItems.Add("No Excise Tax");          // 6: ÖTV No Excise Tax
                    item.SubItems.Add("No VAT");                 // 7: KDV No VAT
                }

                // Orijinal (indirimsiz) tutarı sakla
                item.Tag = tutar;

                lwDisc.Items.Add(item);
            }
        }

        private async Task ApplyDiscountAndTaxes()
        {
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // --- Vergi oranlarını al ---
            decimal sctRate = 0;
            if (cmbSCT.SelectedIndex > 0 && cmbSCT.SelectedItem != null)
            {
                string txt = cmbSCT.SelectedItem.ToString();
                if (isTurkish && txt.StartsWith("%"))
                    decimal.TryParse(txt.Split(' ')[0].Replace("%", ""), out sctRate);
                else
                    foreach (var part in txt.Split(' '))
                        if (part.Contains("%"))
                            decimal.TryParse(part.Replace("%", ""), out sctRate);
            }

            decimal vatRate = 0;
            if (cmbVAT.SelectedIndex > 0 && cmbVAT.SelectedItem != null)
            {
                string txt = cmbVAT.SelectedItem.ToString();
                if (isTurkish && txt.StartsWith("%"))
                    decimal.TryParse(txt.Split(' ')[0].Replace("%", ""), out vatRate);
                else
                    foreach (var part in txt.Split(' '))
                        if (part.Contains("%"))
                            decimal.TryParse(part.Replace("%", ""), out vatRate);
            }

            decimal yuzdeIndirim = GetSelectedDiscountPercent();
            decimal ekIndirim = nmrDisc.Value;

            bool needsExchange = lwDisc.Items
                .Cast<ListViewItem>()
                .Any(it =>
                {
                    if (!it.Checked) return false;
                    string currency = it.SubItems[COL_CURRENCY].Text;
                    return !IsTL(currency);
                });

            Dictionary<string, (decimal BuyRate, decimal SellRate)> exchangeRates = null;
            if (needsExchange)
            {
                try
                {
                    var forexService = new FREEMARKETforex();
                    exchangeRates = await forexService.GetExchangeRatesAsync();
                }
                catch { }
            }

            decimal totalTaxAmount_TL = 0m;

            foreach (ListViewItem item in lwDisc.Items)
            {
                if (!item.Checked) continue;

                decimal orijinalTutar = item.Tag is decimal d ? d : 0m;
                string currency = item.SubItems[COL_CURRENCY].Text;
                bool isForeignCurrency = !IsTL(currency);

                // 1) İndirim
                decimal yuzdeselIndirimMiktar = (orijinalTutar * yuzdeIndirim) / 100m;
                decimal toplamIndirim = yuzdeselIndirimMiktar + ekIndirim;
                decimal tutarIndirimli = Math.Max(orijinalTutar - toplamIndirim, 0);

                // 2) Vergiler
                decimal otvTutar = (tutarIndirimli * sctRate) / 100m;
                decimal tutarOtv = tutarIndirimli + otvTutar;
                decimal kdvTutar = (tutarOtv * vatRate) / 100m;
                decimal tutarSon = tutarOtv + kdvTutar;

                decimal sctInTL = 0m;
                decimal vatInTL = 0m;

                if (!isForeignCurrency)
                {
                    sctInTL = otvTutar;
                    vatInTL = kdvTutar;
                }
                else if (exchangeRates != null)
                {
                    var rateKey = exchangeRates.Keys.FirstOrDefault(k =>
                        k.Contains(currency, StringComparison.OrdinalIgnoreCase));
                    if (rateKey != null)
                    {
                        decimal exchangeRate = exchangeRates[rateKey].SellRate;
                        sctInTL = otvTutar * exchangeRate;
                        vatInTL = kdvTutar * exchangeRate;
                    }
                }

                totalTaxAmount_TL += sctInTL + vatInTL;

                // --- ListView Güncelle ---
                item.SubItems[COL_TOTAL].Text = tutarSon.ToString("F2");
                item.SubItems[COL_DISC_AMOUNT].Text = toplamIndirim.ToString("F2");
                item.SubItems[COL_SCT].Text = sctRate > 0 ? $"%{sctRate} ({sctInTL:F2} TL)" : (isTurkish ? "ÖTV Yok" : "No Excise Tax");
                item.SubItems[COL_VAT].Text = vatRate > 0 ? $"%{vatRate} ({vatInTL:F2} TL)" : (isTurkish ? "KDV Yok" : "No VAT");

                string discStatus;
                if (yuzdeIndirim > 0 && ekIndirim > 0)
                    discStatus = $"%{yuzdeIndirim} (+{ekIndirim:F2})";
                else if (yuzdeIndirim > 0)
                    discStatus = $"%{yuzdeIndirim}";
                else if (ekIndirim > 0)
                    discStatus = $"+{ekIndirim:F2}";
                else
                    discStatus = isTurkish ? "İndirim Yok" : "No Discount";

                item.SubItems[COL_DISC_STATUS].Text = discStatus;

                UpdateMessage();
            }

            // --- lwTax güncelle ---
            lwTax.Items.Clear();
            var row = new ListViewItem("Türk Lirası");
            row.SubItems.Add(totalTaxAmount_TL.ToString("F2", new System.Globalization.CultureInfo("tr-TR")));
            lwTax.Items.Add(row);

            UpdateTotalsFromDisc();
            UpdateDiscsFromDisc();
        }

        private bool IsTL(string currency)
        {
            return string.IsNullOrWhiteSpace(currency) ||
                   currency.Equals("Türk Lirası", StringComparison.OrdinalIgnoreCase) ||
                   currency.Equals("TRY", StringComparison.OrdinalIgnoreCase) ||
                   currency.Equals("₺");
        }


        private void cmbDisc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyDiscountAndTaxes();
        }

        private void nmrDisc_ValueChanged(object sender, EventArgs e)
        {
            ApplyDiscountAndTaxes();
        }

        private void nmrDisc_KeyUp(object sender, KeyEventArgs e)
        {
            ApplyDiscountAndTaxes();
        }

        private void cmbSCT_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyDiscountAndTaxes();

        }

        private void cmbVAT_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyDiscountAndTaxes();

        }

        private void nmrCargo_ValueChanged(object sender, EventArgs e)
        {
            ApplyDiscountAndTaxes();
        }

        private void nmrCargo_KeyUp(object sender, KeyEventArgs e)
        {
            ApplyDiscountAndTaxes();
        }

        private decimal GetSelectedDiscountPercent()
        {
            if (cmbDisc?.SelectedItem == null) return 0m;

            string txt = cmbDisc.SelectedItem.ToString().Trim();
            bool isTR = LanguageService.CurrentLanguage == "Türkçe";

            // "İndirim Yapılmıyor" / "No Discount"
            if (isTR ? txt.StartsWith("İndirim", StringComparison.OrdinalIgnoreCase)
                     : txt.StartsWith("No", StringComparison.OrdinalIgnoreCase))
                return 0m;

            // TR: "%10 İndirim Yap"
            if (isTR && txt.StartsWith("%"))
            {
                string num = txt.Split(' ')[0].Replace("%", "");
                if (decimal.TryParse(num, NumberStyles.Number, CultureInfo.InvariantCulture, out var p))
                    return p;
            }

            // EN: "Apply 10% Discount"
            foreach (var part in txt.Split(' '))
            {
                if (part.Contains("%"))
                {
                    string num = part.Replace("%", "");
                    if (decimal.TryParse(num, NumberStyles.Number, CultureInfo.InvariantCulture, out var p))
                        return p;
                }
            }

            return 0m;
        }

        private bool TryParseDecimalFlexible(string s, out decimal value)
        {
            value = 0m;
            if (string.IsNullOrWhiteSpace(s)) return false;

            s = s.Trim();

            // Yaygın para işaretlerini temizle
            s = s.Replace("₺", "").Replace("TL", "")
                 .Replace("$", "").Replace("€", "")
                 .Replace("£", "");

            // Önce mevcut kültür
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
                return true;

            // TR, EN, Invariant dene
            if (decimal.TryParse(s, NumberStyles.Any, new CultureInfo("tr-TR"), out value)) return true;
            if (decimal.TryParse(s, NumberStyles.Any, new CultureInfo("en-US"), out value)) return true;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out value)) return true;

            return false;
        }

        private void lwDisc_ItemCheck(object sender, ItemCheckEventArgs e) // CHECKLERE GÖRE İNDİRİM VE VERGİLERE ULAŞABİLME
        {
            this.BeginInvoke(new Action(() => UpdateControlsAccessibility()));
        }

        private void lwDisc_ItemChecked(object sender, ItemCheckedEventArgs e) // CHECKLERE GÖRE İNDİRİM VE VERGİLERE ULAŞABİLME
        {
            UpdateControlsAccessibility();
        }

        private void UpdateControlsAccessibility() // CHECKLERE GÖRE İNDİRİM VE VERGİLERE ULAŞABİLME
        {
            bool anyItemChecked = false;

            foreach (ListViewItem item in lwDisc.Items)
            {
                if (item.Checked)
                {
                    anyItemChecked = true;
                    break;
                }
            }

            // Kontrollerin erişilebilirliğini ayarla
            cmbDisc.Enabled = anyItemChecked;
            nmrDisc.Enabled = anyItemChecked;
            cmbSCT.Enabled = anyItemChecked;
            cmbVAT.Enabled = anyItemChecked;

            // Eğer hiçbir öğe seçili değilse, kontrolleri sıfırla
            if (!anyItemChecked)
            {
                cmbDisc.SelectedIndex = 0;
                nmrDisc.Value = 0;
                cmbSCT.SelectedIndex = 0;
                cmbVAT.SelectedIndex = 0;
            }

            // Eğer hiç check yoksa sıfırla
            if (!anyItemChecked)
            {
                ResetDiscountControls();  // Aynı sıfırlama işlemi
            }
        }

        // Kontrolleri kapatıp sıfırlayan metod
        private void ResetDiscountControls()
        {
            cmbDisc.Enabled = false;
            nmrDisc.Enabled = false;
            cmbSCT.Enabled = false;
            cmbVAT.Enabled = false;

            // Varsayılan değerler
            cmbDisc.SelectedIndex = 0;
            nmrDisc.Value = 0;
            cmbSCT.SelectedIndex = 0;
            cmbVAT.SelectedIndex = 0;
        }

        private void pbScale_Click(object sender, EventArgs e)
        {
            if (isExpanded)
            {
                // Daralt
                grbCustomerMsg.Height = 38;
                this.Height = 555;

                // Ok yönünü eski haline döndür
                pbScale.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                pbScale.Refresh();

                isExpanded = false;
            }
            else
            {
                // Genişlet
                grbCustomerMsg.Height = originalHeight;
                this.Height = 722;

                // Oku ters çevir
                pbScale.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                pbScale.Refresh();

                isExpanded = true;
            }
        }

        private void UpdateMessage()
        {
            // lwTotal'dan para birimi ve toplamları al
            Dictionary<string, decimal> currencyTotals = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            foreach (ListViewItem item in lwTotal.Items)
            {
                string currency = item.SubItems[0].Text.Trim(); // 1. kolon: Para Birimi
                if (string.IsNullOrWhiteSpace(currency))
                    currency = türkçeToolStripMenuItem.Checked ? "TL" : "TRY";

                if (decimal.TryParse(item.SubItems[1].Text, out decimal val)) // 2. kolon: Toplam
                {
                    currencyTotals[currency] = val;
                }
            }

            string message;

            if (türkçeToolStripMenuItem.Checked)
            {
                // Türkçe mesaj
                string totals = currencyTotals.Count == 0
                    ? "Toplam tutar: 0 TL"
                    : "Toplam Sipariş Bedeli:\n" + string.Join("\n", currencyTotals.Select(kvp => $"- {kvp.Value:F2} {kvp.Key.ToUpper()}"));

                message = $"Sayın Müşterimiz,\n" +
                          $"Siparişinizi tamamlamadan önce, fiyat bilgisi aşağıda yer almaktadır:\n" +
                          $"{totals}\n" +
                          $"Farklı bir isteğiniz yoksa, ürünler {LoggedInUser.Cargo} ile gönderilecek olup, kargo ücreti alıcıya ait olacaktır.\n" +
                          $"Sipariş süreciyle ilgili her türlü sorunuz için tekrardan bizimle iletişime geçebilirsiniz.";
            }
            else
            {
                // İngilizce mesaj
                string totals = currencyTotals.Count == 0
                    ? "Total amount: 0 TRY"
                    : "Total Order Amount:\n" + string.Join("\n", currencyTotals.Select(kvp => $"- {kvp.Value:F2} {kvp.Key.ToUpper()}"));

                message = $"Dear Customer,\n" +
                          $"Before completing your order, please find the price information below:\n" +
                          $"{totals}\n" +
                          $"Unless you have any other requests, your items will be shipped via {LoggedInUser.Cargo}, and shipping cost will be borne by the recipient.\n" +
                          $"If you have any questions regarding your order, please contact us again.";
            }

            rchMessage.Text = message;
        }

        private void MakeRichTextBoxUnselectable(RichTextBox rtb) // RichTextBox içeriğinin seçilmesini engelleme
        {
            rtb.ReadOnly = true;  // Düzenlemeyi engeller
            rtb.TabStop = false;  // Tab ile seçilemeyi engeller

            // Seçimi engellemek için olaylar
            rtb.MouseDown += (s, e) => { e = null; }; // Mouse tıklamalarını yok say
            rtb.MouseMove += (s, e) => { rtb.DeselectAll(); }; // Seçimi sürekli kaldır
            rtb.Enter += (s, e) => { this.ActiveControl = null; }; // Focus olmasını engelle
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            string question = isTurkish
                ? "Müşteri mesajını kopyalama işlemini onaylıyor musunuz?"
                : "Do you confirm copying the customer message?";

            DialogResult result = MessageBox.Show(
                question,
                isTurkish ? "Onay" : "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                if (!string.IsNullOrWhiteSpace(rchMessage.Text))
                {
                    Clipboard.SetText(rchMessage.Text);
                    MessageBox.Show(
                        isTurkish ? "Mesaj panoya kopyalandı." : "Message copied to clipboard.",
                        isTurkish ? "Bilgi" : "Info",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        isTurkish ? "Kopyalanacak bir mesaj yok." : "No message to copy.",
                        isTurkish ? "Uyarı" : "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            string question;
            string caption;

            if (türkçeToolStripMenuItem.Checked)
            {
                question = "Panodaki kopyalanmış içeriği silmek istediğinize emin misiniz?";
                caption = "Onay";
            }
            else
            {
                question = "Are you sure you want to clear the copied content from the clipboard?";
                caption = "Confirmation";
            }

            DialogResult result = MessageBox.Show(
                question,
                caption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Clipboard.Clear(); // Panoyu temizler

                if (türkçeToolStripMenuItem.Checked)
                    MessageBox.Show("Panodaki içerik başarıyla temizlendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Clipboard content has been successfully cleared.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void isWhatsApp()
        {
            // Local AppData yolunu kontrol et
            string localPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"WhatsApp\WhatsApp.exe"
            );

            if (File.Exists(localPath))
            {
                btnWhatsapp.Tag = "local"; // WhatsApp mevcut
                btnWhatsapp.Enabled = true;
            }
            else
            {
                btnWhatsapp.Tag = "store"; // WhatsApp Microsoft Store
                btnWhatsapp.Enabled = true; // Yine de buton aktif
            }
        }

        private void btnWhatsapp_Click(object sender, EventArgs e)
        {
            string question, caption;

            if (türkçeToolStripMenuItem.Checked)
            {
                question = "WhatsApp uygulamasını açmak istediğinize emin misiniz?";
                caption = "Onay";
            }
            else
            {
                question = "Are you sure you want to open WhatsApp application?";
                caption = "Confirmation";
            }

            DialogResult result = MessageBox.Show(
                question,
                caption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (btnWhatsapp.Tag.ToString() == "local")
                    {
                        // Local exe ile aç
                        string localPath = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            @"WhatsApp\WhatsApp.exe"
                        );
                        Process.Start(localPath);
                    }
                    else
                    {
                        // Microsoft Store üzerinden aç
                        Process.Start("explorer.exe", "whatsapp://send");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        türkçeToolStripMenuItem.Checked ? "WhatsApp açılamadı." : "Failed to open WhatsApp.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private void lwDisc_MouseClick(object sender, MouseEventArgs e)
        {
            var info = lwDisc.HitTest(e.Location);
            if (info.Item != null) // bir satıra tıklanmışsa
            {
                // Checkbox durumunu tersine çevir
                info.Item.Checked = !info.Item.Checked;
            }
        }

        private void InitTotalListViewTotal()
        {
            lwTotal.View = View.Details;
            lwTotal.HeaderStyle = ColumnHeaderStyle.None; // Başlıkları gizle
            lwTotal.FullRowSelect = true;
            lwTotal.GridLines = true;

            lwTotal.Columns.Clear();
            lwTotal.Columns.Add("", 90); // Para Birimi
            lwTotal.Columns.Add("", 60); // Toplam
        }

        private void InitTotalListViewDisc()
        {
            lwDiscList.View = View.Details;
            lwDiscList.HeaderStyle = ColumnHeaderStyle.None; // Başlıkları gizle
            lwDiscList.FullRowSelect = true;
            lwDiscList.GridLines = true;

            lwDiscList.Columns.Clear();
            lwDiscList.Columns.Add("", 90); // Para Birimi
            lwDiscList.Columns.Add("", 60); // Toplam
        }

        private void InitTotalListViewTax()
        {
            lwTax.View = View.Details;
            lwTax.HeaderStyle = ColumnHeaderStyle.None; // Başlıkları gizle
            lwTax.FullRowSelect = true;
            lwTax.GridLines = true;

            lwTax.Columns.Clear();
            lwTax.Columns.Add("", 70); // Para Birimi
            lwTax.Columns.Add("", 80); // Toplam
        }

        private void UpdateTotalsFromDisc()
        {
            var currencyTotals = new Dictionary<string, decimal>();

            foreach (ListViewItem item in lwDisc.Items)
            {
                if (!decimal.TryParse(item.SubItems[COL_TOTAL].Text, out decimal total))
                    continue;

                string currency = item.SubItems[COL_CURRENCY].Text.Trim();
                if (string.IsNullOrEmpty(currency)) continue;

                if (currencyTotals.ContainsKey(currency))
                    currencyTotals[currency] += total;
                else
                    currencyTotals[currency] = total;
            }

            lwTotal.Items.Clear();

            foreach (var kvp in currencyTotals)
            {
                ListViewItem row = new ListViewItem(kvp.Key); // 1. kolon: Para Birimi
                row.SubItems.Add(kvp.Value.ToString("N2"));   // 2. kolon: Toplam
                lwTotal.Items.Add(row);
            }
        }

        private void UpdateDiscsFromDisc()
        {
            var currencyTotals = new Dictionary<string, decimal>();

            foreach (ListViewItem item in lwDisc.Items)
            {
                if (!decimal.TryParse(item.SubItems[COL_DISC_AMOUNT].Text, out decimal total))
                    continue;

                string currency = item.SubItems[COL_CURRENCY].Text.Trim();
                if (string.IsNullOrEmpty(currency)) continue;

                if (currencyTotals.ContainsKey(currency))
                    currencyTotals[currency] += total;
                else
                    currencyTotals[currency] = total;
            }

            lwDiscList.Items.Clear();

            foreach (var kvp in currencyTotals)
            {
                ListViewItem row = new ListViewItem(kvp.Key); // 1. kolon: Para Birimi
                row.SubItems.Add(kvp.Value.ToString("N2"));   // 2. kolon: İndirim Toplamı
                lwDiscList.Items.Add(row);
            }
        }

        private void ResetTax()
        {
            lwTax.Items.Clear();

            // Eğer dgwOrderDetails tablosunda hiç ürün yoksa boş bırak
            if (dgwOrderDetails.Rows.Count == 0)
                return;

            // Eğer ürün varsa ama vergi yoksa Türk Lirası 0,00 yaz
            var row = new ListViewItem("Türk Lirası");
            row.SubItems.Add("0,00");
            lwTax.Items.Add(row);
        }

        private void InitializeHoverTooltips()
        {
            // Tooltip davranışını uygulamak istediğimiz tüm ListViewler
            List<ListView> listViews = new List<ListView> { lwTotal, lwDiscList, lwTax };

            foreach (var lw in listViews)
            {
                lw.MouseEnter += lw_MouseEnter;
                lw.MouseLeave += lw_MouseLeave;
            }

            hoverTimer.Tick += hoverTimer_Tick;
        }

        // Ortak event handlerlar
        private void lw_MouseEnter(object sender, EventArgs e)
        {
            isHovering = true;
            hoveredListView = sender as ListView; // Hangi kontrolde hover olduğunu tutuyoruz
            hoverTimer.Start();
        }

        private void lw_MouseLeave(object sender, EventArgs e)
        {
            isHovering = false;
            hoverTimer.Stop();
            if (sender is ListView lw) ttAll.Hide(lw);
        }

        private void hoverTimer_Tick(object sender, EventArgs e)
        {
            hoverTimer.Stop();

            if (isHovering && hoveredListView != null)
            {
                bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
                string msg = isTurkish ? "Detaylı görmek için iki kere tıklayınız" : "Double click to see details";

                // İmlecin olduğu noktayı al
                var pt = hoveredListView.PointToClient(Cursor.Position);

                // Tooltip göster
                ttAll.Show(msg, hoveredListView, pt.X + 10, pt.Y + 10, 15000);
            }
        }
        private void btnTotal_Click(object sender, EventArgs e)
        {
            // Dil kontrolü
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // lwTotal boş mu kontrol et
            if (lwTotal.Items.Count == 0)
            {
                string message = isTurkish ? "Gösterilecek veri yok" : "No data to display";
                MessageBox.Show(message, isTurkish ? "Bilgi" : "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Pencereyi aç
            var detailForm = new TotalTreeViewForm(lwTotal, lwDisc);

            // Form başlığı dile göre
            detailForm.Text = isTurkish ? "Toplam ve Detaylar" : "Totals and Details";

            // Form boyutu ve pozisyonu
            detailForm.Size = new System.Drawing.Size(400, 500);
            detailForm.StartPosition = FormStartPosition.CenterParent;
            detailForm.AutoScroll = true;

            // MainMenu'daki chbTop durumuna göre TopMost ayarla
            var mainMenu = Application.OpenForms["MainMenu"] as MainMenu;
            if (mainMenu != null && mainMenu.chbTop.Checked)
            {
                detailForm.TopMost = true;
            }

            // Formu göster
            detailForm.ShowDialog();
        }

        private void lwTotal_DoubleClick(object sender, EventArgs e)
        {
            btnTotal_Click(sender, e);
        }

        private void btnDisc_Click(object sender, EventArgs e)
        {
            // Dil kontrolü
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // lwDiscList boş mu kontrol et
            if (lwDiscList.Items.Count == 0)
            {
                string message = isTurkish ? "Gösterilecek veri yok" : "No data to display";
                MessageBox.Show(message, isTurkish ? "Bilgi" : "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Pencereyi aç (sol = indirim toplamları, sağ = indirim detayları)
            //var detailForm = new TotalTreeViewForm(lwDiscList, lwDisc);
            var detailForm = new TotalTreeViewForm(lwDiscList, lwDisc, true);


            // Form başlığı dile göre
            detailForm.Text = isTurkish ? "Toplam İndirim ve Detaylar" : "Total Discount and Details";

            // Form boyutu ve pozisyonu
            detailForm.Size = new System.Drawing.Size(400, 500);
            detailForm.StartPosition = FormStartPosition.CenterParent;
            detailForm.AutoScroll = true;

            // MainMenu'daki chbTop durumuna göre TopMost ayarla
            var mainMenu = Application.OpenForms["MainMenu"] as MainMenu;
            if (mainMenu != null && mainMenu.chbTop.Checked)
            {
                detailForm.TopMost = true;
            }

            // Formu göster
            detailForm.ShowDialog();
        }

        private void lwDiscList_DoubleClick(object sender, EventArgs e)
        {
            btnDisc_Click(sender, e);
        }

        private void btnTax_Click(object sender, EventArgs e)
        {
            // Dil kontrolü
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // lwTax boş mu kontrol et
            if (lwTax.Items.Count == 0)
            {
                string message = isTurkish ? "Gösterilecek veri yok" : "No data to display";
                MessageBox.Show(message, isTurkish ? "Bilgi" : "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Pencereyi aç (sol = vergi toplamları, sağ = vergi detayları)
            var detailForm = new TotalTreeViewForm(lwTax, lwDisc, false, true);

            // Form başlığı dile göre
            detailForm.Text = isTurkish ? "Vergi ve Detaylar" : "Tax and Details";

            // Form boyutu ve pozisyonu
            detailForm.Size = new System.Drawing.Size(400, 500);
            detailForm.StartPosition = FormStartPosition.CenterParent;
            detailForm.AutoScroll = true;

            // MainMenu'daki chbTop durumuna göre TopMost ayarla
            var mainMenu = Application.OpenForms["MainMenu"] as MainMenu;
            if (mainMenu != null && mainMenu.chbTop.Checked)
            {
                detailForm.TopMost = true;
            }

            // Formu göster
            detailForm.ShowDialog();
        }

        private void lwTax_DoubleClick(object sender, EventArgs e)
        {
            btnTax_Click(sender, e);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            // Dil kontrolü
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // Yeni form oluştur
            Form allTablesForm = new Form();
            allTablesForm.Text = isTurkish ? "Tüm Tablolar (Sadece Görüntüleme)"
                                           : "All Tables (View Only)";
            allTablesForm.Size = new Size(900, 700);
            allTablesForm.StartPosition = FormStartPosition.CenterParent;

            // Panel (dikey layout için)
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                AutoSize = true
            };
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));

            // Üst üç tablo dikey ekle
            panel.Controls.Add(CloneDataGridView(dgwProducts), 0, 0);
            panel.Controls.Add(CloneDataGridView(dgwOrderDetails), 0, 1);
            panel.Controls.Add(CloneListView(lwDisc), 0, 2);

            // Alt kısımda yatay panel
            var bottomPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
            };
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));

            bottomPanel.Controls.Add(CloneListView(lwTotal), 0, 0);
            bottomPanel.Controls.Add(CloneListView(lwDiscList), 1, 0);
            bottomPanel.Controls.Add(CloneListView(lwTax), 2, 0);

            panel.Controls.Add(bottomPanel, 0, 3);

            // Form'a ekle ve göster
            allTablesForm.Controls.Add(panel);
            allTablesForm.ShowDialog();
        }

        // Yardımcı metodlar BÜTÜN TABLOLARI YENİ BİR FORMDA GÖSTERMEK İÇİN
        private DataGridView CloneDataGridView(DataGridView original)
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,                  // Tamamen readonly
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeColumns = false, // Kullanıcı sütun genişliğini değiştiremesin
                AllowUserToResizeRows = false,    // Kullanıcı satır yüksekliğini değiştiremesin
                AllowUserToOrderColumns = false,  // Kullanıcı sütunları sürükleyemesin
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing // ✅ Başlık yüksekliği sabit
            };

            foreach (DataGridViewColumn col in original.Columns)
            {
                var newCol = (DataGridViewColumn)col.Clone();
                newCol.ReadOnly = true; // Sütunlar da readonly
                dgv.Columns.Add(newCol);
            }

            foreach (DataGridViewRow row in original.Rows)
            {
                if (!row.IsNewRow)
                {
                    int idx = dgv.Rows.Add();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dgv.Rows[idx].Cells[i].Value = row.Cells[i].Value;
                    }
                }
            }

            return dgv;
        }

        private ListView CloneListView(ListView original)
        {
            var lv = new ListView
            {
                Dock = DockStyle.Fill,
                View = original.View,
                CheckBoxes = false,          // Kullanıcı işaretleyemesin
                FullRowSelect = true,
                GridLines = true,
                LabelEdit = false,           // Hücre düzenleme kapalı
                HeaderStyle = original.HeaderStyle
            };

            foreach (ColumnHeader col in original.Columns)
            {
                lv.Columns.Add((ColumnHeader)col.Clone());
            }

            foreach (ListViewItem item in original.Items)
            {
                var newItem = (ListViewItem)item.Clone();
                lv.Items.Add(newItem);
            }

            // Kullanıcının sütun genişliklerini değiştirmesini engelle
            lv.ColumnWidthChanging += (s, e) =>
            {
                e.Cancel = true;
                e.NewWidth = lv.Columns[e.ColumnIndex].Width;
            };

            return lv;
        }

        // BÜTÜN TABLOLARI YENİ BİR FORMDA GÖSTERMEK İÇİN BİTİŞ

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // 1. Müşteri ismi kontrolü
            string customerName = txtCustomerName.Text.Trim();
            if (string.IsNullOrWhiteSpace(customerName) ||
                customerName == "Müşteri İsmini Yazınız" ||
                customerName == "Enter Customer Name")
            {
                MessageBox.Show(
                    isTurkish ? "Müşteri ismini boş bırakmayınız." : "Please do not leave customer name empty.",
                    isTurkish ? "Uyarı" : "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtCustomerName.Focus();
                return;
            }

            // 2. Telefon numarası kontrolü
            string phone = new string(mskPhoneNo.Text.Where(char.IsDigit).ToArray());
            if (phone.Length < 10) // Türkiye için 10 haneli (5317091246)
            {
                MessageBox.Show(
                    isTurkish ? "Numarayı eksiksiz giriniz." : "Please enter the full phone number.",
                    isTurkish ? "Uyarı" : "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                mskPhoneNo.Focus();
                return;
            }

            // 3. Kargo ismi kontrolü (Duruma bağlı)
            if (cmbOrderStatus.SelectedIndex == 3 || cmbOrderStatus.SelectedIndex == 4) // 4. veya 5. seçenek
            {
                if (cmbCargo.SelectedIndex == 1) // 2. seçenek = Diğer
                {
                    string cargoName = txtCargo.Text.Trim();
                    if (string.IsNullOrWhiteSpace(cargoName) ||
                        cargoName == "Diğer Kargo Şirketinin Adını Yazınız" ||
                        cargoName == "Enter Other Cargo Company Name")
                    {
                        MessageBox.Show(
                            isTurkish ? "Lütfen kargo şirketi adını giriniz." : "Please enter the cargo company name.",
                            isTurkish ? "Uyarı" : "Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        txtCargo.Focus();
                        return;
                    }
                }

                // 4. Kargo takip numarası kontrolü
                string tracker = txtCargoTracker.Text.Trim();
                if (string.IsNullOrWhiteSpace(tracker) ||
                    tracker == "Kargo Takip Numarasını Yazınız" ||
                    tracker == "Enter Cargo Tracking Number")
                {
                    MessageBox.Show(
                        isTurkish ? "Lütfen kargo takip numarasını giriniz." : "Please enter the cargo tracking number.",
                        isTurkish ? "Uyarı" : "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    txtCargoTracker.Focus();
                    return;
                }
            }

            // Onay mesajı göster
            string confirmMessage = isTurkish
                ? "Bu siparişi eklemek istediğinize emin misiniz?"
                : "Are you sure you want to add this order?";

            var result = MessageBox.Show(confirmMessage,
                isTurkish ? "Onay" : "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            var order = new Order
            {
                ID = Guid.NewGuid().ToString(),
                CustomerName = txtCustomerName.Text,
                CustomerPhone = mskPhoneNo.Text,
                Cargo = (cmbCargo.SelectedIndex == 0 ? cmbCargo.Text : txtCargo.Text),
                CargoTrackingNumber = txtCargoTracker.Text,
                PayableStatues = cmbPayment.SelectedItem?.ToString(),
                OrderStatues = cmbOrderStatus.SelectedItem?.ToString(),
                CargoCost = nmrCargo.Value,
                OrderDate = DateTime.Now,
                AddedBy = LoggedInUser?.Name
            };

            // TotalPrice (lwTotal)
            foreach (ListViewItem item in lwTotal.Items)
            {
                string currency = item.SubItems[0].Text;
                if (decimal.TryParse(item.SubItems[1].Text, out decimal value))
                    order.TotalPrice[currency] = value;
            }

            // TotalDiscount (lwDiscList)
            foreach (ListViewItem item in lwDiscList.Items)
            {
                string currency = item.SubItems[0].Text;
                if (decimal.TryParse(item.SubItems[1].Text, out decimal value))
                    order.TotalDiscount[currency] = value;
            }

            // PayableAmount (lwTotal tekrar, indirim sonrası)
            foreach (ListViewItem item in lwTotal.Items)
            {
                string currency = item.SubItems[0].Text;
                if (decimal.TryParse(item.SubItems[1].Text, out decimal value))
                    order.PayableAmount[currency] = value;
            }

            // Vergi (lwTax sadece TL)
            if (lwTax.Items.Count > 0)
            {
                if (decimal.TryParse(lwTax.Items[0].SubItems[1].Text, out decimal tax))
                    order.Tax = tax;
            }

            // Vergi oranları
            order.TaxPercentageVAT = GetSelectedPercentFromCombo(cmbVAT);
            order.TaxPercentageSCT = GetSelectedPercentFromCombo(cmbSCT);

            // Sipariş detayları (dgwOrderDetails)
            foreach (DataGridViewRow row in dgwOrderDetails.Rows)
            {
                if (row.IsNewRow) continue;

                var detail = new Order.OrderDetail
                {
                    ProductName = row.Cells["Name"].Value?.ToString(),
                    OrderPrice = ParseDecimalFlexible(row.Cells["Price"].Value?.ToString() ?? "0"),
                    Quantity = Convert.ToInt32(row.Cells["OrderQuantity"].Value ?? 0),
                    Currency = row.Cells["PriceCurrency"].Value?.ToString(),
                    Total = ParseDecimalFlexible(row.Cells["Total"].Value?.ToString() ?? "0")
                };

                order.OrderContent.Add(detail);
            }

            // Kaydetme işlemi
            var service = new OrderService();
            service.AddOrder(order);

            MessageBox.Show(
                isTurkish ? "Sipariş başarıyla eklendi." : "Order has been added successfully.",
                isTurkish ? "Bilgi" : "Info",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // === E-Posta gönderimi (Excel ekli) ===
            if (!string.IsNullOrWhiteSpace(LoggedInUser?.Email))
            {
                var emailService = new EmailService("stockmasterapp@gmail.com", "bfbi cpom gikz azjx");

                emailService.SendOrderWithExcelAttachment(
                    LoggedInUser.Email,
                    LoggedInUser.Name,
                    order.PayableAmount.Values.Sum(),
                    dgwOrderDetails,
                    lwDisc,
                    lwTotal,
                    lwDiscList,
                    lwTax,
                    cmbDisc,
                    nmrDisc,
                    cmbSCT,
                    cmbVAT,
                    nmrCargo,
                    txtCustomerName,
                    mskPhoneNo,
                    DateTime.Now,
                    LoggedInUser.Language,
                    LoggedInUser.Password
                );
            }

            // dgwProducts tablosundaki Sipariş Adeti sütunu sıfırlansın
            foreach (DataGridViewRow row in dgwProducts.Rows)
            {
                if (row.Cells["OrderQuantity"] != null)
                    row.Cells["OrderQuantity"].Value = 0;
            }

            // dgwOrderDetails içeriğini temizle
            dgwOrderDetails.Rows.Clear();

            // TextBox ve MaskedTextBox’ları temizle
            txtCargo.Text = "";
            txtCargoTracker.Text = "";
            txtCustomerName.Text = "";
            mskPhoneNo.Text = "";

            // NumericUpDown’ları sıfırla
            nmrCargo.Value = 0;
            nmrDisc.Value = 0;

            // ComboBox’ları ilk seçeneğe getir
            if (cmbDisc.Items.Count > 0) cmbDisc.SelectedIndex = 0;
            if (cmbSCT.Items.Count > 0) cmbSCT.SelectedIndex = 0;
            if (cmbVAT.Items.Count > 0) cmbVAT.SelectedIndex = 0;
            if (cmbPayment.Items.Count > 0) cmbPayment.SelectedIndex = 0;
            if (cmbOrderStatus.Items.Count > 0) cmbOrderStatus.SelectedIndex = 0;
            if (cmbCargo.Items.Count > 0) cmbCargo.SelectedIndex = 0;

            // ListView’leri temizle
            lwDisc.Items.Clear();
            lwDiscList.Items.Clear();
            lwTotal.Items.Clear();
            lwTax.Items.Clear();

            // Placeholderları geri yükle
            UpdatePlaceholders();
        }

        private decimal GetSelectedPercentFromCombo(ComboBox cmb)
        {
            if (cmb.SelectedIndex <= 0) return 0;

            string txt = cmb.SelectedItem.ToString();
            foreach (var part in txt.Split(' '))
            {
                if (part.Contains("%") && decimal.TryParse(part.Replace("%", ""), out decimal val))
                    return val;
            }
            return 0;
        }
    }

}