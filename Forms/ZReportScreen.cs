using neoStockMasterv2.Data.Entities;
using neoStockMasterv2.Data.Services;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using neoStockMasterv2.Controls;

namespace neoStockMasterv2.Forms
{
    public partial class ZReportScreen : Form
    {
        public static User LoggedInUser { get; set; }
        OrderService orderService = new OrderService();
        ProductService productService = new ProductService();
        private CustomCalendar _calendar;
        private DateTime? _activeFilterDate = null; // null = tarih filtresi yok

        // DataGridView/FormsPlot uzerinde calisantooltip yonetimi
        private readonly Dictionary<Control, (ToolTip tt, string text)> _tooltipMap =
            new Dictionary<Control, (ToolTip, string)>();

        public ZReportScreen(string language)
        {
            InitializeComponent();
            InitCalendar();
            LanguageService.LanguageChanged += LanguageService_LanguageChanged;
            LanguageService.SetLanguage(LanguageService.CurrentLanguage);

            DisableOrderFields(false); // Sipariş alanlarını düzenlenemez yap
            LoadUserOrderStats(); // Sipariş istatistiklerini yükle
            LoadLoggedInUserProducts(); // Giriş yapmış kullanıcının ürünlerini yükle

            // ── RadioButton Olaylarını Bağlama ──
            rbOrderOne.CheckedChanged += rbOrderStatus_CheckedChanged;
            rbOrderTwo.CheckedChanged += rbOrderStatus_CheckedChanged;
            rbOrderThree.CheckedChanged += rbOrderStatus_CheckedChanged;
            rbOrderFour.CheckedChanged += rbOrderStatus_CheckedChanged;
            rbOrderFive.CheckedChanged += rbOrderStatus_CheckedChanged;
            // RadioButton Click olaylarını bağlama
            rbOrderOne.Click += rbOrder_Click;
            rbOrderTwo.Click += rbOrder_Click;
            rbOrderThree.Click += rbOrder_Click;
            rbOrderFour.Click += rbOrder_Click;
            rbOrderFive.Click += rbOrder_Click;

            PopulateOrderGrid();// Sipariş detay tablosunu doldur
            PopulateFinancialGrids(); // Finansal tabloları doldur
        }

        private void ZReportScreen_Load(object sender, EventArgs e)
        {
            mcDate.Visible = false;         // sadece gizle
            this.Controls.Add(_calendar);   // yeni takvimi üstüne koy
        }

        private void LanguageService_LanguageChanged()
        {
            UpdateFormTexts();
        }

        private void UpdateFormTexts()
        {
            türkçeToolStripMenuItem.Text = LanguageService.GetString("Türkçe");
            englishToolStripMenuItem.Text = LanguageService.GetString("English2");

            lblTotalOrders.Text = LanguageService.GetString("Toplam Sipariş Adeti");
            grbBusiness.Text = LanguageService.GetString("İşletme Verileri");
            lblGrossProfit.Text = LanguageService.GetString("Brüt Kazanç");
            lblNetProfit.Text = LanguageService.GetString("Net Kazanç");
            grbOrderStatus.Text = LanguageService.GetString("Siparişlerin Durumu");
            lblOrderOne.Text = LanguageService.GetString("Siparişe Başlanmadı");
            lblOrderTwo.Text = LanguageService.GetString("Hazırlanıyor");
            lblOrderThree.Text = LanguageService.GetString("Kargoya Verilecek");
            lblOrderFourth.Text = LanguageService.GetString("Kargolandı");
            lblOrderFive.Text = LanguageService.GetString("Bitti");
            grbProductDetails.Text = LanguageService.GetString("Ürün Detayları");
            lblStockQuantity.Text = LanguageService.GetString("Stok Adeti");
            lblOrderQuantity.Text = LanguageService.GetString("Sipariş Adeti");
            lblSalesShare.Text = LanguageService.GetString("Satış Payı");
            grbProductStats.Text = LanguageService.GetString("Ürün Satış İstatistikleri");
            btnOrderHistory.Text = LanguageService.GetString("Sipariş Geçmişi");
            btnPriceAnalysis.Text = LanguageService.GetString("Fiyat Analizi");


            this.Text = LanguageService.GetString("Z Raporu");

            // Sipariş Tablosu Sütun Başlıklarını Dil Servisine Göre Güncelleme
            ConfigureAndTranslateOrderGrid();

            // Grid İçeriğini Yeni Dile Göre Yeniden Doldur
            PopulateOrderGrid();

            PopulateFinancialGrids(); // Finansal tabloları da yeni dile göre yeniden doldur

            LoadUserOrderStatsDGW();
            PopulateFinancialGridsDGW();

            UpdateSelectedLanguage();

            // ── Tooltip Metinleri (Dile Göre) ──
            bool isTurkish = LanguageService.CurrentLanguage == "Türkçe";

            // Tooltip genel ayarları — AutomaticDelay KULLANMA, elle set et
            foreach (var tt in new ToolTip[] { ttGross, ttProfit, ttTax })
            {
                tt.Active = true;
                tt.InitialDelay = 400;
                tt.ReshowDelay = 100;
                tt.AutoPopDelay = 6000;
                tt.ShowAlways = true;
            }

            AttachTooltip(dgwGrossProfit, ttGross, isTurkish
                ? "Brüt Kazanç Tablosu: Satışlardan elde edilen toplam geliri gösterir. Ürün maliyeti düşülmeden önceki ham kazancı para birimine göre listeler."
                : "Gross Profit Table: Shows total revenue from sales. Lists raw earnings before product costs are deducted, grouped by currency.");

            AttachTooltip(dgwNetProfit, ttProfit, isTurkish
                ? "Net Kazanç Tablosu: Brüt gelirden vergi ve maliyetler çıkarıldıktan sonra kalan gerçek kazancı para birimine göre gösterir."
                : "Net Profit Table: Shows actual earnings remaining after taxes and costs are deducted from gross revenue, grouped by currency.");

            AttachTooltip(dgwTax, ttTax, isTurkish
                ? "Vergi Tablosu: Siparişlere uygulanan KDV/ÖTV tutarlarını listeler. Toplam vergi yükünü para birimine göre gösterir."
                : "Tax Table: Lists VAT/SCT amounts applied to orders. Shows the total tax burden grouped by currency.");
            }

        private void UpdateSelectedLanguage()
        {
            string currentLanguage = LanguageService.CurrentLanguage;

            türkçeToolStripMenuItem.Checked = currentLanguage == "Türkçe";
            englishToolStripMenuItem.Checked = currentLanguage == "English";

            türkçeToolStripMenuItem.BackColor = türkçeToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;
            englishToolStripMenuItem.BackColor = englishToolStripMenuItem.Checked ? Color.LightBlue : SystemColors.Control;
        }

        private void AttachTooltip(Control control, ToolTip tooltip, string text)
        {
            if (_tooltipMap.ContainsKey(control))
            {
                // Sadece metni guncelle (dil degisimi), event'i yeniden baglama
                _tooltipMap[control] = (tooltip, text);
                return;
            }

            _tooltipMap[control] = (tooltip, text);

            control.MouseEnter += (s, e) =>
            {
                if (_tooltipMap.TryGetValue(control, out var entry))
                {
                    Point screenPos = control.PointToScreen(new Point(10, control.Height - 5));
                    Point formPos = this.PointToClient(screenPos);
                    entry.tt.Show(entry.text, this, formPos.X, formPos.Y, 6000);
                }
            };

            control.MouseLeave += (s, e) =>
            {
                if (_tooltipMap.TryGetValue(control, out var entry))
                    entry.tt.Hide(this);
            };
        }

        private void AttachPlotTooltip(GroupBox groupBox, Control plotControl, ToolTip tooltip, string text)
        {
            // Metin guncellemesi icin map'e kaydet
            if (_tooltipMap.ContainsKey(plotControl))
            {
                _tooltipMap[plotControl] = (tooltip, text);
                return;
            }
            _tooltipMap[plotControl] = (tooltip, text);

            bool _isShowing = false;

            groupBox.MouseMove += (s, e) =>
            {
                // Mouse'un GroupBox icindeki konumunu plotControl'un bounds'u ile karsilastir
                Point posInGroup = e.Location;
                Rectangle plotBounds = plotControl.Bounds;

                if (plotBounds.Contains(posInGroup))
                {
                    if (!_isShowing && _tooltipMap.TryGetValue(plotControl, out var entry))
                    {
                        _isShowing = true;
                        Point screenPos = groupBox.PointToScreen(new Point(posInGroup.X + 10, posInGroup.Y + 15));
                        Point formPos = this.PointToClient(screenPos);
                        entry.tt.Show(entry.text, this, formPos.X, formPos.Y, 6000);
                    }
                }
                else
                {
                    if (_isShowing)
                    {
                        _isShowing = false;
                        tooltip.Hide(this);
                    }
                }
            };

            groupBox.MouseLeave += (s, e) =>
            {
                _isShowing = false;
                tooltip.Hide(this);
            };
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

        private void DisableOrderFields(bool isEnabled)
        {
            nmrTotalOrder.Enabled = isEnabled;
            nmrOrderOne.Enabled = isEnabled;
            nmrOrderTwo.Enabled = isEnabled;
            nmrOrderThree.Enabled = isEnabled;
            nmrOrderFour.Enabled = isEnabled;
            nmrOrderFive.Enabled = isEnabled;
            nmrStockQuantity.Enabled = isEnabled;
            nmrOrderQuantity.Enabled = isEnabled;
            nmrSalesShare.Enabled = isEnabled;
        }

        private void LoadUserOrderStats()
        {
            OrderService.LoggedInUser = LoggedInUser;

            // Tüm siparişleri al
            var allOrders = orderService.GetAllOrders(); // Tüm siparişler
            var userOrders = orderService.GetOrdersByLoggedInUser();

            nmrTotalOrder.Value = userOrders.Count;

            // Her bir sipariş durumu (indeksi) için sayaçları tanımla
            int countOrderOne = 0;   // 0. İndeks: Siparişe Başlanmadı / Not Started
            int countOrderTwo = 0;   // 1. İndeks: Hazırlanıyor / In Preparation
            int countOrderThree = 0; // 2. İndeks: Kargoya Verilecek / To Be Shipped
            int countOrderFour = 0;  // 3. İndeks: Kargolandı / Shipped
            int countOrderFive = 0;  // 4. İndeks: Bitti / Completed

            // Siparişlerin durumlarına göre sayım
            foreach (var order in userOrders)
            {
                string status = order.OrderStatues?.ToString() ?? "";

                if (status == "Siparişe Başlanmadı" || status == "Not Started")
                {
                    countOrderOne++;
                }
                else if (status == "Hazırlanıyor" || status == "In Preparation")
                {
                    countOrderTwo++;
                }
                else if (status == "Kargoya Verilecek" || status == "To Be Shipped")
                {
                    countOrderThree++;
                }
                else if (status == "Kargolandı" || status == "Shipped")
                {
                    countOrderFour++;
                }
                else if (status == "Bitti" || status == "Completed")
                {
                    countOrderFive++;
                }
            }

            // Elde edilen toplamları NumericUpDown kontrollerine yazdır
            nmrOrderOne.Value = countOrderOne;
            nmrOrderTwo.Value = countOrderTwo;
            nmrOrderThree.Value = countOrderThree;
            nmrOrderFour.Value = countOrderFour;
            nmrOrderFive.Value = countOrderFive;
        }

        private void LoadLoggedInUserProducts()
        {
            try
            {
                // 1. ComboBox'ı temizle
                cmbProducts.Items.Clear();

                // 2. ProductService üzerinden tüm ürünleri çek
                var allProducts = productService.GetAllProducts();

                // 3. Sadece giriş yapmış kullanıcının eklediği ürünleri filtrele
                // Not: LoggedInUser formunuzda static veya instance property olarak tanımlı olmalıdır.
                if (LoggedInUser != null && allProducts != null)
                {
                    var userProducts = allProducts
                        .Where(p => p.AddedBy == LoggedInUser.Name) // Veya LoggedInUser.ID projenizin yapısına göre
                        .ToList();

                    // 4. Filtrelenen ürünlerin isimlerini ComboBox'a ekle
                    foreach (var product in userProducts)
                    {
                        if (!string.IsNullOrEmpty(product.Name))
                        {
                            cmbProducts.Items.Add(product.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürünler yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            lwProductDetails.Clear();

            try
            {
                // 1. ComboBox'tan seçilen ürün adını al
                string selectedProductName = cmbProducts.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(selectedProductName))
                {
                    nmrStockQuantity.Value = 0;
                    nmrOrderQuantity.Value = 0;
                    nmrSalesShare.Value = 0;
                    return;
                }

                // 2. Stok Adetini Al ve Yazdır
                int stockQuantity = productService.GetProductStockQuantity(selectedProductName);
                nmrStockQuantity.Value = stockQuantity;

                // 3. Giriş yapan kullanıcının siparişlerini al
                var userOrders = orderService.GetOrdersByLoggedInUser();

                int selectedProductOrderCount = 0; // Seçili ürünün toplam sipariş adeti
                int totalOrderedItemsCount = 0;    // Giriş yapan kullanıcının tüm ürünlerinin toplam sipariş adeti

                // 4. Tüm siparişleri ve sipariş içeriklerini dön
                foreach (var order in userOrders)
                {
                    if (order.OrderContent != null)
                    {
                        foreach (var detail in order.OrderContent)
                        {
                            // Kullanıcının sattığı tüm ürünlerin adetlerini genel toplam için topla
                            totalOrderedItemsCount += detail.Quantity;

                            // Eğer detaydaki ürün seçilen ürün ise onun adedini ayrı topla
                            if (detail.ProductName != null && detail.ProductName.Equals(selectedProductName, StringComparison.OrdinalIgnoreCase))
                            {
                                selectedProductOrderCount += detail.Quantity;
                            }
                        }
                    }
                }

                // 5. Sipariş Adetini Yazdır
                nmrOrderQuantity.Value = selectedProductOrderCount;

                // 6. Satış Payı Yüzdesini Hesapla ve Yazdır
                if (totalOrderedItemsCount > 0)
                {
                    // Yüzde hesabı: (Seçili Ürün Sipariş Adeti / Toplam Sipariş Edilen Ürün Adeti) * 100
                    decimal salesSharePercentage = ((decimal)selectedProductOrderCount / totalOrderedItemsCount) * 100;

                    // NumericUpDown kontrolünün maksimum değer sınırını aşmamak için (Genelde default 100'dür)
                    if (salesSharePercentage > nmrSalesShare.Maximum)
                        nmrSalesShare.Value = nmrSalesShare.Maximum;
                    else
                        nmrSalesShare.Value = salesSharePercentage;
                }
                else
                {
                    nmrSalesShare.Value = 0; // Eğer hiç sipariş yoksa satış payı 0'dır
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürün istatistikleri hesaplanırken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureAndTranslateOrderGrid()
        {
            // Eğer sütunlar henüz oluşturulmadıysa programatik olarak oluşturuyoruz
            if (dgwOrders.Columns.Count == 0)
            {
                dgwOrders.Columns.Add("CustomerName", "Customer Name");
                dgwOrders.Columns.Add("CustomerPhone", "Phone");
                dgwOrders.Columns.Add("OrderDate", "Date");
                dgwOrders.Columns.Add("OrderContent", "Products");
                dgwOrders.Columns.Add("TotalPrice", "Total Price");
                dgwOrders.Columns.Add("TotalDiscount", "Discount");
                dgwOrders.Columns.Add("PayableAmount", "Payable Amount");
                dgwOrders.Columns.Add("Tax", "Tax");
                dgwOrders.Columns.Add("OrderStatues", "Status");
                dgwOrders.Columns.Add("Cargo", "Cargo");
                dgwOrders.Columns.Add("TrackingNumber", "Tracking No");

                // En soldaki boş satır seçme sütununu (Row Header) kaldırır
                dgwOrders.RowHeadersVisible = false;

                // Kullanıcının sütun genişliklerini el ile değiştirmesini engeller
                dgwOrders.AllowUserToResizeColumns = false;

                // Kullanıcının satır yüksekliklerini el ile değiştirmesini engeller
                dgwOrders.AllowUserToResizeRows = false;

                // Hücre genişliklerini içindeki en uzun metne göre otomatik ayarlar
                dgwOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                // İçerik ekrana sığmadığında alttaki yatay kaydırma çubuğunun (Scrollbar) açılmasını sağlar
                dgwOrders.ScrollBars = ScrollBars.Both;

                // Grid satır seçimi ve diğer standart ayarlar
                dgwOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgwOrders.AllowUserToAddRows = false;
                dgwOrders.ReadOnly = true;
            }

            // Dil tercihine göre başlık metinlerini atıyoruz (ID satırı hariç)
            dgwOrders.Columns["CustomerName"].HeaderText = LanguageService.GetString("Müşteri Adı") ?? "Müşteri Adı";
            dgwOrders.Columns["CustomerPhone"].HeaderText = LanguageService.GetString("Telefon") ?? "Telefon";
            dgwOrders.Columns["OrderDate"].HeaderText = LanguageService.GetString("Tarih") ?? "Tarih";
            dgwOrders.Columns["OrderContent"].HeaderText = LanguageService.GetString("Ürün Detayları") ?? "Ürün Detayları";
            dgwOrders.Columns["TotalPrice"].HeaderText = LanguageService.GetString("Toplam Tutar") ?? "Toplam Tutar";
            dgwOrders.Columns["TotalDiscount"].HeaderText = LanguageService.GetString("Toplam İndirim") ?? "Toplam İndirim";
            dgwOrders.Columns["PayableAmount"].HeaderText = LanguageService.GetString("Ödenecek Tutar") ?? "Ödenecek Tutar";
            dgwOrders.Columns["Tax"].HeaderText = LanguageService.GetString("KDV/ÖTV") ?? "KDV/ÖTV";
            dgwOrders.Columns["OrderStatues"].HeaderText = LanguageService.GetString("Sipariş Durumu") ?? "Sipariş Durumu";
            dgwOrders.Columns["Cargo"].HeaderText = LanguageService.GetString("Kargo Firması") ?? "Kargo Firması";
            dgwOrders.Columns["TrackingNumber"].HeaderText = LanguageService.GetString("Takip No") ?? "Takip No";
        }

        private void PopulateOrderGrid()
        {
            try
            {
                dgwOrders.Rows.Clear();

                OrderService.LoggedInUser = LoggedInUser;
                var userOrders = orderService.GetOrdersByLoggedInUser();

                if (userOrders == null || userOrders.Count == 0) return;

                // ── Statü Filtresi (RadioButton) ──────────────────────────────
                string targetStatusTR = "";
                string targetStatusEN = "";

                if (rbOrderOne.Checked) { targetStatusTR = "Siparişe Başlanmadı"; targetStatusEN = "Not Started"; }
                else if (rbOrderTwo.Checked) { targetStatusTR = "Hazırlanıyor"; targetStatusEN = "In Preparation"; }
                else if (rbOrderThree.Checked) { targetStatusTR = "Kargoya Verilecek"; targetStatusEN = "To Be Shipped"; }
                else if (rbOrderFour.Checked) { targetStatusTR = "Kargolandı"; targetStatusEN = "Shipped"; }
                else if (rbOrderFive.Checked) { targetStatusTR = "Bitti"; targetStatusEN = "Completed"; }

                foreach (var order in userOrders)
                {
                    // ── Tarih Filtresi (Takvim) ───────────────────────────────
                    if (_activeFilterDate.HasValue &&
                        order.OrderDate.Date != _activeFilterDate.Value.Date)
                        continue;

                    // ── Statü Filtresi ────────────────────────────────────────
                    if (!string.IsNullOrEmpty(targetStatusTR))
                    {
                        string status = order.OrderStatues?.ToString() ?? "";
                        if (!status.Equals(targetStatusTR, StringComparison.OrdinalIgnoreCase) &&
                            !status.Equals(targetStatusEN, StringComparison.OrdinalIgnoreCase))
                            continue;
                    }

                    // ── Satır Hazırlama ───────────────────────────────────────
                    var productDetails = new List<string>();
                    if (order.OrderContent != null)
                        foreach (var item in order.OrderContent)
                            productDetails.Add($"{item.ProductName} ({item.Quantity}x) {item.OrderPrice} {item.Currency}");

                    string orderContentSummary = string.Join(", ", productDetails);
                    string totalPriceStr = order.TotalPrice != null ? string.Join(" | ", order.TotalPrice.Select(x => $"{x.Value} {x.Key}")) : "0";
                    string totalDiscountStr = order.TotalDiscount != null ? string.Join(" | ", order.TotalDiscount.Select(x => $"{x.Value} {x.Key}")) : "0";
                    string payableAmountStr = order.PayableAmount != null ? string.Join(" | ", order.PayableAmount.Select(x => $"{x.Value} {x.Key}")) : "0";

                    string vatLabel = LanguageService.CurrentLanguage == "Türkçe" ? "KDV" : "VAT";
                    string sctLabel = LanguageService.CurrentLanguage == "Türkçe" ? "ÖTV" : "SCT";

                    dgwOrders.Rows.Add(
                        order.CustomerName,
                        order.CustomerPhone,
                        order.OrderDate.ToString("yyyy-MM-dd HH:mm"),
                        orderContentSummary,
                        totalPriceStr,
                        totalDiscountStr,
                        payableAmountStr,
                        $"{order.Tax} ({vatLabel}: %{order.TaxPercentageVAT} / {sctLabel}: %{order.TaxPercentageSCT})",
                        order.OrderStatues,
                        order.Cargo,
                        order.CargoTrackingNumber
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sipariş tablosu yüklenirken hata oluştu: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbOrderStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rb && rb.Checked)
            {
                // RadioButton seçilince takvim tarih filtresini sıfırla
                _activeFilterDate = null;

                PopulateOrderGrid();
                LoadUserOrderStatsDGW();
            }

        }

        private void rbOrder_Click(object sender, EventArgs e)
        {
            if (sender is RadioButton rb)
            {
                if (rb.Tag != null && (bool)rb.Tag == true)
                {
                    rb.Checked = false;
                    rb.Tag = false;

                    // Toggle ile seçim kalktığında tarih filtresini de sıfırla
                    _activeFilterDate = null;

                    PopulateOrderGrid();
                    LoadUserOrderStatsDGW();
                }
                else
                {
                    rbOrderOne.Tag = false;
                    rbOrderTwo.Tag = false;
                    rbOrderThree.Tag = false;
                    rbOrderFour.Tag = false;
                    rbOrderFive.Tag = false;
                    rb.Tag = true;

                    // Yeni RadioButton seçilince tarih filtresini sıfırla
                    _activeFilterDate = null;
                }
            }
        }

        private void PopulateFinancialGrids()
        {
            try
            {
                // 1. Tabloların mevcut satırlarını ve sütunlarını temizle
                dgwGrossProfit.Rows.Clear();
                dgwGrossProfit.Columns.Clear();

                dgwNetProfit.Rows.Clear();
                dgwNetProfit.Columns.Clear();

                dgwTax.Rows.Clear();
                dgwTax.Columns.Clear();

                // 2. Tabloların tasarım ve sütun ayarlarını yap
                string currencyHeader = LanguageService.CurrentLanguage == "Türkçe" ? "Para Birimi" : "Currency";
                string amountHeader = LanguageService.CurrentLanguage == "Türkçe" ? "Tutar" : "Amount";
                string taxTypeHeader = LanguageService.CurrentLanguage == "Türkçe" ? "Vergi Türü" : "Tax Type";

                dgwGrossProfit.Columns.Add("Currency", currencyHeader);
                dgwGrossProfit.Columns.Add("Amount", amountHeader);

                dgwNetProfit.Columns.Add("Currency", currencyHeader);
                dgwNetProfit.Columns.Add("Amount", amountHeader);

                dgwTax.Columns.Add("TaxType", taxTypeHeader);
                dgwTax.Columns.Add("Currency", currencyHeader);
                dgwTax.Columns.Add("Amount", amountHeader);

                // ── YENİ DÜZENLEME: Tabloların Ekrana Tam Sığması Ayarları ──
                foreach (var grid in new[] { dgwGrossProfit, dgwNetProfit, dgwTax })
                {
                    grid.RowHeadersVisible = false;          // En soldaki boş sütunu gizle
                    grid.AllowUserToResizeColumns = false;   // El ile genişlik değiştirmeyi kapat
                    grid.AllowUserToResizeRows = false;      // El ile yükseklik değiştirmeyi kapat
                    grid.AllowUserToAddRows = false;         // Boş yeni satır eklenmesini engelle
                    grid.ReadOnly = true;                    // Düzenlemeyi kapat
                    grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Satır seçim modu

                    // Çizgilerin ve arka planın tam oturmasını sağla
                    grid.ScrollBars = ScrollBars.None;       // Bu küçük tablolar için kaydırma çubuğunu gizle (tam sığacağı için)
                }

                // Brüt ve Net Kazanç için Sütun Dağılım Modu (Genişlikleri tam paylaştırır)
                dgwGrossProfit.Columns["Currency"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgwGrossProfit.Columns["Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgwNetProfit.Columns["Currency"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgwNetProfit.Columns["Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // Vergi Tablosu için Sütun Dağılım Modu
                dgwTax.Columns["TaxType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgwTax.Columns["Currency"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgwTax.Columns["Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


                // 3. Kullanıcı siparişlerini al
                OrderService.LoggedInUser = LoggedInUser;
                var userOrders = orderService.GetOrdersByLoggedInUser();

                if (userOrders == null || userOrders.Count == 0) return;

                // Para birimlerine göre biriktiriciler (Dictionary)
                Dictionary<string, decimal> grossProfits = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
                Dictionary<string, decimal> netProfits = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
                Dictionary<string, decimal> taxTotals = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

                string vatLabel = LanguageService.CurrentLanguage == "Türkçe" ? "KDV" : "VAT";
                string sctLabel = LanguageService.CurrentLanguage == "Türkçe" ? "ÖTV" : "SCT";

                foreach (var order in userOrders)
                {
                    // A. Brüt Kazanç
                    if (order.PayableAmount != null)
                    {
                        foreach (var kp in order.PayableAmount)
                        {
                            if (grossProfits.ContainsKey(kp.Key)) grossProfits[kp.Key] += kp.Value;
                            else grossProfits[kp.Key] = kp.Value;
                        }
                    }

                    // B. Net Kazanç
                    if (order.OrderContent != null)
                    {
                        foreach (var detail in order.OrderContent)
                        {
                            string currency = detail.Currency ?? "TRY";
                            var dbProduct = productService.GetAllProducts()
                                .FirstOrDefault(p => p.Name.Equals(detail.ProductName, StringComparison.OrdinalIgnoreCase));

                            decimal unitCost = dbProduct?.Cost ?? 0;
                            decimal detailNetProfit = (detail.OrderPrice - unitCost) * detail.Quantity;

                            if (netProfits.ContainsKey(currency)) netProfits[currency] += detailNetProfit;
                            else netProfits[currency] = detailNetProfit;
                        }
                    }

                    // C. Vergi Hesaplama
                    string vatKey = $"{vatLabel}_Türk Lirası";
                    if (taxTotals.ContainsKey(vatKey)) taxTotals[vatKey] += order.TaxAmountVAT;
                    else taxTotals[vatKey] = order.TaxAmountVAT;

                    string sctKey = $"{sctLabel}_Türk Lirası";
                    if (taxTotals.ContainsKey(sctKey)) taxTotals[sctKey] += order.TaxAmountSCT;
                    else taxTotals[sctKey] = order.TaxAmountSCT;
                }

                // 4. Hesaplanan verileri tablolara aktar
                foreach (var gp in grossProfits.OrderBy(x => x.Key))
                {
                    dgwGrossProfit.Rows.Add(gp.Key, gp.Value.ToString("N2"));

                }

                foreach (var np in netProfits.OrderBy(x => x.Key))
                {
                    dgwNetProfit.Rows.Add(np.Key, np.Value.ToString("N2"));
                }

                foreach (var tx in taxTotals.OrderBy(x => x.Key))
                {
                    string[] parts = tx.Key.Split('_');
                    string taxType = parts[0];
                    string currency = parts.Length > 1 ? parts[1] : "TRY";

                    dgwTax.Rows.Add(taxType, currency, tx.Value.ToString("N2"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Finansal veriler yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserOrderStatsDGW()
        {
            OrderService.LoggedInUser = LoggedInUser;

            var allOrders = orderService.GetAllOrders();
            var userOrders = orderService.GetOrdersByLoggedInUser();

            nmrTotalOrder.Value = userOrders.Count;

            int countOrderOne = 0;
            int countOrderTwo = 0;
            int countOrderThree = 0;
            int countOrderFour = 0;
            int countOrderFive = 0;

            foreach (var order in userOrders)
            {
                string status = order.OrderStatues?.ToString() ?? "";

                if (status == "Siparişe Başlanmadı" || status == "Not Started") countOrderOne++;
                else if (status == "Hazırlanıyor" || status == "In Preparation") countOrderTwo++;
                else if (status == "Kargoya Verilecek" || status == "To Be Shipped") countOrderThree++;
                else if (status == "Kargolandı" || status == "Shipped") countOrderFour++;
                else if (status == "Bitti" || status == "Completed") countOrderFive++;
            }

            nmrOrderOne.Value = countOrderOne;
            nmrOrderTwo.Value = countOrderTwo;
            nmrOrderThree.Value = countOrderThree;
            nmrOrderFour.Value = countOrderFour;
            nmrOrderFive.Value = countOrderFive;

            // ── ScottPlot 5.x Tam Uyumlu Dinamik Pasta Grafiği ──
            try
            {
                fpDateMoney.Plot.Clear();

                // 1. Dil Servisinden Dinamik Durum Metinlerini Alıyoruz
                string label1 = LanguageService.GetString("Siparişe Başlanmadı") ?? "Siparişe Başlanmadı";
                string label2 = LanguageService.GetString("Hazırlanıyor") ?? "Hazırlanıyor";
                string label3 = LanguageService.GetString("Kargoya Verilecek") ?? "Kargoya Verilecek";
                string label4 = LanguageService.GetString("Kargolandı") ?? "Kargolandı";
                string label5 = LanguageService.GetString("Bitti") ?? "Bitti";

                // 2. Standart Siberpunk Neon Renk Paleti (Seçili olmayanlar için hafif matlaştırılmış/transparent form)
                Color c1 = rbOrderOne.Checked ? Color.FromArgb(255, 130, 0) : Color.FromArgb(120, 230, 126, 34);
                Color c2 = rbOrderTwo.Checked ? Color.FromArgb(0, 191, 255) : Color.FromArgb(120, 52, 152, 219);
                Color c3 = rbOrderThree.Checked ? Color.FromArgb(186, 85, 211) : Color.FromArgb(120, 155, 89, 182);
                Color c4 = rbOrderFour.Checked ? Color.FromArgb(50, 205, 50) : Color.FromArgb(120, 46, 204, 113);
                Color c5 = rbOrderFive.Checked ? Color.FromArgb(255, 215, 0) : Color.FromArgb(120, 241, 196, 15);

                // 3. ScottPlot 5 uyumlu PieSlice'ları Ekleme
                List<ScottPlot.PieSlice> slices = new List<ScottPlot.PieSlice>();

                int targetSelectedIndex = -1;
                int currentSliceIndex = 0;

                if (countOrderOne > 0)
                {
                    slices.Add(new ScottPlot.PieSlice { Value = countOrderOne, FillColor = ScottPlot.Color.FromColor(c1), Label = $"{label1} ({countOrderOne})" });
                    if (rbOrderOne.Checked) targetSelectedIndex = currentSliceIndex;
                    currentSliceIndex++;
                }

                if (countOrderTwo > 0)
                {
                    slices.Add(new ScottPlot.PieSlice { Value = countOrderTwo, FillColor = ScottPlot.Color.FromColor(c2), Label = $"{label2} ({countOrderTwo})" });
                    if (rbOrderTwo.Checked) targetSelectedIndex = currentSliceIndex;
                    currentSliceIndex++;
                }

                if (countOrderThree > 0)
                {
                    slices.Add(new ScottPlot.PieSlice { Value = countOrderThree, FillColor = ScottPlot.Color.FromColor(c3), Label = $"{label3} ({countOrderThree})" });
                    if (rbOrderThree.Checked) targetSelectedIndex = currentSliceIndex;
                    currentSliceIndex++;
                }

                if (countOrderFour > 0)
                {
                    slices.Add(new ScottPlot.PieSlice { Value = countOrderFour, FillColor = ScottPlot.Color.FromColor(c4), Label = $"{label4} ({countOrderFour})" });
                    if (rbOrderFour.Checked) targetSelectedIndex = currentSliceIndex; // Düzeltilen Satır
                    currentSliceIndex++;
                }

                if (countOrderFive > 0)
                {
                    slices.Add(new ScottPlot.PieSlice { Value = countOrderFive, FillColor = ScottPlot.Color.FromColor(c5), Label = $"{label5} ({countOrderFive})" });
                    if (rbOrderFive.Checked) targetSelectedIndex = currentSliceIndex;
                    currentSliceIndex++;
                }

                if (slices.Count > 0)
                {
                    var pie = fpDateMoney.Plot.Add.Pie(slices);

                    // ScottPlot 5'in orijinal tekil fırlama property'sini atıyoruz
                    pie.ExplodeFraction = 0.04;

                    // Filtre seçildiyse ilgili dilimi daha belirgin hale getiriyoruz
                    if (targetSelectedIndex != -1 && targetSelectedIndex < pie.Slices.Count)
                    {
                        pie.Slices[targetSelectedIndex].LabelFontSize = 14;
                        pie.ExplodeFraction = 0.09; // Seçili dilim varken genel ayrışmayı biraz daha artırarak büyüme hissi verir
                    }

                    // Arka planı şeffaf yapma
                    fpDateMoney.Plot.FigureBackground.Color = ScottPlot.Color.FromColor(Color.Transparent);
                    fpDateMoney.Plot.DataBackground.Color = ScottPlot.Color.FromColor(Color.Transparent);

                    // Eksen çizgilerini kapatma
                    fpDateMoney.Plot.Axes.Frameless();
                    fpDateMoney.Plot.HideGrid();

                    // Gösterge Paneli (Legend) Özelleştirmesi
                    fpDateMoney.Plot.ShowLegend(ScottPlot.Alignment.LowerLeft);
                    fpDateMoney.Plot.Legend.FontColor = ScottPlot.Color.FromColor(Color.White);
                    fpDateMoney.Plot.Legend.FontSize = 11;
                    fpDateMoney.Plot.Legend.BackgroundColor = ScottPlot.Color.FromColor(Color.FromArgb(30, 20, 30, 40));
                }
                else
                {
                    string noDataText = LanguageService.CurrentLanguage == "Türkçe" ? "Henüz Sipariş Verisi Yok" : "No Order Data Available Yet";
                    var text = fpDateMoney.Plot.Add.Text(noDataText, 0, 0);
                    text.LabelFontSize = 13;
                    text.LabelFontColor = ScottPlot.Color.FromColor(Color.DarkGray);
                    text.LabelAlignment = ScottPlot.Alignment.MiddleCenter;

                    fpDateMoney.Plot.Axes.Frameless();
                    fpDateMoney.Plot.HideGrid();
                }

                fpDateMoney.Plot.Axes.AutoScale();
                fpDateMoney.Refresh();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Sipariş durum grafiği çizilirken hata oluştu: " + ex.Message);
            }
        }

        private void PopulateFinancialGridsDGW()
        {
            try
            {
                dgwGrossProfit.Rows.Clear();
                dgwGrossProfit.Columns.Clear();
                dgwNetProfit.Rows.Clear();
                dgwNetProfit.Columns.Clear();
                dgwTax.Rows.Clear();
                dgwTax.Columns.Clear();

                string currencyHeader = LanguageService.CurrentLanguage == "Türkçe" ? "Para Birimi" : "Currency";
                string amountHeader = LanguageService.CurrentLanguage == "Türkçe" ? "Tutar" : "Amount";
                string taxTypeHeader = LanguageService.CurrentLanguage == "Türkçe" ? "Vergi Türü" : "Tax Type";

                dgwGrossProfit.Columns.Add("Currency", currencyHeader);
                dgwGrossProfit.Columns.Add("Amount", amountHeader);
                dgwNetProfit.Columns.Add("Currency", currencyHeader);
                dgwNetProfit.Columns.Add("Amount", amountHeader);
                dgwTax.Columns.Add("TaxType", taxTypeHeader);
                dgwTax.Columns.Add("Currency", currencyHeader);
                dgwTax.Columns.Add("Amount", amountHeader);

                foreach (var grid in new[] { dgwGrossProfit, dgwNetProfit, dgwTax })
                {
                    grid.RowHeadersVisible = false;
                    grid.AllowUserToResizeColumns = false;
                    grid.AllowUserToResizeRows = false;
                    grid.AllowUserToAddRows = false;
                    grid.ReadOnly = true;
                    grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    grid.ScrollBars = ScrollBars.None;
                }

                dgwGrossProfit.Columns["Currency"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgwGrossProfit.Columns["Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgwNetProfit.Columns["Currency"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgwNetProfit.Columns["Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgwTax.Columns["TaxType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgwTax.Columns["Currency"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgwTax.Columns["Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                OrderService.LoggedInUser = LoggedInUser;
                var userOrders = orderService.GetOrdersByLoggedInUser();

                if (userOrders == null || userOrders.Count == 0) return;

                Dictionary<string, decimal> grossProfits = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
                Dictionary<string, decimal> netProfits = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
                Dictionary<string, decimal> taxTotals = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

                string vatLabel = LanguageService.CurrentLanguage == "Türkçe" ? "KDV" : "VAT";
                string sctLabel = LanguageService.CurrentLanguage == "Türkçe" ? "ÖTV" : "SCT";

                foreach (var order in userOrders)
                {
                    if (order.PayableAmount != null)
                    {
                        foreach (var kp in order.PayableAmount)
                        {
                            if (grossProfits.ContainsKey(kp.Key)) grossProfits[kp.Key] += kp.Value;
                            else grossProfits[kp.Key] = kp.Value;
                        }
                    }

                    if (order.OrderContent != null)
                    {
                        foreach (var detail in order.OrderContent)
                        {
                            string currency = detail.Currency ?? "TRY";
                            var dbProduct = productService.GetAllProducts()
                                .FirstOrDefault(p => p.Name.Equals(detail.ProductName, StringComparison.OrdinalIgnoreCase));

                            decimal unitCost = dbProduct?.Cost ?? 0;
                            decimal detailNetProfit = (detail.OrderPrice - unitCost) * detail.Quantity;

                            if (netProfits.ContainsKey(currency)) netProfits[currency] += detailNetProfit;
                            else netProfits[currency] = detailNetProfit;
                        }
                    }

                    string vatKey = $"{vatLabel}_Türk Lirası";
                    if (taxTotals.ContainsKey(vatKey)) taxTotals[vatKey] += order.TaxAmountVAT;
                    else taxTotals[vatKey] = order.TaxAmountVAT;

                    string sctKey = $"{sctLabel}_Türk Lirası";
                    if (taxTotals.ContainsKey(sctKey)) taxTotals[sctKey] += order.TaxAmountSCT;
                    else taxTotals[sctKey] = order.TaxAmountSCT;
                }

                foreach (var gp in grossProfits.OrderBy(x => x.Key)) dgwGrossProfit.Rows.Add(gp.Key, gp.Value.ToString("N2"));
                foreach (var np in netProfits.OrderBy(x => x.Key)) dgwNetProfit.Rows.Add(np.Key, np.Value.ToString("N2"));

                foreach (var tx in taxTotals.OrderBy(x => x.Key))
                {
                    string[] parts = tx.Key.Split('_');
                    string taxType = parts[0];
                    string currency = parts.Length > 1 ? parts[1] : "TRY";
                    dgwTax.Rows.Add(taxType, currency, tx.Value.ToString("N2"));
                }

                // ── ScottPlot 5.x Uyumlu Finansal Sütun Grafiği Çizimi ──
                try
                {
                    fpMoney.Plot.Clear();

                    var orderedGross = grossProfits.OrderBy(x => x.Key).ToList();

                    List<double> grossValues = new List<double>();
                    List<double> netValues = new List<double>();
                    List<ScottPlot.Tick> ticks = new List<ScottPlot.Tick>();

                    for (int i = 0; i < orderedGross.Count; i++)
                    {
                        string currencyKey = orderedGross[i].Key;
                        grossValues.Add((double)orderedGross[i].Value);

                        double netVal = netProfits.ContainsKey(currencyKey) ? (double)netProfits[currencyKey] : 0;
                        netValues.Add(netVal);

                        ticks.Add(new ScottPlot.Tick(i, currencyKey));
                    }

                    if (grossValues.Count > 0)
                    {
                        var barGroup1 = fpMoney.Plot.Add.Bars(grossValues.ToArray());
                        barGroup1.Label = LanguageService.GetString("Brüt Kazanç") ?? "Brüt Kazanç";
                        barGroup1.Color = ScottPlot.Color.FromColor(Color.FromArgb(52, 152, 219));

                        var barGroup2 = fpMoney.Plot.Add.Bars(netValues.ToArray());
                        barGroup2.Label = LanguageService.GetString("Net Kazanç") ?? "Net Kazanç";
                        barGroup2.Color = ScottPlot.Color.FromColor(Color.FromArgb(46, 204, 113));

                        // ScottPlot 5 Eksen ve Manuel Etiket Yönetimi
                        fpMoney.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks.ToArray());

                        fpMoney.Plot.FigureBackground.Color = ScottPlot.Color.FromColor(Color.Transparent);
                        fpMoney.Plot.DataBackground.Color = ScottPlot.Color.FromColor(Color.Transparent);

                        fpMoney.Plot.ShowLegend(ScottPlot.Alignment.UpperRight);
                    }
                    else
                    {
                        var text = fpMoney.Plot.Add.Text("Finansal Veri Bulunamadı", 0, 0);
                        text.LabelFontSize = 14;
                        text.LabelAlignment = ScottPlot.Alignment.MiddleCenter;
                    }

                    fpMoney.Plot.Axes.AutoScale();
                    fpMoney.Refresh();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Grafik 2 çizilirken hata oluştu: " + ex.Message);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Finansal veriler yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitCalendar()
        {
            _calendar = new CustomCalendar
            {
                // ── Konum: mcDate'in Location ve Size değerlerini buraya kopyalayın ──
                Location = mcDate.Location,   // mcDate'i silmeyecekseniz: new Point(X, Y)
                Size = new System.Drawing.Size(300, 280), // istediğiniz boyut
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left,

                // Bugünü başlangıç seçili tarihi olarak ayarla
                SelectedDate = DateTime.Today,
            };

            // Tarih seçildiğinde tetiklenecek olay
            _calendar.DateSelected += Calendar_DateSelected;

            // Formu ilk açtığında bugünün siparişleri gösterilsin
            RefreshCalendarOrderDates();

            // Eğer mcDate hâlâ Designer'da duruyorsa önce onu gizleyin
            if (mcDate != null) mcDate.Visible = false;

            // Takvimi forma ekle (mcDate'in parent'ı neyse onu kullanın)
            this.Controls.Add(_calendar);
            _calendar.BringToFront();
        }

        private void RefreshCalendarOrderDates()
        {
            OrderService.LoggedInUser = LoggedInUser;
            var userOrders = orderService.GetOrdersByLoggedInUser();

            var orderDates = new System.Collections.Generic.HashSet<DateTime>();
            if (userOrders != null)
            {
                foreach (var order in userOrders)
                    orderDates.Add(order.OrderDate.Date);
            }

            _calendar.OrderDates = orderDates;
        }

        private void Calendar_DateSelected(object sender, DateTime selectedDate)
        {
            // 1. Tüm RadioButton'ların seçimini kaldır
            ClearRadioButtonSelections();

            // 2. Aktif tarih filtresini kaydet
            _activeFilterDate = selectedDate.Date;

            // 3. Grid'i yalnızca bu tarihe göre doldur
            PopulateOrderGrid();
        }

        private void ClearRadioButtonSelections()
        {
            // CheckedChanged/Click olaylarının tetiklenmemesi için geçici olarak olay bağlantısını kesiyor, temizliyor, sonra yeniden bağlıyoruz.
            rbOrderOne.CheckedChanged -= rbOrderStatus_CheckedChanged;
            rbOrderTwo.CheckedChanged -= rbOrderStatus_CheckedChanged;
            rbOrderThree.CheckedChanged -= rbOrderStatus_CheckedChanged;
            rbOrderFour.CheckedChanged -= rbOrderStatus_CheckedChanged;
            rbOrderFive.CheckedChanged -= rbOrderStatus_CheckedChanged;

            rbOrderOne.Checked = false;
            rbOrderTwo.Checked = false;
            rbOrderThree.Checked = false;
            rbOrderFour.Checked = false;
            rbOrderFive.Checked = false;

            // Tag'leri de sıfırla (rbOrder_Click toggle mantığıyla uyumlu)
            rbOrderOne.Tag = false;
            rbOrderTwo.Tag = false;
            rbOrderThree.Tag = false;
            rbOrderFour.Tag = false;
            rbOrderFive.Tag = false;

            rbOrderOne.CheckedChanged += rbOrderStatus_CheckedChanged;
            rbOrderTwo.CheckedChanged += rbOrderStatus_CheckedChanged;
            rbOrderThree.CheckedChanged += rbOrderStatus_CheckedChanged;
            rbOrderFour.CheckedChanged += rbOrderStatus_CheckedChanged;
            rbOrderFive.CheckedChanged += rbOrderStatus_CheckedChanged;
        }

        private void btnOrderHistory_Click(object sender, EventArgs e)
        {
            string selectedProduct = cmbProducts.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedProduct))
            {
                MessageBox.Show("Lütfen önce bir ürün seçiniz.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ListView yapılandır
            lwProductDetails.Clear();
            lwProductDetails.View = View.Details;
            lwProductDetails.FullRowSelect = true;
            lwProductDetails.GridLines = true;

            lwProductDetails.Columns.Add("Müşteri Adı", 130);
            lwProductDetails.Columns.Add("Sipariş Tarihi", 130);
            lwProductDetails.Columns.Add("Adet", 60);
            lwProductDetails.Columns.Add("Birim Fiyat", 90);
            lwProductDetails.Columns.Add("Para Birimi", 80);
            lwProductDetails.Columns.Add("Tutar", 90);
            lwProductDetails.Columns.Add("Sipariş Durumu", 120);

            OrderService.LoggedInUser = LoggedInUser;
            var userOrders = orderService.GetOrdersByLoggedInUser();

            bool found = false;

            foreach (var order in userOrders)
            {
                if (order.OrderContent == null) continue;

                foreach (var detail in order.OrderContent)
                {
                    if (detail.ProductName == null) continue;
                    if (!detail.ProductName.Equals(selectedProduct, StringComparison.OrdinalIgnoreCase)) continue;

                    found = true;

                    decimal total = detail.OrderPrice * detail.Quantity;

                    var item = new ListViewItem(order.CustomerName ?? "-");
                    item.SubItems.Add(order.OrderDate.ToString("yyyy-MM-dd HH:mm"));
                    item.SubItems.Add(detail.Quantity.ToString());
                    item.SubItems.Add(detail.OrderPrice.ToString("N2"));
                    item.SubItems.Add(detail.Currency ?? "-");
                    item.SubItems.Add(total.ToString("N2"));
                    item.SubItems.Add(order.OrderStatues ?? "-");

                    lwProductDetails.Items.Add(item);
                }
            }

            if (!found)
                MessageBox.Show($"'{selectedProduct}' ürününe ait sipariş geçmişi bulunamadı.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPriceAnalysis_Click(object sender, EventArgs e)
        {
            string selectedProduct = cmbProducts.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedProduct))
            {
                MessageBox.Show("Lütfen önce bir ürün seçiniz.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ürün bilgisini al
            ProductService.LoggedInUser = LoggedInUser;
            var allProducts = productService.GetAllProducts();
            var product = allProducts.FirstOrDefault(p =>
                p.Name.Equals(selectedProduct, StringComparison.OrdinalIgnoreCase) &&
                p.AddedBy.Equals(LoggedInUser?.Name ?? "", StringComparison.OrdinalIgnoreCase));

            if (product == null)
            {
                MessageBox.Show("Ürün bilgisi bulunamadı.", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Sipariş verilerinden hesapla
            OrderService.LoggedInUser = LoggedInUser;
            var userOrders = orderService.GetOrdersByLoggedInUser();

            // Para birimine göre grupla: currency → (toplam tutar, toplam adet)
            var salesByCurrency = new Dictionary<string, (decimal totalRevenue, int totalQty)>(StringComparer.OrdinalIgnoreCase);

            foreach (var order in userOrders)
            {
                if (order.OrderContent == null) continue;

                foreach (var detail in order.OrderContent)
                {
                    if (detail.ProductName == null) continue;
                    if (!detail.ProductName.Equals(selectedProduct, StringComparison.OrdinalIgnoreCase)) continue;

                    string currency = detail.Currency ?? "?";
                    decimal lineTotal = detail.OrderPrice * detail.Quantity;

                    if (salesByCurrency.ContainsKey(currency))
                    {
                        var existing = salesByCurrency[currency];
                        salesByCurrency[currency] = (existing.totalRevenue + lineTotal, existing.totalQty + detail.Quantity);
                    }
                    else
                    {
                        salesByCurrency[currency] = (lineTotal, detail.Quantity);
                    }
                }
            }

            // ListView yapılandır
            lwProductDetails.Clear();
            lwProductDetails.View = View.Details;
            lwProductDetails.FullRowSelect = true;
            lwProductDetails.GridLines = true;

            lwProductDetails.Columns.Add("Metrik", 180);
            lwProductDetails.Columns.Add("Değer", 200);

            // Sabit satırlar
            void AddRow(string metric, string value)
            {
                var item = new ListViewItem(metric);
                item.SubItems.Add(value);
                lwProductDetails.Items.Add(item);
            }

            AddRow("Ürün Adı", product.Name);
            AddRow("─────────────────", "──────────────────");
            AddRow("Maliyet", $"{product.Cost:N2} {product.CostCurrency}");
            AddRow("Satış Fiyatı (Kayıtlı)", $"{product.Price:N2} {product.PriceCurrency}");
            AddRow("─────────────────", "──────────────────");

            if (salesByCurrency.Count == 0)
            {
                AddRow("Sipariş Verisi", "Henüz satış yok");
            }
            else
            {
                foreach (var kvp in salesByCurrency)
                {
                    string currency = kvp.Key;
                    decimal revenue = kvp.Value.totalRevenue;
                    int qty = kvp.Value.totalQty;
                    decimal avgPrice = qty > 0 ? revenue / qty : 0;
                    decimal grossProfit = avgPrice - product.Cost; // Maliyet ile karşılaştır (aynı para birimi varsayımıyla)

                    AddRow($"Para Birimi", currency);
                    AddRow($"Toplam Satış Adedi", qty.ToString());
                    AddRow($"Toplam Ciro", $"{revenue:N2} {currency}");
                    AddRow($"Ort. Satış Fiyatı", $"{avgPrice:N2} {currency}");
                    AddRow($"Brüt Kar / Adet*", $"{grossProfit:N2} {currency}");
                    AddRow($"Toplam Brüt Kar*", $"{grossProfit * qty:N2} {currency}");
                    AddRow("─────────────────", "──────────────────");
                }

                AddRow("* Maliyet ile hesaplanmıştır", "");
            }
        }
    }
}