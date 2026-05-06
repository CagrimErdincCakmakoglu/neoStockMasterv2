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

            // PricingOrderScreen’deki gibi DataGridView başlık stillerini uygula
            SetModernHeaderStyle(dgwProducts);

            // Comboboxları doldurma metodu
            FillComboBoxes();

            UpdateDateTimePickerLanguage();

            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            LanguageService.SetLanguage(LanguageService.CurrentLanguage);
        }

        private void LanguageService_LanguageChanged()
        {
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

            // Yeni eklendi: radio button'ları sıfırla, formu ilk boyuna döndür
            rbRead.Checked = false;
            rbEdit.Checked = false;
            rbDelete.Checked = false;
            this.Height = 130;

            // Kontrol durumlarını yeniden uygula
            ApplyOrderSelectionState();
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
            cmbDisc.Enabled = true;
            nmrDisc.Enabled = true;
            cmbSCT.Enabled = true;
            cmbVAT.Enabled = true;
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

            // Kolon genişliği değişimini engelle (PricingOrderScreen’deki gibi)
            lwDisc.ColumnWidthChanging += (s, e) =>
            {
                e.Cancel = true;
                e.NewWidth = lwDisc.Columns[e.ColumnIndex].Width;
            };    // Kolon genişliği değişimini engelle (PricingOrderScreen’deki gibi)
            lwDisc.ColumnWidthChanging += (s, e) =>
            {
                e.Cancel = true;
                e.NewWidth = lwDisc.Columns[e.ColumnIndex].Width;
            };
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
                    detail.ProductName,                     // Name
                    detail.OrderPrice.ToString("F2"),       // Price
                    detail.Currency,                        // PriceCurrency
                    detail.Quantity,                        // OrderQuantity
                    detail.Total.ToString("F2")             // Total
                );
            }

            // lwDisc tablosunu doldur
            foreach (var detail in selectedOrder.OrderContent)
            {
                var item = new ListViewItem("");
                item.SubItems.Add(detail.ProductName);
                item.SubItems.Add(detail.Total.ToString("F2"));
                item.SubItems.Add(detail.Currency);

                string discountStatus = selectedOrder.DiscountPercentage > 0
                    ? (isTurkish ? $"%{selectedOrder.DiscountPercentage} İndirim"
                                 : $"Apply {selectedOrder.DiscountPercentage}% Discount")
                    : (isTurkish ? "İndirim Yok" : "No Discount");

                item.SubItems.Add(discountStatus);
                item.SubItems.Add(selectedOrder.ExtraDiscountAmount.ToString("F2"));

                string sctStatus = selectedOrder.TaxPercentageSCT > 0
                    ? (isTurkish ? $"%{selectedOrder.TaxPercentageSCT} ÖTV"
                                 : $"%{selectedOrder.TaxPercentageSCT} SCT")
                    : (isTurkish ? "ÖTV Yok" : "No Excise Tax");

                string vatStatus = selectedOrder.TaxPercentageVAT > 0
                    ? (isTurkish ? $"%{selectedOrder.TaxPercentageVAT} KDV"
                                 : $"%{selectedOrder.TaxPercentageVAT} VAT")
                    : (isTurkish ? "KDV Yok" : "No VAT");

                item.SubItems.Add(sctStatus);
                item.SubItems.Add(vatStatus);

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

            // === lwTotal (Toplam Tutar) ===
            lwTotal.View = View.Details;
            lwTotal.Columns.Clear();
            lwTotal.Columns.Add("", lwTotal.Width / 2);
            lwTotal.Columns.Add("", lwTotal.Width / 2);
            lwTotal.HeaderStyle = ColumnHeaderStyle.None;
            lwTotal.Items.Clear();

            foreach (var kvp in selectedOrder.TotalPrice)
            {
                string label = kvp.Key;
                string formattedTotal = $"{kvp.Value:N2}";
                var totalItem = new ListViewItem(new[] { label, formattedTotal });
                lwTotal.Items.Add(totalItem);
            }

            // === lwDiscList (Toplam İndirim) ===
            lwDiscList.View = View.Details;
            lwDiscList.Columns.Clear();
            lwDiscList.Columns.Add("", lwDiscList.Width / 2);
            lwDiscList.Columns.Add("", lwDiscList.Width / 2);
            lwDiscList.HeaderStyle = ColumnHeaderStyle.None;
            lwDiscList.Items.Clear();

            foreach (var kvp in selectedOrder.TotalDiscount)
            {
                string label = kvp.Key;
                string formattedDisc = $"{kvp.Value:N2}";
                var discItem = new ListViewItem(new[] { label, formattedDisc });
                lwDiscList.Items.Add(discItem);
            }

            // === lwTax (Vergi) — eski stil ===
            lwTax.View = View.Details;
            lwTax.Columns.Clear();
            int halfWidth = lwTax.Width / 2;
            lwTax.Columns.Add("", halfWidth);
            lwTax.Columns.Add("", halfWidth);
            lwTax.HeaderStyle = ColumnHeaderStyle.None;
            lwTax.Items.Clear();

            if (selectedOrder.Tax > 0)
            {
                string label = "Türk Lirası";
                string formattedTax = $"{selectedOrder.Tax:N2} ₺";
                var taxItem = new ListViewItem(new[] { label, formattedTax });
                lwTax.Items.Add(taxItem);
            }

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

            // 🟢 Yeni Ekleme: Fiyat veya Sipariş Adeti düzenleniyorsa önceki değeri sakla
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
                
            }
        }

        // Formu temizlemek için yardımcı metod
        private void ClearFormFields()
        {
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

        private async void CurrencyChanged(object sender, EventArgs e) //(KONTROL ET)
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

                UpdateTotalAmounts(); // Listeleri ve toplamları yeni birimle tazele
            }
            catch (Exception ex)
            {
                MessageBox.Show(isTurkish ? "Hata: " + ex.Message : "Error: " + ex.Message);
            }
        }

        private void UpdateSummaryListViews(decimal subTotal, decimal discount, decimal final, string cur)
        {
            bool isEng = LanguageService.CurrentLanguage == "English";

            // Özet Tablosu (Ara Toplam, KDV, Genel Toplam)
            if (lwTotal != null)
            {
                lwTotal.Items.Clear();

                var itemSub = new ListViewItem(isEng ? "Subtotal" : "Ara Toplam");
                itemSub.SubItems.Add(subTotal.ToString("N2") + " " + cur);
                lwTotal.Items.Add(itemSub);

                var itemFinal = new ListViewItem(isEng ? "Total Amount" : "Toplam Tutar");
                itemFinal.SubItems.Add(final.ToString("N2") + " " + cur);
                itemFinal.Font = new Font(lwTotal.Font, FontStyle.Bold);
                lwTotal.Items.Add(itemFinal);
            }

            // İskonto Tablosu
            if (lwDiscList != null)
            {
                lwDiscList.Items.Clear();
                if (discount > 0)
                {
                    var itemDisc = new ListViewItem(isEng ? "General Discount" : "Genel İskonto");
                    itemDisc.SubItems.Add("-" + discount.ToString("N2") + " " + cur);
                    itemDisc.ForeColor = Color.Red;
                    lwDiscList.Items.Add(itemDisc);
                }
            }

            // Vergi Tablosu
            if (lwTax != null)
            {
                lwTax.Items.Clear();
                var itemVat = new ListViewItem(isEng ? "VAT Total" : "KDV Toplam");
                itemVat.SubItems.Add("0.00 " + cur); // KDV hesaplamanız varsa buraya ekleyebilirsiniz
                lwTax.Items.Add(itemVat);
            }
        }


        // Toplam tutarları güncelleyen yardımcı metodunuz yoksa bunu ekleyin:
        private void UpdateTotalAmounts()
        {
            decimal cargoCost = nmrCargo.Value;
            var totalsByCurrency = new Dictionary<string, decimal>();
            decimal selectedSubTotal = 0;
            string selectedCurrency = null;

            // 1. Ürün bazlı ham toplamları hesapla
            for (int i = 0; i < dgwProducts.Rows.Count; i++)
            {
                DataGridViewRow row = dgwProducts.Rows[i];
                if (row.Cells["Price"].Value == null || row.Cells["OrderQuantity"].Value == null) continue;

                decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                decimal qty = Convert.ToDecimal(row.Cells["OrderQuantity"].Value);
                decimal lineTotal = price * qty;

                row.Cells["Total"].Value = lineTotal.ToString("N2");

                string currency = row.Cells["PriceCurrency"].Value?.ToString() ?? "Türk Lirası";
                if (!totalsByCurrency.ContainsKey(currency)) totalsByCurrency[currency] = 0;
                totalsByCurrency[currency] += lineTotal;

                if (i < lwDisc.Items.Count && lwDisc.Items[i].Checked)
                {
                    selectedSubTotal += lineTotal;
                    selectedCurrency = currency;
                }
            }

            // 2. Form üzerindeki güncel oranları al
            decimal percentageRate = 0;
            if (cmbDisc.SelectedIndex > 0)
            {
                string numericPart = new string(cmbDisc.SelectedItem.ToString().Where(char.IsDigit).ToArray());
                decimal.TryParse(numericPart, out percentageRate);
            }

            decimal fixedAmountDisc = nmrDisc.Value; // Doğrudan düşülecek tutar

            decimal sctRate = 0;
            if (cmbSCT.SelectedIndex > 0)
                decimal.TryParse(new string(cmbSCT.SelectedItem.ToString().Where(char.IsDigit).ToArray()), out sctRate);

            decimal vatRate = 0;
            if (cmbVAT.SelectedIndex > 0)
                decimal.TryParse(new string(cmbVAT.SelectedItem.ToString().Where(char.IsDigit).ToArray()), out vatRate);

            // 3. ListView'i güncelle (Bu metot artık her satırı bağımsız hesaplar)
            UpdateLwDiscItems(percentageRate / 100m, fixedAmountDisc, sctRate, vatRate);

            // 4. Genel Toplamı Hesapla (ListView'deki son tutarları baz alarak)
            decimal totalFinal = 0;
            foreach (ListViewItem item in lwDisc.Items)
            {
                if (decimal.TryParse(item.SubItems[2].Text, out decimal val))
                    totalFinal += val;
            }

            string summaryDisplayCurrency = selectedCurrency ?? (totalsByCurrency.Keys.FirstOrDefault() ?? "Türk Lirası");
            UpdateSummaryListViews(selectedSubTotal, 0, totalFinal, summaryDisplayCurrency);
        }

        private void UpdateLwDiscItems(decimal percRate, decimal fixDisc, decimal sctRate, decimal vatRate)
        {
            if (lwDisc == null) return;

            foreach (ListViewItem item in lwDisc.Items)
            {
                // Sadece checkbox seçili olanlarda formdaki yeni değerleri uygula
                // Seçili olmayanların eski değerlerine dokunma (veya 0 kabul et)
                if (!item.Checked) continue;

                string productName = item.SubItems[1].Text;
                DataGridViewRow row = dgwProducts.Rows.Cast<DataGridViewRow>()
                                      .FirstOrDefault(r => r.Cells["Name"].Value?.ToString() == productName);

                if (row == null) continue;

                decimal originalAmount = Convert.ToDecimal(row.Cells["Total"].Value);

                // --- HESAPLAMA SIRALAMASI ---
                // 1. Yüzde İndirimi (cmbDisc)
                decimal percDiscVal = originalAmount * percRate;
                decimal afterPerc = originalAmount - percDiscVal;

                // 2. Sabit Tutar İndirimi (nmrDisc)
                decimal totalLineDiscount = percDiscVal + fixDisc;
                decimal priceAfterDiscount = originalAmount - totalLineDiscount;
                if (priceAfterDiscount < 0) priceAfterDiscount = 0;

                // 3. ÖTV (SCT) - İndirimli tutar üzerinden
                decimal sctVal = priceAfterDiscount * (sctRate / 100m);

                // 4. KDV (VAT) - (İndirimli Tutar + ÖTV) üzerinden
                decimal vatVal = (priceAfterDiscount + sctVal) * (vatRate / 100m);

                decimal finalLineTotal = priceAfterDiscount + sctVal + vatVal;

                // --- GÖRSEL GÜNCELLEME ---
                item.SubItems[2].Text = finalLineTotal.ToString("N2"); // Final Tutar

                // İndirim sütunu: % ve Sabit indirimi beraber göster
                item.SubItems[4].Text = $"%{percRate * 100:0} + {fixDisc:N2}";
                item.SubItems[5].Text = totalLineDiscount.ToString("N2"); // Toplam İndirim Tutarı

                item.SubItems[6].Text = sctRate > 0 ? $"%{sctRate} (+{sctVal:N2})" : "-";
                item.SubItems[7].Text = vatRate > 0 ? $"%{vatRate} (+{vatVal:N2})" : "-";
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
                    UpdateTotalAmounts();
                }
                else
                {
                    // Kullanıcı Hayır derse önceki değeri geri yükle
                    dgwProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _previousProductName;
                    UpdateTotalAmounts(); // Eski değere göre tekrar hesapla
                }
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
    }

}

