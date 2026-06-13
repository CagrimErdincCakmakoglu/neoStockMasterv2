using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Data.Services;
using neoStockMasterv2.Data.Services.BankServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace neoStockMasterv2.Forms
{
    public partial class OrderViewEditScreen : Form
    {
        public static User LoggedInUser { get; set; }
        private OrderService _orderService = new OrderService();
        private string _previousProductName = null;
        private string _currentCurrency = "Türk Lirası";
        private bool _isLoading = false;
        private Dictionary<string, (decimal BuyRate, decimal SellRate)> _cachedRates = null;
        private ToolTip ttPriceLock = new ToolTip();
        private bool isImageRotated = false;

        // --- Hover tooltip & double-click için alanlar ---
        private ListView hoveredListView;
        private bool isHovering = false;
        private ToolTip ttAll = new ToolTip();
        private System.Windows.Forms.Timer hoverTimer = new System.Windows.Forms.Timer { Interval = 600 };

        public OrderViewEditScreen(string currentLanguage)
        {
            InitializeComponent();

            ApplyOrderSelectionState();
        }

        private void OrderViewEditScreen_Load(object sender, EventArgs e)
        {
            LoadOrdersIntoComboBox();
            InitializeOrderDetailsGrid();
            InitializeLwDisc();
            RefreshFormLayout();

            InitPriceLockFeature();
            InitializeHoverTooltips();

            // PricingOrderScreen’deki gibi DataGridView başlık stillerini uygula
            SetModernHeaderStyle(dgwProducts);

            // Comboboxları doldurma metodu
            FillComboBoxes();

            UpdateDateTimePickerLanguage();

            SetPriceLockToolTip(pbInfo);

            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            LanguageService.SetLanguage(LanguageService.CurrentLanguage);
        }

        private void LanguageService_LanguageChanged()
        {
            _isLoading = true; // Dil değişimi sırasında onay ekranlarını engelle

            UpdateFormTexts();

            // Dil değişince ComboBox'ı sıfırla ve yeniden doldur
            cmbOrders.SelectedIndex = -1;
            LoadOrdersIntoComboBox();

            // Formdaki aktiflik durumunu yeniden uygula
            ApplyOrderSelectionState();

            // Dil değişince yüksekliği ve btnVer'in metnini güncelle
            RefreshFormLayout();

            InitializeOrderDetailsGrid();
            InitializeLwDisc();

            // Comboboxları doldurma metodu
            FillComboBoxes();

            UpdateDateTimePickerLanguage();

            // Radio button'ları sıfırla, formu ilk boyuna döndür
            rbRead.Checked = false;
            rbEdit.Checked = false;
            rbDelete.Checked = false;
            this.Height = 130;

            // Kontrol durumlarını yeniden uygula
            ApplyOrderSelectionState();

            _isLoading = false; // Dil değişimi tamamlandı

            SetPriceLockToolTip(pbInfo);
        }

        private void UpdateFormTexts()
        {
            türkçeToolStripMenuItem.Text = LanguageService.GetString("Türkçe");
            englishToolStripMenuItem.Text = LanguageService.GetString("English2");

            grbOrders.Text = LanguageService.GetString("Siparişler");
            grbMethod.Text = LanguageService.GetString("Yöntem");
            rbRead.Text = LanguageService.GetString("Görüntüle");
            rbEdit.Text = LanguageService.GetString("Düzenle");
            rbDelete.Text = LanguageService.GetString("Sil");
            grbPriceDetails.Text = LanguageService.GetString("Fiyat Detayları");
            grbDisc.Text = LanguageService.GetString("İndirim");
            grbTax.Text = LanguageService.GetString("Vergi");
            grbCargo.Text = LanguageService.GetString("Kargo Bedeli");
            lblTotalPrice.Text = LanguageService.GetString("Toplam Tutar");
            lblTotalDisc.Text = LanguageService.GetString("Toplam İndirim");
            lblTotalTax.Text = LanguageService.GetString("Vergi");
            chbPriceLock.Text = LanguageService.GetString("Fiyat Kilidi");


            this.Text = LanguageService.GetString("Siparişleri Görüntüle - Düzenle");

            // TextBox alanlarını da dile göre güncelle
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            txtCustomerName.Text = isTurkish
                ? "Müşteri Adını Yazınız"
                : "Enter Customer Name";

            if (cmbCargo.SelectedIndex == 0) // “Diğer” seçiliyse placeholder göster
            {
                txtCargo.Text = isTurkish
                    ? "Diğer Kargo Şirketinin Adını Yazınız"
                    : "Enter Other Cargo Company Name";
            }
            else
            {
                txtCargo.Text = isTurkish
                    ? "Kargo Şirketi Adı"
                    : "Cargo Company Name";
            }

            txtCargoTracker.Text = isTurkish
                ? "Kargo Takip Numarasını Yazınız"
                : "Enter Cargo Tracking Number";

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


        private void cmbOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyOrderSelectionState();
            RefreshFormLayout();

            // Eğer sipariş seçimi kaldırıldıysa form alanlarını ve tabloları temizle
            if (cmbOrders.SelectedIndex == -1)
            {
                ClearFormFields();
            }
            // Sipariş değiştiğinde ve bir yöntem seçiliyse detayları hemen yükle
            else if (rbRead.Checked || rbEdit.Checked || rbDelete.Checked)
            {
                LoadSelectedOrderDetails();
            }
        }

        private void ApplyOrderSelectionState()
        {
            bool hasOrderSelection = cmbOrders.SelectedIndex >= 0;
            bool hasMethodSelection = rbRead.Checked || rbEdit.Checked || rbDelete.Checked;

            // Yalnızca ana grupları devre dışı bırak
            grbOrders.Enabled = false;
            grbMethod.Enabled = false;
            grbPriceDetails.Enabled = false;
            grbDisc.Enabled = false;
            grbTax.Enabled = false;
            grbCargo.Enabled = false;
            btnVer.Enabled = false;

            // Menü çubuğu ve sipariş grubu her zaman aktif
            menuStripLanguage.Enabled = true;
            grbOrders.Enabled = true;

            // Sipariş seçildiyse yöntem seçimi aktif olsun
            if (hasOrderSelection)
            {
                grbMethod.Enabled = true;
            }

            // Seçilen yönteme göre alanları kontrol et
            if (hasMethodSelection)
            {
                if (rbRead.Checked)
                {
                    DisableAllDataFields();
                    btnVer.Enabled = false;
                    // Görüntüleme modunda tablo seçimini kaldır
                    dgwProducts.ClearSelection();
                }
                else if (rbEdit.Checked)
                {
                    // Düzenleme modunda tüm alanları etkinleştir (genel)
                    EnableAllDataFields();

                    // Ancak cmbCargo sadece rbEdit ve sponsorlu kargo varsa erişilebilir olacak
                    if (string.IsNullOrWhiteSpace(LoggedInUser?.Cargo))
                    {
                        cmbCargo.Enabled = false;
                    }
                    else
                    {
                        cmbCargo.Enabled = true;
                    }

                    // Ek olarak bu gruplar her zaman aktif olacak
                    grbDisc.Enabled = true;
                    grbTax.Enabled = true;
                    grbCargo.Enabled = true;
                    grbPriceDetails.Enabled = true;

                    // Güncelle butonunu aktif yap
                    btnVer.Enabled = true;
                }
                else if (rbDelete.Checked)
                {
                    DisableAllDataFields();
                    // Silme butonunu aktif yap
                    btnVer.Enabled = true;
                    // Silme modunda tablo seçimini kaldır
                    dgwProducts.ClearSelection();
                }
            }
        }

        private void EnableAllDataFields()
        {
            dgwProducts.Enabled = true;
            lwDisc.Enabled = true;
            // cmbDisc, nmrDisc, cmbSCT, cmbVAT lwDisc'te seçim yapılana kadar kapalı kalır
            cmbDisc.Enabled = false;
            nmrDisc.Enabled = false;
            cmbSCT.Enabled = false;
            cmbVAT.Enabled = false;
            nmrCargo.Enabled = true;
            lwTotal.Enabled = true;
            lwDiscList.Enabled = true;
            lwTax.Enabled = true;
            cmbPayment.Enabled = true;
            cmbCargo.Enabled = false; // varsayılan olarak false; ApplyOrderSelectionState düzeltecek
            txtCustomerName.Enabled = true;
            cmbOrderStatus.Enabled = true;
            txtCargo.Enabled = false;
            mskPhoneNo.Enabled = true;
            txtCargoTracker.Enabled = true;
            dtpOrder.Enabled = true;
            btnVer.Enabled = true;
        }

        private void DisableAllDataFields()
        {
            dgwProducts.Enabled = false;
            lwDisc.Enabled = false;
            cmbDisc.Enabled = false;
            nmrDisc.Enabled = false;
            cmbSCT.Enabled = false;
            cmbVAT.Enabled = false;
            nmrCargo.Enabled = false;
            lwTotal.Enabled = false;
            lwDiscList.Enabled = false;
            lwTax.Enabled = false;
            cmbPayment.Enabled = false;
            cmbCargo.Enabled = false;
            cmbCargo.Enabled = false;
            txtCustomerName.Enabled = false;
            cmbOrderStatus.Enabled = false;
            txtCargo.Enabled = false;
            mskPhoneNo.Enabled = false;
            txtCargoTracker.Enabled = false;
            dtpOrder.Enabled = false;
            btnVer.Enabled = true;
        }

        private void LoadOrdersIntoComboBox()
        {
            cmbOrders.Items.Clear();
            cmbOrders.SelectedIndex = -1;

            // Giriş yapan kullanıcıyı ata
            OrderService.LoggedInUser = LoggedInUser;
            var userOrders = _orderService.GetOrdersByLoggedInUser();

            string dateFormat = LanguageService.CurrentLanguage == "Türkçe" ? "dd.MM.yyyy" : "MM.dd.yyyy";

            foreach (var order in userOrders)
            {
                // ID kullanıcıya görünmeyecek ama ComboBoxItem.Value içinde tutulacak
                string displayText = $"{order.CustomerName} ## {order.OrderDate.ToString(dateFormat)}";

                cmbOrders.Items.Add(new ComboBoxItem
                {
                    Text = displayText,
                    Value = order.ID // ID burada gizli olarak saklanıyor
                });
            }

            cmbOrders.DisplayMember = "Text";
            cmbOrders.ValueMember = "Value";
        }

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void rbRead_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRead.Checked)
            {
                // Yalnızca bir sipariş seçilmişse detayları yükle
                if (cmbOrders.SelectedIndex >= 0)
                {
                    LoadSelectedOrderDetails();
                }
            }

            ApplyOrderSelectionState();
            RefreshFormLayout();
        }

        private void rbEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEdit.Checked)
            {
                if (cmbOrders.SelectedIndex >= 0)
                {
                    LoadSelectedOrderDetails();
                }
            }

            ApplyOrderSelectionState();
            RefreshFormLayout();
        }

        private void rbDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDelete.Checked)
            {
                if (cmbOrders.SelectedIndex >= 0)
                {
                    LoadSelectedOrderDetails();
                }
            }

            ApplyOrderSelectionState();
            RefreshFormLayout();
        }


        private void RefreshFormLayout()
        {
            // Form yeni açıldığında veya hiçbir radiobutton seçili değilse
            if (!rbRead.Checked && !rbEdit.Checked && !rbDelete.Checked)
            {
                this.Height = 130;
            }
            else if (rbRead.Checked)
            {
                this.Height = 670;
            }
            else if (rbEdit.Checked || rbDelete.Checked)
            {
                this.Height = 720;
            }

            // Buton metnini dil ve seçime göre ayarla
            if (btnVer != null)
            {
                string lang = LanguageService.CurrentLanguage;

                if (lang == "Türkçe")
                {
                    if (rbEdit.Checked)
                        btnVer.Text = "Güncelle";
                    else if (rbDelete.Checked)
                        btnVer.Text = "Sil";
                    else
                        btnVer.Text = "Göster";
                }
                else
                {
                    if (rbEdit.Checked)
                        btnVer.Text = "Update";
                    else if (rbDelete.Checked)
                        btnVer.Text = "Delete";
                    else
                        btnVer.Text = "Show";
                }
            }
        }

        private void InitializeOrderDetailsGrid()
        {
            bool isEnglish = LanguageService.CurrentLanguage == "English";

            dgwProducts.Columns.Clear();

            var productNameColumn = new DataGridViewComboBoxColumn()
            {
                Name = "Name",
                HeaderText = isEnglish ? "Name" : "İsmi",
                Width = 150,
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            };

            var productService = new ProductService();
            ProductService.LoggedInUser = LoggedInUser;

            var userProducts = productService.GetProductsByLoggedInUser();

            if (userProducts != null && userProducts.Count > 0)
            {
                foreach (var p in userProducts)
                    productNameColumn.Items.Add(p.Name);
            }

            dgwProducts.Columns.Add(productNameColumn);

            dgwProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Price",
                HeaderText = isEnglish ? "Price" : "Fiyat",
                ReadOnly = false, // Düzenlenebilir olması için false olarak değiştirildi
                Width = 120
            });

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

            currencies.Sort();
            currencies.Insert(0, "Türk Lirası");

            var currencyColumn = new DataGridViewComboBoxColumn()
            {
                Name = "PriceCurrency",
                HeaderText = isEnglish ? "Price Currency" : "Fiyat Para Birimi",
                Width = 160,
                DropDownWidth = 180,
                FlatStyle = FlatStyle.Flat
            };

            currencyColumn.Items.AddRange(currencies.ToArray());
            dgwProducts.Columns.Add(currencyColumn);

            dgwProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "OrderQuantity",
                HeaderText = isEnglish ? "Order Quantity" : "Sipariş Adeti",
                ReadOnly = false, // Düzenlenebilir olması için false olarak değiştirildi
                Width = 120
            });

            dgwProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Total",
                HeaderText = isEnglish ? "Total" : "Tutar",
                ReadOnly = true,
                Width = 120
            });
        }

        private void InitializeLwDisc()
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
                lwDisc.Columns.Add("Tutar", 80);
                lwDisc.Columns.Add("Para Birimi", 100);
                lwDisc.Columns.Add("İndirim Durumu", 115);
                lwDisc.Columns.Add("İndirim", 70);
                lwDisc.Columns.Add("ÖTV Durumu", 100);
                lwDisc.Columns.Add("KDV Durumu", 96);
            }
            else
            {
                lwDisc.Columns.Add("Product Name", 90);
                lwDisc.Columns.Add("Amount", 80);
                lwDisc.Columns.Add("Currency", 100);
                lwDisc.Columns.Add("Discount Status", 115);
                lwDisc.Columns.Add("Discount", 70);
                lwDisc.Columns.Add("SCT Status", 100);
                lwDisc.Columns.Add("VAT Status", 96);
            }

            // Kolon genişliği değişimini engelle (PricingOrderScreen'deki gibi)
            lwDisc.ColumnWidthChanging += (s, e) =>
            {
                e.Cancel = true;
                e.NewWidth = lwDisc.Columns[e.ColumnIndex].Width;
            };

            // Satıra tıklanınca checkbox toggle + kontrol erişimi güncelle
            lwDisc.MouseClick -= lwDisc_MouseClick;
            lwDisc.MouseClick += lwDisc_MouseClick;

            // Yalnızca ItemChecked kullanılır; ItemCheck kaldırıldı (çift tetiklenmeyi önlemek için)
            lwDisc.ItemChecked -= lwDisc_ItemChecked;
            lwDisc.ItemChecked += lwDisc_ItemChecked;
        }

        private void SetModernHeaderStyle(DataGridView dgv)
        {
            var headerStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(245, 245, 245),
                ForeColor = Color.FromArgb(50, 50, 50),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                WrapMode = DataGridViewTriState.False
            };

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle = headerStyle;

            dgv.ColumnHeadersHeight = 30;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            dgv.GridColor = Color.FromArgb(220, 220, 220);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 201);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }

        private void FillComboBoxes()
        {
            _isLoading = true;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            cmbPayment.Items.Clear();
            cmbOrderStatus.Items.Clear();
            cmbCargo.Items.Clear();
            cmbDisc.Items.Clear();
            cmbSCT.Items.Clear();
            cmbVAT.Items.Clear();

            // === Ödeme Durumu ===
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

            // === Sipariş Durumu ===
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

            // === Kargo ===
            cmbCargo.Items.Clear();

            // Eğer LoggedInUser.Cargo null değilse önce kullanıcı kargosunu ekle
            if (!string.IsNullOrWhiteSpace(LoggedInUser?.Cargo))
            {
                cmbCargo.Items.Add(LoggedInUser.Cargo);
            }

            // Sonra Diğer/Other ekle
            cmbCargo.Items.Add(isTurkish ? "Diğer" : "Other");

            cmbCargo.SelectedIndex = 0;

            // === İndirim ===
            if (isTurkish)
            {
                cmbDisc.Items.Add("İndirim Yapılmıyor");
                for (int i = 5; i <= 50; i += 5)
                    cmbDisc.Items.Add($"%{i} İndirim Yap");
            }
            else
            {
                cmbDisc.Items.Add("No Discount");
                for (int i = 5; i <= 50; i += 5)
                    cmbDisc.Items.Add($"Apply {i}% Discount");
            }
            cmbDisc.SelectedIndex = 0;

            // === ÖTV ===
            if (isTurkish)
            {
                cmbSCT.Items.Add("ÖTV Yok");
                foreach (var rate in new int[] { 20, 37, 45, 50, 60, 63, 67, 80, 100, 110, 150, 220 })
                    cmbSCT.Items.Add($"%{rate} ÖTV ekle");
            }
            else
            {
                cmbSCT.Items.Add("No Excise Tax");
                foreach (var rate in new int[] { 20, 37, 45, 50, 60, 63, 67, 80, 100, 110, 150, 220 })
                    cmbSCT.Items.Add($"Add {rate}% SCT");
            }
            cmbSCT.SelectedIndex = 0;

            // === KDV ===
            if (isTurkish)
            {
                cmbVAT.Items.Add("KDV Yok");
                foreach (var rate in new int[] { 1, 8, 10, 18, 20 })
                    cmbVAT.Items.Add($"%{rate} KDV ekle");
            }
            else
            {
                cmbVAT.Items.Add("No VAT");
                foreach (var rate in new int[] { 1, 8, 10, 18, 20 })
                    cmbVAT.Items.Add($"Add {rate}% VAT");
            }
            cmbVAT.SelectedIndex = 0;

            _isLoading = false;
        }

        private void UpdateDateTimePickerLanguage()
        {
            if (dtpOrder == null) return;

            CultureInfo culture;

            if (LanguageService.CurrentLanguage == "Türkçe")
            {
                culture = new CultureInfo("tr-TR");
                dtpOrder.CustomFormat = "dddd, dd.MM.yyyy";
            }
            else
            {
                culture = new CultureInfo("en-US");
                dtpOrder.CustomFormat = "dddd, MM.dd.yyyy";
            }

            // Thread kültürlerini güncelle
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // DateTimePicker’ın kendi format sağlayıcısını da güncelle
            dtpOrder.Format = DateTimePickerFormat.Custom;

            // Gün/ay isimlerinin doğru dilde görünmesi için formatlama sağlayıcısını doğrudan kullan
            dtpOrder.Value = DateTime.Parse(dtpOrder.Value.ToString(culture), culture);

            dtpOrder.Refresh();
        }

        private void LoadSelectedOrderDetails()
        {
            _isLoading = true;

            // Tabloları temizle
            dgwProducts.Rows.Clear();
            lwDisc.Items.Clear();
            lwTotal.Items.Clear();
            lwDiscList.Items.Clear();
            lwTax.Items.Clear();

            if (cmbOrders.SelectedItem == null)
                return;

            var selectedItem = cmbOrders.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;

            string selectedOrderId = selectedItem.Value;
            var selectedOrder = _orderService.GetOrderById(selectedOrderId);
            if (selectedOrder == null) return;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // === Sponsorlu kargo ve sipariş kargo bilgileri kontrolü ===
            if (!string.IsNullOrWhiteSpace(selectedOrder.Cargo))
            {
                // Önce combobox içinde arıyoruz
                bool found = false;

                for (int i = 0; i < cmbCargo.Items.Count; i++)
                {
                    if (cmbCargo.Items[i].ToString().Equals(selectedOrder.Cargo, StringComparison.OrdinalIgnoreCase))
                    {
                        cmbCargo.SelectedIndex = i;
                        found = true;
                        break;
                    }
                }

                // Eğer listede yoksa Diğer seçilsin, textbox sipariş değerini göstersin
                if (!found)
                {
                    cmbCargo.SelectedIndex = cmbCargo.Items.Count - 1; // Diğer
                    txtCargo.Text = selectedOrder.Cargo;
                }
            }
            else
            {
                // Siparişte kargo yoksa sponsor kargo varsa onu seçelim
                if (!string.IsNullOrWhiteSpace(LoggedInUser?.Cargo))
                {
                    if (cmbCargo.Items[0].ToString() == LoggedInUser.Cargo)
                        cmbCargo.SelectedIndex = 0;
                }
                else
                {
                    // Sponsor yok → Diğer
                    cmbCargo.SelectedIndex = cmbCargo.Items.Count - 1;
                }
            }

            // === Siparişin temel bilgilerini forma doldur ===

            // Müşteri Adı
            txtCustomerName.Text = selectedOrder.CustomerName;

            // Telefon
            mskPhoneNo.Text = selectedOrder.CustomerPhone;

            // Sipariş Tarihi
            dtpOrder.Value = selectedOrder.OrderDate;

            // Ödeme Durumu
            if (!string.IsNullOrEmpty(selectedOrder.PayableStatues))
            {
                for (int i = 0; i < cmbPayment.Items.Count; i++)
                {
                    if (cmbPayment.Items[i].ToString().Contains(selectedOrder.PayableStatues))
                    {
                        cmbPayment.SelectedIndex = i;
                        break;
                    }
                }
            }

            // Sipariş Durumu
            if (!string.IsNullOrEmpty(selectedOrder.OrderStatues))
            {
                for (int i = 0; i < cmbOrderStatus.Items.Count; i++)
                {
                    if (cmbOrderStatus.Items[i].ToString().Contains(selectedOrder.OrderStatues))
                    {
                        cmbOrderStatus.SelectedIndex = i;
                        break;
                    }
                }
            }

            // Kargo Şirketi
            if (!string.IsNullOrEmpty(selectedOrder.Cargo))
            {
                bool found = false;

                for (int i = 0; i < cmbCargo.Items.Count; i++)
                {
                    if (cmbCargo.Items[i].ToString().Contains(selectedOrder.Cargo))
                    {
                        cmbCargo.SelectedIndex = i;
                        found = true;
                        break;
                    }
                }

                // Listede yoksa “Diğer” seçtirip textbox’a yazalım
                if (!found)
                {
                    cmbCargo.SelectedIndex = 0; // Diğer
                    txtCargo.Text = selectedOrder.Cargo;
                }
            }

            // Kargo Takip No
            txtCargoTracker.Text = selectedOrder.CargoTrackingNumber ?? "";

            // dgwProducts tablosunu doldur (ürün detayları)
            foreach (var detail in selectedOrder.OrderContent)
            {
                dgwProducts.Rows.Add(
                    detail.ProductName,   // Name
                    detail.OrderPrice,    // Price  — decimal
                    detail.Currency,      // PriceCurrency
                    detail.Quantity,      // OrderQuantity
                    detail.Total          // Total   — decimal
                );
            }

            // lwDisc tablosunu doldur
            foreach (var detail in selectedOrder.OrderContent)
            {
                var item = new ListViewItem("");
                item.SubItems.Add(detail.ProductName);

                // Tutarı parse et - TR kültürü ile (virgül ondalık ayraç)
                decimal rawTotal = detail.Total;
                item.SubItems.Add(rawTotal.ToString("F2"));
                item.SubItems.Add(detail.Currency);

                bool applyDiscount = detail.IsDiscounted;

                // İndirim durumu göster
                string discountStatus;
                if (applyDiscount && selectedOrder.DiscountPercentage > 0 && selectedOrder.ExtraDiscountAmount > 0)
                    discountStatus = $"%{selectedOrder.DiscountPercentage} (+{selectedOrder.ExtraDiscountAmount:F2})";
                else if (applyDiscount && selectedOrder.DiscountPercentage > 0)
                    discountStatus = $"%{selectedOrder.DiscountPercentage}";
                else if (applyDiscount && selectedOrder.ExtraDiscountAmount > 0)
                    discountStatus = $"+{selectedOrder.ExtraDiscountAmount:F2}";
                else
                    discountStatus = isTurkish ? "İndirim Yok" : "No Discount";

                item.SubItems.Add(discountStatus);

                // İndirim miktarı
                string discAmountText = applyDiscount ? selectedOrder.ExtraDiscountAmount.ToString("F2") : "0,00";
                item.SubItems.Add(discAmountText);

                // ÖTV ve KDV — sadece IsDiscounted olanlar için hesapla
                // Vergi her zaman TL cinsinden gösterilir; dövizli ürünlerde kur çarpılır
                decimal sctValOnLoad = 0m, vatValOnLoad = 0m;
                if (applyDiscount)
                {
                    decimal percDisc = rawTotal * (selectedOrder.DiscountPercentage / 100m);
                    decimal priceAfterDisc = rawTotal - percDisc - selectedOrder.ExtraDiscountAmount;
                    if (priceAfterDisc < 0) priceAfterDisc = 0;

                    // Kur çarpanı: dövizliyse _cachedRates kullan (null ise 1 → TL gibi davran)
                    decimal loadRateToTL = 1m;
                    if (!IsTL(detail.Currency) && _cachedRates != null)
                        loadRateToTL = GetRateToTL(detail.Currency, _cachedRates);

                    // Vergi tutarları TL cinsinden
                    sctValOnLoad = priceAfterDisc * loadRateToTL * (selectedOrder.TaxPercentageSCT / 100m);
                    vatValOnLoad = (priceAfterDisc * loadRateToTL + sctValOnLoad) * (selectedOrder.TaxPercentageVAT / 100m);
                }

                string sctStatus = (applyDiscount && selectedOrder.TaxPercentageSCT > 0)
                    ? $"%{selectedOrder.TaxPercentageSCT} ({sctValOnLoad:N2} TL)"
                    : (isTurkish ? "ÖTV Yok" : "No Excise Tax");

                string vatStatus = (applyDiscount && selectedOrder.TaxPercentageVAT > 0)
                    ? $"%{selectedOrder.TaxPercentageVAT} ({vatValOnLoad:N2} TL)"
                    : (isTurkish ? "KDV Yok" : "No VAT");

                item.SubItems.Add(sctStatus);
                item.SubItems.Add(vatStatus);

                item.Tag = rawTotal;
                lwDisc.Items.Add(item);
            }

            // Kontrolleri doldur
            // --- İndirim ---
            if (selectedOrder.DiscountPercentage == 0)
                cmbDisc.SelectedIndex = 0;
            else
            {
                for (int i = 0; i < cmbDisc.Items.Count; i++)
                {
                    if (cmbDisc.Items[i].ToString().Contains(selectedOrder.DiscountPercentage.ToString()))
                    {
                        cmbDisc.SelectedIndex = i;
                        break;
                    }
                }
            }
            nmrDisc.Value = selectedOrder.ExtraDiscountAmount;

            // --- ÖTV ---
            if (selectedOrder.TaxPercentageSCT == 0)
                cmbSCT.SelectedIndex = 0;
            else
            {
                for (int i = 0; i < cmbSCT.Items.Count; i++)
                {
                    if (cmbSCT.Items[i].ToString().Contains(selectedOrder.TaxPercentageSCT.ToString()))
                    {
                        cmbSCT.SelectedIndex = i;
                        break;
                    }
                }
            }

            // --- KDV ---
            if (selectedOrder.TaxPercentageVAT == 0)
                cmbVAT.SelectedIndex = 0;
            else
            {
                for (int i = 0; i < cmbVAT.Items.Count; i++)
                {
                    if (cmbVAT.Items[i].ToString().Contains(selectedOrder.TaxPercentageVAT.ToString()))
                    {
                        cmbVAT.SelectedIndex = i;
                        break;
                    }
                }
            }

            // --- Kargo ---
            nmrCargo.Value = selectedOrder.CargoCost;

            // === lwTotal / lwDiscList / lwTax: yükleme bittikten sonra UpdateSummaryListViews ile doldur ===
            lwTotal.View = View.Details;
            lwTotal.Columns.Clear();
            lwTotal.Columns.Add("", lwTotal.Width / 2);
            lwTotal.Columns.Add("", lwTotal.Width / 2);
            lwTotal.HeaderStyle = ColumnHeaderStyle.None;
            lwTotal.Items.Clear();

            lwDiscList.View = View.Details;
            lwDiscList.Columns.Clear();
            lwDiscList.Columns.Add("", lwDiscList.Width / 2);
            lwDiscList.Columns.Add("", lwDiscList.Width / 2);
            lwDiscList.HeaderStyle = ColumnHeaderStyle.None;
            lwDiscList.Items.Clear();

            lwTax.View = View.Details;
            lwTax.Columns.Clear();
            lwTax.Columns.Add("", lwTax.Width / 2);
            lwTax.Columns.Add("", lwTax.Width / 2);
            lwTax.HeaderStyle = ColumnHeaderStyle.None;
            lwTax.Items.Clear();

            // Tabloları lwDisc'teki güncel verilerden gruplu olarak doldur
            UpdateSummaryListViews(null, null, null);

            // Eğer Görüntüleme (rbRead) modu aktifse hiçbir hücre seçili olmasın
            if (rbRead.Checked)
                dgwProducts.ClearSelection();

            _isLoading = false;
        }

        private void dgwProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Sadece Name sütunu için ComboBox kontrolünü yakala
            if (dgwProducts.CurrentCell.ColumnIndex == dgwProducts.Columns["Name"].Index)
            {
                ComboBox cb = e.Control as ComboBox;
                if (cb != null)
                {
                    cb.SelectedIndexChanged -= ProductNameChanged;
                    cb.SelectedIndexChanged += ProductNameChanged;
                }
            }

            // YENİ: Para Birimi (PriceCurrency) sütunu kontrolü (ÇAĞRIM 61)
            if (dgwProducts.CurrentCell.ColumnIndex == dgwProducts.Columns["PriceCurrency"].Index)
            {
                ComboBox cb = e.Control as ComboBox;
                if (cb != null)
                {
                    cb.SelectedIndexChanged -= CurrencyChanged; // Mükerrerliği önle
                    cb.SelectedIndexChanged += CurrencyChanged;
                }
            }

            // 2. Sayısal Validasyon (Sipariş Adeti ve Fiyat sütunları için)
            string columnName = dgwProducts.CurrentCell.OwningColumn.Name;
            if (columnName == "OrderQuantity" || columnName == "Price")
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    // Olayın mükerrer eklenmemesi için önce çıkarıp sonra ekliyoruz
                    tb.KeyPress -= NumericOnly_KeyPress;
                    tb.KeyPress += NumericOnly_KeyPress;
                }
            }
        }

        // Sadece rakam ve virgül girişine izin veren yardımcı metod
        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Rakam değilse, kontrol tuşu (backspace vb.) değilse ve virgül değilse engelle
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Zaten bir virgül varsa ikinciye izin verme
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void ProductNameChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb == null) return;

            string newProductName = cb.SelectedItem?.ToString();
            string oldProductName = _previousProductName;

            // Eski isim güncellenmeden önce kesin kontrol
            if (string.IsNullOrEmpty(oldProductName) || newProductName == oldProductName)
                return;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            string message = isTurkish
                ? $"'{oldProductName}' sipariş detayını '{newProductName}' olarak değiştirmek istediğinize emin misiniz?"
                : $"Are you sure you want to change the order detail from '{oldProductName}' to '{newProductName}'?";

            string caption = isTurkish ? "Ürün Değiştirme Onayı" : "Product Change Confirmation";

            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                // eski ürün geri yüklensin
                cb.SelectedIndexChanged -= ProductNameChanged;
                cb.SelectedItem = oldProductName;
                cb.SelectedIndexChanged += ProductNameChanged;
            }
            else
            {
                _previousProductName = newProductName;
            }
        }

        private void dgwProducts_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgwProducts.Columns[e.ColumnIndex].Name == "Name")
            {
                _previousProductName = dgwProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            }

            if (dgwProducts.Columns[e.ColumnIndex].Name == "Price" ||
                dgwProducts.Columns[e.ColumnIndex].Name == "OrderQuantity")
            {
                _previousProductName = dgwProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            }

            if (dgwProducts.Columns[e.ColumnIndex].Name == "PriceCurrency")
            {
                _previousProductName = dgwProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            }
        }

        private void cmbCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 1. Durum: Eğer kullanıcının tanımlı bir kargosu YOKSA
            if (string.IsNullOrWhiteSpace(LoggedInUser?.Cargo))
            {
                cmbCargo.Enabled = false; // ComboBox kilitli kalır
                txtCargo.Enabled = true;  // Manuel giriş her zaman açık
            }
            // 2. Durum: Eğer kullanıcının tanımlı bir kargosu VARSA
            else
            {
                // 0. index (Sponsorlu Kargo) seçiliyse txtCargo kapalı
                if (cmbCargo.SelectedIndex == 0)
                {
                    txtCargo.Enabled = false;
                    txtCargo.Clear();
                }
                // 1. index (Diğer/Other) seçiliyse txtCargo açık
                else if (cmbCargo.SelectedIndex == 1)
                {
                    txtCargo.Enabled = true;
                }
            }
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            // 1. Seçili sipariş var mı kontrol et
            if (cmbOrders.SelectedItem == null)
            {
                string errorMsg = LanguageService.CurrentLanguage == "Türkçe" ? "Lütfen bir sipariş seçin." : "Please select an order.";
                MessageBox.Show(errorMsg, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = cmbOrders.SelectedItem as ComboBoxItem;
            string selectedOrderId = selectedItem.Value;

            // 2. Silme Modu Seçili mi?
            if (rbDelete.Checked)
            {
                bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
                string confirmMsg = isTurkish
                    ? $"'{selectedItem.Text}' numaralı siparişi silmek istediğinize emin misiniz?"
                    : $"Are you sure you want to delete the order '{selectedItem.Text}'?";

                string caption = isTurkish ? "Sipariş Silme Onayı" : "Order Delete Confirmation";

                // Kullanıcıdan onay al
                var result = MessageBox.Show(confirmMsg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Servis üzerinden silme işlemini yap
                    _orderService.DeleteOrder(selectedOrderId);

                    MessageBox.Show(isTurkish ? "Sipariş başarıyla silindi." : "Order successfully deleted.",
                                    isTurkish ? "Başarılı" : "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 3. UI Güncelleme: ComboBox'ı yenile ve formu temizle
                    LoadOrdersIntoComboBox();
                    ClearFormFields(); // Formdaki alanları temizlemek için yardımcı metod
                }
            }
            else if (rbEdit.Checked)
            {
                bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

                // ── Mevcut siparişi al 
                var existingOrder = _orderService.GetOrderById(selectedOrderId);
                if (existingOrder == null)
                {
                    string notFoundMsg = isTurkish ? "Sipariş bulunamadı." : "Order not found.";
                    MessageBox.Show(notFoundMsg, isTurkish ? "Hata" : "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ── Formdan güncellenmiş sipariş nesnesini oluştur ───────────────
                var updatedOrder = BuildUpdatedOrderFromForm(existingOrder);

                // ── Değişiklikleri karşılaştır ve fark listesi oluştur ───────────
                var changes = BuildChangeLog(existingOrder, updatedOrder, isTurkish);

                if (changes.Count == 0)
                {
                    string noChangeMsg = isTurkish
                        ? "Herhangi bir değişiklik yapılmadı."
                        : "No changes were made.";
                    MessageBox.Show(noChangeMsg,
                        isTurkish ? "Bilgi" : "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ── Onay diyaloğu: hangi alanlar değişti? ─
                string changeListText = string.Join(Environment.NewLine, changes.Select(c => $"  • {c}"));
                string confirmMsg = isTurkish
                    ? $"Aşağıdaki değişiklikler uygulanacak:{Environment.NewLine}{Environment.NewLine}{changeListText}{Environment.NewLine}{Environment.NewLine}Güncellemek istiyor musunuz?"
                    : $"The following changes will be applied:{Environment.NewLine}{Environment.NewLine}{changeListText}{Environment.NewLine}{Environment.NewLine}Do you want to update?";
                string confirmCaption = isTurkish ? "Güncelleme Onayı" : "Update Confirmation";

                var result = MessageBox.Show(confirmMsg, confirmCaption,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                // ── Güncelleme işlemini yap ─
                _isLoading = true;
                bool success;
                try
                {
                    success = _orderService.UpdateOrder(updatedOrder);
                }
                finally
                {
                    _isLoading = false;
                }

                if (success)
                {
                    string successMsg = isTurkish
                        ? "Sipariş başarıyla güncellendi."
                        : "Order successfully updated.";
                    MessageBox.Show(successMsg,
                        isTurkish ? "Başarılı" : "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Combobox'ı yenile ve formu sıfırla
                    LoadOrdersIntoComboBox();
                    ClearFormFields();
                }
                else
                {
                    string failMsg = isTurkish
                        ? "Sipariş güncellenirken bir hata oluştu."
                        : "An error occurred while updating the order.";
                    MessageBox.Show(failMsg,
                        isTurkish ? "Hata" : "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private Order BuildUpdatedOrderFromForm(Order existing)
        {
            var updated = new Order
            {
                ID         = existing.ID,
                AddedBy    = existing.AddedBy,

                CustomerName        = txtCustomerName.Text.Trim(),
                CustomerPhone       = mskPhoneNo.Text.Trim(),
                OrderDate           = dtpOrder.Value,
                PayableStatues      = cmbPayment.SelectedItem?.ToString() ?? "",
                OrderStatues        = cmbOrderStatus.SelectedItem?.ToString() ?? "",
                CargoTrackingNumber = txtCargoTracker.Text.Trim(),
                CargoCost           = nmrCargo.Value,
            };

            // Kargo şirketi: "Diğer/Other" seçiliyse txtCargo'dan al, yoksa combobox'tan
            bool isOtherCargoSelected =
                cmbCargo.SelectedIndex == cmbCargo.Items.Count - 1 &&
                (cmbCargo.SelectedItem?.ToString() == "Diğer" || cmbCargo.SelectedItem?.ToString() == "Other");

            updated.Cargo = isOtherCargoSelected
                ? txtCargo.Text.Trim()
                : cmbCargo.SelectedItem?.ToString() ?? "";

            // İndirim yüzdesi
            decimal discPerc = 0;
            if (cmbDisc.SelectedIndex > 0)
            {
                var m = System.Text.RegularExpressions.Regex.Match(
                    cmbDisc.SelectedItem.ToString(), @"\d+");
                if (m.Success) decimal.TryParse(m.Value, out discPerc);
            }
            updated.DiscountPercentage  = discPerc;
            updated.ExtraDiscountAmount = nmrDisc.Value;

            // ÖTV
            decimal sctPerc = 0;
            if (cmbSCT.SelectedIndex > 0)
            {
                var m = System.Text.RegularExpressions.Regex.Match(
                    cmbSCT.SelectedItem.ToString(), @"\d+");
                if (m.Success) decimal.TryParse(m.Value, out sctPerc);
            }
            updated.TaxPercentageSCT = sctPerc;

            // KDV
            decimal vatPerc = 0;
            if (cmbVAT.SelectedIndex > 0)
            {
                var m = System.Text.RegularExpressions.Regex.Match(
                    cmbVAT.SelectedItem.ToString(), @"\d+");
                if (m.Success) decimal.TryParse(m.Value, out vatPerc);
            }
            updated.TaxPercentageVAT = vatPerc;

            // Vergi toplamı (Tax) — ÖTV + KDV üzerinden basit hesap
            updated.Tax = existing.Tax; // Değişmemişse eskiyi koru

            // TotalPrice, TotalDiscount, PayableAmount — mevcut Dictionary'leri koru
            updated.TotalPrice      = existing.TotalPrice;
            updated.TotalDiscount   = existing.TotalDiscount;
            updated.PayableAmount   = existing.PayableAmount;

            // Ürün listesi — dgwProducts'tan oku
            updated.OrderContent = new List<Order.OrderDetail>();
            foreach (DataGridViewRow row in dgwProducts.Rows)
            {
                if (row.Cells["Name"].Value == null) continue;
                string productName = row.Cells["Name"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(productName)) continue;

                decimal.TryParse(row.Cells["Price"].Value?.ToString(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.CurrentCulture, out decimal price);
                int.TryParse(row.Cells["OrderQuantity"].Value?.ToString(), out int qty);
                decimal.TryParse(row.Cells["Total"].Value?.ToString(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.CurrentCulture, out decimal total);
                string currency = row.Cells["PriceCurrency"].Value?.ToString() ?? "Türk Lirası";

                updated.OrderContent.Add(new Order.OrderDetail
                {
                    ProductName = productName,
                    OrderPrice  = price,
                    Quantity    = qty,
                    Total       = total,
                    Currency    = currency
                });
            }

            return updated;
        }

        private List<string> BuildChangeLog(Order old, Order updated, bool isTurkish)
        {
            var changes = new List<string>();

            // ── Müşteri Adı
            if (!string.Equals(old.CustomerName, updated.CustomerName, StringComparison.OrdinalIgnoreCase))
            {
                string label = isTurkish ? "Müşteri Adı" : "Customer Name";
                changes.Add($"{label}: \"{old.CustomerName}\" → \"{updated.CustomerName}\"");
            }

            // ── Müşteri Telefon
            if (!string.Equals(old.CustomerPhone, updated.CustomerPhone, StringComparison.OrdinalIgnoreCase))
            {
                string label = isTurkish ? "Telefon" : "Phone";
                changes.Add($"{label}: \"{old.CustomerPhone}\" → \"{updated.CustomerPhone}\"");
            }

            // ── Sipariş Tarihi
            if (old.OrderDate.Date != updated.OrderDate.Date)
            {
                string fmt = isTurkish ? "dd.MM.yyyy" : "MM/dd/yyyy";
                string label = isTurkish ? "Sipariş Tarihi" : "Order Date";
                changes.Add($"{label}: {old.OrderDate.ToString(fmt)} → {updated.OrderDate.ToString(fmt)}");
            }

            // ── Ödeme Durumu
            if (!string.Equals(old.PayableStatues, updated.PayableStatues, StringComparison.OrdinalIgnoreCase))
            {
                string label = isTurkish ? "Ödeme Durumu" : "Payment Status";
                changes.Add($"{label}: \"{old.PayableStatues}\" → \"{updated.PayableStatues}\"");
            }

            // ── Sipariş Durumu
            if (!string.Equals(old.OrderStatues, updated.OrderStatues, StringComparison.OrdinalIgnoreCase))
            {
                string label = isTurkish ? "Sipariş Durumu" : "Order Status";
                changes.Add($"{label}: \"{old.OrderStatues}\" → \"{updated.OrderStatues}\"");
            }

            // ── Kargo Şirketi
            if (!string.Equals(old.Cargo, updated.Cargo, StringComparison.OrdinalIgnoreCase))
            {
                string label = isTurkish ? "Kargo Şirketi" : "Cargo Company";
                changes.Add($"{label}: \"{old.Cargo}\" → \"{updated.Cargo}\"");
            }

            // ── Kargo Takip No
            if (!string.Equals(old.CargoTrackingNumber, updated.CargoTrackingNumber, StringComparison.OrdinalIgnoreCase))
            {
                string label = isTurkish ? "Kargo Takip No" : "Cargo Tracking Number";
                changes.Add($"{label}: \"{old.CargoTrackingNumber}\" → \"{updated.CargoTrackingNumber}\"");
            }

            // ── Kargo Bedeli
            if (old.CargoCost != updated.CargoCost)
            {
                string label = isTurkish ? "Kargo Bedeli" : "Cargo Cost";
                changes.Add($"{label}: {old.CargoCost:N2} → {updated.CargoCost:N2}");
            }

            // ── İndirim Yüzdesi
            if (old.DiscountPercentage != updated.DiscountPercentage)
            {
                string label = isTurkish ? "İndirim Yüzdesi" : "Discount Percentage";
                changes.Add($"{label}: %{old.DiscountPercentage} → %{updated.DiscountPercentage}");
            }

            // ── Ekstra İndirim Tutarı
            if (old.ExtraDiscountAmount != updated.ExtraDiscountAmount)
            {
                string label = isTurkish ? "Ekstra İndirim Tutarı" : "Extra Discount Amount";
                changes.Add($"{label}: {old.ExtraDiscountAmount:N2} → {updated.ExtraDiscountAmount:N2}");
            }

            // ── ÖTV Yüzdesi
            if (old.TaxPercentageSCT != updated.TaxPercentageSCT)
            {
                string label = isTurkish ? "ÖTV Oranı" : "SCT Rate";
                changes.Add($"{label}: %{old.TaxPercentageSCT} → %{updated.TaxPercentageSCT}");
            }

            // ── KDV Yüzdesi
            if (old.TaxPercentageVAT != updated.TaxPercentageVAT)
            {
                string label = isTurkish ? "KDV Oranı" : "VAT Rate";
                changes.Add($"{label}: %{old.TaxPercentageVAT} → %{updated.TaxPercentageVAT}");
            }

            // ── Ürün listesi (adet ve fiyat değişimleri)
            var oldMap = old.OrderContent.ToDictionary(x => x.ProductName, x => x);
            var newMap = updated.OrderContent.ToDictionary(x => x.ProductName, x => x);

            // Silinen ürünler
            foreach (var key in oldMap.Keys.Except(newMap.Keys))
            {
                string label = isTurkish ? "Ürün Silindi" : "Product Removed";
                changes.Add($"{label}: \"{key}\"");
            }

            // Eklenen ürünler
            foreach (var key in newMap.Keys.Except(oldMap.Keys))
            {
                var nd = newMap[key];
                string label = isTurkish ? "Yeni Ürün Eklendi" : "Product Added";
                changes.Add($"{label}: \"{key}\" (x{nd.Quantity} @ {nd.OrderPrice:N2} {nd.Currency})");
            }

            // Değişen ürünler (adet, fiyat, para birimi)
            foreach (var key in oldMap.Keys.Intersect(newMap.Keys))
            {
                var od = oldMap[key];
                var nd = newMap[key];

                if (od.Quantity != nd.Quantity)
                {
                    string label = isTurkish ? "Sipariş Adeti" : "Order Quantity";
                    changes.Add($"\"{key}\" - {label}: {od.Quantity} → {nd.Quantity}");
                }

                if (od.OrderPrice != nd.OrderPrice)
                {
                    string label = isTurkish ? "Birim Fiyat" : "Unit Price";
                    changes.Add($"\"{key}\" - {label}: {od.OrderPrice:N2} → {nd.OrderPrice:N2}");
                }

                if (!string.Equals(od.Currency, nd.Currency, StringComparison.OrdinalIgnoreCase))
                {
                    string label = isTurkish ? "Para Birimi" : "Currency";
                    changes.Add($"\"{key}\" - {label}: {od.Currency} → {nd.Currency}");
                }
            }

            return changes;
        }

        // Formu temizlemek için yardımcı metod
        private void ClearFormFields()
        {
            _isLoading = true; // Temizleme sırasında ValueChanged/SelectedIndexChanged onay diyaloglarını engelle
            // Metin ve sayısal alanları temizle
            txtCustomerName.Clear();
            mskPhoneNo.Clear();
            txtCargoTracker.Clear();
            txtCargo.Clear();
            nmrDisc.Value = 0.00M; // İndirim tutarını sıfırla
            nmrCargo.Value = 0;

            // Tabloları temizle
            dgwProducts.Rows.Clear();
            lwDisc.Items.Clear();
            lwTotal.Items.Clear();
            lwDiscList.Items.Clear();
            lwTax.Items.Clear();

            // ComboBox seçimlerini sıfırla
            cmbOrders.SelectedIndex = -1;
            cmbDisc.SelectedIndex = -1;
            cmbPayment.SelectedIndex = 0;
            cmbOrderStatus.SelectedIndex = 0;

            // DateTimePicker'ı bugüne sıfırla
            dtpOrder.Value = DateTime.Now;

            // RadioButton seçimlerini kaldır (Yöntem grubunu sıfırla)
            rbRead.Checked = false;
            rbEdit.Checked = false;
            rbDelete.Checked = false;

            // Form yüksekliğini ve kontrollerin aktiflik durumunu resetle
            this.Height = 130;
            ApplyOrderSelectionState();
            _isLoading = false; // Temizleme tamamlandı
        }


        private void UpdateCargoFieldsAccessibility()
        {
            // Sadece Düzenleme (rbEdit) modunda ve ComboBox doluysa bu kontroller anlamlıdır
            if (!rbEdit.Checked || cmbOrderStatus.Items.Count == 0) return;

            // 1. Kural: Ödeme Durumu "Ödeme Alınmadı" (Index 0) ise her şey kapalı
            if (cmbPayment.SelectedIndex == 0)
            {
                cmbOrderStatus.Enabled = false;
                cmbCargo.Enabled = false;
                txtCargo.Enabled = false;
                txtCargoTracker.Enabled = false;

                // Ödeme alınmadıysa sipariş durumunu "Siparişe Başlanmadı"ya çekebilirsin (isteğe bağlı)
                cmbOrderStatus.SelectedIndex = 0;
            }
            // 2. Kural: Ödeme Durumu "Ödeme Alındı" (Index 1) ise
            else if (cmbPayment.SelectedIndex == 1)
            {
                cmbOrderStatus.Enabled = true;

                // 3. Kural: Sipariş Durumuna göre kargo kontrolleri
                // 0: Başlanmadı/Not Started, 1: Hazırlanıyor, 2: Kargoya Verilecek
                if (cmbOrderStatus.SelectedIndex >= 0 && cmbOrderStatus.SelectedIndex <= 2)
                {
                    cmbCargo.Enabled = false;
                    txtCargo.Enabled = false;
                    txtCargoTracker.Enabled = false;
                }
                // 3. ve 4. Index: "Kargolandı" veya "Bitti"
                else if (cmbOrderStatus.SelectedIndex == 3 || cmbOrderStatus.SelectedIndex == 4)
                {
                    if (!string.IsNullOrWhiteSpace(LoggedInUser?.Cargo))
                    {
                        cmbCargo.Enabled = true;

                        // Eğer "Diğer/Other" (yani 1. index) seçiliyse txtCargo'yu aç, değilse (0. index) kapat
                        txtCargo.Enabled = (cmbCargo.SelectedIndex == 1);
                    }
                    else
                    {
                        // Kullanıcının kargosu yoksa ComboBox hep kapalı, TextBox hep açık
                        cmbCargo.Enabled = false;
                        txtCargo.Enabled = true;
                    }

                    // Takip numarası her iki durumda da (Kargolandı/Bitti) aktif olmalı
                    txtCargoTracker.Enabled = true;
                }
            }
        }

        private void cmbPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCargoFieldsAccessibility();
        }

        private void cmbOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCargoFieldsAccessibility();
        }

        private async void CurrencyChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb == null) return;

            string newCurrency = cb.SelectedItem?.ToString();
            string oldCurrency = _previousProductName;

            if (string.IsNullOrEmpty(oldCurrency) || newCurrency == oldCurrency)
                return;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // CheckBox kontrolünü bulalım
            var chbPriceLock = this.Controls.Find("chbPriceLock", true).FirstOrDefault() as CheckBox;
            bool isLocked = chbPriceLock != null && chbPriceLock.Checked;

            string message = "";
            if (isLocked)
            {
                message = isTurkish
                    ? $"Fiyat kilidi açık. '{oldCurrency}' olan para birimi '{newCurrency}' olarak güncellenecek ancak birim fiyatlar DEĞİŞMEYECEKTİR. Onaylıyor musunuz?"
                    : $"Price lock is active. Currency will change from '{oldCurrency}' to '{newCurrency}' but unit prices will NOT change. Do you confirm?";
            }
            else
            {
                message = isTurkish
                    ? $"Para birimini '{oldCurrency}' -> '{newCurrency}' olarak değiştirmek üzeresiniz. Birim fiyatlar kur farkına göre otomatik DÖNÜŞTÜRÜLECEKTİR. Onaylıyor musunuz?"
                    : $"Changing currency from '{oldCurrency}' to '{newCurrency}'. Unit prices will be automatically CONVERTED. Do you confirm?";
            }

            if (MessageBox.Show(message, isTurkish ? "Para Birimi Güncelleme" : "Currency Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                cb.SelectedIndexChanged -= CurrencyChanged;
                cb.SelectedItem = oldCurrency;
                cb.SelectedIndexChanged += CurrencyChanged;
                return;
            }

            try
            {
                // Fiyat kilidi kapalıysa kur dönüşümü yap
                if (!isLocked)
                {
                    var forexService = new neoStockMasterv2.Data.Services.BankServices.FREEMARKETforex();
                    var rates = await forexService.GetExchangeRatesAsync();

                    decimal oldRateToTL = GetRateToTL(oldCurrency, rates);
                    decimal newRateToTL = GetRateToTL(newCurrency, rates);

                    if (dgwProducts.CurrentRow != null)
                    {
                        var row = dgwProducts.CurrentRow;
                        if (row.Cells["Price"].Value != null)
                        {
                            decimal currentPrice = Convert.ToDecimal(row.Cells["Price"].Value);
                            decimal newPrice = (currentPrice * oldRateToTL) / newRateToTL;
                            row.Cells["Price"].Value = Math.Round(newPrice, 2);
                        }
                    }
                }

                _currentCurrency = newCurrency; // Yeni birimi kaydet

                //Hücre henüz commit olmadığından yeni para birimini elle grid'e yaz
                if (dgwProducts.CurrentRow != null)
                {
                    dgwProducts.CurrentRow.Cells["PriceCurrency"].Value = newCurrency;
                }

                // Düzenleme modundan çık ki hücre değerleri kesinleşsin
                dgwProducts.EndEdit();

                // Yeni para birimini hücreye elle yaz ve edit modundan çık
                if (dgwProducts.CurrentRow != null)
                    dgwProducts.CurrentRow.Cells["PriceCurrency"].Value = newCurrency;

                dgwProducts.EndEdit();

                // lwDisc'teki ilgili ürünün para birimi sütununu (SubItems[3]) güncelle
                if (dgwProducts.CurrentRow != null)
                {
                    string changedProductName = dgwProducts.CurrentRow.Cells["Name"].Value?.ToString();
                    if (!string.IsNullOrEmpty(changedProductName))
                    {
                        foreach (ListViewItem lwItem in lwDisc.Items)
                        {
                            // SubItems[1] = Ürün Adı
                            if (lwItem.SubItems[1].Text == changedProductName)
                            {
                                // SubItems[3] = Para Birimi
                                lwItem.SubItems[3].Text = newCurrency;
                                break;
                            }
                        }
                    }
                }

                UpdateTotalAmounts(preserveRates: true); // Listeleri ve toplamları yeni birimle tazele
                _cachedRates = null; // Yeni kur çekimi için cache'i sıfırla
            }
            catch (Exception ex)
            {
                MessageBox.Show(isTurkish ? "Hata: " + ex.Message : "Error: " + ex.Message);
            }
        }

        private async void UpdateSummaryListViews(object _unused1, object _unused2, object _unused3)
        {
            bool isEng = LanguageService.CurrentLanguage == "English";

            // ── Para birimine göre gruplu toplamlar
            // key: para birimi adı  value: (araToplamHam, finalToplamIndirimliVergi, toplamIndirim, toplamVergi)
            var groupedTotals = new Dictionary<string, (decimal SubTotal, decimal FinalTotal, decimal Discount, decimal Tax)>();

            foreach (ListViewItem lwItem in lwDisc.Items)
            {
                // Para birimini dgwProducts'tan al (SubItems[3] = para birimi sütunu lwDisc'te)
                string currency = lwItem.SubItems.Count > 3 ? lwItem.SubItems[3].Text : "Türk Lirası";
                if (string.IsNullOrWhiteSpace(currency)) currency = "Türk Lirası";

                // Ham toplam — Tag'den oku, string parse hatası yok
                decimal rawTotal = lwItem.Tag is decimal tagVal ? tagVal : 0m;

                // Final tutar = rawTotal ile başla (Tag zaten güncel değeri tutar)
                decimal finalVal = rawTotal;

                // İndirim tutarı
                decimal percD = 0, fixD = 0;
                var pm = System.Text.RegularExpressions.Regex.Match(
                    lwItem.SubItems.Count > 4 ? lwItem.SubItems[4].Text : "", @"%(\d+)");
                if (pm.Success) { decimal.TryParse(pm.Groups[1].Value, out percD); percD /= 100m; }
                if (lwItem.SubItems.Count > 5)
                    decimal.TryParse(lwItem.SubItems[5].Text,
                        System.Globalization.NumberStyles.Any,
                        new System.Globalization.CultureInfo("tr-TR"), out fixD);
                decimal discountVal = rawTotal * percD + fixD;

                // Vergi tutarı (ÖTV + KDV parantez içinden) — her zaman TL cinsinden
                decimal taxVal = 0;
                if (lwItem.SubItems.Count > 6)
                {
                    string sctText = lwItem.SubItems[6].Text;
                    int so = sctText.IndexOf('('), sc = sctText.IndexOf(')');
                    if (so >= 0 && sc > so)
                    {
                        // "TL" veya boşluk gibi son ekleri temizle
                        string num = sctText.Substring(so + 1, sc - so - 1)
                                            .TrimStart('+').Replace("TL", "").Trim();
                        if (decimal.TryParse(num, System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.CurrentCulture, out decimal sv)) taxVal += sv;
                    }
                }
                if (lwItem.SubItems.Count > 7)
                {
                    string vatText = lwItem.SubItems[7].Text;
                    int vo = vatText.IndexOf('('), vc = vatText.IndexOf(')');
                    if (vo >= 0 && vc > vo)
                    {
                        string num = vatText.Substring(vo + 1, vc - vo - 1)
                                            .TrimStart('+').Replace("TL", "").Trim();
                        if (decimal.TryParse(num, System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.CurrentCulture, out decimal vv)) taxVal += vv;
                    }
                }

                if (!groupedTotals.ContainsKey(currency))
                    groupedTotals[currency] = (0, 0, 0, 0);

                var g = groupedTotals[currency];
                groupedTotals[currency] = (
                    g.SubTotal + rawTotal,
                    g.FinalTotal + finalVal,
                    g.Discount + discountVal,
                    g.Tax + taxVal
                );
            }

            // Hiç satır yoksa dgwProducts'tan en azından ham toplamı göster
            if (groupedTotals.Count == 0)
            {
                foreach (DataGridViewRow row in dgwProducts.Rows)
                {
                    string currency = row.Cells["PriceCurrency"].Value?.ToString() ?? "Türk Lirası";
                    if (string.IsNullOrWhiteSpace(currency)) currency = "Türk Lirası";

                    decimal rawTotal = 0;
                    decimal.TryParse(row.Cells["Total"].Value?.ToString(),
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.CurrentCulture, out rawTotal);

                    if (!groupedTotals.ContainsKey(currency))
                        groupedTotals[currency] = (0, 0, 0, 0);
                    var g = groupedTotals[currency];
                    groupedTotals[currency] = (g.SubTotal + rawTotal, g.FinalTotal + rawTotal, 0, 0);
                }
            }

            // ── lwTotal: Para Birimi | Tutar
            if (lwTotal != null)
            {
                lwTotal.Items.Clear();
                foreach (var kvp in groupedTotals)
                {
                    var item = new ListViewItem(kvp.Key);
                    item.SubItems.Add(kvp.Value.FinalTotal.ToString("N2"));
                    item.Font = new Font(lwTotal.Font, FontStyle.Bold);
                    lwTotal.Items.Add(item);
                }
            }

            // ── lwDiscList: Para Birimi | İndirim Tutarı
            if (lwDiscList != null)
            {
                lwDiscList.Items.Clear();
                foreach (var kvp in groupedTotals)
                {
                    if (kvp.Value.Discount <= 0) continue;
                    var item = new ListViewItem(kvp.Key);
                    item.SubItems.Add("-" + kvp.Value.Discount.ToString("N2"));
                    item.ForeColor = Color.Red;
                    lwDiscList.Items.Add(item);
                }
            }

            // ── lwTax: ÖTV (TL) | KDV (TL) | TOPLAM(TL)
            if (lwTax != null)
            {
                lwTax.Items.Clear();

                // Döviz kurlarını bir kez al (cache kullan)
                if (_cachedRates == null)
                {
                    try
                    {
                        var forexService = new neoStockMasterv2.Data.Services.BankServices.FREEMARKETforex();
                        _cachedRates = await forexService.GetExchangeRatesAsync();
                    }
                    catch
                    {
                        _cachedRates = new Dictionary<string, (decimal, decimal)>(); // boş = TL fallback
                    }
                }

                bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

                // Tüm grupları gez; ÖTV ve KDV toplamlarını tek değişkende biriktir
                decimal totalSctTL = 0m;
                decimal totalVatTL = 0m;

                foreach (var kvp in groupedTotals)
                {
                    if (kvp.Value.Tax <= 0) continue;

                    decimal rateToTL = GetRateToTL(kvp.Key, _cachedRates);

                    foreach (ListViewItem lwItem in lwDisc.Items)
                    {
                        string itemCurrency = lwItem.SubItems.Count > 3 ? lwItem.SubItems[3].Text : "Türk Lirası";
                        if (string.IsNullOrWhiteSpace(itemCurrency)) itemCurrency = "Türk Lirası";
                        if (itemCurrency != kvp.Key) continue;

                        // ÖTV tutarını SubItems[6]'dan parantez içinden oku
                        // Değer zaten TL cinsinden yazılmış — rateToTL ile çarpma!
                        if (lwItem.SubItems.Count > 6)
                        {
                            string sctText = lwItem.SubItems[6].Text;
                            int so = sctText.IndexOf('('), sc = sctText.IndexOf(')');
                            if (so >= 0 && sc > so)
                            {
                                string num = sctText.Substring(so + 1, sc - so - 1)
                                                    .TrimStart('+').Replace("TL", "").Trim();
                                if (decimal.TryParse(num, System.Globalization.NumberStyles.Any,
                                    System.Globalization.CultureInfo.CurrentCulture, out decimal sv))
                                    totalSctTL += sv; // Zaten TL — kur çarpmaya gerek yok
                            }
                        }

                        // KDV tutarını SubItems[7]'den parantez içinden oku
                        if (lwItem.SubItems.Count > 7)
                        {
                            string vatText = lwItem.SubItems[7].Text;
                            int vo = vatText.IndexOf('('), vc = vatText.IndexOf(')');
                            if (vo >= 0 && vc > vo)
                            {
                                string num = vatText.Substring(vo + 1, vc - vo - 1)
                                                    .TrimStart('+').Replace("TL", "").Trim();
                                if (decimal.TryParse(num, System.Globalization.NumberStyles.Any,
                                    System.Globalization.CultureInfo.CurrentCulture, out decimal vv))
                                    totalVatTL += vv; // Zaten TL — kur çarpmaya gerek yok
                            }
                        }
                    }
                }

                // Tek ÖTV satırı — tüm ürünlerin ÖTV toplamı
                if (totalSctTL > 0)
                {
                    string sctLabel = isTurkish ? "ÖTV" : "SCT";
                    var sctItem = new ListViewItem(sctLabel);
                    sctItem.SubItems.Add(totalSctTL.ToString("N2"));
                    lwTax.Items.Add(sctItem);
                }

                // Tek KDV satırı — tüm ürünlerin KDV toplamı
                if (totalVatTL > 0)
                {
                    string vatLabel = isTurkish ? "KDV" : "VAT";
                    var vatItem = new ListViewItem(vatLabel);
                    vatItem.SubItems.Add(totalVatTL.ToString("N2"));
                    lwTax.Items.Add(vatItem);
                }

                // TOPLAM(TL) satırı — tüm grupların TL toplamı
                decimal grandTaxTL = totalSctTL + totalVatTL;
                if (grandTaxTL > 0)
                {
                    string totalLabel = isTurkish ? "TOPLAM(TL)" : "TOTAL(TRY)";
                    var totalItem = new ListViewItem(totalLabel);
                    totalItem.SubItems.Add(grandTaxTL.ToString("N2"));
                    totalItem.Font = new Font(lwTax.Font, FontStyle.Bold);
                    lwTax.Items.Add(totalItem);
                }
            }
        }


        // Toplam tutarları güncelleyen yardımcı metot
        private async void UpdateTotalAmounts(bool preserveRates = false)
        {
            // 1. Ürün bazlı ham toplamları dgwProducts'tan hesapla ve Total hücresine yaz
            for (int i = 0; i < dgwProducts.Rows.Count; i++)
            {
                DataGridViewRow row = dgwProducts.Rows[i];
                if (row.Cells["Price"].Value == null || row.Cells["OrderQuantity"].Value == null) continue;

                if (!decimal.TryParse(row.Cells["Price"].Value?.ToString(), out decimal price)) continue;
                if (!decimal.TryParse(row.Cells["OrderQuantity"].Value?.ToString(), out decimal qty)) continue;

                decimal lineTotal = price * qty;
                row.Cells["Total"].Value = lineTotal;
            }

            // 2. İndirim ve vergi oranlarını oku
            decimal percentageRate = 0;
            if (cmbDisc.SelectedIndex > 0)
            {
                var match = System.Text.RegularExpressions.Regex.Match(
                    cmbDisc.SelectedItem.ToString(), @"\d+");
                if (match.Success) decimal.TryParse(match.Value, out percentageRate);
            }

            decimal fixedAmountDisc = nmrDisc.Value;

            decimal sctRate = 0;
            if (cmbSCT.SelectedIndex > 0)
            {
                var match = System.Text.RegularExpressions.Regex.Match(
                    cmbSCT.SelectedItem.ToString(), @"\d+");
                if (match.Success) decimal.TryParse(match.Value, out sctRate);
            }

            decimal vatRate = 0;
            if (cmbVAT.SelectedIndex > 0)
            {
                var match = System.Text.RegularExpressions.Regex.Match(
                    cmbVAT.SelectedItem.ToString(), @"\d+");
                if (match.Success) decimal.TryParse(match.Value, out vatRate);
            }

            // 3. Döviz kurlarını önceden çek (dövizli+işaretli satır varsa)
            bool needsExchange = lwDisc.Items.Cast<ListViewItem>()
                .Any(it => it.Checked && !IsTL(it.SubItems.Count > 3 ? it.SubItems[3].Text : ""));

            if (needsExchange && _cachedRates == null)
            {
                try
                {
                    var forexService = new neoStockMasterv2.Data.Services.BankServices.FREEMARKETforex();
                    _cachedRates = await forexService.GetExchangeRatesAsync();
                }
                catch
                {
                    _cachedRates = new Dictionary<string, (decimal, decimal)>();
                }
            }

            // 4. lwDisc satırlarını güncelle
            UpdateLwDiscItems(percentageRate / 100m, fixedAmountDisc, sctRate, vatRate,
                              applyRates: !preserveRates, exchangeRates: _cachedRates);

            // 5. Özet tablolarını lwDisc'ten yeniden hesapla (multi-currency gruplu)
            UpdateSummaryListViews(null, null, null);
        }

        // Para biriminin TL olup olmadığını kontrol eder (PricingOrderScreen ile aynı mantık)
        private bool IsTL(string currency)
        {
            return string.IsNullOrWhiteSpace(currency) ||
                   currency.Equals("Türk Lirası", StringComparison.OrdinalIgnoreCase) ||
                   currency.Equals("TRY", StringComparison.OrdinalIgnoreCase) ||
                   currency.Equals("TL", StringComparison.OrdinalIgnoreCase) ||
                   currency.Equals("₺", StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateLwDiscItems(decimal percRate, decimal fixDiscPerLine,
                                       decimal sctRate, decimal vatRate,
                                       bool applyRates = true,
                                       Dictionary<string, (decimal BuyRate, decimal SellRate)> exchangeRates = null)
        {
            if (lwDisc == null) return;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            foreach (ListViewItem item in lwDisc.Items)
            {
                string productName = item.SubItems[1].Text;
                DataGridViewRow row = dgwProducts.Rows.Cast<DataGridViewRow>()
                                      .FirstOrDefault(r => r.Cells["Name"].Value?.ToString() == productName);

                if (row == null) continue;

                // Para birimini al (SubItems[3])
                string itemCurrency = item.SubItems.Count > 3 ? item.SubItems[3].Text : "Türk Lirası";
                if (string.IsNullOrWhiteSpace(itemCurrency)) itemCurrency = "Türk Lirası";
                bool isForeignCurrency = !IsTL(itemCurrency);

                // Kur çarpanını hesapla: dövizliyse kur çarp, TL ise 1
                decimal rateToTL = 1m;
                if (isForeignCurrency && exchangeRates != null)
                    rateToTL = GetRateToTL(itemCurrency, exchangeRates);

                // Tag'de decimal varsa direkt oku; yoksa hücreden parse et
                decimal originalAmount = 0;
                if (item.Tag is decimal tagAmt)
                    originalAmount = tagAmt;
                else if (row.Cells["Total"].Value is decimal cellDec)
                    originalAmount = cellDec;
                else
                    decimal.TryParse(row.Cells["Total"].Value?.ToString(),
                        System.Globalization.NumberStyles.Any,
                        new System.Globalization.CultureInfo("tr-TR"),
                        out originalAmount);

                if (!item.Checked)
                {
                    // Seçili olmayan satır: SubItems'taki mevcut oranlarla tam hesaplama yap
                    decimal uPercRate = 0, uFixDisc = 0, uSctRate = 0, uVatRate = 0;

                    var pm2 = System.Text.RegularExpressions.Regex.Match(item.SubItems[4].Text, @"%(\d+)");
                    if (pm2.Success) { decimal.TryParse(pm2.Groups[1].Value, out uPercRate); uPercRate /= 100m; }

                    decimal.TryParse(item.SubItems[5].Text,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.CurrentCulture, out uFixDisc);

                    var sm2 = System.Text.RegularExpressions.Regex.Match(item.SubItems[6].Text, @"%(\d+)");
                    if (sm2.Success) decimal.TryParse(sm2.Groups[1].Value, out uSctRate);

                    var vm2 = System.Text.RegularExpressions.Regex.Match(item.SubItems[7].Text, @"%(\d+)");
                    if (vm2.Success) decimal.TryParse(vm2.Groups[1].Value, out uVatRate);

                    decimal uPercDisc = originalAmount * uPercRate;
                    decimal uAfterDisc = originalAmount - uPercDisc - uFixDisc;
                    if (uAfterDisc < 0) uAfterDisc = 0;

                    // Vergi TL cinsinden hesaplanır
                    decimal uSctVal_TL = uAfterDisc * rateToTL * (uSctRate / 100m);
                    decimal uVatVal_TL = (uAfterDisc * rateToTL + uSctVal_TL) * (uVatRate / 100m);
                    decimal uFinal = uAfterDisc + (uSctVal_TL + uVatVal_TL) / rateToTL;

                    item.SubItems[2].Text = uFinal.ToString("N2");
                    // NOT: item.Tag güncellenmez — ham tutar korunur.

                    item.SubItems[4].Text = uPercRate > 0
                        ? $"%{uPercRate * 100:0} ({uPercDisc:N2})"
                        : (isTurkish ? "İndirim Yok" : "No Discount");

                    item.SubItems[5].Text = uFixDisc.ToString("N2");

                    item.SubItems[6].Text = uSctRate > 0
                        ? $"%{uSctRate} ({uSctVal_TL:N2} TL)"
                        : (isTurkish ? "ÖTV Yok" : "No Excise Tax");

                    item.SubItems[7].Text = uVatRate > 0
                        ? $"%{uVatRate} ({uVatVal_TL:N2} TL)"
                        : (isTurkish ? "KDV Yok" : "No VAT");

                    continue;
                }

                // applyRates=false ise mevcut SubItems metinlerinden oranları parse et
                decimal usePercRate = percRate;
                decimal useFixDisc = fixDiscPerLine;
                decimal useSctRate = sctRate;
                decimal useVatRate = vatRate;

                if (!applyRates)
                {
                    // İndirim Durumu sütunundan (SubItems[4]) yüzde oranını çıkar
                    usePercRate = 0;
                    useFixDisc = 0;
                    string discStatus = item.SubItems[4].Text;
                    var percMatch = System.Text.RegularExpressions.Regex.Match(discStatus, @"%(\d+)");
                    if (percMatch.Success)
                        decimal.TryParse(percMatch.Groups[1].Value, out usePercRate);
                    usePercRate /= 100m; // % → oran

                    // Sabit indirim: SubItems[5]'ten doğrudan oku
                    decimal.TryParse(item.SubItems[5].Text,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.CurrentCulture,
                        out useFixDisc);

                    // ÖTV oranı: SubItems[6] → "%20 (...)"
                    useSctRate = 0;
                    var sctMatch = System.Text.RegularExpressions.Regex.Match(
                        item.SubItems[6].Text, @"%(\d+)");
                    if (sctMatch.Success)
                        decimal.TryParse(sctMatch.Groups[1].Value, out useSctRate);

                    // KDV oranı: SubItems[7] → "%18 (...)"
                    useVatRate = 0;
                    var vatMatch = System.Text.RegularExpressions.Regex.Match(
                        item.SubItems[7].Text, @"%(\d+)");
                    if (vatMatch.Success)
                        decimal.TryParse(vatMatch.Groups[1].Value, out useVatRate);
                }

                // --- HESAPLAMA SIRALAMASI ---
                // 1. Yüzde İndirimi
                decimal percDiscVal = originalAmount * usePercRate;

                // 2. Sabit Tutar İndirimi
                decimal totalLineDiscount = percDiscVal + useFixDisc;
                decimal priceAfterDiscount = originalAmount - totalLineDiscount;
                if (priceAfterDiscount < 0) priceAfterDiscount = 0;

                // 3. ÖTV — dövizse önce TL'ye çevir, vergiyi TL üzerinden hesapla
                decimal sctVal_TL = priceAfterDiscount * rateToTL * (useSctRate / 100m);

                // 4. KDV — (İndirimli Tutar + ÖTV) TL üzerinden
                decimal vatVal_TL = (priceAfterDiscount * rateToTL + sctVal_TL) * (useVatRate / 100m);

                // 5. Final tutar: döviz kısmı orijinal birimde + vergiler orijinal birime geri çevrilmiş
                decimal finalLineTotal = priceAfterDiscount + (sctVal_TL + vatVal_TL) / rateToTL;

                // --- GÖRSEL GÜNCELLEME ---
                item.SubItems[2].Text = finalLineTotal.ToString("N2");
                // NOT: item.Tag kasıtlı olarak güncellenmez — ham tutar korunur.

                // --- GÖRSEL FORMAT ---
                if (usePercRate > 0)
                    item.SubItems[4].Text = $"%{usePercRate * 100:0} ({percDiscVal:N2})";
                else
                    item.SubItems[4].Text = isTurkish ? "İndirim Yok" : "No Discount";

                item.SubItems[5].Text = useFixDisc.ToString("N2");

                // ÖTV ve KDV her zaman TL olarak gösterilir (PricingOrderScreen ile aynı)
                item.SubItems[6].Text = useSctRate > 0
                    ? $"%{useSctRate} ({sctVal_TL:N2} TL)"
                    : (isTurkish ? "ÖTV Yok" : "No Excise Tax");

                item.SubItems[7].Text = useVatRate > 0
                    ? $"%{useVatRate} ({vatVal_TL:N2} TL)"
                    : (isTurkish ? "KDV Yok" : "No VAT");
            }
        }

        // Yardımcı Metot: Para biriminin TL karşılığını bulur
        private decimal GetRateToTL(string currency, Dictionary<string, (decimal BuyRate, decimal SellRate)> rates)
        {
            if (string.IsNullOrEmpty(currency) || currency == "Türk Lirası" || currency == "TL")
                return 1.0m;

            // Sınıflarınızda Dictionary Key'leri "Dolar" ve "Euro" olarak tutulduğu için:
            if (currency.Contains("Dolar") || currency.Contains("USD")) return rates["Dolar"].SellRate;
            if (currency.Contains("Euro") || currency.Contains("EUR")) return rates["Euro"].SellRate;
            if (currency.Contains("Sterlin") || currency.Contains("GBP")) return rates["Sterlin"].SellRate;

            return 1.0m; // Bulunamazsa 1 döndür (TL kabul et)
        }

        private void dgwProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Yalnızca Düzenleme (rbEdit) modunda çalışsın
            if (!rbEdit.Checked) return;

            string columnName = dgwProducts.Columns[e.ColumnIndex].Name;

            if (columnName == "Price" || columnName == "OrderQuantity")
            {
                bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
                var cellValue = dgwProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

                // Önceki değer ile yeni değer aynıysa işlem yapma
                if (cellValue == _previousProductName) return;

                string message = isTurkish
                    ? $"'{_previousProductName}' olan değeri '{cellValue}' olarak güncellemek istediğinize emin misiniz?"
                    : $"Are you sure you want to update the value from '{_previousProductName}' to '{cellValue}'?";

                string caption = isTurkish ? "Değişiklik Onayı" : "Change Confirmation";

                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Tutarın ve genel toplamların anlık olarak güncellenmesi
                    UpdateTotalAmounts(preserveRates: true);
                }
                else
                {
                    // Kullanıcı Hayır derse önceki değeri geri yükle
                    dgwProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _previousProductName;
                    UpdateTotalAmounts(preserveRates: true); // Eski değere göre tekrar hesapla
                }
            }
        }

        // lwDisc: Satıra tıklanınca checkbox toggle (PricingOrderScreen mantığı)
        private void lwDisc_MouseClick(object sender, MouseEventArgs e)
        {
            var info = lwDisc.HitTest(e.Location);
            if (info.Item != null)
            {
                info.Item.Checked = !info.Item.Checked;
            }
        }

        // lwDisc_ItemCheck artık InitializeLwDisc'te bağlanmıyor.
        // Çift tetiklenmeyi önlemek için devre dışı bırakıldı; yalnızca lwDisc_ItemChecked kullanılır.
        private void lwDisc_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Kasıtlı olarak boş bırakıldı.
        }

        private void lwDisc_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateControlsAccessibility();
        }

        // Herhangi bir satır işaretliyse indirim/vergi kontrollerini aç, yoksa kapat ve sıfırla
        private void UpdateControlsAccessibility()
        {
            if (_isLoading) return;      // Veri yükleme sırasında tetiklenmesin
            if (!rbEdit.Checked) return; // Yalnızca düzenleme modunda çalışsın

            bool anyChecked = lwDisc.Items.Cast<ListViewItem>().Any(i => i.Checked);

            cmbDisc.Enabled = anyChecked;
            nmrDisc.Enabled = anyChecked;
            cmbSCT.Enabled = anyChecked;
            cmbVAT.Enabled = anyChecked;

            if (!anyChecked)
            {
                _isLoading = true;
                cmbDisc.SelectedIndex = 0;
                nmrDisc.Value = 0;
                cmbSCT.SelectedIndex = 0;
                cmbVAT.SelectedIndex = 0;
                _isLoading = false;
            }
        }

        private void cmbDisc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // Eğer "İndirim Yapılmıyor" (Index 0) seçilirse
            if (cmbDisc.SelectedIndex == 0)
            {
                string msgNoDiscount = isTurkish
                    ? "İndirimi kaldırmak istediğinize emin misiniz?"
                    : "Are you sure you want to remove the discount?";

                if (MessageBox.Show(msgNoDiscount, "Değişiklik Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    UpdateTotalAmounts();
                }
                else
                {

                }
                return;
            }

            if (cmbDisc.SelectedIndex <= 0) return;

            string selectedValue = cmbDisc.SelectedItem.ToString();
            string message = isTurkish
                ? $"İndirim oranını '{selectedValue}' olarak güncellemek istediğinize emin misiniz?"
                : $"Are you sure you want to update the discount rate to '{selectedValue}'?";

            if (MessageBox.Show(message, "Değişiklik Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UpdateTotalAmounts();
            }
            else
            {
                cmbDisc.SelectedIndex = 0;
            }
        }

        private void nmrDisc_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
            string message = isTurkish
                ? "Ekstra indirim miktarını güncellemek üzeresiniz, onaylıyor musunuz?"
                : "You are about to update the extra discount amount, do you confirm?";

            if (MessageBox.Show(message, "Ekstra İndirim Güncellemesi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UpdateTotalAmounts();
            }
        }

        private void cmbSCT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // Eğer "ÖTV Yok" (Index 0) seçilirse
            if (cmbSCT.SelectedIndex == 0)
            {
                string msgNoSct = isTurkish
                    ? "ÖTV (SCT) değerini devre dışı bırakmak istediğinize emin misiniz?"
                    : "Are you sure you want to disable the SCT value?";

                if (MessageBox.Show(msgNoSct, "ÖTV Değişikliği Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    UpdateTotalAmounts();
                }
                return;
            }

            if (cmbSCT.SelectedIndex <= 0) return;

            string selectedValue = cmbSCT.SelectedItem.ToString();
            string message = isTurkish
                ? $"ÖTV (SCT) oranını '{selectedValue}' olarak değiştirmek istediğinize emin misiniz?"
                : $"Are you sure you want to change the SCT rate to '{selectedValue}'?";

            if (MessageBox.Show(message, "ÖTV Değişikliği Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UpdateTotalAmounts();
            }
            else
            {
                cmbSCT.SelectedIndex = 0;
            }
        }

        private void cmbVAT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // Eğer "KDV Yok" (Index 0) seçilirse
            if (cmbVAT.SelectedIndex == 0)
            {
                string msgNoVat = isTurkish
                    ? "KDV (VAT) değerini devre dışı bırakmak istediğinize emin misiniz?"
                    : "Are you sure you want to disable the VAT value?";

                if (MessageBox.Show(msgNoVat, "KDV Değişikliği Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    UpdateTotalAmounts();
                }
                return;
            }

            if (cmbVAT.SelectedIndex <= 0) return;

            string selectedValue = cmbVAT.SelectedItem.ToString();
            string message = isTurkish
                ? $"KDV (VAT) oranını '{selectedValue}' olarak değiştirmek istediğinize emin misiniz?"
                : $"Are you sure you want to change the VAT rate to '{selectedValue}'?";

            if (MessageBox.Show(message, "KDV Değişikliği Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UpdateTotalAmounts();
            }
            else
            {
                cmbVAT.SelectedIndex = 0;
            }
        }

        private void nmrCargo_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";
            string message = isTurkish
                ? "Kargo bedeli güncelleniyor. Bu işlem toplam Türk Lirası tutarına yansıyacaktır, onaylıyor musunuz?"
                : "You are updating the cargo cost. This will be reflected in the total Turkish Lira amount, do you confirm?";

            if (MessageBox.Show(message, "Kargo Bedeli Güncellemesi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UpdateTotalAmounts();
            }
        }

        private void OrderViewEditScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            LanguageService.LanguageChanged -= LanguageService_LanguageChanged;
        }

        private void SetPriceLockToolTip(PictureBox pictureBox, int delay = 500)
        {
            string currentLanguage = LanguageService.CurrentLanguage;
            string text = "";

            if (currentLanguage == "Türkçe")
            {
                text = "✔ Fiyat Kilidi AÇIK (işaretli):\n" +
                       "Para birimi değiştirildiğinde ürün birim fiyatları olduğu gibi KORUNUR.\n" +
                       "Kur dönüşümü yapılmaz; sadece para birimi etiketi değişir.\n\n" +
                       "✘ Fiyat Kilidi KAPALI (işaretsiz):\n" +
                       "Para birimi değiştirildiğinde birim fiyatlar güncel kur üzerinden\n" +
                       "otomatik olarak YENİ BİRİME DÖNÜŞTÜRÜLÜR.";
            }
            else
            {
                text = "✔ Price Lock ON (checked):\n" +
                       "When currency is changed, unit prices are KEPT as-is.\n" +
                       "No conversion is applied; only the currency label changes.\n\n" +
                       "✘ Price Lock OFF (unchecked):\n" +
                       "When currency is changed, unit prices are automatically\n" +
                       "CONVERTED to the new currency using the current exchange rate.";
            }

            // Formundaki ToolTip nesnesinin adı neyse onu buraya yazmalısın
            ttPriceLock.InitialDelay = delay;
            ttPriceLock.ReshowDelay = delay;
            ttPriceLock.AutoPopDelay = int.MaxValue;
            ttPriceLock.ShowAlways = true;

            ttPriceLock.SetToolTip(pictureBox, text);
        }

        private void InitPriceLockFeature()
        {
            // 1. ToolTip metinlerini ve gecikme ayarlarını yap
            SetPriceLockToolTip(pbInfo);

            // 2. pbInfo'yu parent hiyerarşisinden çıkarıp doğrudan forma taşı.
            if (pbInfo.Parent != null && pbInfo.Parent != this)
            {
                // Mevcut görsel konumu ekran koordinatına çevir
                Point screenPos = pbInfo.Parent.PointToScreen(pbInfo.Location);
                // Ekran koordinatını form koordinatına çevir
                Point formPos = this.PointToClient(screenPos);

                // Eski parent'tan çıkar, forma ekle
                pbInfo.Parent.Controls.Remove(pbInfo);
                this.Controls.Add(pbInfo);

                // Formdaki yeni konumunu uygula (görsel pozisyon korunur)
                pbInfo.Location = formPos;
            }

            // 3. En öne getir (artık doğrudan form child'ı olduğu için kesin çalışır)
            pbInfo.BringToFront();

            // 4. Form ilk açıldığında hiçbir şey seçili olmadığı için PictureBox'ı gizle
            pbInfo.Visible = false;

            // 5. Seçim değişikliklerini (Event) dinamik olarak bağla
            rbRead.CheckedChanged += (s, ev) => RotateInfo();
            rbEdit.CheckedChanged += (s, ev) => RotateInfo();
            rbDelete.CheckedChanged += (s, ev) => RotateInfo();
        }

        private void RotateInfo()
        {
            // Üç seçenekten herhangi biri işaretli mi kontrol et
            bool shouldShow = rbRead.Checked || rbEdit.Checked || rbDelete.Checked;

            // Eğer görünürlük durumu zaten değişmediyse gereksiz işlem yapma
            if (pbInfo.Visible == shouldShow) return;

            // Görünürlüğü ayarla
            pbInfo.Visible = shouldShow;

            // Görünür hale geldiğinde resmi SADECE YATAYDA (soldan sağa - d'yi b yapacak şekilde) çevirir
            if (shouldShow && pbInfo.Image != null)
            {
                // RotateNoneFlipX: Baş aşağı çevirmez, sadece sol-sağ aynalama yapar!
                pbInfo.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pbInfo.Refresh();
            }
            else if (!shouldShow && pbInfo.Image != null)
            {
                // Gizlendiğinde resmi tekrar orijinal (eski) haline döndürürüz ki bir sonraki açılışta ters kalmasın
                pbInfo.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pbInfo.Refresh();
            }
        }

        // ─── Hover tooltip (çift tıkla detay göster ipucu)

        private void InitializeHoverTooltips()
        {
            var listViews = new List<ListView> { lwTotal, lwDiscList, lwTax };

            foreach (var lw in listViews)
            {
                lw.MouseEnter += lw_MouseEnter;
                lw.MouseLeave += lw_MouseLeave;
                lw.DoubleClick += lw_DoubleClick;
            }

            hoverTimer.Tick += hoverTimer_Tick;
        }

        private void lw_MouseEnter(object sender, EventArgs e)
        {
            isHovering = true;
            hoveredListView = sender as ListView;
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

                var pt = hoveredListView.PointToClient(Cursor.Position);
                ttAll.Show(msg, hoveredListView, pt.X + 10, pt.Y + 10, 15000);
            }
        }

        // ─── Çift tıklama ortak yönlendirici 
        private void lw_DoubleClick(object sender, EventArgs e)
        {
            if (sender == lwTotal)
                OpenTotalDetail();
            else if (sender == lwDiscList)
                OpenDiscDetail();
            else if (sender == lwTax)
                OpenTaxDetail();
        }

        // ─── lwTotal çift tıklama 
        private void OpenTotalDetail()
        {
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            if (lwTotal.Items.Count == 0)
            {
                string message = isTurkish ? "Gösterilecek veri yok" : "No data to display";
                MessageBox.Show(message, isTurkish ? "Bilgi" : "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var detailForm = new TotalTreeViewForm(lwTotal, lwDisc);
            detailForm.Text = isTurkish ? "Toplam ve Detaylar" : "Totals and Details";
            detailForm.Size = new System.Drawing.Size(400, 500);
            detailForm.StartPosition = FormStartPosition.CenterParent;
            detailForm.AutoScroll = true;
            detailForm.MaximizeBox = false;

            var mainMenu = Application.OpenForms["MainMenu"] as MainMenu;
            if (mainMenu != null && mainMenu.chbTop.Checked)
                detailForm.TopMost = true;

            detailForm.ShowDialog();
        }

        // ─── lwDiscList çift tıklama 
        private void OpenDiscDetail()
        {
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            if (lwDiscList.Items.Count == 0)
            {
                string message = isTurkish ? "Gösterilecek veri yok" : "No data to display";
                MessageBox.Show(message, isTurkish ? "Bilgi" : "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var detailForm = new TotalTreeViewForm(lwDiscList, lwDisc, true);
            detailForm.Text = isTurkish ? "Toplam İndirim ve Detaylar" : "Total Discount and Details";
            detailForm.Size = new System.Drawing.Size(400, 500);
            detailForm.StartPosition = FormStartPosition.CenterParent;
            detailForm.AutoScroll = true;
            detailForm.MaximizeBox = false;

            var mainMenu = Application.OpenForms["MainMenu"] as MainMenu;
            if (mainMenu != null && mainMenu.chbTop.Checked)
                detailForm.TopMost = true;

            detailForm.ShowDialog();
        }

        // ─── lwTax çift tıklama 
        private void OpenTaxDetail()
        {
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            if (lwTax.Items.Count == 0)
            {
                string message = isTurkish ? "Gösterilecek veri yok" : "No data to display";
                MessageBox.Show(message, isTurkish ? "Bilgi" : "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var detailForm = new TotalTreeViewForm(lwTax, lwDisc, false, true);
            detailForm.Text = isTurkish ? "Vergi ve Detaylar" : "Tax and Details";
            detailForm.Size = new System.Drawing.Size(400, 500);
            detailForm.StartPosition = FormStartPosition.CenterParent;
            detailForm.AutoScroll = true;
            detailForm.MaximizeBox = false;

            var mainMenu = Application.OpenForms["MainMenu"] as MainMenu;
            if (mainMenu != null && mainMenu.chbTop.Checked)
                detailForm.TopMost = true;

            detailForm.ShowDialog();
        }
    }
}