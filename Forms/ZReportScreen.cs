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
using neoStockMasterv2.Data.Services.BankServices;

namespace neoStockMasterv2.Forms
{
    public partial class ZReportScreen : Form
    {
        public static User LoggedInUser { get; set; }
        OrderService orderService = new OrderService();
        ProductService productService = new ProductService();
        private CustomCalendar _calendar;
        private DateTime? _activeFilterDate = null; // null = tarih filtresi yok

        // Net kazanç için döviz kurları (ekran açıkken bir kez çekilir)
        private Dictionary<string, (decimal BuyRate, decimal SellRate)>? _rates;
        private bool _ratesTried = false;

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

            this.FormClosed += (s, e) => LanguageService.LanguageChanged -= LanguageService_LanguageChanged;
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
                ? "Brüt Kazanç Tablosu: Satışlardan elde edilen toplam geliri gösterir. \n Ürün maliyeti düşülmeden önceki ham kazancı para birimine göre listeler."
                : "Gross Profit Table: Shows total revenue from sales.\n Lists raw earnings before product costs are deducted, grouped by currency.");

            AttachTooltip(dgwNetProfit, ttProfit, isTurkish
                ? "Net Kazanç Tablosu: Vergiler (ÖTV/KDV) çıkarılmış satış tutarından ürün maliyeti düşülerek hesaplanır.\nFarklı para birimindeki maliyetler güncel kurla çevrilir. '*' satırı hesaplanamayan kalemleri gösterir."
                : "Net Profit Table: Sales amount excluding taxes (SCT/VAT) minus product cost.\nCosts in other currencies are converted at the current rate. The '*' row shows items that could not be calculated.");

            AttachTooltip(dgwTax, ttTax, isTurkish
                ? "Vergi Tablosu: Siparişlere uygulanan KDV/ÖTV tutarlarını listeler.\n Toplam vergi yükünü para birimine göre gösterir."
                : "Tax Table: Lists VAT/SCT amounts applied to orders.\n Shows the total tax burden grouped by currency.");
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

        private void LoadLoggedInUserProducts()
        {
            cmbProducts.Items.Clear();

            foreach (var product in productService.GetProductsByLoggedInUser())
            {
                if (!string.IsNullOrEmpty(product.Name))
                    cmbProducts.Items.Add(product.Name);
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
                MessageBox.Show(LanguageService.T("Ürün istatistikleri hesaplanırken bir hata oluştu: ", "An error occurred while calculating product statistics: ") + ex.Message,
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(LanguageService.T("Sipariş tablosu yüklenirken hata oluştu: ", "An error occurred while loading the order table: ") + ex.Message,
                    LanguageService.T("Hata", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void LoadUserOrderStatsDGW()
        {
            OrderService.LoggedInUser = LoggedInUser;

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
            bool tr = !LanguageService.IsEnglish;

            try
            {
                dgwGrossProfit.Rows.Clear();
                dgwGrossProfit.Columns.Clear();
                dgwNetProfit.Rows.Clear();
                dgwNetProfit.Columns.Clear();
                dgwTax.Rows.Clear();
                dgwTax.Columns.Clear();

                string currencyHeader = tr ? "Para Birimi" : "Currency";
                string amountHeader = tr ? "Tutar" : "Amount";
                string taxTypeHeader = tr ? "Vergi Türü" : "Tax Type";

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

                var grossProfits = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
                var netProfits = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
                decimal totalVatTL = 0m, totalSctTL = 0m;
                int skippedLines = 0;

                var myProducts = productService.GetProductsByLoggedInUser();

                foreach (var order in userOrders)
                {
                    // Brüt: müşteriden alınan toplam (değişmedi)
                    if (order.PayableAmount != null)
                        foreach (var kp in order.PayableAmount)
                            grossProfits[kp.Key] = grossProfits.GetValueOrDefault(kp.Key) + kp.Value;

                    // Net: vergisiz satış − maliyet (maliyet satış para birimine çevrilir)
                    if (order.OrderContent != null)
                    {
                        decimal taxFactor = (1 + order.TaxPercentageSCT / 100m) * (1 + order.TaxPercentageVAT / 100m);

                        foreach (var detail in order.OrderContent)
                        {
                            string currency = string.IsNullOrWhiteSpace(detail.Currency) ? "Türk Lirası" : detail.Currency;

                            var dbProduct = myProducts.FirstOrDefault(p =>
                                p.Name.Equals(detail.ProductName, StringComparison.OrdinalIgnoreCase));
                            if (dbProduct == null) { skippedLines++; continue; }   // ürün silinmiş

                            decimal lineTotal = detail.Total != 0 ? detail.Total : detail.OrderPrice * detail.Quantity;
                            decimal lineWithoutTax = detail.IsDiscounted && taxFactor > 0 ? lineTotal / taxFactor : lineTotal;

                            decimal? unitCost = ConvertCurrency(dbProduct.Cost, dbProduct.CostCurrency, currency);
                            if (unitCost == null) { skippedLines++; continue; }   // kur bulunamadı

                            netProfits[currency] = netProfits.GetValueOrDefault(currency)
                                                   + lineWithoutTax - unitCost.Value * detail.Quantity;
                        }
                    }

                    totalVatTL += order.TaxAmountVAT;
                    totalSctTL += order.TaxAmountSCT;
                }

                foreach (var gp in grossProfits.OrderBy(x => x.Key))
                    dgwGrossProfit.Rows.Add(gp.Key, gp.Value.ToString("N2"));

                foreach (var np in netProfits.OrderBy(x => x.Key))
                    dgwNetProfit.Rows.Add(np.Key, np.Value.ToString("N2"));

                if (skippedLines > 0)
                    dgwNetProfit.Rows.Add("*", tr ? $"{skippedLines} kalem hesaplanamadı" : $"{skippedLines} item(s) not calculated");

                string tlName = tr ? "Türk Lirası" : "Turkish Lira";
                dgwTax.Rows.Add(tr ? "KDV" : "VAT", tlName, totalVatTL.ToString("N2"));
                dgwTax.Rows.Add(tr ? "ÖTV" : "SCT", tlName, totalSctTL.ToString("N2"));

                // ── ScottPlot: brüt / net sütun grafiği ──
                try
                {
                    fpMoney.Plot.Clear();

                    var orderedGross = grossProfits.OrderBy(x => x.Key).ToList();
                    var grossValues = new List<double>();
                    var netValues = new List<double>();
                    var ticks = new List<ScottPlot.Tick>();

                    for (int i = 0; i < orderedGross.Count; i++)
                    {
                        string key = orderedGross[i].Key;
                        grossValues.Add((double)orderedGross[i].Value);
                        netValues.Add(netProfits.TryGetValue(key, out decimal nv) ? (double)nv : 0);
                        ticks.Add(new ScottPlot.Tick(i, key));
                    }

                    if (grossValues.Count > 0)
                    {
                        var barGroup1 = fpMoney.Plot.Add.Bars(grossValues.ToArray());
                        barGroup1.Label = LanguageService.GetString("Brüt Kazanç") ?? (tr ? "Brüt Kazanç" : "Gross Profit");
                        barGroup1.Color = ScottPlot.Color.FromColor(Color.FromArgb(52, 152, 219));

                        var barGroup2 = fpMoney.Plot.Add.Bars(netValues.ToArray());
                        barGroup2.Label = LanguageService.GetString("Net Kazanç") ?? (tr ? "Net Kazanç" : "Net Profit");
                        barGroup2.Color = ScottPlot.Color.FromColor(Color.FromArgb(46, 204, 113));

                        fpMoney.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks.ToArray());
                        fpMoney.Plot.FigureBackground.Color = ScottPlot.Color.FromColor(Color.Transparent);
                        fpMoney.Plot.DataBackground.Color = ScottPlot.Color.FromColor(Color.Transparent);
                        fpMoney.Plot.ShowLegend(ScottPlot.Alignment.UpperRight);
                    }
                    else
                    {
                        var text = fpMoney.Plot.Add.Text(tr ? "Finansal Veri Bulunamadı" : "No Financial Data Found", 0, 0);
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
                MessageBox.Show((tr ? "Finansal veriler yüklenirken hata oluştu: " : "An error occurred while loading financial data: ") + ex.Message,
                    tr ? "Hata" : "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool IsTL(string? currency) =>
         string.IsNullOrWhiteSpace(currency) ||
        currency.Equals("Türk Lirası", StringComparison.OrdinalIgnoreCase) ||
        currency.Equals("TRY", StringComparison.OrdinalIgnoreCase) ||
        currency.Equals("TL", StringComparison.OrdinalIgnoreCase);

        // Tutarı bir para biriminden diğerine TL üzerinden çevirir. Kur yoksa null döner.
        private decimal? ConvertCurrency(decimal amount, string? from, string? to)
        {
            if (IsTL(from) && IsTL(to)) return amount;
            if (string.Equals(from, to, StringComparison.OrdinalIgnoreCase)) return amount;

            if (!_ratesTried)
            {
                _ratesTried = true;
                try
                {
                    // Task.Run: arayüz iş parçacığını kilitlenmeye sokmadan bekler
                    _rates = Task.Run(() => new FREEMARKETforex().GetExchangeRatesAsync()).GetAwaiter().GetResult();
                }
                catch
                {
                    _rates = null;   // internet yoksa: farklı para birimli satırlar "hesaplanamadı" sayılır
                }
            }

            decimal? RateToTL(string? currency)
            {
                if (IsTL(currency)) return 1m;
                if (_rates != null && currency != null && _rates.TryGetValue(currency, out var r) && r.SellRate > 0)
                    return r.SellRate;
                return null;
            }

            decimal? fromRate = RateToTL(from);
            decimal? toRate = RateToTL(to);
            if (fromRate == null || toRate == null) return null;

            return amount * fromRate.Value / toRate.Value;
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
            bool tr = !LanguageService.IsEnglish;
            string selectedProduct = cmbProducts.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedProduct))
            {
                MessageBox.Show(tr ? "Lütfen önce bir ürün seçiniz." : "Please select a product first.",
                    tr ? "Uyarı" : "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lwProductDetails.Clear();
            lwProductDetails.View = View.Details;
            lwProductDetails.FullRowSelect = true;
            lwProductDetails.GridLines = true;

            lwProductDetails.Columns.Add(tr ? "Müşteri Adı" : "Customer", 130);
            lwProductDetails.Columns.Add(tr ? "Sipariş Tarihi" : "Order Date", 130);
            lwProductDetails.Columns.Add(tr ? "Adet" : "Qty", 60);
            lwProductDetails.Columns.Add(tr ? "Birim Fiyat" : "Unit Price", 90);
            lwProductDetails.Columns.Add(tr ? "Para Birimi" : "Currency", 80);
            lwProductDetails.Columns.Add(tr ? "Tutar" : "Amount", 90);
            lwProductDetails.Columns.Add(tr ? "Sipariş Durumu" : "Status", 120);

            OrderService.LoggedInUser = LoggedInUser;
            bool found = false;

            foreach (var order in orderService.GetOrdersByLoggedInUser())
            {
                if (order.OrderContent == null) continue;

                foreach (var detail in order.OrderContent)
                {
                    if (detail.ProductName == null ||
                        !detail.ProductName.Equals(selectedProduct, StringComparison.OrdinalIgnoreCase)) continue;

                    found = true;
                    var item = new ListViewItem(order.CustomerName ?? "-");
                    item.SubItems.Add(order.OrderDate.ToString(tr ? "dd.MM.yyyy HH:mm" : "MM/dd/yyyy HH:mm"));
                    item.SubItems.Add(detail.Quantity.ToString());
                    item.SubItems.Add(detail.OrderPrice.ToString("N2"));
                    item.SubItems.Add(detail.Currency ?? "-");
                    item.SubItems.Add((detail.OrderPrice * detail.Quantity).ToString("N2"));
                    item.SubItems.Add(order.OrderStatues ?? "-");
                    lwProductDetails.Items.Add(item);
                }
            }

            if (!found)
                MessageBox.Show(tr ? $"'{selectedProduct}' ürününe ait sipariş geçmişi bulunamadı." : $"No order history found for '{selectedProduct}'.",
                    tr ? "Bilgi" : "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPriceAnalysis_Click(object sender, EventArgs e)
        {
            bool tr = !LanguageService.IsEnglish;
            string selectedProduct = cmbProducts.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedProduct))
            {
                MessageBox.Show(tr ? "Lütfen önce bir ürün seçiniz." : "Please select a product first.",
                    tr ? "Uyarı" : "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var product = productService.GetProductsByLoggedInUser()
                .FirstOrDefault(p => p.Name.Equals(selectedProduct, StringComparison.OrdinalIgnoreCase));

            if (product == null)
            {
                MessageBox.Show(tr ? "Ürün bilgisi bulunamadı." : "Product information not found.",
                    tr ? "Hata" : "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Para birimine göre: (vergisiz ciro, adet)
            var salesByCurrency = new Dictionary<string, (decimal revenue, int qty)>(StringComparer.OrdinalIgnoreCase);

            OrderService.LoggedInUser = LoggedInUser;
            foreach (var order in orderService.GetOrdersByLoggedInUser())
            {
                if (order.OrderContent == null) continue;
                decimal taxFactor = (1 + order.TaxPercentageSCT / 100m) * (1 + order.TaxPercentageVAT / 100m);

                foreach (var detail in order.OrderContent)
                {
                    if (detail.ProductName == null ||
                        !detail.ProductName.Equals(selectedProduct, StringComparison.OrdinalIgnoreCase)) continue;

                    string currency = detail.Currency ?? "?";
                    decimal lineTotal = detail.Total != 0 ? detail.Total : detail.OrderPrice * detail.Quantity;
                    decimal lineWithoutTax = detail.IsDiscounted && taxFactor > 0 ? lineTotal / taxFactor : lineTotal;

                    var (rev, qty) = salesByCurrency.GetValueOrDefault(currency);
                    salesByCurrency[currency] = (rev + lineWithoutTax, qty + detail.Quantity);
                }
            }

            lwProductDetails.Clear();
            lwProductDetails.View = View.Details;
            lwProductDetails.FullRowSelect = true;
            lwProductDetails.GridLines = true;
            lwProductDetails.Columns.Add(tr ? "Metrik" : "Metric", 180);
            lwProductDetails.Columns.Add(tr ? "Değer" : "Value", 200);

            void AddRow(string metric, string value)
            {
                var item = new ListViewItem(metric);
                item.SubItems.Add(value);
                lwProductDetails.Items.Add(item);
            }
            const string line1 = "─────────────────", line2 = "──────────────────";

            AddRow(tr ? "Ürün Adı" : "Product", product.Name);
            AddRow(line1, line2);
            AddRow(tr ? "Maliyet" : "Cost", $"{product.Cost:N2} {product.CostCurrency}");
            AddRow(tr ? "Satış Fiyatı (Kayıtlı)" : "Listed Price", $"{product.Price:N2} {product.PriceCurrency}");
            AddRow(line1, line2);

            if (salesByCurrency.Count == 0)
            {
                AddRow(tr ? "Sipariş Verisi" : "Order Data", tr ? "Henüz satış yok" : "No sales yet");
                return;
            }

            foreach (var kvp in salesByCurrency)
            {
                string currency = kvp.Key;
                decimal revenue = kvp.Value.revenue;
                int qty = kvp.Value.qty;
                decimal avgPrice = qty > 0 ? revenue / qty : 0;
                decimal? unitCost = ConvertCurrency(product.Cost, product.CostCurrency, currency);

                AddRow(tr ? "Para Birimi" : "Currency", currency);
                AddRow(tr ? "Toplam Satış Adedi" : "Units Sold", qty.ToString());
                AddRow(tr ? "Toplam Ciro (vergisiz)" : "Revenue (excl. tax)", $"{revenue:N2} {currency}");
                AddRow(tr ? "Ort. Satış Fiyatı" : "Avg. Price", $"{avgPrice:N2} {currency}");

                if (unitCost != null)
                {
                    decimal profitPerUnit = avgPrice - unitCost.Value;
                    AddRow(tr ? "Kâr / Adet" : "Profit / Unit", $"{profitPerUnit:N2} {currency}");
                    AddRow(tr ? "Toplam Kâr" : "Total Profit", $"{profitPerUnit * qty:N2} {currency}");
                }
                else
                {
                    AddRow(tr ? "Kâr" : "Profit", tr ? "Kur bulunamadı, hesaplanamadı" : "No exchange rate, not calculated");
                }
                AddRow(line1, line2);
            }
        }
    }
}